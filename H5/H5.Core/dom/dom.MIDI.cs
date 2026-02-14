using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class MIDIAccess : dom.EventTarget
        {
            public static dom.MIDIAccess prototype { get; set; }

            public virtual dom.MIDIInputMap inputs { get; }
            public virtual dom.MIDIOutputMap outputs { get; }
            public virtual bool sysexEnabled { get; }

            public virtual dom.MIDIAccess.onstatechangeFn onstatechange { get; set; }

            [Generated]
            public delegate void onstatechangeFn(dom.MIDIConnectionEvent ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class MIDIPort : dom.EventTarget
        {
            public static dom.MIDIPort prototype { get; set; }

            public virtual string id { get; }
            public virtual string manufacturer { get; }
            public virtual string name { get; }
            public virtual dom.MIDIPortType type { get; }
            public virtual string version { get; }
            public virtual dom.MIDIPortDeviceState state { get; }
            public virtual dom.MIDIPortConnectionState connection { get; }

            public virtual extern es5.Promise<dom.MIDIPort> open();
            public virtual extern es5.Promise<dom.MIDIPort> close();

            public virtual dom.MIDIPort.onstatechangeFn onstatechange { get; set; }

            [Generated]
            public delegate void onstatechangeFn(dom.MIDIConnectionEvent ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class MIDIInput : dom.MIDIPort
        {
            public static dom.MIDIInput prototype { get; set; }

            public virtual dom.MIDIInput.onmidimessageFn onmidimessage { get; set; }

            [Generated]
            public delegate void onmidimessageFn(dom.MIDIMessageEvent ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class MIDIOutput : dom.MIDIPort
        {
            public static dom.MIDIOutput prototype { get; set; }

            public virtual extern void send(es5.Uint8Array data);
            public virtual extern void send(es5.Uint8Array data, double timestamp);
            public virtual extern void send(byte[] data); // Helper overload
            public virtual extern void send(byte[] data, double timestamp);
            public virtual extern void clear();
        }

        [CombinedClass]
        [FormerInterface]
        public class MIDIMessageEvent : dom.Event
        {
            public extern MIDIMessageEvent(string type, dom.MIDIMessageEventInit eventInitDict);
            public static dom.MIDIMessageEvent prototype { get; set; }

            public virtual es5.Uint8Array data { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class MIDIConnectionEvent : dom.Event
        {
            public extern MIDIConnectionEvent(string type, dom.MIDIConnectionEventInit eventInitDict);
            public static dom.MIDIConnectionEvent prototype { get; set; }

            public virtual dom.MIDIPort port { get; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MIDIOptions : IObject
        {
            public bool? sysex { get; set; }
            public bool? software { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MIDIMessageEventInit : dom.EventInit
        {
            public es5.Uint8Array data { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MIDIConnectionEventInit : dom.EventInit
        {
            public dom.MIDIPort port { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class MIDIInputMap : es5.Map<string, dom.MIDIInput>
        {
            public static dom.MIDIInputMap prototype { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class MIDIOutputMap : es5.Map<string, dom.MIDIOutput>
        {
            public static dom.MIDIOutputMap prototype { get; set; }
        }

        [Name("System.String")]
        public class MIDIPortType : LiteralType<string>
        {
            [Template("<self>\"input\"")]
            public static readonly dom.MIDIPortType input;
            [Template("<self>\"output\"")]
            public static readonly dom.MIDIPortType output;
            private extern MIDIPortType();
            public static extern implicit operator dom.MIDIPortType(string value);
        }

        [Name("System.String")]
        public class MIDIPortDeviceState : LiteralType<string>
        {
            [Template("<self>\"disconnected\"")]
            public static readonly dom.MIDIPortDeviceState disconnected;
            [Template("<self>\"connected\"")]
            public static readonly dom.MIDIPortDeviceState connected;
            private extern MIDIPortDeviceState();
            public static extern implicit operator dom.MIDIPortDeviceState(string value);
        }

        [Name("System.String")]
        public class MIDIPortConnectionState : LiteralType<string>
        {
            [Template("<self>\"open\"")]
            public static readonly dom.MIDIPortConnectionState open;
            [Template("<self>\"closed\"")]
            public static readonly dom.MIDIPortConnectionState closed;
            [Template("<self>\"pending\"")]
            public static readonly dom.MIDIPortConnectionState pending;
            private extern MIDIPortConnectionState();
            public static extern implicit operator dom.MIDIPortConnectionState(string value);
        }
    }
}
