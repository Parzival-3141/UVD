using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRPlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform inputRig;
    public Transform HMDTransform;
    public Transform playerBody;

    [Header("Movement")]
    public SteamVR_Input_Sources movementHand = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Action_Vector2 actionMovement;
    public float moveSpeed;
    public float stickDeadzone = 0.1f;
    public float vertTrackingOffset = -0.5f;

    public Transform bodyTransform { get => bodyRB.transform; }
    public Vector3 bodyVelocity { get => bodyRB.velocity; }

    private Transform headTransform;
    private CapsuleCollider bodyCollider;
    private Rigidbody bodyRB; // @Refactor: Needs to be split up when using more complex bodyRBs. 
    private Vector2 joystick; //            Will probably need them for handling crouching and neck joint stuff.
    private Vector3 lastHMDPos;


    private void Awake()
    {
        headTransform = HMDTransform.GetComponentInChildren<Transform>(); //@Incomplete: Head Transform needs to be calculated with respect to the player's neck
        bodyRB = playerBody.GetComponent<Rigidbody>();
        bodyCollider = playerBody.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        joystick = actionMovement.GetAxis(movementHand);
        UpdateBodyCollider();
    }

    private void FixedUpdate()
    {
        RoomscaleMovement();
        JoystickMovement();
    }

    private void RoomscaleMovement()
    {
        Vector3 headDelta = HMDTransform.localPosition - lastHMDPos;
        headDelta.y = 0f;

        Vector3 bodyDelta = playerBody.TransformDirection(headDelta);
        bodyRB.MovePosition(bodyRB.position + bodyDelta);

        Vector3 newInputRigPos = (inputRig.localPosition - HMDTransform.localPosition) + lastHMDPos;
        newInputRigPos.y = vertTrackingOffset; //@Incomplete: No idea what offset should be

        inputRig.localPosition = newInputRigPos;

        lastHMDPos = HMDTransform.localPosition;
    }
    private void JoystickMovement()
    {
        Vector3 moveDir = Quaternion.AngleAxis(Angle(joystick) + HMDTransform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;
        if (joystick.magnitude > stickDeadzone)
            bodyRB.AddForce(moveDir.x * moveSpeed - bodyRB.velocity.x, 0, moveDir.z * moveSpeed - bodyRB.velocity.z, ForceMode.VelocityChange);
    }

    private void UpdateBodyCollider()
    {
        //@Incomplete: Need a way of changing hitbox when crouching. Seems to work?
        bodyCollider.height = Mathf.Clamp(HMDTransform.localPosition.y, inputRig.position.y + bodyCollider.radius * 2, 10f);
        bodyCollider.center = new Vector3(0, bodyCollider.height / 2f, 0);
    }


    public static float Angle(Vector2 v)
    {
        if (v.x < 0)
        {
            return 360 - (Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg;
        }
    }
}
