using System;
using LD48Project.Common;
using UnityEngine;

namespace LD48Project.Leaderboards {
	public class LeaderboardState : IState {
		public string UserName;
		public Guid   UserGuid;
		
		public void Load() {
			UserName = PlayerPrefs.GetString("user_name");
			UserGuid = LoadGuid("user_guid");
		}

		public void Save() {
			PlayerPrefs.SetString("user_name", UserName);
			PlayerPrefs.SetString("user_guid", UserGuid.ToString());
		}

		Guid LoadGuid(string prefsName) {
			var guid = PlayerPrefs.GetString(prefsName);
			return string.IsNullOrEmpty(guid) ? Guid.Empty : new Guid(guid);
		}
	}
}