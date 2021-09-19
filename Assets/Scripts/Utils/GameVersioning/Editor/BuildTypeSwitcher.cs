using LD48Project.Utils.GameVersioning;
using UnityEditor;

namespace LD48Project.Utils.Editor.Build {
	public static class BuildTypeSwitcher {
		static readonly BuildTargetGroup[] SupportedGroups = {
			BuildTargetGroup.Android,
			BuildTargetGroup.iOS,
			BuildTargetGroup.Standalone,
			BuildTargetGroup.WebGL
		};
		
		[MenuItem("Diving Festival/Switch to internal")]
		static void SetInternalDefines() {
			GameVersionInternalScriptableObject.GetInstance().CurrentBuildType = BuildType.Internal;
			UpdateDefines();
		}
		
		[MenuItem("Diving Festival/Switch to production")]
		static void SetProductionDefines() {
			GameVersionInternalScriptableObject.GetInstance().CurrentBuildType = BuildType.Production;
			UpdateDefines();
		}
		
		public static void UpdateDefines() {
			var versionInfo = GameVersionInternalScriptableObject.GetInstance();
			foreach ( var supportedGroup in SupportedGroups ) {
				PlayerSettings.SetScriptingDefineSymbolsForGroup(supportedGroup, $"BUILD_TYPE_{versionInfo.CurrentBuildType.ToString().ToUpper()}");
			}
		}
	}
}