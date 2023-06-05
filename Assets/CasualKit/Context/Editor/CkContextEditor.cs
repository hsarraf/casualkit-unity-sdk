using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Casualkit.Toolkit.Api;
using CasualKit.Factory;
using CasualKit.Toolkit.Loader;

[InitializeOnLoad]
class CkContextEditor
{
    static Texture2D _ckContextIcon;
    static int _ckContextId;

    static CkContextEditor()
    {
        _ckContextIcon = AssetDatabase.LoadAssetAtPath("Assets/CasualKit/Context/Editor/Icons/CKContextIcon.png", typeof(Texture2D)) as Texture2D;

        EditorApplication.update += UpdateCB;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
    }

    static void UpdateCB()
    {
        CKContext ckCtx = Object.FindObjectOfType<CKContext>();

        if (ckCtx != null)
            _ckContextId = ckCtx.gameObject.GetInstanceID();
    }

    static void HierarchyItemCB(int instanceID, Rect selectionRect)
    {
        Rect r = new Rect(selectionRect);
        r.x = r.width - 0;
        r.width = 64;
        if (_ckContextId == instanceID)
            GUI.Label(r, _ckContextIcon);
    }

}