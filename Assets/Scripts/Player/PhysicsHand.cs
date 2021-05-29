using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsHand : MonoBehaviour
{
    [Header("References")]
    public Transform trackedController;
    public VRPlayerController playerController;
    public SteamVR_Action_Skeleton actionSkeleton;

    [Header("PD Control")]
    public bool enablePD = true;
    [Min(0f)]  public float frequency = 1f;
    [Min(0f)]  public float damping = 1f;
    
    [ReadOnly] public float kp;
    [ReadOnly] public float kd;

    private readonly PDController pd = new PDController();
    private Rigidbody rb;
    private Vector3 lastVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 9999f;
        pd.ComputeKpAndKd(frequency, damping);

        lastVel = rb.velocity;
    }

    private void FixedUpdate()
    {
        if (enablePD)
        {
            // @Incomplete: Hands dont account for player movement. 
            rb.AddForce(pd.SPDComputeForce(transform.position , trackedController.position, rb.velocity + playerController.bodyVelocity, lastVel));
            rb.AddTorque(pd.ComputeTorque(trackedController.rotation, transform.rotation, rb));
            lastVel = rb.velocity;
        }


        // @Incomplete: Possibly skip Behaviour_Skeleton component and just access values manually?
        //var t = actionSkeleton.thumbCurl;
        //var i = actionSkeleton.indexCurl;
        //var m = actionSkeleton.middleCurl;
        //var r = actionSkeleton.ringCurl;
        //var p = actionSkeleton.pinkyCurl;
    }

    private void OnValidate()
    {
        pd.ComputeKpAndKd(frequency, damping);
        kp  = pd.Kp;
        kd  = pd.Kd;

        if (TryGetComponent(out Rigidbody rBody))
            rBody.useGravity = false;
    }
}