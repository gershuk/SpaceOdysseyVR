#nullable enable

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceOdysseyVR.UI
{
    public sealed class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private LevelLoadingUI _levelLoadingUI;

        private Coroutine? _sceneLoadingCoroutine;

        public event Action? OnSceneLoaded;

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

        private void OnDisable () => _levelLoadingUI.gameObject.SetActive(false);

        private void OnEnable () => _levelLoadingUI.gameObject.SetActive(true);

        public void Restart ()
        {
            _sceneLoadingCoroutine = _sceneLoadingCoroutine == null
                                   ? StartCoroutine(LoadAsyncScene(SceneManager.GetActiveScene().name))
                                   : throw new System.Exception();
        }

        public void StartLoadingScene (string sceneName) =>
                                    _sceneLoadingCoroutine = _sceneLoadingCoroutine == null
                                    ? StartCoroutine(LoadAsyncScene(sceneName))
                                    : throw new System.Exception();
    }
}