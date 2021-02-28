using System.Collections.Generic;
using KeenTokenizer.Tokens;

namespace KeenAbstractSyntaxParser
{
    public class Node
    {
        public Token Value { get; set; }

        public List<Node> Children = new List<Node>();

        public Node FirstChild => Children.Count > 0 ? Children[0] : null;
        public Node SecondChild => Children.Count > 1 ? Children[1] : null;

    }
}