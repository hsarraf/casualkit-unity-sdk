using CasualKit.Api.Settings;
using UnityEditor;


namespace CasualKit.Api.UI
{

    [CustomEditor(typeof(ApiSettings))]
    public class AuthSettingsEditor : Editor
    {
        [MenuItem("CasualKit/Api/Settings")]
        public static void CreateMyAsset()
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/CasualKit/Framework/Api/Resources/ApiSettings.asset");
            EditorUtility.FocusProjectWindow();
        }
    }

}