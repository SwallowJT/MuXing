using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏状态的基类
/// </summary>
public abstract class BaseState : IUpdate {
    public GameState State;

    public BaseState(GameState state)
    {
        State = state;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// 退出状态
    /// </summary>
    public abstract void OnExit();

    /// <summary>
    /// 完成状态
    /// </summary>
    public abstract void OnComplete();

    public abstract void Update();
}
