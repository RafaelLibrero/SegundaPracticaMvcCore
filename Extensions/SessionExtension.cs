using Newtonsoft.Json;

namespace SegundaPracticaMvcCore.Extensions
{
    public static class SessionExtension
    {
        public static void SetObject
            (this ISession session,string key, object obj)
        {
            string data = JsonConvert.SerializeObject (obj);
            session.SetString(key, data);
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            string data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
        }

    }
}
