using UnityEngine;
using System.Collections;

/// <summary>
/// UI层级
/// </summary>
public enum UILayer
{
    NONE = -1,
    UI = 0, //UI界面层
    ALERT = 200, //弹窗层
    TIPS = 300, //提示层
    TOP = 400, //顶层
}