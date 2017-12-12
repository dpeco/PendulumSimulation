using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENTICourse.IK
{

    // A typical error function to minimise
    public delegate float ErrorFunction(Vector3 target, float[] solution);

    public struct PositionRotation
    {
        Vector3 position;
        Quaternion rotation;

        public PositionRotation(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        // PositionRotation to Vector3
        public static implicit operator Vector3(PositionRotation pr)
        {
            return pr.position;
        }
        // PositionRotation to Quaternion
        public static implicit operator Quaternion(PositionRotation pr)
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
        private Vector3 target;

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


        public ErrorFunction ErrorFunction;



        [Header("Debug")]
        public bool DebugDraw = true;



        // Use this for initialization
        void Start()
        {
            Joints = null;
            if (Joints == null)
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
            target = Destination.transform.position;
            //ApproachTarget(target);
            //ForwardKinematics(Solution);



            if (ErrorFunction(target, Solution) > StopThreshold)
            {
                ApproachTarget(target);
                for (int i = 0; i < Joints.Length; i++)
                {
                    Joints[i].MoveArm(Solution[i]);
                }
            }

            if (DebugDraw)
            {
                Debug.DrawLine(Effector.transform.position, target, Color.green);
                Debug.DrawLine(Destination.transform.position, target, new Color(0, 0.5f, 0));
            }
        }

        public void ApproachTarget(Vector3 target)
        {
            for (int i = 0; i < Solution.Length; i++)
            {
                float pendent = CalculateGradient(target, Solution, i, DeltaGradient);
                Solution[i] -= LearningRate * pendent;
            }


        }


        public float CalculateGradient(Vector3 target, float[] Solution, int i, float delta)
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
        public float DistanceFromTarget(Vector3 target, float[] Solution)
        {
            Vector3 point = ForwardKinematics(Solution);
            return Vector3.Distance(point, target);
        }


        /* Simulates the forward kinematics,
         * given a solution. */


        public PositionRotation ForwardKinematics(float[] Solution)
        {
            Vector3 prevPoint = Joints[0].transform.position;


            // Takes object initial rotation into account
            Quaternion rotation = transform.rotation;
            for (int i = 1; i < Joints.Length; i++)
            {
                rotation *= Quaternion.AngleAxis(Solution[i - 1], Joints[i - 1].Axis);
                Vector3 nextPoint = prevPoint + rotation * Joints[i].StartOffset;

                if (DebugDraw)
                    Debug.DrawLine(prevPoint, nextPoint, Color.blue);

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