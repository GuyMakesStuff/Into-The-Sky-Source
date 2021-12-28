using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IntoTheSky.Visuals
{
    public class HIScoreMarker : MonoBehaviour
    {
        [Header("Colors")]
        public Color StartColor = Color.white;
        public Color NewHIColor = Color.green;

        [Header("General")]
        public float DecreaseSpeed;
        float ColRatio;
        Material Mat;

        // Start is called before the first frame update
        void Start()
        {
            Mat = GetComponent<MeshRenderer>().sharedMaterial;
        }

        // Update is called once per frame
        void Update()
        {
            ColRatio -= DecreaseSpeed * Time.deltaTime;
            ColRatio = Mathf.Clamp01(ColRatio);
            Mat.color = Color.Lerp(StartColor, NewHIColor, ColRatio);
        }

        public void ShowNewHIAnim()
        {
            ColRatio = 1f;
        }
    }
}