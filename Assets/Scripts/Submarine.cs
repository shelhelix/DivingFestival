using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using LD48Project.Starter;

using GameComponentAttributes.Attributes;
using TMPro;
using UnityEngine.UI;

namespace LD48Project {
	public class Submarine : GameplayComponent {
		public enum Subsystem {
			LeftShield = 0,
			Engine = 1,
			RightShield = 2
		}
		
		const int TotalEnergyUnits = 5;
		
		public float MaxSubmarineSpeed = 1;

		[NotNullOrEmpty] public List<PowerView> PowerViews;

		[NotNull] public TMP_Text TotalPowerUse;
		[NotNull] public Image    TotalPowerImage;

		Dictionary<Subsystem, int> EnergyDistribution = new Dictionary<Subsystem, int> {
			{Subsystem.LeftShield, 0},
			{Subsystem.Engine, TotalEnergyUnits},
			{Subsystem.RightShield, 0}
		};

		Dictionary<Subsystem, (string up, string down)> SubsystemsControls =
			new Dictionary<Subsystem, (string, string)> {
				{Subsystem.LeftShield, ("q", "a")},
				{Subsystem.Engine, ("w", "s")},
				{Subsystem.RightShield, ("e", "d")}
			};
		
		
		public float CurSubmarineSpeed => MaxSubmarineSpeed * ((float)EnergyDistribution[Subsystem.Engine] / TotalEnergyUnits);
		
		int TotalUsedPower => EnergyDistribution.Sum(item => item.Value);

		public override void Init(GameplayStarter starter) {
			foreach ( var powerView in PowerViews ) {
				powerView.UpKey.text   = SubsystemsControls[powerView.System].up;
				powerView.DownKey.text = SubsystemsControls[powerView.System].down;
				powerView.SystemName.text = powerView.System.ToString();
			}
		}

		void Update() {
			foreach ( var control in SubsystemsControls ) {
				if ( Input.GetKeyDown(control.Value.up) ) {
					if ( !TryAddPowerToSystem(control.Key) ) {
						RunRedAnimation(TotalPowerImage);
					}
				}
				if ( Input.GetKeyDown(control.Value.down) ) {
					if ( !TrySubtractPower(control.Key) ) {
						var powerView = PowerViews.Find(x => x.System == control.Key);
						RunRedAnimation(powerView.BackgroundImage);
					}
				}
			}

			foreach ( var powerView in PowerViews ) {
				powerView.PowerUsageText.text = EnergyDistribution[powerView.System].ToString();
			}

			TotalPowerUse.text = $"{TotalUsedPower.ToString()}/{MaxSubmarineSpeed}";
		}

		bool TryAddPowerToSystem(Subsystem system) {
			if ( TotalUsedPower == TotalEnergyUnits ) {
				Debug.LogWarning($"Can't add power to {system}. All power is in use");
				return false;
			}
			EnergyDistribution[system]++;
			return true;
		}
		
		bool TrySubtractPower(Subsystem system) {
			if ( EnergyDistribution[system] == 0 ) {
				Debug.LogWarning($"Can't sub power. power for system {system} is zero");
				return false;
			}
			EnergyDistribution[system]--;
			return true;
		}

		void RunRedAnimation(Image image) {
			var seq = DOTween.Sequence();
			seq.Append(image.DOColor(Color.red, 0.3f));
			seq.Append(image.DOColor(Color.white, 0.3f));
		}
	}
}