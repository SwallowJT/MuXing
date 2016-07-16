using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 单例基类（用于简化单例的编写）
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : Singleton<T> {
    protected static T _Instance = null;
    protected Singleton() { }

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                if (ctor == null)
                    throw new Exception("Non-Public ctor found");
                _Instance = ctor.Invoke(null) as T;
            }
            return _Instance;
        }
    }
}
