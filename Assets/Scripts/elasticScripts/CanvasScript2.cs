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
    private bool state = true; 
    private ElasticSceneManager managerScript;

    // Use this for initialization
    void Start()
    {
        managerScript = manager.GetComponent<ElasticSceneManager>();
        pendulum = pendulum.GetComponent<PendulumElastic>();

        gravitySlider.onValueChanged.AddListener(delegate { SetGravity(); });
        iterationsSlider.onValueChanged.AddListener(delegate { SetIterations(); });
        partitionsSlider.onValueChanged.AddListener(delegate { SetStringParts(); });
        partitionsLengthSlider.onValueChanged.AddListener(delegate { SetStringPartsLength(); });
        elasticitySlider.onValueChanged.AddListener(delegate { SetStringElasticity(); });
        dampingSlider.onValueChanged.AddListener(delegate { SetStringDamping(); });
        ropeSegmentMassSlider.onValueChanged.AddListener(delegate { SetMassRopeSegment(); });
        finalRopeSegmentMassSlider.onValueChanged.AddListener(delegate { SetLastRopeSegmentMass(); });
        minStretchSlider.onValueChanged.AddListener(delegate { SetMinStretch(); });
        maxStretchSlider.onValueChanged.AddListener(delegate { SetMaxStretch(); });
        
        gravInput.onValueChanged.AddListener(delegate { InputSetGravity(); });
        iterationsInput.onValueChanged.AddListener(delegate { InputSetIterations(); });
        partitionsInput.onValueChanged.AddListener(delegate { InputSetStringParts(); });
        partitionsLengthInput.onValueChanged.AddListener(delegate { InputSetStringPartsLength(); });
        elasticitySlider.onValueChanged.AddListener(delegate { InputSetStringElasticity(); });
        dampingInput.onValueChanged.AddListener(delegate { InputSetStringDamping(); });
        ropeSegmentMassSlider.onValueChanged.AddListener(delegate { InputSetMassRopeSegment(); });
        finalRopeSegmentMassSlider.onValueChanged.AddListener(delegate { InputSetLastRopeSegmentMass(); });
        minStretchSlider.onValueChanged.AddListener(delegate { InputSetMinStretch(); });
        maxStretchSlider.onValueChanged.AddListener(delegate { InputSetMaxStretch(); });
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
        managerScript.StartTheSimulation();
        state = !state;
        AllowInput(state);
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
    //Iterations
    public void SetIterations() //Establecer gravedad modo scroller
    {
        iterationsInput.text = iterationsSlider.value.ToString(); //Ajustar tambien el valor del input

        //Asignar valor
        pendulum.SetIterations(int.Parse(iterationsInput.GetComponent<InputField>().text));
    }
    public void InputSetIterations() //Establecer gravedad modo input
    {
        iterationsSlider.value = float.Parse(iterationsInput.text); //Ajustar tambien el valor del slider

        //Asignar valor/
        pendulum.SetIterations(int.Parse(iterationsInput.GetComponent<InputField>().text));
    }
    public void SetStringParts()
    {
        partitionsInput.text = partitionsSlider.value.ToString();
        //Asignar valor
        pendulum.SetPartitions(int.Parse(partitionsInput.GetComponent<InputField>().text));
    }
    public void InputSetStringParts()
    {
        partitionsSlider.value = float.Parse(partitionsInput.text);
        //Asignar valor
        pendulum.SetPartitions(int.Parse(partitionsInput.GetComponent<InputField>().text));
    }

    public void SetStringPartsLength()
    {
        partitionsLengthInput.text = partitionsLengthSlider.value.ToString();
        //Asignar valor
        pendulum.SetPartLength(float.Parse(partitionsLengthInput.GetComponent<InputField>().text));
    }
    public void InputSetStringPartsLength()
    {
        partitionsLengthSlider.value = float.Parse(partitionsLengthInput.text);
        //Asignar valor
        pendulum.SetPartLength(float.Parse(partitionsLengthInput.GetComponent<InputField>().text));
    }

    public void SetStringElasticity()
    {
        elasticityInput.text = elasticitySlider.value.ToString();
        //Asignar valor
        pendulum.SetElasticity(float.Parse(elasticityInput.GetComponent<InputField>().text));
    }
    public void InputSetStringElasticity()
    {
        elasticitySlider.value = float.Parse(elasticityInput.text);
        //Asignar valor
        pendulum.SetElasticity(float.Parse(elasticityInput.GetComponent<InputField>().text));
    }

    public void SetStringDamping()
    {
        dampingInput.text = dampingSlider.value.ToString();
        //Asignar valor
        pendulum.SetDamping(float.Parse(dampingInput.GetComponent<InputField>().text));
    }
    public void InputSetStringDamping()
    {
        dampingSlider.value = float.Parse(dampingInput.text);
        //Asignar valor
        pendulum.SetDamping(float.Parse(dampingInput.GetComponent<InputField>().text));
    }

    public void SetMassRopeSegment()
    {
        ropeSegmentMassInput.text = ropeSegmentMassSlider.value.ToString();
        //Asignar valor
        pendulum.SetMassSegment(float.Parse(ropeSegmentMassInput.GetComponent<InputField>().text));
    }
    public void InputSetMassRopeSegment()
    {
        ropeSegmentMassSlider.value = float.Parse(ropeSegmentMassInput.text);
        //Asignar valor
        pendulum.SetMassSegment(float.Parse(ropeSegmentMassInput.GetComponent<InputField>().text));
    }

    public void SetLastRopeSegmentMass()
    {
        finalRopeSegmentMassInput.text = finalRopeSegmentMassSlider.value.ToString();
        //Asignar valor
        pendulum.SetLastMassSegment(float.Parse(finalRopeSegmentMassInput.GetComponent<InputField>().text));
    }
    public void InputSetLastRopeSegmentMass()
    {
        finalRopeSegmentMassSlider.value = float.Parse(finalRopeSegmentMassInput.text);
        //Asignar valor
        pendulum.SetLastMassSegment(float.Parse(finalRopeSegmentMassInput.GetComponent<InputField>().text));
    }

    public void SetMinStretch()
    {
        minStretchInput.text = minStretchSlider.value.ToString();
        //Asignar valor
        pendulum.SetMinStretch(float.Parse(minStretchInput.GetComponent<InputField>().text));
    }
    public void InputSetMinStretch()
    {
        minStretchSlider.value = float.Parse(minStretchInput.text);
        //Asignar valor
        pendulum.SetMinStretch(float.Parse(minStretchInput.GetComponent<InputField>().text));
    }

    public void SetMaxStretch()
    {
        maxStretchInput.text = maxStretchSlider.value.ToString();
        //Asignar valor
        pendulum.SetMaxStretch(float.Parse(maxStretchInput.GetComponent<InputField>().text));
    }
    public void InputSetMaxStretch()
    {
        maxStretchSlider.value = float.Parse(maxStretchInput.text);
        //Asignar valor
        pendulum.SetMaxStretch(float.Parse(maxStretchInput.GetComponent<InputField>().text));
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
