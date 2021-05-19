using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AssetManager : UnityEditor.AssetModificationProcessor

{
    //Detect if asset deleted change the name of asset
    static AssetDeleteResult OnWillDeleteAsset(string path,RemoveAssetOptions opt)
    {
        PlayerPrefs.SetInt("DesignCount", PlayerPrefs.GetInt("DesignCount") - 1);
        return AssetDeleteResult.DidNotDelete;
    }
    
}
