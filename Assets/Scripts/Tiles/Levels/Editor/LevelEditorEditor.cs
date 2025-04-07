using Game.Testing;
using UnityEditor;
using UnityEngine;

namespace Game.Tiles.Levels.Editor {
	[CustomEditor(typeof(LevelEditor))]
	public class LevelEditorEditor: UnityEditor.Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			if (GUILayout.Button("Save")) {
				var levelEditor = (LevelEditor)target;
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