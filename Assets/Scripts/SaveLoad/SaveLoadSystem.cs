using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;

public static class SaveLoadSystem
{
    public static int SaveDataVersion { get; private set; } = 1;

    private static readonly string SaveFileName = "SaveAuto.sav";

    private static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    public static bool Save(SaveData data)
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, SaveFileName);
        Logger.Log($"SaveSuccess: {SaveDirectory}/{SaveFileName}");

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Serialize(writer, data);
        }

        return true;
    }

    public static SaveData Load()
    {
        var path = Path.Combine(SaveDirectory, SaveFileName);
        if (!File.Exists(path))
        {
            return null;
        }

        Debug.Log("Load2");
        SaveData data = null;
        using (var reader = new JsonTextReader(new StreamReader(path)))
        {
            var serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Converters.Add(new SaveDataConverter());
            data = serializer.Deserialize<SaveData>(reader);
        }

        while (data.Version < SaveDataVersion)
        {
            data = data.VersionUp();
        }

        return data;
    }
}
