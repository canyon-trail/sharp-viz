using System.Drawing;

namespace SharpViz
{
    public class Flowchart : Digraph
    {
        public Flowchart()
        {
            DefaultNode.FillColor = Color.FromArgb(0xee, 0xee, 0xf3);
            DefaultNode.Style = NodeStyle.Filled;
            DefaultNode.Shape = NodeShape.Box;
            DefaultNode.FontName = "Open Sans";

            this.AddDefaultEdgeAttribute("fontname=\"Open Sans\"");
        }

        public Node AddStartEndNode(string key)
        {
            return AddStartEndNode(key, null);
        }

        public virtual Node AddStartEndNode(string key, string label)
        {
            var node = AddNode(key, label);

            node.Style = NodeStyle.Rounded;

            return node;
        }

        public virtual Node AddDecisionNode(string key, string label)
        {
            var node = AddNode(key, label);

            node.Shape = NodeShape.Diamond;

            return node;
        }
    }
}
