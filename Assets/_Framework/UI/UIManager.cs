using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region 单例
    private static UIManager _Instance;

    public static UIManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject("UIManager");
                DontDestroyOnLoad(obj);
                _Instance = obj.AddComponent<UIManager>();
            }
            return _Instance;
        }
    }
    #endregion

    /// <summary>
    /// UI根目录
    /// </summary>
    private Transform _UIRoot;
    public Transform UIRoot
    {
        set { _UIRoot = value; }
        get { return _UIRoot; }
    }

    /// <summary>
    /// MVC的外观模式
    /// </summary>
    private IFacade _MVCFacade;
    public IFacade MVCFacade
    {
        set { _MVCFacade = value; }
        get { return _MVCFacade; }
    }

    #region 层级
    /// <summary>
    /// UI场景层级
    /// </summary>
    private Dictionary<string, Transform> UISceneLayers;

    /// <summary>
    /// UI层级对应的深度
    /// </summary>
    private Dictionary<UILayer, int> Layer2Depth;

    /// <summary>
    /// UI层级对应的UI列表
    /// </summary>
    private Dictionary<UILayer, List<RectTransform>> Layer2UIList;
    #endregion

    #region 消息
    public class UINotification
    {
        public Notification Notif;
        public float DelayTime;
        public float StartTime;
    }

    /// <summary>
    /// 消息队列
    /// </summary>
    private List<UINotification> NotificationQueue;
    /// <summary>
    /// 需要移除的消息
    /// </summary>
    private List<UINotification> NotificationToRemove;

    #endregion

    public void Init(Transform root)
    {
        ModulesLib.Init();
        LuaScriptManager.Init();

        this.transform.parent = root;

        UIRoot = GameObject.Find(GameConfig.UI_ROOT).transform;
        DontDestroyOnLoad(UIRoot);
        MVCFacade = Facade.Instance;

        UISceneLayers = new Dictionary<string, Transform>();
        NotificationQueue = new List<UINotification>();
        NotificationToRemove = new List<UINotification>();
        Layer2Depth = new Dictionary<UILayer, int>();
        Layer2UIList = new Dictionary<UILayer, List<RectTransform>>();

        InitUILayer();
        InitMediator();
        InitCommand();

        StartCoroutine(UpdateNotification());
    }

    /// <summary>
    /// 初始化UI层级和深度
    /// </summary>
    private void InitUILayer()
    {
        //Scene[] scenes = SceneManager.GetAllScenes();
        //foreach (Scene scene in scenes)
        //{
        //    CreateSceneUILayer(scene.name);
        //}

        foreach (string scene in MySceneManager.Instance.SceneList)
        {
            CreateSceneUILayer(scene);
        }

        UILayer[] layers = (UILayer[])System.Enum.GetValues(typeof(UILayer));
        foreach (UILayer layer in layers)
        {
            Layer2Depth.Add(layer, (int)layer);
        }
    }

    /// <summary>
    /// 创建场景对应的画布
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    public void CreateSceneUILayer(string sceneName)
    {
        Debug.Log("CreateSceneUILayer");
        if (UISceneLayers.ContainsKey(sceneName) == false)
        {
            GameObject UILayer = Resources.Load("UI/Prefab/UICanvas") as GameObject;
            UILayer = GameObject.Instantiate(UILayer);
            UILayer.name = sceneName;
            UILayer.layer = LayerMask.NameToLayer("UI");
            UILayer.transform.parent = UIRoot;
            UILayer.transform.localPosition = new Vector3(0, 0, 0);
            UILayer.transform.localScale = new Vector3(1, 1, 1);

            UISceneLayers.Add(sceneName, UILayer.transform);
        }

        RefreshSceneUILayer(MySceneManager.Instance.CurScene);
    }

    public void RefreshSceneUILayer(string name)
    {
        foreach (Transform t in UISceneLayers.Values)
        {
            if (t.name != name)
            {
                t.gameObject.SetActive(false);
                Debug.Log(t.name + " set false");
            }
            else
            {
                t.gameObject.SetActive(true);
                Debug.Log(t.name + " set true");
            }
        }
    }

    /// <summary>
    /// 添加界面
    /// </summary>
    /// <param name="view">界面Prefab</param>
    /// <param name="layer">层级</param>
    public void Add(GameObject view, UILayer layer)
    {
        if (Layer2UIList.ContainsKey(layer) == false)
            Layer2UIList.Add(layer, new List<RectTransform>());

        view.SetActive(true);
        RectTransform rect = view.GetComponent<RectTransform>();
        if (Layer2UIList[layer].Contains(rect) == false)
        {
            Layer2UIList[layer].Add(rect);

            view.layer = LayerMask.NameToLayer("UI");
            view.transform.parent = UISceneLayers[MySceneManager.Instance.CurScene];

            rect.SetSiblingIndex(Layer2Depth[layer]);

            Layer2Depth[layer]++;
        }
        //换到同一层级中的最高级
        else
        {
            int nowIndex = rect.GetSiblingIndex();
            if (nowIndex + 1 >= Layer2Depth[layer]) return;

            Layer2UIList[layer].Remove(rect);
            Layer2UIList[layer].Add(rect);
            int index = (int)layer;
            foreach (RectTransform item in Layer2UIList[layer])
            {
                item.SetSiblingIndex(index);
                index++;
            }
        }
    }

    /// <summary>
    /// 移除界面
    /// </summary>
    /// <param name="view">界面Prefab</param>
    public void Remove(GameObject view)
    {
        view.gameObject.SetActive(false);

        RectTransform rect = view.GetComponent<RectTransform>();
        UILayer curLayer = UILayer.NONE;
        foreach (UILayer layer in Layer2UIList.Keys)
        {
            if (Layer2UIList[layer].Contains(rect))
            {
                curLayer = layer;
                break;
            }
        }

        if (curLayer != UILayer.NONE)
        {
            Layer2Depth[curLayer]--;
            Layer2UIList[curLayer].Remove(rect);
            int index = (int)curLayer;
            foreach (RectTransform item in Layer2UIList[curLayer])
            {
                item.SetSiblingIndex(index);
                index++;
            }
        }
    }

    private void InitMediator()
    {
        MediatorRegister.Register();
    }

    private void InitCommand()
    {
        //RegisterCommand(ProtocolInfo.GAME_START, typeof(GameStartCommand));
    }

    public IData GetDataModelByModuleName(string moduleName)
    {
        IProxy proxy = MVCFacade.RetrieveProxy(moduleName);
        if (proxy != null)
        {
            return proxy.Data;
        }
        return null;
    }

    #region Register
    public void RegisterMediator(BaseMediator mediator, bool isLoadLua = false)
    {
        MVCFacade.RegisterMediator(mediator);
        if (isLoadLua)
        {
            mediator.InitLuaMediator();
        }
    }

    public void RegisterCommand(string infoName, System.Type command)
    {
        MVCFacade.RegisterCommand(infoName, command);
    }

    public void RegisterModel(string moduleName, IData data)
    {
        MVCFacade.RegisterProxy(new Proxy(moduleName, data));
    }
    #endregion

    #region 消息
    /// <summary>
    /// 每帧处理消息
    /// </summary>
    IEnumerator UpdateNotification()
    {
        while (true)
        {
            int count = NotificationQueue.Count;
            for (int index = 0; index < count; index++)
            {
                UINotification item = NotificationQueue[index];
                //达到延迟时间才处理消息
                float passedTime = Time.time - item.StartTime;
                if (passedTime >= item.DelayTime)
                {
                    NotificationToRemove.Add(item);
                    Notification notif = item.Notif;
                    MVCFacade.SendNotification(notif.ModuleName, notif.Name, notif.Body, notif.Type);
                }
            }
            //处理完的消息放入移除列表
            if (NotificationToRemove.Count > 0)
            {
                count = NotificationToRemove.Count;
                for (int i = 0; i < count; i++)
                {
                    NotificationQueue.Remove(NotificationToRemove[i]);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// 向特定模块发送消息
    /// </summary>
    /// <param name="moduleName">模块名</param>
    /// <param name="infoName">消息名</param>
    /// <param name="data">数据</param>
    /// <param name="delayTime">延时（单位是秒）</param>
    public void SendNotification(string moduleName, string infoName, object data, float delayTime = 0)
    {
        Notification notif = new Notification(moduleName, infoName, data, null);
        UINotification UINotif = new UINotification();
        UINotif.DelayTime = delayTime;
        UINotif.StartTime = Time.time;
        UINotif.Notif = notif;
        NotificationQueue.Add(UINotif);
    }

    /// <summary>
    /// 广播消息
    /// </summary>
    public void Broadcast(string infoName, object data, float delayTime = 0)
    {
        foreach (string moduleName in ModulesLib.Module2Lua.Keys)
        {
            SendNotification(moduleName, infoName, data, delayTime);
        }
    }
    #endregion
}
