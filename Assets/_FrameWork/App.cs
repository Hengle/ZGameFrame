using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ZGameFrame 
{
	public enum AppMode
	{
	    Developing,
	    QA,
	    Release
	}
		
	public class App : MonoSingleton<App> {
	    public AppMode mode = AppMode.Developing;
	    public int frameRate = 60;

	    public static App GetInstance()
	    {
	        return Instance;
	    }

	    protected override void Awake()
	    {
	        base.Awake();
	        Application.targetFrameRate = frameRate;
	    }

	    public void UpdateFinish()
	    {
	    }

	    IEnumerator Start()
	    {
	        Log.GetInstance();
			AutoUpdate autoUpdate = this.gameObject.AddComponent<AutoUpdate>();
			yield return autoUpdate.StartUpdate();
			yield return null;
	    }

	    #region 全局生命周期回调
	    public delegate void LifeCircleCallback();

	    public LifeCircleCallback onUpdate = null;
	    public LifeCircleCallback onFixedUpdate = null;
	    public LifeCircleCallback onLatedUpdate = null;
	    public LifeCircleCallback onGUI = null;
	    public LifeCircleCallback onApplicationQuit = null;
	    public LifeCircleCallback onDestroy = null;
	    
	    void Update()
	    {
	        if (this.onUpdate != null)
	        {
	            this.onUpdate();
	        }
	    }

	    void FixedUpdate()
	    {
	        if (this.onFixedUpdate != null)
	        {
	            this.onFixedUpdate();
	        }
	    }
	    
	    void LatedUpdate()
	    {
	        if (this.onLatedUpdate != null)
	        {
	            this.onLatedUpdate();
	        }
	    }

	    void OnGUI()
	    {
	        if (this.onGUI != null)
	        {
	            this.onGUI();
	        }
	    }

	    void OnApplicationQuit()
	    {
	        if (this.onApplicationQuit != null)
	        {
	            this.onApplicationQuit();
	        }
	    }

	    void OnDestroy()
	    {
	        if (this.onDestroy != null)
	        {
	            this.onDestroy();
	        }
	    }
	    #endregion

	}

}
