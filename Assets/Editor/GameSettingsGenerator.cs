using System;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GameSettingsGenerator : EditorWindow
    {
        private const string FILE_EXTENSION = ".dat";
        
        private string _fileName;
        private SettingsSerializer _settingsSerializer;
        private GameSettingsWrapper _dataWrapper;
        private SerializedObject _serializedObject;
        
        [MenuItem("Cooking game/Game Settings")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<GameSettingsGenerator>("Game Settings Generator");
            window.minSize = new Vector2(500, 200);
        }

        private void OnEnable()
        {
            _dataWrapper = ScriptableObject.CreateInstance<GameSettingsWrapper>();
            _serializedObject = new SerializedObject(_dataWrapper);
            _settingsSerializer = new SettingsSerializer();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginVertical();
            
            _fileName = EditorGUILayout.TextField("File Name", _fileName);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Settings"))
            {
                _serializedObject.ApplyModifiedProperties();
                SaveData();
            }
            if (GUILayout.Button("Load Settings"))
            {
                LoadData();
            }

            if (GUILayout.Button("Load Selected In Project Tab"))
            {
                LoadSelected();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(20);
            
            var property = _serializedObject.FindProperty("GameSettings");
            EditorGUILayout.PropertyField(property);
            
            EditorGUILayout.EndVertical();
        }

        private void LoadSelected()
        {
            var firstSelected = Selection.objects[0];
            _fileName = firstSelected.name;
            LoadData();
        }

        private void LoadData()
        {
            var endPath = _fileName + FILE_EXTENSION;
            var loadedSettings = _settingsSerializer.LoadSettings<GameSettings>(endPath);
            _dataWrapper.GameSettings = loadedSettings;
            _serializedObject.Update();
            _serializedObject.ApplyModifiedProperties();
        }

        private void SaveData()
        {
            var endPath = _fileName + FILE_EXTENSION;
            _settingsSerializer.SerializeToAssets(_dataWrapper.GameSettings,endPath);
            AssetDatabase.Refresh();
        }
    }
}