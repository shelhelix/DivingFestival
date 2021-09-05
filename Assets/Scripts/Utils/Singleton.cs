namespace LD48Project.Utils {
	public class Singleton<T> where T : class, new() {
		static T _instance;
		
		public static T Instance => _instance ?? (_instance = new T());
	}
}