using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CilinderBody {
    
    public Vector3Class pos;
    public Vector3Class speed;
    public Vector3Class acc;

    public CilinderBody(Vector3Class position)
    {
        pos = position;
        speed = new Vector3Class();
        acc = new Vector3Class();
    }
}
