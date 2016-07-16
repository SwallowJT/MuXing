using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {
    /// <summary>
    /// 生命值
    /// </summary>
    public static int HP = 3;

    /// <summary>
    /// 目前得到的分数
    /// </summary>
    public static int Score = 0;

    /// <summary>
    /// 已走过的里程数
    /// </summary>
    public static int Distance = 0;

    /// <summary>
    /// 从数据库中提取数据
    /// </summary>
    public static void Load()
    {
        if (PlayerPrefs.HasKey("hp"))
            HP = PlayerPrefs.GetInt("hp");
        if (PlayerPrefs.HasKey("score"))
            Score = PlayerPrefs.GetInt("score");
        if (PlayerPrefs.HasKey("distance"))
            Distance = PlayerPrefs.GetInt("distance");
    }

    /// <summary>
    /// 保存数据到数据库中
    /// </summary>
    public static void Save()
    {
        PlayerPrefs.SetInt("hp", HP);
        PlayerPrefs.SetInt("score", Score);
        PlayerPrefs.SetInt("distance", Distance);
    }
}
