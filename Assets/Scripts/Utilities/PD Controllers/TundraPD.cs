using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UVD.Utility
{
    public class TundraPD : MonoBehaviour
    {
        public Rigidbody rb;
        public Transform target;

        [Min(0f)] public float Kp;
        [Min(0f)] public float Kd;

        private Vector3 targetLastPos;
        private Quaternion targetLastRot;

        private Vector3 rbLastVel;

        private void Start()
        {
            targetLastPos = target.position;
        }

        private void FixedUpdate()
        {
            rb.AddForce(AccelCorrectionForce());
            rb.AddForce(ComputePDForce());
            //rb.AddForce(SPDComputeForce(rb.position, targetLastPos, rb.velocity, rbLastVel));

            rbLastVel = rb.velocity;
            targetLastPos = target.position;
        }


        #region PD

        public Vector3 ComputePDForce()
        {
            var targetVelocity = (target.position - targetLastPos) / Time.fixedDeltaTime;
            return (targetLastPos - rb.transform.position) * Kp + (targetVelocity - rb.velocity) * Kd;
        }

        public Vector3 SPDComputeForce(Vector3 currentPosition, Vector3 desiredPosition, Vector3 currentVelocity, Vector3 lastVelocity)
        {
            Vector3 acceleration = (currentVelocity - lastVelocity) / Time.fixedDeltaTime;
            return -Kp * (currentPosition + (Time.fixedDeltaTime * currentVelocity) - desiredPosition) - Kd * (currentVelocity + (Time.fixedDeltaTime * acceleration));
        }

        public Quaternion ComputePDTorque()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Acceleration Correction
        
        public Vector3 AccelCorrectionForce()
        {
            //  Code from Alec
            //  https://discord.com/channels/631250853327536139/631271033063604234/957057920938442773

            Vector3 targetFrameVelocity = (target.position - targetLastPos) / Time.fixedDeltaTime;
            Vector3 FrameAcceleration = (targetFrameVelocity - rb.velocity) / Time.fixedDeltaTime;

            return rb.mass * FrameAcceleration;
        }

        public Quaternion AccelCorrectionTorque()
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}
