using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

namespace IntoTheSky.Managers
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class PPManager : Manager<PPManager>
    {
        [Space]
        public bool Enabled;
        public PostProcessProfile Profile;
        PostProcessVolume Vol;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);

            Vol = GetComponent<PostProcessVolume>();
        }

        // Update is called once per frame
        void Update()
        {
            Vol.profile = Profile;
            Vol.enabled = Enabled;
        }
    }
}