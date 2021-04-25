using UnityEngine;

using System.Collections.Generic;

using LD48Project.Starter;

namespace LD48Project.UI {
	public class ShieldView : GameplayComponent {
		public Submarine.Side ShieldSide;
		public List<GameObject> ShieldElements;
		public override void Init(GameplayStarter starter) {
			var system = starter.Submarine.SideToSystem[ShieldSide];
			starter.Submarine.EnergyDistribution[system].OnValueChanged += OnValueChanged;
			OnValueChanged(starter.Submarine.EnergyDistribution[system].Value);
		}

		void OnValueChanged(int power) {
			power = Mathf.Clamp(power, 0, ShieldElements.Count);
			for ( var i = 0; i < ShieldElements.Count; i++ ) {
				ShieldElements[i].gameObject.SetActive(i < power);
			}
		}
	}
}