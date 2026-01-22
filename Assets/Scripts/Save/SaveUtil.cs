using System.IO;
using UnityEngine;
using System;
using JetBrains.Annotations;
using Ink.Parsed;
using System.Collections.Generic;

[Serializable]

public class Points {
    public PlayerName playerName;
    public Difficulty difficulty;
    public int highScore;

}


[ Serializable]
public class SaveData
{
    public List<Points>  points;



}
public static class SaveUtil
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static SaveData Load()
    {
        if (!File.Exists(path))
            return new SaveData(); // returns new blank save if none found

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void ResetSave()
    {
        if (File.Exists(path))
            File.Delete(path);
    }



}
