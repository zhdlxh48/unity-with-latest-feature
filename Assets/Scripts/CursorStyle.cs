using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
internal class CursorStyleData
{
    [SerializeField] private string styleName;
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 hotSpot;

    public string StyleName => styleName;
    public Texture2D CursorTexture => cursorTexture;
    public Vector2 HotSpot => hotSpot;
}

public class CursorStyle : MonoBehaviour
{
    [SerializeField] private List<CursorStyleData> cursorStyles;

    [SerializeField] private string defaultCursorStyle;
    private Dictionary<string, (Texture2D tex, Vector2 hotSpot)> _cursorStyleDic = new();

    private void Awake()
    {
        _cursorStyleDic =
            cursorStyles.ToDictionary(style => style.StyleName, style => (style.CursorTexture, style.HotSpot));

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        SetCursorStyle(defaultCursorStyle);
    }

    public void SetCursorStyle(string style)
    {
        var cursorStyle = _cursorStyleDic[style];
        print(cursorStyle.hotSpot);
        Cursor.SetCursor(cursorStyle.tex, cursorStyle.hotSpot, CursorMode.ForceSoftware);
    }
}