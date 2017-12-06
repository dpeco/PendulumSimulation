using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulo : MonoBehaviour {

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

    private Vector3 planeXY;
    private Vector3 planeZY;

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
        planeXY = new Vector3(1, 1, 0);
        planeZY = new Vector3(0, 1, 1);
        
    }

    // Update is called once per frame
    void LateUpdate () {
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
