using System;
using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class ArgumentsInfo
    {
        public IEmitter Emitter { get; private set; }

        public Expression Expression { get; private set; }

        public InvocationResolveResult ResolveResult { get; private set; }

        public OperatorResolveResult OperatorResolveResult { get; private set; }

        public Expression[] ArgumentsExpressions { get; set; }

        public string[] ArgumentsNames { get; set; }

        public Expression ParamsExpression { get; private set; }

        public NamedParamExpression[] NamedExpressions { get; set; }

        public TypeParamExpression[] TypeArguments { get; private set; }

        public object ThisArgument { get; set; }

        public string ThisName { get; set; }

        public bool IsExtensionMethod { get; set; }

        public bool HasTypeArguments { get; set; }

        public IMethod Method { get; private set; }

        public IAttribute Attribute { get; set; }

        public string[] StringArguments { get; set; }

        public IType ThisType { get; set; }

        public ArgumentsInfo(IEmitter emitter, IMethod method)
        {
            Emitter = emitter;
            Expression = null;
            Method = method;
            BuildTypedArguments(method);
        }

        public ArgumentsInfo(IEmitter emitter, IAttribute attr)
        {
            Emitter = emitter;
            Expression = null;
            Attribute = attr;
        }

        public ArgumentsInfo(IEmitter emitter, string[] args)
        {
            Emitter = emitter;
            Expression = null;
            StringArguments = args;
        }

        public ArgumentsInfo(IEmitter emitter, ConstructorInitializer initializer)
        {
            Emitter = emitter;
            Expression = null;

            var arguments = initializer.Arguments.ToList();
            ResolveResult = emitter.Resolver.ResolveNode(initializer) as InvocationResolveResult;

            BuildArgumentsList(arguments);
            if (ResolveResult != null)
            {
                HasTypeArguments = ((IMethod)ResolveResult.Member).TypeArguments.Count > 0;
            }
        }

        public ArgumentsInfo(IEmitter emitter, InvocationExpression invocationExpression, IMethod method = null)
        {
            Emitter = emitter;
            Expression = invocationExpression;

            var arguments = invocationExpression.Arguments.ToList();
            var rr = emitter.Resolver.ResolveNode(invocationExpression);
            ResolveResult = rr as InvocationResolveResult;

            if (ResolveResult == null && rr is DynamicInvocationResolveResult drr)
            {
                BuildDynamicArgumentsList(drr, arguments);
            }
            else
            {
                BuildArgumentsList(arguments);
            }

            if (ResolveResult != null)
            {
                HasTypeArguments = ((IMethod)ResolveResult.Member).TypeArguments.Count > 0;
                BuildTypedArguments(invocationExpression.Target);
            }

            if (method != null && method.Parameters.Count > 0)
            {
                ThisArgument = invocationExpression;
                var name = method.Parameters[0].Name;

                if (!ArgumentsNames.Contains(name))
                {
                    var list = ArgumentsNames.ToList();
                    list.Add(name);
                    ArgumentsNames = list.ToArray();

                    var expr = ArgumentsExpressions.ToList();
                    expr.Insert(0, invocationExpression);
                    ArgumentsExpressions = expr.ToArray();

                    var namedExpr = NamedExpressions.ToList();
                    namedExpr.Insert(0, new NamedParamExpression(name, invocationExpression));
                    NamedExpressions = namedExpr.ToArray();
                }
            }
        }

        public ArgumentsInfo(IEmitter emitter, IndexerExpression invocationExpression, InvocationResolveResult rr = null)
        {
            Emitter = emitter;
            Expression = invocationExpression;

            var arguments = invocationExpression.Arguments.ToList();
            ResolveResult = rr ?? emitter.Resolver.ResolveNode(invocationExpression) as InvocationResolveResult;

            BuildArgumentsList(arguments);
            if (ResolveResult != null)
            {
                BuildTypedArguments(ResolveResult.Member);
            }
        }

        public ArgumentsInfo(IEmitter emitter, Expression expression, InvocationResolveResult rr)
        {
            Emitter = emitter;
            Expression = expression;
            ResolveResult = rr;

            ArgumentsExpressions = new Expression[] { expression };
            ArgumentsNames = new string[] { rr.Member.Parameters.Count > 0 ? rr.Member.Parameters.First().Name : "{this}" };
            ThisArgument = expression;
            NamedExpressions = CreateNamedExpressions(ArgumentsNames, ArgumentsExpressions);

            BuildTypedArguments(rr.Member);
        }

        public ArgumentsInfo(IEmitter emitter, Expression expression, IMethod method)
        {
            Emitter = emitter;
            Expression = expression;

            ArgumentsExpressions = new Expression[] { expression };
            ArgumentsNames = new string[] {method.Parameters.Count > 0 ? method.Parameters.First().Name : "{this}" };
            ThisArgument = expression;
            NamedExpressions = CreateNamedExpressions(ArgumentsNames, ArgumentsExpressions);

            BuildTypedArguments(method);
        }

        public ArgumentsInfo(IEmitter emitter, Expression expression, ResolveResult rr = null)
        {
            Emitter = emitter;
            Expression = expression;

            ArgumentsExpressions = new Expression[] { expression };
            ArgumentsNames = new string[] { "{this}" };
            ThisArgument = expression;
            NamedExpressions = CreateNamedExpressions(ArgumentsNames, ArgumentsExpressions);

            if (rr is MemberResolveResult)
            {
                BuildTypedArguments(((MemberResolveResult)rr).Member);
            }
        }

        public ArgumentsInfo(IEmitter emitter, ObjectCreateExpression objectCreateExpression, IMethod method = null)
        {
            Emitter = emitter;
            Expression = objectCreateExpression;

            var arguments = objectCreateExpression.Arguments.ToList();
            var rr = emitter.Resolver.ResolveNode(objectCreateExpression);
            if (rr is DynamicInvocationResolveResult drr)
            {
                if (drr.Target is MethodGroupResolveResult group && group.Methods.Count() > 1)
                {
                    throw new EmitterException(objectCreateExpression, Constants.Messages.Exceptions.DYNAMIC_INVOCATION_TOO_MANY_OVERLOADS);
                }
            }

            ResolveResult = rr as InvocationResolveResult;
            BuildArgumentsList(arguments);
            BuildTypedArguments(objectCreateExpression.Type);

            if (method != null && method.Parameters.Count > 0)
            {
                ThisArgument = objectCreateExpression;
                var name = method.Parameters[0].Name;

                if (!ArgumentsNames.Contains(name))
                {
                    var list = ArgumentsNames.ToList();
                    list.Add(name);
                    ArgumentsNames = list.ToArray();

                    var expr = ArgumentsExpressions.ToList();
                    expr.Add(objectCreateExpression);
                    ArgumentsExpressions = expr.ToArray();

                    var namedExpr = NamedExpressions.ToList();
                    namedExpr.Add(new NamedParamExpression(name, objectCreateExpression));
                    NamedExpressions = namedExpr.ToArray();
                }
            }
        }

        public ArgumentsInfo(IEmitter emitter, AssignmentExpression assignmentExpression, OperatorResolveResult operatorResolveResult, IMethod method)
        {
            Emitter = emitter;
            Expression = assignmentExpression;
            OperatorResolveResult = operatorResolveResult;

            BuildOperatorArgumentsList(new Expression[] { assignmentExpression.Left, assignmentExpression.Right }, operatorResolveResult.UserDefinedOperatorMethod ?? method);
            BuildOperatorTypedArguments();
        }

        public ArgumentsInfo(IEmitter emitter, BinaryOperatorExpression binaryOperatorExpression, OperatorResolveResult operatorResolveResult, IMethod method)
        {
            Emitter = emitter;
            Expression = binaryOperatorExpression;
            OperatorResolveResult = operatorResolveResult;

            BuildOperatorArgumentsList(new Expression[] { binaryOperatorExpression.Left, binaryOperatorExpression.Right }, operatorResolveResult.UserDefinedOperatorMethod ?? method);
            BuildOperatorTypedArguments();
        }

        public ArgumentsInfo(IEmitter emitter, UnaryOperatorExpression unaryOperatorExpression, OperatorResolveResult operatorResolveResult, IMethod method)
        {
            Emitter = emitter;
            Expression = unaryOperatorExpression;
            OperatorResolveResult = operatorResolveResult;

            BuildOperatorArgumentsList(new Expression[] { unaryOperatorExpression.Expression }, operatorResolveResult.UserDefinedOperatorMethod ?? method);
            BuildOperatorTypedArguments();
        }

        private void BuildTypedArguments(AstType type)
        {
            if (type is SimpleType simpleType)
            {
                AstNodeCollection<AstType> typedArguments = simpleType.TypeArguments;
                IList<ITypeParameter> typeParams = null;

                if (ResolveResult.Member.DeclaringTypeDefinition != null)
                {
                    typeParams = ResolveResult.Member.DeclaringTypeDefinition.TypeParameters;
                }
                else if (ResolveResult.Member is SpecializedMethod)
                {
                    typeParams = ((SpecializedMethod)ResolveResult.Member).TypeParameters;
                }

                TypeArguments = new TypeParamExpression[typedArguments.Count];
                var list = typedArguments.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    TypeArguments[i] = new TypeParamExpression(typeParams[i].Name, list[i], null);
                }
            }
        }

        private void BuildTypedArguments(IMember member)
        {
            var typeParams = member.DeclaringTypeDefinition.TypeParameters;
            var typeArgs = member.DeclaringType.TypeArguments;
            var temp = new TypeParamExpression[typeParams.Count];

            for (int i = 0; i < typeParams.Count; i++)
            {
                temp[i] = new TypeParamExpression(typeParams[i].Name, null, typeArgs[i], true);
            }

            TypeArguments = temp;
        }

        private void BuildTypedArguments(Expression expression)
        {
            AstNodeCollection<AstType> typedArguments = null;

            if (expression is IdentifierExpression identifierExpression)
            {
                typedArguments = identifierExpression.TypeArguments;
            }
            else
            {
                if (expression is MemberReferenceExpression memberRefExpression)
                {
                    typedArguments = memberRefExpression.TypeArguments;
                }
            }


            if (ResolveResult.Member is IMethod method)
            {
                TypeArguments = new TypeParamExpression[method.TypeParameters.Count];

                if (typedArguments != null && typedArguments.Count == method.TypeParameters.Count)
                {
                    var list = typedArguments.ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        TypeArguments[i] = new TypeParamExpression(method.TypeParameters[i].Name, list[i], null);
                    }
                }
                else
                {
                    for (int i = 0; i < method.TypeArguments.Count; i++)
                    {
                        TypeArguments[i] = new TypeParamExpression(method.TypeParameters[i].Name, null, method.TypeArguments[i]);
                    }
                }

                if (method.DeclaringType != null && method.DeclaringTypeDefinition != null && method.DeclaringTypeDefinition.TypeParameters.Count > 0)
                {
                    var typeParams = method.DeclaringTypeDefinition.TypeParameters;
                    var typeArgs = method.DeclaringType.TypeArguments;
                    var temp = new TypeParamExpression[typeParams.Count];

                    for (int i = 0; i < typeParams.Count; i++)
                    {
                        temp[i] = new TypeParamExpression(typeParams[i].Name, null, typeArgs[i], true);
                    }

                    TypeArguments = TypeArguments.Concat(temp).ToArray();
                }
            }
        }

        private void BuildOperatorTypedArguments()
        {
            var method = OperatorResolveResult.UserDefinedOperatorMethod;

            if (method != null)
            {
                for (int i = 0; i < method.TypeArguments.Count; i++)
                {
                    TypeArguments[i] = new TypeParamExpression(method.TypeParameters[i].Name, null, method.TypeArguments[i]);
                }
            }
        }

        private void BuildDynamicArgumentsList(DynamicInvocationResolveResult drr, IList<Expression> arguments)
        {
            Expression paramsArg = null;
            string paramArgName = null;
            IMethod method = null;

            if (drr.Target is MethodGroupResolveResult group && group.Methods.Count() > 1)
            {
                method = group.Methods.FirstOrDefault(m =>
                {
                    if (drr.Arguments.Count != m.Parameters.Count)
                    {
                        return false;
                    }

                    for (int i = 0; i < m.Parameters.Count; i++)
                    {
                        var argType = drr.Arguments[i].Type;

                        if (argType.Kind == TypeKind.Dynamic)
                        {
                            argType = Emitter.Resolver.Compilation.FindType(TypeCode.Object);
                        }

                        if (!m.Parameters[i].Type.Equals(argType))
                        {
                            return false;
                        }
                    }

                    return true;
                });

                if (method == null)
                {
                    throw new EmitterException(Expression, Constants.Messages.Exceptions.DYNAMIC_INVOCATION_TOO_MANY_OVERLOADS);
                }
            }

            if (method != null)
            {
                var member = method;
                var parameters = method.Parameters;

                Expression[] result = new Expression[parameters.Count];
                string[] names = new string[result.Length];
                bool named = false;
                int i = 0;
                bool isInterfaceMember = false;

                if (member != null)
                {
                    var inlineStr = Emitter.GetInline(member);
                    named = !string.IsNullOrEmpty(inlineStr);

                    isInterfaceMember = member.DeclaringTypeDefinition != null &&
                                        member.DeclaringTypeDefinition.Kind == TypeKind.Interface;
                }

                foreach (var arg in arguments)
                {
                    if (arg is NamedArgumentExpression namedArg)
                    {
                        var namedParam = parameters.First(p => p.Name == namedArg.Name);
                        var index = parameters.IndexOf(namedParam);

                        result[index] = namedArg.Expression;
                        names[index] = namedArg.Name;
                        named = true;

                        if (paramsArg == null && (parameters.Count > i) && parameters[i].IsParams)
                        {
                            if (member.DeclaringTypeDefinition == null || !Emitter.Validator.IsExternalType(member.DeclaringTypeDefinition))
                            {
                                paramsArg = namedArg.Expression;
                            }

                            paramArgName = namedArg.Name;
                        }
                    }
                    else
                    {
                        if (paramsArg == null && (parameters.Count > i) && parameters[i].IsParams)
                        {
                            if (member.DeclaringTypeDefinition == null || !Emitter.Validator.IsExternalType(member.DeclaringTypeDefinition))
                            {
                                paramsArg = arg;
                            }

                            paramArgName = parameters[i].Name;
                        }

                        if (i >= result.Length)
                        {
                            var list = result.ToList();
                            list.AddRange(new Expression[arguments.Count - i]);

                            var strList = names.ToList();
                            strList.AddRange(new string[arguments.Count - i]);

                            result = list.ToArray();
                            names = strList.ToArray();
                        }

                        result[i] = arg;
                        names[i] = i < parameters.Count ? parameters[i].Name : paramArgName;
                    }

                    i++;
                }

                for (i = 0; i < result.Length; i++)
                {
                    if (result[i] == null)
                    {
                        var p = parameters[i];
                        object t = null;
                        if (p.Type.Kind == TypeKind.Enum)
                        {
                            t = Helpers.GetEnumValue(Emitter, p.Type, p.ConstantValue);
                        }
                        else
                        {
                            t = p.ConstantValue;
                        }
                        if ((named || isInterfaceMember) && !p.IsParams)
                        {
                            if (t == null)
                            {
                                result[i] = new PrimitiveExpression(new RawValue("void 0"));
                            }
                            else
                            {
                                result[i] = new PrimitiveExpression(t);
                            }
                        }

                        names[i] = parameters[i].Name;
                    }
                }

                ArgumentsExpressions = result;
                ArgumentsNames = names;
                ParamsExpression = paramsArg;
                NamedExpressions = CreateNamedExpressions(names, result);
            }
            else
            {
                ArgumentsExpressions = arguments.ToArray();
            }
        }

        private void BuildArgumentsList(IList<Expression> arguments)
        {
            Expression paramsArg = null;
            string paramArgName = null;
            var resolveResult = ResolveResult;

            if (resolveResult != null)
            {
                var parameters = resolveResult.Member.Parameters;
                var isDelegate = resolveResult.Member.DeclaringType.Kind == TypeKind.Delegate;
                int shift = 0;

                if (resolveResult.Member is IMethod resolvedMethod && resolveResult is CSharpInvocationResolveResult invocationResult &&
                    resolvedMethod.IsExtensionMethod && invocationResult.IsExtensionMethodInvocation)
                {
                    shift = 1;
                    ThisName = resolvedMethod.Parameters[0].Name;
                    IsExtensionMethod = true;
                }

                Expression[] result = new Expression[parameters.Count - shift];
                string[] names = new string[result.Length];
                bool named = false;
                int i = 0;
                bool isInterfaceMember = false;

                if (resolveResult.Member != null)
                {
                    var inlineStr = Emitter.GetInline(resolveResult.Member);
                    named = !string.IsNullOrEmpty(inlineStr);

                    isInterfaceMember = resolveResult.Member.DeclaringTypeDefinition != null &&
                                        resolveResult.Member.DeclaringTypeDefinition.Kind == TypeKind.Interface;
                }

                var expandParams = resolveResult.Member.Attributes.Any(a => a.AttributeType.FullName == "H5.ExpandParamsAttribute");

                foreach (var arg in arguments)
                {
                    if (arg is NamedArgumentExpression namedArg)
                    {
                        var namedParam = parameters.First(p => p.Name == namedArg.Name);
                        var index = parameters.IndexOf(namedParam) - shift;

                        if (index >= 0 && index < result.Length)
                        {
                            result[index] = namedArg.Expression;
                            names[index] = namedArg.Name;
                        }

                        named = true;

                        if (paramsArg == null && parameters.FirstOrDefault(p => p.Name == namedArg.Name).IsParams)
                        {
                            if (resolveResult.Member.DeclaringTypeDefinition == null || !Emitter.Validator.IsExternalType(resolveResult.Member.DeclaringTypeDefinition))
                            {
                                paramsArg = namedArg.Expression;
                            }

                            paramArgName = namedArg.Name;
                        }
                    }
                    else
                    {
                        if (paramsArg == null && (parameters.Count > (i + shift)) && parameters[i + shift].IsParams)
                        {
                            if (resolveResult.Member.DeclaringTypeDefinition == null || !Emitter.Validator.IsExternalType(resolveResult.Member.DeclaringTypeDefinition) || expandParams)
                            {
                                paramsArg = arg;
                            }

                            paramArgName = parameters[i + shift].Name;
                        }

                        if (i >= result.Length)
                        {
                            var list = result.ToList();
                            list.AddRange(new Expression[arguments.Count - i]);

                            var strList = names.ToList();
                            strList.AddRange(new string[arguments.Count - i]);

                            result = list.ToArray();
                            names = strList.ToArray();
                        }

                        result[i] = arg;
                        names[i] = (i + shift) < parameters.Count ? parameters[i + shift].Name : paramArgName;
                    }

                    i++;
                }

                for (i = 0; i < result.Length; i++)
                {
                    if (result[i] == null)
                    {
                        var p = parameters[i + shift];
                        object t = null;
                        if (p.Type.Kind == TypeKind.Enum)
                        {
                            t = Helpers.GetEnumValue(Emitter, p.Type, p.ConstantValue);
                        }
                        else
                        {
                            t = p.ConstantValue;
                        }
                        if ((named || isInterfaceMember || isDelegate) && !p.IsParams)
                        {
                            if (t == null)
                            {
                                result[i] = new PrimitiveExpression(new RawValue("void 0"));
                            }
                            else
                            {
                                result[i] = new PrimitiveExpression(t);
                            }
                        }

                        names[i] = parameters[i + shift].Name;
                    }
                }

                ArgumentsExpressions = result;
                ArgumentsNames = names;
                ParamsExpression = paramsArg;
                NamedExpressions = CreateNamedExpressions(names, result);
            }
            else
            {
                ArgumentsExpressions = arguments.ToArray();
            }
        }

        private void BuildOperatorArgumentsList(IList<Expression> arguments, IMethod method)
        {
            if (method != null)
            {
                var parameters = method.Parameters;

                Expression[] result = new Expression[parameters.Count];
                string[] names = new string[result.Length];

                int i = 0;
                foreach (var arg in arguments)
                {
                    if (arg is NamedArgumentExpression namedArg)
                    {
                        var namedParam = parameters.First(p => p.Name == namedArg.Name);
                        var index = parameters.IndexOf(namedParam);

                        result[index] = namedArg.Expression;
                        names[index] = namedArg.Name;
                    }
                    else
                    {
                        if (i >= result.Length)
                        {
                            var list = result.ToList();
                            list.AddRange(new Expression[arguments.Count - i]);

                            var strList = names.ToList();
                            strList.AddRange(new string[arguments.Count - i]);

                            result = list.ToArray();
                            names = strList.ToArray();
                        }

                        result[i] = arg;
                        names[i] = parameters[i].Name;
                    }

                    i++;
                }

                for (i = 0; i < result.Length; i++)
                {
                    if (result[i] == null)
                    {
                        var p = parameters[i];

                        if (p.Type.Kind == TypeKind.Enum)
                        {
                            result[i] = new PrimitiveExpression(Helpers.GetEnumValue(Emitter, p.Type, p.ConstantValue));
                        }
                        else
                        {
                            result[i] = new PrimitiveExpression(p.ConstantValue);
                        }
                        names[i] = parameters[i].Name;
                    }
                }

                ArgumentsExpressions = result;
                ArgumentsNames = names;
                NamedExpressions = CreateNamedExpressions(names, result);
            }
            else
            {
                ArgumentsExpressions = arguments.ToArray();
            }
        }

        public NamedParamExpression[] CreateNamedExpressions(string[] names, Expression[] expressions)
        {
            var result = new NamedParamExpression[names.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new NamedParamExpression(names[i], expressions[i]);
            }

            return result;
        }

        public string GetThisValue()
        {
            if (ThisArgument != null)
            {
                if (ThisArgument is string)
                {
                    return ThisArgument.ToString();
                }

                if (ThisArgument is Expression)
                {
                    ((Expression)ThisArgument).AcceptVisitor(Emitter);
                    return null;
                }
            }

            return "null";
        }

        public void AddExtensionParam()
        {
            var result = ArgumentsExpressions;
            var namedResult = NamedExpressions;
            var names = ArgumentsNames;

            if (IsExtensionMethod)
            {
                var list = result.ToList();
                list.Insert(0, null);

                var strList = names.ToList();
                strList.Insert(0, null);

                var namedList = namedResult.ToList();
                namedList.Insert(0, new NamedParamExpression(ThisName, null));

                result = list.ToArray();
                names = strList.ToArray();
                namedResult = namedList.ToArray();

                result[0] = null;
                names[0] = ThisName;
            }

            ArgumentsExpressions = result;
            ArgumentsNames = names;
            NamedExpressions = namedResult;
        }
    }

    public class NamedParamExpression
    {
        public NamedParamExpression(string name, Expression expression)
        {
            Name = name;
            Expression = expression;
        }

        public string Name { get; private set; }

        public Expression Expression { get; private set; }
    }

    public class TypeParamExpression
    {
        public TypeParamExpression(string name, AstType type, IType iType, bool inherited = false)
        {
            Name = name;
            AstType = type;
            IType = iType;
            Inherited = inherited;
        }

        public string Name { get; private set; }

        public AstType AstType { get; private set; }

        public IType IType { get; private set; }

        public bool Inherited { get; private set; }
    }
}