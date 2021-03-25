using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDController
{
    public float Kp { get; private set; }
    public float Kd { get; private set; }
    
    public float Kpg { get; private set; }
    public float Kdg { get; private set; }

    //private float Kp;
    //private float Kd;

    //private float Kpg;
    //private float Kdg;

    public void ComputeKpAndKd(float frequency, float damping)
    {
        // Frequency is the speed of convergence. 
        // If damping is 1, frequency is the 1/time taken to reach ~95% of the target value. 
        // i.e. a frequency of 6 will bring you very close to your target within 1/6 seconds.

        // Damping = 1: system critically damped
        // Damping > 1: system over-damped
        // Damping < 1: system under-damped (will have oscillations)

        Kp = (6f * frequency) * (6f * frequency) * 0.25f;
        Kd = 4.5f * frequency * damping;

        ComputeKpgAndKdg();
    }

    public void ComputeKpgAndKdg()
    {
        float dt = Time.fixedDeltaTime;
        float g = 1f / (1f + Kd * dt + Kp * dt * dt);
        Kpg = Kp * g;
        Kdg = (Kd + Kp * dt) * g;
    }

    #region PD Control
    public Vector3 ComputeForce(Vector3 desiredPosition, Vector3 currentPosition, Vector3 desiredVelocity, Vector3 rbVelocity)
    {
        return (desiredPosition - currentPosition) * Kpg + (desiredVelocity - rbVelocity) * Kdg;
    }

    public Vector3 ComputeTorque(Quaternion desiredRotation, Quaternion currentRotation, Rigidbody rb)
    {
        Quaternion delta = desiredRotation * Quaternion.Inverse(currentRotation);
        if (delta.w < 0)
        {
            delta.x = -delta.x;
            delta.y = -delta.y;
            delta.z = -delta.z;
            delta.w = -delta.w;
        }

        delta.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);
        rotationAxis.Normalize();
        rotationAxis *= Mathf.Deg2Rad;

        Vector3 pdv = Kp * (rotationAxis * angleInDegrees) - Kd * rb.angularVelocity;
        Quaternion rotInertia2World = rb.inertiaTensorRotation * currentRotation;

        pdv = Quaternion.Inverse(rotInertia2World) * pdv;
        pdv.Scale(rb.inertiaTensor);
        pdv = rotInertia2World * pdv;

        return pdv;
    }
    #endregion

    #region SPD Control
    public Vector3 SPDComputeForce(Vector3 currentPosition, Vector3 desiredPosition, Vector3 currentVelocity, Vector3 lastVelocity)
    {
        Vector3 acceleration = (currentVelocity - lastVelocity) / Time.fixedDeltaTime;
        return -Kpg * (currentPosition + (Time.fixedDeltaTime * currentVelocity) - desiredPosition) - Kdg * (currentVelocity + (Time.fixedDeltaTime * acceleration));
    }

    public Vector3 SPDComputeForce(Vector3 desiredPosition, Rigidbody rb, Vector3 lastVelocity)
    {
        Vector3 acceleration = (rb.velocity - lastVelocity) / Time.fixedDeltaTime;
        return -Kpg * (rb.position + (Time.fixedDeltaTime * rb.velocity) - desiredPosition) - Kdg * (rb.velocity + (Time.fixedDeltaTime * acceleration));
    }


    //public Vector3 SPDComputeTorque(Rigidbody rb, Quaternion desiredRotation, Vector3 lastAngularVelocity)
    //{
    //    // stupdi bad math dont use it  bad yea

    //    Vector3 angularAcceleration = (rb.angularVelocity - lastAngularVelocity) / Time.fixedDeltaTime;
    //    return -kpg * (rb.rotation.eulerAngles + (Time.fixedDeltaTime * rb.angularVelocity) - desiredRotation.eulerAngles) - kdg * (rb.angularVelocity + (Time.fixedDeltaTime * angularAcceleration));

    //}
    #endregion
}