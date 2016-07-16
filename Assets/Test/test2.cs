using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class test2 : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

    public void Change()
    {
        SceneManager.LoadScene("Start");
    }
}
