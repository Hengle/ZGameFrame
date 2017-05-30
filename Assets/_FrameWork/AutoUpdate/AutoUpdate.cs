using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ZGameFrame
{
	public class AutoUpdate : CachedMonoBehaviour
	{
		Transform m_UIRoot;

		Transform m_UIAutoUpdate;
		Transform m_ProcessTrans;
		Transform m_IndicatorTrans;
		Transform m_HintTrans;

		Transform m_UIMessageNotice;

		void Awake()
		{
			m_UIRoot = GameObject.Find("UIRoot").transform;
			m_UIAutoUpdate = m_UIRoot.Find("NormalRoot/UIAutoUpdate");
			m_ProcessTrans = m_UIAutoUpdate.Find("process");
			m_IndicatorTrans = m_UIAutoUpdate.Find("indicator");
			m_HintTrans = m_UIAutoUpdate.Find("hint");

			m_UIMessageNotice = m_UIRoot.Find("PopupRoot/UIMessageNotice");	
			m_UIMessageNotice.gameObject.SetActive(false);
		}

	    public IEnumerator StartUpdate()
	    {
	        string deviceAssetBundlePath = AppConst.DeviceAssetBundlePath;

	        //设备ab目录不存在, 第一次启动程序
	        if (!Directory.Exists(deviceAssetBundlePath))
	        {
	            Directory.CreateDirectory(deviceAssetBundlePath);
	        }

	        string onlineAssetBundleURL = AppConst.OnlineAssetBundleURL;
	        if (onlineAssetBundleURL != "")
	        {
	            yield return App.GetInstance().StartCoroutine(
	                        UpdateAB(onlineAssetBundleURL,
	                                AppConst.DeviceAssetBundleURL,
	                                delegate ()
	                                {
										SetHint("Update success!");
										SetProgress(1);
	                                },
	                                delegate (float progress, string abName)
	                                {
										SetHint("Update " + abName);
										SetProgress(progress);
	                                },
	                                delegate (string error)
	                                {
										m_UIAutoUpdate.gameObject.SetActive(false);
										m_UIMessageNotice.gameObject.SetActive(true);
										m_UIMessageNotice.Find("text").GetComponent<Text>().text = error;
	                                }));
	        }
	        else
	        {
				SetProgress(1);
				SetHint("Update client success!");

				yield return new WaitForSeconds(1);
				m_UIAutoUpdate.gameObject.SetActive(false);
	        }

	    }
			

		void SetProgress(float progress)
		{
			Image img = m_ProcessTrans.GetComponent<Image>();
			img.fillAmount = progress;
			Debug.Log(string.Format("set progress: {0}", progress));
			string text = string.Format("{0:00}%", progress * 100);
			SetLabel(text);
		}

		void SetLabel(string value)
		{
			m_IndicatorTrans.GetComponent<Text>().text = value;
		}

		void SetHint(string value)
		{
			m_HintTrans.GetComponent<Text>().text = value;
		}

	    string SplitURL(string url)
	    {
	        int index = url.IndexOf("//");
	        Debug.Assert(index != -1, "split url error, url: " + url);
	        string str = url.Substring(index + 2);
	        //Debug.Log(string.Format("url: {0}, x:{1}, str:{2}", url, index, str));
	        return str;
	    }
	    /// <summary>
	    /// 从源目录中同步资源到目的目录
	    /// </summary>
	    /// <returns></returns>
	    IEnumerator UpdateAB(string srcURL,
	                         string desURL,
	                         Action onSuccAction,
	                         Action<float, string> onProgressAction,
	                         Action<string> onFailAction)
	    {

	        string srcPath = SplitURL(srcURL);
	        string desPath = SplitURL(desURL);

	        Debug.Log(string.Format("UpdateAB, src:{0}, des:{1}, srcPath:{2}，desPath:{3}", srcURL, desURL, srcPath, desPath));

	        string platformName = Platform.GetPlatformName();
	        string srcAssetBundleURL = string.Format("{0}/{1}", srcURL, platformName);
	        WWW www = new WWW(srcAssetBundleURL);
	        yield return www;
	        if (www.error != null)
	        {
	            Debug.Log("srcUrl assetbundlemainfest is null, error: " + www.error + " url:" + srcAssetBundleURL);
	            onFailAction(www.error);
	            yield return null;
	        }

	        byte[] srcAssetBundleContent = www.bytes;
	        AssetBundleManifest srcManifest = www.assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
	        www.assetBundle.Unload(false);

	        // 下载AssetBundleManifest  
	        string srcAssetBundleManifestURL = string.Format("{0}/{1}.manifest", srcURL, platformName);
	        www = new WWW(srcAssetBundleManifestURL);
	        yield return www;
	        byte[] srcAssetBundleManifestContent = www.bytes;

	        List<string> downloadAssetBundleList = new List<string>();

	        string desAssetBundleURL = string.Format("{0}/{1}", desURL, platformName);
	        www = new WWW(desAssetBundleURL);
	        yield return www;
	        if (www.error != null)
	        {
	            // 如果目标目录不存在manifest文件就下载源目录的所有文件
	            string[] srcAssetBundles = srcManifest.GetAllAssetBundles();
	            for (int i = 0; i < srcAssetBundles.Length; ++i)
	            {
	                downloadAssetBundleList.Add(srcAssetBundles[i]);
	            }
	        }
	        else
	        {
	            AssetBundleManifest desManifest = www.assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
	            www.assetBundle.Unload(false);

	            string[] srcAssetBundles = srcManifest.GetAllAssetBundles();
	            for (int i = 0; i < srcAssetBundles.Length; ++i)
	            {
	                string srcAssetBundle = srcAssetBundles[i];
	                if (!File.Exists(desPath + srcAssetBundle))
	                {
	                    downloadAssetBundleList.Add(srcAssetBundle);
	                }
	                else
	                {
	                    Hash128 desAssetBundleHash = desManifest.GetAssetBundleHash(srcAssetBundle);
	                    Hash128 srcAssetBundleHash = srcManifest.GetAssetBundleHash(srcAssetBundle);
	                    if (!desAssetBundleHash.Equals(srcAssetBundleHash))
	                        downloadAssetBundleList.Add(srcAssetBundle);
	                }
	            }
	        }

	        string assetbundlePath;
	        for (int i = 0; i < downloadAssetBundleList.Count; ++i)
	        {
	            string assetbundle = downloadAssetBundleList[i];

	            float progress = (i / (float)downloadAssetBundleList.Count);
	            onProgressAction(progress, assetbundle);

	            srcAssetBundleURL = string.Format("{0}/{1}", srcURL, assetbundle);
	            www = new WWW(srcAssetBundleURL);
	            yield return www;

	            assetbundlePath = string.Format("{0}/{1}", desPath, assetbundle);
	            FileManager.CreateFileWithBytes(assetbundlePath, www.bytes, www.bytes.Length);

	            www = new WWW(string.Format("{0}/{1}.manifest", srcURL, assetbundle));
	            yield return www;

	            string manifestPath = string.Format("{0}/{1}.manifest", desPath, assetbundle);
	            FileManager.CreateFileWithBytes(manifestPath, www.bytes, www.bytes.Length);
	        }

	        assetbundlePath = string.Format("{0}/{1}", desPath, platformName);
	        FileManager.CreateFileWithBytes(assetbundlePath, srcAssetBundleContent, srcAssetBundleContent.Length);

	        string assetbundleManifestPath = string.Format("{0}/{1}.manifest", desPath, platformName);
	        FileManager.CreateFileWithBytes(assetbundleManifestPath, srcAssetBundleManifestContent, srcAssetBundleManifestContent.Length);

	        www.Dispose();
	        www = null;

	        onSuccAction();

			yield return Resources.UnloadUnusedAssets();

			yield return new WaitForEndOfFrame();
			m_UIAutoUpdate.gameObject.SetActive(false);
	    }
	}
}
