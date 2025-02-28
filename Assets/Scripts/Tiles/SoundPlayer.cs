using Core.Events;
using UnityEngine;

namespace Game.Tiles {
	public class SoundPlayer: MonoBehaviour {
		[SerializeField] private float _basePitch = 1;
		[SerializeField] private float _randomPitch = 0.1f;
		[SerializeField] private AudioSource _source;

		private void OnEvent(PlaySoundEvent gameEvent) {
			_source.pitch = _basePitch + Random.Range(-_randomPitch, _randomPitch);
			_source.PlayOneShot(gameEvent.Clip);
		}
		private void OnEnable() {
			EventBus<PlaySoundEvent>.Event += OnEvent;
		}
		private void OnDisable() {
			EventBus<PlaySoundEvent>.Event -= OnEvent;
		}
	}
}