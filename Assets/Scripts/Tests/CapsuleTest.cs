using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleTest : MonoBehaviour
{
    public Rigidbody capsule;
    public Transform target;

    private void FixedUpdate()
    {
        capsule.MovePosition(target.position);
    }
}
