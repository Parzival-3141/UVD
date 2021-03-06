using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsHand : MonoBehaviour
{
    public Transform trackedController;
    [Min(0f)] public float frequency = 1f;
    [Min(0f)] public float damping = 1f;

    private Rigidbody rb;
    private PDController pd = new PDController();
    private Vector3 trackedLastPos;

    private Vector3 tempLastVel;
    private Vector3 tempLastAngularVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 9999f;
        pd.ComputeKpAndKd(frequency, damping);

        tempLastVel = rb.velocity;
        tempLastAngularVel = rb.angularVelocity;
    }

    private void FixedUpdate()
    {
        rb.AddForce(pd.SPDComputeForce(trackedController.position, rb, tempLastVel));

        //rb.AddForce(pdController.ComputeForce(trackedController.position, GetTrackedVelocity(), rb.velocity));
        rb.AddTorque(pd.ComputeTorque(trackedController.rotation, transform.rotation, rb));

        tempLastVel = rb.velocity;
        //tempLastAngularVel = rb.angularVelocity;
    }

    //private Vector3 GetTrackedVelocity()
    //{
    //    Vector3 trackedVelocity = (trackedController.position - trackedLastPos) / Time.deltaTime;
    //    trackedLastPos = trackedController.position;
    //    return trackedVelocity;
    //}

    //private bool IsValidNumber(float value)
    //{
    //    return !float.IsNaN(value) && !float.IsInfinity(value);
    //}

    private void OnValidate()
    {
        pd.ComputeKpAndKd(frequency, damping);

        if (TryGetComponent(out Rigidbody rBody))
            rBody.useGravity = false;
    }
}