using UnityEngine;
using System.Collections;
using LuaInterface;

/// <summary>
/// 管理UI显示
/// 工作：（1）界面初始化（2）打开界面（3）关闭界面
/// </summary>
public class BaseView : MonoBehaviour
{
    /// <summary>
    /// 模块名
    /// </summary>
    string _ModuleName;
    public string ModuleName
    {
        get { return _ModuleName; }
        set { _ModuleName = value; }
    }
    /// <summary>
    /// 是否显示界面
    /// </summary>
    bool _IsShow = false;
    public bool IsShow
    {
        get { return _IsShow; }
        set { _IsShow = value; }
    }
    /// <summary>
    /// UI所在的层级
    /// </summary>
    protected UILayer _UILayer = UILayer.UI;

    #region Lua
    /// <summary>
    /// 第一次加载界面时调用
    /// </summary>
    const string _LuaOnInitName = "_OnInit";
    LuaFunction _LuaOnInit;
    /// <summary>
    /// 显示界面时调用
    /// </summary>
    const string _LuaOnShowViewName = "_OnShowView";
    LuaFunction _LuaOnShowView;
    /// <summary>
    /// 关闭界面时调用
    /// </summary>
    const string _LuaOnCloseViewName = "_OnCloseView";
    LuaFunction _LuaOnCloseView;
    #endregion

    /// <summary>
    /// 初始化界面
    /// </summary>
    /// <param name="name"></param>
    public virtual void Init(string name)
    {
        _ModuleName = name;

        Transform view = this.transform;
        view.name = ModulesLib.Module2Resource[ModuleName];
        view.localScale = new Vector3(1, 1, 1);
        UIManager.Instance.Add(view.gameObject, _UILayer);

        string luaFileName = ModulesLib.Module2Lua[ModuleName];
        _LuaOnInit = LuaScriptManager.Instance.GetFunction(luaFileName + _LuaOnInitName);
        _LuaOnShowView = LuaScriptManager.Instance.GetFunction(luaFileName + _LuaOnShowViewName);
        _LuaOnCloseView = LuaScriptManager.Instance.GetFunction(luaFileName + _LuaOnCloseViewName);
        if (_LuaOnInit != null)
        {
            _LuaOnInit.Call(this);
        }
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    public virtual void ShowView()
    {
        _IsShow = true;
        if (_LuaOnShowView != null)
        {
            _LuaOnShowView.Call();
        }
        UIManager.Instance.Add(transform.gameObject, _UILayer);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public virtual void CloseView()
    {
        _IsShow = false;
        if (_LuaOnCloseView != null)
        {
            _LuaOnCloseView.Call();
        }
        UIManager.Instance.Remove(transform.gameObject);
    }
}
