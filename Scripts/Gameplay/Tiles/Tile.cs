using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IntoTheSky.Gameplay.Tiles
{
    public class Tile : MonoBehaviour
    {
        public Vector2Int Coord;

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3((float)Coord.x, (float)Coord.y, 0f);
            SubUpdate();
        }

        protected virtual void SubUpdate()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                OnCollidesWithPlayer(other);
            }
        }

        protected virtual void OnCollidesWithPlayer(Collider Player)
        {

        }
    }
}