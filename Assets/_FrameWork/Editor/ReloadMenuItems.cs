using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class ReloadMenuItems
{
    const string Menu = "ZFTools/Reload";
    [MenuItem(Menu + " %g")]
    static void ReloadLua()
    {
    }

}
