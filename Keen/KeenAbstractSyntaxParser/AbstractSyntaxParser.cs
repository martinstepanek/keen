using System;
using System.Collections.Generic;
using System.Linq;
using KeenTokenizer.Tokens;

namespace KeenAbstractSyntaxParser
{
    public class AbstractSyntaxParser
    {
        private readonly List<Token> _tokens;
        private List<Node> _nodes;

        public AbstractSyntaxParser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public List<Node> GetNodes()
        {
            Parse();
            return _nodes;
        }

        private void Parse()
        {
            _nodes = new List<Node>();
            var rowTokens = GetRowTokens();

            foreach (var rowToken in rowTokens)
            {
                _nodes.Add(GetRowNode(rowToken));
            }
        }

        private List<BracketPair> GetBracketPairs(List<Token> tokens)
        {
            var stack = new Stack<BracketPair>();
            var pairs = new List<BracketPair>();

            for (var i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] is OpeningBracket)
                {
                    var bracketPair = new BracketPair {From = i};
                    if (stack.Count > 0)
                    {
                        bracketPair.Parent = stack.Peek();
                    }

                    stack.Push(bracketPair);
                }

                if (tokens[i] is ClosingBracket)
                {
                    var pair = stack.Pop();
                    pair.To = i;
                    pairs.Add(pair);
                }
            }

            return pairs;
        }

        private Node GetRowNode(List<Token> tokens)
        {
            var bracketPairs = GetBracketPairs(tokens);
            var node = GetNode(bracketPairs, tokens, 0, tokens.Count - 1);
            return node;
        }

        private Node GetNode(List<BracketPair> bracketPairs, List<Token> tokens, int from, int to)
        {
            if (to < from)
            {
                return null;
            }

            var lastIndex = to;
            var lastToken = tokens[lastIndex];

            if (lastToken is ClosingBracket)
            {
                var node = new Node();
                var bracketPair = bracketPairs.Find(pair => pair.To == lastIndex);
                var parent = tokens[bracketPair!.From - 1]; // function name
                var firstChildIndex = bracketPair.From - 3;
                var firstChild = tokens[firstChildIndex]; // function name, dot and variable name
                Node childNode = null;
                
                node.Value = parent;

                if (firstChild is ClosingBracket)
                {
                    var innerFrom = bracketPair.Parent?.From + 1 ?? 0;
                    childNode = GetNode(bracketPairs, tokens, innerFrom, firstChildIndex);
                    if (childNode != null)
                    {
                        node.Children.Add(childNode);
                    }
                }
                else
                {
                    node.Children.Add(new Node {Value = firstChild});
                }

                childNode = GetNode(bracketPairs, tokens, bracketPair.From + 1, bracketPair.To - 1);
                if (childNode != null)
                {
                    node.Children.Add(childNode);
                }

                return node;
            }

            if (lastToken is Word || lastToken is Number || lastToken is QuotedContent)
            {
                return new Node {Value = lastToken};
            }

            throw new Exception("Unexpected token: " + lastToken.GetType());
        }

        private IEnumerable<List<Token>> GetRowTokens()
        {
            var rowTokens = new List<List<Token>>();
            var currentRowToken = new List<Token>();

            foreach (var token in _tokens)
            {
                if (token is Semicolon)
                {
                    rowTokens.Add(currentRowToken);
                    currentRowToken = new List<Token>();
                }
                else
                {
                    currentRowToken.Add(token);
                }
            }

            return rowTokens;
        }
    }
}