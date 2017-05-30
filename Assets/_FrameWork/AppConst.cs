using UnityEngine;
using System.Collections;

namespace ZGameFrame
{
	public static class AppConst {

	    public static string DefalutLuaCode = "code";
	    public static string DefalutLuaFileSuffix = "lua";
	    public static string DefalutLuaRootPath = "assets/luaproject";
	    public static string DefalutAssetBundleRoot = "AssetBundles";

	    public static string AssetBundlePath
	    {
	        get
	        {
	            return string.Format("{0}/{1}",
	                                  Setting.GetInstance().GetString("AssetBundleRoot", AppConst.DefalutAssetBundleRoot),
	                                  Platform.GetPlatformName());
	        }
	    }

	    #region 线上资源目录相关
	    // 线上资源目录URL，带http://协议头
	    public static string OnlineAssetBundleURL
	    {
	        get
	        {
	            string addr = Setting.GetInstance().GetString("AssetBundleServerAddr");
	            if (addr == "")
	            {
	                return "";
	            }

	            if (addr == "streamingAssetsPath")
	            {
	                addr = Platform.GetStreamingAssetsPath();
	            }
	            return string.Format("{0}/{1}",
	                                 addr,
	                                 AppConst.AssetBundlePath);
	        }
	    }

	    public static string DefalutAssetBundleServerURL
	    {
	        get
	        {
	            return "file://" + Application.dataPath + "/..";
	        }
	    }
	    #endregion

	    #region 应用资源目录相关
	    // 应用资源目录URL，带不同平台的协议头
	    public static string AppAssetBundleURL
	    {
	        get
	        {
	            if (Application.isEditor)
	                return "file://" + AppAssetBundlePath;

	#if UNITY_ANDROID
	            return AppAssetBundlePath;
	#else
	            return "file://" + AppAssetBundlePath;
	#endif

	        }
	    }

	    public static string AppAssetBundlePath
	    {
	        get
	        {
	            return string.Format("{0}/{1}",
	                Application.streamingAssetsPath,
	                AppConst.AssetBundlePath);
	        }
	    }
	    #endregion

	    #region 设备更新目录
	    // 设备更新目录
	    public static string DeviceAssetBundlePath
	    {
	        get
	        {
	            return string.Format("{0}/{1}",
	                    Platform.GetDevicePresistentPath(),
	                    AppConst.AssetBundlePath);
	        }
	    }

	    // 设备更新目录URL，带file://协议头
	    public static string DeviceAssetBundleURL
	    {
	        get
	        {
	            if (Application.isEditor)
	                return "file://" + DeviceAssetBundlePath;

	#if UNITY_ANDROID
	            return "file://" + DeviceAssetBundlePath;
	#else
	            return "file://" + DeviceAssetBundlePath;
	#endif
	        }
	    }
	    #endregion

	}
}

