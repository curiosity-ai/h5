// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using System;

namespace H5.Core
{
    public static partial class es5
    {
        [CombinedClass]
        [StaticInterface("FunctionConstructor")]
        [FormerInterface]
        public class Function : IObject
        {
            [ExpandParams]
            public extern Function(params string[] args);

            [Name("prototype")]
            public static es5.Function prototype_Static
            {
                get;
            }

            [ExpandParams]
            public static extern es5.Function Self(params string[] args);

            [Template("({this}.$method || {this}).apply({thisArg})")]
            public virtual extern object apply(object thisArg);

            [Template("({this}.$method || {this}).apply({thisArg}, {argArray})")]
            public virtual extern object apply(object thisArg, object argArray);

            [Template("({this}.$method || {this}).call({thisArg}, {*argArray})")]
            [ExpandParams]
            public virtual extern object call(object thisArg, params object[] argArray);

            [Template("({this}.$method || {this}).bind({thisArg}, {*argArray})")]
            [ExpandParams]
            public virtual extern object bind(object thisArg, params object[] argArray);

            public virtual extern string toString();

            public virtual object prototype
            {
                get; set;
            }

            public virtual double length
            {
                get;
            }

            public virtual object arguments
            {
                get; set;
            }

            public virtual es5.Function caller
            {
                get; set;
            }

            public static extern implicit operator es5.Function(Delegate d);

            public static extern implicit operator Delegate(es5.Function f);
        }
    }
}
