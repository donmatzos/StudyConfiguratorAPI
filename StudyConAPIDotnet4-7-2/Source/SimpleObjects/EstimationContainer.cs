using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using StudyConAPIDotnet4_7_2.Source.Enumeration;

namespace StudyCon.Source.SimpleObjects
{
    /// <summary>
    /// <c>EstimationContainer</c> used to store a degree course and corresponding values.
    /// </summary>
    public class EstimationContainer// : ICollection
    {
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
        
        [JsonProperty("degreeCourse")]
        public EnumDegreeCourse DegreeCourse { get; set; }
        
        [JsonProperty("value")]
        public double Value { get; set; }

        //TODO - Implement ICollection like https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1?view=net-5.0
        // public IEnumerator GetEnumerator()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public void CopyTo(Array array, int index)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public int Count { get; }
        // public bool IsSynchronized { get; }
        // public object SyncRoot { get; }
    }
}