using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LuaInterface;

/// <summary>
/// 在滚动视图中利用协程加载内容，用于解决一次性加载太多内容导致的卡顿（主要在Lua中使用）
/// 使用：创建ScrollView，把本脚本挂在Content里，而具体参数在GridLayoutGroup中设置
/// </summary>
[RequireComponent(typeof(GridLayoutGroup))]
public class UIGrid : MonoBehaviour {
    private string _ItemName; //内容Prefab的名字
    private int _Count; //item的个数
    private int _Rows; //行数

    private RectTransform _Content;
    private GridLayoutGroup _LayoutCom;

    public delegate void CreateItemComplete(GameObject go, int index); //创建一个Item后的回调
    public CreateItemComplete OnCreateComplete;

    public void CreateItem(string itemName, object callback, int count)
    {
        _Content = GetComponent<RectTransform>();
        _LayoutCom = GetComponent<GridLayoutGroup>();
        _ItemName = itemName;
        _Count = count;
        _Rows = Mathf.CeilToInt((float)count / (float)_LayoutCom.constraintCount);
        _Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_LayoutCom.cellSize.y + _LayoutCom.spacing.y) * _Rows);

        OnCreateComplete = delegate(GameObject go, int index)
        {
            (callback as LuaFunction).Call(go, index);
        };

        StartCoroutine(CreateAll());
    }

    IEnumerator CreateAll()
    {
        for (int i = 0; i < _Count; i++)
        {
            LoadItem(i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void LoadItem(int index)
    {
        //TODO: 改用ResourcesManager
        GameObject item = GameObject.Instantiate(Resources.Load("UI/Prefab/" + _ItemName)) as GameObject;
        item.transform.parent = this.transform;
        item.transform.localScale = new Vector3(1, 1, 1);
        OnCreateComplete(item, index);
    }
}
