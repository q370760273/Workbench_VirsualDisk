using UnityEngine;
using System.Collections;

public class GuiLabel : GuiView
{
    private string _text;
    private GUIStyle _style;

    public GuiLabel(Rect rect) : base(rect) { }

    public GuiLabel(Rect rect, string text) : base(rect)
    {
        _text = text;
        _style = new GUIStyle();
        _style.alignment = TextAnchor.MiddleLeft;
    }

    public GuiLabel(Rect rect, string text, GUIStyle style) : base(rect)
    {
        _text = text;
        _style = style;
    }

    public override void Draw()
    {
        GUI.Label(_rect, _text, _style);
    }

    public void SetText(string text)
    {
        _text = text;
    }
}
