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
        x = a;
        y = b;
        z = c;
        w = d;
    }
    public QuaternionClass(Quaternion quat)
    {
        x = quat.x;
        y = quat.y;
        z = quat.z;
        w = quat.w;
    }
    public QuaternionClass multiply(QuaternionClass q2, QuaternionClass q1)
    {
        QuaternionClass q3 = new QuaternionClass();
        q3.w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
        q3.x = q1.w * q2.x + q1.x * q2.w - q1.y * q2.z + q1.z * q2.y;
        q3.y = q1.w * q2.y + q1.x * q2.z + q1.y * q2.w - q1.z * q2.x;
        q3.z = q1.w * q2.z - q1.x * q2.y + q1.y * q2.x - q1.z * q2.w;

        return q3;
    }
    public Vector3 multiplyVec3(QuaternionClass quat, Vector3 vec)
    {
        Vector3 temp;

        //rotation matrix
        float xPart1 = 1 - 2 * Mathf.Pow(quat.y, 2) - 2 * Mathf.Pow(quat.z, 2);
        float xPart2 = 2 * quat.x * quat.y - 2 * quat.z * quat.w;
        float xPart3 = 2 * quat.x * quat.z + 2 * quat.y * quat.w;

        float yPart1 = 2 * quat.x * quat.y + 2 * quat.z * quat.w;
        float yPart2 = 1 - 2 * Mathf.Pow(quat.x, 2) - 2 * Mathf.Pow(quat.z, 2);
        float yPart3 = 2 * quat.y * quat.z - 2 * quat.x * quat.w;

        float zPart1 = 2 * quat.x * quat.z - 2 * quat.y * quat.w;
        float zPart2 = 2 * quat.y * quat.z + 2 * quat.x * quat.w;
        float zPart3 = 1 - 2 * Mathf.Pow(quat.x, 2) - 2 * Mathf.Pow(quat.y, 2);

        //calculo vector
        temp.x = xPart1 * vec.x + xPart2 * vec.y + xPart3 * vec.z;
        temp.y = yPart1 * vec.x + yPart2 * vec.y + yPart3 * vec.z;
        temp.z = zPart1 * vec.x + zPart2 * vec.y + zPart3 * vec.z;

        return temp;

    }
    public QuaternionClass inverse(QuaternionClass q1)
    {
        QuaternionClass q2 = new QuaternionClass();

        //transpose
        q2.w = q1.w;
        q2.x = -q1.x;
        q2.y = -q1.y;
        q2.z = -q1.z;
        return multiply(q1, q2);
    }
    public void convertFromAxisAngle(Vector3 axis, float a)
    {

        a = a / 360 * (float)Mathf.PI * 2;
        w = Mathf.Cos(a / 2); //si w es menor que 0, l'angle es major que pi (LUL)

        x = axis.x * Mathf.Sin(a / 2);
        y = axis.y * Mathf.Sin(a / 2);
        z = axis.z * Mathf.Sin(a / 2);

    }
    public void convertToAxisAngle(QuaternionClass q1)
    {
        float a;
        Vector3 axis;

        a = 2 * Mathf.Acos(q1.w);
        axis.x = q1.x / Mathf.Sqrt(1 - Mathf.Pow(q1.w, 2));
        axis.y = q1.y / Mathf.Sqrt(1 - Mathf.Pow(q1.w, 2));
        axis.z = q1.z / Mathf.Sqrt(1 - Mathf.Pow(q1.w, 2));

    }
    public Vector3Class rotation()
    {
        return new Vector3Class(x, y, z);
    }

    public void SetValues(Quaternion quat)
    {
        x = quat.x;
        y = quat.y;
        z = quat.z;
        w = quat.w;
    }
    public Quaternion GetValues()
    {
        return new Quaternion(x, y, z, w);
    }

    public static QuaternionClass operator *(QuaternionClass q2, QuaternionClass q1)
    {
        QuaternionClass q3 = new QuaternionClass();
        q3.w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
        q3.x = q1.w * q2.x + q1.x * q2.w - q1.y * q2.z + q1.z * q2.y;
        q3.y = q1.w * q2.y + q1.x * q2.z + q1.y * q2.w - q1.z * q2.x;
        q3.z = q1.w * q2.z - q1.x * q2.y + q1.y * q2.x - q1.z * q2.w;

        return q3;
    }
}