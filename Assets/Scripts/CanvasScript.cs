using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

    public GameObject manager;
    public GameObject pendulum;
    public GameObject eulerToggle;
    public GameObject gravitySlider;
    public GameObject initAngleXSlider;
    public GameObject initAngleZSlider;
    public GameObject initVelXSlider;
    public GameObject initVelZSlider;

    private bool method = true;

    private SceneManager managerScript;
    private Pendulo pendulumScript;
    //private bool startSimulation;

    // Use this for initialization
    void Start () {
        managerScript = manager.GetComponent<SceneManager>();
        pendulumScript = pendulum.GetComponent<Pendulo>();
    }

    // Update is called once per frame
    public void Simulate() {
        managerScript.startSimulation = true;
    }
    public void setEuler()
    {
        method = !method;
        pendulumScript.method = method;
    }
    public void setGravity() {
        pendulumScript.gravity = gravitySlider.GetComponent<Slider>().value;
    }
    public void setinitAngularVelX() {
        pendulumScript.initAngularVelX = initAngleXSlider.GetComponent<Slider>().value;
    }
    public void setinitAngularVelZ() {
        pendulumScript.initAngularVelZ = initAngleZSlider.GetComponent<Slider>().value;
    }
    public void setinitAngleX() {
        pendulumScript.initAngleX = initVelXSlider.GetComponent<Slider>().value;
    }
    public void setinitAngleZ() {
        pendulumScript.initAngleZ = initVelZSlider.GetComponent<Slider>().value;
    }

}
