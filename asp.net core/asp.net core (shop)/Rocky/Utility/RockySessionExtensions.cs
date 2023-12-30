using System.Text.Json;

namespace Rocky.Utility
{
    public static class RockySessionExtensions
    {
        public static void Set<T>(ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(ISession session, string key)
        {
            var value = session.Get(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
