using System.Collections;
using IntoTheSky.Managers;
using UnityEngine;

namespace IntoTheSky.Gameplay
{
    public class Lava : MonoBehaviour
    {
        public float RiseSpeed;
        float StartRiseSpeed;
        public float MaxRiseSpeed;
        public float RiseSpeedIncreaseRate;
        public GameObject ExplotionFX;
        Transform Cam;
        [HideInInspector]
        public float YPos;

        // Start is called before the first frame update
        void Start()
        {
            StartRiseSpeed = RiseSpeed;
            YPos = transform.position.y;
            Cam = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if(transform.position.y < Cam.position.y)
            {
                YPos += RiseSpeed * Time.deltaTime;
            }
            transform.position = new Vector3(Cam.position.x, YPos, 0f);
        }

        public void IncreaseRiseSpeed()
        {
            if (RiseSpeed < MaxRiseSpeed)
            {
                // float Gap = MaxRiseSpeed - StartRiseSpeed;
                RiseSpeed += RiseSpeedIncreaseRate;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Platform")
            {
                Destroy(Instantiate(ExplotionFX, other.transform.position + other.GetComponent<BoxCollider>().center, ExplotionFX.transform.rotation), 5f);
                other.GetComponent<Platform>().Delete();
            }
            else if(other.tag == "Player")
            {
                other.GetComponent<Player>().Kill();
            }
        }
    }
}