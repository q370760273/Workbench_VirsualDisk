using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.Collections.Generic;

public class AssetDatasDrawer : GuiDrawer
{
    private int _showCount;
    public AssetDatasDrawer(GuiView view) : base(view)
    {
    }

    public override object[] Draw()
    {
       AssetData[] rootChilds = (_view as GuiFoldoutTree).rootChilds;
        _showCount = 0;

        for (int i = 0; i < rootChilds.Length; i++)
        {
            if (!rootChilds[i].Visible)
                continue;

            _showCount++;

            if (rootChilds[i].IsDirectory())
            {
                DrawFoldout(rootChilds[i] as AssetDirectory);
            }
            else
            {
                if(GUILayout.Button(rootChilds[i].Name, GUI.skin.label))
                {
                    EventHandler.Instance.HandleInvoke("Show_Dependencies_Info", rootChilds[i]);
                }
            }
        }

        return new object[] { _showCount };
    }

    private void DrawFoldout(AssetDirectory directory)
    {
        if (directory.FoldState = EditorGUILayout.Foldout(directory.FoldState, directory.Name))
        {
            EditorGUI.indentLevel++;
            string textSpace = OutoFillSpace();
            foreach (AssetData child in directory.childs)
            {
                if (!child.Visible)
                    continue;

                _showCount++;

                if (child.IsDirectory())
                {
                    DrawFoldout(child as AssetDirectory);
                }
                else
                {
                    if (GUILayout.Button(textSpace + child.Name, GUI.skin.label))
                    {
                        EventHandler.Instance.HandleInvoke("Show_Dependencies_Info", child);
                    }
                }
            }
            EditorGUI.indentLevel--;
        }
    }

    private string OutoFillSpace()
    {
        return "".PadLeft(EditorGUI.indentLevel * 4);
    }
}
