using Newtonsoft.Json;
using System.IO;

namespace Common.Json
{
    public class JsonManager
    {
        public JsonSerializer Serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings { Formatting = Formatting.Indented });

        public T Deserialize<T>(string path)
        {
            T result;

            using (StreamReader streamReader = new StreamReader(path))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                {
                    result = this.Serializer.Deserialize<T>(jsonReader);

                    jsonReader.Close();
                }
                streamReader.Close();
            }

            return result;
        }

        public void Serialize(object value, string path)
        {
            using (StreamWriter streamReader = new StreamWriter(path))
            {
                using (JsonTextWriter jsonReader = new JsonTextWriter(streamReader))
                {
                    this.Serializer.Serialize(jsonReader, value);

                    jsonReader.Close();
                }
                streamReader.Close();
            }
        }
    }
}
