using UnityEngine;
using System.Collections;

public class testFont : MonoBehaviour {

	void Start () {
        TextMesh tm = GetComponent<TextMesh>();
        tm.font = new Font("Arial");
	}
	
	void Update () {
	
	}
}
