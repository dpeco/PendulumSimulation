using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasScript2 : MonoBehaviour
{

    public GameObject manager;
    public PendulumElastic pendulum;

    public Dropdown sceneChoice;

    public Camera theCamera;

    public Slider gravitySlider;
    public Slider iterationsSlider;
    public Slider partitionsSlider;
    public Slider partitionsLengthSlider;
    public Slider elasticitySlider;
    public Slider dampingSlider;
    public Slider ropeSegmentMassSlider;
    public Slider finalRopeSegmentMassSlider;
    public Slider minStretchSlider;
    public Slider maxStretchSlider;

    public InputField gravInput;
    public InputField iterationsInput;
    public InputField partitionsInput;
    public InputField partitionsLengthInput;
    public InputField elasticityInput;
    public InputField dampingInput;
    public InputField ropeSegmentMassInput;
    public InputField finalRopeSegmentMassInput;
    public InputField minStretchInput;
    public InputField maxStretchInput;

    private bool method = true;

    private ElasticSceneManager managerScript;

    // Use this for initialization
    void Start()
    {
        managerScript = manager.GetComponent<ElasticSceneManager>();
        pendulum = pendulum.GetComponent<PendulumElastic>();

        gravitySlider.onValueChanged.AddListener(delegate { SetGravity(); });


        gravInput.onValueChanged.AddListener(delegate { InputSetGravity(); });

    }

    //ESTABLECER ESCENA
    public void SetScene()
    {
        if (sceneChoice.value == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PendulumScene");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PendulumScene2");
        }
    }

    //COMENZAR IMULACIÓN
    public void Simulate()
    {
        //pendulum.ResetPendulum();
        managerScript.startSimulation = true;

        AllowInput(false);
    }

    //CAMERA
    public void FreeCamera()
    {
        theCamera.GetComponent<CameraScript>().enabled = !theCamera.GetComponent<CameraScript>().enabled;
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

    public void SetStringParts()
    {

    }
    public void InputSetStringParts()
    {

    }

    public void SetStringPartsLength()
    {

    }
    public void InputSetStringPartsLength()
    {

    }

    public void SetStringElasticity()
    {

    }
    public void InputSetStringElasticity()
    {

    }

    public void SetStringDamping()
    {

    }
    public void InputSetStringDamping()
    {

    }

    public void SetMassRopeSegment()
    {

    }
    public void InputSetMassRopeSegment()
    {

    }

    public void SetLastRopeSegmentMass()
    {

    }
    public void InputSetLastRopeSegmentMass()
    {

    }

    //BLOQUEO/ACTIVACIÓN DE INPUT
    private void AllowInput(bool to)
    {
        gravitySlider.interactable = to;
        iterationsSlider.interactable = to;
        partitionsSlider.interactable = to;
        partitionsLengthSlider.interactable = to;
        elasticitySlider.interactable = to;
        dampingSlider.interactable = to;
        ropeSegmentMassSlider.interactable = to;
        finalRopeSegmentMassSlider.interactable = to;
        minStretchSlider.interactable = to;
        maxStretchSlider.interactable = to;

        gravInput.interactable = to;
        iterationsInput.interactable = to;
        partitionsInput.interactable = to;
        partitionsLengthInput.interactable = to;
        elasticityInput.interactable = to;
        dampingInput.interactable = to;
        ropeSegmentMassInput.interactable = to;
        finalRopeSegmentMassInput.interactable = to;
        minStretchInput.interactable = to;
        maxStretchInput.interactable = to;
    }
}
