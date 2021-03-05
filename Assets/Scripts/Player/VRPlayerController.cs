using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRPlayerController : MonoBehaviour
{
    // Notes:
    // 

    [Header("References")]
    public GameObject inputRig;
    public GameObject head;
    //public GameObject playerHead;
    public GameObject playerBody;

    public SteamVR_Behaviour_Pose leftController;
    public SteamVR_Behaviour_Pose rightController;

    [Header("Movement")]
    public SteamVR_Input_Sources movementHand = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Action_Vector2 actionMovement;
    public float moveSpeed;
    public float stickDeadzone = 0.1f;


    //private SphereCollider headCollider;
    private CapsuleCollider bodyCollider;
    private Vector2 trackpad;
    private Rigidbody playerRB; // needs to change if using more complex playerRBs


    private void Awake()
    {
        //headCollider = playerHead.GetComponent<SphereCollider>();
        playerRB = playerBody.GetComponent<Rigidbody>(); 
        bodyCollider = playerBody.GetComponent<CapsuleCollider>();
        UpdateBodyCollider();
    }

    private void Update()
    {
        //playerHead.transform.position = headsetCam.transform.position;
        UpdateBodyCollider();
        inputRig.transform.position = playerBody.transform.position;
        trackpad = actionMovement.GetAxis(movementHand);
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = ComputeMoveDirection(head.transform);
        if(trackpad.magnitude > stickDeadzone)
            playerRB.AddForce(moveDir.x * moveSpeed - playerRB.velocity.x, 0, moveDir.z * moveSpeed - playerRB.velocity.z, ForceMode.VelocityChange);

    }

    private void UpdateBodyCollider()
    {
        bodyCollider.height = Mathf.Clamp(head.transform.localPosition.y, inputRig.transform.position.y + bodyCollider.radius * 2, 100f);
        bodyCollider.center = new Vector3(head.transform.localPosition.x, head.transform.localPosition.y / 2, head.transform.localPosition.z);
    }

    private Vector3 ComputeMoveDirection(Transform relativeTransform)
    {
        return Quaternion.AngleAxis(Angle(trackpad) + relativeTransform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    #region copyPaste go brrrrrrrrrr
    //void Update()
    //{
    //    //Set size and position of the capsule collider so it maches our head.
    //    Collider.height = Head.transform.localPosition.y;
    //    Collider.center = new Vector3(Head.transform.localPosition.x, Head.transform.localPosition.y / 2, Head.transform.localPosition.z);

    //    moveDirection = Quaternion.AngleAxis(Angle(trackpad) + AxisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;//get the angle of the touch and correct it for the rotation of the controller
    //    updateInput();
    //    if (GetComponent<Rigidbody>().velocity.magnitude < speed && trackpad.magnitude > Deadzone)
    //    {//make sure the touch isn't in the deadzone and we aren't going to fast.
    //        GetComponent < Rigidbody().AddForce(moveDirection * 30);
    //    }
    //}
    //public static float Angle(Vector2 p_vector2)
    //{
    //    if (p_vector2.x < 0)
    //    {
    //        return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
    //    }
    //    else
    //    {
    //        return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
    //    }
    //}
    //private void updateInput()
    //{
    //    trackpad = SteamVR_Actions._default.MovementAxis.GetAxis(Hand);
    //}
    #endregion
}
