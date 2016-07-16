using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;

/// <summary>
/// 完成注册模块名 + 注册Mediator + 创建MVC文件 + 创建Lua文件
/// </summary>
public class CreateMVCTool : EditorWindow
{
    private string _UIModuleName = "";
    private string _ResourceName = "";
    private string _LuaName = "";
    private string _FolderPath = "";

    [MenuItem("Tools/一键创建MVC")]
    static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 400, 100);
        CreateMVCTool window = (CreateMVCTool)EditorWindow.GetWindowWithRect(typeof(CreateMVCTool), wr, true, "创建MVC");
        window.Show();
    }

    void OnGUI()
    {
        _UIModuleName = EditorGUILayout.TextField("请输入UI模块名:", _UIModuleName);
        _ResourceName = EditorGUILayout.TextField("请输入资源名:", _ResourceName);
        _LuaName = EditorGUILayout.TextField("请输入Lua名:", _LuaName);

        if (GUILayout.Button("一键创建MVC", GUILayout.Width(400)))
        {
            if (_UIModuleName.Equals("") || _UIModuleName == null)
                Debug.LogError("UI模块名不能为空");
            else if (_ResourceName.Equals("") || _ResourceName == null)
                Debug.LogError("UI模块资源名不能为空");
            else if (_LuaName.Equals("") || _LuaName == null)
                Debug.LogError("Lua名不能为空");
            else
            {
                InitPath();
                CreateNeededFolder();
                CreateMVCScripts();
                RegisterModulesToLib();
                RegisterMediator();
                CreateLua();

                AssetDatabase.Refresh();
            }
        }
    }

    /// <summary>
    /// 找到模块对应的脚本目录
    /// </summary>
    private void InitPath()
    {
        _FolderPath = GameConfig.UI_MODULES() + _UIModuleName;
    }

    /// <summary>
    /// 在Scripts/UIModules目录下创建需要的文件夹
    /// </summary>
    private void CreateNeededFolder()
    {
        if (FileTool.IsDirectoryExist(_FolderPath))
        {
            Debug.Log(_FolderPath + "目录已经存在，不再创建");
            return;
        }
        FileTool.CreateDirectory(_FolderPath);
        Debug.Log("创建MVC文件夹成功，路径是" + _FolderPath);
    }

    /// <summary>
    /// 创建MVC所需的脚本（这里只创建必需的Mediator脚本）
    /// </summary>
    private void CreateMVCScripts()
    {
        string mediatorName = _UIModuleName + "Mediator";
        string mediatorPath = _FolderPath + "/" + mediatorName + ".cs";
        if (FileTool.IsFileExist(mediatorPath))
        {
            Debug.Log(mediatorPath + "脚本已经存在，不再创建");
            return;
        }
        string mediatorContent = CreateMediatorContent(mediatorName);
        FileTool.CreateFile(mediatorPath, mediatorContent);
        Debug.Log("创建MVC脚本成功");
    }

    private string CreateMediatorContent(string name)
    {
        string content = @"
using UnityEngine;
using System.Collections;

public class xxx : BaseMediator
{
    public xxx(string ModuleName)
        : base(ModuleName)
    {

    }
    protected override void OnCheckDataProxy()
    {

    }
}
";
        content = content.Replace("xxx", name);
        return content;
    }

    /// <summary>
    /// 把模块在ModulesLib脚本中注册
    /// </summary>
    private void RegisterModulesToLib()
    {
        string path = GameConfig.ModulesLib_path();
        string newContent = "public const string xxx = \"yyy\";";
        string constantName = ChangeString2ConstantStyle(_UIModuleName);
        string content = FileTool.ReadFileContent(path);

        if (content.IndexOf(constantName) > -1)
        {
            Debug.Log("在ModulesLib.cs中，" + constantName + "模块已经存在");
            return;
        }
            
        newContent = newContent.Replace("xxx", constantName);
        newContent = newContent.Replace("yyy", _UIModuleName);
        content = content.Replace("For CreateMVC 1", "For CreateMVC 1\n\t" + newContent);

        newContent = "Module2Resource[xxx] = \"yyy\";";
        newContent = newContent.Replace("xxx", constantName);
        newContent = newContent.Replace("yyy", _ResourceName);
        content = content.Replace("For CreateMVC 2", "For CreateMVC 2\n\t\t" + newContent);

        newContent = "Module2Lua[xxx] = \"yyy\";";
        newContent = newContent.Replace("xxx", constantName);
        newContent = newContent.Replace("yyy", _LuaName);
        content = content.Replace("For CreateMVC 3", "For CreateMVC 3\n\t\t" + newContent);

        FileTool.DeleteFile(path);
        FileTool.CreateFile(path, content);
    }

    /// <summary>
    /// 注册Mediator
    /// </summary>
    private void RegisterMediator()
    {
        string path = GameConfig.MediatorRegister_path();
        string constantName = ChangeString2ConstantStyle(_UIModuleName);
        string mediatorName = _UIModuleName + "Mediator";
        string content = FileTool.ReadFileContent(path);

        if (content.IndexOf(constantName) > -1)
        {
            Debug.Log("在MediatorRegister.cs中" + constantName + "模块已经注册");
            return;
        }

        string newContent = "manager.RegisterMediator(new xxx(ModulesLib.yyy), true);";
        newContent = newContent.Replace("xxx", mediatorName);
        newContent = newContent.Replace("yyy", constantName);
        content = content.Replace("For CreateMVC", "For CreateMVC\n\t\t" + newContent);
        FileTool.DeleteFile(path);
        FileTool.CreateFile(path, content);
    }

    /// <summary>
    /// 将类似"MainView"这样的字符串转换为"MAIN_VIEW"
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string ChangeString2ConstantStyle(string str)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < str.Length; i++)
        {
            int ascii = (int)str[i];
            //Check low
            if (ascii >= 97 && ascii <= 122)
            {
                char c = (char)(ascii - 32);
                sb.Append(c);
            }
            //Check up
            else if (ascii >= 65 && ascii <= 90)
            {
                if (i != 0)
                    sb.Append("_");
                sb.Append(str[i]);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 创建Lua文件
    /// </summary>
    private void CreateLua()
    {
        CreateLuaTemplateFile(_LuaName);
        Debug.Log("创建Lua文件完成");
    }

    /// <summary>
    /// 创建以参数为名字的Lua模板文件
    /// </summary>
    /// <param name="name"></param>
    private void CreateLuaTemplateFile(string name)
    {
        string path = GameConfig.Lua_path();
        string filePath = path + "/" + name + ".txt";
        if (FileTool.IsFileExist(filePath))
        {
            Debug.Log("已经存在" + name + ".txt文件，不再创建");
            return;
        }

        string content =
@"
local ModuleName = ModulesLib.yyy;--所属模块名
local SelfPanel = nil;--模块界面
local ViewManager = nil;--视图管理
local ModelManager = nil;--数据管理
local Mediator = nil;--消息管理

--模块初始化
function xxx_OnInit(BaseUIView)
	SelfPanel   = BaseUIView.gameObject;
	ViewManager = BaseUIView;
	ModelManager = UIManager.Instance:GetDataModelByModuleName(ModuleName);
	xxx_BindUI();
	xxx_AddEvent();
end

--绑定UI
function xxx_BindUI()

end

--绑定事件
function xxx_AddEvent()

end

function xxx_AddBtn(btn)
	UIEventListener.AddEventListener(btn, UIEventType.CLICK, xxx_OnClick);
end

--事件监听
function xxx_OnClick(GameObject)
	local TargetName = GameObject.name;

end

--显示界面调用
function xxx_OnShowView()

end

--关闭界面调用
function xxx_OnCloseView()

end

--收到消息时调用
function xxx_OnReceive(InfoName,Data)

end

--注册消息时调用
function xxx_OnRegister(BaseMediator)
	Mediator = BaseMediator;
end

";
        content = content.Replace("xxx", name);
        content = content.Replace("yyy", ChangeString2ConstantStyle(_UIModuleName));
        FileTool.CreateFile(filePath, content);
        Debug.Log(path + "/" + name + ".txt");
    }
}
