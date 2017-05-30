using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


class AppStart
{
    /// <summary>
    /// 进入游戏
    /// </summary>
    public static IEnumerator ApplicationDidFinishLaunching()
    {
		yield return null;
        //yield return AutoUpdate.GetInstance().Init();
        //yield return ResMgr.GetInstance().Init();
        //yield return ABManager.GetInstance().Init();
    }
}

