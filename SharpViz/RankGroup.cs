using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpViz
{
    public sealed class RankGroup
    {
        private readonly string _rank;
        private readonly IEnumerable<Node> _nodes;

        public static RankGroup Same(params Node[] nodes)
        {
            return new RankGroup("same", nodes);
        }

        private RankGroup(string rank, IEnumerable<Node> nodes)
        {
            _rank = rank;
            _nodes = nodes;
        }

        public string Render()
        {
            var nodes = string.Join("; ", _nodes.Select(x => $"\"{x.Key}\""));

            return $"{{ rank={_rank}; {nodes} }}";
        }
    }
}
