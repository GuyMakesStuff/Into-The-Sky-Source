using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IntoTheSky.Visuals
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Target;
        Vector3 PrevPos;
        public Vector3 Offset;
        public bool Smooth;
        public float SmoothTime;
        Vector3 Velocity;
        [Space]
        public float Range;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(Target != null)
            {
                PrevPos = Target.position;
            }
            Vector3 TargetPos = PrevPos + Offset;
            Vector3 SmoothedPos = Vector3.SmoothDamp(transform.position, TargetPos, ref Velocity, SmoothTime);
            transform.position = (Smooth) ? SmoothedPos : TargetPos;
        }

        public bool InRangeOfCamera(Transform OBJ)
        {
            return Vector3.Distance(transform.position, OBJ.position) < Range;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Range);
        }
    }
}