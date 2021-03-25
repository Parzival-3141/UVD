using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DebugControllers : MonoBehaviour
{
    public SteamVR_Behaviour_Skeleton handLeftSkel;
    public SteamVR_Behaviour_Skeleton handRightSkel;
    public GameObject controllerLeftModel;
    public GameObject controllerRightModel;
    public SteamVR_Action_Boolean enableControllerDebug;

    private void Update()
    {
        if (enableControllerDebug.changed)
        {
            controllerLeftModel.SetActive(enableControllerDebug.state);
            controllerRightModel.SetActive(enableControllerDebug.state);
            
            if (enableControllerDebug.state)
            {
                handLeftSkel.rangeOfMotion = EVRSkeletalMotionRange.WithController;
                handRightSkel.rangeOfMotion = EVRSkeletalMotionRange.WithController;
            }
            else
            {
                handLeftSkel.rangeOfMotion = EVRSkeletalMotionRange.WithoutController;
                handRightSkel.rangeOfMotion = EVRSkeletalMotionRange.WithoutController;
            }
        }
    }
}
