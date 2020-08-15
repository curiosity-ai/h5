//Translation library 'client' from https://github.com/pragmatrix/tnt, 
//   imported directly here as it is not possible to consume the TNT.T nuget package due how 
//   consumed NuGet packages must depend on H5 to work, and I don't think it's worth publishing 
//   an extra package for 20 lines of code 😅.
//   The method signatures bellow must match what is expected by TNT to work

namespace TNT
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class T
    {
        private static Dictionary<string, string> _currentTranslation = null;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string t(FormattableString formattableString)
        {
            var format = formattableString.Format;
            if (_currentTranslation is object && _currentTranslation.TryGetValue(format, out var translatedFormat))
            {
                format = translatedFormat;
            }
            return string.Format(format, formattableString.GetArguments());
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string t(this string originalString)
        {
            if (_currentTranslation is object && _currentTranslation.TryGetValue(originalString, out var translated))
            {
                return translated;
            }
            return originalString;
        }

        public static void SetTranslation(Dictionary<string, string> translation)
        {
            _currentTranslation = translation;
        }
    }
}


//MIT License
//
//Copyright(c) 2020 Armin Sander
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.