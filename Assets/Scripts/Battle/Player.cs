using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float ForwardSpeed = 1;
    public float UpDownSpeed = 1;
    public float QuickSpeed = 15;
    public Transform UpBorder; //上边界
    public Transform DownBorder; //下边界

    private bool _IsMovingUp;
    private bool _IsSpeedUp;

	// Use this for initialization
	void Start () {
        _IsMovingUp = true;
        _IsSpeedUp = false;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = this.transform.localPosition;
        pos.x += ForwardSpeed * Time.deltaTime;
        this.transform.localPosition = pos;

        if(this.transform.position.y <= DownBorder.position.y) 
            _IsSpeedUp = false;

        if (!_IsSpeedUp)
            MoveUpAndDown();
        else
            MoveFastDown();

        HandleInput();
	}

    /// <summary>
    /// 自动上下移动
    /// </summary>
    void MoveUpAndDown()
    {
        Vector3 pos = this.transform.position;

        if (pos.y >= UpBorder.position.y)
            _IsMovingUp = false;
        if (pos.y <= DownBorder.position.y)
            _IsMovingUp = true;

        float deltaY = _IsMovingUp ? UpDownSpeed : -UpDownSpeed;
        pos.y += deltaY * Time.deltaTime;
        this.transform.position = pos;
    }

    /// <summary>
    /// 处理玩家输入
    /// </summary>
    void HandleInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = this.transform.position;
            if(pos.y > DownBorder.position.y)
            {
                _IsSpeedUp = true;
            }
            else
            {
                _IsSpeedUp = false;
            }
        }
    }

    /// <summary>
    /// 加速下降
    /// </summary>
    void MoveFastDown()
    {
        Vector3 pos = this.transform.position;
        pos.y -= Time.deltaTime * QuickSpeed;
        this.transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D collidedObject)
    {
        Debug.Log("Player Collider");
    }
}
