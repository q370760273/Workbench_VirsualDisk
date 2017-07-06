using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class AssetDirectory : AssetData
{
    protected List<AssetData> _childs = new List<AssetData>();

    public AssetDirectory(string name) : base(name)
    {
    }

    public void AddChild(AssetData child)
    {
        child.SetParent(this);
        _childs.Add(child);
    }
}
