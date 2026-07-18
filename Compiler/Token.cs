using System.Collections.Generic;

namespace VibeFlow.Compiler
{
    public enum TokenType { Keyword, Identifier, Number, StringLiteral, Operator, Punctuation, EOF, Error }

    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public override string ToString() => $"<{Type}, '{Value}'> at Line {Line}";
    }

    public abstract class ASTNode
    {
        public string Name { get; set; }
        public List<ASTNode> Children { get; set; } = new List<ASTNode>();
    }

    public class ProgramNode : ASTNode { public ProgramNode() { Name = "Program"; } }
    public class VarDeclNode : ASTNode { public VarDeclNode(string id, string val) { Name = $"Declaration: {id} = {val}"; } }
    public class PrintNode : ASTNode { public PrintNode(string text) { Name = $"Print: {text}"; } }
    public class IfNode : ASTNode { public IfNode(string cond) { Name = $"If: {cond}"; } }
    public class LoopNode : ASTNode { public LoopNode(string cond) { Name = $"Loop: {cond}"; } }
}