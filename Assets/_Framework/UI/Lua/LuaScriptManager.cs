using UnityEngine;
using System.Collections;
using LuaInterface;

public class LuaScriptManager {
    private static LuaState _Instance;

    public static LuaState Instance
    {
        get
        {
            if (_Instance == null)
            {
                Init();
            }
            return _Instance;
        }
    }

    public static void Init()
    {
        if (_Instance != null) return;

        _Instance = new LuaState();
        DoFile("InitLua");
        DoFile("Tool");
        DoFile("Global");
        DoFile("CMD");
    }

    public static void DoFile(string luaName)
    {
        string dir = Application.dataPath + "/RawRes" + "/Lua/";
        string path = dir + luaName + ".txt";
        _Instance.DoFile(path); 
    }
}

/*
public class LuaScriptManager
{
    private static LuaScriptMgr _Instance;

    public static LuaScriptMgr Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new LuaScriptMgr();
                _Instance.Start();
                _Instance.DoFile("global", false);
                _Instance.DoFile("game_ui_info_lib", false);
                _Instance.DoFile("gameCMDS", false);
            }
            return _Instance;
        }
    }
}
*/