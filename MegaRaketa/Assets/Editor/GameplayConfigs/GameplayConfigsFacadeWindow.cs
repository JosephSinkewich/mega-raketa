using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MegaRaketa.Tools.GameplayConfigs
{
    public class GameplayConfigsFacadeWindow : EditorWindow
    {
        private const string FacadeAssetPath = "Assets/Editor/GameplayConfigs/GameplayConfigsFacade.asset";

        private GameplayConfigsFacade _facade;
        private Vector2 _scrollPosition;
        private readonly Dictionary<int, bool> _sectionFoldouts = new Dictionary<int, bool>();
        private readonly Dictionary<int, UnityEditor.Editor> _configEditors = new Dictionary<int, UnityEditor.Editor>();

        [MenuItem("MegaRaketa/Gameplay Configs")]
        public static void ShowWindow()
        {
            var window = GetWindow<GameplayConfigsFacadeWindow>();
            window.titleContent = new GUIContent("Gameplay Configs");
            window.minSize = new Vector2(420f, 320f);
            window.Show();
        }

        private void OnEnable()
        {
            LoadFacade();
        }

        private void OnDisable()
        {
            DestroyEditors();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(4f);

            EditorGUI.BeginChangeCheck();
            _facade = (GameplayConfigsFacade)EditorGUILayout.ObjectField(
                "Facade",
                _facade,
                typeof(GameplayConfigsFacade),
                false);
            if (EditorGUI.EndChangeCheck())
            {
                DestroyEditors();
            }

            if (_facade == null)
            {
                EditorGUILayout.HelpBox(
                    $"Assign a {nameof(GameplayConfigsFacade)} asset or create one at {FacadeAssetPath}.",
                    MessageType.Info);

                if (GUILayout.Button("Create Facade Asset"))
                {
                    CreateFacadeAsset();
                }

                return;
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            GameplayConfigsFacade.ConfigSection[] sections = _facade.GetSections();
            for (int i = 0; i < sections.Length; i++)
            {
                DrawSection(i, sections[i]);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawSection(int index, GameplayConfigsFacade.ConfigSection section)
        {
            if (!_sectionFoldouts.TryGetValue(index, out bool isExpanded))
            {
                isExpanded = true;
                _sectionFoldouts[index] = isExpanded;
            }

            isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(isExpanded, section.Title);
            _sectionFoldouts[index] = isExpanded;

            if (isExpanded)
            {
                EditorGUI.indentLevel++;

                if (section.Configs == null || section.Configs.Length == 0)
                {
                    EditorGUILayout.HelpBox("No configs in this section.", MessageType.Warning);
                }
                else
                {
                    for (int i = 0; i < section.Configs.Length; i++)
                    {
                        DrawConfig(section.Configs[i]);
                    }
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(6f);
        }

        private void DrawConfig(ScriptableObject config)
        {
            if (config == null)
            {
                EditorGUILayout.HelpBox("Missing config reference.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField(config.name, EditorStyles.boldLabel);

            if (!_configEditors.TryGetValue(config.GetInstanceID(), out UnityEditor.Editor editor) || editor == null)
            {
                editor = UnityEditor.Editor.CreateEditor(config);
                _configEditors[config.GetInstanceID()] = editor;
            }

            EditorGUI.indentLevel++;
            editor.OnInspectorGUI();
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(4f);
        }

        private void LoadFacade()
        {
            _facade = AssetDatabase.LoadAssetAtPath<GameplayConfigsFacade>(FacadeAssetPath);
        }

        private void CreateFacadeAsset()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Editor"))
            {
                AssetDatabase.CreateFolder("Assets", "Editor");
            }

            if (!AssetDatabase.IsValidFolder("Assets/Editor/GameplayConfigs"))
            {
                AssetDatabase.CreateFolder("Assets/Editor", "GameplayConfigs");
            }

            var facade = CreateInstance<GameplayConfigsFacade>();
            AssetDatabase.CreateAsset(facade, FacadeAssetPath);
            AssetDatabase.SaveAssets();
            _facade = facade;
            Selection.activeObject = facade;
            EditorGUIUtility.PingObject(facade);
        }

        private void DestroyEditors()
        {
            foreach (UnityEditor.Editor editor in _configEditors.Values)
            {
                if (editor != null)
                {
                    DestroyImmediate(editor);
                }
            }

            _configEditors.Clear();
        }
    }
}
