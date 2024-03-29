// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/*=============================================================================
**
**
**
** Purpose: Exception class for Arthimatic Overflows.
**
**
=============================================================================*/

using System.Runtime.Serialization;

namespace System
{
    [Serializable]
    [System.Runtime.CompilerServices.TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
    public class OverflowException : ArithmeticException
    {
        public OverflowException()
            : base("Arithmetic operation resulted in an overflow.")
        // TODO: SR
        //: base(SR.Arg_OverflowException)
        {
            HResult = HResults.COR_E_OVERFLOW;
        }

        public OverflowException(string message)
            : base(message)
        {
            HResult = HResults.COR_E_OVERFLOW;
        }

        public OverflowException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = HResults.COR_E_OVERFLOW;
        }

        // TODO: NotSupported
        //protected OverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //}
    }
}
