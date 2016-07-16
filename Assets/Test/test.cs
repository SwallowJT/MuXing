using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
