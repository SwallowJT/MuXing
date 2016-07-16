using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StateEnterGame : BaseState {
    GameObject _ProgressBar;
    public StateEnterGame(GameState state) : base(state) { }

    public override void OnEnter()
    {
        _ProgressBar = GameObject.FindGameObjectWithTag("Loading");
    }

    public override void OnExit()
    {
        //TODO
        GameObject LoadingView = _ProgressBar.transform.parent.transform.parent.gameObject;
        LoadingView.SetActive(false);
    }

    public override void OnComplete()
    {
        Debug.Log("StateEnterGame complete");
    }

    float Timer = 0;
    public override void Update()
    {
        //测试：用3秒模拟加载资源等
        const float waitTime = 1;
        Timer += Time.deltaTime;
        float x = Mathf.Lerp(0, 1, Timer / waitTime);
        _ProgressBar.transform.localScale = new Vector3(x, 1, 1);

        if (Timer > waitTime)
        {
            //GameFSM.Instance.ChangeState(GameState.Login);
            GameFSM.Instance.ChangeState(GameState.Battle);
        }
    }
}
