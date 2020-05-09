// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Reflection
{
    // TODO: C#7
    //public readonly struct ParameterModifier
    public struct ParameterModifier
    {
        private readonly bool[] _byRef;

        public ParameterModifier(int parameterCount)
        {
            if (parameterCount <= 0)
                throw new ArgumentException("Must specify one or more parameters.");
            // TODO: SR
            //throw new ArgumentException(SR.Arg_ParmArraySize);

            _byRef = new bool[parameterCount];
        }

        public bool this[int index]
        {
            get
            {
                return _byRef[index];
            }
            set
            {
                _byRef[index] = value;
            }
        }

#if CORECLR
        internal bool[] IsByRefArray => _byRef;
#endif
    }
}