﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour {

    public GameObject manager;
    public Pendulo pendulum;

    public Dropdown sceneChoice;

    public Toggle eulerToggle;

    public Slider gravitySlider;
    public Slider initAngleXSlider;
	public Slider initAngleZSlider;
	public Slider initVelXSlider;
	public Slider initVelZSlider;

    public InputField gravInput;
    public InputField inAngleXInput;
    public InputField inAngleZInput;
    public InputField inAngleVelXInput;
    public InputField inAngleVelZInput;

    private bool method = true;

    private SceneManager managerScript;
    //private bool startSimulation;

    // Use this for initialization
    void Start ()
    {
        managerScript = manager.GetComponent<SceneManager>();
        pendulum = pendulum.GetComponent<Pendulo>();

        gravInput.onValueChanged.AddListener(delegate { InputSetGravity(); });
        inAngleXInput.onValueChanged.AddListener(delegate { InputSetInitAngleX(); });
        inAngleZInput.onValueChanged.AddListener(delegate { InputSetInitAngleZ(); });
        inAngleVelXInput.onValueChanged.AddListener(delegate { InputSetInitAngularVelX(); });
        inAngleVelZInput.onValueChanged.AddListener(delegate { InputSetInitAngularVelZ(); });

        gravitySlider.onValueChanged.AddListener(delegate { SetGravity(); });
        initAngleXSlider.onValueChanged.AddListener(delegate { SetInitAngleX(); });
        initAngleZSlider.onValueChanged.AddListener(delegate { SetInitAngleZ(); });
        initVelXSlider.onValueChanged.AddListener(delegate { SetInitAngularVelX(); });
        initVelZSlider.onValueChanged.AddListener(delegate { SetInitAngularVelZ(); });
    }

    //ESTABLECER ESCENA
    public void SetScene()
    {
        if (sceneChoice.value == 0)
        {
            //SceneManager.LoadScene("PendulumScene");
        }
        else {
            //SceneManager.LoadScene("PendulumScene2");
        }
    }

    //COMENZAR IMULACIÓN
    public void Simulate()
    {
        pendulum.ResetPendulum();
        managerScript.startSimulation = true;

        AllowInput(false);
    }

    //EULER
    public void SetEuler()
    {
        method = !method;
        pendulum.method = method;
    }

    //GRAVEDAD
    public void SetGravity() //Establecer gravedad modo scroller
    {
        gravInput.text = gravitySlider.value.ToString(); //Ajustar tambien el valor del input

        //Asignar valor
        pendulum.SetGravity(gravitySlider.GetComponent<Slider>().value);
    }
    public void InputSetGravity() //Establecer gravedad modo input
    {
        gravitySlider.value = float.Parse(gravInput.text); //Ajustar tambien el valor del slider

        //Asignar valor
        pendulum.SetGravity(float.Parse(gravInput.GetComponent<InputField>().text));
    }

    //ANGULO EN X
    public void SetInitAngleX()
    {
        inAngleXInput.text = initAngleXSlider.value.ToString();

        Vector3Class pendulumAngle = new Vector3Class(pendulum.transform.localEulerAngles);
        pendulumAngle.x = initAngleXSlider.GetComponent<Slider>().value;
        pendulum.NewPendulumAngle(pendulumAngle);
    }
    public void InputSetInitAngleX()
    {
        initAngleXSlider.value = float.Parse(inAngleXInput.text);

        Vector3Class pendulumAngle = new Vector3Class(pendulum.transform.localEulerAngles);
        pendulumAngle.x = float.Parse(inAngleXInput.GetComponent<InputField>().text);
        pendulum.NewPendulumAngle(pendulumAngle);
    }

    //ANGULO EN Z
    public void SetInitAngleZ()
    {
        inAngleZInput.text = initAngleZSlider.value.ToString();

        Vector3Class pendulumAngle = new Vector3Class(pendulum.transform.localEulerAngles);
        pendulumAngle.z = initAngleZSlider.GetComponent<Slider>().value;
        pendulum.NewPendulumAngle(pendulumAngle);
    }
    public void InputSetInitAngleZ()
    {
        initAngleZSlider.value = float.Parse(inAngleZInput.text);

        Vector3Class pendulumAngle = new Vector3Class(pendulum.transform.localEulerAngles);
        pendulumAngle.z = float.Parse(inAngleZInput.GetComponent<InputField>().text);
        pendulum.NewPendulumAngle(pendulumAngle);
    }

    //VELOCIDAD ANGULAR INICIAL EN X (Lo mismo...)
    public void SetInitAngularVelX()
    {
        inAngleVelXInput.text = initVelXSlider.value.ToString();

        pendulum.SetAngularVelX(initVelXSlider.GetComponent<Slider>().value);
    }
    public void InputSetInitAngularVelX()
    {
        initVelXSlider.value = float.Parse(inAngleVelXInput.text);
        
        pendulum.SetAngularVelX(float.Parse(inAngleVelXInput.GetComponent<InputField>().text));
    }

    //VELOCIDAD ANGULAR INICIAL EN Z
    public void SetInitAngularVelZ()
    {
        inAngleVelZInput.text = initVelZSlider.value.ToString();

        pendulum.SetAngularVelZ(initVelZSlider.GetComponent<Slider>().value);
    }
    public void InputSetInitAngularVelZ()
    {
        initVelZSlider.value = float.Parse(inAngleVelZInput.text);

        pendulum.SetAngularVelZ(float.Parse(inAngleVelZInput.GetComponent<InputField>().text));
    }

    //BLOQUEO/ACTIVACIÓN DE INPUT
    private void AllowInput(bool to) {

        eulerToggle.interactable = to;

        gravitySlider.interactable = to;
        initAngleXSlider.interactable = to;
        initAngleZSlider.interactable = to;
        initVelXSlider.interactable = to;
        initVelZSlider.interactable = to;

        gravInput.interactable = to;
        inAngleXInput.interactable = to;
        inAngleZInput.interactable = to;
        inAngleVelXInput.interactable = to;
        inAngleVelZInput.interactable = to;

    }
}
