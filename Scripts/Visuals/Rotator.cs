using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IntoTheSky.Visuals
{
    public class Rotator : MonoBehaviour
    {
        public float RotSpeed;
        public Vector3 Eulers;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Eulers, RotSpeed * 360 * Time.deltaTime);
        }
    }
}