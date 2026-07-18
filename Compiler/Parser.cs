//Phase - 2
using System;
using System.Collections.Generic;

namespace VibeFlow.Compiler
{
    public class Parser
    {
        private List<Token> tokens;
        private int pos = 0;

        public Parser(List<Token> tokens) { this.tokens = tokens; }

        public ProgramNode Parse()
        {
            ProgramNode program = new ProgramNode();
            while (pos < tokens.Count && tokens[pos].Type != TokenType.EOF)
            {
                if (Match("set")) program.Children.Add(ParseDeclaration());
                else if (Match("show")) program.Children.Add(ParsePrint());
                else if (Match("check")) program.Children.Add(ParseIf());
                else if (Match("loop")) program.Children.Add(ParseLoop());
                else throw new Exception($"Syntax Error at Line {tokens[pos].Line}: Unexpected token '{tokens[pos].Value}'. Expected a statement (set, show, check, loop).");
            }
            return program;
        }

        private VarDeclNode ParseDeclaration()
        {
            string id = tokens[pos].Value;
            Expect(TokenType.Identifier); // Must be a variable name
            Expect("=");

            string val = tokens[pos].Value;
            if (tokens[pos].Type == TokenType.Number || tokens[pos].Type == TokenType.Identifier || tokens[pos].Type == TokenType.StringLiteral) pos++;
            else throw new Exception($"Syntax Error at Line {tokens[pos].Line}: Expected a number, string, or variable after '='.");
            if (pos < tokens.Count && (tokens[pos].Value == "+" || tokens[pos].Value == "-" || tokens[pos].Value == "*"))
            {
                val += $" {tokens[pos++].Value} {tokens[pos++].Value}";
            }
            Expect(";"); // Will throw error if semicolon is missing
            return new VarDeclNode(id, val);
        }

        private PrintNode ParsePrint()
        {
            Expect("(");
            string text = tokens[pos].Value;

            // Allow raw strings, numbers, OR variables (Identifiers) inside show()
            if (tokens[pos].Type == TokenType.StringLiteral ||
                tokens[pos].Type == TokenType.Number ||
                tokens[pos].Type == TokenType.Identifier)
            {
                pos++;
            }
            else
            {
                throw new Exception($"Syntax Error at Line {tokens[pos].Line}: Expected a string, number, or variable inside show().");
            }

            Expect(")");
            Expect(";");
            return new PrintNode(text);
        }

        private IfNode ParseIf()
        {
            Expect("(");
            string condition = tokens[pos].Value + " " + tokens[pos + 1].Value + " " + tokens[pos + 2].Value;
            pos += 3;
            Expect(")");
            Expect("{");

            IfNode ifNode = new IfNode(condition);
            while (pos < tokens.Count && tokens[pos].Value != "}")
            {
                if (Match("set")) ifNode.Children.Add(ParseDeclaration());
                else if (Match("show")) ifNode.Children.Add(ParsePrint());
                else throw new Exception($"Syntax Error at Line {tokens[pos].Line}: Invalid statement inside 'check' block.");
            }
            Expect("}");
            return ifNode;
        }

        private LoopNode ParseLoop()
        {
            Expect("(");
            string condition = tokens[pos].Value + " " + tokens[pos + 1].Value + " " + tokens[pos + 2].Value;
            pos += 3;
            Expect(")");
            Expect("{");

            LoopNode loopNode = new LoopNode(condition);
            while (pos < tokens.Count && tokens[pos].Value != "}")
            {
                if (Match("set")) loopNode.Children.Add(ParseDeclaration());
                else if (Match("show")) loopNode.Children.Add(ParsePrint());
                else if (Match("check")) loopNode.Children.Add(ParseIf());
                else throw new Exception($"Syntax Error at Line {tokens[pos].Line}: Invalid statement inside 'loop' block.");
            }
            Expect("}");
            return loopNode;
        }

        // The Strict Enforcer
        private void Expect(string expectedValue)
        {
            if (pos < tokens.Count && tokens[pos].Value == expectedValue) pos++;
            else throw new Exception($"Syntax Error at Line {tokens[pos].Line}: Expected '{expectedValue}' but found '{tokens[pos].Value}'.");
        }

        // The Strict Type Enforcer
        private void Expect(TokenType expectedType)
        {
            if (pos < tokens.Count && tokens[pos].Type == expectedType) pos++;
            else throw new Exception($"Syntax Error at Line {tokens[pos].Line}: Expected a {expectedType} but found '{tokens[pos].Value}'.");
        }

        private bool Match(string expectedValue)
        {
            if (pos < tokens.Count && tokens[pos].Value == expectedValue) { pos++; return true; }
            return false;
        }
    }
}