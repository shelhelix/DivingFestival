using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using LD48Project.Starter;
using LD48Project.UI;
using LD48Project.Utils;

using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameComponentAttributes.Attributes;
using LD48Project.Leaderboards;

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
		public float InvincibleTimeSec = 2;
		public int   StartHp           = 5;

		public ReactiveValue<int> Hp = new ReactiveValue<int>();

		public ReactiveValue<float> Depth    = new ReactiveValue<float>();

		bool _stopEveryting;
		bool _isInvincible;

		LeaderboardController _leaderboardController;

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
			Hp.Value                                            =  StartHp;
			Hp.OnValueChanged                                   += OnDied;
			EnergyDistribution[Subsystem.Engine].OnValueChanged += OnEnginePowerChanged;
			_leaderboardController                              =  starter.LeaderboardController;
			Control.Init(this);
		}
		
		public void RestoreSubmarine() {
			_stopEveryting = false;
			Hp.Value       = Mathf.Clamp(Hp.Value + 2, 2, StartHp);
			UniTask.Void(MakeInvincibleForTime);
		}
		
		public void TakeDamage(Side side, int damage) {
			if ( _stopEveryting || _isInvincible ) {
				return;
			}
			var systemName  = SideToSystem[side];
			var shieldPower = EnergyDistribution[systemName].Value;
			Hp.Value -= Math.Max(damage - shieldPower, 0);
		}

		async UniTaskVoid MakeInvincibleForTime() {
			_isInvincible = true;
			await UniTask.Delay(TimeSpan.FromSeconds(InvincibleTimeSec));
			_isInvincible = false;
		}

		void OnDied(int value) {
			if ( value > 0 ) {
				return;
			}
			_stopEveryting = true;
			EndgameWindow.Init(_leaderboardController, this);
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