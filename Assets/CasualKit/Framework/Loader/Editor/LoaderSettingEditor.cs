using CasualKit.Quick.Settings;
using UnityEditor;


namespace CasualKit.Loader.Settings
{
    [CustomEditor(typeof(QuickSettings))]
    public class LoaderSettingEditor : Editor
    {
        [MenuItem("CasualKit/Loader/Settings")]
        public static void CreateMyAsset()
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/CasualKit/Framework/Loader/Resources/LoaderSettings.asset");
            EditorUtility.FocusProjectWindow();
        }
    }

}