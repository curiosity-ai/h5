using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H5.Fuzzer
{
    public class CodeGenerator
    {
        private readonly Random _random;
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly List<string> _intVariables = new List<string>();
        private readonly List<string> _boolVariables = new List<string>();
        private readonly List<MethodSignature> _methods = new List<MethodSignature>();
        private int _variableCounter = 0;
        private int _indentation = 0;
        private int _depth = 0;
        private const int MaxDepth = 5;
        private bool _currentMethodIsAsync = false;
        private string _currentMethodName = "";

        private class MethodSignature
        {
            public string Name { get; set; } = "";
            public string ReturnType { get; set; } = "void";
            public List<string> ParameterTypes { get; set; } = new List<string>();
            public bool IsAsync { get; set; }
        }

        private CodeGenerator(int seed)
        {
            _random = new Random(seed);
        }

        public static string Generate(int seed)
        {
            var generator = new CodeGenerator(seed);
            return generator.GenerateProgram();
        }

        private string GenerateProgram()
        {
            _builder.AppendLine("using System;");
            _builder.AppendLine("using System.Threading.Tasks;");
            _builder.AppendLine("using System.Collections.Generic;");
            _builder.AppendLine("using System.Linq;");
            _builder.AppendLine();
            _builder.AppendLine("public class Program");
            _builder.AppendLine("{");
            _indentation = 1;

            int methodCount = _random.Next(3, 8);
            for(int i=0; i<methodCount; i++)
            {
                GenerateMethod();
                _builder.AppendLine();
            }

            _builder.AppendLine("    public static async Task Main(string[] args)");
            _builder.AppendLine("    {");
            _indentation = 2;
            _currentMethodIsAsync = true;
            _intVariables.Clear();
            _boolVariables.Clear();

            AppendLine("Console.WriteLine(\"Start\");");

            for (int i = 0; i < 20; i++)
            {
                GenerateStatement();
            }

            AppendLine("Console.WriteLine(\"End\");");
            AppendLine("await Task.CompletedTask;");
            _indentation = 1;
            _builder.AppendLine("    }");

            _indentation = 0;
            _builder.AppendLine("}");

            return _builder.ToString();
        }

        private void GenerateMethod()
        {
             string name = $"Method_{_random.Next(1000)}_{_variableCounter++}";
             bool isAsync = _random.Next(2) == 0;
             string returnType = _random.Next(2) == 0 ? "int" : "void";
             if (isAsync) returnType = returnType == "void" ? "Task" : "Task<int>";

             // Parameters
             var paramsList = new List<string>();
             int paramCount = _random.Next(3);
             var paramDecl = new List<string>();
             for(int i=0; i<paramCount; i++)
             {
                 string pName = $"p{i}";
                 paramsList.Add("int");
                 paramDecl.Add($"int {pName}");
             }

             _methods.Add(new MethodSignature { Name = name, ReturnType = returnType, ParameterTypes = paramsList, IsAsync = isAsync });

             string decl = $"public static {(isAsync ? "async " : "")}{returnType} {name}({string.Join(", ", paramDecl)})";
             AppendLine(decl);
             AppendLine("{");
             _indentation++;
             _depth++;
             _currentMethodIsAsync = isAsync;
             _currentMethodName = name;

             _intVariables.Clear();
             _boolVariables.Clear();
             for(int i=0; i<paramCount; i++) _intVariables.Add($"p{i}");

             AppendLine($"Console.WriteLine(\"Inside {name}\");");

             int statements = _random.Next(2, 6);
             for(int i=0; i<statements; i++) GenerateStatement();

             if (returnType.Contains("int"))
             {
                 string ret = GenerateIntExpression();
                 AppendLine($"return {ret};");
             }
             else if (isAsync && returnType == "Task")
             {
                 // Ensure we await something or just return completed task if no awaits happened (CS1998)
                 // But generating 'await Task.CompletedTask' is safe.
                 AppendLine("await Task.CompletedTask;");
             }

             _indentation--;
             _depth--;
             _currentMethodName = "";
             AppendLine("}");
        }

        private void AppendLine(string line)
        {
            _builder.AppendLine(new string(' ', _indentation * 4) + line);
        }

        private void GenerateStatement()
        {
            if (_depth > MaxDepth)
            {
                GenerateConsoleWriteLine();
                return;
            }

            int choice = _random.Next(9);
            switch (choice)
            {
                case 0: GenerateIntDeclaration(); break;
                case 1: GenerateBoolDeclaration(); break;
                case 2: GenerateIntAssignment(); break;
                case 3: GenerateConsoleWriteLine(); break;
                case 4: GenerateIfStatement(); break;
                case 5: GenerateWhileStatement(); break;
                case 6: GenerateForStatement(); break;
                case 7: GenerateConsoleWriteLine(); break;
                case 8: GenerateMethodCall(); break;
            }
        }

        private void GenerateMethodCall()
        {
             if (_methods.Count == 0) return;

             // Filter methods: if current is sync, can only call sync methods (or avoid await)
             // But simpler: if current is sync, filter out async methods from candidates.
             // Also avoid self-recursion
             var candidates = _methods.Where(m => (_currentMethodIsAsync || !m.IsAsync) && m.Name != _currentMethodName).ToList();
             if (candidates.Count == 0) return;

             var method = candidates[_random.Next(candidates.Count)];

             // Arguments
             var args = new List<string>();
             foreach(var p in method.ParameterTypes)
             {
                 args.Add(GenerateIntExpression());
             }
             string argsStr = string.Join(", ", args);
             string call = $"{method.Name}({argsStr})";

             if (method.IsAsync)
             {
                 call = $"await {call}";
             }

             if (method.ReturnType.Contains("int"))
             {
                 string varName = $"ret_{_variableCounter++}";
                 AppendLine($"int {varName} = {call};");
                 _intVariables.Add(varName);
                 AppendLine($"Console.WriteLine(\"{varName} = \" + {varName});");
             }
             else
             {
                 AppendLine($"{call};");
             }
        }

        private void GenerateIntDeclaration()
        {
            string name = $"v{_variableCounter++}";
            string expr = GenerateIntExpression();
            AppendLine($"int {name} = unchecked({expr});");
            _intVariables.Add(name);
            AppendLine($"Console.WriteLine(\"{name} = \" + {name});");
        }

        private void GenerateBoolDeclaration()
        {
            string name = $"b{_variableCounter++}";
            string expr = GenerateBoolExpression();
            AppendLine($"bool {name} = {expr};");
            _boolVariables.Add(name);
            AppendLine($"Console.WriteLine(\"{name} = \" + {name});");
        }

        private void GenerateIntAssignment()
        {
            if (_intVariables.Count == 0)
            {
                GenerateIntDeclaration();
                return;
            }
            string name = _intVariables[_random.Next(_intVariables.Count)];
            string expr = GenerateIntExpression();
            AppendLine($"{name} = unchecked({expr});");
            AppendLine($"Console.WriteLine(\"Updated {name} = \" + {name});");
        }

        private void GenerateConsoleWriteLine()
        {
            if (_intVariables.Count > 0 && _random.Next(2) == 0)
            {
                string name = _intVariables[_random.Next(_intVariables.Count)];
                AppendLine($"Console.WriteLine(\"Value of {name}: \" + {name});");
            }
            else
            {
                AppendLine($"Console.WriteLine(\"Random output: {_random.Next()}\");");
            }
        }

        private void GenerateIfStatement()
        {
            string cond = GenerateBoolExpression();
            AppendLine($"if ({cond})");
            GenerateBlock();
            if (_random.Next(2) == 0)
            {
                AppendLine("else");
                GenerateBlock();
            }
        }

        private void GenerateWhileStatement()
        {
            string counterName = $"loop_{_variableCounter++}";
            string limit = _random.Next(5, 10).ToString();

            AppendLine($"int {counterName} = 0;");
            string cond = GenerateBoolExpression();

            AppendLine($"while ({counterName} < {limit})");
            AppendLine("{");
            _indentation++;
            _depth++;

            AppendLine($"if (!({cond})) break;");

            int statements = _random.Next(1, 4);
            int originalIntCount = _intVariables.Count;
            int originalBoolCount = _boolVariables.Count;

            for(int i=0; i<statements; i++) GenerateStatement();

            RemoveVariablesSince(originalIntCount, _intVariables);
            RemoveVariablesSince(originalBoolCount, _boolVariables);

            AppendLine($"{counterName}++;");
            _indentation--;
            _depth--;
            AppendLine("}");
        }

        private void GenerateForStatement()
        {
            string varName = $"i_{_variableCounter++}";
            string limit = _random.Next(5, 10).ToString();

            AppendLine($"for (int {varName} = 0; {varName} < {limit}; {varName}++)");
            GenerateBlock();
        }

        private void GenerateBlock()
        {
            AppendLine("{");
            _indentation++;
            _depth++;
            int originalIntCount = _intVariables.Count;
            int originalBoolCount = _boolVariables.Count;

            int count = _random.Next(1, 4);
            for(int i=0; i<count; i++) GenerateStatement();

            RemoveVariablesSince(originalIntCount, _intVariables);
            RemoveVariablesSince(originalBoolCount, _boolVariables);

            _indentation--;
            _depth--;
            AppendLine("}");
        }

        private void RemoveVariablesSince(int count, List<string> list)
        {
            if (list.Count > count)
            {
                list.RemoveRange(count, list.Count - count);
            }
        }

        private string GenerateIntExpression(int depth = 0)
        {
            if (depth > 3 || _random.Next(3) == 0)
            {
                if (_intVariables.Count > 0 && _random.Next(2) == 0)
                {
                    return _intVariables[_random.Next(_intVariables.Count)];
                }
                return _random.Next(-100, 101).ToString();
            }

            int op = _random.Next(3);
            string left = GenerateIntExpression(depth + 1);
            string right = GenerateIntExpression(depth + 1);

            switch (op)
            {
                case 0: return $"({left} + {right})";
                case 1: return $"({left} - {right})";
                case 2: return $"({left} * {right})";
                default: return left;
            }
        }

        private string GenerateBoolExpression(int depth = 0)
        {
             if (depth > 2 || _random.Next(3) == 0)
            {
                if (_boolVariables.Count > 0 && _random.Next(2) == 0)
                {
                    return _boolVariables[_random.Next(_boolVariables.Count)];
                }
                return (_random.Next(2) == 0 ? "true" : "false");
            }

            if (_intVariables.Count > 0 && _random.Next(2) == 0)
            {
                string left = GenerateIntExpression(depth + 1);
                string right = GenerateIntExpression(depth + 1);
                int op = _random.Next(4);
                switch(op)
                {
                    case 0: return $"({left} == {right})";
                    case 1: return $"({left} != {right})";
                    case 2: return $"({left} > {right})";
                    case 3: return $"({left} < {right})";
                }
            }

            string bLeft = GenerateBoolExpression(depth + 1);
            string bRight = GenerateBoolExpression(depth + 1);
            return _random.Next(2) == 0 ? $"({bLeft} && {bRight})" : $"({bLeft} || {bRight})";
        }
    }
}
