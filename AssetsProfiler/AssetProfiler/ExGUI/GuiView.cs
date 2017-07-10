using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiView
{
    protected Rect _rect;
    protected GuiView _parent;
    protected List<GuiView> _childs = new List<GuiView>();

    public GuiView(Rect rect)
    {
        _rect = rect;
    }

    public virtual void Dispose()
    {
        _parent = null;

        foreach (GuiView child in _childs)
            child.Dispose();

        _childs.Clear();
    }

    public virtual void Draw()
    {
        GUI.BeginGroup(_rect);
        foreach (GuiView child in _childs)
        {
            child.Draw();
        }
        GUI.EndGroup();
    }

    public void AddChild(GuiView child)
    {
        child._parent = this;
        _childs.Add(child);
    }

    public Rect rect
    { get { return _rect; } }
}
