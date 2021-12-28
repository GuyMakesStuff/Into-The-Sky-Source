using IntoTheSky.Visuals;
using IntoTheSky.Audio;
using UnityEngine;

namespace IntoTheSky.Gameplay.Tiles
{
    public class SpikeTile : Tile
    {
        [Space]
        public Animator Spikes;
        public float AttackDelay;
        public float StayTime;
        float Timer;
        bool IsOn;
        BoxCollider boxCollider;

        private void Start()
        {
            IsOn = false;
            Timer = AttackDelay;
            boxCollider = GetComponent<BoxCollider>();
        }

        protected override void SubUpdate()
        {
            base.SubUpdate();
            Timer -= Time.deltaTime;
            Spikes.SetBool("Attacking", IsOn);
            boxCollider.enabled = IsOn;
            if(Timer <= 0f)
            {
                IsOn = !IsOn;
                Timer = (IsOn) ? StayTime : AttackDelay;
                if (FindObjectOfType<CameraFollow>().InRangeOfCamera(transform))
                {
                    AudioManager.Instance.InteractWithSFX("Spikes Attack", SoundEffectBehaviour.Play);
                }
            }
        }

        protected override void OnCollidesWithPlayer(Collider Player)
        {
            if (Time.timeSinceLevelLoad > 1f)
            {
                Player.GetComponent<Player>().Kill();
            }
        }
    }
}