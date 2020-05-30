// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/*============================================================
**
** Interface:  DictionaryEntry
**
**
**
**
** Purpose: Return Value for IDictionaryEnumerator::GetEntry
**
**
===========================================================*/
namespace System.Collections {

    using System;
    // A DictionaryEntry holds a key and a value from a dictionary.
    // It is returned by IDictionaryEnumerator::GetEntry().
[System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public struct DictionaryEntry
    {
        private object _key;
        private object _value;

        // Constructs a new DictionaryEnumerator by setting the Key
        // and Value fields appropriately.
        public DictionaryEntry(object key, object value) {
            _key = key;
            _value = value;
        }

        public object Key {
            get {
                return _key;
            }

            set {
                _key = value;
            }
        }

        public object Value {
            get {
                return _value;
            }

            set {
                _value = value;
            }
        }
    }
}
