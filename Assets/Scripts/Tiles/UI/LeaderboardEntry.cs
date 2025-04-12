using Core;
using Core.LiteLocalization;
using TMPro;
using UnityEngine;

namespace Game.Tiles.UI {
	public class LeaderboardEntry: MonoBehaviour {
		[SerializeField] private TMP_Text _rank;
		[SerializeField] private TMP_Text _name;
		[SerializeField] private TMP_Text _score;
		[SerializeField] private LocalizeStringEvent _localizeName;
		private Player _player;
		
		public void Bind(Player player, int score, int rank) {
			_player = player;
			_score.text = score.ToString();
			_rank.text = $"#{rank}";
			if (player.HasFlag(PlayerFlags.Human)) {
				_name.text = _localizeName.GetLocalized();
				_localizeName.AddListener(SetName);
			} else {
				_name.text = $"<color={player.Color.ToHex()}>Player {player.Color.ToHex()}</color>";
				_localizeName.RemoveListener(SetName);
			}
		}

		private void SetName(string text) {
			_name.text = text;
		}
	}
}