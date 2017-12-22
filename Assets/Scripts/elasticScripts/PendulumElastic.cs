using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumElastic : MonoBehaviour {

    public CilinderBody[] rigidBodies;
    public float[] initSpace;
    public float kElasticity;
    public float kDamping;
    public float gravity;
    public float maxDist;
    void Start () {
        rigidBodies = this.GetComponentsInChildren<CilinderBody>();
        Vector3Class temp = new Vector3Class();

        initSpace = new float[rigidBodies.Length - 1];
        for (int i = 0; i < rigidBodies.Length - 1; i++){
            initSpace[i] = temp.Distance(rigidBodies[i].pos, rigidBodies[i + 1].pos);
        }
        //ResetForces();
    }
	
	// Update is called once per frame
	void Update () {
        //calculate all spring forces
		for (int i = 0; i < rigidBodies.Length - 1; i++) 
        {
            //get force from string
            Vector3Class forceString = StringForces(rigidBodies[i], rigidBodies[i + 1], initSpace[i]);
            rigidBodies[i].force += forceString; //+F
            rigidBodies[i + 1].force -= forceString; //-F

            if (i < rigidBodies.Length - 2)
            {
                Vector3Class forceString2 = StringForces(rigidBodies[i], rigidBodies[i + 1], initSpace[i]);
                rigidBodies[i].force += forceString2; //+F
                rigidBodies[i + 2].force -= forceString2; //-F
            }
        }
        UpdateSprings();
        //check max distances
        for (int i = 0; i < rigidBodies.Length - 1; i++)
        {
            Vector3Class tmp = new Vector3Class();
            float dist = tmp.Distance(rigidBodies[i].pos, rigidBodies[i + 1].pos);
            if (dist > maxDist)
            {
                Vector3Class vec = rigidBodies[i + 1].pos - rigidBodies[i].pos;
                vec = vec.Normalize(vec);
                rigidBodies[i + 1].pos = rigidBodies[i].pos + vec * maxDist;
            }
        }

        ResetForces();


        for (int i = 0; i < rigidBodies.Length - 1; i++)
        {
            Debug.DrawLine(rigidBodies[i].pos.GetValues(), rigidBodies[i + 1].pos.GetValues(), Color.blue);
        }
    }
    Vector3Class StringForces(CilinderBody p1, CilinderBody p2, float sSpace)
    {
        //we set up some new variables so typing the spring formula
        Vector3Class ptV = p1.pos - p2.pos; //p1 - p2
        float ptD = ptV.Size(); // ||p1-p2||
        Vector3Class ptMod = new Vector3Class(ptV.x / ptD, ptV.y / ptD, ptV.z / ptD); //  p1 - p2 / ||p1-p2||

        Vector3Class ptVelV = p1.speed - p2.speed; // v1 - v2
                                                                                 //spring formula
        float part1 = -1 * (kElasticity * (ptD - sSpace)) + (ptVelV.DotProduct(ptVelV * kDamping, ptMod)); //one part of the formula
        Vector3Class force = new Vector3Class(ptMod.x * part1, ptMod.y * part1, ptMod.z * part1);
        return force;
    }
    void UpdateSprings()
    {
        for (int i = 1; i < rigidBodies.Length; i++)
        {
            rigidBodies[i].ManualUpdate();
        }
    }
    void ResetForces()
    {
        for (int i = 1; i < rigidBodies.Length; i++)
        {
            rigidBodies[i].resetForce(gravity);
        }
    }
}
