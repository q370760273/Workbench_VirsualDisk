using UnityEngine;
using System.Collections;
using System;

public class GuiTextField : GuiView
{
    protected string _inputText = "";
    protected string _lastInputText = "";
    protected Action<string> _handle;

    public GuiTextField(Rect rect) : base(rect)
    {
    }

    public override void Draw()
    {
        _inputText = GUI.TextField(_rect, _inputText);
        if (_handle != null && _inputText != _lastInputText)
        {
            _lastInputText = _inputText;
            _handle(_inputText);
        }
    }

    public void OnTextChange(Action<string> handle)
    {
        _handle = handle;
    }

    public string InputText
    {
        get { return _inputText; }
    }
}
