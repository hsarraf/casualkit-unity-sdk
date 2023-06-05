using CasualKit.Quick.Settings;
using UnityEditor;


namespace CasualKit.Quick.UI
{
    [CustomEditor(typeof(QuickSettings))]
    public class QuickSettingsEditor : Editor
    {
        [MenuItem("CasualKit/Quick/Settings")]
        public static void CreateMyAsset()
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/CasualKit/Framework/Quick/Resources/QuickSettings.asset");
            EditorUtility.FocusProjectWindow();
        }
    }

}