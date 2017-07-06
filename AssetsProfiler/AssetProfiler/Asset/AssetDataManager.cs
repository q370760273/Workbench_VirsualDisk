using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AssetDataManager
{
    public static void ParseAssetDatas()
    {
        _dependenciesDic.Clear();
        AssetDirectory directory = new AssetDirectory("Assets/AssetbundleResources");
        BuildDirectory(directory);
        BuildDeferenceInfo();
    }

    public static void BuildDirectory(AssetDirectory directory)
    {
        string[] folders = AssetDatabase.GetSubFolders(directory.Path);
        foreach (string folder in folders)
        {
            AssetDirectory subDirectory = new AssetDirectory(folder.Substring(folder.LastIndexOf("/") + 1));
            directory.AddChild(subDirectory);
            BuildDirectory(subDirectory);
        }

        BuildSubFiles(directory);
    }

    public static void BuildSubFiles(AssetDirectory directory)
    {
        DirectoryInfo direction = new DirectoryInfo(directory.Path);
        FileInfo[] files = direction.GetFiles();

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta"))
            {
                continue;
            }

            AssetFile file = new AssetFile(files[i].Name);
            directory.AddChild(file);

            string[] test = AssetDatabase.GetDependencies(directory.Path + "/" + files[i].Name);
            _dependenciesDic.Add(directory.Path + "/" + files[i].Name, test);
            Debug.Log("Name:" + files[i].Name);
            //Debug.Log( "FullName:" + files[i].FullName );  
            //Debug.Log( "DirectoryName:" + files[i].DirectoryName );  
        }
    }

    public static void BuildDeferenceInfo()
    {

    }

    public static Dictionary<string, string[]> _dependenciesDic = new Dictionary<string, string[]>();
}
