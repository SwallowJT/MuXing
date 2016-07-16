using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StateLogin : BaseState {
    public StateLogin(GameState state) : base(state) { }

    public override void OnEnter()
    {
        //SceneManager.LoadScene("Login");
        MySceneManager.Instance.ChangeScene("Login");
        UIManager.Instance.SendNotification("Login", ProtocolInfo.SHOW_VIEW, null);
    }

    public override void OnExit()
    {
        Debug.Log("State Login exit");
    }

    public override void OnComplete()
    {
        Debug.Log("StateLogin complete");
    }

    public override void Update()
    {
        Debug.Log("StateLogin Update");
    }
}
