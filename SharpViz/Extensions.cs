using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SharpViz
{
    public static class Extensions
    {
        public static string ToRgbHex(this Color color)
        {
            var rawHex = color.ToArgb().ToString("x8").Substring(2);

            return "#" + rawHex;
        }

        public static IEnumerable<DirectedEdge> UniquePaths(this IEnumerable<DirectedEdge> edges)
        {
            return edges
                .GroupBy(x => new { x.From, x.To })
                .Select(x => x.First());
        }

        public static IEnumerable<DirectedEdge> UniquePathsLast(this IEnumerable<DirectedEdge> edges)
        {
            return edges
                .GroupBy(x => new { x.From, x.To })
                .Select(x => x.Last());
        }

        public static string Render(this IEnumerable<DirectedEdge> edges)
        {
            return string.Join(Environment.NewLine, edges.Select(x => x.Render()));
        }

        public static string Render(this IEnumerable<Node> nodes)
        {
            return string.Join(Environment.NewLine, nodes.Select(x => x.Render()));
        }

        public static string Render(this IEnumerable<RankGroup> groups)
        {
            return string.Join(Environment.NewLine, groups.Select(x => x.Render()));
        }
    }
}
