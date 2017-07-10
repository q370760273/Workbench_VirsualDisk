using UnityEngine;
using System.Collections;
using System;

public class GuiSelectionGrid : GuiView
{
    private string[] _selects;
    private int _selectedIndex;
    private Action[] _handles;

    public GuiSelectionGrid(Rect rect, string[] selects) : base(rect)
    {
        if (selects == null)
            throw new Exception("未设置有效触发器");

        _selects = selects;
        _handles = new Action[selects.Length]; 
    }

    public GuiSelectionGrid(Rect rect, string[] selects, Action[] handles) : base(rect)
    {
        if (selects == null)
            throw new Exception("未设置有效触发器");

        if (selects.Length != handles.Length)
            throw new Exception("触发器与监听器不能对应!");

        _selects = selects;
        _handles = handles;
    }

    public override void Draw()
    {
        int lastSelectedIndex = _selectedIndex;
        _selectedIndex = GUI.SelectionGrid(_rect, lastSelectedIndex, _selects, _selects.Length);

        if (lastSelectedIndex != _selectedIndex)
        {
            HandleSelected();
        }
    }

    public void RegisterHandler(Action[] handles)
    {
        _handles = handles;
    }

    public void HandleSelected()
    {
        if (_handles[_selectedIndex] != null)
            _handles[_selectedIndex]();
    }

    public void SetSelectedIndex(int index)
    {
        _selectedIndex = index;
    }
}
