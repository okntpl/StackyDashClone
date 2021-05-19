using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    [Header("GameStats")]
    [SerializeField] int level_id;
    [SerializeField] int total_level_count;

    public int Level_id
    {
        get => PlayerPrefs.GetInt("Level_id", level_id);
        set => PlayerPrefs.SetInt("Level_id", value);
    }

    public int Total_Level_Count
    {
        get => total_level_count;
       
    }
}
