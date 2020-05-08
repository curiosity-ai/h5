using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using System.Collections.Generic;

namespace HighFive.Contract
{
    public interface IPlugins
    {
        void OnConfigRead(IAssemblyInfo config);

        System.Collections.Generic.IEnumerable<string> GetConstructorInjectors(HighFive.Contract.IConstructorBlock constructorBlock);

        IInvocationInterceptor OnInvocation(IAbstractEmitterBlock block, InvocationExpression expression, InvocationResolveResult resolveResult);

        IReferenceInterceptor OnReference(IAbstractEmitterBlock block, MemberReferenceExpression expression, MemberResolveResult resolveResult);

        bool HasConstructorInjectors(HighFive.Contract.IConstructorBlock constructorBlock);

        System.Collections.Generic.IEnumerable<HighFive.Contract.IPlugin> Parts
        {
            get;
        }

        void BeforeEmit(HighFive.Contract.IEmitter emitter, HighFive.Contract.ITranslator translator);

        void AfterEmit(HighFive.Contract.IEmitter emitter, HighFive.Contract.ITranslator translator);

        void BeforeTypesEmit(IEmitter emitter, IList<ITypeInfo> types);

        void AfterTypesEmit(IEmitter emitter, IList<ITypeInfo> types);

        void BeforeTypeEmit(IEmitter emitter, ITypeInfo type);

        void AfterTypeEmit(IEmitter emitter, ITypeInfo type);

        void AfterOutput(HighFive.Contract.ITranslator translator, string outputPath, bool nocore);
    }
}