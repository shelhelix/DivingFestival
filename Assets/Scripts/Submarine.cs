using System;

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using LD48Project.Starter;

using GameComponentAttributes.Attributes;

namespace LD48Project {
	public class Submarine : GameplayComponent {
		public enum Subsystem {
			LeftShield = 0,
			Engine = 1,
			RightShield = 2
		}
		
		public enum Side {
			LeftSide = 0,
			RightSide = 1
		}
		
		const int TotalEnergyUnits = 5;
		
		public float MaxSubmarineSpeed = 1;
		public int   Hp = 5;
		public float Power = 100;
		
		[NotNullOrEmpty] public List<PowerView> PowerViews;

		[NotNull] public BasicItemView UsedPower;
		[NotNull] public BasicItemView SubmarineHp;

		readonly Dictionary<Side, Subsystem> SideToSystem = new Dictionary<Side, Subsystem> {
			{Side.LeftSide, Subsystem.LeftShield},
			{Side.RightSide, Subsystem.RightShield}
		};

		readonly Dictionary<Subsystem, int> EnergyDistribution = new Dictionary<Subsystem, int> {
			{Subsystem.LeftShield, 0},
			{Subsystem.Engine, TotalEnergyUnits},
			{Subsystem.RightShield, 0}
		};

		readonly Dictionary<Subsystem, (string up, string down)> SubsystemsControls =
			new Dictionary<Subsystem, (string, string)> {
				{Subsystem.LeftShield, ("q", "a")},
				{Subsystem.Engine, ("w", "s")},
				{Subsystem.RightShield, ("e", "d")}
			};

		public float CurSubmarineSpeed => MaxSubmarineSpeed * ((float)EnergyDistribution[Subsystem.Engine] / TotalEnergyUnits);
		
		int TotalUsedPower => EnergyDistribution.Sum(item => item.Value);
		
		void Update() {
			foreach ( var control in SubsystemsControls ) {
				if ( Input.GetKeyDown(control.Value.up) ) {
					if ( !TryAddPowerToSystem(control.Key) ) {
						RunRedAnimation(UsedPower.Background);
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

			UsedPower.Text.text = $"{TotalUsedPower.ToString()}/{MaxSubmarineSpeed}";
			SubmarineHp.Text.text = SubmarineHp.ToString();
			Power -= TotalUsedPower * Time.deltaTime;
			if ( (Power <= 0) || (Hp <= 0) ) {
				Debug.LogError("Need to end the game");
				// TODO: End game
			}
		}

		public override void Init(GameplayStarter starter) {
			foreach ( var powerView in PowerViews ) {
				powerView.UpKey.text   = SubsystemsControls[powerView.System].up;
				powerView.DownKey.text = SubsystemsControls[powerView.System].down;
				powerView.SystemName.text = powerView.System.ToString();
			}
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

		public void TakeDamage(Side side, int damage) {
			var systemName = SideToSystem[side];
			var shieldPower = EnergyDistribution[systemName];
			Hp -= Math.Max(damage - shieldPower, 0);
		}
	}
}