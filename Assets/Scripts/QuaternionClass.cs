using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionClass
{
    public float w;
    public float x;
    public float y;
    public float z;
    // Use this for initialization
    public QuaternionClass()
    {
        w = 0;
        x = 0;
        y = 0;
        z = 0;
    }
    public QuaternionClass(float a, float b, float c, float d)
    {
        w = a;
        x = b;
        y = c;
        z = d;
    }

    QuaternionClass multiply(QuaternionClass q1, QuaternionClass q2)
    {
        QuaternionClass q3 = new QuaternionClass();
        q3.w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
        q3.x = q1.w * q2.x + q1.x * q2.w - q1.y * q2.z + q1.z * q2.y;
        q3.y = q1.w * q2.y + q1.x * q2.z + q1.y * q2.w - q1.z * q2.x;
        q3.z = q1.w * q2.z - q1.x * q2.y + q1.y * q2.x - q1.z * q2.w;

        return q3;
    }
    QuaternionClass inverse(QuaternionClass q1)
    {
        QuaternionClass q2 = new QuaternionClass();

        //transpose
        q2.w = q1.w;
        q2.x = -q1.x;
        q2.y = -q1.y;
        q2.z = -q1.z;
        return multiply(q1, q2);
    }
    void convertFromAxisAngle(Vector3 axis, float a)
    {

        a = a / 360 * (float)Mathf.PI * 2;
        w = Mathf.Cos(a / 2); //si w es menor que 0, l'angle es major que pi (LUL)

        x = axis.x * Mathf.Sin(a / 2);
        y = axis.y * Mathf.Sin(a / 2);
        z = axis.z * Mathf.Sin(a / 2);

    }
    void convertToAxisAngle(QuaternionClass q1)
    {
        float a;
        Vector3 axis;

        a = 2 * Mathf.Acos(q1.w);
        axis.x = q1.x / Mathf.Sqrt(1 - Mathf.Pow(q1.w, 2));
        axis.y = q1.y / Mathf.Sqrt(1 - Mathf.Pow(q1.w, 2));
        axis.z = q1.z / Mathf.Sqrt(1 - Mathf.Pow(q1.w, 2));

    }
    Vector3 rotation()
    {
        return new Vector3(x, y, z);
    }
}