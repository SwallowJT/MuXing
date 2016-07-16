using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FXManager : MonoBehaviour {
    public Dictionary<string, ParticleSystem> Name2Fx;

	// Use this for initialization
	void Start () {
        Name2Fx = new Dictionary<string, ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
