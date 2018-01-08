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
    }

    // Update is called once per frame
    void Update()
    {

        if (statusSimulation != States.done)
            timer += Time.deltaTime;

        if (startSimulation /*Input.GetKeyDown("space")*/ && statusSimulation == States.done) //Start
        {
            startSimulation = false;
            statusSimulation = States.init;
            pendulum.ResetPendulum(new Vector3Class(targetSphere.position));
            pendulum.SetMove(true);
        }
        /*else if (Input.GetKeyDown("space") && statusSimulation == States.init) //Stop
        {
            startSimulation = false;
            statusSimulation = States.done;
            pendulum.SetMove(false);
        }*/
    }
    void StartTheSimulation()
    {
        startSimulation = true;
    }

}
