using UnityEngine;
using System.Collections;

public class GameTool {

    /// <summary>
    /// 通过名字来寻找子对象
    /// </summary>
    /// <param name="Parent">父对象</param>
    /// <param name="ChildName">子对象名字</param>
    /// <param name="IsDepth">是否查找非直接子对象</param>
    /// <returns></returns>
    public static GameObject GetChildByName(GameObject Parent, string ChildName, bool IsDepth = false)
    {
        Transform Child = null;
        GameObject CurChild = null;
        Transform TempChild = null;

        Child = Parent.transform.FindChild(ChildName);
        if (Child != null)
        {
            CurChild = Child.gameObject;
            return CurChild;
        }
        if (IsDepth == true)
        {
            int ChildNum = Parent.transform.childCount;
            for (int i = 0; i < ChildNum; i++)
            {
                Child = Parent.transform.GetChild(i);
                TempChild = Child.FindChild(ChildName);
                if (TempChild != null)
                {
                    CurChild = TempChild.gameObject;
                    break;
                }
                else
                {
                    CurChild = GetChildByName(Child.gameObject, ChildName);
                    if (CurChild != null)
                    {
                        break;
                    }
                }
            }
        }

        return CurChild;
    }

    /// <summary>
    /// 将绝对路径修改为项目的相对路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string Absolute2Relative(string path)
    {
        return path.Replace("\\", "/").Replace(Application.dataPath, "Assets");
    }

    /// <summary>
    /// 获取UI对象的深度
    /// </summary>
    /// <param name="go">UI对象</param>
    /// <returns>深度值</returns>
    public static int GetDepth(GameObject go)
    {
        RectTransform rect = go.GetComponent<RectTransform>();
        if (rect != null)
            return rect.GetSiblingIndex();

        return -1;
    }

    public static Vector3 CreateVector3(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }

    public static void SetPos(GameObject go, float x, float y, float z)
    {
        go.transform.localPosition = new Vector3(x, y, z);
    }

    
}
