using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocosphereTest : MonoBehaviour
{
    // Need to account for playspace movement
    // Get horizontal delta
    // Cancel out playspace movement
    // Generate torque needed to move player to uncorrected playspace pos


    public Transform headTracker;
    public float maxSpeed;
    public float acceleration;
    public float drag;

    //private ConfigurableJoint cJoint;
    private Rigidbody sphereRB;
    private Vector3 moveDir;
    private Vector3 spinDir;

    private void Start()
    {
        //cJoint = GetComponent<ConfigurableJoint>();
        sphereRB = GetComponent<Rigidbody>();
        sphereRB.maxAngularVelocity = 9999f;
    }

    private void Update()
    {
        moveDir = (Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward).normalized;
        spinDir = Vector3.Cross(-moveDir, Vector3.up); // perpendicular to movement plane
        Debug.DrawRay(transform.position, moveDir, Color.blue);
    }

    private void FixedUpdate()
    {
        Vector3 horzVelocity = new Vector3(sphereRB.velocity.x, 0, sphereRB.velocity.z);

        float spinSpeed = (maxSpeed - horzVelocity.magnitude) * acceleration + drag;
        sphereRB.AddTorque(spinDir * spinSpeed);
        Debug.DrawRay(transform.position, spinDir * spinSpeed, Color.green);

        // Horizontal Drag
        if (horzVelocity.magnitude > 0)
            sphereRB.AddTorque(Vector3.Cross(horzVelocity, Vector3.up) * drag);

        Debug.DrawRay(transform.position, -spinDir * drag, Color.red);
    }
}