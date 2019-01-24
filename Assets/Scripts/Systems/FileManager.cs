using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager
{

    public static T Load<T>(string filename) where T : new()
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        T output;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            output = JsonUtility.FromJson<T>(dataAsJson);
        }
        else
        {
            output = new T();
        }

        return output;
    }

    public static void Save<T>(string filename, T content)
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);

        string dataAsJson = JsonUtility.ToJson(content);
        File.WriteAllText(filePath, dataAsJson);
    }
}
