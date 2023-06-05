using Casualkit.Toolkit.Model;
using CasualKit.Factory;
using UnityEditor;
using UnityEngine;


namespace CasualKit.Toolkit.Model.Menu
{

    public class TkModelEditor : Editor
    {
        [MenuItem("CasualKit/Model/TkModel")]
        public static void TkRegister()
        {
            if (FindObjectOfType<TkModelModule>() == null)
                PrefabUtility.InstantiatePrefab(Resources.Load<TkModelModule>("TkModelModule"));
            if (FindObjectOfType<CKContext>() == null)
                PrefabUtility.InstantiatePrefab(Resources.Load<CKContext>("CKContext"));
        }
        [MenuItem("CasualKit/Model/Clear")]
        public static void ClearData()
        {
            PlayerPrefs.DeleteAll();
        }
    }

    [InitializeOnLoad]
    class HierarchyIcon
    {
        static Texture2D _moduleIcon;
        static int _moduleId;

        static HierarchyIcon()
        {
            _moduleIcon = AssetDatabase.LoadAssetAtPath("Assets/CasualKit/Toolkit/Model/Editor/Icons/TkModelModuleIcon.png", typeof(Texture2D)) as Texture2D;

            EditorApplication.update += UpdateCB;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        }
        
        static void UpdateCB()
        {
            TkModelModule modelModule = Object.FindObjectOfType<TkModelModule>();

            if (modelModule != null)
                _moduleId = modelModule.gameObject.GetInstanceID();
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