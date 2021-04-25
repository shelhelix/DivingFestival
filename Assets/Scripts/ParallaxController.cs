using UnityEngine;

using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;

namespace LD48Project {
	public class ParallaxController : GameComponent {
		[NotNull] 
		public Submarine Submarine;
		
		[Serializable]
		public class ParallaxObject {
			public Transform Object;
			public float     SpeedMultiplier = 1;
		}
    
		public List<ParallaxObject> Objects;
		
		void Update() {
			var offset = Submarine.CurSubmarineSpeed * Time.deltaTime;
			foreach ( var obj in Objects ) {
				obj.Object.position += (offset * obj.SpeedMultiplier) * Vector3.up;
			}
		}
	}

	
}