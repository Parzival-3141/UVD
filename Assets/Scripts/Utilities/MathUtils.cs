using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
