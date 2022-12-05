#nullable enable

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SpaceOdysseyVR.UI
{
    public class LevelLoadingUI : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private TextMeshProUGUI _textMesh;

        public float LoadingPercent
        {
            get => _slider.value;
            set
            {
                _slider.value = value;
                _textMesh.text = $"{(value * 100):N2}%";
            }
        }

        [ContextMenu("Test0")]
        private void Test () => LoadingPercent = 0f;

        [ContextMenu("Test0.5")]
        private void Test1 () => LoadingPercent = 0.5f;

        [ContextMenu("Test1")]
        private void Test2 () => LoadingPercent = 1f;
    }
}