using UnityEngine;

using System;

using GameComponentAttributes.Attributes;

namespace LD48Project.Enemies {
	[Serializable]
	public class EnemyWithSignContainer {
		[NotNull] public Enemy Enemy;
		[NotNull] public GameObject WarningSign;
	}
}