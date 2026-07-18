
//phase-1
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VibeFlow.Compiler
{
    public class Lexer
    {
        private string source;
        private int position = 0, line = 1, column = 1;

        public Lexer(string sourceCode) { this.source = sourceCode; }

        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();

            while (position < source.Length)
            {
                char current = source[position];
                if (char.IsWhiteSpace(current)) { if (current == '\n') { line++; column = 0; } Advance(); continue; }
                if (current == '/' && Peek() == '/') { while (position < source.Length && source[position] != '\n') Advance(); continue; }

                string remaining = source.Substring(position);
                Match match;

                if ((match = Regex.Match(remaining, "^\"(.*?)\"")).Success) { tokens.Add(CreateToken(TokenType.StringLiteral, match.Value, match.Length)); continue; }
                if ((match = Regex.Match(remaining, "^[0-9]+(\\.[0-9]+)?")).Success) { tokens.Add(CreateToken(TokenType.Number, match.Value, match.Length)); continue; }
                if ((match = Regex.Match(remaining, "^(set|check|otherwise|loop|show)\\b")).Success) { tokens.Add(CreateToken(TokenType.Keyword, match.Value, match.Length)); continue; }
                if ((match = Regex.Match(remaining, "^[a-zA-Z_][a-zA-Z0-9_]*")).Success) { tokens.Add(CreateToken(TokenType.Identifier, match.Value, match.Length)); continue; }
                if ((match = Regex.Match(remaining, "^(==|>|<|\\+|-|=)")).Success) { tokens.Add(CreateToken(TokenType.Operator, match.Value, match.Length)); continue; }
                if ((match = Regex.Match(remaining, "^(\\{|\\}|\\(|\\)|;)")).Success) { tokens.Add(CreateToken(TokenType.Punctuation, match.Value, match.Length)); continue; }

                throw new Exception($"Lexical Error: Unrecognized character '{current}' at Line {line}");
            }
            tokens.Add(new Token { Type = TokenType.EOF, Value = "EOF", Line = line, Column = column });
            return tokens;
        }

        private Token CreateToken(TokenType type, string value, int length)
        {
            var token = new Token { Type = type, Value = value, Line = line, Column = column };
            position += length; column += length; return token;
        }
        private void Advance() { position++; column++; }
        private char Peek() => position + 1 < source.Length ? source[position + 1] : '\0';
    }
}