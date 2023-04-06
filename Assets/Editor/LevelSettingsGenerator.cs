using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LevelSettingsGenerator : EditorWindow
    {
        private const string FILE_EXTENSION = ".dat";

        private string _fileName;
        private SettingsSerializer _settingsSerializer;
        private LevelSettingsWrapper _dataWrapper;
        private SerializedObject _serializedObject;
        private string _errorString;
        private bool _hasError;

        [MenuItem("Cooking game/Level Settings")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<LevelSettingsGenerator>("Level Settings Generator");
            window.minSize = new Vector2(500, 600);
        }

        private void OnEnable()
        {
            _dataWrapper = ScriptableObject.CreateInstance<LevelSettingsWrapper>();
            _dataWrapper.LevelSettings = new LevelSettings();
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
            if (_hasError)
            {
                EditorGUILayout.HelpBox(_errorString, MessageType.Error);
                EditorGUILayout.Space(20);
            }

            var property = _serializedObject.FindProperty("LevelSettings");
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
            var loadedSettings = _settingsSerializer.LoadSettings<LevelSettings>(endPath);
            _dataWrapper.LevelSettings = loadedSettings;
            _hasError = false;
            _serializedObject.Update();
            _serializedObject.ApplyModifiedProperties();
        }

        private void SaveData()
        {
            if (_fileName == "")
            {
                ShowError("File name is empty");
                return;
            }

            if (!ValidateSettings())
            {
                return;
            }

            _hasError = false;
            var endPath = _fileName + FILE_EXTENSION;
            _settingsSerializer.SerializeToAssets(_dataWrapper.LevelSettings, endPath);
            AssetDatabase.Refresh();
        }

        private void ShowError(string errorString)
        {
            _hasError = true;
            _errorString = errorString;
        }

        private bool ValidateSettings()
        {
            var settings = _dataWrapper.LevelSettings;

            if (settings.OrdersTotal <= 0)
            {
                ShowError("Orders can't be lower than 1.");
                return false;
            }

            if (settings.PremadeOrders.Count > settings.OrdersTotal)
            {
                ShowError("Amount of premade orders can't be higher than orders total.");
                return false;
            }

            var foodTotal = 0;
            var orderNumbers = new HashSet<int>();
            foreach (var order in settings.PremadeOrders)
            {
                if (order.OrderNumber <= 0 || order.FoodInOrder.Count == 0 || order.FoodInOrder.Count > settings.MaxFoodPerOrder)
                {
                    ShowError("Incorrect premade order setup.");
                    return false;
                }

                if (orderNumbers.Contains(order.OrderNumber))
                {
                    ShowError("Duplicate premade order numbers.");
                    return false;
                }

                orderNumbers.Add(order.OrderNumber);
                foodTotal += order.FoodInOrder.Count;
            }

            var randomOrders = settings.OrdersTotal - settings.PremadeOrders.Count;
            if (foodTotal + randomOrders * settings.MaxFoodPerOrder < settings.FoodTotal)
            {
                ShowError("Food total is too high.");
                return false;
            }
            foodTotal += randomOrders;
            if (foodTotal > settings.FoodTotal)
            {
                ShowError("Food total is too low.");
                return false;
            }

            return true;
        }
    }
}