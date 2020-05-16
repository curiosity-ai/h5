using H5.Contract;
using System.Collections.Generic;

namespace H5.Translator
{
    public class JsDoc : IJsDoc
    {
        public JsDoc()
        {
            Init();
        }

        public List<string> Namespaces { get; set; }

        public void Init()
        {
            Namespaces = new List<string>();
            Callbacks = new List<string>();
        }

        public List<string> Callbacks { get; set; }
    }
}