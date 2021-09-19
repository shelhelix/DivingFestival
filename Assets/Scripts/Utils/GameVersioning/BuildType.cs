using UnityEditor;

namespace LD48Project.Utils.Editor.Build {
	public enum BuildType {
		Internal,
		Production
	}

	public static class MenuItems {
		[MenuItem("Diving Festival/Switch to internal")]
		static void SwitchToInternal() {
			foreach ( var buildTargetGroup in new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.WebGL } ) {
			}
		}
	}
}