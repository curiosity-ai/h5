using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public class AggregateException : Exception
    {
        public extern AggregateException();

        [HighFive.Template("new System.AggregateException(null, {innerExceptions})")]
        public extern AggregateException(IEnumerable<Exception> innerExceptions);

        [HighFive.Template("new System.AggregateException(null, {innerExceptions:array})")]
        public extern AggregateException(params Exception[] innerExceptions);

        public extern AggregateException(string message);

        public extern AggregateException(string message, IEnumerable<Exception> innerExceptions);

        [HighFive.Template("new System.AggregateException({message}, {innerExceptions:array})")]
        public extern AggregateException(string message, params Exception[] innerExceptions);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<Exception> InnerExceptions
        {
            get;
        }

        public extern AggregateException Flatten();

        public extern void Handle(Func<Exception, bool> predicate);
    }
}