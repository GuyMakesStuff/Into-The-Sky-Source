using System.Collections;
using IntoTheSky.Audio;
using UnityEngine;

namespace IntoTheSky.Gameplay.Tiles
{
    public class CrackedTile : Tile
    {
        [Space]
        public float BreakDelay;
        public GameObject BreakFX;
        bool IsHit;

        protected override void OnCollidesWithPlayer(Collider Player)
        {
            base.OnCollidesWithPlayer(Player);
            if(!IsHit)
            {
                IsHit = true;
                Hit();
            }
        }

        void Hit()
        {
            AudioManager.Instance.InteractWithSFX("Platform Fall", SoundEffectBehaviour.Play);
            StartCoroutine(Flash());
            Invoke("Break", BreakDelay);
        }
        IEnumerator Flash()
        {
            flash();
            yield return new WaitForSeconds(0.0625f);
            flash();
            yield return new WaitForSeconds(0.0625f);
            StartCoroutine(Flash());
        }
        void flash()
        {
            foreach (MeshRenderer MR in GetComponentsInChildren<MeshRenderer>())
            {
                MR.enabled = !MR.enabled;
            }
        }
        public void BreakDelayed()
        {
            Invoke("Break", 0.125f);
        }
        void Break()
        {
            Collider[] NearbyColliders = Physics.OverlapSphere(transform.position, 2f, -1, QueryTriggerInteraction.Collide);
            foreach (Collider NC in NearbyColliders)
            {
                CrackedTile crackedTile = NC.GetComponent<CrackedTile>();
                if(crackedTile != null && crackedTile != this)
                {
                    crackedTile.BreakDelayed();
                }
            }

            AudioManager.Instance.InteractWithSFX("Platform Destroy", SoundEffectBehaviour.Play);
            Destroy(Instantiate(BreakFX, transform.position, BreakFX.transform.rotation), 5f);
            Destroy(gameObject);
        }
    }
}