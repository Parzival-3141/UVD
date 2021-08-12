using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UVD.Utility
{
    public class PDController
    {
        // @Incomplete @Refactor:
        // Rewrite SPD and maybe Torque controller too
        // Do tests
        // Find a good tuning method

        public float Kp;
        public float Kd;

        public void ComputeKpAndKd(float frequency, float damping)
        {
            // Frequency is the speed of convergence. 
            // If damping is 1, frequency is the 1/time taken to reach ~95% of the target value. 
            // i.e. a frequency of 6 will bring you very close to your target within 1/6 seconds.

            // Damping = 1: system critically damped
            // Damping > 1: system over-damped
            // Damping < 1: system under-damped (will have oscillations)

            Kp = (6f * frequency) * (6f * frequency) * 0.25f;
            Kd = 4.5f * frequency * damping;
        }

        #region PD Control
        public Vector3 ComputeForce(Vector3 desiredPosition, Vector3 currentPosition, Vector3 desiredVelocity, Vector3 rbVelocity)
        {
            return (desiredPosition - currentPosition) * Kp + (desiredVelocity - rbVelocity) * Kd;
        }

        public Vector3 ComputeTorque(Quaternion desiredRotation, Quaternion currentRotation, Rigidbody rb)
        {
            // freaks out when freq/damp > 10 and rotation in y/z > |90| 
            Quaternion delta = desiredRotation * Quaternion.Inverse(currentRotation);
            if (delta.w < 0)
            {
                delta.x = -delta.x;
                delta.y = -delta.y;
                delta.z = -delta.z;
                delta.w = -delta.w;
            }

            delta.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);
            rotationAxis.Normalize();
            rotationAxis *= Mathf.Deg2Rad;

            Vector3 pdv = Kp * (rotationAxis * angleInDegrees) - Kd * rb.angularVelocity;
            Quaternion rotInertia2World = rb.inertiaTensorRotation * currentRotation;

            pdv = Quaternion.Inverse(rotInertia2World) * pdv;
            pdv.Scale(rb.inertiaTensor);
            pdv = rotInertia2World * pdv;

            return pdv;
        }

        public Vector3 TorqueTest(Quaternion desiredRotation, Quaternion currentRotation, Rigidbody rb)
        {
            Quaternion delta = MathUtils.ShortestRotation(desiredRotation, currentRotation);
        
            delta.ToAngleAxis(out float rotDegrees, out Vector3 rotAxis);
            rotAxis.Normalize();
        
            float rotRadians = rotDegrees * Mathf.Deg2Rad;

            return (rotAxis * (rotRadians * Kp)) - (rb.angularVelocity * Kd);
        }

        #endregion

        #region SPD Control
        public Vector3 SPDComputeForce(Vector3 currentPosition, Vector3 desiredPosition, Vector3 currentVelocity, Vector3 lastVelocity)
        {
            Vector3 acceleration = (currentVelocity - lastVelocity) / Time.fixedDeltaTime;
            return -Kp * (currentPosition + (Time.fixedDeltaTime * currentVelocity) - desiredPosition) - Kd * (currentVelocity + (Time.fixedDeltaTime * acceleration));
        }

        public Vector3 SPDComputeForce(Rigidbody rb, Vector3 desiredPosition, Vector3 lastVelocity)
        {
            return SPDComputeForce(rb.position, desiredPosition, rb.velocity, lastVelocity);
        }

        //public Vector3 SPDComputeTorque(Rigidbody rb, Quaternion desiredRotation, Vector3 lastAngularVelocity)
        //{
        //    // stupdi bad math dont use it  bad yea

        //    Vector3 angularAcceleration = (rb.angularVelocity - lastAngularVelocity) / Time.fixedDeltaTime;
        //    return -P * (rb.rotation.eulerAngles + (Time.fixedDeltaTime * rb.angularVelocity) - desiredRotation.eulerAngles) - D * (rb.angularVelocity + (Time.fixedDeltaTime * angularAcceleration));

        //}
        #endregion
    }
}