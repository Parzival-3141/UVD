using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModelOffset : MonoBehaviour
{
    public Vector3 positionOffset;
    public Quaternion rotationOffset;

    private void OnValidate()
    {
        transform.localPosition = positionOffset;
        transform.localRotation = rotationOffset;
    }
}