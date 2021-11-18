namespace H5.Contract.Constants
{
    using System.Collections.Generic;

    public class JS
    {
        public class NS
        {
            public const string H5 = "H5";
        }

        public class Fields
        {
            public const string ENTRY_POINT = "$entryPoint";
            public const string MAIN = "$main";
            public const string KIND = "$kind";
            public const string LITERAL = "$literal";
            public const string VARIANCE = "$variance";
            public const string FLAGS = "$flags";
            public const string UNDERLYINGTYPE = "$utype";
            public const string ENUM = "$enum";
            public const string INHERITS = "inherits";
            public const string ENUMERABLE = "enumerable";
            public const string STRUCT = "$struct";
            public const string CONFIG = "config";
            public const string EVENTS = "events";
            public const string PROPERTIES = "props";
            public const string FIELDS = "fields";
            public const string METHODS = "methods";
            public const string STATICS = "statics";
            public const string CTORS = "ctors";
            public const string BOX = "box";

            public const string ASYNC_TASK = "task";
            public const string PROTOTYPE = "prototype";
            public const string CURRENT = "current";
        }

        public class Funcs
        {
            public const string H5_AUTO_STARTUP_METHOD_TEMPLATE = "H5.ready(this.{0});";
            public const string H5_BIND = "H5.fn.bind";
            public const string H5_CACHE_BIND = "H5.fn.cacheBind";
            public const string H5_BIND_SCOPE = "H5.fn.bindScope";
            public const string H5_CAST = "H5.cast";
            public const string H5_CREATEINSTANCE = "H5.createInstance";
            public const string H5_COMBINE = "H5.fn.combine";
            public const string H5_REMOVE = "H5.fn.remove";
            public const string H5_MERGE = "H5.merge";
            public const string H5_IS_DEFINED = "H5.isDefined";
            public const string H5_GET_ENUMERATOR = "H5.getEnumerator";
            public const string H5_GET_TYPE = "H5.getType";
            public const string H5_GET_I = "H5.geti";
            public const string H5_NS = "H5.ns";
            public const string H5_EQUALS = "H5.equals";
            public const string H5_GETHASHCODE = "H5.getHashCode";
            public const string H5_ADDHASH = "H5.addHash";
            public const string H5_REFERENCEEQUALS = "H5.referenceEquals";
            public const string SHORTEN_H5_REFERENCEEQUALS = "H5.rE"; //Was "H5.referenceEquals"
            public const string H5_REF = "H5.ref";
            public const string H5_GETDEFAULTVALUE = "H5." + GETDEFAULTVALUE;
            public const string H5_EVENT = "H5.event";
            public const string H5_PROPERTY = "H5.property";
            public const string H5_TOPLAIN = "H5.toPlain";
            public const string H5_HASVALUE = "H5.hasValue";
            public const string H5_LITERAL = "H5.literal";

            public const string INITIALIZE = "$initialize";
            public const string INIT = "init";
            public const string CLONE = "$clone";
            public const string MOVE_NEXT = "moveNext";
            public const string GET_CURRENT = "Current";
            public const string TOSTIRNG = "toString";
            public const string EQUALS = "equals";
            public const string GETHASHCODE = "getHashCode";
            public const string GETDEFAULTVALUE = "getDefaultValue";
            public const string STRING_FROMCHARCODE = "String.fromCharCode";
            public const string TOJSON = "toJSON";
            public const string GET_TYPE = "$getType";

            public const string ASYNC_BODY = "$asyncBody";
            public const string SHORTEN_ASYNC_BODY = "$ab";
            public const string ASYNC_YIELD_BODY = "moveNext";
            
            public const string GET_AWAITED_RESULT = "getAwaitedResult";
            public const string SHORTEN_GET_AWAITED_RESULT = "gAR";     //Was: getAwaitedResult
            public const string CONTINUE_WITH = "continue";
            public const string SHORTEN_CONTINUE_WITH = "c";            //Was: continue
            public const string IS_COMPLETED = "isCompleted";
            public const string SHORTEN_IS_COMPLETED = "isC";            //Was: isCompleted
            public const string SET_RESULT = "setResult";               
            public const string SHORTEN_SET_RESULT = "sR";              //Was: setResult
            public const string SET_EXCEPTION = "setException";         
            public const string SHORTEN_SET_EXCEPTION = "sE";           //Was: setException

            public const string CONSTRUCTOR = "ctor";
            public const string ENTRY_POINT_NAME = "main";
            public const string APPLY = "apply";
            public const string CALL = "call";
            public const string DEFINE = "define";
            public const string DISPOSE = "System$IDisposable$Dispose";

            public const string SLICE = "slice";

            public class Event
            {
                public const string ADD = "add";
                public const string REMOVE = "remove";
            }

            public class Property
            {
                public const string GET = "get";
                public const string SET = "set";
            }

            public class Math
            {
                public const string LIFT = "lift";
                public const string LIFT1 = "lift1";
                public const string LIFT2 = "lift2";

                public const string LIFTCMP = "liftcmp";
                public const string LIFTEQ = "lifteq";
                public const string LIFTNE = "liftne";
                public const string GT = "gt";
                public const string GTE = "gte";
                public const string EQUALS = "equals";
                public const string NE = "ne";
                public const string LT = "lt";
                public const string LTE = "lte";
                public const string ADD = "add";
                public const string SUB = "sub";
                public const string MUL = "mul";
                public const string UMUL = "umul";
                public const string DIV = "div";
                public const string TO_NUMBER_DIVIDED = "toNumberDivided";
                public const string MOD = "mod";
                public const string AND = "and";
                public const string OR = "or";
                public const string XOR = "xor";
                public const string SHL = "shl";
                public const string SHRU = "shru";
                public const string SHR = "shr";
                public const string BAND = "band";
                public const string BOR = "bor";
                public const string SL = "sl";
                public const string SRR = "srr";
                public const string SR = "sr";
                public const string DEC = "dec";
                public const string INC = "inc";
                public const string NEG = "neg";
                public const string EQ = "eq";
            }
        }

        public class Types
        {
            public const string SYSTEM_UInt64 = "System.UInt64";
            public const string SYSTEM_DECIMAL = "System.Decimal";
            public const string SYSTEM_NULLABLE = "System.Nullable";
            public const string TASK_COMPLETION_SOURCE = "System.Threading.Tasks.TaskCompletionSource";
            public const string SHORTEN_TASK_COMPLETION_SOURCE = "TCS"; //Was "System.Threading.Tasks.TaskCompletionSource"
            public const string H5_IH5Class = "H5.IH5Class";
            public const string H5_INT = "H5.Int";
            public const string H5_ANONYMOUS = "$AnonymousType$";

            public const string BOOLEAN = "Boolean";
            public const string ARRAY = "Array";
            public const string FUNCTION = "Function";
            public const string Uint8Array = "Uint8Array";
            public const string Int8Array = "Int8Array";
            public const string Int16Array = "Int16Array";
            public const string Uint16Array = "Uint16Array";
            public const string Int32Array = "Int32Array";
            public const string Uint32Array = "Uint32Array";
            public const string Float32Array = "Float32Array";
            public const string Float64Array = "Float64Array";
            public const string TypeRef = "H5.TypeRef";

            public class Number
            {
                public const string NaN = "NaN";
                public const string Infinity = "Infinity";
                public const string InfinityNegative = "-Infinity";
            }

            public class Object
            {
                public const string NAME = "Object";
                private const string DOTNAME = NAME + ".";

                public const string DEFINEPROPERTY = DOTNAME + "defineProperty";
            }

            public class System
            {
                private const string DOTNAME = "System.";

                public class Object
                {
                    public const string NAME = System.DOTNAME + "Object";
                }

                public class IDisposable
                {
                    private const string DOTNAME = NAME + ".";
                    public const string NAME = System.DOTNAME + "IDisposable";

                    public const string DISPOSE = "Dispose";
                    public const string INTERFACE_DISPOSE = "System$IDisposable$Dispose";
                }

                public class Array
                {
                    private const string DOTNAME = System.DOTNAME + "Array.";

                    public const string CREATE = DOTNAME + "create";
                    public const string INDEX = DOTNAME + "index";
                    public const string INIT = DOTNAME + "init";
                    public const string MIN = DOTNAME + "min";
                    public const string SHORTEN_MIN = "STEP"; // Was DOTNAME + "min"
                    public const string TYPE = DOTNAME + "type";
                    public const string TO_ENUMERATOR = DOTNAME + "toEnumerator";
                    public const string TO_ENUMERABLE = DOTNAME + "toEnumerable";
                }

                public class DateTime
                {
                    public const string NAME = System.DOTNAME + "DateTime";
                    private const string DOTNAME = NAME + ".";

                    public const string GET_DEFAULT_VALUE = DOTNAME + "getDefaultValue";

                }

                public class Exception
                {
                    public const string NAME = System.DOTNAME + "Exception";
                    private const string DOTNAME = NAME + ".";

                    public const string CREATE = DOTNAME + "create";
                }

                public class String
                {
                    private const string DOTNAME = System.DOTNAME + "String.";

                    public const string CONCAT = DOTNAME + "concat";
                }

                public class Int64
                {
                    public const string NAME = System.DOTNAME + "Int64";
                    private const string DOTNAME = NAME + ".";

                    public const string TONUMBER = DOTNAME + "toNumber";
                    public const string CHECK = DOTNAME + "check";
                }

                public class Reflection
                {
                    public const string NAME = System.DOTNAME + "Reflection";
                    private const string DOTNAME = NAME + ".";

                    public class Assembly
                    {
                        public const string NAME = Reflection.DOTNAME + "Assembly";
                        private const string DOTNAME = NAME + ".";

                        public class Config
                        {
                            public const string NAME = "name";
                            public const string VERSION = "version";
                            public const string COMPILER = "compiler";

                            public const string DEFAULT_VERSION = "";
                        }
                    }
                }
            }

            public class H5
            {
                private const string DOTNAME = NS.H5 + ".";

                public const string APPLY = DOTNAME + "apply";
                public const string COPY_PROPERTIES = DOTNAME + "copyProperties";
                public const string ASSEMBLY = DOTNAME + "assembly";
                public const string ASSEMBLYVERSION = DOTNAME + "assemblyVersion";
                public const string ENSURE_BASE_PROPERTY = DOTNAME + "ensureBaseProperty";
                public const string IS = DOTNAME + "is";
                public const string SET_METADATA = DOTNAME + "setMetadata";
                public const string GET_TYPE_ALIAS = DOTNAME + "getTypeAlias";
                public const string DEFINE = DOTNAME + "define";
                public const string DEFINE_I = DOTNAME + "definei";
                public const string GET_INTERFACE = DOTNAME + "virtual";
                public const string GET_CLASS = DOTNAME + "virtualc";
                public const string INIT = DOTNAME + "init";
                public const string BOX = DOTNAME + "box";
                public const string UNBOX = DOTNAME + "unbox";
                public const string CLONE = DOTNAME + "clone";

                public class Generator
                {
                    public const string NAME = H5.DOTNAME + "GeneratorEnumerator";
                    public const string NAME_GENERIC = NAME + "$1";
                    private const string DOTNAME = NAME + ".";
                    private const string DOTNAME_GENERIC = NAME_GENERIC + ".";
                }

                public class Global
                {
                    public const string NAME = H5.DOTNAME + "global";
                    public const string DOTNAME = NAME + ".";
                }

                public class Reflection
                {
                    public const string NAME = H5.DOTNAME + "Reflection";
                    private const string DOTNAME = NAME + ".";

                    public const string APPLYCONSTRUCTOR = DOTNAME + "applyConstructor";
                }
            }
        }

        public class Vars
        {
            public const string ASM = "$asm";

            public const char D = '$';
            public const string D_ = ASM + ".$";
            public const string DBOX_ = "$box_";
            public const string D_THIS = "$this";

            public const string T = "$t";
            public const string E = "$e";
            public const string V = "$v";
            public const string YIELD = "$yield";
            public const string ENUMERATOR = "$en"; // Was $enumerator
            public const string EXPORTS = "$exports";
            public const string SCOPE = "$scope";
            public const string MODULE = "$module";
            public const string ITERATOR = "$i";

            public const string ASYNC_TCS = "$tcs";             
            public const string ASYNC_TASK = "$t";              //Was $task
            public const string ASYNC_TASK_RESULT = "$tr";      //Was $taskResult
            public const string ASYNC_STEP = "$s";              //Was $step
            public const string ASYNC_E = "$ae";                //Was $async_e
            public const string ASYNC_E1 = "$ae1";              //Was $async_e1
            public const string ASYNC_JUMP = "$jff";            //Was $jumpFromFinally
            public const string ASYNC_RETURN_VALUE = "$rv";     //Was $returnValue

            public const string FIX_ARGUMENT_NAME = "__autofix__";
            public const string ARGUMENTS = "arguments";
        }

        public class Reserved
        {
            public static readonly List<string> StaticNames = new List<string> { "name", "arguments", "caller", "length", "prototype", "ctor" };
            public static readonly string[] Words = new string[]
            {
                "H5", "__proto__", "abstract", "arguments", "as", "boolean", "break", "byte", "case", "catch", "char",
                "class", "continue", "const", "constructor", "ctor", "debugger", "default", "delete", "do", "double", "document",
                "else", "enum", "eval", "export", "extends", "false", "final", "finally", "float", "for", "function",
                "goto", "if", "implements", "import", "in", "instanceof", "int", "interface", "let", "long", "namespace",
                "native", "new", "null", "package", "private", "protected", "public", "return", "short", "static", "super",
                "switch", "synchronized", "this", "throw", "throws", "transient", "true", "try", "typeof", "use", "var",
                "void", "volatile", "while", "window", "with", "yield", "Array", "Date", "eval", "hasOwnProperty", "Infinity",
                "isFinite", "isNaN", "isPrototypeOf", "Math", "NaN", "Number", "Object", "prototype", "String", "toString",
                "undefined", "valueOf"
            };
        }
    }
}