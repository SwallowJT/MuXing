using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public Transform Target;
    public float Target2LeftBorder = 7;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Target != null)
        {
            Vector3 targetPos = Target.localPosition;
            Vector3 pos = this.transform.localPosition;
            pos.x = targetPos.x + Target2LeftBorder;
            this.transform.localPosition = pos;
        }
	}
}
