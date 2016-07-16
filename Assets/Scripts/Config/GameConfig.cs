using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// 游戏的一些全局设置
/// </summary>
public class GameConfig {
    public const string UI_ROOT = "UIRoot";
    public static string UI_MODULES()
    {
        return Application.dataPath + "/Scripts/UIModules/";
    }
    public static string ModulesLib_path()
    {
        return Application.dataPath + "/_Framework/UI/ModulesLib.cs";
    }
    public static string MediatorRegister_path()
    {
        return Application.dataPath + "/_Framework/UI/MVC/MediatorRegister.cs";
    }
    public static string UI_path()
    {
        return Application.dataPath + "/RawRes/UI";
    }
    public static string Lua_path()
    {
        return Application.dataPath + "/RawRes/Lua";
    }
    public static string Atlas_path()
    {
        return Application.dataPath + "/RawRes/Atlas";
    }
    public static string AtlasConfig_path()
    {
        return Application.streamingAssetsPath + "/AtlasConfig.txt";
    }






    public const string LUA_AB = "Lua";

    public static string LuaAB_path()
    {
        return Application.streamingAssetsPath + "/Lua";
    }

    public static string CSharpAB_path()
    {
        return Application.streamingAssetsPath + "/CSharp";
    }






    public static string[] GetPrefabPaths()
    {
        string[] atlasPaths = Directory.GetFiles(Atlas_path(), "*.prefab");
        string[] uiPaths = Directory.GetFiles(UI_path(), "*.prefab");
        string[] paths = new string[atlasPaths.Length + uiPaths.Length];

        int index = 0;
        foreach (string path in atlasPaths)
        {
            paths[index] = GameTool.Absolute2Relative(path);
            index++;
        }
        foreach (string path in uiPaths)
        {
            paths[index] = GameTool.Absolute2Relative(path);
            index++;
        }

        return paths;
    }
}
