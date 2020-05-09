using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace H5
{
    [External]
    [Name("H5")]
    public static class Module
    {
        [Template("H5.loadModule({type:module})")]
        public static extern Task Load(params Type[] type);
    }
}