using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StudyConAPIDotnet4_7_2.Source.Enumeration;

namespace StudyConAPIDotnet4_7_2.Source
{
    /// <summary>
    /// The <c>StudyConNode</c> class.
    /// Contains all methods to adapt tree nodes.
    /// </summary>
    public class StudyConNode
    {
        [JsonIgnore]
        public StudyConNode Parent { get; set; }
        
        [JsonProperty("item")]
        public StudyConItem Item { get; set; }
        
        [JsonProperty("children")]
        public List<StudyConNode> Children { get; set; }
        
        [JsonIgnore]
        public List<StudyConNode> Siblings { get; set; }
        
        
        /*
         * Constructors
         */
        public StudyConNode()
        {
            Parent = null;
            Item = null;
            Siblings = new List<StudyConNode>();
            Children = new List<StudyConNode>();
        }

        public StudyConNode(StudyConItem item)
        {
            Parent = null;
            Item = item;
            Siblings = new List<StudyConNode>();
            Children = new List<StudyConNode>();
        }

        public StudyConNode(StudyConNode parent, StudyConItem item, List<StudyConNode> children)
        {
            Parent = parent;
            Item = item;
            Siblings = new List<StudyConNode>();
            Children = children;
        }

        public StudyConNode(StudyConNode parent,
            StudyConItem item, List<StudyConNode> siblings, List<StudyConNode> children)
        {
            Parent = parent;
            Item = item;
            Siblings = siblings;
            Children = children;
        }

        [JsonConstructor]
        public StudyConNode(StudyConItem item, List<StudyConNode> children)
        {
            Parent = null;
            Item = item;
            Siblings = null;
            Children = children;
        }

        /*
         * Helpers
         */
        /// <summary>
        /// Checks if a <c>StudyConNode</c> has a parent <c>StudyConNode</c>.
        /// </summary>
        /// <returns>True, if the <c>StudyConNode</c> has a parent.</returns>
        public bool HasParent()
        {
            return Parent != null;
        }
        
        /// <summary>
        /// Checks if a <c>StudyConNode</c> has children.
        /// </summary>
        /// <returns>True, if the <c>StudyConNode</c> has children.</returns>
        public bool HasChildren()
        {
            return Children != null && Children.Any();
        }

        /// <summary>
        /// Checks if the <c>StudyConNode</c> is the root node.
        /// </summary>
        /// <returns></returns>
        public bool IsRoot()
        {
            return Item.Name == "root";
        }
        
        /// <summary>
        /// Adds a single child to <c>StudyConNode</c>s children.
        /// </summary>
        /// <param name="child">The child which should be added.</param>
        public void AddChild(StudyConNode child)
        {
            if (Children != null)
            {
                Children.Add(child);
            }
            else
            {
                Children = new List<StudyConNode>{child};
            }
        }

        /// <summary>
        /// Adds a single sibling to a <c>StudyConNode</c>.
        /// </summary>
        /// <param name="sibling">The sibling which should be added.</param>
        public void AddSibling(StudyConNode sibling)
        {
            Siblings.Add(sibling);
        }
        
        /// <summary>
        /// Adds a list of siblings to the <c>StudyConNode</c>.
        /// </summary>
        /// <param name="siblings">The list of siblings which should be added.</param>
        public void AddSiblings(List<StudyConNode> siblings)
        {
            if (Siblings != null)
            {
                foreach (var sib in siblings)
                {
                    Siblings.Add(sib);
                }
            }
            else
            {
                Siblings = siblings;
            }
        }
        
        /// <summary>
        /// Adds a parent <c>StudyConNode</c> to the <c>StudyConNode</c>.
        /// </summary>
        /// <param name="parent">The parent which should be added.</param>
        public void AddParent(StudyConNode parent)
        {
            Parent = parent;
        }
        
        /// <summary>
        /// Deletes a given child <c>StudyConNode</c>.
        /// </summary>
        /// <param name="child">The child node which should be deleted.</param>
        public void DeleteChild(StudyConNode child)
        {
            Children.Remove(child);
        }

        /// <summary>
        /// ToString method.
        /// </summary>
        /// <returns>A string representing the node.</returns>
        public override string ToString()
        {
            return Item.ToString();
        }

        public string ToTreeString()
        {
            var retStr = "[Branch ";
            if (Item.Level.Equals(EnumLevel.L1))
            {
                retStr += Item.Name;
                retStr += "]\n";
                if (!Children.Any())
                {
                    return retStr;
                }
                foreach (var child in Children)
                {
                    retStr += "  └── ";
                    retStr += child.ToString();
                    retStr += "\n";
                    if (!child.HasChildren())
                    {
                        continue;
                    }
                    foreach (var grandchild in child.Children)
                    {
                        retStr += "        └── ";
                        retStr += grandchild.ToString();
                        retStr += "\n";
                    }
                }
            }

            return retStr;
        }
        
        /// <summary>
        /// ToJson method
        /// </summary>
        /// <returns>A json string representing the node</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(new StudyConNode(Parent, Item, Children));
        }
    }
}