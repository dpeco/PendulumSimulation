using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour {

    public GameObject manager;

    private SceneManager managerScript;

    //private bool startSimulation;

    // Use this for initialization
    void Start () {
        managerScript = manager.GetComponent<SceneManager>();
        
    }
	
	// Update is called once per frame
	public void Simulate () {
        managerScript.startSimulation = true;
    }
}
