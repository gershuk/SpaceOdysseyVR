//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/SpaceOdysseyVR.HandTeleporter/Scripts/HandTeleportActionMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SpaceOdysseyVR.HandTeleporter
{
    public partial class @HandTeleportActionMap : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @HandTeleportActionMap()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""HandTeleportActionMap"",
    ""maps"": [
        {
            ""name"": ""HandTeleporter"",
            ""id"": ""14518560-3de3-4ce7-be19-98e0af534eb4"",
            ""actions"": [
                {
                    ""name"": ""Teleport"",
                    ""type"": ""Button"",
                    ""id"": ""6937c0a4-3bad-483b-9535-122c9e413c6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7f2400c5-6b09-4223-a41b-88a7e109e2f0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCDefault"",
                    ""action"": ""Teleport"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PCDefault"",
            ""bindingGroup"": ""PCDefault"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // HandTeleporter
            m_HandTeleporter = asset.FindActionMap("HandTeleporter", throwIfNotFound: true);
            m_HandTeleporter_Teleport = m_HandTeleporter.FindAction("Teleport", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // HandTeleporter
        private readonly InputActionMap m_HandTeleporter;
        private IHandTeleporterActions m_HandTeleporterActionsCallbackInterface;
        private readonly InputAction m_HandTeleporter_Teleport;
        public struct HandTeleporterActions
        {
            private @HandTeleportActionMap m_Wrapper;
            public HandTeleporterActions(@HandTeleportActionMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @Teleport => m_Wrapper.m_HandTeleporter_Teleport;
            public InputActionMap Get() { return m_Wrapper.m_HandTeleporter; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(HandTeleporterActions set) { return set.Get(); }
            public void SetCallbacks(IHandTeleporterActions instance)
            {
                if (m_Wrapper.m_HandTeleporterActionsCallbackInterface != null)
                {
                    @Teleport.started -= m_Wrapper.m_HandTeleporterActionsCallbackInterface.OnTeleport;
                    @Teleport.performed -= m_Wrapper.m_HandTeleporterActionsCallbackInterface.OnTeleport;
                    @Teleport.canceled -= m_Wrapper.m_HandTeleporterActionsCallbackInterface.OnTeleport;
                }
                m_Wrapper.m_HandTeleporterActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Teleport.started += instance.OnTeleport;
                    @Teleport.performed += instance.OnTeleport;
                    @Teleport.canceled += instance.OnTeleport;
                }
            }
        }
        public HandTeleporterActions @HandTeleporter => new HandTeleporterActions(this);
        private int m_PCDefaultSchemeIndex = -1;
        public InputControlScheme PCDefaultScheme
        {
            get
            {
                if (m_PCDefaultSchemeIndex == -1) m_PCDefaultSchemeIndex = asset.FindControlSchemeIndex("PCDefault");
                return asset.controlSchemes[m_PCDefaultSchemeIndex];
            }
        }
        public interface IHandTeleporterActions
        {
            void OnTeleport(InputAction.CallbackContext context);
        }
    }
}
