using IntoTheSky.Managers;
using IntoTheSky.Visuals;
using IntoTheSky.Audio;
using UnityEngine;

namespace IntoTheSky.Gameplay
{
    public class Platform : MonoBehaviour
    {
        Lava lava;
        public GameObject CoinPrefab;

        // Start is called before the first frame update
        void Start()
        {
            lava = FindObjectOfType<Lava>();
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.y < lava.YPos + (lava.transform.localScale.y / 2))
            {
                Delete();
            }

            if(transform.childCount == 0)
            {
                Destroy(gameObject);
            }
        }

        public void TrySpawnCoin()
        {
            int Range = 15;
            int CoinCanSpawn = Random.Range(0, Range + 1);
            if(CoinCanSpawn == Range / 2)
            {
                Vector3 Pos = transform.position + GetComponent<BoxCollider>().center;
                Instantiate(CoinPrefab, Pos + new Vector3(0f, 1.25f, 0f), Quaternion.identity, transform);
            }
        }

        public void Delete()
        {
            if(FindObjectOfType<CameraFollow>().InRangeOfCamera(transform))
            {
                AudioManager.Instance.InteractWithSFX("Platform Erase", SoundEffectBehaviour.Play);
            }
            SpawnManager.Instance.CreateNewPlatform();
            Destroy(gameObject);
        }
    }
}