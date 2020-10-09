using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SharpViz
{
    public class Digraph
    {
        private readonly List<Node> _nodes = new List<Node>();

        private readonly List<DirectedEdge> _edges = new List<DirectedEdge>();

        private readonly List<RankGroup> _rankGroups = new List<RankGroup>();

        private readonly List<string> _defaultEdgeAttributes = new List<string> { "fontname=\"Open Sans\"" };

        private readonly List<string> _attributes = new List<string>();

        private readonly Dictionary<string, Digraph> _clusters = new Dictionary<string, Digraph>();

        public Digraph()
        {
            DefaultNode = new Node("node") { FontName = "Open Sans" };
        }

        public Node DefaultNode { get; }

        public Digraph AddDefaultEdgeAttribute(string attr)
        {
            _defaultEdgeAttributes.Add(attr);

            return this;
        }

        public Digraph AddAttribute(string attr)
        {
            _attributes.Add(attr);

            return this;
        }

        public Node AddNode(string key)
        {
            return AddNode(key, null);
        }

        public Node AddNode(string key, string label)
        {
            return AddNode(new Node(key) { Label = label });
        }

        public Node AddNode(Node node)
        {
            _nodes.Add(node);

            return node;
        }

        public Digraph AddEdge(Node start, Node end)
        {
            return AddEdge(start, end, label: null);
        }

        public Digraph AddEdgeChain(Node start, Node next, params Node[] others)
        {
            var edges = DirectedEdge.Chain(start, next, others);

            foreach(var e in edges)
            {
                AddEdge(e);
            }

            return this;
        }

        public Digraph AddEdge(
            Node start,
            Node end,
            string label = null,
            bool? constraint = null,
            EdgeDirection? direction = null,
            PortLocation? headport = null,
            PortLocation? tailport = null,
            Color? color = null,
            EdgeStyle? style = null,
            string attributes = null
            )
        {
            var edge = new DirectedEdge(start, end) {
                Label = label,
                Constraint = constraint,
                Direction = direction,
                HeadPort = headport,
                TailPort = tailport,
                Color = color,
                Style = style,
                Attrs = attributes
            };

            return AddEdge(edge);
        }

        public Digraph AddEdge(DirectedEdge edge)
        {
            _edges.Add(edge);

            return this;
        }

        public Digraph AddInvisibleEdge(Node start, Node end)
        {
            return AddEdge(new DirectedEdge(start, end) { Style = EdgeStyle.Invis });
        }

        public Digraph WithRankSame(params Node[] nodes)
        {
            _rankGroups.Add(RankGroup.Same(nodes));

            return this;
        }

        public Digraph AddCluster(string key, Digraph cluster)
        {
            if(!key.StartsWith("cluster_"))
            {
                throw new ArgumentException("Cluster names must start with 'cluster_'");
            }
            _clusters[key] = cluster;

            return this;
        }

        public string Render()
        {
            return $@"digraph G {RenderBody()}";
        }

        protected string RenderBody()
        {
            var clusterContent = _clusters
                .Select(x => RenderCluster(x.Key, x.Value));

            return $@"{{
    {string.Join(";\n    ", _attributes)}
	{ DefaultNode.Render() }
    edge [{string.Join(";", _defaultEdgeAttributes)}]

    { _nodes.Render() }

    { string.Join(Environment.NewLine, clusterContent) }

    { _rankGroups.Render() }

    { _edges.Render() }

	}}";
        }

        private string RenderCluster(string key, Digraph cluster)
        {
            var contents = $"subgraph {key} {cluster.RenderBody()}";

            contents = contents.Replace(Environment.NewLine, Environment.NewLine + "    ");

            return contents;
        }
    }
}
