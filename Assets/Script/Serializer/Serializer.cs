namespace NgocDev.Serializer
{
    public interface ISerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string data);
    }

    public class JsonSerializer : ISerializer
    {
 
        public string Serialize<T>(T obj)
        {
            return UnityEngine.JsonUtility.ToJson(obj);
        }
        public T Deserialize<T>(string data)
        {
            return UnityEngine.JsonUtility.FromJson<T>(data);
        }
    }
}