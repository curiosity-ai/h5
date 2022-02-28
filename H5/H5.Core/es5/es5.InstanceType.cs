// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using System.Linq.Expressions;

namespace H5.Core
{
    public  static partial class es5
    {
        [IgnoreGeneric(AllowInTypeScript = true)]
        [IgnoreCast]
        [Virtual]
        [Where("T", new string[] { "InstanceTypeCtorFnAlias" }, EnableImplicitConversion = true)]
        public abstract class InstanceType<T> : TypeAlias<Union<object, object>>
        {
            public static extern implicit operator es5.InstanceType<T>(Union<object, object> value);

            [Template("{0}")]
            public static extern es5.InstanceType<T> Create(object value);

            [Generated]
            [IgnoreGeneric(AllowInTypeScript = true)]
            public class InstanceTypeCtorFn<T> : CtorDelegate
            {
                [Template("({expr}.body.t[{expr}.body.constructor.sn] || {expr}.body.t)")]
                public extern InstanceTypeCtorFn(
                  Expression<es5.InstanceType<T>.InstanceTypeCtorFn<T>.InstanceTypeCtorFnDelegate> expr);

                [Template("new {this}({0})")]
                public extern object Invoke(params object[] args);

                [Generated]
                public delegate object InstanceTypeCtorFnDelegate(params object[] args);
            }
            [IgnoreGeneric(AllowInTypeScript = true)]
            [IgnoreCast]
            [Virtual]
            public abstract class InstanceTypeCtorFnAlias<T> : TypeAlias<es5.InstanceType<T>.InstanceTypeCtorFn<T>>
            {
                public static extern implicit operator InstanceTypeCtorFnAlias<T>(
                  es5.InstanceType<T>.InstanceTypeCtorFn<T> value);
            }
        }
    }
}
