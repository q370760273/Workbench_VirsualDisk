using UnityEngine;
using System.Collections;
using UnityEditor;

public class GuiSearchTextField : GuiTextField
{
    private GUIStyle TextFieldRoundEdge;
    private GUIStyle TextFieldRoundEdgeCancelButton;
    private GUIStyle TextFieldRoundEdgeCancelButtonEmpty;
    private GUIStyle TransparentTextField;
    private Rect _tipsRect;
    private Rect _cancleBtnRect;
    private Rect _searchTextRect;

    public GuiSearchTextField(Rect rect) : base(rect)
    {
        TextFieldRoundEdge = new GUIStyle("SearchTextField"); //设置圆角style的GUIStyle
        TextFieldRoundEdgeCancelButton = new GUIStyle("SearchCancelButton");
        TextFieldRoundEdgeCancelButtonEmpty = new GUIStyle("SearchCancelButtonEmpty");
        //设置输入框的GUIStyle为透明，所以看到的“输入框”是TextFieldRoundEdge的风格
        TransparentTextField = new GUIStyle(EditorStyles.whiteLabel);
        TransparentTextField.normal.textColor = EditorStyles.textField.normal.textColor;

        InitRect();
    }

    private void InitRect()
    {
        _tipsRect = _rect;
        _tipsRect.width -= TextFieldRoundEdgeCancelButton.fixedWidth;

        _cancleBtnRect = new Rect();
        _cancleBtnRect.width = TextFieldRoundEdgeCancelButton.fixedWidth;
        _cancleBtnRect.height = TextFieldRoundEdgeCancelButton.fixedHeight;
        _cancleBtnRect.x = _rect.x + _rect.width - _cancleBtnRect.width;
        _cancleBtnRect.y = _rect.y;

        float num = TextFieldRoundEdge.CalcSize(new GUIContent("")).x - 2f; //为了空出左边那个放大镜的位置
        _searchTextRect = new Rect();
        _searchTextRect.width = _rect.width - num - TextFieldRoundEdgeCancelButton.fixedWidth;
        _searchTextRect.height = _rect.height;
        _searchTextRect.x = _rect.x + num;
        _searchTextRect.y = _rect.y + 1f;//为了和后面的style对其
    }

    public override void Draw()
    {
        //选择取消按钮(x)的GUIStyle
        GUIStyle cancelBtnStyle = (_inputText != "") ? TextFieldRoundEdgeCancelButton : TextFieldRoundEdgeCancelButtonEmpty;

        //如果面板重绘
        if (Event.current.type == EventType.Repaint)
        {
            //根据是否是专业版来选取颜色
            GUI.contentColor = (EditorGUIUtility.isProSkin ? Color.black : new Color(0f, 0f, 0f, 0.5f));
            //当没有输入的时候提示“请输入”
            if (string.IsNullOrEmpty(_inputText))
            {
                TextFieldRoundEdge.Draw(_tipsRect, new GUIContent("资源类型过滤"), 0);
            }
            else
            {
                TextFieldRoundEdge.Draw(_tipsRect, new GUIContent(""), 0);
            }
            //因为是“全局变量”，用完要重置回来
            GUI.contentColor = Color.white;
        }

        _inputText = EditorGUI.TextField(_searchTextRect, _inputText, TransparentTextField);

        //绘制取消按钮，位置要在输入框右边
        if (GUI.Button(_cancleBtnRect, GUIContent.none, cancelBtnStyle) && _inputText != "")
        {
            _inputText = "";
            //用户是否做了输入
            GUI.changed = true;
            //把焦点移开输入框
            GUIUtility.keyboardControl = 0;
        }

        if (_handle != null && _inputText != _lastInputText)
        {
            _lastInputText = _inputText;
            _handle(_inputText);
        }
    }
}
