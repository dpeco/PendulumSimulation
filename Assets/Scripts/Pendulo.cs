using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulo : MonoBehaviour {

    public bool move;

    public Transform pos1;
    public Transform pos2;

    public float gravity;
    public float cDrag;
    public float initAngularVelX;
    public float initAngleX;

    public float initAngularVelZ;
    public float initAngleZ;

    private float curAngularVelX;
    public float curAngleX;

    private float curAngularVelZ;
    private float curAngleZ;

    public float stringDistance;

    private float time = 0; //Tiempo total de ejecución

    public bool method;

    // Use this for initialization
    void Start ()
    {
        if (method)
        {
            initAngularVelX /= Mathf.Rad2Deg;
            initAngularVelZ /= Mathf.Rad2Deg;
        }
        print(transform.rotation.eulerAngles.y);
        Vector3 stringVector = pos2.position - pos1.position;

        initAngleX = transform.localEulerAngles.x;
        initAngleZ = transform.localEulerAngles.z;
        
        curAngleX = initAngleX * Mathf.Deg2Rad;
        curAngularVelX = initAngularVelX;

        curAngleZ = initAngleZ * Mathf.Deg2Rad;
        curAngularVelZ = initAngularVelZ;
        stringDistance = stringVector.magnitude;
        //regula angulos
        if (curAngleX > Mathf.PI)
        {
            curAngleX -= 2 * Mathf.PI;
            print("xd");
        }
        if (curAngleX < -Mathf.PI)
        {
            curAngleX += 2 * Mathf.PI;
            print("xd");
        }

        if (curAngleZ > Mathf.PI)
        {
            curAngleZ -= 2 * Mathf.PI;
            print("xd");
        }
        if (curAngleZ < -Mathf.PI)
        {
            curAngleZ += 2 * Mathf.PI;
            print("xd");
        }
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (move)
        {
            time += Time.deltaTime;

            if (!method)
            {

                //FORMULA CALCULO DE ANGULO:
                //PART1
                float formulaPart1X;

                formulaPart1X = initAngularVelX / Mathf.Sqrt(gravity / stringDistance) * Mathf.Sin(Mathf.Sqrt(gravity / stringDistance) * time);
                //PART2
                float formulaPart2X;

                formulaPart2X = initAngleX * Mathf.Cos(Mathf.Sqrt(gravity / stringDistance) * time);

                float newAngleX = formulaPart1X + formulaPart2X;

                //---------

                //FORMULA CALCULO DE ANGULO:
                //PART1
                float formulaPart1Z;

                formulaPart1Z = initAngularVelZ / Mathf.Sqrt(gravity / stringDistance) * Mathf.Sin(Mathf.Sqrt(gravity / stringDistance) * time);
                //PART2
                float formulaPart2Z;

                formulaPart2Z = initAngleZ * Mathf.Cos(Mathf.Sqrt(gravity / stringDistance) * time);

                float newAngleZ = formulaPart1Z + formulaPart2Z;

                //---------

                transform.localEulerAngles = new Vector3(newAngleX, 0, newAngleZ);

            }

            else
            {
                Vector3 stringVector = pos2.position - pos1.position;
                /*
                Vector3 projectedX = (Mathf.Abs(Vector3.Dot(stringVector, planeXY)) / Mathf.Abs(Vector3.Dot(planeXY, planeXY))) * planeXY;
                Vector3 projectedZ = (Mathf.Abs(Vector3.Dot(stringVector, planeZY)) / Mathf.Abs(Vector3.Dot(planeZY, planeZY))) * planeZY;
                */
                Vector3 stringX = stringVector;
                stringX.x = 0;
                Vector3 stringZ = stringVector;
                stringZ.z = 0;

                float glX = gravity / stringX.magnitude;
                float glZ = gravity / stringZ.magnitude;

                float dragX = cDrag / stringX.magnitude * curAngularVelX;
                float dragZ = cDrag / stringZ.magnitude * curAngularVelZ;

                //calculo numerico pendulo
                curAngularVelX += Time.deltaTime * ((-1 * glX * Mathf.Sin(curAngleX)) - dragX);
                curAngleX += Time.deltaTime * curAngularVelX;

                curAngularVelZ += Time.deltaTime * ((-1 * glZ * Mathf.Sin(curAngleZ)) - dragZ);
                curAngleZ += Time.deltaTime * curAngularVelZ;

                //regula angulos
                if (curAngleX > Mathf.PI)
                {
                    curAngleX -= 2 * Mathf.PI;
                    print("xd");
                }
                if (curAngleX < -Mathf.PI)
                {
                    curAngleX += 2 * Mathf.PI;
                    print("xd");
                }

                if (curAngleZ > Mathf.PI)
                {
                    curAngleZ -= 2 * Mathf.PI;
                    print("xd");
                }
                if (curAngleZ < -Mathf.PI)
                {
                    curAngleZ += 2 * Mathf.PI;
                    print("xd");
                }

                transform.localEulerAngles = new Vector3(curAngleX * Mathf.Rad2Deg, 0, curAngleZ * Mathf.Rad2Deg); ;
            }
        }
    }

    public Vector3 CalculateFuturePosition(float seconds)
    {

        float timeCalc = 0;

        Vector3 initAngles = transform.localEulerAngles;

        float tempAngleX = curAngleX;
        float tempAngleZ = curAngleZ;
        float tempAngularVelX = curAngularVelX;
        float tempAngularVelZ = curAngularVelZ;

        while (timeCalc < seconds)
        {
            timeCalc += Time.deltaTime;

            Vector3 stringVector = pos2.position - pos1.position;

            Vector3 stringX = stringVector;
            stringX.x = 0;
            Vector3 stringZ = stringVector;
            stringZ.z = 0;

            float glX = gravity / stringX.magnitude;
            float glZ = gravity / stringZ.magnitude;

            float dragX = cDrag / stringX.magnitude * tempAngularVelX;
            float dragZ = cDrag / stringZ.magnitude * tempAngularVelZ;

            //calculo numerico pendulo
            tempAngularVelX += Time.deltaTime * ((-1 * glX * Mathf.Sin(tempAngleX)) - dragX);
            tempAngleX += Time.deltaTime * tempAngularVelX;

            tempAngularVelZ += Time.deltaTime * ((-1 * glZ * Mathf.Sin(tempAngleZ)) - dragZ);
            tempAngleZ += Time.deltaTime * tempAngularVelZ;

            //regula angulos
            if (tempAngleX > Mathf.PI)
            {
                curAngleX -= 2 * Mathf.PI;
                print("xd");
            }
            if (tempAngleX < -Mathf.PI)
            {
                curAngleX += 2 * Mathf.PI;
                print("xd");
            }

            if (tempAngleZ > Mathf.PI)
            {
                curAngleZ -= 2 * Mathf.PI;
                print("xd");
            }
            if (tempAngleZ < -Mathf.PI)
            {
                curAngleZ += 2 * Mathf.PI;
                print("xd");
            }

            transform.localEulerAngles = new Vector3(tempAngleX * Mathf.Rad2Deg, 0, tempAngleZ * Mathf.Rad2Deg);
        }
        Vector3 predictedTarget = pos2.position;
        transform.localEulerAngles = initAngles;
        return predictedTarget;
    }

    public void SetMove(bool var)
    {
        move = var;
    }
    public Transform GetBall()
    {
        return pos2;
    }
}