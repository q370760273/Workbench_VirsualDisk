using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

[Serializable]
public class AssetDatas
{
    public DateTime ChangeTime;
    public List<AssetFile> AllAssetFiles;
    public AssetDirectory Root;

    public AssetDatas()
    {
        AllAssetFiles = new List<AssetFile>();
        Root = new AssetDirectory("Assets");
    }

    public void Dispose()
    {
        AllAssetFiles.Clear();
        Root.Dispose();
        Root = null;
    }
}

public class AssetDataManager : Singleton<AssetDataManager>
{
    private Dictionary<string, string[]> _dependenciesDic = new Dictionary<string, string[]>();
    private AssetDatas _assetDatas = new AssetDatas();
    private int _progressValue, _progressTotal;
    public override void Dispose()
    {
        _dependenciesDic.Clear();
        _assetDatas.Dispose();
    }

    public void BuildAllDatas()
    {
        Dispose();
        _assetDatas = new AssetDatas();

        EditorUtility.DisplayProgressBar("Compress file Bar", "Check assets total number", 0);
        DirectoryInfo direction = new DirectoryInfo(_assetDatas.Root.Path);
        FileInfo[] files = direction.GetFiles("*.meta", SearchOption.AllDirectories);
        _progressTotal = files.Length;

        BuildDirectory(_assetDatas.Root);
        BuildDeferenceInfo();
        _assetDatas.ChangeTime = DateTime.Now;

        EditorUtility.ClearProgressBar();
    }

    private void BuildDirectory(AssetDirectory directory)
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

    private void BuildSubFiles(AssetDirectory directory)
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
            _assetDatas.AllAssetFiles.Add(file);

            string[] test = AssetDatabase.GetDependencies(file.Path, false);
            _dependenciesDic.Add(file.Path, test);

            EditorUtility.DisplayProgressBar("Loading Asset Datas", file.Name, (float)(_progressValue++) / _progressTotal);
        }
    }

    private void BuildDeferenceInfo()
    {
        foreach (KeyValuePair<string, string[]> pair in _dependenciesDic)
        {
            AssetFile file = FindAssetFile(pair.Key);
            if (file == null)
                Debug.LogError("error AssetFile path:" + pair.Key);

            foreach (string dependencePath in pair.Value)
            {
                if (string.IsNullOrEmpty(dependencePath))
                    continue;

                AssetFile dependenceFile = FindAssetFile(dependencePath);
                if (dependenceFile == null)
                    Debug.LogError("error AssetFile path:" + dependencePath);

                file.AddDefFile(dependenceFile);
                dependenceFile.AddReDefFile(file);
            }
        }
    }

    private AssetFile FindAssetFile(string filePath)
    {
        string[] paths = filePath.Split('/');
        int index = 0;
        if (paths[index] != _assetDatas.Root.Name)
            return null;

        AssetData target = _assetDatas.Root;
        while (++index < paths.Length)
        {
            target = (target as AssetDirectory).GetChild(paths[index]);
            if (target == null)
                return null;
        }

        return target as AssetFile; //最后一个引用路径名肯定为file
    }

    public List<AssetData> GetAllUnusedFiles()
    {
        List<AssetData> unusedFiles = new List<AssetData>();
        foreach (AssetFile file in _assetDatas.AllAssetFiles)
        {
            if (file.reDefFiles.Count == 0)
                unusedFiles.Add(file);
        }
        return unusedFiles;
    }

    public AssetDirectory Root
    { get { return _assetDatas.Root; } }

    public AssetDatas AssetDatas
    {
        get { return _assetDatas; }
        set { _assetDatas = value; }
    }
}
