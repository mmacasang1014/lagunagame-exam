#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleCreator : MonoBehaviour
{
    [MenuItem("Assets/Build Asset Bundles")]
    static void BuildBundles()
    {
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        buildMap[0].assetBundleName = "TexturesBundle";

        string[] fileEntries = Directory.GetFiles("Assets/textures/");


        List<string> assetToBundle = new List<string>();

        for(int x=0; x< fileEntries.Length; x++)
        {
            if(string.Compare( Path.GetExtension(fileEntries[x]), ".meta") != 0 )
            {
                assetToBundle.Add(fileEntries[x]);
            }
        }

        buildMap[0].assetNames = assetToBundle.ToArray();

        if(fileEntries.Length > 0)
        {
            BuildPipeline.BuildAssetBundles("Assets/output/", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }

        
    }
}
#endif
