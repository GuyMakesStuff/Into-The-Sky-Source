using IntoTheSky.Managers;
using IntoTheSky.Audio;
using UnityEngine;

namespace IntoTheSky.Gameplay
{
    public class Coin : MonoBehaviour
    {
        public float RotSpeed;
        public float SinRange;
        public float SinSpeed;
        public GameObject CollectFX;
        float YPos;

        // Start is called before the first frame update
        void Start()
        {
            YPos = transform.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up, RotSpeed * 360 * Time.deltaTime);
            float Anim = Mathf.Sin(Time.time * SinSpeed) * SinRange;
            transform.position = new Vector3(transform.position.x, YPos + Anim, transform.position.z);
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && Time.timeSinceLevelLoad > 1f)
            {
                AudioManager.Instance.InteractWithSFX("Coin Collect", SoundEffectBehaviour.Play);
                ProgressManager.Instance.progress.Coins++;
                Destroy(Instantiate(CollectFX, transform.position, transform.rotation), 5f);
                Destroy(gameObject);
            }
        }
    }
}