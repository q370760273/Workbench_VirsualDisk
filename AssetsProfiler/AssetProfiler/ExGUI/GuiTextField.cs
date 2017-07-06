using UnityEngine;
using System.Collections;

public class GuiTextField : GuiView
{
    protected string _InputSearchText = "";

    public GuiTextField(Rect rect) : base(rect)
    {
    }

    public override void Draw()
    {
        _InputSearchText = GUI.TextField(_rect, _InputSearchText);
    }
}
