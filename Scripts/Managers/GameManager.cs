using UnityEngine.SceneManagement;
using IntoTheSky.Gameplay;
using IntoTheSky.Visuals;
using IntoTheSky.Audio;
using UnityEngine;
using TMPro;

namespace IntoTheSky.Managers
{
    public class GameManager : Manager<GameManager>
    {
        public Player player;

        [Header("UI")]
        public TMP_Text ScoreText;
        public Transform Player;
        float Score;
        public TMP_Text HIScoreText;
        public Transform HIScoreMarker;
        public GameObject NewHIScoreText;
        float HIScore;
        bool HIBeat;

        [Header("Difficulty")]
        public int ScorePerlevel;
        public Lava lava;
        float PrevScore;

        [Header("Mini Menus")]
        public GameObject PauseMenu;
        [HideInInspector]
        public bool IsPaused;
        bool CanPause;
        public GameObject GameOverMenu;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            CanPause = true;
            HIScore = ProgressManager.Instance.progress.HIScore;
            AudioManager.Instance.SetMusicTrack("Main");
        }

        // Update is called once per frame
        void Update()
        {
            Score = Player.position.y;
            Score = Mathf.Clamp(Score, 0, float.MaxValue);
            ScoreText.text = "Score:" + Score.ToString("000");
            if(Score > HIScore)
            {
                HIScore = Score;
                if (!HIBeat)
                {
                    HIBeat = true;
                    HIScoreMarker.GetComponent<HIScoreMarker>().ShowNewHIAnim();
                    NewHIScoreText.SetActive(true);
                    AudioManager.Instance.InteractWithSFX("New HI", SoundEffectBehaviour.Play);
                }
            }
            HIScoreText.text = "High Score:" + HIScore.ToString("000");
            HIScoreMarker.position = new Vector3(Camera.main.transform.position.x, HIScore, 0f);
            ProgressManager.Instance.progress.HIScore = HIScore;

            if(Score > ScorePerlevel + PrevScore)
            {
                lava.IncreaseRiseSpeed();
                PrevScore = Score;
            }

            if(Input.GetKeyDown(KeyCode.Escape) && CanPause)
            {
                PlaySelectSound();
                IsPaused = !IsPaused;
            }
            Time.timeScale = (IsPaused) ? 0f : 1f;
            PauseMenu.SetActive(IsPaused);
            SoundEffectBehaviour soundEffectBehaviour = (IsPaused) ? SoundEffectBehaviour.Pause : SoundEffectBehaviour.Resume;
            AudioManager.Instance.InteractWithAllSFX(soundEffectBehaviour);
            AudioManager.Instance.InteractWithMusic(soundEffectBehaviour);

            player.SkinIndex = ProgressManager.Instance.progress.SkinSelected;
        }

        public void Retry()
        {
            PlaySelectSound();
            FadeManager.Instance.FadeTo(SceneManager.GetActiveScene().name);
        }
        public void Menu()
        {
            PlaySelectSound();
            IsPaused = false;
            FadeManager.Instance.FadeTo("Menu");
        }

        public void EndGame()
        {
            AudioManager.Instance.InteractWithSFX("Die", SoundEffectBehaviour.Play);
            AudioManager.Instance.MuteMusic();
            CanPause = false;
            GameOverMenu.SetActive(true);
        }
    }
}