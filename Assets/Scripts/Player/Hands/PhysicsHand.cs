using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UVD.Utility;


namespace UVD.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsHand : MonoBehaviour
    {
        [Header("References")]
        public Transform trackedController;
        public PhysicsHandData handData;

        [Header("PD Control")]
        public bool enablePD = true;
        [Min(0)] public float linearKp, linearKd;
        [Min(0)] public float rotationKp, rotationKd;
        [Min(0)] public float maxForce, maxTorque;
        public ForceMode forceMode; // not sure what to use, definitely a derivative though

        private PDController linearPD   = new PDController();
        private PDController rotationPD = new PDController();
        private Rigidbody rb;
        private Vector3 lastVel;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.maxAngularVelocity = 9999f;
            lastVel = rb.velocity;

            InitPdData();

        }

        private void FixedUpdate()
        {
            if (enablePD)
            {
                // @Incomplete: Hands dont account for player movement. 

                Force3 force = new Force3(linearPD.SPDComputeForce(rb, trackedController.position, lastVel));
                Force3 cappedForce = new Force3(Vector3.ClampMagnitude(force.UnityValue, maxForce));

                //Vector3 force = linearPD.SPDComputeForce(rb, trackedController.position, lastVel);
                Vector3 torque = rotationPD.ComputeTorque(trackedController.rotation, transform.rotation, rb);

                //Vector3 cappedForce = Vector3.ClampMagnitude(force, maxForce);
                Vector3 cappedTorque = Vector3.ClampMagnitude(torque, maxTorque);


                rb.AddForce(cappedForce.UnityValue, forceMode);
                rb.AddTorque(cappedTorque);

                
                var guess = forceMode == ForceMode.Force ? (cappedForce.NewtonValue) / rb.mass : cappedForce.NewtonValue;

                Debug.Log($"Raw Force: {force.UnityValue.magnitude} " +
                    $"| Capped Force: {cappedForce.UnityValue.magnitude} " +
                    $"| Newtons: {cappedForce.NewtonValue.magnitude}" +
                    $"\nGuess Accel: {guess.magnitude} " +
                    $"| Prev Real Accel: {(rb.velocity - lastVel).magnitude}" /*+
                    $"\nRaw Torque: {torque.magnitude} | Capped Torque: {cappedTorque.magnitude}"*/);

                
                lastVel = rb.velocity;
            }
        }

        private void OnValidate()
        {
            InitPdData();
        }

        private void InitPdData()
        {
            if (handData != null)
            {
                linearKp    = handData.linearKp;
                linearKd    = handData.linearKd;
                rotationKp  = handData.rotationKp;
                rotationKd  = handData.rotationKd;
                maxForce    = handData.maxForce;
                maxTorque   = handData.maxTorque;
            }
                
            SetPDComponents(linearKp, linearKd, rotationKp, rotationKd);
            
            if (TryGetComponent(out Rigidbody rBody))
                rBody.useGravity = false;
        }

        private void SetPDComponents(float linKp,float linKd, float rotKp, float rotKd)
        {
            linearPD.Kp   = linKp;
            linearPD.Kd   = linKd;
            rotationPD.Kp = rotKp;
            rotationPD.Kd = rotKd;
        }
    }
}