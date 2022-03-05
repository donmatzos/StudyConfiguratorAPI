using Newtonsoft.Json;

namespace StudyConAPIDotnet4_7_2.Source.Enumeration
{
    public enum EnumDegreeCourse
    {
        [JsonProperty("MC")]
        MC,
        [JsonProperty("AC")]
        AC,
        [JsonProperty("HSD")]
        HSD,
        [JsonProperty("MTD")]
        MTD,
        [JsonProperty("KWM")]
        KWM,
        [JsonProperty("DA")]
        DA,
        [JsonProperty("SE")]
        SE,
        [JsonProperty("SI")]
        SI,
        [JsonProperty("MBI")]
        MBI,
        [JsonProperty("Undefined")]
        Undefined
    }
}