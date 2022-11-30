using System;
using System.Collections.Generic;

namespace UnityEngine.XR.OpenXR.Samples.ControllerSample
{
    public class AutomaticTrackingModeChanger : MonoBehaviour
    {
        [SerializeField]
        private float m_ChangeInterval = 5.0f;

        private float m_TimeRemainingTillChange;

        private static List<XRInputSubsystem> s_InputSubsystems = new List<XRInputSubsystem>();
        private static List<TrackingOriginModeFlags> s_SupportedTrackingOriginModes = new List<TrackingOriginModeFlags>();

        private void OnEnable ()
        {
            m_TimeRemainingTillChange = m_ChangeInterval;
        }

        private void Update ()
        {
            m_TimeRemainingTillChange -= Time.deltaTime;
            if (m_TimeRemainingTillChange <= 0.0f)
            {
                List<XRInputSubsystem> inputSubsystems = new List<XRInputSubsystem>();
                SubsystemManager.GetInstances(inputSubsystems);
                XRInputSubsystem subsystem = inputSubsystems?[0];
                if (subsystem != null)
                {
                    UpdateSupportedTrackingOriginModes(subsystem);
                    SetToNextMode(subsystem);
                }
                m_TimeRemainingTillChange += m_ChangeInterval;
            }
        }

        private void UpdateSupportedTrackingOriginModes (XRInputSubsystem subsystem)
        {
            TrackingOriginModeFlags supportedOriginModes = subsystem.GetSupportedTrackingOriginModes();
            s_SupportedTrackingOriginModes.Clear();
            for (int i = 0; i < 31; i++)
            {
                uint modeToCheck = 1u << i;
                if ((modeToCheck & ((UInt32) supportedOriginModes)) != 0)
                {
                    s_SupportedTrackingOriginModes.Add((TrackingOriginModeFlags) modeToCheck);
                }
            }
        }

        private void SetToNextMode (XRInputSubsystem subsystem)
        {
            TrackingOriginModeFlags currentOriginMode = subsystem.GetTrackingOriginMode();
            for (int i = 0; i < s_SupportedTrackingOriginModes.Count; i++)
            {
                if (currentOriginMode == s_SupportedTrackingOriginModes[i])
                {
                    int nextModeIndex = (i + 1) % s_SupportedTrackingOriginModes.Count;
                    subsystem.TrySetTrackingOriginMode(s_SupportedTrackingOriginModes[nextModeIndex]);
                    break;
                }
            }
        }
    }
}