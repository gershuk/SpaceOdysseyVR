#nullable enable

using Unity.Plastic.Newtonsoft.Json.Serialization;

using UnityEngine;

namespace SpaceOdysseyVR.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Action _hideMenu;
        private Action _showMenu;
        public Action BackToGame { get => _hideMenu; set => _hideMenu = value; }
        public Action ShowMenu { get => _showMenu; set => _showMenu = value; }

        [ContextMenu("BackToGame")]
        private void BackToGameSupport () => BackToGame?.Invoke();

        [ContextMenu("ShowMenu")]
        private void ShowMenuSupport () => ShowMenu?.Invoke();
    }
}