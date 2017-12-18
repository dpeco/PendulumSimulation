using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENTICourse.IK
{

    // A typical error function to minimise
    public delegate float ErrorFunction(Vector3Class target, float[] solution);

    public struct PositionRotation
    {
        Vector3Class position;
        QuaternionClass rotation;

        public PositionRotation(Vector3Class position, QuaternionClass rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        // PositionRotation to Vector3
        public static implicit operator Vector3Class(PositionRotation pr)
        {
            return pr.position;
        }
        // PositionRotation to Quaternion
        public static implicit operator QuaternionClass(PositionRotation pr)
        {
            return pr.rotation;
        }
    }

    //[ExecuteInEditMode]
    public class InverseKinematics : MonoBehaviour
    {
        [Header("Joints")]
        public Transform BaseJoint;


        public RobotJoint[] Joints = null;
        // The current angles
        public float[] Solution = null;

        [Header("Destination")]
        public Transform Effector;
        [Space]
        public Transform Destination;
        public float DistanceFromDestination;
        private Vector3Class target;

        [Header("Inverse Kinematics")]
        [Range(0, 1f)]
        public float DeltaGradient = 0.1f; // Used to simulate gradient (degrees)
        [Range(0, 1000f)]
        public float LearningRate = 0.1f; // How much we move depending on the gradient

        [Space()]
        [Range(0, 0.25f)]
        public float StopThreshold = 0.1f; // If closer than this, it stops
        [Range(0, 10f)]
        public float SlowdownThreshold = 0.25f; // If closer than this, it linearly slows down
        public bool perfFollow;

        public ErrorFunction ErrorFunction;



        [Header("Debug")]
        public bool DebugDraw = true;



        // Use this for initialization
        void Start()
        {
            if (this.tag == "finger")
                GetJoints();

            ErrorFunction = DistanceFromTarget;
        }

        public void GetJoints()
        {
            Joints = BaseJoint.GetComponentsInChildren<RobotJoint>();
            Solution = new float[Joints.Length];
        }



        // Update is called once per frame
        void Update()
        {
            target = new Vector3Class(Destination.transform.position);

            if (this.tag == "offset")
            {
                //Algunos ajustes para que la mano se posicione relativamente del target con mas realismo
                Vector3Class P = new Vector3Class(BaseJoint.transform.position); //Joints[0].transform.position ??
                Vector3Class Q = new Vector3Class(Destination.transform.position);

                //Normalizar distancia de Q(Pendulo) a P(Effector) y multiplicar por el radio de la bola del pendulo + offset adicional de ajuste para obtener el offset del target total:
                Vector3Class offsetTarget = Q - P;
                //Debug.Log("x" + offsetTarget.x + "y" + offsetTarget.y + "z" + offsetTarget.z);

                float totalOffset = 0.3f;//<-Poner aqui un offset adicional... En realidad sería la mitad del grosor de la mano si el endefector HAND estuviese justo a la mitad.
                offsetTarget = offsetTarget.Normalize(offsetTarget) * (offsetTarget.Size() - totalOffset);
                offsetTarget += P; //Actualiza posicion del objetivo
                offsetTarget.y = target.y;
                target = offsetTarget;
            }
            //ApproachTarget(target);
            //ForwardKinematics(Solution);

            if (perfFollow && tag == "arm")
            {
                int counterWhile = 0;
                while (counterWhile < 20)
                {
                    counterWhile++;
                    if (ErrorFunction(target, Solution) > StopThreshold)
                    {
                        ApproachTarget(target);
                        for (int i = 0; i < Joints.Length; i++)
                        {
                            Joints[i].MoveArm(Solution[i]);
                        }
                    }
                    else break;
                }
            }
            else
            {
                if (ErrorFunction(target, Solution) > StopThreshold)
                {
                    ApproachTarget(target);
                    for (int i = 0; i < Joints.Length; i++)
                    {
                        Joints[i].MoveArm(Solution[i]);
                    }
                }
            }

            if (DebugDraw)
            {
                Debug.DrawLine(Effector.transform.position, target.GetValues(), Color.green);
                Debug.DrawLine(Destination.transform.position, target.GetValues(), new Color(0, 0.5f, 0));
            }
        }

        public void ApproachTarget(Vector3Class target)
        {
            for (int i = 0; i < Solution.Length; i++)
            {
                float pendent = CalculateGradient(target, Solution, i, DeltaGradient);
                Solution[i] -= LearningRate * pendent;
            }

        }


        public float CalculateGradient(Vector3Class target, float[] Solution, int i, float delta)
        {
            float gradient;

            //pas1
            float dist1 = DistanceFromTarget(target, Solution);
            //pas2
            Solution[i] += delta;
            //pas3
            float dist2 = DistanceFromTarget(target, Solution);

            gradient = (dist2 - dist1) / delta;

            return gradient;
        }

        // Returns the distance from the target, given a solution
        public float DistanceFromTarget(Vector3Class target, float[] Solution)
        {
            Vector3Class point = ForwardKinematics(Solution);
            return point.Distance(point, target);
        }


        /* Simulates the forward kinematics,
         * given a solution. */


        public PositionRotation ForwardKinematics(float[] Solution)
        {
            Vector3Class prevPoint = new Vector3Class(Joints[0].transform.position);

            // Takes object initial rotation into account
            QuaternionClass rotation = new QuaternionClass();
            rotation.SetValues(transform.rotation);
            for (int i = 1; i < Joints.Length; i++)
            {
                QuaternionClass angleAxis = new QuaternionClass();
                angleAxis.convertFromAxisAngle(Joints[i - 1].Axis, Solution[i - 1]);

                rotation *= angleAxis;
                Vector3Class nextPoint = prevPoint + rotation.multiplyVec3(rotation, Joints[i].StartOffset);

                if (DebugDraw)
                    Debug.DrawLine(prevPoint.GetValues(), nextPoint.GetValues(), Color.blue);

                prevPoint = nextPoint;
            }

            // The end of the effector
            return new PositionRotation(prevPoint, rotation);
        }
        public void NewDestination(Transform newtarget)
        {
            Destination = newtarget;
        }


    }

}