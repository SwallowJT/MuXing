using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
    public ParticleSystem FXBoom;

    private bool _ShouldDestory;
    public float _TimeToLive = 0.5f; //存活时间(秒)

	// Use this for initialization
	void Start () {
        FXBoom.Stop();
        _ShouldDestory = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(_ShouldDestory)
        {
            _TimeToLive -= Time.deltaTime;
            if (_TimeToLive <= 0)
                this.gameObject.SetActive(false);
        }
	}

    void OnTriggerEnter2D(Collider2D collidedObject)
    {
        Debug.Log("Obstacle Collider");
        FXBoom.Play();
        _ShouldDestory = true;
        //this.gameObject.SetActive(false);
    }
}
