using Core.Events;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles.Levels.Utils {
	public class SequenceTrigger: MonoBehaviour {
		private SequenceLevel _sequence;
		
		public void SetSequence(SequenceLevel sequence) {
			_sequence = sequence;
		}
		
		private void OnWin(PlayerWinEvent gameEvent) {
			_sequence.OnComplete();
		}
		private void OnEnable() {
			EventBus<PlayerWinEvent>.Event += OnWin;
		}
		private void OnDestroy() {
			EventBus<PlayerWinEvent>.Event -= OnWin;
		}
	}
}