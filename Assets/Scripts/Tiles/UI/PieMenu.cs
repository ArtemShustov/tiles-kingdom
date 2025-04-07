using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Tiles.UI {
	public class PieMenu: MonoBehaviour {
		[SerializeField] private Transform _container;
		[SerializeField] private float _radius = 0.3f;
		[SerializeField] private PieMenuItem[] _items;
		[SerializeField] private float _animationDuration = 0.5f;
		[SerializeField] private CanvasGroup _group;
		private CancellationTokenSource _animation;
		
		public IReadOnlyCollection<PieMenuItem> Items => _items;
		public event Action<PieMenuItem> Selected;
		
		public void Show() {
			_animation?.Cancel();
			_animation = new CancellationTokenSource();
			ShowAnimate().Forget();
		}
		public void Hide() {
			_animation?.Cancel();
			_animation = new CancellationTokenSource();
			HideAnimate().Forget();
		}
		public void HideNow() {
			_container.gameObject.SetActive(false);
		}
		
		private void UpdatePositions() {
			var deg = 360f / _items.Length;
			for (var i = 0; i < _items.Length; i++) {
				var position = Quaternion.Euler(0, 0, i * deg) * Vector3.up * _radius;
				_items[i].transform.localPosition = position;
			}
		}
		private async Task ShowAnimate() {
			_container.gameObject.SetActive(true);
			var t = 0f;
			while (t < _animationDuration) {
				_container.transform.localScale = Vector3.one * Mathf.Lerp(0, 1, t / _animationDuration);
				_container.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(180, 0, t / _animationDuration));
				t += Time.deltaTime;
				await Awaitable.NextFrameAsync(_animation.Token);
			}
			_group.interactable = true;
			_container.transform.localScale = Vector3.one;
			_container.transform.localRotation = Quaternion.identity;
		}
		private async Task HideAnimate() {
			_group.interactable = false;
			var t = 0f;
			while (t < _animationDuration) {
				_container.transform.localScale = Vector3.one * Mathf.Lerp(1, 0, t / _animationDuration);
				_container.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -180, t / _animationDuration));
				t += Time.deltaTime;
				await Awaitable.NextFrameAsync(_animation.Token);
			}
			_container.transform.localScale = Vector3.zero;
			_container.transform.localRotation = Quaternion.identity;
			_container.gameObject.SetActive(false);
		}
		private void OnItemSelected(PieMenuItem item) {
			Selected?.Invoke(item);
		}
		private void OnEnable() {
			UpdatePositions();
			foreach (var item in _items) {
				item.Selected += OnItemSelected;
			}
		}
		private void OnDisable() {
			foreach (var item in _items) {
				item.Selected -= OnItemSelected;
			}
		}
		private void OnValidate() {
			if (_container && _items != null) {
				UpdatePositions();
			}
		}
	}
}