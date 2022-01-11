using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UVD
{
    public class InputRig : Rig
    {
        [Header("References")]
    #pragma warning disable CS0649
        public Transform vrRoot;
        public Transform HMD;
        public Transform head;
        public Transform controllerLeft, controllerRight;
    #pragma warning restore CS0649

        [Header("Debug")]
        public bool drawDebug = false;
        public GameObject debugObjects, standVis;


        #region Helper Methods
   
        public void MoveTrackSpaceInLocal(Vector3 trackSpacePos)
        {
            vrRoot.position += trackSpacePos;
        }


        public Vector3 GetHMDInTrackSpace()
        {
            return HMD.localPosition;
        }

        public Vector3 GetPlayerHeadInTrackSpace()
        {
            return vrRoot.InverseTransformPoint(head.position);
        }

        public Vector3 GetControllerInWorldSpace(bool left)
        {
            return left ? controllerLeft.position : controllerRight.position;
        }

        public Vector3 GetPlayerStandPosInTrackSpace()
        {
            return Vector3.Scale(GetPlayerHeadInTrackSpace(), new Vector3(1, 0, 1));
        }

        public Vector3 GetPlayerStandPosInWorldSpace()
        {
            return vrRoot.TransformPoint(GetPlayerStandPosInTrackSpace());
        }
        #endregion

        private void Update()
        {
            if (debugObjects.activeInHierarchy != drawDebug)
                debugObjects.SetActive(drawDebug);

            if (drawDebug)
            {
                standVis.transform.position = GetPlayerStandPosInWorldSpace();
            }
        }
    }
}
