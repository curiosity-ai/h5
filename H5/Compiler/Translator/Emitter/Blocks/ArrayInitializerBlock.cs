using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using System.Linq;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class ArrayInitializerBlock : AbstractEmitterBlock
    {
        public ArrayInitializerBlock(IEmitter emitter, ArrayInitializerExpression arrayInitializerExpression)
            : base(emitter, arrayInitializerExpression)
        {
            Emitter = emitter;
            ArrayInitializerExpression = arrayInitializerExpression;
        }

        public ArrayInitializerExpression ArrayInitializerExpression { get; set; }

        protected override void DoEmit()
        {
            var elements = ArrayInitializerExpression.Elements;
            var first = elements.Count > 0 ? elements.First() : null;

            var isObjectInitializer = first is NamedExpression || first is NamedArgumentExpression;
            var rr = Emitter.Resolver.ResolveNode(ArrayInitializerExpression) as ArrayCreateResolveResult;
            var at = rr != null ? (ArrayType)rr.Type : null;
            var create = at != null && at.Dimensions > 1;

            if (rr != null) { }
            if (!isObjectInitializer || ArrayInitializerExpression.IsSingleElement)
            {
                if (at != null)
                {
                    Write(create ? JS.Types.System.Array.CREATE : JS.Types.System.Array.INIT);
                    WriteOpenParentheses();
                }

                if (create)
                {
                    var defaultInitializer = new PrimitiveExpression(Inspector.GetDefaultFieldValue(at.ElementType, AstType.Null), "?");

                    if (defaultInitializer.Value is IType)
                    {
                        Write(Inspector.GetStructDefaultValue((IType)defaultInitializer.Value, Emitter));
                    }
                    else if (defaultInitializer.Value is RawValue)
                    {
                        Write(defaultInitializer.Value.ToString());
                    }
                    else
                    {
                        defaultInitializer.AcceptVisitor(Emitter);
                    }

                    WriteComma();
                }

                Write("[");
            }
            else
            {
                BeginBlock();
            }

            new ExpressionListBlock(Emitter, elements, null, null, 0, elements.Count > 2).Emit();

            if (!isObjectInitializer || ArrayInitializerExpression.IsSingleElement)
            {
                Write("]");
                if (at != null)
                {
                    Write(", ");
                    Write(H5Types.ToJsName(at.ElementType, Emitter));

                    if (create)
                    {
                        Emitter.Comma = true;

                        for (int i = 0; i < rr.SizeArguments.Count; i++)
                        {
                            var a = rr.SizeArguments[i];
                            EnsureComma(false);

                            if (a.IsCompileTimeConstant)
                            {
                                Write(a.ConstantValue);
                            }
                            else
                            {
                                AttributeCreateBlock.WriteResolveResult(rr.SizeArguments[i], this);
                            }
                            Emitter.Comma = true;
                        }
                    }

                    Write(")");
                }
            }
            else
            {
                WriteNewLine();
                EndBlock();
            }
        }
    }
}