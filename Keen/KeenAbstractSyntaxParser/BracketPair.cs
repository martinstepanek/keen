namespace KeenAbstractSyntaxParser
{
    public class BracketPair
    {
        public int From { get; set; }
        public int To { get; set; }
        public BracketPair Parent { get; set; }
    }
}