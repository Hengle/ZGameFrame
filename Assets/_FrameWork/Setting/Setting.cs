using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace ZGameFrame
{
	public class Setting : Singleton<Setting>
	{
	    Dictionary<string, string> settings = new Dictionary<string, string>();

	    public static Setting GetInstance()
	    {
	        return Instance;
	    }

	    public IEnumerator Init()
	    {
	        string addr = (Resources.Load<TextAsset>("SettingAddr")).text;
	        addr = addr.Trim();
	        string text;
	        // 存在setting地址，就用setting地址里的配置
	        Debug.Log("addr: " + addr);
	        if (!string.IsNullOrEmpty(addr))
	        {
	            string settingAddr = string.Format("{0}/{1}", addr, "Setting");
	            WWW www = new WWW(settingAddr);
	            yield return www;
	            if (www.error != "")
	            {
	                Debug.LogError(string.Format("setting addr error, {0}", settingAddr));
	                yield return null;
	            }
	            text = www.text;
	        } 
	        else
	        {
	            text = (Resources.Load<TextAsset>("Setting")).text;
	        }
	        Debug.Log("setting text: " + text);
	        Parse(text);
	        yield return null;
	    }

	    public void InitEditor()
	    {
	        string text = (Resources.Load<TextAsset>("Setting")).text;
	        Debug.Log("setting text: " + text);
	        Parse(text);
	    }

	    void Parse(string str)
	    {
	        // 解析配置
	        // 配置格式：key=value
	        string[] lines = str.Split('\n');
	        for (int i = 0; i < lines.Length; ++i)
	        {
	            string line = lines[i].Trim();
	            if (line[0] == ';') continue;

	            string[] kv = line.Split('=');
	            Log.Assert(kv.Length == 2, "Error Setting Format : " + line);
	            string k = kv[0].Trim();
	            string v = kv[1].Trim();
	            settings[k] = v;
	        }
	    }

	    #region get set 方法
	    public int GetInt(string key, int defaultVal = 0)
	    {
	        string val = null;
	        if (settings.TryGetValue(key, out val))
	            return System.Convert.ToInt32(val);
	        settings[key] = defaultVal.ToString();
	        return defaultVal;
	    }

	    public float GetFloat(string key, float defaultVal = 0f)
	    {
	        string val = null;
	        if (settings.TryGetValue(key, out val))
	            return System.Convert.ToSingle(val);
	        settings[key] = defaultVal.ToString();
	        return defaultVal;
	    }

	    public bool GetBool(string key, bool defaultVal = false)
	    {
	        string val = null;
	        if (settings.TryGetValue(key, out val))
	            return System.Convert.ToBoolean(val);
	        settings[key] = defaultVal.ToString();
	        return defaultVal;
	    }

	    public string GetString(string key, string defaultVal = "")
	    {
	        string val = null;
	        if (settings.TryGetValue(key, out val))
	            return val.ToString();
	        settings[key] = defaultVal.ToString();
	        return defaultVal;
	    }

	    public void SetInt(string key, int val)
	    {
	        settings[key] = val.ToString();
	    }

	    public void SetFloat(string key, float val)
	    {
	        settings[key] = val.ToString();
	    }

	    public void SetBool(string key, bool val)
	    {
	        settings[key] = val.ToString();
	    }

	    public void SetString(string key, string val)
	    {
	        settings[key] = val;
	    }
	    #endregion
	}
}

