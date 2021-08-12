using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UVD.Player
{
    public class Grounding : MonoBehaviour
    {
        // @Incomplete:
        // If I want to do a floating capsule thing then
        // I mostly need to do it here.

        public bool IsGrounded { get => grounded; }
        public Vector3? Normal
        {
            get
            {
                if (IsGrounded)
                {
                    return normal;
                }
                return null;
            }
        }


        public LayerMask layerMask;
        public Transform castOrigin;
        public Vector3 castDir;
        public float castLength, castWidth;

        private bool grounded;
        private Vector3 normal;

        private void FixedUpdate()
        {
            grounded = Physics.SphereCast(castOrigin.position, castWidth / 2, castDir, out RaycastHit hitInfo, castLength, layerMask);
            if (grounded)
            {
                normal = hitInfo.normal;
            }
        }

#if UNITY_EDITOR
        Mesh cylinder;
        private void OnDrawGizmosSelected()
        {
            if(!this.enabled || castOrigin == null) { return; }

            if (cylinder == null)
                cylinder = Resources.GetBuiltinResource<Mesh>("Cylinder.fbx");

            Vector3 pos   = castOrigin.position - new Vector3(0, castLength / 2); 
            Vector3 scale = new Vector3(castWidth, castLength, castWidth) / 2;

            Gizmos.DrawWireSphere(castOrigin.position, castWidth / 2);
            Gizmos.DrawWireMesh(cylinder, pos, Quaternion.identity, scale);
            Gizmos.DrawWireSphere(castOrigin.position - new Vector3(0, castLength), castWidth / 2);
        }
#endif

    }
}