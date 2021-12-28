using System.Collections;
using IntoTheSky.IO;
using UnityEngine;

namespace IntoTheSky.Managers
{
    public class ProgressManager : Manager<ProgressManager>
    {
        [System.Serializable]
        public class Progress : SaveFile
        {
            [Space]
            public float HIScore;
            public int Coins;
            public bool[] SkinsUnlocked;
            public int SkinSelected;
        }
        public Progress progress;
        [System.Serializable]
        public class Skin
        {
            public Color SkinColor;
            public int Price;
        }
        public Skin[] Skins;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            LoadProgress();
            FadeManager.Instance.FadeTo("Menu");
        }

        void LoadProgress()
        {
            Progress LoadedProgress = Saver.Load(progress) as Progress;
            if(LoadedProgress != null)
            {
                progress = LoadedProgress;
            }
            else
            {
                ResetProgress();
            }
        }

        public void ResetProgress()
        {
            progress.HIScore = 0f;
            progress.Coins = 0;
            progress.SkinsUnlocked = new bool[Skins.Length];
            progress.SkinsUnlocked[0] = true;
            progress.SkinSelected = 0;
        }

        // Update is called once per frame
        void Update()
        {
            progress.Save();
        }
    }
}