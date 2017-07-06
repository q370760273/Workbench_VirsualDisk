using UnityEngine;
using System.Collections;

public class GuiScrollView : GuiView
{
    private Rect _viewRect;
    private Vector2 _scrollPos = Vector2.zero;

    public GuiScrollView(Rect rect, Rect viewRect) : base(rect)
    {
        _viewRect = viewRect;
    }

    public override void Draw()
    {
        _scrollPos = GUI.BeginScrollView(_rect, _scrollPos, _viewRect);
        GUI.EndScrollView();
    }
}
