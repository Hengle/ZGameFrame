using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ZGameFrame;

public class BuildPlayerMenuItems
{
    const string MenuName = "ZFTools/Build Player/";
    const string PC = "PC";
    const string IOS = "IOS";
    const string Android = "Android";
    const string BuildPlayerPath = "BuildPlayerPath";

    [MenuItem(MenuName + PC)]
    public static void BuildPCPlayer()
    {
        BuildPlayer(BuildTarget.StandaloneWindows);
    }

    [MenuItem(MenuName + IOS)]
    public static void BuildIOSPlayer()
    {
        BuildPlayer(BuildTarget.iOS);
    }

    [MenuItem(MenuName + Android)]
    public static void BuildAndroidPlayer()
    {
        BuildPlayer(BuildTarget.Android);
    }


    public static void BuildPlayer(BuildTarget target)
    {
        Setting.GetInstance().InitEditor();

        string targetName = Platform.GetPlatformForAssetBundles(target);
        if (targetName == null)
            return;

        string rootPath = Setting.GetInstance().GetString("AssetBundleRoot", AppConst.DefalutAssetBundleRoot);

        string outputPath = Path.Combine(Path.Combine(BuildPlayerPath, rootPath), targetName);
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        string[] levels = GetLevelsFromBuildSettings();
        if (levels.Length == 0)
        {
            Debug.Log("Nothing to build.");
            return;
        }

        CopyAssetBundlesTo(Path.Combine(Application.streamingAssetsPath, rootPath), targetName);
        AssetDatabase.Refresh();

        string name = string.Format("{0}/{1}.{2}", outputPath, targetName, GetBuildTargetSuffixName(target));
        Debug.Log(string.Format("name: {0}, asset outpaht: {1}", name, rootPath));
        BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
        BuildPipeline.BuildPlayer(levels, name, target, option);
    }

    static void CopyAssetBundlesTo(string outputPath, string targetName)
    {
        // Clear streaming assets folder.
        FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
        Directory.CreateDirectory(outputPath);

        // Setup the source folder for assetbundles.
        string assetBundlesOutputPath = Setting.GetInstance().GetString("AssetBundleRoot", AppConst.DefalutAssetBundleRoot);
        var source = Path.Combine(Path.Combine(System.Environment.CurrentDirectory, assetBundlesOutputPath), targetName);
        if (!System.IO.Directory.Exists(source))
            Debug.Log("No assetBundle output folder, try to build the assetBundles first.");

        // Setup the destination folder for assetbundles.
        var destination = System.IO.Path.Combine(outputPath, targetName);
        if (System.IO.Directory.Exists(destination))
            FileUtil.DeleteFileOrDirectory(destination);

        FileUtil.CopyFileOrDirectory(source, destination);

        // 复制setting文件
        //string settingName = "/setting.txt";
        //FileUtil.CopyFileOrDirectory(Application.dataPath + "/_Resources/" + settingName, destination + "/../.." + settingName);
        
    }

    static string[] GetLevelsFromBuildSettings()
    {
        List<string> levels = new List<string>();
        for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
        {
            if (EditorBuildSettings.scenes[i].enabled)
                levels.Add(EditorBuildSettings.scenes[i].path);
        }

        return levels.ToArray();
    }

    public static string GetBuildTargetSuffixName(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "apk";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "exe";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "app";
            // Add more build targets for your own.
            default:
                Debug.Log("Target not implemented.");
                return null;
        }

    }
}
