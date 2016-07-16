using UnityEngine;
using System.Collections;
using LuaInterface;

public class testLua : MonoBehaviour {

	void Start () {
        LuaState l = new LuaState();
        string path = Application.dataPath + "/RawRes/Lua/test.txt";
        l.DoFile(path);
	}
	
	void Update () {
	
	}
}
