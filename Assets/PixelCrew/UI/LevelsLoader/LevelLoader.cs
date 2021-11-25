using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.LevelsLoader
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _transitionTime;
        
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            AnalyticsEvent.debugMode = true;
            InitLoader();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private static void InitLoader()
        {
            SceneManager.LoadScene("LevelLoader", LoadSceneMode.Additive);
        }

        public void LoadLevel(string sceneName)
        {
            StartCoroutine(StartAnimation(sceneName));
            
        }

        private IEnumerator StartAnimation(string sceneName)
        {
            _animator.SetBool(Enabled, true);
            yield return new WaitForSeconds(_transitionTime);
            SceneManager.LoadScene(sceneName);
            _animator.SetBool(Enabled, false);
        }
    }
}