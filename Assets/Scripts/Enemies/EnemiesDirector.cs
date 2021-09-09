using UnityEngine;

using System.Collections.Generic;

using DG.Tweening;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;

namespace LD48Project.Enemies {
	public class EnemiesDirector : GameComponent {
		[NotNullOrEmpty] public List<EnemyWithSignContainer> Enemies;
		[NotNull] public Transform Submarine;

		const float WaveTimeChange = 0.1f;

		int _waveIndex;
		
		public float StartDelay = 5;
		
		public float WarningBlinkingTime = 0.5f;
		public int WarningPeriods = 3;
		
		public float MinSpawnTime;
		public float MaxSpawnTime;
		
		float _nextWaveTimer;
		float _startTimer;
		
		bool _isRunning;

		protected override void Awake() {
			base.Awake();
			foreach ( var enemy in Enemies ) {
				enemy.WarningSign.SetActive(false);
			}
			_startTimer = StartDelay;
		}

		void Update() {
			_startTimer -= Time.deltaTime;
			if ( _startTimer > 0f ) {
				return;
			}
			if ( _isRunning ) {
				return;
			}
			_nextWaveTimer -= Time.deltaTime;
			if ( _nextWaveTimer > 0f ) {
				return;
			}
			TryStartNewEnemy();
			ResetTimer();
		}

		void TryStartNewEnemy() {
			_waveIndex++;
			var randomEnemyIndex = Random.Range(0, Enemies.Count);
			var container        = Enemies[randomEnemyIndex];
			var warningTime      = Mathf.Max(WarningPeriods * WarningBlinkingTime - WaveTimeChange * _waveIndex, 0.2f);
			container.Enemy.Run(Submarine.transform, warningTime);
			container.Enemy.OnRunCompleted += OnEnemyWaveCompleted;
			CreateWarningSeq(container.WarningSign);
			_isRunning = true;
		}

		void CreateWarningSeq(GameObject warningRoot) {
			var seq = DOTween.Sequence();
			seq.InsertCallback(0f, () => warningRoot.SetActive(false));
			seq.InsertCallback(WarningBlinkingTime / 2, () => warningRoot.SetActive(true));
			seq.InsertCallback(WarningBlinkingTime, () => warningRoot.SetActive(true));
			seq.SetLoops(WarningPeriods);
			seq.OnComplete(() => warningRoot.SetActive(false));
		}

		void ResetTimer() {
			_nextWaveTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
		}

		void OnEnemyWaveCompleted(Enemy enemy) {
			enemy.OnRunCompleted -= OnEnemyWaveCompleted;
			_isRunning = false;
		}
	}
}