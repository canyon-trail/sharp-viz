using FluentAssertions;
using System;
using System.Drawing;
using Xunit;

namespace SharpViz.Tests
{
    public class NodeTests
    {
        [Theory]
        [InlineData("key", "key")]
        [InlineData("le key", "\"le key\"")]
        public void RenderKey(string key, string expected)
        {
            var testee = new Node(key);

            testee.Render().Should().Be(expected);
        }

        [Theory]
        [InlineData(null, "key")]
        [InlineData("labeltext", "key [label=\"labeltext\"]")]
        public void RenderLabel(string label, string expected)
        {
            var testee = new Node("key") { Label = label };

            testee.Render().Should().Be(expected);
        }

        [Theory]
        [InlineData(NodeStyle.None, "key")]
        [InlineData(NodeStyle.Filled, "key [style=filled]")]
        [InlineData(NodeStyle.Invisible, "key [style=invis]")]
        [InlineData(NodeStyle.Filled | NodeStyle.Rounded, "key [style=\"filled,rounded\"]")]
        public void RenderStyles(NodeStyle style, string expected)
        {
            var testee = new Node("key") { Style = style };

            testee.Render().Should().Be(expected);
        }

        [Fact]
        public void RenderMultipleAttrs()
        {
            var testee = new Node("key") {
                Style = NodeStyle.Filled,
                Label = "ye olde labele"
            };

            testee.Render().Should().Be("key [label=\"ye olde labele\";style=filled]");
        }

        [Fact]
        public void FillColor()
        {
            var testee = new Node("key")
            {
                FillColor = Color.FromArgb(255, 0, 255)
            };

            testee.Render().Should().Be("key [fillcolor=\"#ff00ff\"]");
        }
    }
}
