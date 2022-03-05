using System.Collections.Generic;
using System.Linq;
using StudyConAPIDotnet4_7_2.Source.Enumeration;

namespace StudyConAPIDotnet4_7_2.Source.SimpleObjects
{
   /// <summary>
    /// <c>Result</c> class used to store and evaluate results.
    /// </summary>
    public class Result
    {
        public HashSet<EstimationContainer> ResultSet { get; set; }

        public Result()
        {
            ResultSet = new HashSet<EstimationContainer>()
            {
                new(EnumDegreeCourse.AC, 0),
                new(EnumDegreeCourse.MC, 0),
                new(EnumDegreeCourse.HSD, 0),
                new(EnumDegreeCourse.MTD, 0),
                new(EnumDegreeCourse.KWM, 0),
                new(EnumDegreeCourse.DA, 0),
                new(EnumDegreeCourse.SE, 0),
                new(EnumDegreeCourse.SI, 0),
                new(EnumDegreeCourse.MBI, 0)
            };
        }

        public Result(HashSet<EstimationContainer> result)
        {
            ResultSet = result;
        }

        /// <summary>
        /// Returns a ordered <c>Result</c>.
        /// </summary>
        /// <returns>A descending ordered <c>Result</c> with the top 3 <c>EnumDegreeCourse</c>s</returns>
        public HashSet<EstimationContainer> GetOrderedResult()
        {
            var query = ResultSet.OrderByDescending(container => container.Value)
                .Take(3).ToHashSet();
            return query;
        }
        
        /// <summary>
        /// Gets an <c>EstimationContainer</c> by a given <c>EnumDegreeCourse</c>.
        /// </summary>
        /// <param name="degreeCourse"></param>
        /// <returns></returns>
        public EstimationContainer GetEstimationContainerByEnum(EnumDegreeCourse degreeCourse)
        {
            return ResultSet.FirstOrDefault(c => c.DegreeCourse.Equals(degreeCourse));
        }

        /// <summary>
        /// Adds points to a course by utilizing an estimation container.
        /// </summary>
        /// <param name="estimationContainer">.</param>
        public void AddPointsToCourse(EstimationContainer estimationContainer)
        {
            foreach (var c in ResultSet.Where(c => c.DegreeCourse == estimationContainer.DegreeCourse))
            {
                c.Value += estimationContainer.Value;
                break;
            }
        }
        
    }
}