using Newtonsoft.Json.Linq;
using ApiTestingFramework.Interfaces;

namespace ApiTestingFramework.Implementations
{
    public class JsonExtractor : IJsonExtractor
    {
        public T ExtractValue<T>(string json, string jsonPath)
        {
            try
            {
                // Handle array responses
                if (json.Trim().StartsWith('['))
                {
                    var jArray = JArray.Parse(json);
                    var token = jArray.SelectToken(jsonPath);
                    return token != null ? token.Value<T>() : default;
                }
                else
                {
                    var jObject = JObject.Parse(json);
                    var token = jObject.SelectToken(jsonPath);
                    return token != null ? token.Value<T>() : default;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to extract value from JSON using path '{jsonPath}'. JSON: {json}", ex);
            }
        }

        public string ExtractString(string json, string jsonPath)
        {
            return ExtractValue<string>(json, jsonPath);
        }

        public int ExtractInt(string json, string jsonPath)
        {
            return ExtractValue<int>(json, jsonPath);
        }

        public bool ExtractBool(string json, string jsonPath)
        {
            return ExtractValue<bool>(json, jsonPath);
        }
    }
}