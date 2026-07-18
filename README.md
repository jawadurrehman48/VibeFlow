# 🚀 VibeFlow IDE & Transpiler

> **A custom programming language, compiler, and integrated development environment (IDE) built from scratch using C# and .NET Windows Forms. VibeFlow demonstrates the complete compiler pipeline by translating VibeScript programs into executable Python code while visualizing every stage of compilation.**

---

## 🌟 Highlights

✨ Custom Programming Language (VibeScript)

⚙️ Complete 6-Phase Compiler

🌳 Abstract Syntax Tree (AST)

📑 Symbol Table Generation

🧠 Semantic Analysis

🔄 Three Address Code (3AC)

🐍 Python Code Generation

🖥 Interactive Desktop IDE

📖 Built-in Language Documentation

---

## 📸 Preview

### Workspace

![Workspace](screenshots/workspace.png)

### Compiler Pipeline

![Pipeline](screenshots/compiler-pipeline.png)

### Language Documentation

![Docs](screenshots/vibescript-docs.png)

---

## 🎯 About the Project

VibeFlow is a compiler construction project designed to demonstrate the internal working of modern compilers through an easy-to-use graphical interface.

Instead of simply translating source code, VibeFlow allows users to visualize every stage of compilation—from token generation and syntax analysis to semantic checking, abstract syntax tree creation, intermediate code generation, and Python transpilation.

The project introduces **VibeScript**, a lightweight programming language featuring custom syntax for variables, conditional statements, loops, and console output.

---

## 🔄 Compilation Workflow

```
VibeScript Source Code
          │
          ▼
 Lexical Analysis
          │
          ▼
 Syntax Analysis
          │
          ▼
 Semantic Analysis
          │
          ▼
 Abstract Syntax Tree
          │
          ▼
 Three Address Code
          │
          ▼
 Python Code Generation
```

---

## 💻 Example

### VibeScript

```vb
set count = 5;

check (count > 0) {
    show("Math works!");
}
```

### Generated Python

```python
count = 5

if count > 0:
    print("Math works!")
```

---

## 🔑 VibeScript Keywords

| Keyword | Purpose |
|----------|---------|
| `set` | Declare Variables |
| `check` | Conditional Statement |
| `otherwise` | Else Block |
| `loop` | Loop Execution |
| `show` | Console Output |

---

## 🛠 Built With

- C#
- .NET Windows Forms
- Compiler Construction
- Recursive Descent Parser
- AST Generation
- Three Address Code (3AC)
- Python Code Generator

---

## 🧩 Core Components

- ✔ Lexer
- ✔ Parser
- ✔ Semantic Analyzer
- ✔ Symbol Table
- ✔ AST Builder
- ✔ Intermediate Code Generator
- ✔ Python Transpiler
- ✔ Desktop IDE

---

## 🚀 Future Roadmap

- Functions
- Arrays
- User-defined Classes
- Compiler Optimizations
- Error Recovery
- Syntax Highlighting
- Auto Complete
- Built-in Debugger
- Execute Python from IDE

---

## 👨‍💻 Developer

**Jawad Ur Rehman**

Computer Science Student • Software Developer • UI/UX Designer

GitHub:
https://github.com/jawadurrehman48

---

## ⭐ If you like this project...

Leave a ⭐ on the repository!

It helps support future development and motivates me to build more compiler and programming language projects.

---
