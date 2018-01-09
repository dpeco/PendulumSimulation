using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumElastic : MonoBehaviour
{
    public Transform pos1; //top de la cuerda
    public Transform pos2; //bola

    //Line renderer used to display the rope
    LineRenderer lineRenderer;

    //A list with all rope section
    public List<CilinderBody> ropeSegments = new List<CilinderBody>();

    public int stringPartitions = 10; //cantidad de particiones de la cuerda
    public float stringPartLength = 0.2f;  //distancia inicial de cada segmento de la cuerda

    public float gravity = -9.81f;
    //Iterations establece el numero de veces que se repite la Actualización De Posiciones en una sola iteracion
    //cuantas mas, menos probabilidad de error / la cuerda pete (mecánica flashbacks)
    public int iterations = 50;

    public float kElasticity = 40f; //constante elasticidad para muelle
    public float kDamping = 2f; //constante damping para muelle

    public float massRopeSegment = 0.2f; //masa para segmento de la cuerda
    public float massLastRopeSegment = 1; //masa de la bola del final del pendulo
    bool move = false; //permite controlar si se calcula o no
    public float minStretch = 0.1f; //minimo de lo que se puede contraer un segmento
    public float maxStretch = 0.3f; //maximo de lo que se puede estirar un segmento
    
    void Start()
    {
        //inicializar el line renderer
        lineRenderer = GetComponent<LineRenderer>();
        //recibir las posiciones de los 2 extremos de la cuerda
        Vector3Class initPos = new Vector3Class(pos1.transform.position);
        Vector3Class endPos = new Vector3Class(pos2.transform.position);

        //generador de los puntos de la cuerda, en resumen hace un vector entre pos1,pos2
        Vector3Class stringVector = endPos - initPos;
        stringVector = stringVector.Normalize(stringVector);

        List<Vector3Class> ropePositions = new List<Vector3Class>();
        //determina el punto inicial de todos los segmentos con
        //"largo segmento * posicion en lista * direccion normalizadapos1pos2 + pos1" 
        for (int i = 0; i < stringPartitions; i++)
        {
            Vector3Class partPos = initPos;
            partPos += stringVector * i * stringPartLength;
            ropePositions.Add(partPos);
        }
        //se añaden todos los segmentos con la posicion deseada
        //se introducen los elementos al reves para facilitar algunos calculos
        for (int i = ropePositions.Count - 1; i >= 0; i--)
        {
            ropeSegments.Add(new CilinderBody(ropePositions[i]));
        }
        //actualiza posicion del objeto bola
        pos2.position = ropeSegments[0].pos.GetValues();
    }

    void FixedUpdate()
    {
        if (move)
        {
            if (ropeSegments.Count > 0)
            {
                //Actualizacion de posiciones

                for (int i = 0; i < iterations; i++)
                {
                    //dividimos deltatime por numero iteraciones
                    UpdateRopeSimulation(Time.deltaTime / iterations);
                }
            }

            //actualiza posicion del objeto bola
            pos2.position = ropeSegments[0].pos.GetValues();
        }
        //dibuja la cuerda con valores actualizados
        DisplayRope();
    }

    private void DisplayRope()
    {
        float ropeWidth = 0.05f;

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        //An array with all rope section positions
        Vector3[] positions = new Vector3[ropeSegments.Count];

        for (int i = 0; i < ropeSegments.Count; i++)
        {
            positions[i] = ropeSegments[i].pos.GetValues();
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    private void UpdateRopeSimulation(float deltaTime)
    {
        //actualizar pos1 en la lista por si se ha movido
        CilinderBody lastRopeSection = ropeSegments[ropeSegments.Count - 1];
        lastRopeSection.pos = new Vector3Class(pos1.position);
        ropeSegments[ropeSegments.Count - 1] = lastRopeSection;

        //calcular acceleraciones
        CalculateAccelerations();

        //actualizar la posicion de todos los segmentos excepto el ultimo (top)
        for (int i = 0; i < ropeSegments.Count - 1; i++)
        {
            CilinderBody thisRopeSection = new CilinderBody();

            //Forward Euler
            thisRopeSection.speed = ropeSegments[i].speed + ropeSegments[i].acc * deltaTime;
            thisRopeSection.pos = ropeSegments[i].pos + ropeSegments[i].speed * deltaTime;

            ropeSegments[i] = thisRopeSection;
        }

        //regular estiramiento de los segmentos, tambien se recomienda ejecutar mas de una vez (para corregir el maximo posible)
        //ya que cuando se cambia la posicion de uno, quiza hace falta regular el resto, y quizá sólo serian accesibles repitiendo el proceso
        int maximumStretchIterations = 2;

        for (int i = 0; i < maximumStretchIterations; i++)
        {
            ImplementMaximumStretch();
        }

    }

    private void CalculateAccelerations()
    {
        //calculamos la fuerza de cada muelle y las guardamos en una lista para luego adjudicarla a los segmentos
        List<Vector3Class> springForces = new List<Vector3Class>();

        for (int i = 0; i < ropeSegments.Count - 1; i++)
        {
            Vector3Class p1p2 = ropeSegments[i + 1].pos - ropeSegments[i].pos;

            //elasticity part
            float part1 = kElasticity * (p1p2.Size() - stringPartLength);

            //Damping part
            float part2 = kDamping * ((p1p2.DotProduct(ropeSegments[i + 1].speed - ropeSegments[i].speed, p1p2.Normalize(p1p2))));

            //calculamos total del muelle juntando las 2 partes
            Vector3Class resultForce = p1p2.Normalize(p1p2);
            resultForce *= (part1 + part2) * -1;

            //al estar el orden de pos2 a pos1, este force es el negativo (f2)
            resultForce *= -1;

            springForces.Add(resultForce);
        }

        for (int i = 0; i < ropeSegments.Count - 1; i++)
        {
            Vector3Class springForce = new Vector3Class();
            //spring que corresponde a F1 de la formula
            if (i != 0)
            {
                springForce -= springForces[i - 1];
            }

            //spring que corresponde a F2 de la formula
            springForce += springForces[i];

            //masa del segmento de la cuerda, hacemos variable a parte para añadir el peso de la bola al ultimo
            float springMass = massRopeSegment;
            //segmento con la bola
            if (i == 0)
            {
                springMass = massLastRopeSegment;
            }

            //creamos fuerza de gravedad con la masa
            Vector3Class gravityForce = new Vector3Class(0f, gravity, 0f);
            gravityForce *= springMass;

            //sumamos ambas fuerzas calculadas
            Vector3Class totalForce = springForce + gravityForce;
            //finalmente, calculamos la aceleracion resultante de la fuerza
            Vector3Class acceleration = totalForce / springMass;
            CilinderBody temp = ropeSegments[i];
            temp.acc = acceleration;
            ropeSegments[i] = temp;
        }
    }
    private void ImplementMaximumStretch()
    {

        //loop por todos los puntos para ajustarlos, de arriba a abajo
        for (int i = ropeSegments.Count - 1; i > 0; i--)
        {
            //recibimos 2 segmentos para comparar
            Vector3Class topSegment = ropeSegments[i].pos;
            CilinderBody bottomSegment = ropeSegments[i - 1]; //este es CilinderBody porque modificaremos su pos si hace falta

            //distancia entre ambos segmentos
            float dist = (topSegment - bottomSegment.pos).Size();

            //si esta distancia supera el maximo
            if (dist > maxStretch)
            {
                //calculamos la diferencia entre la distancia actual y el maximo fijado
                float diffenceLength = dist - maxStretch;

                //vector normalizado para establecer la direccion del vector para ajustar el segmento
                Vector3Class adjustDirection = topSegment.Normalize(topSegment - bottomSegment.pos);

                //vector resultante para ajustar al segmento
                Vector3Class adjustVec = adjustDirection * diffenceLength;

                //sumamos vector resultante con posicion de bottomsegment y actualizamos pos del segmento
                bottomSegment.pos += adjustVec;
                ropeSegments[i - 1] = bottomSegment;
            }
            else if (dist < minStretch)
            {
                //calculamos la diferencia entre la distancia actual y el maximo fijado
                float differenceLength = minStretch - dist;

                //vector normalizado para establecer la direccion del vector para ajustar el segmento
                Vector3Class adjustDirection = topSegment.Normalize(bottomSegment.pos - topSegment);

                //vector resultante para ajustar al segmento
                Vector3Class adjustVec = adjustDirection * differenceLength;
                //sumamos vector resultante con posicion de bottomsegment y actualizamos pos del segmento
                bottomSegment.pos += adjustVec;
                ropeSegments[i - 1] = bottomSegment;
            }
        }
    }

    public Transform GetBall()
    {
        return pos2;
    }
    public void SetMove(bool mov)
    {
        move = mov;
    }
    public void ResetPendulum(Vector3Class pos)
    {
        //actualiza la posicion de pos2 para setear la bola segn el target de la escena
        pos2.position = pos.GetValues();
        List<CilinderBody> tempRopeSegments = new List<CilinderBody>();

        //recibir las posiciones de los 2 extremos de la cuerda
        Vector3Class initPos = new Vector3Class(pos1.transform.position);
        Vector3Class endPos = new Vector3Class(pos2.transform.position);

        //generador de los puntos de la cuerda, en resumen hace un vector entre pos1,pos2
        Vector3Class stringVector = endPos - initPos;
        stringVector = stringVector.Normalize(stringVector);

        List<Vector3Class> ropePositions = new List<Vector3Class>();
        //determina el punto inicial de todos los segmentos con
        //"largo segmento * posicion en lista * direccion normalizadapos1pos2 + pos1" 
        for (int i = 0; i < stringPartitions; i++)
        {
            Vector3Class partPos = initPos;
            partPos += stringVector * i * stringPartLength;
            ropePositions.Add(partPos);
        }
        //se añaden todos los segmentos con la posicion deseada
        //se introducen los elementos al reves para facilitar algunos calculos
        for (int i = ropePositions.Count - 1; i >= 0; i--)
        {
            tempRopeSegments.Add(new CilinderBody(ropePositions[i]));
        }
        ropeSegments = tempRopeSegments;
        
        //actualiza posicion del objeto bola
        pos2.position = ropeSegments[0].pos.GetValues();
    }

    public void SetGravity(float value)
    {
        gravity = value;
    }
    public void SetIterations(int value)
    {
        iterations = value;
    }
    public void SetPartitions(int value)
    {
        stringPartitions = value;
    }
    public void SetPartLength(float value)
    {
        stringPartLength = value;
    }
    public void SetElasticity(float value)
    {
        kElasticity = value;
    }
    public void SetDamping(float value)
    {
        kDamping = value;
    }
    public void SetMassSegment (float value)
    {
        massRopeSegment = value;
    }
    public void SetLastMassSegment(float value)
    {
        massLastRopeSegment = value;
    }
    public void SetMinStretch (float value)
    {
        minStretch = value;
    }
    public void SetMaxStretch(float value)
    {
        maxStretch = value;
    }
}