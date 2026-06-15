#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MegaRaketa.Marketing.Editor
{
    [InitializeOnLoad]
    internal static class MarketingCreativeRigPlayModeGuard
    {
        static MarketingCreativeRigPlayModeGuard()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingPlayMode)
            {
                return;
            }

            MarketingCreativeRig[] rigs = Object.FindObjectsByType<MarketingCreativeRig>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            for (int i = 0; i < rigs.Length; i++)
            {
                rigs[i].RestorePlayModeSnapshotsFromEditor();
            }
        }
    }
}
#endif
