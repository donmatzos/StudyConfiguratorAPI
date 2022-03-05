using Newtonsoft.Json;
using StudyConAPIDotnet4_7_2.Source.Enumeration;

namespace StudyConAPIDotnet4_7_2.Source.SimpleObjects
{
    /// <summary>
    /// <c>EstimationContainer</c> used to store a degree course and corresponding values.
    /// </summary>
    public class EstimationContainer
    {
        [JsonProperty("degreeCourse")]
        public EnumDegreeCourse DegreeCourse { get; set; }
        
        [JsonProperty("value")]
        public double Value { get; set; }
        
        [JsonConstructor]
        public EstimationContainer(EnumDegreeCourse degreeCourse, double value)
        {
            DegreeCourse = degreeCourse;
            Value = value;
        }

        public EstimationContainer(EnumDegreeCourse degreeCourse)
        {
            DegreeCourse = degreeCourse;
            Value = 0;
        }
    }
}