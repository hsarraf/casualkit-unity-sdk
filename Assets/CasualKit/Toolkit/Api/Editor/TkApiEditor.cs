using Casualkit.Toolkit.Api;
using CasualKit.Factory;
using UnityEditor;
using UnityEngine;


namespace CasualKit.Toolkit.Api.Menu
{

    public class TkApiEditor : Editor
    {
        [MenuItem("CasualKit/Api/Auth/TkRegister")]
        public static void TkRegister()
        {
            if (FindObjectOfType<TkApiModule>() == null)
                PrefabUtility.InstantiatePrefab(Resources.Load<TkApiModule>("TkApiModule"));
            if (FindObjectOfType<CKContext>() == null)
                PrefabUtility.InstantiatePrefab(Resources.Load<CKContext>("CKContext"));
        }
    }

    [InitializeOnLoad]
    class HierarchyIcon
    {
        static Texture2D _moduleIcon;
        static int _moduleId;

        static HierarchyIcon()
        {
            _moduleIcon = AssetDatabase.LoadAssetAtPath("Assets/CasualKit/Toolkit/Api/Editor/Icons/TkApiModuleIcon.png", typeof(Texture2D)) as Texture2D;

            EditorApplication.update += UpdateCB;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        } 

        static void UpdateCB()
        {
            TkApiModule tkLdr = Object.FindObjectOfType<TkApiModule>();

            if (tkLdr != null)
                _moduleId = tkLdr.gameObject.GetInstanceID();
        }

        static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            Rect r = new Rect(selectionRect);
            r.x = r.width;
            r.width = 64;
            if (_moduleId == instanceID)
                GUI.Label(r, _moduleIcon);
        }

    }

}