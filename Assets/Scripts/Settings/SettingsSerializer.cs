using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace
{
    public class SettingsSerializer
    {
        public void SerializeToAssets<T>(T objectToSerialize, string fileName)
        {
            var fullpath = Application.dataPath + "/Settings/" + fileName;
            try
            {
                var jsonData = JsonConvert.SerializeObject(objectToSerialize);
                using var stream = File.Open(fullpath, FileMode.Create);
                using var writer = new StreamWriter(stream);
                writer.Write(jsonData);
            }
            catch (Exception exception)
            {
                Debug.LogError("Error while trying to save settings: " + exception);
            }
        }

        public T LoadSettings<T>(string fileName)
        {
            var fullpath = Application.dataPath + "/Settings/" + fileName;

            if (!File.Exists(fullpath))
            {
                Debug.LogError("Given settings file doesn't exist.");
                return default;
            }

            try
            {
                using var stream = new FileStream(fullpath, FileMode.Open);
                using var reader = new StreamReader(stream);
                var loadedJson = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<T>(loadedJson);
            }
            catch (Exception exception)
            {
                Debug.LogError("Error while trying to read saved settings: " + exception);
            }

            return default;
        }
    }
}