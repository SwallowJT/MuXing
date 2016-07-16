using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMain : MonoBehaviour {
    public static GameMain Instance;

    private List<IUpdate> _UpdateList = new List<IUpdate>();

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }
	void Start () {
        Init();
	}

    void Init()
    {
        DataManager.Load();
        MySceneManager.Instance.Init(this.transform);
        GameFSM.Instance.Init();
        UIManager.Instance.Init(this.transform);
    }
	
	void Update () {
        for (int i = 0; i < _UpdateList.Count; i++)
        {
            _UpdateList[i].Update();
        }
	}

    public void AddUpdate(IUpdate update)
    {
        if (_UpdateList.Contains(update) == false)
        {
            _UpdateList.Add(update);
        }
    }

    public void RemoveUpdate(IUpdate update)
    {
        if (_UpdateList.Contains(update))
        {
            _UpdateList.Remove(update);
        }
    }
}
