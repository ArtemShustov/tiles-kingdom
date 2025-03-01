using UnityEditor;
using UnityEngine;

namespace Game.Editor {
	[CustomEditor(typeof(BackgroundMusic))]
	public class BackgroundMusicEditor: UnityEditor.Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			
			if (!Application.isPlaying) {
				return;
			}
			BackgroundMusic music = (BackgroundMusic)target;
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(music.Paused ? "Resume" : "Pause")) {
				music.Toggle();
			}
			if (GUILayout.Button("NextTrack")) {
				music.NextTrack();
			}
			GUILayout.EndHorizontal();
		}
	}
}