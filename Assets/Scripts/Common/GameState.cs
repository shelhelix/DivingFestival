using System.Collections.Generic;
using LD48Project.Leaderboards;
using UnityEngine;

namespace LD48Project.Common {
	public class GameState : IState {
		public readonly LeaderboardState LeaderboardState;

		readonly List<IState> _states = new List<IState>();
		
		public GameState() {
			LeaderboardState = RegisterState(new LeaderboardState());
		}
		
		public void Load() {
			foreach ( var state in _states ) {
				state.Load();
			}
		}

		public void Save() {
			foreach ( var state in _states ) {
				state.Save();
			}
			PlayerPrefs.Save();
		}

		T RegisterState<T>(T state) where T : IState {
			_states.Add(state);
			return state;
		}
	}
}