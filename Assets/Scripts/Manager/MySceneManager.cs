using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    #region 单例
    private static MySceneManager _Instance;

    public static MySceneManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject("SceneManager");
                DontDestroyOnLoad(obj);
                _Instance = obj.AddComponent<MySceneManager>();
            }
            return _Instance;
        }
    }
    #endregion

    /// <summary>
    /// 统计进入场景次数
    /// </summary>
    private static Dictionary<string, int> EnterSceneCount;

    /// <summary>
    /// 当前场景
    /// </summary>
    public string CurScene;

    public List<string> SceneList;

    public void Init(Transform root)
    {
        GameObject go = GameObject.Find("SceneManager");
        go.transform.parent = root;
        EnterSceneCount = new Dictionary<string, int>();
        _Instance.CurScene = "Load";
        SceneList = new List<string>();
        SceneList.Add("Load");
        SceneList.Add("Login");
        SceneList.Add("Battle");
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
        CurScene = name;
        UIManager.Instance.RefreshSceneUILayer(name);
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
