using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetFile : AssetData
{
    List<AssetFile> _defFiles = new List<AssetFile>();
    List<AssetFile> _reDefFiles = new List<AssetFile>();

    public AssetFile(string name) : base(name)
    {
    }
}
