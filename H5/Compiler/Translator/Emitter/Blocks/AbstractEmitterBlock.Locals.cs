using System;
using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public partial class AbstractEmitterBlock
    {
        public virtual void PushLocals()
        {
            if (Emitter.LocalsStack == null)
            {
                Emitter.LocalsStack = new Stack<Dictionary<string, AstType>>();
            }

            // Pushes even if null, else it will have nothing to pull later and another test will be needed.
            Emitter.LocalsStack.Push(Emitter.Locals);

            if (Emitter.Locals != null)
            {
                Emitter.Locals = new Dictionary<string, AstType>(Emitter.Locals);
            }
            else
            {
                Emitter.Locals = new Dictionary<string, AstType>();
            }
        }

        public virtual void PopLocals()
        {
            Emitter.Locals = Emitter.LocalsStack.Pop();
        }

        public virtual void ResetLocals()
        {
            Emitter.NamedTempVariables = new Dictionary<string, string>();
            Emitter.TempVariables = new Dictionary<string, bool>();
            Emitter.Locals = new Dictionary<string, AstType>();
            Emitter.IteratorCount = 0;
        }

        public virtual void AddLocals(IEnumerable<ParameterDeclaration> declarations, AstNode statement)
        {
            var visitor = new ReferenceArgumentVisitor(Emitter);
            statement.AcceptVisitor(visitor);

            declarations.ToList().ForEach(item =>
            {
                var rr = item.Parent != null ? (LocalResolveResult)Emitter.Resolver.ResolveNode(item) : null;
                var name = Emitter.GetParameterName(item);
                var vName = AddLocal(item.Name, item, item.Type, name);

                if (item.Parent == null && item.Name == "value" && visitor.DirectionExpression.Any(expr => expr is IdentifierExpression && ((IdentifierExpression)expr).Identifier == "value"))
                {
                    return;
                }

                if (item.ParameterModifier == ParameterModifier.Out || item.ParameterModifier == ParameterModifier.Ref)
                {
                    Emitter.LocalsMap[rr != null ? rr.Variable : new DefaultVariable(ReflectionHelper.FindType(Emitter.Resolver.Compilation, TypeCode.Object), name)] = vName + ".v";
                }
                else
                {
                    Emitter.LocalsMap[rr != null ? rr.Variable : new DefaultVariable(ReflectionHelper.FindType(Emitter.Resolver.Compilation, TypeCode.Object), name)] = vName;
                }
            });

            foreach (var expr in visitor.DirectionExpression)
            {
                var rr = Emitter.Resolver.ResolveNode(expr);
                if (rr is LocalResolveResult lrr && (expr is IdentifierExpression identifierExpression))
                {
                    var name = identifierExpression.Identifier;
                    if (Helpers.IsReservedWord(Emitter, name))
                    {
                        name = Helpers.ChangeReservedWord(name);
                    }
                    Emitter.LocalsMap[lrr.Variable] = name + ".v";
                }
            }

            foreach (var variable in visitor.DirectionVariables)
            {
                var name = variable.Name;

                if (Helpers.IsReservedWord(Emitter, name))
                {
                    name = Helpers.ChangeReservedWord(name);
                }
                Emitter.LocalsMap[variable] = name + ".v";
            }
        }

        public string AddLocal(string name, AstNode node, AstType type, string valueName = null)
        {
            if (Emitter.Locals.ContainsKey(name))
            {
                throw new EmitterException(node, string.Format(Constants.Messages.Exceptions.DUPLICATE_LOCAL_VARIABLE, name));
            }

            Emitter.Locals.Add(name, type);

            name = name.StartsWith(JS.Vars.FIX_ARGUMENT_NAME) ? name.Substring(JS.Vars.FIX_ARGUMENT_NAME.Length) : name;
            string vName = valueName ?? name;

            if (Helpers.IsReservedWord(Emitter, vName))
            {
                vName = Helpers.ChangeReservedWord(vName);
            }

            if (!Emitter.LocalsNamesMap.ContainsKey(name))
            {
                if (Emitter.LocalsNamesMap.ContainsValue(name))
                {
                    Emitter.LocalsNamesMap.Add(name, GetUniqueNameByValue(vName));
                }
                else
                {
                    Emitter.LocalsNamesMap.Add(name, vName);
                }
            }
            else
            {
                Emitter.LocalsNamesMap[name] = GetUniqueName(vName);
            }

            var result = Emitter.LocalsNamesMap[name];
            var lrr = node != null && node.Parent != null ? Emitter.Resolver.ResolveNode(node) as LocalResolveResult : null;

            if (Emitter.LocalsMap != null && lrr != null && Emitter.LocalsMap.ContainsKey(lrr.Variable))
            {
                var oldValue = Emitter.LocalsMap[lrr.Variable];
                Emitter.LocalsMap[lrr.Variable] = result + (oldValue.EndsWith(".v") ? ".v" : "");
            }

            if (Emitter.IsAsync && !Emitter.AsyncVariables.Contains(result) && (lrr == null || !lrr.IsParameter))
            {
                Emitter.AsyncVariables.Add(result);
            }

            return result;
        }

        protected virtual string GetUniqueNameByValue(string name)
        {
            int index = 1;
            string tempName = name + index;

            while (Emitter.LocalsNamesMap.ContainsValue(tempName) || Helpers.IsReservedWord(Emitter, tempName))
            {
                tempName = name + ++index;
            }

            return tempName;
        }

        protected virtual string GetUniqueName(string name)
        {
            int index = 1;

            if (Emitter.LocalsNamesMap.ContainsKey(name))
            {
                var value = Emitter.LocalsNamesMap[name];
                if (value.Length > name.Length)
                {
                    var suffix = value.Substring(name.Length);

                    int subindex;
                    bool isNumeric = int.TryParse(suffix, out subindex);

                    if (isNumeric)
                    {
                        index = subindex + 1;
                    }
                }
            }

            string tempName = name + index;

            while (Emitter.LocalsNamesMap.ContainsValue(tempName) || Helpers.IsReservedWord(Emitter, tempName))
            {
                tempName = name + ++index;
            }

            return tempName;
        }

        public virtual Dictionary<IVariable, string> BuildLocalsMap()
        {
            var prevMap = Emitter.LocalsMap;

            if (prevMap == null)
            {
                Emitter.LocalsMap = new Dictionary<IVariable, string>();
            }
            else
            {
                Emitter.LocalsMap = new Dictionary<IVariable, string>(prevMap);
            }

            return prevMap;
        }

        public virtual void ClearLocalsMap(Dictionary<IVariable, string> prevMap = null)
        {
            Emitter.LocalsMap = prevMap;
        }

        public virtual Dictionary<string, string> BuildLocalsNamesMap()
        {
            var prevMap = Emitter.LocalsNamesMap;

            if (prevMap == null)
            {
                Emitter.LocalsNamesMap = new Dictionary<string, string>();
            }
            else
            {
                Emitter.LocalsNamesMap = new Dictionary<string, string>(prevMap);
            }

            return prevMap;
        }

        public virtual void ClearLocalsNamesMap(Dictionary<string, string> prevMap = null)
        {
            Emitter.LocalsNamesMap = prevMap;
        }

        public virtual void ConvertParamsToReferences(IEnumerable<ParameterDeclaration> declarations)
        {
            if (declarations.Any())
            {
                var p = declarations.First().Parent;
                if (p != null)
                {
                    if (Emitter.Resolver.ResolveNode(p) is MemberResolveResult rr)
                    {
                        if (rr.Member is DefaultResolvedMethod method)
                        {
                            var expandParams = method.Attributes.Any(a => a.AttributeType.FullName == "H5.ExpandParamsAttribute");
                            foreach (var prm in method.Parameters)
                            {
                                if (prm.IsOptional)
                                {
                                    var name = prm.Name;
                                    if (Helpers.IsReservedWord(Emitter, name))
                                    {
                                        name = Helpers.ChangeReservedWord(name);
                                    }

                                    Write(string.Format("if ({0} === void 0) {{ {0} = ", name));
                                    if (prm.ConstantValue == null && prm.Type.Kind == TypeKind.Struct && !prm.Type.IsKnownType(KnownTypeCode.NullableOfT))
                                    {
                                        Write(Inspector.GetStructDefaultValue(prm.Type, Emitter));
                                    }
                                    else if (prm.ConstantValue == null && prm.Type.Kind == TypeKind.TypeParameter)
                                    {
                                        Write(JS.Funcs.H5_GETDEFAULTVALUE + "(" + H5Types.ToJsName(prm.Type, Emitter) + ")");
                                    }
                                    else if (prm.Type.Kind == TypeKind.Enum)
                                    {
                                        var enumMode = Helpers.EnumEmitMode(prm.Type);

                                        if (enumMode >= 3 && enumMode < 7)
                                        {
                                            var members = prm.Type.GetMembers(options: GetMemberOptions.IgnoreInheritedMembers);
                                            var member = members.FirstOrDefault(m => m is IField field && field.ConstantValue == prm.ConstantValue);

                                            if (member != null)
                                            {
                                                string enumStringName = Emitter.GetEntityName(member);
                                                WriteScript(enumStringName);
                                            }
                                            else
                                            {
                                                WriteScript(prm.ConstantValue);
                                            }
                                        }
                                        else
                                        {
                                            WriteScript(prm.ConstantValue);
                                        }
                                    }
                                    else
                                    {
                                        WriteScript(prm.ConstantValue);
                                    }

                                    Write("; }");
                                    WriteNewLine();
                                }
                                else if (prm.IsParams)
                                {
                                    var name = prm.Name;
                                    if (Helpers.IsReservedWord(Emitter, name))
                                    {
                                        name = Helpers.ChangeReservedWord(name);
                                    }

                                    if (expandParams)
                                    {
                                        Write(string.Format("{0} = " + JS.Types.ARRAY + "." + JS.Fields.PROTOTYPE + "." + JS.Funcs.SLICE + "." + JS.Funcs.CALL + "(" + JS.Vars.ARGUMENTS + ", {1});", name, method.Parameters.IndexOf(prm) + method.TypeParameters.Count));
                                    }
                                    else
                                    {
                                        Write(string.Format("if ({0} === void 0) {{ {0} = []; }}", name));
                                    }

                                    WriteNewLine();
                                }
                            }
                        }
                    }
                }
            }

            declarations.ToList().ForEach(item =>
            {
                var lrr = item.Parent != null ? (LocalResolveResult)Emitter.Resolver.ResolveNode(item) : null;
                var isReferenceLocal = lrr != null && Emitter.LocalsMap.ContainsKey(lrr.Variable) && Emitter.LocalsMap[lrr.Variable].EndsWith(".v");

                if (item.Parent == null && item.Name == "value")
                {
                    var p = Emitter.LocalsMap.FirstOrDefault(pair => pair.Key.Name == "value");
                    if (p.Value != null && p.Value.EndsWith(".v"))
                    {
                        isReferenceLocal = true;
                    }
                }

                if (isReferenceLocal && !(item.ParameterModifier == ParameterModifier.Out || item.ParameterModifier == ParameterModifier.Ref))
                {
                    Write(string.Format("{0} = {{v:{0}}};", Emitter.LocalsNamesMap[item.Name]));
                    WriteNewLine();
                }
            });
        }

        protected virtual void IntroduceTempVar(string name)
        {
            Emitter.TempVariables[name] = true;

            if (Emitter.IsAsync && !Emitter.AsyncVariables.Contains(name))
            {
                Emitter.AsyncVariables.Add(name);
            }
        }

        public virtual void RemoveTempVar(string name)
        {
            Emitter.TempVariables[name] = false;
        }

        public virtual string GetTempVarName()
        {
            if (Emitter.TempVariables == null)
            {
                ResetLocals();
            }

            foreach (var pair in Emitter.TempVariables)
            {
                if (!pair.Value)
                {
                    Emitter.TempVariables[pair.Key] = true;
                    return pair.Key;
                }
            }

            string name = JS.Vars.T;
            int i = 0;

            while (Emitter.TempVariables.ContainsKey(name) || (Emitter.ParentTempVariables != null && Emitter.ParentTempVariables.ContainsKey(name)))
            {
                name = JS.Vars.T + ++i;
            }

            name = JS.Vars.T + (i > 0 ? i.ToString() : "");

            IntroduceTempVar(name);

            return name;
        }

        protected virtual void EmitTempVars(int pos, bool skipIndent = false)
        {
            if (Emitter.TempVariables.Count > 0)
            {
                var newLine = Emitter.IsNewLine;
                var temp = Emitter.Output.ToString(pos, Emitter.Output.Length - pos);
                Emitter.Output.Length = pos;

                Emitter.IsNewLine = true;
                Emitter.Comma = false;

                if (!skipIndent)
                {
                    Indent();
                    WriteIndent();
                }
                WriteVar(true);

                foreach (var localVar in Emitter.TempVariables)
                {
                    EnsureComma(false);
                    Write(localVar.Key);
                    Emitter.Comma = true;
                }

                Emitter.Comma = false;
                WriteSemiColon();
                Outdent();
                WriteNewLine();

                Emitter.Output.Append(temp);
                Emitter.IsNewLine = newLine;
            }
        }

        protected virtual void SimpleEmitTempVars(bool newline = true)
        {
            if (Emitter.TempVariables.Count > 0)
            {
                WriteVar(true);

                foreach (var localVar in Emitter.TempVariables)
                {
                    EnsureComma(false);
                    Write(localVar.Key);
                    Emitter.Comma = true;
                }

                Emitter.Comma = false;
                WriteSemiColon();
                if (newline)
                {
                    WriteNewLine();
                }
            }
        }
    }
}