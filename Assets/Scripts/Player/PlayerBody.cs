using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    // Either an in-between for tracking and body systems (guessing what the player's body looks like, IK, etc.)
    // Or a simple Physics-Lite body.

    public InputRig inputRig;
    public new CapsuleCollider collider;

    private Vector3 playerPosInWorld;

    private void FixedUpdate()
    {   
        playerPosInWorld = Vector3.Scale(inputRig.GetPlayerHeadInTrackSpace(), new Vector3(1, 0, 1));

        collider.transform.position = new Vector3(playerPosInWorld.x, playerPosInWorld.y + collider.height / 2, playerPosInWorld.z);
    }
}
