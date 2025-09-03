namespace ApiTestingFramework.Interfaces
{
    public interface IJsonExtractor
    {
        T ExtractValue<T>(string json, string jsonPath);
        string ExtractString(string json, string jsonPath);
        int ExtractInt(string json, string jsonPath);
        bool ExtractBool(string json, string jsonPath);
    }
}