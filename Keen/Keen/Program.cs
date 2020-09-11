using System;
using System.IO;
using KeenAbstractSyntaxParser;
using KeenActionParser;
using KeenInterpreter;
using KeenTokenizer;

namespace Keen
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = File.ReadAllText("../../../../../exampleCode.kn");

            var tokenizer = new Tokenizer(code);
            var tokens = tokenizer.GetTokens();
            
            var abstractSyntaxParser = new AbstractSyntaxParser(tokens);
            var nodes = abstractSyntaxParser.GetNodes();
            
            var actionParser = new ActionParser(nodes);
            var expressions = actionParser.GetExpressions();
            
            var interpreter = new Interpreter();
            interpreter.Run(expressions);
            
            Console.ReadLine();
        }
    }
}