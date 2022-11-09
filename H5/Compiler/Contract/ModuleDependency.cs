using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace H5.Contract
{
    public class ModuleDependency : IModuleDependency
    {
        public string DependencyName { get; set; }

        private string variableName;

        public string VariableName
        {
            // The logic here should be compatible with H5.Contract.Module.ExportAsNamespace.get() below.
            get
            {
                // Ensure a valid variable name is returned even if not specified.
                if (string.IsNullOrWhiteSpace(variableName))
                {
                    variableName = DependencyName;
                }

                // Even after trying to tie DependencyName, if still invalid,
                // then throw an exception.
                if (string.IsNullOrWhiteSpace(variableName))
                {
                    throw new System.FieldAccessException("Tried to access ModuleDependency name and variable before both variable and module name had a valid value.");
                }

                return variableName;
            }
            set
            {
                variableName = value;
            }
        }

        public ModuleType? Type{ get; set; }

        public bool PreventName{ get; set; }
    }

    public class Module
    {
        public Module(string name, ModuleType type, IEmitter emitter, bool preventModuleName = false)
        {
            Name = name;
            Type = type;
            PreventModuleName = preventModuleName;
            Emitter = emitter;
            InitName();
        }

        public Module(string name, IEmitter emitter, bool preventModuleName = false)
        {
            Name = name;
            Type = ModuleType.AMD;
            PreventModuleName = preventModuleName;
            Emitter = emitter;
            InitName();
        }

        public Module(bool preventModuleName, IEmitter emitter) : this(emitter)
        {
            PreventModuleName = preventModuleName;
        }

        public Module(IEmitter emitter) : this()
        {
            Emitter = emitter;
        }

        public Module()
        {
            Name = "";
            Type = ModuleType.AMD;
            PreventModuleName = false;
            InitName();
        }

        public static string EscapeName(string value)
        {
            return Regex.Replace(value, "[^\\w_\\d]", "_");
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                OriginalName = value;
                _name = EscapeName(value);
                NoName = false;
            }
        }

        public string OriginalName
        {
            get;
            private set;
        }

        public ModuleType Type { get; set; }

        public bool PreventModuleName
        {
            get;
            private set;
        }

        public IEmitter Emitter{ get; set; }

        public bool NoName
        {
            get;
            private set;
        }

        private string _exportAsNamespace;
        public string ExportAsNamespace
        {
            get
            {
                var currentTypeInfo = Emitter?.TypeInfo;

                if (currentTypeInfo != null && currentTypeInfo.Module != null && currentTypeInfo.Module.Equals(this))
                {
                    return Name;
                }

                return _exportAsNamespace ?? Name;
            }
            set
            {
                _exportAsNamespace = Regex.Replace(value, "[^\\w_\\d]", "_");
            }
        }

        private static int counter = 0;
        private void InitName()
        {
            if (Name == "" || Name == "---")
            {
                NoName = true;
                Name = "$module" + ++Module.counter;
            }
        }

        protected bool Equals(Module other)
        {
            if (other == null)
            {
                return false;
            }
            return string.Equals(Name, other.Name) && Type == other.Type && _exportAsNamespace == other._exportAsNamespace;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() != GetType() ? false : Equals((Module) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (int) Type;
            }
        }

        public static bool Equals(Module m1, Module m2)
        {
            if (m1 == null || m2 == null)
            {
                return false;
            }

            return m1.Equals(m2);
        }
    }

    public enum ModuleType
    {
        AMD,
        CommonJS,
        UMD,
        ES6
    }
}