using UnityEngine;
using System.Collections;

public class AssetData
{
    protected string _name;
    protected string _path;
    protected AssetData _parent;

    public AssetData(string name)
    {
        _path = _name = name;
    }

    public void SetParent(AssetData parent)
    {
        _parent = parent;
        _path = _parent.Path + "/" + _name;
    }

    public string Path
    { get { return _path; } }
}
