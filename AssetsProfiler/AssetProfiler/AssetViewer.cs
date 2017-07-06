using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class AssetViewer : EditorWindow
{
    [MenuItem("Custom/AssetViewer")]
    static void OpenTool()
    {
        AssetViewer window = GetWindow<AssetViewer>(false, "Asset Viewer", true);
        window.minSize = new Vector2(900f, 600f);
        _lineTexure = new Texture2D(1,1);
        _lineTexure.SetPixel(1, 1, Color.black);
        _lineTexure.Apply();
    }

    private GuiView _view;

    void Awake()
    {
        Init();
    }
    private void Init()
    {
        _view = new GuiView(new Rect(5, 5, 890, 590));

        GuiButton button = new GuiButton(new Rect(0, 0, 110, 20), "刷新资源数据库");
        button.RegisterHandler(RefreshAssets);
        _view.AddChild(button);

        button = new GuiButton(new Rect(340, 2, 16, 16), "?");
        button.RegisterHandler(RefreshAssets);
        _view.AddChild(button);

        GuiSearchTextField searchTextField = new GuiSearchTextField(new Rect(130, 1, 196, 20));
        _view.AddChild(searchTextField);

        GuiLabel label = new GuiLabel(new Rect(400, 0, 500, 20), "您目前使用的是 " + DateTime.Now.ToString() + " 更新的资源数据库");
        _view.AddChild(label);

        GuiSelectionGrid grid = new GuiSelectionGrid(new Rect(0, 35, 200, 20), new string[] { "资源列表", "无引用资源" });
        _view.AddChild(grid);

        grid = new GuiSelectionGrid(new Rect(345, 35, 200, 20), new string[] { "资源依赖项", "反向引用" });
        _view.AddChild(grid);

        GuiScrollView scrollView = new GuiScrollView(new Rect(0, 60, 320, 530), new Rect(0, 60, 300, 3000));
        _view.AddChild(scrollView);

        scrollView = new GuiScrollView(new Rect(340, 60, 550, 530), new Rect(340, 60, 530, 3000));
        _view.AddChild(scrollView);
    }

    void OnGUI()
    {
        if(_view != null)
            _view.Draw();

        GUI.DrawTexture(new Rect(0, 33, 900, 2), _lineTexure);
        GUI.DrawTexture(new Rect(0, 60, 900, 1), _lineTexure);
        GUI.DrawTexture(new Rect(330, 33, 2, 567), _lineTexure);
    }

    private static Texture2D _lineTexure;
    private void RefreshAssets(GuiView view)
    {
        AssetDataManager.ParseAssetDatas();
    }
}
