using System;
using System.Collections.Generic;
using System.Linq;
using StudyCon.Source;
using StudyCon.Source.SimpleObjects;
using StudyConAPIDotnet4_7_2.Source.SimpleObjects;

namespace StudyConAPIDotnet4_7_2.Source.Utils
{
    /// <summary>
    /// Contains static methods to evaluate and compare a tree.
    /// </summary>
    public static class EvaluationUtils
    {
        private static Random _random = new Random();
        
        public static string GenerateId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
        
        /// <summary>
        /// Evaluates a tree and returns a result.
        /// </summary>
        /// <param name="tree">The <c>StudyConTree</c> to evaluate</param>
        /// <returns>Result containing degree courses and corresponding final values</returns>
        public static Result EvaluateTree(StudyConTree tree)
        {
            var result = new Result();
            CalculateResult(result, tree.LoadLevelOneNodes());
            CalculateResult(result, tree.LoadLevelTwoNodes());
            CalculateResult(result, tree.LoadLevelThreeNodes());
            return result;
        }

        /// <summary>
        /// Calculates a <c>Result</c> based on a list of nodes.
        /// </summary>
        /// <param name="result">The <c>Result</c> to update</param>
        /// <param name="nodes">A list of <c>StudyConNode</c>s</param>
        private static void CalculateResult(Result result, List<StudyConNode> nodes)
        {
            foreach (var container in nodes.SelectMany(node => node.Item.DegreeCourses))
            {
                result.AddPointsToCourse(container);
            }
        }
        
        public static void CompareTreesAndUpdateValues(StudyConTree persTree, StudyConTree cmpTree)
        {
            foreach (var cmpChild in cmpTree.Root.Children
                .Where(cmpChild => !persTree.Root.Children.Contains(cmpChild)))
            {
                persTree.Root.Children.Add(cmpChild);
            }
            foreach (var persBranch in persTree.Root.Children)
            {
                foreach (var cmpBranch in cmpTree.Root.Children
                    .Where(cmpBranch => persBranch.Item.Name.Equals(cmpBranch.Item.Name)))
                {
                    CompareBranchesAndUpdateValues(persBranch, cmpBranch);
                }
            }
        }
            
        /// <summary>
        /// Compares two existing branches to each other.
        /// The branches names have to be equal.
        /// The full branch, from L1 over L2 to L3 is compared.
        /// </summary>
        /// <param name="persisting">The L1 <c>StudyConNode</c> containing the branch that should be kept</param>
        /// <param name="cmp">The L1 <c>StudyConNode</c> containing the branch that should be compared</param>
        public static void CompareBranchesAndUpdateValues(StudyConNode persisting, StudyConNode cmp)
        {
            if (!persisting.Item.Name.Equals(cmp.Item.Name)) return;
            foreach (var cmpChild in cmp.Children
                .Where(cmpChild => !persisting.Children.Contains(cmpChild)))
            {
                persisting.Children.Add(cmpChild);
            }
            UpdateDegreeCourseValues(persisting, cmp);
            foreach (var persChild in persisting.Children)
            {
                foreach (var cmpChild in cmp.Children)
                {
                    CompareL2NodeAndChildren(persChild, cmpChild);
                }
            }
        }

        /// <summary>
        /// Compares two L2 <c>StudyConNode</c> and their children.
        /// The branches <c>StudyConNode</c> have to be equal.
        /// </summary>
        /// <param name="persisting">The persisting L2 node</param>
        /// <param name="cmp">The node to compare with</param>
        private static void CompareL2NodeAndChildren(StudyConNode persisting, StudyConNode cmp)
        {
            if (!persisting.Item.Name.Equals(cmp.Item.Name)) return;
            foreach (var cmpChild in cmp.Children
                .Where(cmpChild => !persisting.Children.Contains(cmpChild)))
            {
                persisting.Children.Add(cmpChild);
            }
            UpdateDegreeCourseValues(persisting, cmp);
            foreach (var persChild in persisting.Children)
            {
                foreach (var cmpChild in cmp.Children)
                {
                    UpdateDegreeCourseValues(persChild, cmpChild);
                }
            }
        }
        
        /// <summary>
        /// Iterates over the degree courses and updates to the higher value.
        /// </summary>
        /// <param name="persisting">The persisting <c>StudyConNode</c></param>
        /// <param name="cmp">The <c>StudyConNode</c> to compare values with</param>
        private static void UpdateDegreeCourseValues(StudyConNode persisting, StudyConNode cmp)
        {
            if (!persisting.Item.Name.Equals(cmp.Item.Name)) return;
            foreach (var cmpContainer in cmp.Item.DegreeCourses)
            {
                if (persisting.Item.ContainsDegreeCourse(cmpContainer.DegreeCourse))
                {
                    persisting.Item.GetContainerByDegreeCourse(cmpContainer.DegreeCourse).Value = 
                        Math.Max(persisting.Item.GetContainerByDegreeCourse(cmpContainer.DegreeCourse).Value, cmpContainer.Value);;
                }
                else
                {
                    persisting.Item.DegreeCourses.Add(cmpContainer);
                }
            }
        }
        
         /// <summary>
        /// Adds a list of child nodes to a persisting list of child nodes.
        /// If a duplicate node is encountered, the degree course values are updated.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public static List<StudyConNode> AddChildrenWithoutDuplicate(List<StudyConNode> first, List<StudyConNode> second)
        {
            foreach (var nodeFirst in first)
            {
                foreach (var nodeSecond in second.Where(nodeSecond => nodeFirst.Item.Name == nodeSecond.Item.Name))
                {
                    UpdateDegreeCourseValues(nodeFirst, nodeSecond);
                    UpdateDegreeCourseValues(nodeSecond, nodeFirst);
                }
            }
            first.AddRange(second);
            return first.GroupBy(node => node.Item.Name)
                .Select(x => x.First())
                .ToList();
        }
    }
}