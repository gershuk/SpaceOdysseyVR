using SpaceOdysseyVR.Asteroids;

using UnityEngine;

namespace SpaceOdysseyVR.UI
{
    public class GameStarter : MonoBehaviour
    {
        private AsteroidsSpawner _asteroidsSpawner;

        [SerializeField]
        [Range(0f, 100f)]
        private float spawnInterval = 10;

        [SerializeField]
        [Range(0, 100)]
        private uint allDirectAsteroidsCount = 50;

        [SerializeField]
        [Range(0, 100)]
        private uint partitionDirectAsteroidsCount = 5;

        private void Start ()
        {
            _asteroidsSpawner = FindObjectOfType<AsteroidsSpawner>();
            Invoke(nameof(StartSpawning), 30);
        }

        private void StartSpawning () =>
            _asteroidsSpawner.StartSpawning(new(spawnInterval, allDirectAsteroidsCount, partitionDirectAsteroidsCount));
    }
}