using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFSM : Singleton<GameFSM>, IUpdate {
    /// <summary>
    /// 当前游戏的状态
    /// </summary>
    public GameState State = GameState.EnterGame;

    private Dictionary<GameState, BaseState> _StateDic;

    private GameFSM() { }

    public void Init()
    {
        _StateDic = new Dictionary<GameState, BaseState>();
        _StateDic.Add(GameState.EnterGame, new StateEnterGame(GameState.EnterGame));
        _StateDic.Add(GameState.Login, new StateLogin(GameState.Login));
        _StateDic.Add(GameState.Battle, new StateBattle(GameState.Battle));
        //添加其他State
        //....
        _StateDic[State].OnEnter();
        GameMain.Instance.AddUpdate(this);
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    public void ChangeState(GameState newState)
    {
        if (State != newState)
        {
            _StateDic[State].OnExit();
        }
        State = newState;
        _StateDic[State].OnEnter();
    }

    public void Update()
    {
        _StateDic[State].Update();
    }
}
