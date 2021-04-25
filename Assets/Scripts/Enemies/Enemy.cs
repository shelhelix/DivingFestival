using UnityEngine;

using System;

using DG.Tweening;
using GameComponentAttributes;

namespace LD48Project.Enemies {
	public class Enemy : GameComponent {
		public int            Damage = 3;
		public Submarine.Side Side;
		public float          IncomeTime = 1f;
		public float          StartDelay = 1f;
		
		Vector3 _startPoint;

		public event Action<Enemy> OnRunCompleted;
		
		protected override void Awake() {
			base.Awake();
			_startPoint = transform.position;
		}
		
		void OnCollisionEnter2D(Collision2D other) {
			DealDamage(other.gameObject.GetComponent<Submarine>());
		}

		public void Run(Transform target, float warningTime) {
			var seq = DOTween.Sequence();
			seq.SetDelay(warningTime);
			seq.Append(transform.DOMove(target.position, IncomeTime));
			seq.Append(transform.DOMove(_startPoint, IncomeTime));
			seq.AppendCallback(() => {
				OnRunCompleted?.Invoke(this);
			});
		}

		void DealDamage(Submarine submarine) {
			submarine.TakeDamage(Side, Damage);
		}
	}
}