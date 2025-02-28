using Core.Events;
using UnityEngine;

namespace Game.Tiles {
	public class PlaySoundEvent: IGameEvent {
		public AudioClip Clip { get; private set; }

		public PlaySoundEvent(AudioClip clip) {
			Clip = clip;
		}
	}
}