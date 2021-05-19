using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CreateSection

{
  
    [MenuItem("Assets/Create/LevelDesign/Level")]

    public static SectionList Create()
    {
        SectionList asset = ScriptableObject.CreateInstance<SectionList>();
      
        AssetDatabase.CreateAsset(asset, $"Assets/StackyDash/Scripts/ScriptableObjects/"+SceneManager.GetActiveScene().name+".asset");
        AssetDatabase.SaveAssets();
        IncrementDesignCount();
        return asset;
    }


    private static void IncrementDesignCount()
    {
        PlayerPrefs.SetInt("DesignCount", PlayerPrefs.GetInt("DesignCount") + 1);
    }


}
