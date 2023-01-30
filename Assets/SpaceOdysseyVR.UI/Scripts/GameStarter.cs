using SpaceOdysseyVR.Asteroids;

using UnityEngine;

namespace SpaceOdysseyVR.UI
{
    public class GameStarter : MonoBehaviour
    {
        private AsteroidsSpawner _asteroidsSpawner;

        private void Start ()
        {
            _asteroidsSpawner = FindObjectOfType<AsteroidsSpawner>();
            Invoke(nameof(StartSpawning), 10);
        }

        private void StartSpawning () =>
            _asteroidsSpawner.StartSpawning(new(8, 50, 5));
    }
}