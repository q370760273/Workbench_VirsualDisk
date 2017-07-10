using UnityEngine;
using System.Collections;

public class GuiScrollView : GuiView
{
    private Rect _viewRect;
    private Vector2 _scrollPos = Vector2.zero;

    public GuiScrollView(Rect rect) : base(rect)
    {
        _viewRect = rect;
        _viewRect.width -= 20;
    }

    public override void Draw()
    {
        _scrollPos = GUI.BeginScrollView(_rect, _scrollPos, _viewRect);

        float maxHeight = 0;
        foreach (GuiView child in _childs)
        {
            child.Draw();

            float height = child.rect.y - _rect.y + child.rect.height;
            if (height > maxHeight)
            {
                maxHeight = height;
            }
        }
        
        _viewRect.height = Mathf.Max(maxHeight, _rect.height);

        GUI.EndScrollView();
    }
}
