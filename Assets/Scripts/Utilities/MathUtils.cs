using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UVD.Utility
{
    public static class MathUtils
    {
        #region Quaternions
        public static Quaternion Mutliply(Quaternion quat, float scalar)
        {
            return new Quaternion
                (
                    quat.x * scalar, 
                    quat.y * scalar, 
                    quat.z * scalar, 
                    quat.w * scalar
                );
        }

        public static Quaternion ShortestRotation(Quaternion q1, Quaternion q2)
        {
            if (Quaternion.Dot(q1, q2) < 0)
                return q1 * Quaternion.Inverse(Mutliply(q2, -1));
            else
                return q1 * Quaternion.Inverse(q2);
        }

        #endregion
    }
    

    /// <summary>
    /// Converts Unity Force units to Newtons
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Force<T>
    {
        public virtual T NewtonValue { get; }
        public T UnityValue { get; set; }

        public Force(T value) => UnityValue = value;
    }

    public class Force3 : Force<Vector3>
    {
        public Force3(Vector3 value) : base(value) { }

        // Uses normal deltaTime if called it from Update, still returns proper value if called from FixedUpdate
        public override Vector3 NewtonValue { get => UnityValue * Time.deltaTime; }
    }
}
