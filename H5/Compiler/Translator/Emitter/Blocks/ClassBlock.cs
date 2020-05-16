using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Newtonsoft.Json;
using Object.Net.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace H5.Translator
{
    public class ClassBlock : AbstractEmitterBlock
    {
        public ClassBlock(IEmitter emitter, ITypeInfo typeInfo)
            : base(emitter, typeInfo.TypeDeclaration)
        {
            TypeInfo = typeInfo;
        }

        public ITypeInfo TypeInfo { get; set; }

        public bool IsGeneric { get; set; }

        public int StartPosition { get; set; }

        public bool HasEntryPoint { get; set; }

        protected override void DoEmit()
        {
            XmlToJsDoc.EmitComment(this, Emitter.Translator.EmitNode);
            string globalTarget = H5Types.GetGlobalTarget(TypeInfo.Type.GetDefinition(), TypeInfo.TypeDeclaration);

            if (globalTarget != null)
            {
                CheckGlobalClass();
                Emitter.NamedFunctions = new Dictionary<string, string>();
                WriteTopInitMethods();

                Write(JS.Types.H5.APPLY);
                WriteOpenParentheses();
                Write(globalTarget);
                Write(", ");
                BeginBlock();

                new MethodBlock(Emitter, TypeInfo, true).Emit();
                EmitMetadata();

                WriteNewLine();
                EndBlock();
                WriteCloseParentheses();
                WriteSemiColon();

                EmitAnonymousTypes();
                EmitNamedFunctions();

                WriteAfterInitMethods();

                WriteNewLine();
            }
            else
            {
                EmitClassHeader();

                Emitter.NamedFunctions = new Dictionary<string, string>();

                if (TypeInfo.TypeDeclaration.ClassType != ClassType.Interface)
                {
                    MethodDeclaration entryPoint = null;
                    if (TypeInfo.StaticMethods.Any(group =>
                    {
                        return group.Value.Any(method =>
                        {
                            var result = Helpers.IsEntryPointMethod(Emitter, method);
                            if (result)
                            {
                                entryPoint = method;
                            }
                            return result;
                        });
                    }))
                    {
                        if (!entryPoint.Body.IsNull)
                        {
                            Emitter.VisitMethodDeclaration(entryPoint);
                        }
                    }

                    EmitStaticBlock();
                    EmitInstantiableBlock();
                }

                EmitClassEnd();
            }
        }

        protected void CheckGlobalClass()
        {
            var type = TypeInfo.Type.GetDefinition();
            if (!type.IsStatic)
            {
                throw new EmitterException(TypeInfo.TypeDeclaration, string.Format("The type {0} must be static in order to be decorated with a [MixinAttribute] or [GlobalMethodsAttribute]", TypeInfo.Type.FullName));
            }

            if (type.TypeParameters.Count > 0)
            {
                throw new EmitterException(TypeInfo.TypeDeclaration, string.Format("[MixinAttribute] or [GlobalMethodsAttribute] cannot be applied to the generic type {0}.", TypeInfo.Type.FullName));
            }

            if (type.Members.Any(m => !m.IsStatic && m.SymbolKind != SymbolKind.Method))
            {
                throw new EmitterException(TypeInfo.TypeDeclaration, string.Format("The type {0} can contain only methods in order to be decorated with a [MixinAttribute] or [GlobalMethodsAttribute]", TypeInfo.Type.FullName));
            }
        }

        protected virtual void EmitClassHeader()
        {
            WriteTopInitMethods();

            var typeDef = Emitter.GetTypeDefinition();
            string name = Emitter.Validator.GetCustomTypeName(typeDef, Emitter, false);
            IsGeneric = typeDef.GenericParameters.Count > 0 && !Helpers.IsIgnoreGeneric(TypeInfo.Type, Emitter);

            if (name.IsEmpty())
            {
                name = H5Types.ToJsName(TypeInfo.Type, Emitter, asDefinition: true, nomodule: true, ignoreLiteralName: false);
            }

            if (typeDef.IsInterface && typeDef.HasGenericParameters)
            {
                Write(JS.Types.H5.DEFINE_I);
            }
            else
            {
                Write(JS.Types.H5.DEFINE);
            }

            WriteOpenParentheses();

            WriteScript(name);
            StartPosition = Emitter.Output.Length;
            Write(", ");

            if (IsGeneric)
            {
                if (TypeInfo.Module != null)
                {
                    Write(TypeInfo.Module.Name);
                    Write(", ");
                }

                WriteFunction();
                WriteOpenParentheses();

                foreach (var p in typeDef.GenericParameters)
                {
                    if (typeDef.GenericParameters.Count(gp => gp.FullName == p.FullName) > 1)
                    {
                        throw new EmitterException(TypeInfo.TypeDeclaration, $"Type parameter '{p.FullName}' has the same name as the type parameter from outer type.");
                    }
                    EnsureComma(false);
                    Write(p.Name);
                    Emitter.Comma = true;
                }
                Emitter.Comma = false;
                WriteCloseParentheses();

                Write(" { return ");
            }

            BeginBlock();

            string extend = Emitter.GetTypeHierarchy();

            if (extend.IsNotEmpty() && !TypeInfo.IsEnum)
            {
                var h5Type = Emitter.H5Types.Get(Emitter.TypeInfo);

                if (TypeInfo.InstanceMethods.Any(m => m.Value.Any(subm => Emitter.GetEntityName(subm) == JS.Fields.INHERITS)) ||
                    TypeInfo.InstanceConfig.Fields.Any(m => m.GetName(Emitter) == JS.Fields.INHERITS))
                {
                    Write(JS.Vars.D);
                }

                Write(JS.Fields.INHERITS);
                WriteColon();
                if (Helpers.IsTypeArgInSubclass(h5Type.TypeDefinition, h5Type.TypeDefinition, Emitter, false))
                {
                    WriteFunction();
                    WriteOpenCloseParentheses(true);
                    WriteOpenBrace(true);
                    WriteReturn(true);
                    Write(extend);
                    WriteSemiColon();
                    WriteCloseBrace(true);
                }
                else
                {
                    Write(extend);
                }

                Emitter.Comma = true;
            }

            WriteKind();
            EmitMetadata();
            WriteObjectLiteral();

            if (TypeInfo.Module != null)
            {
                WriteScope();
                WriteModule();
            }

            WriteVariance();
        }

        protected virtual void EmitMetadata()
        {
            if ((Emitter.HasModules || Emitter.AssemblyInfo.Reflection.Target == MetadataTarget.Type) && Emitter.ReflectableTypes.Any(t => t == Emitter.TypeInfo.Type))
            {
                var meta = MetadataUtils.ConstructTypeMetadata(TypeInfo.Type.GetDefinition(), Emitter, false, TypeInfo.TypeDeclaration.GetParent<SyntaxTree>());

                if (meta != null)
                {
                    EnsureComma();
                    Write("$metadata : function () { return ");
                    Write(meta.ToString(Formatting.None));
                    Write("; }");
                    Emitter.Comma = true;
                }
            }
        }

        private void WriteTopInitMethods()
        {
            var beforeDefineMethods = GetBeforeDefineMethods();

            if (beforeDefineMethods.Any())
            {
                foreach (var method in beforeDefineMethods)
                {
                    if (!Emitter.IsNewLine)
                    {
                        WriteNewLine();
                    }

                    Write(method);
                }

                WriteNewLine();
                WriteNewLine();
            }

            var topDefineMethods = GetTopDefineMethods();

            if (topDefineMethods.Any())
            {
                foreach (var method in topDefineMethods)
                {
                    Emitter.EmitterOutput.TopOutput.Append(method);
                }
            }
        }

        protected virtual void WriteVariance()
        {
            var itypeDef = TypeInfo.Type.GetDefinition();
            if (itypeDef.Kind == TypeKind.Interface && MetadataUtils.IsJsGeneric(itypeDef, Emitter) &&
                itypeDef.TypeParameters != null &&
                itypeDef.TypeParameters.Any(typeParameter => typeParameter.Variance != VarianceModifier.Invariant))
            {
                EnsureComma();
                Write(JS.Fields.VARIANCE);
                WriteColon();
                WriteScript(
                    itypeDef.TypeParameters.Select(typeParameter => ClassBlock.ConvertVarianceToInt(typeParameter.Variance))
                        .ToArray());
                Emitter.Comma = true;
            }
        }

        private static int ConvertVarianceToInt(VarianceModifier variance)
        {
            switch (variance)
            {
                case VarianceModifier.Covariant:
                    return 1;

                case VarianceModifier.Contravariant:
                    return 2;

                default:
                    return 0;
            }
        }

        protected virtual void WriteScope()
        {
            EnsureComma();
            Write(JS.Vars.SCOPE);
            WriteColon();
            Write(TypeInfo.Module.Name);
            Emitter.Comma = true;
        }

        protected virtual void WriteModule()
        {
            EnsureComma();
            Write(JS.Vars.MODULE);
            WriteColon();
            WriteScript(TypeInfo.Module.Name);
            Emitter.Comma = true;
        }

        protected virtual void WriteKind()
        {
            var isNested = TypeInfo.Type.DeclaringType != null;
            if (TypeInfo.Type.Kind == TypeKind.Class && !isNested)
            {
                return;
            }

            EnsureComma();
            Write(JS.Fields.KIND);
            WriteColon();
            WriteScript( (isNested ? "nested " : "") + TypeInfo.Type.Kind.ToString().ToLowerInvariant());
            Emitter.Comma = true;
        }

        protected virtual void WriteObjectLiteral()
        {
            if (TypeInfo.IsObjectLiteral)
            {
                EnsureComma();
                Write(JS.Fields.LITERAL);
                WriteColon();
                WriteScript(true);
                Emitter.Comma = true;
            }
        }

        protected virtual void EmitStaticBlock()
        {
            int pos = Emitter.Output.Length;
            bool comma = Emitter.Comma;
            bool newLine = Emitter.IsNewLine;

            Emitter.StaticBlock = true;
            EnsureComma();

            if (TypeInfo.InstanceMethods.Any(m => m.Value.Any(subm => Emitter.GetEntityName(subm) == JS.Fields.STATICS)) ||
                TypeInfo.InstanceConfig.Fields.Any(m => m.GetName(Emitter) == JS.Fields.STATICS))
            {
                Write(JS.Vars.D);
            }

            Write(JS.Fields.STATICS);
            WriteColon();
            BeginBlock();
            int checkOutputPos = Emitter.Output.Length;

            var ctorBlock = new ConstructorBlock(Emitter, TypeInfo, true);
            ctorBlock.Emit();
            HasEntryPoint = ctorBlock.HasEntryPoint;

            new MethodBlock(Emitter, TypeInfo, true).Emit();
            var clear = checkOutputPos == Emitter.Output.Length;
            WriteNewLine();
            EndBlock();

            if (clear)
            {
                Emitter.Output.Length = pos;
                Emitter.Comma = comma;
                Emitter.IsNewLine = newLine;
            }
            else
            {
                Emitter.Comma = true;
            }

            Emitter.StaticBlock = false;
        }

        protected virtual void EmitInstantiableBlock()
        {
            if (TypeInfo.IsEnum)
            {
                if (Emitter.GetTypeDefinition(TypeInfo.Type)
                        .CustomAttributes.Any(attr => attr.AttributeType.FullName == "System.FlagsAttribute"))
                {
                    EnsureComma();
                    Write(JS.Fields.FLAGS + ": true");
                    Emitter.Comma = true;
                }

                var etype = TypeInfo.Type.GetDefinition().EnumUnderlyingType;
                var enumMode = Helpers.EnumEmitMode(TypeInfo.Type);
                var isString = enumMode >= 3 && enumMode <= 6;
                if (isString)
                {
                    etype = Emitter.Resolver.Compilation.FindType(KnownTypeCode.String);
                }
                if (!etype.IsKnownType(KnownTypeCode.Int32))
                {
                    EnsureComma();
                    Write(JS.Fields.UNDERLYINGTYPE + ": ");
                    Write(H5Types.ToJsName(etype, Emitter));

                    Emitter.Comma = true;
                }
            }

            if (HasEntryPoint)
            {
                EnsureComma();
                Write(JS.Fields.ENTRY_POINT + ": true");
                Emitter.Comma = true;
            }

            var ctorBlock = new ConstructorBlock(Emitter, TypeInfo, false);

            if (TypeInfo.HasRealInstantiable(Emitter) || Emitter.Plugins.HasConstructorInjectors(ctorBlock) || TypeInfo.ClassType == ClassType.Struct)
            {
                ctorBlock.Emit();
                new MethodBlock(Emitter, TypeInfo, false).Emit();
            }
        }

        protected virtual void EmitClassEnd()
        {
            WriteNewLine();
            EndBlock();

            var classStr = Emitter.Output.ToString().Substring(StartPosition);

            if (Regex.IsMatch(classStr, "^\\s*,\\s*\\{\\s*\\}\\s*$", RegexOptions.Multiline))
            {
                Emitter.Output.Remove(StartPosition, Emitter.Output.Length - StartPosition);
            }

            if (IsGeneric)
            {
                Write("; }");
            }

            WriteCloseParentheses();
            WriteSemiColon();

            EmitAnonymousTypes();
            EmitNamedFunctions();

            WriteAfterInitMethods();

            WriteNewLine();
        }

        private void WriteAfterInitMethods()
        {
            var afterDefineMethods = GetAfterDefineMethods();

            if (afterDefineMethods.Any())
            {
                WriteNewLine();
            }

            foreach (var method in afterDefineMethods)
            {
                WriteNewLine();
                Write(method);
            }

            var bottomDefineMethods = GetBottomDefineMethods();

            if (bottomDefineMethods.Any())
            {
                foreach (var method in bottomDefineMethods)
                {
                    Emitter.EmitterOutput.BottomOutput.Append(method);
                }
            }
        }

        protected virtual void EmitAnonymousTypes()
        {
            var types = Emitter.AnonymousTypes.Values.Where(t => !t.Emitted).ToArray();
            if (types.Any())
            {
                Emitter.Comma = false;
                foreach (IAnonymousTypeConfig type in types)
                {
                    WriteNewLine();
                    WriteNewLine();

                    type.Emitted = true;
                    Write(type.Code);
                }
            }
        }

        protected virtual void EmitNamedFunctions()
        {
            if (Emitter.NamedFunctions.Count > 0)
            {
                Emitter.Comma = false;

                var name = H5Types.ToJsName(Emitter.TypeInfo.Type, Emitter, true);

                WriteNewLine();
                WriteNewLine();
                Write(JS.Funcs.H5_NS);
                WriteOpenParentheses();
                WriteScript(name);
                Write(", " + JS.Vars.D_ + ")");
                WriteSemiColon();

                WriteNewLine();
                WriteNewLine();
                Write(JS.Types.H5.APPLY + "(" + JS.Vars.D_ + ".");
                Write(name);
                Write(", ");
                BeginBlock();

                foreach (KeyValuePair<string, string> namedFunction in Emitter.NamedFunctions)
                {
                    EnsureComma();
                    Write(namedFunction.Key + ": " + namedFunction.Value);
                    Emitter.Comma = true;
                }

                WriteNewLine();
                EndBlock();
                WriteCloseParentheses();
                WriteSemiColon();
            }
        }

        protected virtual IEnumerable<string> GetDefineMethods(InitPosition value, Func<MethodDeclaration, IMethod, string> fn)
        {
            var methods = TypeInfo.InstanceMethods;
            var attrName = "H5.InitAttribute";

            foreach (var methodGroup in methods)
            {
                foreach (var method in methodGroup.Value)
                {
                    foreach (var attrSection in method.Attributes)
                    {
                        foreach (var attr in attrSection.Attributes)
                        {
                            var rr = Emitter.Resolver.ResolveNode(attr.Type);
                            if (rr.Type.FullName == attrName)
                            {
                                throw new EmitterException(attr, "Instance method cannot be Init method");
                            }
                        }
                    }
                }
            }

            methods = TypeInfo.StaticMethods;
            List<string> list = new List<string>();

            foreach (var methodGroup in methods)
            {
                foreach (var method in methodGroup.Value)
                {
                    MemberResolveResult rrMember = null;
                    IMethod rrMethod = null;
                    foreach (var attrSection in method.Attributes)
                    {
                        foreach (var attr in attrSection.Attributes)
                        {
                            var rr = Emitter.Resolver.ResolveNode(attr.Type);
                            if (rr.Type.FullName == attrName)
                            {
                                InitPosition? initPosition = null;
                                if (attr.HasArgumentList)
                                {
                                    if (attr.Arguments.Count > 0)
                                    {
                                        var argExpr = attr.Arguments.First();
                                        var argrr = Emitter.Resolver.ResolveNode(argExpr);
                                        if (argrr.ConstantValue is int)
                                        {
                                            initPosition = (InitPosition)argrr.ConstantValue;
                                        }
                                    }
                                    else
                                    {
                                        initPosition = InitPosition.After;
                                    }
                                }
                                else
                                {
                                    initPosition = InitPosition.After;
                                }

                                if (initPosition == value)
                                {
                                    if (rrMember == null)
                                    {
                                        rrMember = Emitter.Resolver.ResolveNode(method) as MemberResolveResult;
                                        rrMethod = rrMember != null ? rrMember.Member as IMethod : null;
                                    }

                                    if (rrMethod != null)
                                    {
                                        if (rrMethod.TypeParameters.Count > 0)
                                        {
                                            throw new EmitterException(method, "Init method cannot be generic");
                                        }

                                        if (rrMethod.Parameters.Count > 0)
                                        {
                                            throw new EmitterException(method, "Init method should not have parameters");
                                        }

                                        if (rrMethod.ReturnType.Kind != TypeKind.Void
                                            && !rrMethod.ReturnType.IsKnownType(KnownTypeCode.Void))
                                        {
                                            throw new EmitterException(method, "Init method should not return anything");
                                        }

                                        list.Add(fn(method, rrMethod));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        protected virtual IEnumerable<string> GetBeforeDefineMethods()
        {
            return GetDefineMethods(InitPosition.Before,
                (method, rrMethod) =>
                {
                    var level = Emitter.Level;

                    PushWriter(JS.Types.H5.INIT + "(function () {0});");
                    ResetLocals();
                    var prevMap = BuildLocalsMap();
                    var prevNamesMap = BuildLocalsNamesMap();

                    Emitter.InitPosition = InitPosition.Before;
                    Emitter.ResetLevel();

                    method.Body.AcceptVisitor(Emitter);

                    Emitter.InitPosition = null;
                    Emitter.ResetLevel(level);

                    ClearLocalsMap(prevMap);
                    ClearLocalsNamesMap(prevNamesMap);

                    return PopWriter(true);
                });
        }

        protected virtual IEnumerable<string> GetTopDefineMethods()
        {
            return GetDefineMethods(InitPosition.Top,
                (method, rrMethod) =>
                {
                    var prevLevel = Emitter.Level;
                    var prevInitialLevel = Emitter.InitialLevel;

                    PushWriter("{0}");
                    ResetLocals();
                    var prevMap = BuildLocalsMap();
                    var prevNamesMap = BuildLocalsNamesMap();
                    Emitter.NoBraceBlock = method.Body;

                    Emitter.InitPosition = InitPosition.Top;
                    ((Emitter)Emitter).InitialLevel = Emitter.InitialLevel > 1 ? Emitter.InitialLevel - 1 : 0;
                    Emitter.ResetLevel();

                    method.Body.AcceptVisitor(Emitter);

                    Emitter.InitPosition = null;
                    ((Emitter)Emitter).InitialLevel = prevInitialLevel;
                    Emitter.ResetLevel(prevLevel);

                    ClearLocalsMap(prevMap);
                    ClearLocalsNamesMap(prevNamesMap);

                    return PopWriter(true);
                });
        }

        protected virtual IEnumerable<string> GetBottomDefineMethods()
        {
            return GetDefineMethods(InitPosition.Bottom,
                (method, rrMethod) =>
                {
                    var prevLevel = Emitter.Level;
                    var prevInitialLevel = Emitter.InitialLevel;

                    PushWriter("{0}");
                    ResetLocals();
                    var prevMap = BuildLocalsMap();
                    var prevNamesMap = BuildLocalsNamesMap();
                    Emitter.NoBraceBlock = method.Body;

                    Emitter.InitPosition = InitPosition.Bottom;
                    ((Emitter)Emitter).InitialLevel = Emitter.InitialLevel > 1 ? Emitter.InitialLevel - 1 : 0;
                    Emitter.ResetLevel();

                    method.Body.AcceptVisitor(Emitter);

                    Emitter.InitPosition = null;
                    ((Emitter)Emitter).InitialLevel = prevInitialLevel;
                    Emitter.ResetLevel(prevLevel);

                    ClearLocalsMap(prevMap);
                    ClearLocalsNamesMap(prevNamesMap);

                    return PopWriter(true);
                });
        }

        protected virtual IEnumerable<string> GetAfterDefineMethods()
        {
            return GetDefineMethods(InitPosition.After,
                (method, rrMethod) =>
                {
                    Emitter.InitPosition = InitPosition.After;

                    var callback = JS.Types.H5.INIT + "(function () { " + H5Types.ToJsName(rrMethod.DeclaringTypeDefinition, Emitter) + "." +
                           Emitter.GetEntityName(method) + "(); });";

                    Emitter.InitPosition = null;

                    return callback;
                });
        }
    }
}