using UnityEngine;

namespace SpaceOdysseyVR.UI
{
    public class Starter : MonoBehaviour
    {
        [SerializeField]
        private SceneController _sceneController;

        private void Start ()
        {
            if (!_sceneController)
                _sceneController = FindObjectOfType<SceneController>(true);
            Invoke(nameof(StartGame), 0.01f);
        }

        private void StartGame () => _sceneController.LoadScene("FirstLevel");
    }
}