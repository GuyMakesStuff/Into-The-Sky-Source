using System.Collections;
using System.Collections.Generic;
using IntoTheSky.Gameplay;
using IntoTheSky.Gameplay.Tiles;
using UnityEngine;

namespace IntoTheSky.Managers
{
    public class GridManager : Manager<GridManager>
    {
        public List<Tile> TilePalatte;
        [HideInInspector]
        public Tile Brush;
        [HideInInspector]
        public int PlatformNum;
        [HideInInspector]
        public int PrevWidth;
        public Transform PlatformContainer;
        public GameObject CoinPrefab;

        // Start is called before the first frame update
        void Awake()
        {
            Init(this);

            SetBrush(0);
            CreatePlatform(Vector2Int.zero, 10);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetBrush(int TileIndex)
        {
            Brush = TilePalatte[TileIndex];
        }
        public void SetRandomBrush()
        {
            SetBrush(Random.Range(0, TilePalatte.Count));
        }

        public void CreatePlatform(Vector2Int Center, int Size)
        {
            Transform PlatformParent = new GameObject("Platform_" + PlatformNum).transform;
            PlatformParent.gameObject.tag = "Platform";
            PlatformParent.gameObject.layer = 3;
            PlatformParent.transform.SetParent(PlatformContainer);
            bool IsRound = Size % 2 == 0;
            Vector2Int Pos = Center - new Vector2Int(Size / 2, 0);
            PlatformParent.position = new Vector3(Pos.x, Pos.y, 0f);
            BoxCollider boxCollider = PlatformParent.gameObject.AddComponent<BoxCollider>();
            for (int T = 0; T <= Size; T++)
            {
                PlaceTile(Pos + new Vector2Int(T, 0), PlatformParent);
                if (T > 0)
                {
                    boxCollider.size += new Vector3(1, 0, 0);
                }
            }
            boxCollider.center = new Vector3((boxCollider.size.x / 2) - 0.5f, 0, 0);
            PlatformParent.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            Platform platform = PlatformParent.gameObject.AddComponent<Platform>();
            platform.CoinPrefab = CoinPrefab;
            if (PlatformNum != 0)
            {
                platform.TrySpawnCoin();
            }
            PrevWidth = Size;
            PlatformNum++;
            if(PlatformNum > 1)
            {
                SpawnManager.Instance.CheckPlatformNum();
            }
        }

        public void PlaceTile(Vector2Int Coord, Transform Parent)
        {
            Tile NewTile = Instantiate(Brush.gameObject, Vector3.zero, Brush.transform.rotation, Parent).GetComponent<Tile>();
            NewTile.Coord = Coord;
        }
    }
}