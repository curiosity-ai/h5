// Copyright (c) 2010-2013 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
    public partial class GetClassTypeReference
    {
        public IType Resolve(ITypeResolveContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            IType type = null;

            if (assembly != null) 
            {
                IAssembly asm = assembly.Resolve(context);
                if (asm == null && assembly.ToString () == "System.Runtime")
                    asm = DefaultAssemblyReference.Corlib.Resolve (context);
                if (asm != null) {
                    type = asm.GetTypeDefinition(fullTypeName);
                }
            }

            if (type == null)
            {
                if (context.CurrentAssembly != null)
                {
                    type = context.CurrentAssembly.GetTypeDefinition(fullTypeName);
                }
                if (type == null)
                {
                    var compilation = context.Compilation;
                    foreach (var asm in compilation.Assemblies)
                    {
                        type = asm.GetTypeDefinition(fullTypeName);
                        if (type != null)
                            break;
                    }
                }
            }

            return type ?? new UnknownType(fullTypeName);
        }
    }
}
