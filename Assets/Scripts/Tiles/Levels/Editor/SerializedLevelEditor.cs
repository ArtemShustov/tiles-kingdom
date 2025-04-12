using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Game.Tiles.Levels.Editor {
	[CustomEditor(typeof(SerializedLevel), false)]
	public class SerializedLevelEditor: UnityEditor.Editor {
		private readonly List<Type> _derivedTypes;

		public SerializedLevelEditor() {
			Type baseType = typeof(SerializedLevel);
			_derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract)
				.ToList();
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			SerializedLevel level = (SerializedLevel)target;

			if (GUILayout.Button("🔄 Convert to derived type")) {
				ShowConversionMenu(level);
			}
		}

		void ShowConversionMenu(SerializedLevel level) {
			GenericMenu menu = new GenericMenu();

			foreach (var type in _derivedTypes) {
				if (type == level.GetType()) continue;

				menu.AddItem(new GUIContent(type.Name), false, () => {
					ConvertAsset(level, type);
				});
			}

			menu.ShowAsContext();
		}

		void ConvertAsset(SerializedLevel original, Type newType) {
			string path = AssetDatabase.GetAssetPath(original);

			var players = GetPrivateField<Color[]>(original, "_players");
			var cells = GetPrivateField<SerializedLevel.CellData[]>(original, "_cells");
			Debug.Log(players.Length);

			DestroyImmediate(original, true);
			AssetDatabase.SaveAssets();

			SerializedLevel newAsset = (SerializedLevel)ScriptableObject.CreateInstance(newType);
			SetPrivateField(newAsset, "_players", players);
			SetPrivateField(newAsset, "_cells", cells);

			AssetDatabase.CreateAsset(newAsset, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = newAsset;

			Debug.Log($"Converted to {newType.Name} successfully!");
		}

		T GetPrivateField<T>(object obj, string fieldName) {
			return (T)obj.GetType()
				.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
				?.GetValue(obj);
		}

		void SetPrivateField(object obj, string fieldName, object value) {
			var t = obj.GetType();
			while (t != typeof(object) && t != null) {
				var field = t.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				if (field != null) {
					field.SetValue(obj, value);
					return;
				}
				t = t.BaseType;
			}
		}
	}
}