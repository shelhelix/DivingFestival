using System.IO;
using LD48Project.Utils.GameVersioning;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LD48Project.Utils.Editor.Build {
	public class BuildPreprocessor : IPreprocessBuildWithReport {
		public int callbackOrder { get; } = 1;
		
		public void OnPreprocessBuild(BuildReport report) {
			var version = GameVersion.Instance;
			version.BumpInternalVersion();
			BuildTypeSwitcher.UpdateDefines();
			if ( report.summary.platform == BuildTarget.Android ) {
				AndroidPrepareSteps();
			}
			Debug.Log($"Bumping internal version to {version.ConvertVersionToString()} ({version.ConvertVersionToBundleCode()})");
		}

		void AndroidPrepareSteps() {
			PlayerSettings.Android.bundleVersionCode = GameVersion.Instance.ConvertVersionToBundleCode();
			var password = File.ReadAllText("./Keys/keyPassword.txt");
			Debug.Log($"Signing with pass {password} and keystore {PlayerSettings.Android.keystoreName} and key alias {PlayerSettings.Android.keyaliasName}");
			PlayerSettings.Android.keyaliasPass = password;
			PlayerSettings.Android.keystorePass = password;
		}
	}
}