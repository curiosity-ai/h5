using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public abstract class StringComparer : IComparer<string>, IEqualityComparer<string>
    {
        private static readonly StringComparer _ordinal = new OrdinalComparer(false);
        private static readonly StringComparer _ordinalIgnoreCase = new OrdinalComparer(true);

        public static StringComparer Ordinal
        {
            get
            {
                return _ordinal;
            }
        }

        public static StringComparer OrdinalIgnoreCase
        {
            get
            {
                return _ordinalIgnoreCase;
            }
        }

        public int Compare(object x, object y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            if (x is String sa)
            {
                if (y is String sb)
                {
                    return Compare(sa, sb);
                }
            }

            if (x is IComparable ia)
            {
                return ia.CompareTo(y);
            }

            throw new ArgumentException("At least one object must implement IComparable.");
        }


        public new bool Equals(Object x, Object y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;

            if (x is String sa)
            {
                if (y is String sb)
                {
                    return Equals(sa, sb);
                }
            }
            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (obj is string s)
            {
                return GetHashCode(s);
            }
            return obj.GetHashCode();
        }

        public abstract int Compare(String x, String y);
        public abstract bool Equals(String x, String y);
        public abstract int GetHashCode(string obj);
    }


    internal sealed class OrdinalComparer : StringComparer
    {
        private bool _ignoreCase;

        internal OrdinalComparer(bool ignoreCase)
        {
            _ignoreCase = ignoreCase;
        }

        public override int Compare(string x, string y)
        {
            if (Object.ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            if (_ignoreCase)
            {
                return String.Compare(x, y, StringComparison.OrdinalIgnoreCase);
            }

            return String.Compare(x, y, false);
        }

        public override bool Equals(string x, string y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;

            if (_ignoreCase)
            {
                if (x.Length != y.Length)
                {
                    return false;
                }
                return (String.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0);
            }
            return x.Equals(y);
        }

        public override int GetHashCode(string obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (_ignoreCase && obj != null)
            {
                return obj.ToLower().GetHashCode();
            }

            return obj.GetHashCode();
        }

        // Equals method for the comparer itself.
        public override bool Equals(Object obj)
        {
            if (!(obj is OrdinalComparer comparer))
            {
                return false;
            }
            return (this._ignoreCase == comparer._ignoreCase);
        }

        public override int GetHashCode()
        {
            string name = "OrdinalComparer";
            int hashCode = name.GetHashCode();
            return _ignoreCase ? (~hashCode) : hashCode;
        }
    }
}
