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

    public float frequency;
    public float damping;

    //private ConfigurableJoint cJoint;
    private Rigidbody sphereRB;
    private PDController pd = new PDController();

    private Vector3 spinDir;
    private Vector3 horzVelocity;
    private Vector3 headTrackVel;
    private Vector3 headTrackLastPos;
    private float sphereRadius;

    private void Awake()
    {   
        //cJoint = GetComponent<ConfigurableJoint>();
        sphereRB = GetComponent<Rigidbody>();
        sphereRB.maxAngularVelocity = 9999f;
        sphereRadius = GetComponent<SphereCollider>().radius;
        pd.ComputeKpAndKd(frequency, damping);
    }

    private void OnValidate() => pd.ComputeKpAndKd(frequency, damping);

    private void Update()
    {
        horzVelocity = new Vector3(sphereRB.velocity.x, 0, sphereRB.velocity.z);
        headTrackVel = headTracker.position - headTrackLastPos;
        headTrackLastPos = headTracker.position;

        Vector3 trackingMoveDir = (new Vector3(headTracker.position.x, 0, headTracker.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        Vector3 controllerMoveDir = (Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward).normalized;

        spinDir = Vector3.Cross(/*-controllerMoveDir */-trackingMoveDir, Vector3.up); // perpendicular to movement plane
        Debug.DrawRay(transform.position, /*controllerMoveDir + */trackingMoveDir, Color.blue);
    }

    private void FixedUpdate()
    {
        // Match tracking
        var horzTrackPos = new Vector3(headTracker.position.x, 0, headTracker.position.z);
        var horzTransformPos = new Vector3(transform.position.x, 0, transform.position.z);

        Vector3 pdForce = pd.ComputeForce(horzTrackPos, horzTransformPos, headTrackVel, horzVelocity);
        float pdTorque = pdForce.magnitude * sphereRadius * Mathf.Sin(90);

        // Analog controls
        float spinSpeed = (maxSpeed - horzVelocity.magnitude) * acceleration + drag;
        sphereRB.AddTorque(spinDir * (/*spinSpeed + */pdTorque));

        // Horizontal Drag
        if (horzVelocity.magnitude > 0)
            sphereRB.AddTorque(Vector3.Cross(horzVelocity, Vector3.up) * drag);

        Debug.DrawRay(transform.position, spinDir * (/*spinSpeed + */pdTorque), Color.green);
        Debug.DrawRay(transform.position, -spinDir * drag, Color.red);
    }
}