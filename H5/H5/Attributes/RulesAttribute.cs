using System;

#if H5_COMPILER
namespace H5.Contract
#else
namespace H5
#endif
{
    /// <summary>
    /// Allow to control some aspects of generated code
    /// </summary>
#if H5_COMPILER
    public class CompilerRule
#else
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = false)]
    public class RulesAttribute : Attribute
#endif
    {
        public
#if  H5_COMPILER
            LambdaRule?
#else
            LambdaRule
#endif
        Lambda { get; set; }

        public
#if H5_COMPILER
            BoxingRule?
#else
            BoxingRule
#endif
        Boxing { get; set; }

        public
#if H5_COMPILER
            ArrayIndexRule?
#else
            ArrayIndexRule
#endif
        ArrayIndex { get; set; }

        public
#if H5_COMPILER
            IntegerRule?
#else
            IntegerRule
#endif
        Integer { get; set; }

        public
#if H5_COMPILER
            AnonymousTypeRule?
#else
            AnonymousTypeRule
#endif
        AnonymousType { get; set; }

        public
#if H5_COMPILER
            AutoPropertyRule?
#else
            AutoPropertyRule
#endif
        AutoProperty { get; set; }

        public
#if H5_COMPILER
            InlineCommentRule?
#else
            InlineCommentRule
#endif
        InlineComment { get; set; }

        public
#if H5_COMPILER
            ExternalCastRule?
#else
            ExternalCastRule
#endif
        ExternalCast { get; set; }

#if H5_COMPILER
        public CompilerRuleLevel Level { get; set; }
#endif
    }

#if !H5_COMPILER
    [NonScriptable]
#endif
    public enum LambdaRule
    {
        Managed = 0,
        Plain = 1
    }

#if !H5_COMPILER
    [NonScriptable]
#endif
    public enum BoxingRule
    {
        Managed = 0,
        Plain = 1
    }

#if !H5_COMPILER
    [NonScriptable]
#endif
    public enum ArrayIndexRule
    {
        Managed = 0,
        Plain = 1
    }

#if !H5_COMPILER
    [NonScriptable]
#endif
    public enum IntegerRule
    {
        Managed = 0,
        Plain = 1
    }

#if !H5_COMPILER
    [NonScriptable]
#endif
    public enum AnonymousTypeRule
    {
        Managed = 0,
        Plain = 1
    }

#if !H5_COMPILER
    [NonScriptable]
#endif
    public enum AutoPropertyRule
    {
        Managed = 0,
        Plain = 1
    }

#if !H5_COMPILER
    [NonScriptable]
#endif
    public enum InlineCommentRule
    {
        Managed = 0,
        Plain = 1
    }

#if !H5_COMPILER
    [NonScriptable]
#endif
    public enum ExternalCastRule
    {
        Managed = 0,
        Plain = 1
    }
}