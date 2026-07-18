//Phase 4&5
namespace VibeFlow.Compiler
{
    public class PythonTranspiler
    {
        public string Transpile(ASTNode node, int indentLevel = 0)
        {
            string indent = new string(' ', indentLevel * 4);
            string code = "";

            if (node is VarDeclNode varNode)
            {
                string[] parts = varNode.Name.Split(':')[1].Split('=');
                code += $"{indent}{parts[0].Trim()} = {parts[1].Trim()}\n";
            }
            else if (node is PrintNode printNode)
            {
                code += $"{indent}print({printNode.Name.Substring(7)})\n";
            }
            else if (node is IfNode ifNode)
            {
                code += $"{indent}if {ifNode.Name.Substring(4)}:\n";
                foreach (var child in node.Children) code += Transpile(child, indentLevel + 1);
            }
            else if (node is LoopNode loopNode)
            {
                code += $"{indent}while {loopNode.Name.Substring(6)}:\n";
                foreach (var child in node.Children) code += Transpile(child, indentLevel + 1);
            }
            else if (node is ProgramNode)
            {
                foreach (var child in node.Children) code += Transpile(child, indentLevel);
            }
            return code;
        }
    }

    public class IRGenerator
    {
        private int tempCount = 1, labelCount = 1;

        public string GenerateIR(ASTNode node)
        {
            string irCode = "";

            if (node is VarDeclNode varNode)
            {
                string[] parts = varNode.Name.Split(':')[1].Split('=');
                irCode += $"{parts[0].Trim()} := {parts[1].Trim()}\n";
            }
            else if (node is PrintNode printNode)
            {
                irCode += $"param {printNode.Name.Substring(7)}\ncall print\n";
            }
            else if (node is IfNode ifNode)
            {
                string tempVar = $"t{tempCount++}";
                string labelEnd = $"L{labelCount++}";
                irCode += $"{tempVar} := {ifNode.Name.Substring(4)}\nifFalse {tempVar} goto {labelEnd}\n";
                foreach (var child in node.Children) irCode += GenerateIR(child);
                irCode += $"{labelEnd}:\n";
            }
            else if (node is LoopNode loopNode)
            {
                string tempVar = $"t{tempCount++}", labelStart = $"L{labelCount++}", labelEnd = $"L{labelCount++}";
                irCode += $"{labelStart}:\n{tempVar} := {loopNode.Name.Substring(6)}\nifFalse {tempVar} goto {labelEnd}\n";
                foreach (var child in node.Children) irCode += GenerateIR(child);
                irCode += $"goto {labelStart}\n{labelEnd}:\n";
            }
            else if (node is ProgramNode)
            {
                foreach (var child in node.Children) irCode += GenerateIR(child);
            }
            return irCode;
        }
    }
}