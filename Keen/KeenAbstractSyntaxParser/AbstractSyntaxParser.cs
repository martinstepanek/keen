using System;
using System.Collections.Generic;
using KeenTokenizer.Tokens;

namespace KeenAbstractSyntaxParser
{
    public class AbstractSyntaxParser
    {
        private List<Token> _tokens;
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

            for (int i = 0; i < tokens.Count; i++)
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

                node.Value = parent;

                if (firstChild is ClosingBracket)
                {
                    var innerFrom = bracketPair.Parent?.From + 1 ?? 0;
                    node.FirstChild = GetNode(bracketPairs, tokens, innerFrom, firstChildIndex);
                }
                else
                {
                    node.AddFirstChild(firstChild);
                }

                node.SecondChild = GetNode(bracketPairs, tokens, bracketPair.From + 1, bracketPair.To - 1);

                return node;
            }

            if (lastToken is Word || lastToken is Number || lastToken is QuotedContent)
            {
                return new Node {Value = lastToken};
            }

            throw new Exception("Unexpected token: " + lastToken.GetType());
        }

        private List<List<Token>> GetRowTokens()
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