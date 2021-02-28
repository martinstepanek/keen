using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using KeenTokenizer.Tokens;

namespace KeenTokenizer
{
    public class Tokenizer
    {
        private string _code;
        private Token _currentToken;
        private List<Token> _tokens;

        public Tokenizer(string code)
        {
            _code = code;
        }

        public List<Token> GetTokens()
        {
            CleanCode();
            Tokenize();
            return _tokens;
        }

        private void CleanCode()
        {
            // replace all white spaces except ones in single quotes (')
            _code = Regex.Replace(_code, @"[ ]+(?=[^']*(?:'[^']*'[^']*)*$)", "");
        }

        private void Tokenize()
        {
            _tokens = new List<Token>();
            _currentToken = null;

            for (int i = 0; i < _code.Length; i++)
            {
                char ch = _code[i];

                if (ch != '\'' && _currentToken is QuotedContent)
                {
                    _currentToken.Value += ch;
                    continue;
                }
                
                if (ch == '\'')
                {
                    if (!(_currentToken is QuotedContent))
                    {
                        // beginning of quotes
                        StashCurrentToken();
                        _currentToken = new QuotedContent();
                    }
                    else
                    {
                        // end of quotes
                        StashCurrentToken();
                    }
                }

                if (Char.IsLetter(ch))
                {
                    if (!(_currentToken is Word))
                    {
                        StashCurrentToken();
                        _currentToken = new Word();
                    }

                    _currentToken.Value += ch;
                }

                if (Char.IsDigit(ch))
                {
                    if (!(_currentToken is Number))
                    {
                        StashCurrentToken();
                        _currentToken = new Number();
                    }

                    _currentToken.Value += ch;
                }

                if (ch == '(')
                {
                    StashCurrentToken();
                    _currentToken = new OpeningBracket();
                    StashCurrentToken();
                }

                if (ch == ')')
                {
                    StashCurrentToken();
                    _currentToken = new ClosingBracket();
                    StashCurrentToken();
                }

                if (ch == ';')
                {
                    StashCurrentToken();
                    _currentToken = new Semicolon();
                    StashCurrentToken();
                }

                if (ch == '.')
                {
                    StashCurrentToken();
                    _currentToken = new Dot();
                    StashCurrentToken();
                }

                if (ch == ',')
                {
                    StashCurrentToken();
                    _currentToken = new Comma();
                    StashCurrentToken();
                }
            }

            StashCurrentToken();
        }

        private void StashCurrentToken()
        {
            if (_currentToken != null)
            {
                _tokens.Add(_currentToken);
                _currentToken = null;
            }
        }
    }
}