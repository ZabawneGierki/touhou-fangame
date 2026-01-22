using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    SaveData SaveData;

    private void Awake()
    {

        instance = this;
        LoadData();
         

    }

 


    private void LoadData()
    {
        // print current setting
        Debug.Log(PlayerData.selectedPlayer + " " + PlayerData.gameDifficulty);
        Debug.Log("Loading Save Data...");

        SaveData = SaveUtil.Load();
         
    }


    public void SaveDataPoints(Points points)
    {
        // Find index of existing entry with same player name and difficulty
        int existingIndex = SaveData.points.FindIndex(p => 
            p.playerName == points.playerName && 
            p.difficulty == points.difficulty);

        if (existingIndex >= 0)
        {
            // Update existing entry at the found index
            
            SaveData.points[existingIndex] = points;
        }
        else
        {
            // Add new entry
            SaveData.points.Add(points);
        }

        SaveUtil.Save(SaveData);
        
    }

    public Points LoadDataPoints(PlayerName playerName, Difficulty difficulty)
    {
        Points points = SaveData.points.Find(p => 
            p.playerName == playerName && 
            p.difficulty == difficulty);
        if (points != null)
        {
             return points;
        }
        else
        {

            Debug.Log("No Points found for Player: " + playerName + " Difficulty: " + difficulty);
            return null;
        }

    }
    public List<Points> LoadSaveDataPoints()
    {
        return SaveData.points;
    }
}
