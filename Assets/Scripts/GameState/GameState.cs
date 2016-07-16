using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏状态
/// </summary>
public enum GameState {
    EnterGame = 1, //进入游戏
    InitSDK, //初始化SDK
    CheckPacketUpdate, //检测包的更新
    CheckResourceUpdate, //检测资源更新
    LoadResource, //加载游戏资源
    Login, //登录
    Main, //进入游戏主界面
    Battle, //战斗
}
