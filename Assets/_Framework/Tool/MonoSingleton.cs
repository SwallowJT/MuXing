using UnityEngine;
using System.Collections;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
    protected static T _Instace = null;
    public static T Instance
    {
        get
        {
            if (_Instace == null)
            {
                _Instace = FindObjectOfType<T>();
                //是否有多个实例
                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.LogError("More than one MonoSingleton");
                    return _Instace;
                }

                if (_Instace == null)
                {
                    string name = typeof(T).Name;
                    GameObject go = GameObject.Find(name);
                    if (go == null)
                        go = new GameObject(name);
                    _Instace = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }

            return _Instace;
        }
    }

    protected virtual void OnDestroy()
    {
        _Instace = null;
    }
}
