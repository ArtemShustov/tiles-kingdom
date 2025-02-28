using UnityEditor;
using UnityEngine;

namespace Game.Tiles.Levels.Editor {
	[CustomEditor(typeof(SequenceLevel))]
	public class SequenceLevelEditor: UnityEditor.Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			var level = (SequenceLevel)target;
			if (GUILayout.Button("Reset")) {
				level.ResetCurrent();
			}
		}
	}
}