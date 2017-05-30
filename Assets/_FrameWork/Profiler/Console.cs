using UnityEngine;
using System.Collections;

namespace ZGameFrame
{
	public class Console {
	    protected static Console m_Instance;
	    public static Console Instance
	    {
	        get
	        {
	            if (m_Instance == null)
	            {
	                m_Instance = new Console();
	            }
	            return m_Instance;
	        }
	    }

	    public static Console GetInstance()
	    {
	        return Instance;
	    }

	    private FPSCounter fpsCounter = null;
	    private MemoryDetector memoryDetector = null;
	    private bool showGUI = false;
	    protected Console()
	    {
	        this.fpsCounter = new FPSCounter();
	        this.memoryDetector = new MemoryDetector();

	        App.Instance.onUpdate += Update;
	        App.Instance.onGUI += OnGUI;
	    }

	    void Update()
	    {

	#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
	        if (Input.GetKeyUp(KeyCode.F1))
	            this.showGUI = !this.showGUI;
	#elif UNITY_ANDROID
	        if (Input.GetKeyUp(KeyCode.Escape))
	            this.showGUI = !this.showGUI;
	#elif UNITY_IOS
	        if (Input.GetKeyUp(KeyCode.Home))
	            this.showGUI = !this.showGUI;
	#endif

	        this.fpsCounter.Update();
	    }

	    void OnGUI()
	    {
	        if (!this.showGUI)
	            return;

	        this.fpsCounter.OnGUI();
	        this.memoryDetector.OnGUI();
	    }
	}

}
