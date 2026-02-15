#undef PARALLEL
namespace H5.Translator
{
    public static class CompilerBuiltInTypes
    {
        public static bool IsBuiltIn(string type)
        {
            switch(type)
            {
                case "System.Runtime.CompilerServices.IsReadOnlyAttribute":
                case "Microsoft.CodeAnalysis.EmbeddedAttribute":

                case "System.Runtime.CompilerServices.AsyncStateMachineAttribute":
                case "System.Runtime.CompilerServices.IteratorStateMachineAttribute":
                case "System.Runtime.CompilerServices.AsyncIteratorStateMachineAttribute":

                case "System.Runtime.CompilerServices.NullableAttribute":
                case "System.Runtime.CompilerServices.NullableContextAttribute":
                case "System.Runtime.CompilerServices.NullablePublicOnlyAttribute":

                case "System.Runtime.CompilerServices.DynamicAttribute":
                case "System.Runtime.CompilerServices.TupleElementNamesAttribute":

                case "System.Runtime.CompilerServices.CompilerGeneratedAttribute":
                case "System.Diagnostics.DebuggerHiddenAttribute":
                case "System.Diagnostics.DebuggerStepThroughAttribute":

                case "System.Runtime.CompilerServices.IsByRefLikeAttribute":
                case "System.Runtime.CompilerServices.RequiredMemberAttribute":
                case "System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute":
                case "System.Runtime.CompilerServices.InterpolatedStringHandlerAttribute":
                case "System.Runtime.CompilerServices.InterpolatedStringHandlerArgumentAttribute":
                case "System.Runtime.CompilerServices.UnsafeValueTypeAttribute":
                case "System.Runtime.CompilerServices.SkipLocalsInitAttribute":
                case "System.Runtime.CompilerServices.ScopedRefAttribute":
                case "System.Runtime.CompilerServices.IsExternalInit":
                case "System.Runtime.CompilerServices.ReferenceAssemblyAttribute":
                case "System.Runtime.CompilerServices.RuntimeCompatibilityAttribute":
                case "System.CodeDom.Compiler.GeneratedCodeAttribute":
                    return true;
            }
            return false;
        }
    }
}