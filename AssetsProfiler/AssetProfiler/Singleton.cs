using UnityEngine;
using System.Collections;
using System;

public abstract class Singleton<T>
{

    private static T _instance;
    public static T Instance { get { return _instance; } }

    public static T CreateInstance(params object[] args)
    {
        if (_instance != null)
        {
            DestoryInstance();
        }
        _instance = (T)Activator.CreateInstance(typeof(T), args);
        return Instance;
    }
    public static T CreateInstance()
    {
        if (_instance != null)
        {
            DestoryInstance();
        }
        _instance = Activator.CreateInstance<T>();
        return Instance;
    }

    public static void DestoryInstance()
    {
        if (_instance == null)
        {
            throw new InvalidOperationException(typeof(T).ToString() + "is not Create before Destory");
        }
        (_instance as Singleton<T>).Dispose();
        _instance = default(T);
    }

    public abstract void Dispose();
}
