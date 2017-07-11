using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AssetData
{
    protected string _name;
    protected string _path;
    protected AssetData _parent;
    protected bool _visible;

    public AssetData(string name)
    {
        _path = _name = name;
        _visible = true;
    }

    public virtual void Dispose()
    {
        _parent = null;
    }

    public virtual bool IsDirectory()
    {
        throw new NotImplementedException();
    }

    public virtual bool ApplyPattern(string pattern)
    {
        throw new NotImplementedException();
    }

    public void SetParent(AssetData parent)
    {
        _parent = parent;
        _path = _parent.Path + "/" + _name;
    }

    public bool FoldState
    { get; set; }

    public bool Visible
    { get { return _visible; } }

    public string Path
    { get { return _path; } }

    public string Name
    { get { return _name; } }
}
