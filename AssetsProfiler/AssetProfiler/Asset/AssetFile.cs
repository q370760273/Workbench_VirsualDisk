using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class AssetFile : AssetData
{
    List<AssetFile> _defFiles = new List<AssetFile>();
    List<AssetFile> _reDefFiles = new List<AssetFile>();

    public AssetFile(string name) : base(name)
    {
    }

    public override void Dispose()
    {
        base.Dispose();
        _defFiles.Clear();
        _reDefFiles.Clear();
    }

    public override bool IsDirectory()
    {
        return false;
    }

    public override bool ApplyPattern(string pattern)
    {
        _visible = _name.ToLower().Contains(pattern);
        return _visible;
    }

    public void AddDefFile(AssetFile file)
    {
        _defFiles.Add(file);
    }

    public void AddReDefFile(AssetFile file)
    {
        _reDefFiles.Add(file);
    }

    public List<AssetFile> defFiles
    { get { return _defFiles; } }

    public List<AssetFile> reDefFiles
    { get { return _reDefFiles; } }
}
