#nullable enable

using UnityEngine;
using UnityEngine.UI;

namespace SpaceOdysseyVR.UI
{
    public sealed class MenuUI : MonoBehaviour
    {
        [SerializeField]
        private Button _exitAppButton;

        [SerializeField]
        private Button _exitButton;

        [SerializeField]
        private GameObject _exitPanel;

        [SerializeField]
        private GameObject _mainPanel;

        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private Button _returnButton;

        [SerializeField]
        private SceneController _sceneController;

        [SerializeField]
        private Slider _volumeSlider;

        [ContextMenu(nameof(RestartLevel))]
        private void Exit () => Application.Quit();

        [ContextMenu(nameof(RestartLevel))]
        private void RestartLevel () => _sceneController.RestartLevel();

        private void SetVolume (float volume) => AudioListener.volume = volume;

        [ContextMenu(nameof(ShowExitPanel))]
        private void ShowExitPanel ()
        {
            _mainPanel.SetActive(false);
            _exitPanel.SetActive(true);
        }

        [ContextMenu(nameof(ShowMainPanel))]
        private void ShowMainPanel ()
        {
            _mainPanel.SetActive(true);
            _exitPanel.SetActive(false);
        }

        private void Start ()
        {
            _sceneController = FindObjectOfType<SceneController>(true);
            _restartButton.onClick.AddListener(RestartLevel);
            _exitButton.onClick.AddListener(ShowExitPanel);
            _volumeSlider.onValueChanged.AddListener(SetVolume);
            _exitAppButton.onClick.AddListener(Exit);
            _returnButton.onClick.AddListener(ShowMainPanel);
            ShowMainPanel();
        }
    }
}