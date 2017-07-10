using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System;

[Serializable]
public class AssetDirectory : AssetData
{
    protected List<AssetData> _childs = new List<AssetData>();
    public AssetDirectory(string name) : base(name)
    {
        FoldState = false;
    }

    public override void Dispose()
    {
        base.Dispose();

        foreach (AssetData child in _childs)
            child.Dispose();

        _childs.Clear();
    }

    public override bool IsDirectory()
    {
        return true;
    }

    public void AddChild(AssetData child)
    {
        child.SetParent(this);
        _childs.Add(child);
    }

    public AssetData GetChild(string name)
    {
        foreach (AssetData child in _childs)
        {
            if (child.Name == name)
                return child;
        }
        return null;
    }

    public List<AssetData> childs
    { get { return _childs; } }
}
