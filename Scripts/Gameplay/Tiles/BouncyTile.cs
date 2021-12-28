using System.Collections;
using System.Collections.Generic;
using IntoTheSky.Audio;
using UnityEngine;

namespace IntoTheSky.Gameplay.Tiles
{
    public class BouncyTile : Tile
    {
        [Space]
        public float BounceHeight;
        Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        protected override void OnCollidesWithPlayer(Collider Player)
        {
            base.OnCollidesWithPlayer(Player);
            SetNearbyTiles();
            Player.attachedRigidbody.velocity = Vector3.zero;
            Player.attachedRigidbody.AddForce(0, BounceHeight * 100f, 0f);
            animator.SetTrigger("Bounce");
            AudioManager.Instance.InteractWithSFX("Bounce", SoundEffectBehaviour.Play);
        }

        void SetNearbyTiles()
        {
            Collider[] NearbyColliders = Physics.OverlapSphere(transform.position, 2f, -1,  QueryTriggerInteraction.Collide);
            foreach (Collider C in NearbyColliders)
            {
                BouncyTile bouncyTile = C.gameObject.GetComponent<BouncyTile>();
                if (bouncyTile != null && bouncyTile != this)
                {
                    bouncyTile.SetOnOff();
                }
            }
        }

        public void SetOnOff()
        {
            ToggleOnOff();
            Invoke("ToggleOnOff", 0.25f);
        }
        void ToggleOnOff()
        {
            GetComponent<BoxCollider>().enabled = !GetComponent<BoxCollider>().enabled;
        }
    }
}