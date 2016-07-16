using UnityEngine;
using System.Collections;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using LuaInterface;
using System.Collections.Generic;

public class BaseMediator : Mediator, IDispose {
    /// <summary>
    /// UI管理器
    /// </summary>
    protected BaseView _ViewManager;
    public BaseView ViewManager
    {
        set { _ViewManager = value; }
        get { return _ViewManager; }
    }

    /// <summary>
    /// 要监听的信息
    /// </summary>
    protected List<string> _MessageList;
    public List<string> MessageList
    {
        set { _MessageList = value; }
        get { return _MessageList; }
    }

    #region lua
    /// <summary>
    /// 注册信息
    /// </summary>
    const string _LuaOnRegisterName = "_OnRegister";
    protected LuaFunction _LuaOnRegister;
    /// <summary>
    /// 接收信息
    /// </summary>
    const string _LuaOnReceiveName = "_OnReceive";
    protected LuaFunction _LuaOnReceive;

    public bool IsInitLua = false;
    #endregion

    /// <summary>
    /// 是否正在加载界面
    /// </summary>
    bool _IsLoading = false;
    public bool IsLoading
    {
        set { _IsLoading = value; }
        get { return _IsLoading; }
    }

    public BaseMediator(string moduleName)
        : base(moduleName)
    {
        this.m_mediatorName = moduleName;
        _MessageList = new List<string>();
        RegisterAllNotification(); //注册监听常用的消息
        OnRegisterData(); //注册消息模块
        OnCheckDataProxy(); //检测是否有消息模块
    }

    /// <summary>
    /// 初始化Lua中注册和监听消息的函数，并调用注册函数
    /// </summary>
    public virtual void InitLuaMediator()
    {
        if (IsInitLua) return;

        string luaFileName = ModulesLib.Module2Lua[m_mediatorName];
        LuaScriptManager.DoFile(luaFileName);
        _LuaOnReceive = LuaScriptManager.Instance.GetFunction(luaFileName + _LuaOnReceiveName);
        if (_LuaOnReceive == null)
        {
            Debug.LogError(luaFileName + "该Lua文件中没有实现" + luaFileName + _LuaOnReceiveName);
        }
        _LuaOnRegister = LuaScriptManager.Instance.GetFunction(luaFileName + _LuaOnRegisterName);
        if (_LuaOnRegister == null)
        {
            Debug.LogError(luaFileName + "该Lua文件中没有实现" + luaFileName + _LuaOnRegisterName);
        }

        _LuaOnRegister.Call(this);
        IsInitLua = true;
    }

    /// <summary>
    /// 注册常用的消息类型
    /// </summary>
    protected virtual void RegisterAllNotification()
    {
        AddListenerType(ProtocolInfo.SHOW_VIEW);
        AddListenerType(ProtocolInfo.CLOSE_VIEW);
        AddListenerType(ProtocolInfo.CLEAR_DATA);
    }

    /// <summary>
    /// 增加监听类型
    /// </summary>
    /// <param name="infoName">消息名</param>
    public virtual void AddListenerType(string infoName)
    {
        if (_MessageList.Contains(infoName) == false)
        {
            MessageList.Add(infoName);
        }
    }

    /// <summary>
    /// 移除监听类型
    /// </summary>
    /// <param name="infoName">消息名</param>
    public virtual void RemoveListenerType(string infoName)
    {
        if (MessageList.Contains(infoName) == true)
        {
            MessageList.Remove(infoName);
            UIManager.Instance.MVCFacade.RemoveObserver(infoName, this);
        }
    }

    protected virtual void OnRegisterData()
    {

    }

    /// <summary>
    /// 检测该模块是否已经注册数据模块，每个模块必定携带数据模块
    /// （如果不想检测，就重载该方法）
    /// </summary>
    protected virtual void OnCheckDataProxy()
    {
        IProxy proxy = UIManager.Instance.MVCFacade.RetrieveProxy(m_mediatorName);
        if (proxy == null || proxy.Data == null)
        {
            Debug.LogError(m_mediatorName + "模块没有注册数据模块，请重载BaseMediator中的OnRegisterData方法");
        }
    }

    public override IList<string> ListNotificationInterests()
    {
        return _MessageList;
    }

    /// <summary>
    /// 处理消息
    /// </summary>
    /// <param name="notification">消息</param>
    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);
        switch (notification.Name)
        {
            case ProtocolInfo.SHOW_VIEW:
                if (_IsLoading) return;

                if (ViewManager == null)
                {
                    _IsLoading = true;
                    //TODO: 之后改为ResourceManager来加载
                    GameObject view = GameObject.Instantiate(Resources.Load("UI/Prefab/" + ModulesLib.Module2Resource[m_mediatorName])) as GameObject;
                    ViewManager = view.GetComponent<BaseView>();
                    ViewManager.Init(m_mediatorName);
                    
                    _IsLoading = false;
                }
                ViewManager.ShowView();
                break;
            case ProtocolInfo.CLOSE_VIEW:
                if (ViewManager != null)
                {
                    ViewManager.CloseView();
                }
                break;
            case ProtocolInfo.CLEAR_DATA:
                IProxy proxy = UIManager.Instance.MVCFacade.RetrieveProxy(m_mediatorName);
                if (proxy != null)
                {
                    proxy.Data.ClearData();
                }
                break;
        }

        if (_LuaOnReceive != null)
        {
            _LuaOnReceive.Call(notification.Name, notification.Body);
        }
    }

    public void Dispose()
    {
        UIManager.Instance.MVCFacade.RemoveMediator(m_mediatorName);
        if (ViewManager != null)
        {
            UIManager.Instance.Remove(ViewManager.gameObject);
            GameObject.DestroyImmediate(ViewManager.gameObject);
        }
        IData data = UIManager.Instance.GetDataModelByModuleName(m_mediatorName);
        if (data != null)
        {
            data.ClearData();
        }
        ViewManager = null;
        _LuaOnReceive = null;
        _LuaOnRegister = null;
    }



    public void PrintMessageList()
    {
        Debug.Log("PrintMessageList");
        foreach (string info in _MessageList)
        {
            Debug.Log("info = " + info);
        }
    }
}
