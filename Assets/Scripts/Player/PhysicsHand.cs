using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsHand : MonoBehaviour
{
    public SteamVR_Action_Skeleton actionSkeleton;
    public Transform trackedController;
    [Min(0f)] public float frequency = 1f;
    [Min(0f)] public float damping = 1f;

    [ReadOnly] public float kp;
    [ReadOnly] public float kd;

    private Rigidbody rb;
    private PDController pd = new PDController();

    private Vector3 tempLastVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 9999f;
        pd.ComputeKpAndKd(frequency, damping);

        tempLastVel = rb.velocity;
    }

    private void FixedUpdate()
    {
        // @Incomplete: Hands dont account for player movement. 
        // Also moves the playerRB when tracker is intersecting the bodyCollider which shouldn't happen
        rb.AddForce(pd.SPDComputeForce(trackedController.position, rb, tempLastVel));
        rb.AddTorque(pd.ComputeTorque(trackedController.rotation, transform.rotation, rb));
        tempLastVel = rb.velocity;


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
        kp = (6f * frequency) * (6f * frequency) * 0.25f; // @Refactor: Stupid
        kd = 4.5f * frequency * damping;

        if (TryGetComponent(out Rigidbody rBody))
            rBody.useGravity = false;
    }
}