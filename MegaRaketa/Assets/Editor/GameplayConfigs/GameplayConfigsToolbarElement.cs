using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MegaRaketa.Tools.GameplayConfigs
{
    [InitializeOnLoad]
    internal static class GameplayConfigsToolbarHook
    {
        private static readonly Type ToolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static ScriptableObject _currentToolbar;

        static GameplayConfigsToolbarHook()
        {
            EditorApplication.update -= TryInstall;
            EditorApplication.update += TryInstall;
        }

        private static void TryInstall()
        {
            if (_currentToolbar != null || ToolbarType == null)
            {
                return;
            }

            UnityEngine.Object[] toolbars = Resources.FindObjectsOfTypeAll(ToolbarType);
            if (toolbars.Length == 0)
            {
                return;
            }

            _currentToolbar = (ScriptableObject)toolbars[0];

            FieldInfo rootField = ToolbarType.GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
            if (rootField?.GetValue(_currentToolbar) is not VisualElement root)
            {
                _currentToolbar = null;
                return;
            }

            VisualElement toolbarZone = root.Q("ToolbarZoneRightAlign");
            if (toolbarZone == null)
            {
                _currentToolbar = null;
                return;
            }

            var parent = new VisualElement
            {
                style =
                {
                    flexGrow = 1,
                    flexDirection = FlexDirection.Row,
                }
            };

            var container = new IMGUIContainer(OnToolbarGUI);
            container.style.flexGrow = 1;
            parent.Add(container);
            toolbarZone.Add(parent);
        }

        private static void OnToolbarGUI()
        {
            if (GUILayout.Button(new GUIContent("Configs", "Open Gameplay Configs")))
            {
                GameplayConfigsFacadeWindow.ShowWindow();
            }
        }
    }
}
