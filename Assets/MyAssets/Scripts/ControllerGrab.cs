using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerGrab : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;

    private GameObject collidingObject;
    private GameObject objectInHand;
    private VelocityEstimator velocityEstimator;

    private void Update()
    {
        if (grabAction.GetLastStateDown(handType) && collidingObject)
        {
            GrabObject();
        }

        if (grabAction.GetLastStateUp(handType) && objectInHand)
        {
            ReleaseObject();
        }
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();

        velocityEstimator = objectInHand.AddComponent<VelocityEstimator>();
        velocityEstimator.BeginEstimatingVelocity();
    }

    private void ReleaseObject()
    {
        FixedJoint fj = GetComponent<FixedJoint>();
        if (fj)
        {
            fj.connectedBody = null;
            Destroy(fj);

            Rigidbody objInHandRB = objectInHand.GetComponent<Rigidbody>();

            GetReleaseVelocities(out Vector3 velocity, out Vector3 angularVelocity);
            objInHandRB.velocity = velocity;
            objInHandRB.angularVelocity = angularVelocity;

            Destroy(velocityEstimator);
        }

        objectInHand = null;
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fj = gameObject.AddComponent<FixedJoint>();
        fj.breakForce = 20000;
        fj.breakTorque = 20000;
        return fj;
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
    }

    public void GetReleaseVelocities(out Vector3 velocity, out Vector3 angularVelocity)
    {
        velocityEstimator.FinishEstimatingVelocity();
        velocity = velocityEstimator.GetVelocityEstimate();
        angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
    }


    public void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
            SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }
}