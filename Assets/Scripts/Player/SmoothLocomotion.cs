using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


namespace UVD.Player
{
    public class SmoothLocomotion : MonoBehaviour
    {
        // @Incomplete:
        // Need to implement a body system before I can apply movement forces.
        // Needs more work anyway.

        public Transform headTransform;
        public Rigidbody rb;

        [Header("Movement Settings")]
        public float maxSpeed = 5f;
        public float moveAcceleration = 10f;
        public float dragAcceleration = 7f;
        public float turnAccelerationScale = 2f;

        private Grounding grounding;
        private SteamVR_Action_Vector2_Source analogStickPos;


        private void Start()
        {
            grounding = GetComponent<Grounding>();
            analogStickPos = SteamVR_Actions.default_AnalogStickPosition[SteamVR_Input_Sources.RightHand];
        }

        private void FixedUpdate()
        {
            Move();
        }

        private Vector3 GetMoveVector()
        {
            Vector2 joy2 = analogStickPos.axis;
            Vector3 controllerOutput = new Vector3(joy2.x, 0, joy2.y); // analogStick in 3D space

            Vector3 facingDirection = headTransform.forward; // Could be hmd.forward, controller.forward, etc.
            facingDirection.Scale(new Vector3(1, 0, 1)); // Flatten the vector

            Vector3 joystick = Quaternion.LookRotation(facingDirection).normalized * controllerOutput; // apply Direction's rotation to controllerOutput

            Vector3 projectedDir = Vector3.ProjectOnPlane(joystick, Vector3.up); // Project onto ground normal; @Refactor: replace normal with groundNormal
            return projectedDir.normalized * controllerOutput.magnitude;
        }

        private void Move()
        {
            var moveDir = GetMoveVector();
            var dotScale = Vector3.Dot(rb.velocity.normalized, moveDir) < 0 ? turnAccelerationScale : 1;

            var horzVelocity = Vector3.Scale(rb.velocity, new Vector3(1, 0, 1));

            Vector3 movementAccel = moveDir * (moveAcceleration + dragAcceleration) * dotScale;
            Vector3 dragAccel = -horzVelocity.normalized * dragAcceleration;

            rb.AddForce(movementAccel + dragAccel);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }


        private void OnDrawGizmos()
        {
            if(Application.isPlaying)
            {
                Vector3 headPlusMove = headTransform.position + GetMoveVector();

                Gizmos.DrawLine(headTransform.position, headPlusMove);
                Gizmos.DrawWireSphere(headPlusMove, 0.1f);
            }
        }

    }
}
