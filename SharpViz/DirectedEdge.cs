using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SharpViz
{
    [Flags]
    public enum EdgeDirection
    {
        Forward = 1,
        Back = 2,
        Both = 3,
        None = 0
    }

    [Flags]
    public enum EdgeStyle
    {
        Invis = 1,
        Dashed = 2
    }

    public enum PortLocation
    {
        N,
        S,
        E,
        W,
        NE,
        NW,
        SE,
        SW
    }

    public sealed class DirectedEdge
    {
        public DirectedEdge(string from, string to)
        {
            From = from;
            To = to;
        }

        public DirectedEdge(Node from, Node to)
        {
            From = from.Key;
            To = to.Key;
        }

        public static IEnumerable<DirectedEdge> Chain(Node start, Node next, params Node[] others)
        {
            yield return new DirectedEdge(start, next);

            var current = next;
            foreach(var node in others)
            {
                yield return new DirectedEdge(current, node);
                current = node;
            }
        }

        public string From { get; }
        public string To { get; }

        public IEnumerable<string> Ends => new[] { From, To };

        public double? Length { get; set; }

        public EdgeDirection? Direction { get; set; }

        public EdgeStyle? Style { get; set; }

        public PortLocation? HeadPort { get; set; }

        public PortLocation? TailPort { get; set; }

        public int? Weight { get; set; }

        public Color? Color { get; set; }

        public string Label { get; set; }

        public bool? Constraint { get; set; }

        public string Attrs { get; set; }

        public string Render()
        {
            var spec = $"\"{From}\" -> \"{To}\"";

            var attrs = GetAttributes().ToArray();

            if (attrs.Any())
            {
                return $"{spec} [{string.Join(";", attrs)}]";
            }

            return spec;
        }

        private IEnumerable<string> GetAttributes()
        {
            if (!string.IsNullOrWhiteSpace(Attrs))
            {
                yield return Attrs;
            }

            if (Length.HasValue)
            {
                yield return $"len={Length}";
            }

            if (Direction.HasValue)
            {
                yield return $"dir={Direction.Value.ToString().ToLower()}";
            }

            if (Style.HasValue)
            {
                yield return $"style={Style.Value.ToString().ToLower()}";
            }

            if (Weight.HasValue)
            {
                yield return $"weight={Weight.Value}";
            }

            if (Constraint.HasValue)
            {
                yield return $"constraint={Constraint.Value.ToString().ToLower()}";
            }

            if (Color.HasValue)
            {
                yield return $"color=\"{Color.Value.ToRgbHex()}\"";
            }

            if(Label != null)
            {
                yield return $"label=\"{Label}\"";
            }

            if (HeadPort.HasValue)
            {
                yield return $"headport={HeadPort.Value.ToString().ToLower()}";
            }

            if (TailPort.HasValue)
            {
                yield return $"tailport={TailPort.Value.ToString().ToLower()}";
            }
        }
    }
}
