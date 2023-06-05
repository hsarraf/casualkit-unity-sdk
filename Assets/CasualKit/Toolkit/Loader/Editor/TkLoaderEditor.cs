using Casualkit.Toolkit.Api;
using Casualkit.Toolkit.Loader;
using CasualKit.Factory;
using UnityEditor;
using UnityEngine;


namespace CasualKit.Toolkit.Loader.Menu
{

    public class TkLoaderEditor : Editor
    {
        [MenuItem("CasualKit/Loader/TkLoader")]
        public static void TkRegister()
        {
            if (FindObjectOfType<TkRegisterPanel>() == null)
                PrefabUtility.InstantiatePrefab(Resources.Load<TkRegisterPanel>("TkRegisterPanel"));
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

        static Texture2D _registerPanelIcon;
        static int _registerPanelId;

        static HierarchyIcon()
        {
            _moduleIcon = AssetDatabase.LoadAssetAtPath("Assets/CasualKit/Toolkit/Loader/Editor/Icons/TkLoaderModuleIcon.png", typeof(Texture2D)) as Texture2D;
            _registerPanelIcon = AssetDatabase.LoadAssetAtPath("Assets/CasualKit/Toolkit/Loader/Editor/Icons/TkRegisterPanelIcon.png", typeof(Texture2D)) as Texture2D;

            EditorApplication.update += UpdateCB;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        }
        
        static void UpdateCB()
        {
            TkLoaderModule loaderModule = Object.FindObjectOfType<TkLoaderModule>();
            TkRegisterPanel registerPanel = Object.FindObjectOfType<TkRegisterPanel>();

            if (loaderModule != null)
                _moduleId = loaderModule.gameObject.GetInstanceID();
            if (registerPanel != null)
                _registerPanelId = registerPanel.gameObject.GetInstanceID();
        }

        static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            Rect r = new Rect(selectionRect);
            r.x = r.width;
            r.width = 64;
            if (_moduleId == instanceID)
                GUI.Label(r, _moduleIcon);
            else if (_registerPanelId == instanceID)
                GUI.Label(r, _registerPanelIcon);
        }

    }

}