using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using ZGameFrame;

public class AssetBundlesMenuItems
{
    const string BuildAssetBundlesMenu = "ZFTools/Build AssetBundles/";
    const string PC = "PC";
    const string Android = "Android";
    const string IOS = "IOS";
    const string Reload = "Reload";

    [MenuItem(BuildAssetBundlesMenu + PC)]
    static void BuildPCAssetBundles()
    {
        BuildAssetBundles(BuildTarget.StandaloneWindows);
    }

    [MenuItem(BuildAssetBundlesMenu + IOS)]
    static void BuildIOSAssetBundles()
    {
        BuildAssetBundles(BuildTarget.iOS);
    }

    [MenuItem(BuildAssetBundlesMenu + Android)]
    static void BuildAndroidAssetBundles()
    {
        BuildAssetBundles(BuildTarget.Android);
    }

    [MenuItem(BuildAssetBundlesMenu + Reload)]
    static void ReloadLua()
    {
    }

    /// <summary>
    /// 打包assetbundle
    /// </summary>
    /// <param name="target"></param>
    public static void BuildAssetBundles(BuildTarget target)
    {
        // Choose the output path according to the build target.
        Setting.GetInstance().InitEditor();

        string assetBundlesOutputPath = Setting.GetInstance().GetString("AssetBundleRoot", AppConst.DefalutAssetBundleRoot);
        string outputPath = Path.Combine(assetBundlesOutputPath, Platform.GetPlatformForAssetBundles(target));
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        //@TODO: use append hash... (Make sure pipeline works correctly with it.)
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, target);
        AssetDatabase.Refresh();
        Debug.Log("Build AssetBundles OK, outputPath: " + outputPath);
    }
}
