using VibeFlow.Compiler;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VibeFlow
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CompileBtn_Click(object sender, RoutedEventArgs e)
        {
            ConsoleOutput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4EC9B0"));
            ConsoleOutput.Text = "Running 6-Phase Compilation...\n";

            // Clear previous UI states
            ASTTreeView.Items.Clear();
            TokenListBox.Items.Clear();
            SymbolListBox.Items.Clear();
            IROutput.Clear();

            try
            {
                string sourceCode = CodeEditor.Text;

                // PHASE 1: Lexical Analysis
                Lexer lexer = new Lexer(sourceCode);
                List<Token> tokens = lexer.Tokenize();
                foreach (var token in tokens) TokenListBox.Items.Add(token.ToString());

                // PHASE 2: Syntax Analysis (AST Construction)
                Parser parser = new Parser(tokens);
                ProgramNode ast = parser.Parse();

                TreeViewItem rootItem = new TreeViewItem { Header = ast.Name, IsExpanded = true, Foreground = Brushes.White };
                BuildASTTree(ast, rootItem);
                ASTTreeView.Items.Add(rootItem);

                // PHASE 3: Semantic Analysis
                SemanticAnalyzer semantics = new SemanticAnalyzer();
                semantics.Analyze(ast);
                foreach (var kvp in semantics.GetSymbols())
                {
                    var sym = kvp.Value;
                    // Formats the string to look like a professional memory table
                    SymbolListBox.Items.Add(string.Format("{0,-12} | {1,-8} | {2}", sym.Name, sym.DataType, sym.Scope));
                }

                // PHASE 4: Optimization (Optional - skipped for lean build)

                // PHASE 5: Intermediate Representation (3AC)
                IRGenerator irGen = new IRGenerator();
                IROutput.Text = irGen.GenerateIR(ast);

                // PHASE 6: Target Code Generation (Python)
                PythonTranspiler transpiler = new PythonTranspiler();
                string pythonCode = transpiler.Transpile(ast);

                ConsoleOutput.Text += "\n--- Compilation Successful! ---\n\n";
                ConsoleOutput.Text += pythonCode;
            }
            catch (Exception ex)
            {
                ConsoleOutput.Foreground = new SolidColorBrush(Colors.Red);
                ConsoleOutput.Text = $"❌ COMPILER ERROR:\n{ex.Message}";
            }
        }

        private void BuildASTTree(ASTNode node, TreeViewItem treeItem)
        {
            foreach (var child in node.Children)
            {
                TreeViewItem childItem = new TreeViewItem { Header = child.Name, IsExpanded = true, Foreground = Brushes.White };
                treeItem.Items.Add(childItem);
                BuildASTTree(child, childItem);
            }
        }
    }
}