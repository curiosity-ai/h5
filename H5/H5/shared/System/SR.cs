// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
    internal static partial class SR
    {
        public const string ArgumentException_ValueTupleIncorrectType = "Argument must be of type {0}.";
        public const string ArgumentException_ValueTupleLastArgumentNotAValueTuple = "The last element of an eight element ValueTuple must be a ValueTuple.";

        private static ResourceManager ResourceManager
        {
            get;
            set;
        }

        // This method is used to decide if we need to append the exception message parameters to the message when calling SR.Format.
        // by default it returns false.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool UsingResourceKeys()
        {
            return false;
        }

        // Needed for debugger integration
        internal static string GetResourceString(string resourceKey)
        {
            return GetResourceString(resourceKey, null);
        }

        internal static string GetResourceString(string resourceKey, string defaultString)
        {
            string resourceString = null;
            try { resourceString = InternalGetResourceString(resourceKey); }
            catch (MissingManifestResourceException) { }

            if (defaultString != null && resourceKey.Equals(resourceString, StringComparison.Ordinal))
            {
                return defaultString;
            }

            return resourceString;
        }

        // TODO: Revised H5 [not required until InternalGetResourceString supported fully]
        //private static List<string> _currentlyLoading;
        //private static int _infinitelyRecursingCount;
        //private static bool _resourceManagerInited = false;

        private static string InternalGetResourceString(string key)
        {
            if (key == null || key.Length == 0)
            {
                Debug.Fail("SR::GetResourceString with null or empty key.  Bug in caller, or weird recursive loading problem?");
                return key;
            }

            return key;
        }

        internal static string Format(string resourceFormat, params object[] args)
        {
            if (args != null)
            {
                if (UsingResourceKeys())
                {
                    return resourceFormat + string.Join(", ", args);
                }

                return string.Format(resourceFormat, args);
            }

            return resourceFormat;
        }

        internal static string Format(string resourceFormat, object p1)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1);
            }

            return string.Format(resourceFormat, p1);
        }

        internal static string Format(string resourceFormat, object p1, object p2)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1, p2);
            }

            return string.Format(resourceFormat, p1, p2);
        }

        internal static string Format(string resourceFormat, object p1, object p2, object p3)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1, p2, p3);
            }
            return string.Format(resourceFormat, p1, p2, p3);
        }
    }
}
