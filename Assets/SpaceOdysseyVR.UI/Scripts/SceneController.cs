#nullable enable

using SpaceOdysseyVR.Asteroids;
using SpaceOdysseyVR.ElectroProps;
using SpaceOdysseyVR.Player;

using UnityEngine;

namespace SpaceOdysseyVR.UI
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField]
        private AsteroidsSpawner _asteroidsSpawner;

        private GameState _gameState;

        [SerializeField]
        private MenuUI? _menuUI;

        [SerializeField]
        private PlayerController? _player;

        [SerializeField]
        private SceneLoader? _sceneLoader;

        [SerializeField]
        private SpaceShipHull _spaceShipHull;

        public GameState GameState
        {
            get => _gameState;
            private set
            {
                if (_gameState == value)
                    return;
                _gameState = value;
                _ = _gameState switch
                {
                    GameState.None => SetNoneState(),
                    GameState.Game => SetGameState(),
                    GameState.SceneLoading => SetSceneLoadingState(),
                    GameState.Menu => SetMenuState(),
                    _ => throw new System.NotImplementedException(),
                };
            }
        }

        private void Awake ()
        {
            DontDestroyOnLoad(gameObject);
            //if (_menuUI != null)
            //    DontDestroyOnLoad(_menuUI.gameObject);
            //if (_sceneLoader != null)
            //    DontDestroyOnLoad(_sceneLoader.gameObject);
        }

        private void FindAllRequeredComponents ()
        {
            _menuUI = FindObjectOfType<MenuUI>(true);
            _sceneLoader = FindObjectOfType<SceneLoader>(true);

            _player = FindObjectOfType<PlayerController>(true);
            if (_player != null)
            {
                _player.ShowMenu = () => GameState = GameState.Menu;
                _player.BackToGame = () => GameState = GameState.Game;
            }

            _spaceShipHull = FindObjectOfType<SpaceShipHull>(true);
            if (_spaceShipHull != null)
                _spaceShipHull.OnShipDeath += () => LoadScene("LoseScene");

            _asteroidsSpawner = FindObjectOfType<AsteroidsSpawner>(true);
            if (_asteroidsSpawner != null)
                _asteroidsSpawner.SpawningEndedAndAllDestoried += () => LoadScene("WinScene");
        }

        private void OnSceneLoadingDone ()
        {
            FindAllRequeredComponents();
            GameState = GameState.Game;
        }

        [ContextMenu(nameof(SetGameState))]
        private SceneController SetGameState ()
        {
            if (_menuUI != null)
                _menuUI.gameObject.SetActive(false);
            if (_player != null)
                _player.gameObject.SetActive(true);
            if (_sceneLoader != null)
                _sceneLoader.gameObject.SetActive(false);
            return this;
        }

        [ContextMenu(nameof(SetMenuState))]
        private SceneController SetMenuState ()
        {
            if (_menuUI != null)
                _menuUI.gameObject.SetActive(true);
            if (_player != null)
                _player.gameObject.SetActive(true);
            if (_sceneLoader != null)
                _sceneLoader.gameObject.SetActive(false);
            return this;
        }

        [ContextMenu(nameof(SetNoneState))]
        private SceneController SetNoneState ()
        {
            if (_menuUI != null)
                _menuUI.gameObject.SetActive(false);
            if (_player != null)
                _player.gameObject.SetActive(false);
            if (_sceneLoader != null)
                _sceneLoader.gameObject.SetActive(false);
            return this;
        }

        [ContextMenu(nameof(SetSceneLoadingState))]
        private SceneController SetSceneLoadingState ()
        {
            if (_menuUI != null)
                _menuUI.gameObject.SetActive(false);
            if (_player != null)
                _player.gameObject.SetActive(false);
            if (_sceneLoader != null)
                _sceneLoader.gameObject.SetActive(true);
            return this;
        }

        private void Start ()
        {
            FindAllRequeredComponents();

            if (_sceneLoader != null)
                _sceneLoader.OnSceneLoaded += OnSceneLoadingDone;
            else
                Debug.LogError("Scene loader not found.");

            GameState = GameState.Game;
        }

        public void LoadScene (string name)
        {
            GameState = GameState.SceneLoading;
            if (_sceneLoader != null)
                _sceneLoader.StartLoadingScene(name);
            else
                Debug.LogError("Scene loader not found.");
        }

        public void RestartLevel ()
        {
            GameState = GameState.SceneLoading;
            if (_sceneLoader != null)
                _sceneLoader.Restart();
            else
                Debug.LogError("Scene loader not found.");
        }
    }

    public enum GameState
    {
        None = 0,
        Game = 1,
        SceneLoading = 2,
        Menu = 3,
    }
}