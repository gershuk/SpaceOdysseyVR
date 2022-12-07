#nullable enable

using UnityEngine;

namespace SpaceOdysseyVR.AsteroidRadar
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class RadarPointController : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public Vector3 Position
        {
            get => _rectTransform.anchoredPosition3D;
            set => _rectTransform.anchoredPosition3D = value;
        }

        public float Size { get; set; }

        private void Start ()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.sizeDelta = new(Size, Size);
        }
    }
}