using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using System.Collections.Generic;

namespace H5.Contract
{
    public interface IPlugins
    {
        void OnConfigRead(IAssemblyInfo config);

        System.Collections.Generic.IEnumerable<string> GetConstructorInjectors(H5.Contract.IConstructorBlock constructorBlock);

        IInvocationInterceptor OnInvocation(IAbstractEmitterBlock block, InvocationExpression expression, InvocationResolveResult resolveResult);

        IReferenceInterceptor OnReference(IAbstractEmitterBlock block, MemberReferenceExpression expression, MemberResolveResult resolveResult);

        bool HasConstructorInjectors(H5.Contract.IConstructorBlock constructorBlock);

        System.Collections.Generic.IEnumerable<H5.Contract.IPlugin> Parts { get; }

        void BeforeEmit(H5.Contract.IEmitter emitter, H5.Contract.ITranslator translator);

        void AfterEmit(H5.Contract.IEmitter emitter, H5.Contract.ITranslator translator);

        void BeforeTypesEmit(IEmitter emitter, IList<ITypeInfo> types);

        void AfterTypesEmit(IEmitter emitter, IList<ITypeInfo> types);

        void BeforeTypeEmit(IEmitter emitter, ITypeInfo type);

        void AfterTypeEmit(IEmitter emitter, ITypeInfo type);

        void AfterOutput(H5.Contract.ITranslator translator, string outputPath);
    }
}