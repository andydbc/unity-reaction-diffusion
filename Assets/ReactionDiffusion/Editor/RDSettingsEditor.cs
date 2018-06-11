using UnityEditor;
using UnityEngine;

namespace ReactionDiffusion
{
    public class RDSettingsEditor : MonoBehaviour
    {
        [MenuItem("Assets/Create/Reaction Diffusion Settings")]
        public static void CreateMyAsset()
        {
            RDSettings asset = ScriptableObject.CreateInstance<RDSettings>();

            AssetDatabase.CreateAsset(asset, "Assets/RDSettings.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
