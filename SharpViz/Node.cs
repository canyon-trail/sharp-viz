using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SharpViz
{
    [Flags]
    public enum NodeStyle
    {
        None        = 0,
        Filled      = 0x1,
        Rounded     = 0x2,
        Invisible   = 0x4,
        Dashed      = 0x8
    }

    public enum NodeShape
    {
        Box,
        Parallelogram,
        Diamond,
        Circle,
        Ellipse,
        Point,
        Plaintext
    }

    public sealed class Node
    {
        public string Key { get; }

        public string Label { get; set; }

        public NodeStyle Style { get; set; }

        public NodeShape? Shape { get; set; }

        public Color? FillColor { get; set; }

        public Color? Color { get; set; }

        public string FontName { get; set; }

        public string FontSize { get; set; }

        public double? Height { get; set; }
        public double? Width { get; set; }

        public string Attributes { get; set; }

        public Node(string key)
        {
            Key = key;
        }

        public string Render()
        {
            var keyText = RenderKey();

            var attrs = GetAttributes()
                .OrderBy(x => x)
                .ToArray();

            if (!attrs.Any())
                return keyText;
            
            return $"{keyText} [{string.Join(";", attrs)}]";
        }

        private string RenderKey()
        {
            if (Key.Contains(" "))
                return $"\"{Key}\"";

            return Key;
        }

        private IEnumerable<string> GetAttributes()
        {
            if (Label != null)
            {
                yield return $"label=\"{Label}\"";
            }

            if (Style != NodeStyle.None)
            {
                yield return $"style={GetStyleString(Style)}";
            }

            if(Shape != null)
            {
                yield return $"shape={Shape.ToString().ToLower()}";
            }

            if(FillColor.HasValue)
            {
                var colorVal = FillColor
                    .Value
                    .ToArgb()
                    .ToString("x8")
                    .Substring(2);

                yield return $"fillcolor=\"#{colorVal}\"";
            }

            if (Color.HasValue)
            {
                var colorVal = Color
                    .Value
                    .ToArgb()
                    .ToString("x8")
                    .Substring(2);

                yield return $"color=\"#{colorVal}\"";
            }

            if (FontName != null)
            {
                yield return $"fontname=\"{FontName}\"";
            }

            if(FontSize != null)
            {
                yield return $"fontsize=\"{FontSize}\"";
            }

            if (Height != null)
            {
                yield return $"height=\"{Height.Value}\"";
            }

            if (Width != null)
            {
                yield return $"width=\"{Width.Value}\"";
            }

            if(Attributes != null)
            {
                yield return Attributes;
            }
        }

        private static string GetStyleString(NodeStyle style)
        {
            switch (style)
            {
                case NodeStyle.Invisible:
                    return "invis";
                case NodeStyle.Filled:
                case NodeStyle.Rounded:
                case NodeStyle.Dashed:
                    return style.ToString().ToLower();
            }

            var flagStrings = Enum.GetValues(typeof(NodeStyle))
                .Cast<NodeStyle>()
                .Where(x => x != NodeStyle.None)
                .Where(x => style.HasFlag(x))
                .Select(x => GetStyleString(x))
                .OrderBy(x => x)
                .ToArray();

            return $"\"{string.Join(",", flagStrings)}\"";
        }
    }
}
