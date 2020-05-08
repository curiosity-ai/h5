using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public class AggregateException : Exception
    {
        public extern AggregateException();

        [H5.Template("new System.AggregateException(null, {innerExceptions})")]
        public extern AggregateException(IEnumerable<Exception> innerExceptions);

        [H5.Template("new System.AggregateException(null, {innerExceptions:array})")]
        public extern AggregateException(params Exception[] innerExceptions);

        public extern AggregateException(string message);

        public extern AggregateException(string message, IEnumerable<Exception> innerExceptions);

        [H5.Template("new System.AggregateException({message}, {innerExceptions:array})")]
        public extern AggregateException(string message, params Exception[] innerExceptions);

        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<Exception> InnerExceptions
        {
            get;
        }

        public extern AggregateException Flatten();

        public extern void Handle(Func<Exception, bool> predicate);
    }
}