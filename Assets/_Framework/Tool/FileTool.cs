using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class FileTool 
{

    public static void GetFiles(DirectoryInfo directory, ref List<string> paths, bool recursion = false)
    {
        FileInfo[] files = directory.GetFiles();
        foreach (FileInfo file in files)
        {
            paths.Add(file.FullName);
        }
        if (recursion)//递归遍历子文件夹
        {
            DirectoryInfo[] directories = directory.GetDirectories();
            foreach (DirectoryInfo info in directories)
            {
                if (info.FullName.Contains(".svn"))
                {
                    continue;
                }
                GetFiles(info, ref paths, recursion);
            }
        }
    }

    public static bool IsFileExist(string filePath)
    {
        FileInfo info = new FileInfo(filePath);
        return info.Exists;
    }

    public static bool IsDirectoryExist(string path)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        return directoryInfo.Exists;
    }

    public static void CreateDirectory(string path, bool isReplace = false)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);

        if (directoryInfo.Exists)
        {
            if (isReplace)
            {
                FileTool.DeleteFolder(path);
            }
            else
            {
                Debug.Log("已经存在" + path);
                return;
            }
        }

        Directory.CreateDirectory(path);
    }

    public static void CreateFile(string path, string content = "", bool isReplace = false)
    {
        if (File.Exists(path))
        {
            if (isReplace)
            {
                FileTool.DeleteFile(path);
            }
            else
            {
                Debug.Log("已经存在" + path);
                return;
            }
        }

        FileStream myFs = new FileStream(path, FileMode.Create);
        StreamWriter mySw = new StreamWriter(myFs, Encoding.UTF8);
        mySw.Write(content);
        mySw.Close();
        myFs.Close();
    }

    public static void ParseKeyValueText(string text, ref Dictionary<string, string> dic)
    {
        if (text == null || text == string.Empty)
        {
            return;
        }
        char[] array = new char[]
		{
			' ',
			'\n',
			'\t',
			'\r'
		};
        StringReader stringReader = new StringReader(text);
        string text2;
        while ((text2 = stringReader.ReadLine()) != null)
        {
            string text3 = text2.Trim(array);
            if (!text3.StartsWith("#") && !(text3 == string.Empty))
            {
                string[] array2 = text3.Split(new char[]
				{
					'='
				});
                if (array2.Length == 2)
                {
                    string text4 = array2[0].Trim().ToUpper();
                    string text5 = array2[1].Trim(array);
                    if (dic.ContainsKey(text4))
                    {
                        dic[text4] = text5;
                    }
                    else
                    {
                        dic.Add(text4, text5);
                    }
                }
            }
        }
    }

    public static void DeleteFolder(string path)
    {
        Debug.LogWarning("Delete Folder : " + path);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

    public static void DeleteFile(string path)
    {
        FileInfo info = new FileInfo(path);
        if (info.Exists)
        {
            info.Delete();
        }
    }

    

    public static string ReadFileContent(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.Default);
        string line;
        StringBuilder sb = new StringBuilder();
        while ((line = sr.ReadLine()) != null)
        {
            sb.Append(line.ToString() + "\n");
        }
        sr.Close();
        return sb.ToString();
    }
}
