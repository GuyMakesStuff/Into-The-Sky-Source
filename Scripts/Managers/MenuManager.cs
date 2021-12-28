using System.Collections.Generic;
using IntoTheSky.Interface;
using IntoTheSky.Gameplay;
using UnityEngine.Events;
using IntoTheSky.Audio;
using UnityEngine.UI;
using IntoTheSky.IO;
using UnityEngine;
using TMPro;

namespace IntoTheSky.Managers
{
    public class MenuManager : Manager<MenuManager>
    {
        [Header("Shop")]
        public TMP_Text CoinCounter;
        public Player player;
        public PrevNextMenu ShopNavigation;
        public TMP_Text PriceText;
        public GameObject BuyButton;
        public GameObject SelectButton;
        int CurSkinIndex;

        [Header("Options")]
        public Slider MusicSlider;
        public Slider SFXSlider;
        public TMP_Dropdown QualDropdown;
        public TMP_Dropdown ResDropdown;
        Resolution[] Resolutions;
        public Toggle FSToggle;
        public Toggle PPToggle;
        [System.Serializable]
        public class Settings : SaveFile
        {
            [Space]
            public float MusicVol;
            public float SFXVol;
            public int QualLevel;
            public int ResIndex;
            public bool FS;
            public bool PP;
        }
        public Settings settings;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            AudioManager.Instance.SetMusicTrack("Menu");

            /* Usually Dont Write Comments
            However There Is A Lot Of Mess Here
            So Here We Go :)
            */

            // Shop Init
            ShopNavigation.Value = ProgressManager.Instance.progress.SkinSelected;
            ShopNavigation.MinValue = 0;
            ShopNavigation.MaxValue = ProgressManager.Instance.Skins.Length - 1;

            // Settings Init
            // Resolution Updating Takes More Code Unfortunatly :/
            #region Resolution
            Resolutions = Screen.resolutions;
            ResDropdown.ClearOptions();
            List<string> Res2String = new List<string>();
            int CurResIndex = 0;
            for (int R = 0; R < Resolutions.Length; R++)
            {
                Resolution Res = Resolutions[R];
                string String = Res.width + "x" + Res.height;
                // Bonus Code For Getting Cuurent Resolution Index
                Resolution CurRes = Screen.currentResolution;
                if(Res.width == CurRes.width && Res.height == CurRes.height)
                {
                    CurResIndex = R;
                }

                Res2String.Add(String);
            }
            ResDropdown.AddOptions(Res2String);
            #endregion
            // Back To Regularly Scedualed Program
            Settings LoadedSettings = Saver.Load(settings) as Settings;
            if(LoadedSettings != null)
            {
                settings = LoadedSettings;
            }
            else
            {
                settings.MusicVol = 0f;
                settings.SFXVol = 0f;
                settings.QualLevel = QualitySettings.GetQualityLevel();
                settings.ResIndex = CurResIndex;
                settings.FS = Screen.fullScreen;
                settings.PP = PPManager.Instance.Enabled;
            }
            ResDropdown.onValueChanged.AddListener(new UnityAction<int>(SetRes));
            FSToggle.onValueChanged.AddListener(new UnityAction<bool>(SetFS));
            SetSettings();
        }
        void SetSettings()
        {
            MusicSlider.value = settings.MusicVol;
            SFXSlider.value = settings.SFXVol;
            QualDropdown.value = settings.QualLevel;
            ResDropdown.value = settings.ResIndex;
            FSToggle.isOn = settings.FS;
            PPToggle.isOn = settings.PP;
        }

        // Update is called once per frame
        void Update()
        {
            // Shop Updating
            CoinCounter.text = ProgressManager.Instance.progress.Coins.ToString("000");
            PriceText.text = "Price:" + ProgressManager.Instance.Skins[ShopNavigation.Value].Price;
            BuyButton.SetActive(!ProgressManager.Instance.progress.SkinsUnlocked[ShopNavigation.Value]);
            SelectButton.SetActive(ProgressManager.Instance.progress.SkinsUnlocked[ShopNavigation.Value] && ProgressManager.Instance.progress.SkinSelected != ShopNavigation.Value);
            player.SkinIndex = ShopNavigation.Value;

            // Settings Updating
            AudioManager.Instance.SetMusicVolume(MusicSlider.value);
            settings.MusicVol = MusicSlider.value;
            AudioManager.Instance.SetSFXVolume(SFXSlider.value);
            settings.SFXVol = SFXSlider.value;
            QualitySettings.SetQualityLevel(QualDropdown.value);
            settings.QualLevel = QualDropdown.value;
            settings.ResIndex = ResDropdown.value;
            settings.FS = FSToggle.isOn;
            PPManager.Instance.Enabled = PPToggle.isOn;
            settings.PP = PPToggle.isOn;
            settings.Save();
        }

        public void Play()
        {
            FadeManager.Instance.FadeTo("Main");
        }
        public void QuitGame()
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
            #endif
        }

        public void Buy()
        {
            if(ProgressManager.Instance.progress.Coins >= ProgressManager.Instance.Skins[ShopNavigation.Value].Price)
            {
                ProgressManager.Instance.progress.SkinsUnlocked[ShopNavigation.Value] = true;
                ProgressManager.Instance.progress.Coins -= ProgressManager.Instance.Skins[ShopNavigation.Value].Price;
                AudioManager.Instance.InteractWithSFX("Buy", SoundEffectBehaviour.Play);
            }
            else
            {
                AudioManager.Instance.InteractWithSFX("Land", SoundEffectBehaviour.Play);
            }
        }
        public void Select()
        {
            ProgressManager.Instance.progress.SkinSelected = ShopNavigation.Value;
            PlaySelectSound();
        }
        
        public void SetFS(bool FS)
        {
            UpdateScreen();
        }
        public void SetRes(int ResIndex)
        {
            UpdateScreen();
        }
        void UpdateScreen()
        {
            Resolution Res = Resolutions[ResDropdown.value];
            Screen.SetResolution(Res.width, Res.height, FSToggle.isOn);
        }
        public void ResetProgress()
        {
            ProgressManager.Instance.ResetProgress();
            AudioManager.Instance.InteractWithSFX("Reset Progress", SoundEffectBehaviour.Play);
        }
    }
}