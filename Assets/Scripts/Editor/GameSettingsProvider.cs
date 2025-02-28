using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Editor {
	public class GameSettingsProvider: SettingsProvider {
		private GameSettings _settings;
		private SerializedObject _serialized;
		
		private const string MenuPath = "Custom/Game Settings";

		public GameSettingsProvider(GameSettings settings): base(MenuPath, SettingsScope.Project) {
			_settings = settings;
		}
		
		[SettingsProvider]
		public static SettingsProvider CreateProvider() {
			if (!AssetDatabase.AssetPathExists($"Assets/Resources/{GameSettings.DefaultPath}.asset")) {
				CreateDefaultSettings();
			}
			var settings = AssetDatabase.LoadAssetAtPath<GameSettings>($"Assets/Resources/{GameSettings.DefaultPath}.asset");
			var provider = new GameSettingsProvider(settings);
			return provider;
		}
		private static void CreateDefaultSettings() {
			if (!Directory.Exists("Assets/Resources")) {
				Directory.CreateDirectory("Assets/Resources");
			}

			var settings = ScriptableObject.CreateInstance<GameSettings>();
			AssetDatabase.CreateAsset(settings, $"Assets/Resources/{GameSettings.DefaultPath}.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public override void OnGUI(string searchContext) {
			EditorGUILayout.PropertyField(_serialized.FindProperty("_targetFps"));
		}
		public override void OnActivate(string searchContext, VisualElement rootElement) {
			_serialized = new SerializedObject(_settings);
		}
	}
}