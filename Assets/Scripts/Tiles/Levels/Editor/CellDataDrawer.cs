using System;
using Core;
using UnityEditor;
using UnityEngine;

namespace Game.Tiles.Levels.Editor {
	[CustomPropertyDrawer(typeof(SerializedLevel.CellData))]
	public class CellDataDrawer : PropertyDrawer {
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 4;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			Rect r = position;
			r.height = EditorGUIUtility.singleLineHeight;
			
			var positionProp = property.FindPropertyRelative("<Position>k__BackingField");
			EditorGUI.PropertyField(r, positionProp);
			r.y += r.height + 2;

			var buildingProp = property.FindPropertyRelative("<Building>k__BackingField");
			EditorGUI.PropertyField(r, buildingProp);
			r.y += r.height + 2;

			var ownerProp = property.FindPropertyRelative("<Owner>k__BackingField");
			var ownedProp = property.FindPropertyRelative("<Owned>k__BackingField");
			if (property.serializedObject.targetObject is SerializedLevel level && level.Players.Count != 0) {
				ownerProp.intValue = EditorGUI.Popup(r, "Owner", ownerProp.intValue + 1, GetPlayerNames(level)) - 1;
				ownedProp.boolValue = ownerProp.intValue != -1;
			} else {
				EditorGUI.PropertyField(r, ownedProp);
				EditorGUI.PropertyField(r, ownerProp);
			}

			EditorGUI.EndProperty();
		}

		private string[] GetPlayerNames(SerializedLevel level) {
			var colors = level.Players;

			if (colors == null) return Array.Empty<string>();

			string[] names = new string[colors.Count + 1];
			names[0] = "None";
			for (int i = 1; i < colors.Count + 1; i++) {
				names[i] = $"Player {i - 1} ({colors[i - 1].ToHex()})";
			}
			return names;
		}
	}
}