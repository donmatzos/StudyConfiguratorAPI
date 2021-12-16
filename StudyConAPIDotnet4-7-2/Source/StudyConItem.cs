using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StudyCon.Source.SimpleObjects;
using StudyConAPIDotnet4_7_2.Source.Enumeration;
using StudyConAPIDotnet4_7_2.Source.Utils;

namespace StudyConAPIDotnet4_7_2.Source
{
    /// <summary>
    /// The <c>StudyConItem</c> class.
    /// Contains methods to adapt items.
    /// </summary>
    public class StudyConItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("level")][JsonConverter(typeof(StringEnumConverter))]
        public EnumLevel Level { get; set; }
        
        [JsonProperty("degrees")]
        public HashSet<EstimationContainer> DegreeCourses { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonIgnore] 
        private readonly EstimationContainer[] _defaultContainerSet =
        {
            new(EnumDegreeCourse.AC),
            new(EnumDegreeCourse.DA),
            new(EnumDegreeCourse.MC),
            new(EnumDegreeCourse.SE),
            new(EnumDegreeCourse.SI),
            new(EnumDegreeCourse.HSD),
            new(EnumDegreeCourse.KWM),
            new(EnumDegreeCourse.MBI),
            new(EnumDegreeCourse.MTD)
        };
        
        /*
         * Constructors
         */
        public StudyConItem(string name, EnumLevel level)
        {
            Id = EvaluationUtils.GenerateId();
            Name = name;
            Level = level;
            DegreeCourses = new HashSet<EstimationContainer>(_defaultContainerSet);
            Description = "No Description";
        }

        public StudyConItem(string name, EnumLevel level, EstimationContainer container, string description)
        {
            Id = EvaluationUtils.GenerateId();
            Name = name;
            Level = level;
            var updatedContainerSet = _defaultContainerSet;
            updatedContainerSet[(int) container.DegreeCourse] = container;
            DegreeCourses = new HashSet<EstimationContainer>(updatedContainerSet);
            Description = description;
        }
        
        public StudyConItem(string name, EnumLevel level, HashSet<EstimationContainer> degrees)
        {
            Id = EvaluationUtils.GenerateId();
            Name = name;
            Level = level;
            DegreeCourses = degrees;
            Description = "No Description";
        }
        
        public StudyConItem(string name, EnumLevel level, HashSet<EstimationContainer> degrees, string description)
        {
            Id = EvaluationUtils.GenerateId();
            Name = name;
            Level = level;
            DegreeCourses = degrees;
            Description = description;
        }
        
        public StudyConItem(string id, string name, EnumLevel level)
        {
            Id = id;
            Name = name;
            Level = level;
            DegreeCourses = new HashSet<EstimationContainer>(_defaultContainerSet);
            Description = "No Description";
        }
        
        public StudyConItem(string id, string name, EnumLevel level, HashSet<EstimationContainer> degrees)
        {
            Id = id;
            Name = name;
            Level = level;
            DegreeCourses = degrees;
            Description = "No Description";
        }
        
        [JsonConstructor]
        public StudyConItem(string id, string name, EnumLevel level, HashSet<EstimationContainer> degrees, string description)
        {
            Id = id;
            Name = name;
            Level = level;
            DegreeCourses = degrees;
            Description = description;
        }
        
        /*
         * Helpers
         */
        /// <summary>
        /// Checks if a <c>StudyConItem</c> contains a given degree course.
        /// </summary>
        /// <param name="degreeCourse"></param>
        /// <returns>True, if the <c>StudyConItem</c> contains the <c>EnumDegreeCourse</c></returns>
        public bool ContainsDegreeCourse(EnumDegreeCourse degreeCourse)
        {
            return DegreeCourses.Any(estimationContainer => estimationContainer.DegreeCourse.Equals(degreeCourse));
        }

        /// <summary>
        /// Returns an <c>EstimationContainer</c> by a given <c>EnumDegreeCourse</c>.
        /// </summary>
        /// <param name="degreeCourse"></param>
        /// <returns>The requested <c>EstimationContainer</c></returns>
        public EstimationContainer GetContainerByDegreeCourse(EnumDegreeCourse degreeCourse)
        {
            return DegreeCourses.First(estimationContainer => estimationContainer.DegreeCourse.Equals(degreeCourse));
        }
        
        /// <summary>
        /// ToString method.
        /// </summary>
        /// <returns>A string representing the item.</returns>
        public override string ToString()
        {
            return Name;
        }
        
        /// <summary>
        /// ToJson method
        /// </summary>
        /// <returns>A json string representing the item</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(new StudyConItem(Id, Name, Level, DegreeCourses, Description));
        }
    }
}