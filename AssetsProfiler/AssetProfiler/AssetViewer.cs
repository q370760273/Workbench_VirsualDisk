using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;

public class AssetViewer : EditorWindow
{
    [MenuItem("Custom/AssetViewer")]
    static void OpenTool()
    {
        AssetViewer window = GetWindow<AssetViewer>(false, "Asset Viewer", true);
        window.minSize = new Vector2(900f, 600f);
    }

    public DateTime OpenTime;
    private Texture2D _lineTexure;
    private GuiView _view;
    private GuiSearchTextField _searchTextField;
    private GuiLabel _timeLabel;
    private GuiSelectionGrid _assetDataSelectedGrid;
    private GuiSelectionGrid _dependenceSelectedGrid;
    private GuiFoldoutTree _assetDatafoldoutTree;
    private GuiFoldoutTree _dependencefoldoutTree;

    private void OnDestroy()
    {
        AssetDataManager.Instance.ApplyPattern("");
        LocalDataManager.Serialize(AssetDataManager.Instance.AssetDatas);
        EventHandler.DestoryInstance();
        AssetDataManager.DestoryInstance();
        AssetDatabase.Refresh();
    }

    void Awake()
    {
        OpenTime = DateTime.Now;
        EventHandler.CreateInstance();
        AssetDataManager.CreateInstance();
        RegisterHandlers();
        InitTexture();
        InitView();


        if (LocalDataManager.CheckExist())
        {
            AssetDataManager.Instance.AssetDatas = LocalDataManager.Deserialize() as AssetDatas;
        }
        else
        {
            AssetDataManager.Instance.BuildAllDatas();
        }

        RefreshView();
    }

    private void RegisterHandlers()
    {
        EventHandler.Instance.RegisterHandler(EventCode.ShowDependencies, ShowDependenciesInfo);
    }

    private void InitTexture()
    {
        _lineTexure = new Texture2D(1, 1);
        _lineTexure.SetPixel(1, 1, Color.black);
        _lineTexure.Apply();
    }

    private void InitView()
    {
        _view = new GuiView(new Rect(5, 5, 890, 590));

        GuiButton button = new GuiButton(new Rect(0, 0, 110, 20), "刷新资源数据库");
        button.RegisterHandler(RefreshAssets);
        _view.AddChild(button);

        button = new GuiButton(new Rect(340, 2, 16, 16), "?");
        button.RegisterHandler(ShowHelpInfo);
        _view.AddChild(button);

        _searchTextField = new GuiSearchTextField(new Rect(130, 1, 196, 20));
        _searchTextField.OnTextChange(OnSearchTextChange);
        _view.AddChild(_searchTextField);

        _timeLabel = new GuiLabel(new Rect(400, 0, 500, 20), "");
        _view.AddChild(_timeLabel);

        _assetDataSelectedGrid = new GuiSelectionGrid(new Rect(0, 35, 200, 20), new string[] { "资源列表", "无引用资源" }, new Action[] { ShowAllAssets, ShowUnusedAssets });
        _view.AddChild(_assetDataSelectedGrid);

        _dependenceSelectedGrid = new GuiSelectionGrid(new Rect(345, 35, 200, 20), new string[] { "资源依赖项", "反向引用" }, new Action[] { ShowDependencies, ShowRedependencies });
        _view.AddChild(_dependenceSelectedGrid);

        GuiScrollView scrollView = new GuiScrollView(new Rect(0, 60, 320, 530));
        _view.AddChild(scrollView);

        _assetDatafoldoutTree = new GuiFoldoutTree(new Rect(0, 60, 320, 3000));
        _assetDatafoldoutTree.AttachDrawer(new AssetDatasDrawer(_assetDatafoldoutTree));
        scrollView.AddChild(_assetDatafoldoutTree);

        scrollView = new GuiScrollView(new Rect(340, 60, 550, 530));
        _view.AddChild(scrollView);

        _dependencefoldoutTree = new GuiFoldoutTree(new Rect(340, 60, 320, 3000));
        _dependencefoldoutTree.AttachDrawer(new DependenceInfoDrawer(_dependencefoldoutTree));
        scrollView.AddChild(_dependencefoldoutTree);

    }

    void OnGUI()
    {
        if(_view != null)
            _view.Draw();

        GUI.DrawTexture(new Rect(0, 33, 900, 2), _lineTexure);
        GUI.DrawTexture(new Rect(0, 60, 900, 1), _lineTexure);
        GUI.DrawTexture(new Rect(330, 33, 2, 567), _lineTexure);
    }

    private void RefreshAssets(GuiView view)
    {
        AssetDataManager.Instance.BuildAllDatas();
        RefreshView();
    }

    public void RefreshView()
    {
        _timeLabel.SetText("您目前使用的是 " + AssetDataManager.Instance.AssetDatas.ChangeTime.ToString("yyyy/MM/dd  HH:mm:ss") + " 更新的资源数据库");
        _assetDataSelectedGrid.HandleSelected();
        _dependenceSelectedGrid.HandleSelected();
    }

    private void ShowHelpInfo(GuiView view)
    {
        EditorUtility.DisplayDialog("使用说明", HelpInformation.GetHelpInformation(), "确定");
    }

    private void OnSearchTextChange(string pattern)
    {
        AssetDataManager.Instance.ApplyPattern(pattern);
    }

    private void ShowAllAssets()
    {
        _assetDatafoldoutTree.Reset(AssetDataManager.Instance.GetAllAssetDatas());
    }

    private void ShowUnusedAssets()
    {
        if (ShowRefreshAssetDatasDialog())
            return;

        _assetDatafoldoutTree.Reset(AssetDataManager.Instance.GetAllUnusedAssetDatas());
    }

    private void ShowDependencies()
    {
        _dependencefoldoutTree.AttachDrawer(new DependenceInfoDrawer(_dependencefoldoutTree));
    }

    private void ShowRedependencies()
    {
        if (ShowRefreshAssetDatasDialog())
            return;

        _dependencefoldoutTree.AttachDrawer(new RedependenceInfoDrawer(_dependencefoldoutTree));
    }

    private void ShowDependenciesInfo(object[] param)
    {
        _dependencefoldoutTree.Reset(new AssetData[] { param[0] as AssetData });
    }

    private bool ShowRefreshAssetDatasDialog()
    {
        if (OpenTime > AssetDataManager.Instance.AssetDatas.ChangeTime)
        {
            if (EditorUtility.DisplayDialog("", "当前资源数据未更新，请先更新资源数据", "确定"))
            {
                RefreshAssets(null);
                return true;
            }
        }
        return false;
    }
}
