using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputTest : MonoBehaviour
{
    public SteamVR_Input_Sources handType; 
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean grabPinchAction;
    public SteamVR_Action_Boolean grabGripAction;

    private void Update()
    {
        if (GetTeleportDown())
        {
            print("Teleport " + handType);
        }

        if (GetGrabPinch())
        {
            print("GrabPinch " + handType);
        }
        
        if (GetGrabGrip())
        {
            print("GrabGrip " + handType);
        }
    }

    public bool GetTeleportDown()
    {
        return teleportAction.GetStateDown(handType);
    }

    public bool GetGrabPinch()
    {
        return grabPinchAction.GetState(handType);
    }

    public bool GetGrabGrip()
    {
        return grabGripAction.GetState(handType);
    }
}
