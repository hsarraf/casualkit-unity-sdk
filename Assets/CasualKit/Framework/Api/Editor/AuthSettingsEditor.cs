using UnityEditor;
using UnityEngine;

namespace CasualKit.Api.Auth.Menu
{

    public class AuthSettingsEditor : Editor
    {
        [MenuItem("CasualKit/Api/Auth/Settings")]
        public static void CreateMyAsset()
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/CasualKit/Framework/Api/Resources/AuthSettings.asset");
            EditorUtility.FocusProjectWindow();
        }
    }

}