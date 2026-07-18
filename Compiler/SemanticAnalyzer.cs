//Phase-3
using System;
using System.Collections.Generic;

namespace VibeFlow.Compiler
{
    // 1. The Standard Lab-Manual Symbol Record
    public class SymbolRecord
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Scope { get; set; }
    }

    public class SemanticAnalyzer
    {
        // 2. Upgraded to a Dictionary (Hash Map)
        private Dictionary<string, SymbolRecord> symbolTable = new Dictionary<string, SymbolRecord>();

        public Dictionary<string, SymbolRecord> GetSymbols() => symbolTable;

        // 3. We pass 'currentScope' down the tree to track Global vs Local
        public void Analyze(ASTNode node, string currentScope = "Global")
        {
            if (node is VarDeclNode varNode)
            {
                string[] parts = varNode.Name.Split(':')[1].Split('=');
                string varName = parts[0].Trim();
                string varValue = parts[1].Trim();

                // DYNAMIC TYPE INFERENCE: Detects the type just like a real compiler
                string dataType = "Integer";
                if (varValue.StartsWith("\"")) dataType = "String";
                else if (varValue.Contains(".")) dataType = "Float";

                // Add to Hash Map
                symbolTable[varName] = new SymbolRecord { Name = varName, DataType = dataType, Scope = currentScope };
            }
            else if (node is IfNode ifNode)
            {
                string varUsed = ifNode.Name.Split(':')[1].Trim().Split(' ')[0];
                if (!symbolTable.ContainsKey(varUsed) && !double.TryParse(varUsed, out _))
                    throw new Exception($"Semantic Error: Variable '{varUsed}' is used before declaration.");

                // Enter local scope for 'check' block
                foreach (var child in node.Children) Analyze(child, "Local_Check");
            }
            else if (node is LoopNode loopNode)
            {
                string varUsed = loopNode.Name.Split(':')[1].Trim().Split(' ')[0];
                if (!symbolTable.ContainsKey(varUsed) && !double.TryParse(varUsed, out _))
                    throw new Exception($"Semantic Error: Variable '{varUsed}' is used before declaration.");

                // Enter local scope for 'loop' block
                foreach (var child in node.Children) Analyze(child, "Local_Loop");
            }
            else
            {
                foreach (var child in node.Children) Analyze(child, currentScope);
            }
        }
    }
}