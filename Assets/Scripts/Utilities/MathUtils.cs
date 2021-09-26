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

        /// <summary>
        /// Unity uses Newtons for Forces, but scales them by <see cref="Time.deltaTime"/>
        /// when applying them in a frame (unless using <see cref="ForceMode.Impulse"/>).
        /// <para>This returns the Force actually applied in a frame. </para>
        /// </summary>
        /// <returns>
        /// <paramref name="force"/> scaled by <see cref="Time.deltaTime"/> or <see cref="Time.fixedDeltaTime"/>.
        /// </returns>
        public static Vector3 GetScaledForce(Vector3 force, bool useFixedDeltaTime = false)
        {
            if (useFixedDeltaTime)
                return force * Time.fixedDeltaTime;
            else
                return force * Time.deltaTime;
        }

        #endregion
    }
}
