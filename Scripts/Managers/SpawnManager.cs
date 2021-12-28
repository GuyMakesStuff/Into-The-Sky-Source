using System.Collections;
using IntoTheSky.Gameplay.Tiles;
using UnityEngine;

namespace IntoTheSky.Managers
{
    public class SpawnManager : Manager<SpawnManager>
    {
        [Header("Generator Spawn Settings")]
        public MinMax XIncreaseMinMax;
        public MinMax YIncreaseMinMax;
        public MinMax WidthMinMax;
        Vector2Int CurCoord;
        [System.Serializable]
        public class MinMax
        {
            public int Min;
            public int Max;

            public int GetRandom()
            {
                return Random.Range(Min, Max + 1);
            }
        }

        [Header("Difficulty")]
        public int StartPlatformAmount;
        public int PlatformsPerLevel;
        int PrevPlatformNum;
        int Level;
        public Tile[] Tiles;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);

            for (int i = 0; i < StartPlatformAmount; i++)
            {
                CreateNewPlatform();
            }
        }

        void Update()
        {
            
        }

        public void CheckPlatformNum()
        {
            if (GridManager.Instance.PlatformNum > (PlatformsPerLevel + PrevPlatformNum))
            {
                if (Level < Tiles.Length)
                {
                    PrevPlatformNum = GridManager.Instance.PlatformNum;
                    GridManager.Instance.TilePalatte.Add(Tiles[Level]);
                    if (WidthMinMax.Max > WidthMinMax.Min)
                    {
                        WidthMinMax.Max--;
                    }
                    Level++;
                }
            }
        }

        public void CreateNewPlatform()
        {
            bool BuildDir = Random.value > 0.5f;
            int NewWidth = WidthMinMax.GetRandom();
            int PrevWidth = GridManager.Instance.PrevWidth;
            int Addon = PrevWidth + NewWidth + 1;
            int XIncrease = XIncreaseMinMax.GetRandom() + Addon + (int)Random.value;
            XIncrease = (BuildDir) ? XIncrease : -XIncrease;
            int YIncrease = YIncreaseMinMax.GetRandom();
            CurCoord += new Vector2Int(XIncrease, YIncrease);
            GridManager.Instance.SetRandomBrush();
            GridManager.Instance.CreatePlatform(CurCoord, NewWidth);
        }
    }
}