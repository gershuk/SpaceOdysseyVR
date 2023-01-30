#nullable enable

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SpaceOdysseyVR.UI
{
    public sealed class LoseWinMenu : MonoBehaviour
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
        private TextMeshProUGUI _resultLabel;

        [SerializeField]
        private Button _returnButton;

        [SerializeField]
        private SceneController _sceneController;

        [SerializeField]
        private ResultState _state;

        [ContextMenu(nameof(RestartLevel))]
        private void Exit () => Application.Quit();

        [ContextMenu(nameof(RestartLevel))]
        private void RestartLevel () => _sceneController.LoadScene("FirstLevel");

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
            _resultLabel.text = _state switch
            {
                ResultState.Win => "You win",
                ResultState.Lose => "You lose",
            };

            _sceneController = FindObjectOfType<SceneController>(true);
            _restartButton.onClick.AddListener(RestartLevel);
            _exitButton.onClick.AddListener(ShowExitPanel);
            _exitAppButton.onClick.AddListener(Exit);
            _returnButton.onClick.AddListener(ShowMainPanel);
            ShowMainPanel();
        }
    }

    public enum ResultState
    {
        Win,
        Lose,
    }
}