using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public Pendulo pendulum;
    public ENTICourse.IK.InverseKinematics roboticHand;

    public float timeToStartSimulation;
    public float timeToStopHandFollow;
    public float timeToStopSimulation;
    public float timeToPredictStopPosition;

    float timer;

    enum States { init, simulationHandFollow, simulationNoHand, stop, done };

    States statusSimulation;
    public Transform defHandPos;
    public Transform target;
    void Start()
    {
        pendulum.GetComponent<Pendulo>();
        pendulum.SetMove(false);
        roboticHand.GetComponent<ENTICourse.IK.InverseKinematics>();
        timer = 0;
        statusSimulation = States.init;
        roboticHand.NewDestination(pendulum.GetBall());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (statusSimulation == States.init && timer > timeToStartSimulation) //empieza movimiento pendulo con la mano
        {
            pendulum.SetMove(true);
            timer = 0;
            statusSimulation = States.simulationHandFollow;
        }
        if (statusSimulation == States.simulationHandFollow && timer > timeToStopHandFollow) //la mano deja de seguir al pendulo
        {
            target = defHandPos;
            roboticHand.NewDestination(target);
            statusSimulation = States.simulationNoHand;
        }
        if (statusSimulation == States.simulationNoHand && timer > timeToStopSimulation) //la mano se dirige a la posicion predict
        {
            target.position = pendulum.CalculateFuturePosition(timeToPredictStopPosition);
            roboticHand.NewDestination(target);
            timer = 0;
            statusSimulation = States.stop;
        }
        if (statusSimulation == States.stop && timer > timeToPredictStopPosition) //se para el pendulo
        {
            pendulum.SetMove(false);
            statusSimulation = States.done;
        }
    }
}

