using UnityEditor;
using UnityEngine;

namespace Game.Testing.Editor {
	[CustomEditor(typeof(NewLevelEditor))]
	public class NewLevelEditorEditor: UnityEditor.Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			if (GUILayout.Button("Save")) {
				var levelEditor = (NewLevelEditor)target;
				var level = levelEditor.GetLevel();
				var path = EditorUtility.SaveFilePanel(
					"Save new level",
					"",
					"NewLevel.asset",
					"asset"
				);

				if (path.Length == 0) {
					return;
				}
				if (path.StartsWith(Application.dataPath)) {
					path = "Assets" + path.Substring(Application.dataPath.Length);
				} else {
					Debug.LogError("Invalid path");
					return;
				}
				AssetDatabase.CreateAsset(level, path);
				AssetDatabase.SaveAssets();
			}
		}
	}
}