using UnityEngine;
using System.Collections;
using System;

public abstract class GuiDrawer
{
    protected GuiView _view;

    public GuiDrawer(GuiView view)
    {
        _view = view;
    }

    public abstract object[] Draw();
}
