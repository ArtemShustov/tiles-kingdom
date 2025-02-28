using Game.Tiles.Levels.Utils;
using UnityEngine;

namespace Game.Tiles.Levels {
	[CreateAssetMenu(menuName = "Levels/Sequence")]
	public class SequenceLevel: Level {
		[SerializeField] private string _id = "levelSequence.id";
		[SerializeField] private Level[] _levels;
		private int _current = 0;
		
		public override void Build(LevelRoot root) {
			_current = PlayerPrefs.GetInt(_id, 0);
			root.gameObject.AddComponent<SequenceTrigger>().SetSequence(this);
			_current = Mathf.Clamp(_current, 0, _levels.Length - 1);
			_levels[_current].Build(root);
		}

		public void OnComplete() {
			_current += 1;
			PlayerPrefs.SetInt(_id, _current);
		}

		public void ResetCurrent() {
			_current = 0;
			PlayerPrefs.DeleteKey(_id);
		}
	}
}