using Newtonsoft.Json;

namespace StudyConAPIDotnet4_7_2.Source.Enumeration
{
    public enum EnumLevel
    {
        [JsonProperty("Root")]
        Root,
        [JsonProperty("L1")]
        L1,
        [JsonProperty("L2")]
        L2,
        [JsonProperty("L3")]
        L3,
        [JsonProperty("Undefined")]
        Undefined
    }
}