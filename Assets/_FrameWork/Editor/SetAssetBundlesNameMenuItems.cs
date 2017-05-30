using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SetAssetBundlesNameMenuItems
{
    const string SetAssetBundlesNameMenu = "ZFTools/";
    const string Name = "Set AssetBundles Name";
    
    [MenuItem(SetAssetBundlesNameMenu + Name)]
    static void SetAssetBundlesName()
    {
        string path = Application.dataPath + "/LuaProject";
        SetAssetBundleName(path, "code", ".txt");
    }

    static void SetAssetBundleName(string path, string name, params string[] exts)
    {
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] fis = di.GetFiles("*.*", SearchOption.AllDirectories);
        foreach (FileInfo fi in fis)
        {
            if (!Filter(fi.Name, exts))
                continue;
            string assetPath = fi.FullName.Replace(Path.DirectorySeparatorChar, '/');
            assetPath = "Assets" + assetPath.Substring(Application.dataPath.Length);
            AssetImporter ai = AssetImporter.GetAtPath(assetPath);
            ai.assetBundleName = name;
        }
        AssetDatabase.Refresh();
        Debug.Log("Set " + path + " OK!");
    }

    static bool Filter(string str, params string[] exts)
    {
        if (exts == null)
        {
            return false;
        }
        foreach (string ext in exts)
        {
            if (str.EndsWith(ext))
                return true;
        }
        return false;
    }

}
