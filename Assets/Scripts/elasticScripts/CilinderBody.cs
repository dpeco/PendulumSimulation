using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CilinderBody : MonoBehaviour {
    
    public Vector3Class pos;
    public Vector3Class speed;
    public Vector3Class force;
    public Vector3 initForce;
    public float mass;
	void Start () {
        pos = new Vector3Class(this.transform.position);
        speed = new Vector3Class(0, 0 ,0);
        force = new Vector3Class(initForce);
	}
	public void resetForce(float gravity)
    {
        force = new Vector3Class(0, gravity, 0);
    }
    public void ManualUpdate()
    {
        speed += force * Time.deltaTime;
        pos += speed * Time.deltaTime;
        transform.position = pos.GetValues();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
