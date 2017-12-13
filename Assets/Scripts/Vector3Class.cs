using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Class {

    public float x;
    public float y;
    public float z;
    // Use this for initialization
    public Vector3Class()
    {
        x = 0;
        y = 0;
        z = 0;
    }
    public Vector3Class(float a, float b, float c)
    {
        x = a;
        y = b;
        z = c;
    }
    public void GetValues(Vector3 vec)
    {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }
    public Vector3 SetValues()
    {
        return new Vector3(x, y, z);
    }
    public float DotProduct(Vector3Class v1, Vector3Class v2)
    {
        float temp;

        temp = (v1.x * v2.x) + (v1.y + v2.y) + (v1.z + v2.z);
        return temp;
    }
    public Vector3Class CrossProduct(Vector3Class v1, Vector3Class v2)
    {
        Vector3Class v3 = new Vector3Class();
        //sarrus
        v3.x = (v1.y * v2.z) - (v1.z * v2.y);
        v3.y = (-1 * (v1.x * v2.z)) + (v1.z * v2.x);
        v3.z = (v1.x * v2.y) - (v1.y * v2.z);
        return v3;
    }
}
