using UnityEngine;

namespace Game {
	public class BackgroundMusic: MonoBehaviour {
		[SerializeField] private AudioClip[] _clips;
		[SerializeField] private AudioSource _source;

		public bool Paused { get; private set; } = false;
		private int _track = 0;

		public void ChangeTrack(int index) {
			_track = Mathf.Clamp(index, 0, _clips.Length - 1);
			_source.clip = _clips[_track];
			if (Paused) {
				_source.Stop();
			} else {
				_source.Play();
			}
		}
		public void NextTrack() {
			var index = _track + 1 >= _clips.Length ? 0 : _track + 1;
			ChangeTrack(index);
		}
		public void Toggle() {
			Paused = !Paused;
			if (Paused) {
				_source.Stop();
			} else {
				_source.Play();
			}
		}
		public void Play() {
			Paused = false;
			_source.Play();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InitializeOnLoad() {
			var prefab = Resources.Load<GameObject>("BackgroundMusic");
			if (prefab == null) {
				return;
			}
			var instance = GameObject.Instantiate(prefab);
			instance.name = "BackgroundMusic";
			DontDestroyOnLoad(instance);
			var backgroundMusic = instance.GetComponent<BackgroundMusic>();
			backgroundMusic.ChangeTrack(0);
			backgroundMusic.Play();
		}
	}
}