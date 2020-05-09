using System;
using H5;
using static H5.Core.dom;

namespace Tesserae
{
    public static class Hotkeys
    {
        public static bool Shift { get { return Script.Write<bool>("hotkeys.shift == true"); } }
        public static bool Ctrl { get { return Script.Write<bool>("hotkeys.ctrl == true"); } }
        public static bool Alt { get { return Script.Write<bool>("hotkeys.alt == true"); } }
        public static bool OptionKey { get { return Script.Write<bool>("hotkeys.option == true"); } }
        public static bool Control { get { return Script.Write<bool>("hotkeys.control == true"); } }
        public static bool Cmd { get { return Script.Write<bool>("hotkeys.cmd == true"); } }
        public static bool Command { get { return Script.Write<bool>("hotkeys.command == true"); } }

        public static string GetScope()
        {
            return Script.Write<string>("hotkeys.getScope();");
        }

        public static void SetScope(string scope)
        {
            Script.Write("hotkeys.setScope({0});", scope);
        }

        public static void DeleteScope(string scope)
        {
            Script.Write("hotkeys.deleteScope({0});", scope);
        }

        public static void Bind(string keys, Option option, Action<Event, Handler> action)
        {
            Script.Write("hotkeys({0},{1},{2});", keys, option, action);
        }

        public static void Unbind(string keys, Option option, Action<Event, Handler> action)
        {
            Script.Write("hotkeys.unbind({0},{1},{2});", keys, option, action);
        }

        public static void BindGlobal(string keys, Action<Event, Handler> action)
        {
            Script.Write("hotkeys({0},{1});", keys, action);
        }

        public static void UnbindGlobal(string keys, Action<Event, Handler> action)
        {
            Script.Write("hotkeys.unbind({0},{1});", keys, action);
        }

        public static bool IsPressed(int key)
        {
            return Script.Write<bool>("hotkeys.isPressed({0});", key);
        }

        public static bool IsPressed(string key)
        {
            return Script.Write<bool>("hotkeys.isPressed({0});", key);
        }

        public static bool IsPressed(char key)
        {
            return Script.Write<bool>("hotkeys.isPressed({0});", key);
        }

        public static int[] GetPressedKeyCodes()
        {
            return Script.Write<int[]>("hotkeys.getPressedKeyCodes();");
        }

        public static void Filter(Func<Event, bool> onFilter)
        {
            Script.Write("hotkeys.filter = {0};", onFilter);
        }

        [ObjectLiteral]
        public class Handler
        {
            public string key;
            public string shortcut;
            public string scope;
            public bool keyup;
            public bool keydown;
            public int[] mods;
        }

        [ObjectLiteral]
        public class Option
        {
            public string scope;
            public HTMLElement element;
            public bool keyup;
            public bool keydown;
        }
    }
}