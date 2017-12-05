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

    public bool method;
    // Use this for initialization
    void Start () {
        stringDistance = Vector2.Distance(pos1.position, pos2.position);

        initAngleX = pos1.transform.eulerAngles.x;

        initAngleZ = pos1.transform.eulerAngles.z;

        if (initAngleX > 180) initAngleX -= 360;
        if (initAngleZ < -180) initAngleZ += 360;

        curAngleX = initAngleX * Mathf.Deg2Rad;
        curAngularVelX = initAngularVelX;

        curAngleZ = initAngleZ * Mathf.Deg2Rad;;
        curAngularVelZ = initAngularVelZ;


    }

    // Update is called once per frame
    void Update () {
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

            pos1.transform.localEulerAngles = new Vector3(newAngleX, 0, newAngleZ);
        }
        else
        {
            //Mathf.Sin(curAngleX * Mathf.Rad2Deg)
            float gl = gravity / stringDistance;
            float drag;
            //calculo numerico
            drag = cDrag / stringDistance * curAngularVelX;
            curAngularVelX += Time.deltaTime * ((-1 * gl * Mathf.Sin(curAngleX)) - drag);
            curAngleX += Time.deltaTime * curAngularVelX;

            drag = cDrag / stringDistance * curAngularVelZ;
            curAngularVelZ += Time.deltaTime * ((-1 * gl * Mathf.Sin(curAngleZ)) - drag);
            curAngleZ += Time.deltaTime * curAngularVelZ;

            if (curAngleX > Mathf.PI) curAngleX -= 2 * Mathf.PI;
            if (curAngleX < -Mathf.PI) curAngleX += 2 * Mathf.PI;

            if (curAngleZ > Mathf.PI) curAngleZ -= 2 * Mathf.PI;
            if (curAngleZ < -Mathf.PI) curAngleZ += 2 * Mathf.PI;

            pos1.transform.localEulerAngles = new Vector3(curAngleX * Mathf.Rad2Deg, 0, curAngleZ * Mathf.Rad2Deg);
        }
    }
}
