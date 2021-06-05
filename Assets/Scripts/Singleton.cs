public class Singleton<T> where T : class, new() {
    static T _instance;
    public static T Instance => _instance ??= new T();
}
