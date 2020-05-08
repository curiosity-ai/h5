namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.IgnoreCast]
    public sealed class Tuple<T1>
    {
        [H5.Template("{ Item1: {item1} }")]
        public extern Tuple(T1 item1);

        public extern T1 Item1
        {
            [H5.Template("Item1")]
            get;
        }

        [H5.Template("H5.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [H5.Template("H5.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.IgnoreCast]
    public sealed class Tuple<T1, T2>
    {
        [H5.Template("{ Item1: {item1}, Item2: {item2} }")]
        public extern Tuple(T1 item1, T2 item2);

        public extern T1 Item1
        {
            [H5.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [H5.Template("Item2")]
            get;
        }

        [H5.Template("H5.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [H5.Template("H5.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.IgnoreCast]
    public sealed class Tuple<T1, T2, T3>
    {
        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3);

        public extern T1 Item1
        {
            [H5.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [H5.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [H5.Template("Item3")]
            get;
        }

        [H5.Template("H5.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [H5.Template("H5.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4>
    {
        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4);

        public extern T1 Item1
        {
            [H5.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [H5.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [H5.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [H5.Template("Item4")]
            get;
        }

        [H5.Template("H5.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [H5.Template("H5.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4, T5>
    {
        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5);

        public extern T1 Item1
        {
            [H5.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [H5.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [H5.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [H5.Template("Item4")]
            get;
        }

        public extern T5 Item5
        {
            [H5.Template("Item5")]
            get;
        }

        [H5.Template("H5.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [H5.Template("H5.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4, T5, T6>
    {
        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6);

        public extern T1 Item1
        {
            [H5.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [H5.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [H5.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [H5.Template("Item4")]
            get;
        }

        public extern T5 Item5
        {
            [H5.Template("Item5")]
            get;
        }

        public extern T6 Item6
        {
            [H5.Template("Item6")]
            get;
        }

        [H5.Template("H5.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [H5.Template("H5.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7>
    {
        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6}, Item7: {item7} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7);

        public extern T1 Item1
        {
            [H5.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [H5.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [H5.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [H5.Template("Item4")]
            get;
        }

        public extern T5 Item5
        {
            [H5.Template("Item5")]
            get;
        }

        public extern T6 Item6
        {
            [H5.Template("Item6")]
            get;
        }

        public extern T7 Item7
        {
            [H5.Template("Item7")]
            get;
        }

        [H5.Template("H5.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [H5.Template("H5.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.IgnoreCast]
    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>
    {
        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6}, Item7: {item7}, rest: {rest} }")]
        public extern Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest);

        public extern T1 Item1
        {
            [H5.Template("Item1")]
            get;
        }

        public extern T2 Item2
        {
            [H5.Template("Item2")]
            get;
        }

        public extern T3 Item3
        {
            [H5.Template("Item3")]
            get;
        }

        public extern T4 Item4
        {
            [H5.Template("Item4")]
            get;
        }

        public extern T5 Item5
        {
            [H5.Template("Item5")]
            get;
        }

        public extern T6 Item6
        {
            [H5.Template("Item6")]
            get;
        }

        public extern T7 Item7
        {
            [H5.Template("Item7")]
            get;
        }

        public extern TRest Rest
        {
            [H5.Template("rest")]
            get;
        }

        [H5.Template("H5.objectEquals({this}, {o}, true)")]
        public override extern bool Equals(object o);

        [H5.Template("H5.getHashCode({this}, false, true)")]
        public override extern int GetHashCode();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.IgnoreCast]
    public static class Tuple
    {
        [H5.Template("{ Item1: {item1} }")]
        public static extern Tuple<T1> Create<T1>(T1 item1);

        [H5.Template("{ Item1: {item1}, Item2: {item2} }")]
        public static extern Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2);

        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3} }")]
        public static extern Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3);

        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4} }")]
        public static extern Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4);

        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5} }")]
        public static extern Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5);

        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6} }")]
        public static extern Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6);

        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6}, Item7: {item7} }")]
        public static extern Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7);

        [H5.Template("{ Item1: {item1}, Item2: {item2}, Item3: {item3}, Item4: {item4}, Item5: {item5}, Item6: {item6}, Item7: {item7}, rest: {rest} }")]
        public static extern Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Create<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest);
    }
}