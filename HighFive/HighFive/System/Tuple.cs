namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.IgnoreCast]
    public sealed class Tuple<T1>
    {
        [HighFive.Template("{ Item1: {item1} }")]
        public extern Tuple(T1 item1);

        public extern T1 Item1
        {
            [HighFive.Template("Item1")]
            get;
        }

        [HighFive.Template("HighFive.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [HighFive.Template("HighFive.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.IgnoreCast]
    public sealed class Tuple<T1, T2>
    {
        [HighFive.Template("{ Item1: {item1}, Item2: {item2} }")]
        public extern Tuple(T1 item1, T2 item2);

        public extern T1 Item1
        {
            [HighFive.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [HighFive.Template("Item2")]
            get;
        }

        [HighFive.Template("HighFive.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [HighFive.Template("HighFive.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.IgnoreCast]
    public sealed class Tuple<T1, T2, T3>
    {
        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3);

        public extern T1 Item1
        {
            [HighFive.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [HighFive.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [HighFive.Template("Item3")]
            get;
        }

        [HighFive.Template("HighFive.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [HighFive.Template("HighFive.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4>
    {
        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4);

        public extern T1 Item1
        {
            [HighFive.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [HighFive.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [HighFive.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [HighFive.Template("Item4")]
            get;
        }

        [HighFive.Template("HighFive.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [HighFive.Template("HighFive.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4, T5>
    {
        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5);

        public extern T1 Item1
        {
            [HighFive.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [HighFive.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [HighFive.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [HighFive.Template("Item4")]
            get;
        }

        public extern T5 Item5
        {
            [HighFive.Template("Item5")]
            get;
        }

        [HighFive.Template("HighFive.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [HighFive.Template("HighFive.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4, T5, T6>
    {
        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6);

        public extern T1 Item1
        {
            [HighFive.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [HighFive.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [HighFive.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [HighFive.Template("Item4")]
            get;
        }

        public extern T5 Item5
        {
            [HighFive.Template("Item5")]
            get;
        }

        public extern T6 Item6
        {
            [HighFive.Template("Item6")]
            get;
        }

        [HighFive.Template("HighFive.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [HighFive.Template("HighFive.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7>
    {
        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6}, Item7: {item7} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7);

        public extern T1 Item1
        {
            [HighFive.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [HighFive.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [HighFive.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [HighFive.Template("Item4")]
            get;
        }

        public extern T5 Item5
        {
            [HighFive.Template("Item5")]
            get;
        }

        public extern T6 Item6
        {
            [HighFive.Template("Item6")]
            get;
        }

        public extern T7 Item7
        {
            [HighFive.Template("Item7")]
            get;
        }

        [HighFive.Template("HighFive.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [HighFive.Template("HighFive.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>
    {
        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6}, Item7: {item7}, rest: {rest} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest);

        public extern T1 Item1
        {
            [HighFive.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [HighFive.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [HighFive.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [HighFive.Template("Item4")]
            get;
        }

        public extern T5 Item5
        {
            [HighFive.Template("Item5")]
            get;
        }

        public extern T6 Item6
        {
            [HighFive.Template("Item6")]
            get;
        }

        public extern T7 Item7
        {
            [HighFive.Template("Item7")]
            get;
        }

        public extern TRest Rest
        {
            [HighFive.Template("rest")]
            get;
        }

        [HighFive.Template("HighFive.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [HighFive.Template("HighFive.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.IgnoreCast]
    public static class Tuple
    {
        [HighFive.Template("{ Item1: {item1} }")]
        public static extern Tuple<T1> Create<T1>(T1 item1);

        [HighFive.Template("{ Item1: {item1}, Item2: {item2} }")]
        public static extern Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2);

        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3} }")]
        public static extern Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3);

        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4} }")]
        public static extern Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4);

        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5} }")]
        public static extern Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5);

        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6} }")]
        public static extern Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6);

        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6}, Item7: {item7} }")]
        public static extern Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7);

        [HighFive.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6}, Item7: {item7}, rest: {rest} }")]
        public static extern Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Create<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest);
    }
}