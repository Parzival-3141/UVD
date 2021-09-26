//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Action_Pose p_default_Pose;
        
        private static SteamVR_Action_Skeleton p_default_SkeletonLeftHand;
        
        private static SteamVR_Action_Skeleton p_default_SkeletonRightHand;
        
        private static SteamVR_Action_Boolean p_default_HeadsetOnHead;
        
        private static SteamVR_Action_Boolean p_default_SnapTurnLeft;
        
        private static SteamVR_Action_Boolean p_default_SnapTurnRight;
        
        private static SteamVR_Action_Vector2 p_default_TrackpadPosition;
        
        private static SteamVR_Action_Vector2 p_default_AnalogStickPosition;
        
        private static SteamVR_Action_Single p_default_GripForce;
        
        private static SteamVR_Action_Boolean p_default_AClick;
        
        private static SteamVR_Action_Boolean p_default_BClick;
        
        private static SteamVR_Action_Vibration p_default_Haptic;
        
        public static SteamVR_Action_Pose default_Pose
        {
            get
            {
                return SteamVR_Actions.p_default_Pose.GetCopy<SteamVR_Action_Pose>();
            }
        }
        
        public static SteamVR_Action_Skeleton default_SkeletonLeftHand
        {
            get
            {
                return SteamVR_Actions.p_default_SkeletonLeftHand.GetCopy<SteamVR_Action_Skeleton>();
            }
        }
        
        public static SteamVR_Action_Skeleton default_SkeletonRightHand
        {
            get
            {
                return SteamVR_Actions.p_default_SkeletonRightHand.GetCopy<SteamVR_Action_Skeleton>();
            }
        }
        
        public static SteamVR_Action_Boolean default_HeadsetOnHead
        {
            get
            {
                return SteamVR_Actions.p_default_HeadsetOnHead.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_SnapTurnLeft
        {
            get
            {
                return SteamVR_Actions.p_default_SnapTurnLeft.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_SnapTurnRight
        {
            get
            {
                return SteamVR_Actions.p_default_SnapTurnRight.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Vector2 default_TrackpadPosition
        {
            get
            {
                return SteamVR_Actions.p_default_TrackpadPosition.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Vector2 default_AnalogStickPosition
        {
            get
            {
                return SteamVR_Actions.p_default_AnalogStickPosition.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Single default_GripForce
        {
            get
            {
                return SteamVR_Actions.p_default_GripForce.GetCopy<SteamVR_Action_Single>();
            }
        }
        
        public static SteamVR_Action_Boolean default_AClick
        {
            get
            {
                return SteamVR_Actions.p_default_AClick.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_BClick
        {
            get
            {
                return SteamVR_Actions.p_default_BClick.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Vibration default_Haptic
        {
            get
            {
                return SteamVR_Actions.p_default_Haptic.GetCopy<SteamVR_Action_Vibration>();
            }
        }
        
        private static void InitializeActionArrays()
        {
            Valve.VR.SteamVR_Input.actions = new Valve.VR.SteamVR_Action[] {
                    SteamVR_Actions.default_Pose,
                    SteamVR_Actions.default_SkeletonLeftHand,
                    SteamVR_Actions.default_SkeletonRightHand,
                    SteamVR_Actions.default_HeadsetOnHead,
                    SteamVR_Actions.default_SnapTurnLeft,
                    SteamVR_Actions.default_SnapTurnRight,
                    SteamVR_Actions.default_TrackpadPosition,
                    SteamVR_Actions.default_AnalogStickPosition,
                    SteamVR_Actions.default_GripForce,
                    SteamVR_Actions.default_AClick,
                    SteamVR_Actions.default_BClick,
                    SteamVR_Actions.default_Haptic};
            Valve.VR.SteamVR_Input.actionsIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.default_Pose,
                    SteamVR_Actions.default_SkeletonLeftHand,
                    SteamVR_Actions.default_SkeletonRightHand,
                    SteamVR_Actions.default_HeadsetOnHead,
                    SteamVR_Actions.default_SnapTurnLeft,
                    SteamVR_Actions.default_SnapTurnRight,
                    SteamVR_Actions.default_TrackpadPosition,
                    SteamVR_Actions.default_AnalogStickPosition,
                    SteamVR_Actions.default_GripForce,
                    SteamVR_Actions.default_AClick,
                    SteamVR_Actions.default_BClick};
            Valve.VR.SteamVR_Input.actionsOut = new Valve.VR.ISteamVR_Action_Out[] {
                    SteamVR_Actions.default_Haptic};
            Valve.VR.SteamVR_Input.actionsVibration = new Valve.VR.SteamVR_Action_Vibration[] {
                    SteamVR_Actions.default_Haptic};
            Valve.VR.SteamVR_Input.actionsPose = new Valve.VR.SteamVR_Action_Pose[] {
                    SteamVR_Actions.default_Pose};
            Valve.VR.SteamVR_Input.actionsBoolean = new Valve.VR.SteamVR_Action_Boolean[] {
                    SteamVR_Actions.default_HeadsetOnHead,
                    SteamVR_Actions.default_SnapTurnLeft,
                    SteamVR_Actions.default_SnapTurnRight,
                    SteamVR_Actions.default_AClick,
                    SteamVR_Actions.default_BClick};
            Valve.VR.SteamVR_Input.actionsSingle = new Valve.VR.SteamVR_Action_Single[] {
                    SteamVR_Actions.default_GripForce};
            Valve.VR.SteamVR_Input.actionsVector2 = new Valve.VR.SteamVR_Action_Vector2[] {
                    SteamVR_Actions.default_TrackpadPosition,
                    SteamVR_Actions.default_AnalogStickPosition};
            Valve.VR.SteamVR_Input.actionsVector3 = new Valve.VR.SteamVR_Action_Vector3[0];
            Valve.VR.SteamVR_Input.actionsSkeleton = new Valve.VR.SteamVR_Action_Skeleton[] {
                    SteamVR_Actions.default_SkeletonLeftHand,
                    SteamVR_Actions.default_SkeletonRightHand};
            Valve.VR.SteamVR_Input.actionsNonPoseNonSkeletonIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.default_HeadsetOnHead,
                    SteamVR_Actions.default_SnapTurnLeft,
                    SteamVR_Actions.default_SnapTurnRight,
                    SteamVR_Actions.default_TrackpadPosition,
                    SteamVR_Actions.default_AnalogStickPosition,
                    SteamVR_Actions.default_GripForce,
                    SteamVR_Actions.default_AClick,
                    SteamVR_Actions.default_BClick};
        }
        
        private static void PreInitActions()
        {
            SteamVR_Actions.p_default_Pose = ((SteamVR_Action_Pose)(SteamVR_Action.Create<SteamVR_Action_Pose>("/actions/default/in/Pose")));
            SteamVR_Actions.p_default_SkeletonLeftHand = ((SteamVR_Action_Skeleton)(SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/default/in/SkeletonLeftHand")));
            SteamVR_Actions.p_default_SkeletonRightHand = ((SteamVR_Action_Skeleton)(SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/default/in/SkeletonRightHand")));
            SteamVR_Actions.p_default_HeadsetOnHead = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/HeadsetOnHead")));
            SteamVR_Actions.p_default_SnapTurnLeft = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/SnapTurnLeft")));
            SteamVR_Actions.p_default_SnapTurnRight = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/SnapTurnRight")));
            SteamVR_Actions.p_default_TrackpadPosition = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/default/in/TrackpadPosition")));
            SteamVR_Actions.p_default_AnalogStickPosition = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/default/in/AnalogStickPosition")));
            SteamVR_Actions.p_default_GripForce = ((SteamVR_Action_Single)(SteamVR_Action.Create<SteamVR_Action_Single>("/actions/default/in/GripForce")));
            SteamVR_Actions.p_default_AClick = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/AClick")));
            SteamVR_Actions.p_default_BClick = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/BClick")));
            SteamVR_Actions.p_default_Haptic = ((SteamVR_Action_Vibration)(SteamVR_Action.Create<SteamVR_Action_Vibration>("/actions/default/out/Haptic")));
        }
    }
}
