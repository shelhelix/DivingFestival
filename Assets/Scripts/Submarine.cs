using System;

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using LD48Project.Starter;

using GameComponentAttributes.Attributes;
using LD48Project.UI;
using LD48Project.Utils;

namespace LD48Project {
	public class Submarine : GameplayComponent {
		public enum Subsystem {
			LeftShield  = 0,
			Engine      = 1,
			RightShield = 2,
		}

		public enum Side {
			LeftSide  = 0,
			RightSide = 1
		}

		public const int TotalEnergyUnits = 5;

		const int DefaultBubbleEmission = 7;

		public float MaxSubmarineSpeed = 1;
		public float StartPower        = 100;
		public int   StartHp           = 5;

		public ReactiveValue<int> Hp = new ReactiveValue<int>();

		public ReactiveValue<float> CurPower = new ReactiveValue<float>();
		public ReactiveValue<float> Depth    = new ReactiveValue<float>();

		bool _stopEveryting;

		[NotNull] public BasicItemView UsedPower;

		[NotNull] public ParticleSystem EngineBubbles;

		[NotNull] public EndgameWindow EndgameWindow;

		[NotNull] public MobileSubmarineControl Control;
		
		public readonly Dictionary<Subsystem, ReactiveValue<int>> EnergyDistribution =
			new Dictionary<Subsystem, ReactiveValue<int>> {
				{Subsystem.LeftShield, new ReactiveValue<int>()},
				{Subsystem.Engine, new ReactiveValue<int>(TotalEnergyUnits)},
				{Subsystem.RightShield, new ReactiveValue<int>()}
			};

		public readonly Dictionary<Subsystem, string> SubsystemsControls =
			new Dictionary<Subsystem, string> {
				{Subsystem.LeftShield, "a"},
				{Subsystem.Engine, "s"},
				{Subsystem.RightShield, "d"}

			};

		public readonly Dictionary<Side, Subsystem> SideToSystem = new Dictionary<Side, Subsystem> {
			{Side.LeftSide, Subsystem.LeftShield},
			{Side.RightSide, Subsystem.RightShield}
		};

		public float CurSubmarineSpeed => MaxSubmarineSpeed * EnginePower;

		public int TotalUsedPower => EnergyDistribution.Sum(item => item.Value.Value);

		float EnginePower => ((float)EnergyDistribution[Subsystem.Engine].Value / TotalEnergyUnits);

		void Update() {
			if ( _stopEveryting ) {
				return;
			}
			ControlPowerWithKeys();
			UsedPower.Text.text =  $"{TotalUsedPower.ToString()}/{MaxSubmarineSpeed}";
			Depth.Value         += Time.deltaTime * EnginePower;
		}

		public void ControlPower(Subsystem subsystem) {
			if ( !TryAddPowerToSystem(subsystem) ) {
				RunRedAnimation(UsedPower.Background);
			}
		}

		public override void Init(GameplayStarter starter) {
			CurPower.Value                                      =  StartPower;
			Hp.Value                                            =  StartHp;
			Hp.OnValueChanged                                   += OnDied;
			EnergyDistribution[Subsystem.Engine].OnValueChanged += OnEnginePowerChanged;
			Control.Init(this);
		}
		
		public void RestoreSubmarine() {
			_stopEveryting = false;
			Hp.Value       = Mathf.Clamp(Hp.Value + 2, 2, StartHp);
			CurPower.Value = Mathf.Clamp(CurPower.Value + StartPower / 2, StartPower / 2, StartPower);
		}
		
		public void TakeDamage(Side side, int damage) {
			if ( _stopEveryting ) {
				return;
			}
			var systemName = SideToSystem[side];
			var shieldPower = EnergyDistribution[systemName].Value;
			Hp.Value -= Math.Max(damage - shieldPower, 0);
		}

		void OnDied(int value) {
			if ( value > 0 ) {
				return;
			}
			_stopEveryting = true;
			EndgameWindow.Init(this);
		}

		void ControlPowerWithKeys() {
			foreach ( var control in SubsystemsControls ) {
				if ( Input.GetKeyDown(control.Value) ) {
					ControlPower(control.Key);
				}
			}
		}

		bool TryAddPowerToSystem(Subsystem system) {
			if ( EnergyDistribution[system].Value == TotalEnergyUnits ) {
				Debug.LogWarning($"Can't add power to {system}. All power is in use");
				return false;
			}

			Subsystem? maxSystem = null;
			var maxEnergy = 0;
			foreach ( var energy in EnergyDistribution ) {
				if ( (energy.Value.Value > maxEnergy) && (energy.Key != system) ) {
					maxEnergy = energy.Value.Value;
					maxSystem = energy.Key;
				}
			}
			if ( maxSystem.HasValue ) {
				EnergyDistribution[maxSystem.Value].Value--;
			}
			EnergyDistribution[system].Value++;
			return true;
		}

		void RunRedAnimation(Image image) {
			var seq = DOTween.Sequence();
			seq.Append(image.DOColor(Color.red, 0.3f));
			seq.Append(image.DOColor(Color.white, 0.3f));
		}

		void OnEnginePowerChanged(int newPower) {
			var emission = EngineBubbles.emission;
			emission.rateOverTimeMultiplier = DefaultBubbleEmission * ((float)newPower / TotalEnergyUnits);
		}
	}
}