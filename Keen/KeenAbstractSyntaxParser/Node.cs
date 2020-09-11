using KeenTokenizer.Tokens;

namespace KeenAbstractSyntaxParser
{
    public class Node
    {
        public Token Value { get; set; }

        public Node FirstChild { get; set; }
        public Node SecondChild { get; set; }

        public void AddFirstChild(Token value)
        {
            FirstChild = new Node {Value = value};
        }
        
        public void AddSecondChild(Token value)
        {
            SecondChild = new Node {Value = value};
        }
    }
}