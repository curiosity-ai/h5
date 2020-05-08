using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HighFive
{
    [External]
    [Name("HighFive")]
    public static class Module
    {
        [Template("HighFive.loadModule({type:module})")]
        public static extern Task Load(params Type[] type);
    }
}