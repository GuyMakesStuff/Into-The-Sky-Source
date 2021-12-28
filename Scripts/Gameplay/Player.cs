using IntoTheSky.Managers;
using System.Collections;
using IntoTheSky.Audio;
using UnityEngine.UI;
using UnityEngine;

namespace IntoTheSky.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public bool IsMain;

        [Header("Movement")]
        public float Speed;
        float X;
        public float JumpForce;
        public LayerMask GroundLayer;
        bool IsGrounded;
        bool IsJumping;
        bool CanGroundCheck;
        Rigidbody Body;

        [Header("Visuals")]
        public Transform ArrowCanvas;
        public Image Arrow;

        [Header("Other")]
        public GameObject KillFX;
        public Material PlayerMat;
        public ParticleSystem GlowPS;
        [HideInInspector]
        public int SkinIndex;

        // Start is called before the first frame update
        void Start()
        {
            Body = GetComponent<Rigidbody>();
            CanGroundCheck = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(IsMain)
            {
                X = Input.GetAxis("Horizontal") * Speed;
                RaycastHit hit = new RaycastHit();
                IsGrounded = Physics.SphereCast(transform.position, 0.495f, Vector3.down, out hit, 0.501f, GroundLayer, QueryTriggerInteraction.Ignore) && CanGroundCheck;

                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f);
                Vector3 MousePos = Body.position - hit.point;
                float Angle = Mathf.Atan2(MousePos.y, MousePos.x) * Mathf.Rad2Deg + 90f;
                ArrowCanvas.rotation = Quaternion.Euler(0, 0, Angle);

                if(IsGrounded)
                {
                    Arrow.color = Color.green;

                    if(Input.GetMouseButtonDown(0) && !GameManager.Instance.IsPaused)
                    {
                        AudioManager.Instance.InteractWithSFX("Jump", SoundEffectBehaviour.Play);
                        StartCoroutine(ToggleGroundCheck());
                        IsJumping = true;
                        Body.velocity = Vector3.zero;
                        Body.AddForce(ArrowCanvas.up * JumpForce * 100f);
                        return;
                    }

                    if(IsJumping)
                    {
                        AudioManager.Instance.InteractWithSFX("Land", SoundEffectBehaviour.Play);
                        IsJumping = false;
                    }
                }
                else
                {
                    if(IsJumping)
                    {
                        Arrow.color = Color.red;
                        return;
                    }

                    Arrow.color = Color.yellow;
                }
            }

            PlayerMat.color = ProgressManager.Instance.Skins[SkinIndex].SkinColor;
            PlayerMat.SetColor("_EmissionColor", PlayerMat.color);
            ParticleSystem.MainModule M = GlowPS.main;
            M.startColor = PlayerMat.color;
        }
        IEnumerator ToggleGroundCheck()
        {
            IsGrounded = false;
            CanGroundCheck = false;
            yield return new WaitForSeconds(0.25f);
            CanGroundCheck = true;
        }

        void FixedUpdate()
        {
            if(IsMain)
            {
                if(IsJumping == false)
                {
                    Body.velocity = new Vector3(X, Body.velocity.y, 0);
                }
                else
                {
                    Body.velocity = Body.velocity;
                }
            }
        }

        public void Kill()
        {
            Destroy(Instantiate(KillFX, transform.position, KillFX.transform.rotation), 5f);
            Destroy(gameObject);
            GameManager.Instance.EndGame();
        }
    }
}