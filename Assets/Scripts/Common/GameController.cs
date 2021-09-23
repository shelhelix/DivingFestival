using System;
using System.Collections.Generic;
using LD48Project.Leaderboards;
using LD48Project.Utils;

namespace LD48Project.Common {
	public class GameController : Singleton<GameController> {
		GameState _gameState;

		readonly Dictionary<Type, object> _controllers = new Dictionary<Type, object>();

		public bool IsActive => _gameState != null;
		
		public LeaderboardController LeaderboardController { get; private set; }

		public void Activate() {
			if ( IsActive ) {
				return;
			}
			_gameState = new GameState();
			_gameState.Load();
			AddControllers(_gameState);
		}

		void AddControllers(GameState state) {
			LeaderboardController = RegisterController(new LeaderboardController(state));
		}

		T RegisterController<T>(T instance) where T : class {
			_controllers.Add(typeof(T), instance);
			return instance;
		}
	}
}