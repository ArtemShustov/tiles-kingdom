using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Events;
using Game.Tiles.Effects;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles {
	// - Ничего не знаю. Фортран, например, не нужно знать, а джаву обязательно.
	// - А луа?
	// - Можно, но не обязательно
	public class Cell: MonoBehaviour {
		[Header("Settings")]
		[SerializeField] private bool _animationOnAwake = true;
		[SerializeField] private float _animationDuration = 0.25f;
		[SerializeField] private int _baseCaptureCost = 2;
		[Header("Components")]
		[field: SerializeField] protected SpriteRenderer Renderer { get; private set; }
		
		private readonly ReactiveProperty<Player> _owner = new ReactiveProperty<Player>();
		private readonly ReactiveProperty<Building> _building = new ReactiveProperty<Building>();
		private readonly List<ICellEffect> _effects = new List<ICellEffect>();
		public Vector2Int Position { get; private set; }
		public PlayGrid Grid { get; private set; }
		
		public IReadOnlyReactiveProperty<Player> Owner => _owner;
		public IReadOnlyReactiveProperty<Building> Building => _building;
		public IReadOnlyList<ICellEffect> Effects => _effects;

		private void Awake() {
			if (_animationOnAwake) {
				StartCoroutine(Animate(transform));
			}
		}
		public void Init(PlayGrid grid, Vector2Int position) {
			Grid = grid;
			Position = position;
		}
		
		public void Capture(Player player) {
			var prevOwner = _owner.Value;
			_owner.Value = player;
			EventBus<CellCapturedEvent>.Raise(new CellCapturedEvent(this, prevOwner,player));
			StartCoroutine(Animate(Renderer.transform));
		}
		public void SetBuilding(Building building) {
			_building.Value?.UnBind(this);
			_building.Value = building;
			_building.Value?.Bind(this);
		}
		public int GetCaptureCostFor(Player player) {
			var cost = _baseCaptureCost 
			       + (_building.Value?.CaptureCost ?? 0) 
			       + GetEffects<ICaptureCostEffect>().Sum(e => e.GetCaptureCostFor(player, this));
			return cost;
		}

		public void AddEffect(ICellEffect effect) {
			_effects.Add(effect);
		}
		public void RemoveEffect(ICellEffect effect) {
			_effects.Remove(effect);
		}
		public bool HasEffect<T>() where T: ICellEffect {
			return _effects.Any(e => e is T);
		}
		public IEnumerable<T> GetEffects<T>() where T: ICellEffect {
			return _effects.OfType<T>();
		}

		#region Events
		private void OnOwnerChanged() {
			Renderer.color = _owner.Value?.Color ?? Color.white;
		}

		private void OnEnable() {
			_owner.ValueChanged += OnOwnerChanged;
		}
		private void OnDisable() {
			_owner.ValueChanged += OnOwnerChanged;
		}
		#endregion

		private IEnumerator Animate(Transform transform) {
			var timer = 0f;
			var duration = 0.25f;
			while (timer < _animationDuration) {
				timer += Time.deltaTime;
				transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / duration);
				yield return new WaitForEndOfFrame();
			}
			transform.localScale = Vector3.one;
		}
	}
}