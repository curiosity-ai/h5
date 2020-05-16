using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System.Linq;

namespace H5.Translator
{
    public class ArrayCreateBlock : ConversionBlock
    {
        public ArrayCreateBlock(IEmitter emitter, ArrayCreateExpression arrayCreateExpression)
            : base(emitter, arrayCreateExpression)
        {
            Emitter = emitter;
            ArrayCreateExpression = arrayCreateExpression;
        }

        public ArrayCreateBlock(IEmitter emitter, ArrayCreateExpression arrayCreateExpression, ArrayCreateResolveResult arrayCreateResolveResult)
            : base(emitter, null)
        {
            Emitter = emitter;
            ArrayCreateExpression = arrayCreateExpression;
            ArrayCreateResolveResult = arrayCreateResolveResult;
        }

        public ArrayCreateExpression ArrayCreateExpression { get; set; }

        public ArrayCreateResolveResult ArrayCreateResolveResult { get; set; }

        protected override Expression GetExpression()
        {
            return ArrayCreateExpression;
        }

        protected override void EmitConversionExpression()
        {
            VisitArrayCreateExpression();
        }

        protected void VisitArrayCreateExpression()
        {
            ArrayCreateExpression arrayCreateExpression = ArrayCreateExpression;
            var rr = ArrayCreateResolveResult ?? (Emitter.Resolver.ResolveNode(arrayCreateExpression) as ArrayCreateResolveResult);
            var at = (ArrayType)rr.Type;
            var rank = arrayCreateExpression.Arguments.Count;

            if (arrayCreateExpression.Initializer.IsNull && rank == 1)
            {
                string typedArrayName = null;
                if (Emitter.AssemblyInfo.UseTypedArrays && (typedArrayName = Helpers.GetTypedArrayName(at.ElementType)) != null)
                {
                    Write(JS.Types.System.Array.INIT);
                    WriteOpenParentheses();

                    Write("new ", typedArrayName, "(");
                    if (ArrayCreateResolveResult != null)
                    {
                        AttributeCreateBlock.WriteResolveResult(ArrayCreateResolveResult.SizeArguments.First(), this);
                    }
                    else
                    {
                        arrayCreateExpression.Arguments.First().AcceptVisitor(Emitter);
                    }
                    Write(")");
                    Write(", ");
                    Write(H5Types.ToJsName(at.ElementType, Emitter));
                    Write(")");
                }
                else
                {
                    Write(JS.Types.System.Array.INIT);
                    WriteOpenParentheses();

                    if (ArrayCreateResolveResult != null)
                    {
                        AttributeCreateBlock.WriteResolveResult(ArrayCreateResolveResult.SizeArguments.First(), this);
                    }
                    else
                    {
                        arrayCreateExpression.Arguments.First().AcceptVisitor(Emitter);
                    }
                    WriteComma();

                    var def = Inspector.GetDefaultFieldValue(at.ElementType, arrayCreateExpression.Type);
                    if (def == at.ElementType || def is RawValue)
                    {
                        WriteFunction();
                        WriteOpenCloseParentheses();
                        BeginBlock();
                        WriteReturn(true);
                        if (def is RawValue)
                        {
                            Write(def.ToString());
                        }
                        else
                        {
                            Write(Inspector.GetStructDefaultValue(at.ElementType, Emitter));
                        }

                        WriteSemiColon();
                        WriteNewLine();
                        EndBlock();
                    }
                    else
                    {
                        WriteScript(def);
                    }

                    Write(", ");
                    Write(H5Types.ToJsName(at.ElementType, Emitter));

                    Write(")");
                }
                return;
            }

            if (at.Dimensions > 1)
            {
                Write(JS.Types.System.Array.CREATE);
                WriteOpenParentheses();

                var def = Inspector.GetDefaultFieldValue(at.ElementType, arrayCreateExpression.Type);
                var defaultInitializer = new PrimitiveExpression(def, "?");

                if (def == at.ElementType || def is RawValue)
                {
                    WriteFunction();
                    WriteOpenCloseParentheses();
                    BeginBlock();
                    WriteReturn(true);
                    if (def is RawValue)
                    {
                        Write(def.ToString());
                    }
                    else
                    {
                        Write(Inspector.GetStructDefaultValue(at.ElementType, Emitter));
                    }

                    WriteSemiColon();
                    WriteNewLine();
                    EndBlock();
                }
                else if (defaultInitializer.Value is IType)
                {
                    Write(Inspector.GetStructDefaultValue((IType) defaultInitializer.Value, Emitter));
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
            else
            {
                Write(JS.Types.System.Array.INIT);
                WriteOpenParentheses();
            }

            if (rr.InitializerElements != null && rr.InitializerElements.Count > 0)
            {
                string typedArrayName = null;
                bool isTyped = Emitter.AssemblyInfo.UseTypedArrays && (typedArrayName = Helpers.GetTypedArrayName(at.ElementType)) != null;
                if (isTyped)
                {
                    Write("new ", typedArrayName, "(");
                }

                WriteOpenBracket();

                if (ArrayCreateResolveResult != null)
                {
                    bool needComma = false;
                    foreach (ResolveResult item in ArrayCreateResolveResult.InitializerElements)
                    {
                        if (needComma)
                        {
                            WriteComma();
                        }

                        needComma = true;

                        AttributeCreateBlock.WriteResolveResult(item, this);
                    }
                }
                else
                {
                    var elements = arrayCreateExpression.Initializer.Elements;
                    new ExpressionListBlock(Emitter, elements, null, null, 0).Emit();
                }

                WriteCloseBracket();

                if (isTyped)
                {
                    Write(")");
                }
            }
            else if (at.Dimensions > 1)
            {
                Write("null");
            }
            else
            {
                Write("[]");
            }

            Write(", ");
            Write(H5Types.ToJsName(at.ElementType, Emitter));

            if (at.Dimensions > 1)
            {
                Emitter.Comma = true;

                for (int i = 0; i < rr.SizeArguments.Count; i++)
                {
                    var a = rr.SizeArguments[i];
                    EnsureComma(false);

                    if (a.IsCompileTimeConstant)
                    {
                        this.Write(a.ConstantValue);
                    }
                    else if (ArrayCreateResolveResult != null)
                    {
                        AttributeCreateBlock.WriteResolveResult(ArrayCreateResolveResult.SizeArguments[i], this);
                    }
                    else if (arrayCreateExpression.Arguments.Count > i)
                    {
                        var arg = arrayCreateExpression.Arguments.ElementAt(i);

                        if (arg != null)
                        {
                            arg.AcceptVisitor(Emitter);
                        }
                    }
                    Emitter.Comma = true;
                }
            }

            Write(")");
            Emitter.Comma = false;
        }
    }
}