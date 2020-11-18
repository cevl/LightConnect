using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class LevelDataAssetCreator
{
    public static void CreateLevelDataAsset(List<LevelData> levelsData)
    {
        LevelDataAsset asset = ScriptableObject.CreateInstance<LevelDataAsset>();
        asset.levelsData = levelsData;
        string path = "Assets/Resources/";
        string assetName = "LevelsData.asset";

        string assetPathAndName = path + assetName;

        try
        {
            AssetDatabase.DeleteAsset(assetPathAndName);
            Debug.Log("Old asset deleted.");
        } catch
        {
            Debug.Log("Failed deleting asset.");
        }
        finally
        {
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}