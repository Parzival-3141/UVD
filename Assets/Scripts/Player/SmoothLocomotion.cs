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
        public Transform movementTransform;
        public float maxSpeed = 1.5f;

        private Grounding grounding;
        private SteamVR_Action_Vector2_Source analogStickPos;

        private void Start()
        {
            grounding = GetComponent<Grounding>();
            analogStickPos = SteamVR_Actions.default_AnalogStickPosition[SteamVR_Input_Sources.LeftHand];
        }

        private void Update()
        {
            if(analogStickPos.axis != Vector2.zero)
            {
                MoveTransform(movementTransform);
            }
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

        private void MoveTransform(Transform toMove)
        {
            toMove.position = Vector3.MoveTowards
                (
                    toMove.position,
                    toMove.position + GetMoveVector(),
                    analogStickPos.axis.magnitude * maxSpeed * Time.deltaTime
                );
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
