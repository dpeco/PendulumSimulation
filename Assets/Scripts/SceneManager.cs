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

    enum States { init, simulationHandFollow, simulationNoHand, closeHand, stopPendulum, done };

    States statusSimulation;
    public Transform defHandPos;
    public Transform target;

    public ENTICourse.IK.InverseKinematics[] fingers;

    void Start()
    {
        pendulum.GetComponent<Pendulo>();
        pendulum.SetMove(false);
        roboticHand.GetComponent<ENTICourse.IK.InverseKinematics>();
        timer = 0;
        statusSimulation = States.init;
        roboticHand.NewDestination(pendulum.GetBall());

        for(int i = 0; i < 5; i++)
        {
            fingers[i].GetComponent<FingerManager>().CloseHand();
        }
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
            for (int i = 0; i < 5; i++)
            {
                fingers[i].GetComponent<FingerManager>().OpenHand();
            }
        }
        if (statusSimulation == States.simulationNoHand && timer > timeToStopSimulation) //la mano se dirige a la posicion predict
        {
            Vector3Class tPos = new Vector3Class();
            tPos = pendulum.CalculateFuturePosition(timeToPredictStopPosition);
            target.position = tPos.GetValues();
            roboticHand.NewDestination(target);
            timer = 0;
            statusSimulation = States.closeHand;
        }
        if (statusSimulation == States.closeHand && timer > timeToPredictStopPosition - 0.3f) //se para el pendulo
        {
            statusSimulation = States.stopPendulum;
            for (int i = 0; i < 5; i++)
            {
                fingers[i].GetComponent<FingerManager>().CloseHand();
            }
        }
        if (statusSimulation == States.stopPendulum && timer > timeToPredictStopPosition) //se para el pendulo
        {
            pendulum.SetMove(false);
            statusSimulation = States.done;
        }
    }
}

