#nullable enable

using System;
using System.Collections;

using SpaceOdysseyVR.UI;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceOdysseyVR.SceneController
{
    public sealed class SceneLoader : MonoBehaviour
    {
        public event Action? OnSceneLoaded;

        [SerializeField]
        private LevelLoadingUI _levelLoadingUI;

        private Coroutine? _sceneLoadingCoroutine;

        private IEnumerator LoadAsyncScene (string sceneName)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!asyncLoad.isDone)
            {
                _levelLoadingUI.LoadingPercent = asyncLoad.progress;
                yield return null;
            }

            _levelLoadingUI.LoadingPercent = asyncLoad.progress;
            yield return new WaitForSeconds(5f);
            _sceneLoadingCoroutine = null;
            OnSceneLoaded?.Invoke();
        }

        private void OnDisable ()
        {
            _levelLoadingUI.gameObject.SetActive(false);
        }

        private void OnEnable ()
        {
            _levelLoadingUI.gameObject.SetActive(true);
        }

        public void StartLoadingScene (string sceneName) =>
                            _sceneLoadingCoroutine = _sceneLoadingCoroutine == null
                                    ? StartCoroutine(LoadAsyncScene(sceneName))
                                    : throw new System.Exception();
    }
}