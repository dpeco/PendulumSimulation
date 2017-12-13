using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerManager : MonoBehaviour {

    public Transform posOpen;
    public Transform posClosed;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OpenHand()
    {
        this.GetComponent<ENTICourse.IK.InverseKinematics>().NewDestination(posOpen);
    }
    public void CloseHand()
    {
        this.GetComponent<ENTICourse.IK.InverseKinematics>().NewDestination(posClosed);
    }
}
