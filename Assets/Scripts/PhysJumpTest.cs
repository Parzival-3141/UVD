using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysJumpTest : MonoBehaviour
{
    public Rigidbody rb;
    public int force;

    private void FixedUpdate()
    {
        if (Input.GetButton("Jump"))
        {
            rb.AddForce(Vector3.down * force);

            // accelerate down by pulling legs up & crouching, lowering center of mass

            // reach lowest crouching position, where center of mass is the lowest

            // accelerate back up by pushing legs into the ground, resulting force should both return to standing position and reverse that downwards velocity

            // return to standing position, ideally in the air
        }
    }
}
