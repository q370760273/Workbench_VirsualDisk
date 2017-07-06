using UnityEngine;
using System.Collections;

public class GuiSelectionGrid : GuiView
{
    private string[] _selects;
    private int _selectedIndex;
    public GuiSelectionGrid(Rect rect, string[] selects) : base(rect)
    {
        if (selects == null)
            _selects = new string[0];

        _selects = selects;
    }

    public override void Draw()
    {
        _selectedIndex = GUI.SelectionGrid(_rect, _selectedIndex, _selects, _selects.Length);
    }

    public void SetSelectedIndex(int index)
    {
        _selectedIndex = index;
    }
}
