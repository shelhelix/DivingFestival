using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using GameComponentAttributes;

namespace LD48Project.UI.PowerPanel {
	public class PowerBar : BasePowerBar {
		public List<Image> Sections;

		public override void SetValue(int value) {
			value = Mathf.Clamp(value, 0, Sections.Count);
			for ( var i = 0; i < Sections.Count; i++ ) {
				Sections[i].color = (i < value) ? Color.green : Color.gray;
			}
		} 
	}
}