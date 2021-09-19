#if UNITY_EDITOR
using UnityEditor;
#endif
using LD48Project.Utils.Editor.Build;
using UnityEngine;

namespace LD48Project.Utils.GameVersioning {
	[CreateAssetMenu(fileName = "GameVersion", menuName = "Diving Festival/Create GameVersionAsset")]
	public class GameVersionInternalScriptableObject : ScriptableObject {
		const string AssetPathInResourceFolder = "GameVersion";
		
		public int MajorVersion;
		public int MinorVersion;
		public int InternalVersion;

		public BuildType CurrentBuildType;
		public static GameVersionInternalScriptableObject GetInstance() {
			return Resources.Load<GameVersionInternalScriptableObject>(AssetPathInResourceFolder);
		}
	}
}