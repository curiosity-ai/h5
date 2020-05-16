using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Newtonsoft.Json;
using Object.Net.Utilities;

namespace H5.Translator
{
    public class AnonymousTypeCreateBlock : AbstractObjectCreateBlock
    {
        public AnonymousTypeCreateBlock(IEmitter emitter, AnonymousTypeCreateExpression anonymousTypeCreateExpression, bool plain = false)
            : base(emitter, anonymousTypeCreateExpression)
        {
            Emitter = emitter;
            AnonymousTypeCreateExpression = anonymousTypeCreateExpression;
            Plain = plain;
        }

        public AnonymousTypeCreateExpression AnonymousTypeCreateExpression { get; set; }

        public bool Plain
        {
            get; private set;
        }

        protected override void DoEmit()
        {
            if (Plain || Emitter.Rules.AnonymousType == AnonymousTypeRule.Plain)
            {
                VisitPlainAnonymousTypeCreateExpression();
            }
            else
            {
                var rr = Emitter.Resolver.ResolveNode(AnonymousTypeCreateExpression.Parent);
                var member_rr = rr as MemberResolveResult;

                if (member_rr == null)
                {
                    if (rr is OperatorResolveResult o_rr)
                    {
                        member_rr = o_rr.Operands[0] as MemberResolveResult;
                    }
                }

                if (member_rr != null && member_rr.Member.DeclaringTypeDefinition != null &&
                    Emitter.Validator.IsObjectLiteral(member_rr.Member.DeclaringTypeDefinition))
                {
                    VisitPlainAnonymousTypeCreateExpression();
                }
                else
                {
                    VisitAnonymousTypeCreateExpression();
                }
            }
        }

        protected void VisitPlainAnonymousTypeCreateExpression()
        {
            AnonymousTypeCreateExpression anonymousTypeCreateExpression = AnonymousTypeCreateExpression;

            WriteOpenBrace();
            WriteSpace();

            if (anonymousTypeCreateExpression.Initializers.Count > 0)
            {
                WriteObjectInitializer(anonymousTypeCreateExpression.Initializers, false);

                WriteSpace();
                WriteCloseBrace();
            }
            else
            {
                WriteCloseBrace();
            }
        }

        protected void VisitAnonymousTypeCreateExpression()
        {
            AnonymousTypeCreateExpression anonymousTypeCreateExpression = AnonymousTypeCreateExpression;
            var invocationrr = Emitter.Resolver.ResolveNode(anonymousTypeCreateExpression) as InvocationResolveResult;
            var type = invocationrr.Type as AnonymousType;
            IAnonymousTypeConfig config = null;

            if (!Emitter.AnonymousTypes.ContainsKey(type))
            {
                config = CreateAnonymousType(type);
                Emitter.AnonymousTypes.Add(type, config);
            }
            else
            {
                config = Emitter.AnonymousTypes[type];
            }

            WriteNew();
            Write(config.Name);
            WriteOpenParentheses();

            if (anonymousTypeCreateExpression.Initializers.Count > 0)
            {
                WriteObjectInitializer(anonymousTypeCreateExpression.Initializers, true, true);
            }

            WriteCloseParentheses();
        }

        public virtual IAnonymousTypeConfig CreateAnonymousType(AnonymousType type)
        {
            var config = new AnonymousTypeConfig();
            config.Name = JS.Types.H5_ANONYMOUS + (Emitter.AnonymousTypes.Count + 1);
            config.Type = type;

            var oldWriter = SaveWriter();
            NewWriter();

            Write(JS.Types.H5.DEFINE);
            WriteOpenParentheses();
            WriteScript(config.Name);
            config.Name = JS.Vars.ASM + "." + config.Name;
            Write(", " + JS.Vars.ASM + ", ");
            BeginBlock();
            Emitter.Comma = false;
            GenereateCtor(type);

            EnsureComma();
            Write(JS.Fields.METHODS);
            WriteColon();
            BeginBlock();

            GenereateEquals(config);
            GenerateHashCode(config);
            GenereateToJSON(config);

            WriteNewLine();
            EndBlock();

            GenereateMetadata(config);

            WriteNewLine();
            EndBlock();
            Write(");");
            config.Code = Emitter.Output.ToString();

            RestoreWriter(oldWriter);

            return config;
        }

        private void GenereateCtor(AnonymousType type)
        {
            EnsureComma();
            Write(JS.Fields.KIND);
            WriteColon();
            WriteScript(TypeKind.Anonymous.ToString().ToLowerInvariant());
            WriteComma(true);
            Write(JS.Fields.CTORS);
            WriteColon();
            BeginBlock();
            Write(JS.Funcs.CONSTRUCTOR + ": function (");
            foreach (var property in type.Properties)
            {
                EnsureComma(false);
                Write(property.Name.ToLowerCamelCase());
                Emitter.Comma = true;
            }
            Write(") ");
            Emitter.Comma = false;
            BeginBlock();

            foreach (var property in type.Properties)
            {
                var name = Emitter.GetEntityName(property);

                Write(string.Format("this.{0} = {1};", name, property.Name.ToLowerCamelCase()));
                WriteNewLine();
            }

            EndBlock();
            WriteNewLine();
            EndBlock();
            Emitter.Comma = true;
        }

        private void GenereateMetadata(IAnonymousTypeConfig config)
        {
            var meta = MetadataUtils.ConstructITypeMetadata(config.Type, Emitter);

            if (meta != null)
            {
                EnsureComma();
                Write("statics : ");
                BeginBlock();

                Write(JS.Fields.METHODS);
                WriteColon();
                BeginBlock();

                Write("$metadata : function () { return ");
                Write(meta.ToString(Formatting.None));
                Write("; }");
                WriteNewLine();
                EndBlock();
                WriteNewLine();
                EndBlock();
            }
        }

        private void GenereateEquals(IAnonymousTypeConfig config)
        {
            EnsureComma();
            Write(JS.Funcs.EQUALS + ": function (o) ");
            BeginBlock();

            Write("if (!" + JS.Types.H5.IS + "(o, ");
            Write(config.Name);
            Write(")) ");
            BeginBlock();
            Write("return false;");
            WriteNewLine();
            EndBlock();
            WriteNewLine();
            Write("return ");

            bool and = false;

            foreach (var property in config.Type.Properties)
            {
                var name = Emitter.GetEntityName(property);

                if (and)
                {
                    Write(" && ");
                }

                and = true;

                Write(JS.Funcs.H5_EQUALS + "(this.");
                Write(name);
                Write(", o.");
                Write(name);
                Write(")");
            }

            Write(";");

            WriteNewLine();
            EndBlock();
            Emitter.Comma = true;
        }

        private void GenerateHashCode(IAnonymousTypeConfig config)
        {
            EnsureComma();
            Write(JS.Funcs.GETHASHCODE + ": function () ");
            BeginBlock();
            Write("var h = " + JS.Funcs.H5_ADDHASH + "([");

            var nameHashValue = new HashHelper().GetDeterministicHash(config.Name);
            Write(nameHashValue);

            foreach (var property in config.Type.Properties)
            {
                var name = Emitter.GetEntityName(property);
                Write(", this." + name);
            }

            Write("]);");

            WriteNewLine();
            Write("return h;");
            WriteNewLine();
            EndBlock();
            Emitter.Comma = true;
        }

        private void GenereateToJSON(IAnonymousTypeConfig config)
        {
            EnsureComma();
            Write(JS.Funcs.TOJSON + ": function () ");
            BeginBlock();
            Write("return ");
            BeginBlock();

            foreach (var property in config.Type.Properties)
            {
                EnsureComma();
                var name = Emitter.GetEntityName(property);

                Write(string.Format("{0} : this.{0}", name));
                Emitter.Comma = true;
            }

            WriteNewLine();
            EndBlock();
            WriteSemiColon();
            WriteNewLine();
            EndBlock();
            Emitter.Comma = true;
        }
    }
}