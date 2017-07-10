using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;

public class DependenceInfoDrawer : GuiDrawer
{
    private int _showCount;
    private List<AssetData> _showFiles = new List<AssetData>();
    public DependenceInfoDrawer(GuiView view) : base(view)
    {
    }

    public override object[] Draw()
    {
        _showFiles.Clear();
        _showCount = 0;
        AssetData[] rootChilds = (_view as GuiFoldoutTree).rootChilds;

        for (int i = 0; i < rootChilds.Length; i++)
        {
            if (rootChilds[i].IsDirectory())
                continue;

            _showCount++;

            AssetFile file = rootChilds[i] as AssetFile;
            _showFiles.Add(file);

            if (file.defFiles.Count > 0)
            {
                DrawFoldout(file);
            }
            else
            {
                EditorGUILayout.LabelField(file.Name);
            }
        }

        return new object[] { _showCount };
    }

    private void DrawFoldout(AssetFile file)
    {
        if (file.FoldState = EditorGUILayout.Foldout(file.FoldState, file.Name))
        {
            _showCount += file.defFiles.Count;
            EditorGUI.indentLevel++;
            foreach (AssetFile child in file.defFiles)
            {
                if (CheckLoop(child))
                {
                    EditorGUILayout.LabelField(child.Name + " [检测到循环]");
                }
                else
                {
                    _showFiles.Add(child);

                    if (child.defFiles.Count > 0)
                    {
                        DrawFoldout(child);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(child.Name);
                    }
                }
            }
            EditorGUI.indentLevel--;
        }
    }

    private bool CheckLoop(AssetFile child)
    {
        if (_showFiles.Contains(child))
            return true;

        return false;
    }
}
