using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

    public GameObject manager;
    public Pendulo pendulum;
    public GameObject eulerToggle;
    public GameObject gravitySlider;
    public Slider initAngleXSlider;
	public Slider initAngleZSlider;
	public Slider initVelXSlider;
	public Slider initVelZSlider;

    private bool method = true;

    private SceneManager managerScript;
    //private bool startSimulation;

    // Use this for initialization
    void Start () {
        managerScript = manager.GetComponent<SceneManager>();
        pendulum = pendulum.GetComponent<Pendulo>();


        initAngleXSlider.onValueChanged.AddListener(delegate { SetinitAngleX(); });
        initAngleZSlider.onValueChanged.AddListener(delegate { SetinitAngleZ(); });
        initVelXSlider.onValueChanged.AddListener(delegate { SetinitAngularVelX(); });
        initVelZSlider.onValueChanged.AddListener(delegate { SetinitAngularVelZ(); });

    }

    // Update is called once per frame
    public void Simulate() {
        pendulum.ResetPendulum();
        managerScript.startSimulation = true;

    }
    public void setEuler()
    {
        method = !method;
        pendulum.method = method;
    }
    public void setGravity() {
        pendulum.gravity = gravitySlider.GetComponent<Slider>().value;
    }
    public void SetinitAngularVelX() {
        pendulum.SetAngularVelX(initVelXSlider.GetComponent<Slider>().value);
    }
    public void SetinitAngularVelZ() {
        pendulum.SetAngularVelZ(initVelZSlider.GetComponent<Slider>().value);
    }
    public void SetinitAngleX()
    {
        Vector3Class pendulumAngle = new Vector3Class(pendulum.transform.localEulerAngles);
        pendulumAngle.x = initAngleXSlider.GetComponent<Slider>().value;
        pendulum.NewPendulumAngle(pendulumAngle);
        print(pendulumAngle.x);
    }
    public void SetinitAngleZ() {
        Vector3Class pendulumAngle = new Vector3Class(pendulum.transform.localEulerAngles);
        pendulumAngle.z = initAngleZSlider.GetComponent<Slider>().value;
        pendulum.NewPendulumAngle(pendulumAngle);
    }

}
