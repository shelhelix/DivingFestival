using LD48Project.Utils.Editor.Build;

namespace LD48Project.Utils.GameVersioning {
	public class GameVersion : Singleton<GameVersion> {
		readonly GameVersionInternalScriptableObject _gameVersionInternal;

		public BuildType BuildType => _gameVersionInternal.CurrentBuildType;

		public bool IsInternal   => BuildType == BuildType.Internal;
		public bool IsProduction => BuildType == BuildType.Production;
		
		public GameVersion() {
			_gameVersionInternal = GameVersionInternalScriptableObject.GetInstance();
		}
		
		public string ConvertVersionToString() {
			return $"{_gameVersionInternal.MajorVersion}.{_gameVersionInternal.MinorVersion}.{_gameVersionInternal.InternalVersion}";
		}

		public int ConvertVersionToBundleCode() {
			return _gameVersionInternal.MajorVersion * 100 * 1000 + _gameVersionInternal.MinorVersion * 1000 + _gameVersionInternal.InternalVersion;
		}

		public void BumpInternalVersion() {
			_gameVersionInternal.InternalVersion++;
		}

		public void BumpMinorVersion() {
			_gameVersionInternal.MinorVersion++;
		}
	}
}