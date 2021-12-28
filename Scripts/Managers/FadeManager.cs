using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace IntoTheSky.Managers
{
    public class FadeManager : Manager<FadeManager>
    {
        [Space]
        public Animator FadePanel;
        bool IsFaded;

        void Awake()
        {
            Init(this);
        }

        private void Update()
        {
            FadePanel.SetBool("Is Faded", IsFaded);
        }

        public void FadeTo(string SceneName)
        {
            StartCoroutine(fadeTo(SceneName));
        }
        IEnumerator fadeTo(string scene)
        {
            IsFaded = true;

            yield return new WaitForSeconds(0.75f);

            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            while(!operation.isDone)
            {
                yield return null;
            }

            IsFaded = false;
        }
    }
}