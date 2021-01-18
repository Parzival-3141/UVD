using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcForceToDesiredTransform : MonoBehaviour
{
    public Transform trackedObject;
    [Range(0f, 1f)] public float velocityDampening = 0.9f;
    [Range(0f, 1f)] public float angularVeloctiyDampening = 1f;

    private Rigidbody rb;
    //private Vector3 lastForce;
    private Vector3 lastTorque = Vector3.up;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // kinematic movement
        //rb.MovePosition(trackedController.position);
        //rb.MoveRotation(trackedController.rotation);

        // force based movement
        rb.velocity *= velocityDampening;
        rb.angularVelocity *= angularVeloctiyDampening;
        rb.AddForce(ComputeForce(trackedObject.position));
        rb.AddTorque(ComputeTorque(trackedObject.rotation));
    }


    private Vector3 ComputeForce(Vector3 desiredPosition)
    {
        float dt = Time.fixedDeltaTime;
        return rb.mass * (desiredPosition - transform.position - rb.velocity * dt) / dt;
    }
    private Vector3 ComputeTorque(Quaternion desiredRotation)
    {
        Quaternion delta = desiredRotation * Quaternion.Inverse(transform.rotation);

        // convert to angle axis 
        delta.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);
        rotationAxis.Normalize();

        // prevent flipping
        if (angleInDegrees > 180)
            angleInDegrees -= 360;

        // w == desired angular velocity
        Vector3 w = rotationAxis * angleInDegrees * Mathf.Deg2Rad / Time.fixedDeltaTime;

        if (!IsValidAngularVelocity(w))
            w = lastTorque;

        w -= rb.angularVelocity; // wDelta

        // inertia tensors & sum other shit idk man
        Vector3 wLocal = transform.InverseTransformDirection(w);
        wLocal = rb.inertiaTensorRotation * wLocal;
        wLocal.Scale(rb.inertiaTensor);

        Vector3 T1 = Quaternion.Inverse(rb.inertiaTensorRotation) * wLocal;
        Vector3 T = transform.TransformDirection(T1);
        return T;
    }

    private bool IsValidNumber(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }

    private bool IsValidAngularVelocity(Vector3 vector)
    {
        if (!IsValidNumber(vector.x))
            return false;

        lastTorque = vector;
        return true;
    }

    private void OnValidate()
    {
        if (TryGetComponent(out Rigidbody rBody))
            rBody.useGravity = false;
    }
}
