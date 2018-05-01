using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SharpViz
{
    public sealed class LegendNode
    {
        private readonly List<Color> _colors = new List<Color>();
        private readonly List<string> _colorLabels = new List<string>();

        private readonly List<string> _lineStyles = new List<string>();
        private readonly List<string> _lineLabels = new List<string>();

        public LegendNode() : this("legend")
        {

        }

        public LegendNode(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public LegendNode AddColor(Color color, string label)
        {
            _colors.Add(color);
            _colorLabels.Add(label);

            return this; // ugh... forgive me...
        }

        public LegendNode AddLine(string lineStyle, string label)
        {
            _lineStyles.Add(lineStyle);
            _lineLabels.Add(label);

            return this; // ugh... forgive me...
        }

        public string Render()
        {
            var colorRows = _colors
                .Zip(_colorLabels, (color, label) => RenderColorRow(color, label));

            var lineLeftPlaceholders = _lineStyles
                .Select((style, idx) => $"<tr><td port=\"l{idx}\">&nbsp;</td></tr>");

            var lineStyleLabels = _lineLabels
                .Select((label, idx) => $"<tr><td COLOR=\"white\"></td><td port=\"r{idx}\">{label}</td></tr>");

            var lineStyleEdges = _lineStyles
                .Select((style, idx) => $"legendOther:l{idx}:e -> legend:r{idx}:w [{style};weight=0]");

            return $@"
    subgraph cluster_legend {{
        fontname=""Open Sans"";
        label=""Legend""
        legendOther [shape=none;label=<
            <TABLE BORDER=""0"" CELLBORDER=""1"" COLOR=""white"" CELLSPACING=""0"" CELLPADDING=""4"">
                {string.Join("\n", _colors.Select(x => "<tr><td>&nbsp;</td></tr>"))}
                {string.Join("\n", lineLeftPlaceholders)}
            </TABLE>
        >];

        legend [shape=none;label=<
            <TABLE BORDER=""0"" CELLBORDER=""1"" CELLSPACING=""0"" CELLPADDING=""4"">
                {string.Join("\n", colorRows)}
                {string.Join("\n", lineStyleLabels)}
            </TABLE>
        >];

        {string.Join("\n", lineStyleEdges)}
    }}
";
        }

        private static string RenderColorRow(Color c, string label)
        {
            return $@"<tr>
                
                <td BGCOLOR=""{c.ToRgbHex()}"">&nbsp;&nbsp;</td>
                
                <td>&nbsp;{label}</td>
            </tr>";
        }
    }
}
