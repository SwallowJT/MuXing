using UnityEngine;
using System.Collections;

public class StateBattle : BaseState {
    public StateBattle(GameState state) : base(state) { }
    public override void OnEnter()
    {
        MySceneManager.Instance.ChangeScene("Battle");
        UIManager.Instance.SendNotification(ModulesLib.BATTLE, ProtocolInfo.SHOW_VIEW, null);
    }

    public override void OnComplete()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void Update()
    {
        
    }
}
