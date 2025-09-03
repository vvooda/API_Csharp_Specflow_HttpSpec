using Newtonsoft.Json.Linq;

namespace ApiTestingFramework.Implementations
{
    public static class JsonArrayHelper
    {
        public static string GetFirstArrayElement(string jsonArray)
        {
            if (string.IsNullOrEmpty(jsonArray) || !jsonArray.Trim().StartsWith('['))
                return jsonArray;

            var jArray = JArray.Parse(jsonArray);
            return jArray[0].ToString();
        }

        public static JArray ParseJsonArray(string jsonArray)
        {
            return JArray.Parse(jsonArray);
        }

        public static bool IsJsonArray(string json)
        {
            return !string.IsNullOrEmpty(json) && json.Trim().StartsWith('[');
        }
    }
}
