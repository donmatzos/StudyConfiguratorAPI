using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudyConAPIDotnet4_7_2.Source.Enumeration;
using StudyConAPIDotnet4_7_2.Source.Utils;

namespace StudyConAPIDotnet4_7_2.Source
{
    /// <summary>
    /// The main <c>StudyConTree</c> class.
    /// Contains all methods to navigate and adapt the tree.
    /// </summary>
    public class StudyConTree
    {
        [JsonProperty("root")]
        public StudyConNode Root { get; set; }

        [JsonIgnore]
        public HashSet<string> IdSet { get; }

        /*
         * Constructors
         */
        public StudyConTree()
        {
            Root = new StudyConNode(new StudyConItem("000000", 
                "root", 
                EnumLevel.Root,
                null, 
                "The root node"));
            Root.AddChild(new StudyConNode(new StudyConItem("000001",
                "Programmieren",
                EnumLevel.L1)));
            Root.AddChild(new StudyConNode(new StudyConItem("000002",
                "Design",
                EnumLevel.L1)));
            Root.AddChild(new StudyConNode(new StudyConItem("000003",
                "Technische Grundlagen",
                EnumLevel.L1)));
            Root.AddChild(new StudyConNode(new StudyConItem("000004",
                "Sicherheit",
                EnumLevel.L1)));
            Root.AddChild(new StudyConNode(new StudyConItem("000005",
                "Kommunikation und Medien",
                EnumLevel.L1)));
            Root.AddChild(new StudyConNode(new StudyConItem("000006",
                "Hardware",
                EnumLevel.L1)));
            Root.AddChild(new StudyConNode(new StudyConItem("000007",
                "Naturwissenschaften",
                EnumLevel.L1)));
            Root.AddChild(new StudyConNode(new StudyConItem("000008",
                "Social Skills",
                EnumLevel.L1)));
        }

        [JsonConstructor]
        public StudyConTree(StudyConNode root)
        {
            Root = root;
            IdSet = new HashSet<string>();
            CreateIdSet();
        }

        /*
         * Getters
         */
        /// <summary>
        /// Gets a <c>StudyConNode</c> by its ID.
        /// </summary>
        /// <param name="id">The <c>StudyConNode</c>s ID</param>
        /// <returns>The requested <c>StudyConNode</c></returns>
        public StudyConNode GetById(string id)
        {
            if (!Root.HasChildren()) return null;
            var childrenL1 = Root.Children;
            foreach (var childL1 in childrenL1)
            {
                if (childL1.Item.Id == id)
                {
                    return childL1;
                }
                if (!childL1.HasChildren()) continue;
                var childrenL2 = childL1.Children;
                foreach (var childL2 in childrenL2)
                {
                    if (childL2.Item.Id == id)
                    {
                        return childL2;
                    }

                    if (!childL2.HasChildren()) continue;
                    var childrenL3 = childL2.Children;
                    foreach (var childL3 in childrenL3.Where(childL3 => childL3.Item.Id == id))
                    {
                        return childL3;
                    }
                }
            }
            return null;
        }
        
        /// <summary>
        /// Gets a <c>StudyConNode</c> by its name.
        /// </summary>
        /// <param name="name">The <c>StudyConNode</c>s name</param>
        /// <returns>The requested <c>StudyConNode</c></returns>
        public StudyConNode GetByName(string name)
        {
            if (!Root.HasChildren()) return null;
            var childrenL1 = Root.Children;
            foreach (var childL1 in childrenL1)
            {
                if (childL1.Item.Name == name)
                {
                    return childL1;
                }

                if (!childL1.HasChildren()) continue;
                var childrenL2 = childL1.Children;
                foreach (var childL2 in childrenL2)
                {
                    if (childL2.Item.Name == name)
                    {
                        return childL2;
                    }

                    if (!childL2.HasChildren()) continue;
                    var childrenL3 = childL2.Children;
                    foreach (var childL3 in childrenL3.Where(childL3 => childL3.Item.Name == name))
                    {
                        return childL3;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all the level three items of a level one category by its name.
        /// </summary>
        /// <param name="name">The name of the L1 <c>StudyConNode</c>.</param>
        /// <returns>A list that contains all the requested L3 <c>StudyConNode</c>s.</returns>
        public List<StudyConNode> GetLevelThreeByLevelOneName(string name)
        {
            if (!Root.HasChildren()) return new List<StudyConNode>();
            var retList = new List<StudyConNode>();
            foreach (var childL3 in from childL1 in Root.Children 
                where childL1.Item.Name == name && childL1.HasChildren() from childL2 in childL1.Children 
                where childL2.HasChildren() from childL3 in childL2.Children 
                where !retList.Contains(childL3) select childL3)
            {
                retList.Add(childL3);
            }


            return retList;
        }
        
        /*
         * Helpers
         */
        /// <summary>
        /// Deletes a <c>StudyConNode</c> by its ID.
        /// </summary>
        /// <param name="id">The <c>StudyConNode</c>s ID.</param>
        /// <returns>True if the <c>StudyConNode</c> was deleted.</returns>
        public bool DeleteById(string id)
        {
            if (!Root.HasChildren()) return false;
            foreach (var childL1 in Root.Children)
            {
                if (childL1.Item.Id == id)
                {
                    Root.DeleteChild(childL1);
                    return true;
                }
                if (!childL1.HasChildren()) continue;
                foreach (var childL2 in childL1.Children)
                {
                    if (childL2.Item.Id == id)
                    {
                        childL1.DeleteChild(childL2);
                        return true;
                    }
                    if (!childL2.HasChildren()) continue;
                    var childrenL3 = childL2.Children;
                    foreach (var childL3 in childrenL3.Where(childL3 => childL3.Item.Id == id))
                    {
                        childL2.DeleteChild(childL3);
                        return true;
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// Deletes a <c>StudyConNode</c> by its name.
        /// </summary>
        /// <param name="name">The <c>StudyConNode</c>s name.</param>
        /// <returns>True, if the <c>StudyConNode</c> was deleted.</returns>
        public bool DeleteByName(string name)
        {
            if (!Root.HasChildren()) return false;
            foreach (var childL1 in Root.Children)
            {
                if (childL1.Item.Name == name)
                {
                    Root.DeleteChild(childL1);
                    return true;
                }
                if (!childL1.HasChildren()) continue;
                foreach (var childL2 in childL1.Children)
                {
                    if (childL2.Item.Name == name)
                    {
                        childL1.DeleteChild(childL2);
                        return true;
                    }
                    if (!childL2.HasChildren()) continue;
                    var childrenL3 = childL2.Children;
                    foreach (var childL3 in childrenL3.Where(childL3 => childL3.Item.Name == name))
                    {
                        childL2.DeleteChild(childL3);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the trees root has children.
        /// </summary>
        /// <returns>True if the root hast children.</returns>
        public bool RootHasChildren()
        {
            return Root.HasChildren();
        }
        
        /// <summary>
        /// Adds level one <c>StudyConNode</c>s.
        /// </summary>
        /// <param name="node">The <c>StudyConNode</c> which should be added.</param>
        public void AddLevelOne(StudyConNode node)
        {
            node.AddParent(Root);
            if (Root.HasChildren())
            {
                node.AddSiblings(Root.Children);
            }
            Root.AddChild(node);
        }
        
        /// <summary>
        /// Adds a child to a parent <c>StudyConNode</c> by the parents ID.
        /// </summary>
        /// <param name="id">The parents ID.</param>
        /// <param name="node">The child <c>StudyConNode</c> which should be added.</param>
        /// <returns>True, if the <c>StudyConNode</c> has been added.</returns>
        public bool AddToParentById(string id, StudyConNode node)
        {
            if (!Root.HasChildren()) return false;
            foreach (var childL1 in Root.Children)
            {
                if (childL1.Item.Id == id)
                {
                    childL1.AddChild(node);
                    UpdateSiblings();
                    return true;
                }
                if (!childL1.HasChildren()) continue;
                foreach (var childL2 in childL1.Children.Where(childL2 => childL2.Item.Id == id))
                {
                    childL2.AddChild(node);
                    UpdateSiblings();
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Adds a child to a parent <c>StudyConNode</c> by the parents name.
        /// </summary>
        /// <param name="parentName">The parents name.</param>
        /// <param name="node">The child <c>StudyConNode</c> which should be added.</param>
        /// <returns>True, if the <c>StudyConNode</c> has been added.</returns>
        public bool AddToParentByName(string parentName, StudyConNode node)
        {
            if (!Root.HasChildren()) return false;
            foreach (var childL1 in Root.Children)
            {
                if (childL1.Item.Name == parentName)
                {
                    childL1.AddChild(node);
                    UpdateSiblings();
                    return true;
                }
                if (!childL1.HasChildren()) continue;
                foreach (var childL2 in childL1.Children.Where(childL2 => childL2.Item.Name == parentName))
                {
                    childL2.AddChild(node);
                    UpdateSiblings();
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Loads all level one <c>StudyConNode</c>s.
        /// </summary>
        /// <returns>A list filled with all level one <c>StudyConNode</c>s</returns>
        public List<StudyConNode> LoadLevelOneNodes()
        {
            if (!Root.HasChildren()) return null;
            return Root.Children;
        }
    
        /// <summary>
        /// Loads all level two <c>StudyConNode</c>s.
        /// </summary>
        /// <returns>A list filled with all level two <c>StudyConNode</c>s</returns>
        public List<StudyConNode> LoadLevelTwoNodes()
        {
            var retList = new List<StudyConNode>();
            if (!Root.HasChildren()) return null;
            foreach (var child in Root.Children
                .Where(child => child.HasChildren()))
            {
                retList.AddRange(child.Children);
            }
            return retList;
        }
    
        /// <summary>
        /// Loads all level three <c>StudyConNode</c>s.
        /// </summary>
        /// <returns>A list filled with all level three <c>StudyConNode</c></returns>
        public List<StudyConNode> LoadLevelThreeNodes()
        {
            var retList = new List<StudyConNode>();
            if (!Root.HasChildren()) return null;
            foreach (var child in Root.Children)
            {
                if (!child.HasChildren()) continue;
                foreach (var childL2 in child.Children)
                {
                    if (!childL2.HasChildren()) continue;
                    retList.AddRange(childL2.Children);
                }
            }
            return retList;
        }
        
        /// <summary>
        /// Updates all <c>StudyConNode</c>s siblings.
        /// </summary>
        private void UpdateSiblings()
        {
            if (!Root.HasChildren()) return;
            foreach (var childL1 in Root.Children)
            {
                childL1.Siblings = Root.Children;
                if (!childL1.HasChildren()) continue;
                foreach (var childL2 in childL1.Children)
                {
                    childL2.Siblings = childL1.Children;
                    if (!childL2.HasChildren()) continue;
                    foreach (var childL3 in childL2.Children)
                    {
                        childL3.Siblings = childL2.Children;
                    }
                }
            }

        }

        /// <summary>
        /// Updates all <c>StudyConNode</c>s parents.
        /// </summary>
        private void UpdateParent()
        {
            if (!Root.HasChildren()) return;
            foreach (var childL1 in Root.Children)
            {
                childL1.Parent = Root;
                if (!childL1.HasChildren()) continue;
                foreach (var childL2 in childL1.Children)
                {
                    childL2.Parent = childL1;
                    if (!childL2.HasChildren()) continue;
                    foreach (var childL3 in childL2.Children)
                    {
                        childL3.Parent = childL2;
                    }
                }
            }
        }
        
        private void CreateIdSet()
        {
            if (!Root.HasChildren()) return;
            foreach (var childL1 in Root.Children)
            {
                HandleChildrenIdCreation(childL1);
                if (!childL1.HasChildren()) continue;
                foreach (var childL2 in childL1.Children)
                {
                    HandleChildrenIdCreation(childL2);
                    if (!childL2.HasChildren()) continue;
                    foreach (var childL3 in childL2.Children)
                    {
                        HandleChildrenIdCreation(childL3);
                    }
                }
            }
        }

        private void HandleChildrenIdCreation(StudyConNode child)
        {
            if (child.Item.Id == null)
            {
                child.Item.Id = EvaluationUtils.GenerateId();
            }
            if (!IdSet.Contains(child.Item.Id))
            {
                IdSet.Add(child.Item.Id);
            }
            else
            {
                while (IdSet.Contains(child.Item.Id))
                {
                    child.Item.Id = EvaluationUtils.GenerateId();
                }
            }
        }
        
        /// <summary>
        /// ToString method.
        /// </summary>
        /// <returns>A string representing the tree.</returns>
        public override string ToString()
        {
            if (!Root.HasChildren()) return "";
            var retStr = "[Root: " + Root.Item.Name + "]\n";
            foreach (var childL1 in Root.Children)
            {
                retStr += "[Branch]\n";
                retStr += childL1.ToString();
                retStr += "\n";
                if (!childL1.HasChildren())
                {
                    continue;
                }
                foreach (var childL2 in childL1.Children)
                {
                    retStr += "  └── ";
                    retStr += childL2.ToString();
                    retStr += "\n";
                    if (!childL2.HasChildren())
                    {
                        continue;
                    }
                    foreach (var childL3 in childL2.Children)
                    {
                        retStr += "        └── ";
                        retStr += childL3.ToString();
                        retStr += "\n";
                    }
                }
            }
            return retStr;
        }

        /// <summary>
        /// ToJson method
        /// </summary>
        /// <returns>A json string representing the <c>StudyConTree</c></returns>
        public string ToJson()
        {
            return JToken.Parse(JsonConvert.SerializeObject(new StudyConTree(Root))).ToString(Formatting.Indented);
        }

        /*
         * Static methods
         */
        /// <summary>
        /// Parses a json string to create the represented <c>StudyConTree</c>.
        /// </summary>
        /// <param name="jsonString">Json string</param>
        /// <returns>A tree with the information provided in the json string</returns>
        public static StudyConTree ParseFromJsonString(string jsonString)
        {
            var retTree = JsonConvert.DeserializeObject<StudyConTree>(jsonString);
            retTree?.UpdateParent();
            retTree?.UpdateSiblings();
            return retTree;
        }

        /// <summary>
        /// Parses a json file to create the represented <c>StudyConTree</c>.
        /// </summary>
        /// <param name="filePath">Path to the json file</param>
        /// <returns>A tree with the information provided in the json file</returns>
        public static StudyConTree ParseFromJsonFilePath(string filePath)
        {
            var jsonTreeString = File.ReadAllText(filePath);
            var retTree = JsonConvert.DeserializeObject<StudyConTree>(jsonTreeString);
            retTree?.UpdateParent();
            retTree?.UpdateSiblings();
            return retTree;
        }
    }
}