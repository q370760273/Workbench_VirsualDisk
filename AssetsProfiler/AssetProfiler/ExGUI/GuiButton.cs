using UnityEngine;
using System.Collections;
using System;

public class GuiButton : GuiView
{
    private string _text;
    private Action<GuiView> _handle;
    public GuiButton(Rect rect, string text) : base(rect)
    {
        _text = text;
    }

    public override void Draw()
    {
        if (GUI.Button(_rect, _text))
        {
            if (_handle != null)
            {
                _handle(this);
            }
        }
    }

    public void RegisterHandler(Action<GuiView> handle)
    {
        _handle = handle;
    }
}
