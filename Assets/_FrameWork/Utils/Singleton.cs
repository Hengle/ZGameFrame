using UnityEngine;
using System;
using System.Threading;

public class Singleton<T> where T : class, new()
{
    protected static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Singleon: " + typeof(T) + " id: " + Thread.CurrentThread.ManagedThreadId.ToString());
                _instance = new T();
            }
            return _instance;
        }
    }
}
