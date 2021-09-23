using System.Collections.Generic;
using GameComponentAttributes;

namespace LD48Project.Starter {
	public abstract class GameplayComponent : GameComponent {
		public static List<GameplayComponent> Instances = new List<GameplayComponent>();

		void OnEnable() {
			Instances.Add(this);
		}

		void OnDisable() {
			Instances.Remove(this);
		}

		public abstract void Init(GameplayStarter starter);
	}
}