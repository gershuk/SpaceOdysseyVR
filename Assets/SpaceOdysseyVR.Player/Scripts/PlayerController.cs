#nullable enable

using Unity.Plastic.Newtonsoft.Json.Serialization;

using UnityEngine;

using Valve.VR;

namespace SpaceOdysseyVR.Player
{
    public class PlayerController : MonoBehaviour
    {
        public SteamVR_Action_Boolean action;
        public SteamVR_Input_Sources handType;
        private Action _hideMenu;
        private Action _showMenu;
        public Action BackToGame { get => _hideMenu; set => _hideMenu = value; }
        public Action ShowMenu { get => _showMenu; set => _showMenu = value; }

        [ContextMenu("BackToGame")]
        private void BackToGameSupport () => BackToGame?.Invoke();

        [ContextMenu("ShowMenu")]
        private void ShowMenuSupport () => ShowMenu?.Invoke();

        void Start ()
        {
            action.AddOnStateDownListener(TriggerDown, handType);
            action.AddOnStateUpListener(TriggerUp, handType);
            gameObject.SetActive(false);
        }

        public void TriggerUp (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            BackToGameSupport();
        }

        public void TriggerDown (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            ShowMenuSupport();
        }
    }
}