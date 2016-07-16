using UnityEngine;
using UnityEditor;
using System.Collections;

public class HierarchySort {
    /// <summary>
    /// 按名字排序
    /// </summary>
    public static int CompareName(GameObject lhs, GameObject rhs)
    {
        if (lhs == rhs) return 0;
        if (lhs == null) return -1;
        if (rhs == null) return 1;
        return EditorUtility.NaturalCompare(lhs.name, rhs.name);
    }
    /// <summary>
    /// 按Instance ID排序
    /// </summary>
    public static int CompareInstanceID(GameObject lhs, GameObject rhs)
    {
        if (lhs == rhs) return 0;
        if (lhs == null) return -1;
        if (rhs == null) return 1;
        return lhs.GetInstanceID().CompareTo(rhs.GetInstanceID());
    }
}

public class 名字升序 : BaseHierarchySort
{
    public override int Compare(GameObject lhs, GameObject rhs)
    {
        return HierarchySort.CompareName(lhs, rhs);
    }
    public override GUIContent content
    {
        get 
        { 
            return new GUIContent("升序"); 
        }
    }
}

public class 名字降序 : BaseHierarchySort
{
    public override int Compare(GameObject lhs, GameObject rhs)
    {
        return HierarchySort.CompareName(rhs, lhs);
    }

    public override GUIContent content
    {
        get
        {
            return new GUIContent("降序");
        }
    }

}