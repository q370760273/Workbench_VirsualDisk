using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class GuiFoldoutTree : GuiView
{
    private const int _textHeight = 16;
    private const int _textInterval = 2;

    private AssetData[] _rootChilds = new AssetData[0];
    private int _showCount;
    private GuiDrawer _drawer;

    public GuiFoldoutTree(Rect rect) : base(rect)
    {
    }

    public override void Dispose()
    {
        base.Dispose();
        _drawer = null;
    }

    public void AttachDrawer(GuiDrawer drawer)
    {
        _drawer = drawer;
    }

    public override void Draw()
    {
        GUILayout.BeginArea(_rect);
        object[] results = _drawer.Draw();
        _showCount = Convert.ToInt32(results[0]);
        _rect.height = _showCount * (_textHeight + _textInterval);
        GUILayout.EndArea();
    }

    public void Reset(AssetData[] rootChilds)
    {
        _rootChilds = rootChilds;
    }

    public AssetData[] rootChilds
    { get { return _rootChilds; } }

    public int textHeight
    { get { return _textHeight; } }

    public int textInterval
    { get { return _textInterval; } }
}
