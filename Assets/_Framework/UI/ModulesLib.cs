using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 整个游戏所有模块库
/// </summary>
public class ModulesLib {
    /// <summary>
    /// 模块名对应资源名
    /// </summary>
    public static Dictionary<string, string> Module2Resource = new Dictionary<string, string>();

    /// <summary>
    /// 模块名对应Lua名
    /// </summary>
    public static Dictionary<string, string> Module2Lua = new Dictionary<string, string>();

    #region 模块名
    //----For CreateMVC 1
	public const string BATTLE = "Battle";
	public const string MAIN = "Main";
	public const string LOGIN = "Login";
    #endregion


    public static void Init()
    {
        InitResource();
        InitLua();
    }

    /// <summary>
    /// 映射模块名和资源名
    /// </summary>
    private static void InitResource()
    {
        //----For CreateMVC 2
		Module2Resource[BATTLE] = "BattleView";
		Module2Resource[MAIN] = "Main";
		Module2Resource[LOGIN] = "Login";
    }

    /// <summary>
    /// 映射模块名和Lua名
    /// </summary>
    private static void InitLua()
    {
        //----For CreateMVC 3
		Module2Lua[BATTLE] = "Battle";
		Module2Lua[MAIN] = "Main";
		Module2Lua[LOGIN] = "Login";
    }
}
