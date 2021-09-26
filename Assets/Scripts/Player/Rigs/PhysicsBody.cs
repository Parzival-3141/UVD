using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PhysicsBody : MonoBehaviour
{
    // @Incomplete:
    
    // Controls the player's physics body.
    // Handle matching and applying TrackingSpace movement to the body, moving the body based on player input, 
    // and applying external forces to the body.
    
    // 

    public InputRig inputRig;

    [Header("Body References")]
    public Rigidbody head;
    public Rigidbody bodyCore;
    public Rigidbody knees;
    public Rigidbody locoSphere;

    [Header("Movement")]
    public float angularBreakDrag = 200f;
    public float minAngularDrag = 5f;

    private Vector3 lastPlayerStandPos = Vector3.zero;
    private Vector3 lastHMDPos = Vector3.zero;
    //private Vector3 lastBodyStandPos = Vector3.zero;

    private void Start()
    {
        // Move everything to a stable position so it wont explode on the first update

        //Vector3 hmdPos = inputRig.GetHMDInTrackSpace();
        //Vector3 hmdDelta = hmdPos - lastHMDPos; hmdDelta.y = 0f; // flat hmd delta in track space
        //Debug.Log("hmdDelta: " + hmdDelta);

        //TeleportBodyPart(head,       hmdDelta);
        //TeleportBodyPart(bodyCore,   hmdDelta);
        //TeleportBodyPart(knees,      hmdDelta);
        //TeleportBodyPart(locoSphere, hmdDelta);

        //ResetVelocity(head);
        //ResetVelocity(bodyCore);
        //ResetVelocity(knees);
        //ResetVelocity(locoSphere);

        //inputRig.vrRoot.position = GetBodyStandPos() + new Vector3(-hmdPos.x, 0f, -hmdPos.z); // move vrRoot to body stand - hmd offset, placing the player on the body

        //lastHMDPos = hmdPos;
    }

    private void FixedUpdate()
    {
        HandleTrackSpaceMovement(locoSphere);

        // Fake Movement
        var standPos = inputRig.GetPlayerStandPosInTrackSpace();

        if (!SteamVR_Actions.default_BClick[SteamVR_Input_Sources.LeftHand].state) // Skating
        {
            var delta = standPos - lastPlayerStandPos;
            bodyCore.AddForce(delta / Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        lastPlayerStandPos = standPos;

        // Breaking
        if (SteamVR_Actions.default_AClick[SteamVR_Input_Sources.LeftHand].state) 
            locoSphere.angularDrag = angularBreakDrag;
        else
            locoSphere.angularDrag = minAngularDrag;

        #region trackspace tests
        //// get hmd delta
        //Vector3 hmdPos = inputRig.GetHMDInTrackSpace();
        //Vector3 hmdDelta = hmdPos - lastHMDPos;
        //hmdDelta.y = 0f;

        //// move rig back
        //inputRig.vrRoot.position -= hmdDelta;

        //// ASK IN PHYSDEV SERVER ABOUT THIS, SOME REAL BULLSHIT RIGHT HERE (needs to move over to green vector without locking in place there)
        //bodyCore.MovePosition(bodyCore.position + hmdDelta);

        //lastHMDPos = hmdPos;


        //Debug.DrawLine(Vector3.zero, hmdDelta);
        //Debug.DrawLine(Vector3.zero, bodyCore.position + hmdDelta,Color.blue);
        //Debug.DrawLine(inputRig.GetPlayerStandPosInWorldSpace(), inputRig.GetPlayerStandPosInWorldSpace() + inputRig.GetPlayerStandPosInTrackSpace(), Color.green);

        //// move body to player pos
        //var bodyRelToStandPos = bodyCore.position - GetBodyStandPos();
        //bodyCore.MovePosition(inputRig.GetPlayerStandPosInTrackSpace() + inputRig.GetPlayerStandPosInWorldSpace() + bodyRelToStandPos);



        // Catch root up to bodyCore
        //inputRig.vrRoot.position += GetBodyStandPos() - inputRig.GetPlayerStandPosInWorldSpace();

        //CancelOutTrackSpaceMovement();

        //var delta = CancelOutTrackSpaceMovement();

        //if (SteamVR_Actions.default_BClick[SteamVR_Input_Sources.RightHand].state)
        //    bodyCore.MovePosition(bodyCore.position + delta);
        //else
        //    bodyCore.AddForce(delta / Time.deltaTime, ForceMode.VelocityChange);




        //if (SteamVR_Actions.default_AClick[SteamVR_Input_Sources.LeftHand].state) // Unlocked the clutch
        //{
        //    // only move relative to tracking space center, and don't interfere with existing velocity

        //    bodyCore.AddForce(delta, ForceMode.VelocityChange);
        //}
        //else // Skating 
        //{
        //    bodyCore.AddForce(delta / Time.fixedDeltaTime, ForceMode.VelocityChange);
        //}
        #endregion
    }


    public Vector3 GetBodyStandPos()
    {
        if (locoSphere.gameObject.activeInHierarchy)
            return locoSphere.position + Vector3.down * 0.2f; // @Refactor: replace with locoSphere.radius
        else
            return bodyCore.position + Vector3.down;
    }


    private void HandleTrackSpaceMovement(Rigidbody toMove)
    {
        Vector3 hmdPos = inputRig.GetHMDInTrackSpace();
        Vector3 hmdDelta = hmdPos - lastHMDPos; hmdDelta.y = 0f; // flat hmd delta in track space
        Debug.Log("hmdDelta: " + hmdDelta);

        hmdDelta = toMove.transform.TransformDirection(hmdDelta); // delta is equivalent to toMove localSpace, so transform into worldspace
        toMove.MovePosition(toMove.position + hmdDelta);

        inputRig.vrRoot.position = GetBodyStandPos() + new Vector3(-hmdPos.x, 0f, -hmdPos.z); // move vrRoot to body stand - hmd offset, placing the player on the body

        lastHMDPos = hmdPos;
    }


    private void TeleportBodyPart(Rigidbody toMove, Vector3 delta)
    {
        if(!toMove.gameObject.activeInHierarchy) { return; }

        delta = toMove.transform.TransformDirection(delta); // delta is equivalent to toMove localSpace, so transform into worldspace
        toMove.MovePosition(toMove.position + delta);
    }

    private void ResetVelocity(Rigidbody rb)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void CoolMovement()
    {
        var playerPos = inputRig.GetPlayerStandPosInTrackSpace();

        var delta = playerPos - lastPlayerStandPos;

        inputRig.MoveTrackSpaceInLocal(-delta);
        bodyCore.AddForce(delta / Time.fixedDeltaTime, ForceMode.VelocityChange);

        //bodyCore.MovePosition(bodyCore.position + delta);

        lastPlayerStandPos = playerPos;
    }
}
