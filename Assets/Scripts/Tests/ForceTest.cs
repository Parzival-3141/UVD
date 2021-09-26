using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UVD.Utility;

public class ForceTest : MonoBehaviour
{
    public bool doTest = false;
    public float acceleration = -9.81f;
    public float mass = 1f;
    [Space]
    public float distance;
    public float time;
    [Space]
    [ReadOnly] public float timer = 0;

    public Rigidbody rb;
    public ForceMode forceMode;

    private Vector3 prevVel = Vector3.zero;
    private void Start()
    {
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        if (doTest)
        {
            Vector3 accel = new Vector3(0, acceleration);
            Vector3 force = mass * accel;

            rb.mass = mass;


            if(forceMode == ForceMode.VelocityChange)
            {
                force = Vector3.right * (distance / time) * Time.fixedDeltaTime;
            }

            rb.AddForce(force, forceMode);
            var velAfter = rb.velocity;

            Debug.Log($"Unscaled Newtons: {force} " +
                $"| Scaled Newtons: {MathUtils.GetScaledForce(force)} " +
                $"| Vel Before: {prevVel} " +
                $"| Vel After: {velAfter}");

            timer += Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.position = transform.position;
            timer = 0f;
        }

        prevVel = rb.velocity;
    }
}
