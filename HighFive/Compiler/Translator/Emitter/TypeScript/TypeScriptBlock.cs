using H5.Contract;
using H5.Contract.Constants;

using Mono.Cecil;
using Object.Net.Utilities;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator.TypeScript
{
    public class TypeScriptBlock : AbstractEmitterBlock
    {
        public TypeScriptBlock(IEmitter emitter, AstNode operatorDeclaration)
            : base(emitter, operatorDeclaration)
        {
        }

        public override int Level
        {
            get
            {
                return base.Level - this.Emitter.InitialLevel;
            }
        }

        protected override void DoEmit()
        {
        }
    }
}