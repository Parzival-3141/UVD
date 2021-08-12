using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PhysicsHandData")]
public class PhysicsHandData : ScriptableObject
{
    [Header("PD Data")]
    [Min(0)] public float linearKp, linearKd;
    [Min(0)] public float rotationKp, rotationKd;
    [Min(0)] public float maxForce, maxTorque;
}
