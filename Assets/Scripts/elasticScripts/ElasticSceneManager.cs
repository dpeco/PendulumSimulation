using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticSceneManager : MonoBehaviour {

    public PendulumElastic pendulum;
    public Transform targetSphere;
    public bool startSimulation = false;
    float timer;

    enum States { init, stop, simulationNoHand, closeHand, stopPendulum, done };

    States statusSimulation;
    
    void Start()
    {
        pendulum.GetComponent<PendulumElastic>();

        pendulum.SetMove(false);
        statusSimulation = States.done;
        startSimulation = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (statusSimulation != States.done)
            timer += Time.deltaTime;

        if (startSimulation && statusSimulation == States.done) //Start
        {
            startSimulation = false;
            statusSimulation = States.init;
            pendulum.ResetPendulum(new Vector3Class(targetSphere.position));
            pendulum.SetMove(true);
        }
        else if (startSimulation && statusSimulation == States.init) //Stop
        {
            startSimulation = false;
            statusSimulation = States.done;
            pendulum.SetMove(false);
        }
    }
    public void StartTheSimulation()
    {
        startSimulation = true;
    }

}
