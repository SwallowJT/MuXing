using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using LuaInterface;
using UnityEngine.UI;
public class UIEventListener : MonoBehaviour,
    #region Interface
                                IPointerClickHandler,
                                IPointerDownHandler,
                                IPointerEnterHandler,
                                IPointerExitHandler,
                                IPointerUpHandler,
                                ISelectHandler,
                                IUpdateSelectedHandler,
                                IDeselectHandler,
                                IBeginDragHandler,
                                IDragHandler,
                                IEndDragHandler,
                                IDropHandler,
                                IScrollHandler,
                                IMoveHandler
#endregion
{
    #region Delegate
    public delegate void VoidDelegate(GameObject go);
    public delegate void MoveDelegate(MoveDirection Direction);
    public delegate void PointDelegate(PointerEventData PointerEvent);
    #endregion

    #region Callback
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onDeSelect;
    public PointDelegate onBeginDrag;
    public PointDelegate onDrag;
    public PointDelegate onDragEnd;
    public PointDelegate onDrop;
    public VoidDelegate onScroll;
    public VoidDelegate onLongPress;
    public MoveDelegate onMove;
    #endregion

    public object parameter;

    private bool IsClickDown;
    private float StartDownTime;
    private const float LongPressTime = 0.3f;

    private static ScrollRect ScrollRectComp;

    #region 监听屏幕事件（系统自带的）
    public void OnPointerClick(PointerEventData eventData) { if (onClick != null) onClick(gameObject); }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null)
        {
            onDown(gameObject);
        }
        IsClickDown = true;
        StartDownTime = Time.time;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null)
        {
            onUp(gameObject);
        }
        IsClickDown = false;
    }
    public void OnPointerEnter(PointerEventData eventData) { if (onEnter != null) onEnter(gameObject); }
    public void OnPointerExit(PointerEventData eventData) { if (onExit != null) onExit(gameObject); }
    public void OnSelect(BaseEventData eventData) { if (onSelect != null) onSelect(gameObject); }
    public void OnUpdateSelected(BaseEventData eventData) { if (onUpdateSelect != null) onUpdateSelect(gameObject); }
    public void OnDeselect(BaseEventData eventData) { if (onDeSelect != null) onDeSelect(gameObject); }
    public void OnBeginDrag(PointerEventData eventData) { if (onBeginDrag != null) onBeginDrag(eventData); }
    public void OnDrag(PointerEventData eventData) { if (onDrag != null) onDrag(eventData); }
    public void OnEndDrag(PointerEventData eventData) { if (onDragEnd != null) onDragEnd(eventData); }
    public void OnDrop(PointerEventData eventData) { if (onDrop != null) onDrop(eventData); }
    public void OnScroll(PointerEventData eventData) { if (onScroll != null) onScroll(gameObject); }
    public void OnMove(AxisEventData eventData) { if (onMove != null) onMove(eventData.moveDir); }
    #endregion

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="obj">要检测事件的对象</param>
    /// <param name="EventType">事件类型</param>
    /// <param name="CallBack">回调函数</param>
    public static void AddEventListener(GameObject obj, string EventType, object CallBack)
    {
        UIEventListener Listener = UIEventListener.GetListenerFrom(obj);
        VoidDelegate Call = null;
        MoveDelegate MoveCall = null;
        PointDelegate DragCall = null;
        if (EventType != UIEventType.MOVE)
        {
            if (CallBack is LuaFunction)
            {
                Call = delegate(GameObject Obj)
                {
                    (CallBack as LuaFunction).Call(obj);
                };
            }
            else
            {
                Call = CallBack as VoidDelegate;
            }
        }
        else
        {
            if (CallBack is LuaFunction)
            {
                MoveCall = delegate(MoveDirection Obj)
                {
                    (CallBack as LuaFunction).Call(obj);
                };
            }
            else
            {
                MoveCall = CallBack as MoveDelegate;
            }
        }

        switch (EventType)
        {
            case UIEventType.CLICK:
                Listener.onClick += Call;
                CheckScrollRect(Listener, obj);
                break;
            case UIEventType.CLICK_DOWN:
                Listener.onDown = Call;
                break;
            case UIEventType.CLICK_UP:
                Listener.onUp = Call;
                break;
            case UIEventType.DRAG:
                if (CallBack is LuaFunction)
                {
                    DragCall = delegate(PointerEventData Obj)
                    {
                        (CallBack as LuaFunction).Call(obj);
                    };
                }
                else
                {
                    DragCall = CallBack as PointDelegate;
                }
                Listener.onDrag += DragCall;
                break;
            case UIEventType.DROP:
                if (CallBack is LuaFunction)
                {
                    DragCall = delegate(PointerEventData Obj)
                    {
                        (CallBack as LuaFunction).Call(obj);
                    };
                }
                else
                {
                    DragCall = CallBack as PointDelegate;
                }
                Listener.onDragEnd += DragCall;
                break;
            case UIEventType.LONG_PRESS:
                Listener.onLongPress = Call;
                break;
            case UIEventType.SCROLL:
                Listener.onScroll = Call;
                break;
            case UIEventType.MOVE:
                Listener.onMove = MoveCall;
                break;
        }
    }

    /// <summary>
    /// 从游戏对象上获取监听器组件
    /// </summary>
    static public UIEventListener GetListenerFrom(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }

    /// <summary>
    /// 兼容滚动条拖动
    /// </summary>
    /// <param name="Listener"></param>
    /// <param name="Obj"></param>
    private static void CheckScrollRect(UIEventListener Listener, GameObject Obj)
    {
        if (ScrollRectComp == null)
            ScrollRectComp = Obj.GetComponentInParent<ScrollRect>();
        if (ScrollRectComp != null)
        {
            Listener.onBeginDrag += Listener.OnScrollBeginDrag;
            Listener.onDrag += Listener.OnScrollDrag;
            Listener.onDragEnd += Listener.OnScrollEndDrag;
        }
    }

    void Update()
    {
        //处理长按事件
        if (IsClickDown == true)
        {
            float OffsetTime = Time.time - StartDownTime;
            if (OffsetTime > LongPressTime)
            {
                OnLongPress(this.gameObject);
            }
        }

    }

    #region 监听屏幕事件（自写的）
    public void OnLongPress(GameObject obj) { if (onLongPress != null) onLongPress(obj); }

    private void OnScrollBeginDrag(PointerEventData Point)
    {
        if (ScrollRectComp != null)
            ScrollRectComp.OnBeginDrag(Point);
    }

    private void OnScrollDrag(PointerEventData Point)
    {
        if (ScrollRectComp != null)
            ScrollRectComp.OnDrag(Point);
    }

    private void OnScrollEndDrag(PointerEventData Point)
    {
        if (ScrollRectComp != null)
            ScrollRectComp.OnEndDrag(Point);
    }
    #endregion
}