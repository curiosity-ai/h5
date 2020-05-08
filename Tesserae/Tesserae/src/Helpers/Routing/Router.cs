using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HighFive;
using H5;
using Tesserae.Components;
using static H5.Core.dom;
using H5.Core;

namespace Tesserae
{
    public static class Router
    {
        public class State
        {
            public Parameters Parameters;
            public string RouteName;
            public string Path;
            public string FullPath;
        }

        internal class RoutePart
        {
            public string Path;
            public bool IsVariable;
            public string VariableName;

            public RoutePart(string path)
            {
                Path = path;
                IsVariable = path.StartsWith(":");
                VariableName = IsVariable ? path.TrimStart(':') : "";
            }

            public bool IsMatch(string pathPart, out string capturedVariable)
            {
                if (IsVariable)
                {
                    capturedVariable = pathPart;
                    return true;
                }
                else
                {
                    capturedVariable = null;
                    return string.Equals(pathPart, Path, StringComparison.InvariantCultureIgnoreCase);
                }
            }
        }

        internal class Route
        {
            private RoutePart[] Parts;

            public string Name { get; }
            public string Path { get; }
            private Action<Parameters> _action;

            public Route(string name, string path, Action<Parameters> action)
            {
                Name = name;
                Path = path;

                Parts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(p => new RoutePart(p)).ToArray();
                _action = action;
            }

            public bool IsMatch(string[] parts, Dictionary<string, string> parameters)
            {
                if (parts.Length == Parts.Length)
                {
                    bool isMatch = true;

                    for (int i = 0; i < Parts.Length; i++)
                    {
                        isMatch &= Parts[i].IsMatch(parts[i], out var variable);
                        if (isMatch && Parts[i].IsVariable)
                        {
                            parameters.Add(Parts[i].VariableName, variable);
                        }
                        if (!isMatch)
                        {
                            return false;
                        }
                    }

                    return isMatch;
                }
                else
                {
                    return false;
                }
            }

            internal void Activate(Parameters parameters)
            {
                _action(parameters);
            }
        }


        private static State _currentState;

        public delegate void NavigatedHandler(State toState, State fromState);
        public delegate bool CanNavigateHandler(State toState, State fromState);

        public static event NavigatedHandler onNavigated;
        public static event CanNavigateHandler onBeforeNavigate;

        public static void OnNavigated(NavigatedHandler onNavigated)
        {
            Router.onNavigated += onNavigated;
        }

        public static void OnBeforeNavigate(CanNavigateHandler onBeforeNavigate)
        {
            //We only keep track of one active BeforeNavigate event
            foreach (Delegate d in Router.onBeforeNavigate.GetInvocationList())
            {
                Router.onBeforeNavigate -= (CanNavigateHandler)d;
            }

            if(onBeforeNavigate is null) onBeforeNavigate = (a, b) => true;

            Router.onBeforeNavigate += onBeforeNavigate;
        }

        public static void Initialize()
        {
            if (!_initialized)
            {
                Script.Write(
@"
    window.history.pushState = ( f => function pushState(){
        var ret = f.apply(this, arguments);
        window.dispatchEvent(new Event('pushstate'));
        window.dispatchEvent(new Event('locationchange'));
        return ret;
    })(window.history.pushState);

    window.history.replaceState = ( f => function replaceState(){
        var ret = f.apply(this, arguments);
        window.dispatchEvent(new Event('replacestate'));
        window.dispatchEvent(new Event('locationchange'));
        return ret;
    })(window.history.replaceState);

    window.addEventListener('popstate',()=>{
        window.dispatchEvent(new Event('locationchange'))
    });
");
                window.addEventListener("locationchange", onLocationChanged);
            }
            _initialized = true;           
        }

        private static bool _initialized = false;
        private static Dictionary<string, Action<Parameters>> _routes = new Dictionary<string, Action<Parameters>>();
        private static Dictionary<string, string> _paths = new Dictionary<string, string>();

        public static void Push(string path)
        {
            if (AlreadyThere(path)) return; //Nothing to do

            if (_currentState is null)
            {
                _currentState = new State()
                {
                    FullPath = path
                };
            }
            else
            {
                _currentState.FullPath = path;
            }

            window.history.pushState(null, "", path);
        }

        public static void Replace(string path)
        {
            if (AlreadyThere(path)) return; //Nothing to do

            if (_currentState is null)
            {
                _currentState = new State()
                {
                    FullPath = path
                };
            }
            else
            {
                _currentState.FullPath = path;
            }

            window.history.replaceState(null, "", path);
        }

        /// <summary>
        /// This will navigate the User to the specified path (pushing a new entry in the navigation history stack, so the current page / URL will appear in the browser's back button history) unless the path is that which the browser is already at - this
        /// behaviour may be overridden by setting the optional <paramref name="reload"/> to true (this does not force a reload of the page, it forces a reload of the current view by firing an OnNavigated event whether the specifeid path is 'new' or not)
        /// </summary>
        public static void Navigate(string path, bool reload = false)
        {
            if (reload)
            {
                window.location.href = "./#/donothing";
            }
            else
            {
                if (_currentState is object && path == _currentState.FullPath)
                    return; // Nothing to do
            }

            if (!AlreadyThere(path))
            {
                window.location.href = path;
            }
            else if (_currentState is null)
            {
                onLocationChanged(null);
            }
        }

        private static bool AlreadyThere(string path)
        {
            return window.location.href == path || window.location.hash == path;
        }

        private static string LowerCasePath(string path)
        {
            var sb = new StringBuilder();
            bool inParameter = false;
            foreach(var c in path)
            {
                if (c == ':')
                {
                    inParameter = true; sb.Append(':');
                }
                else if (c == '/')
                {
                    inParameter = false;
                    sb.Append('/');
                }
                else
                {
                    if (inParameter) sb.Append(c);
                    else sb.Append(char.ToLower(c));
                }
            }
            return sb.ToString();
        }

        public static void Register(string uniqueIdentifier, string path, Func<Parameters, Task> actionTask)
        {
            Register(uniqueIdentifier, path, (p) => actionTask(p).FireAndForget());
        }

        public static void Register(string uniqueIdentifier, string path, Action<Parameters> action, bool replace = false)
        {
            uniqueIdentifier = uniqueIdentifier.ToLower();

            if (_routes.ContainsKey(uniqueIdentifier) && !replace)
            {
                // 2020-02-12 DWR: The last thing that the Mosaik App class does is register default routes - this means that the default routes are declared after any routes custom to the current app and this means that it
                // wouldn't be possible to have custom home pages (for example).. unless we ignore any repeat calls that specify the same uniqueIdentifier. Ignoring them allows the current app to specify a "home" route and
                // for the "home" route in the DefaultRouting.Initialize to then be ignored.
                return;
            }

            var lowerCaseID = $"path-{uniqueIdentifier}";
            var upperCaseID = $"PATH-{uniqueIdentifier.ToUpper()}";

            _paths.Remove(uniqueIdentifier);
            _paths.Remove(lowerCaseID);
            _paths.Remove(upperCaseID);
            _routes.Remove(uniqueIdentifier);
            _routes.Remove(lowerCaseID);
            _routes.Remove(upperCaseID);

            var lowerCasePath = LowerCasePath(path);

            if (path != lowerCasePath)
            {
                _routes[lowerCaseID] = action;
                _paths[lowerCaseID] = lowerCasePath;
            }

            _routes[uniqueIdentifier] = action;
            _paths[uniqueIdentifier] = path;

            Refresh();
        }

        private static List<Route> Routes;

        public static void Refresh(Action onDone = null)
        {
            if (!_initialized) { return; }

            Routes = new List<Route>();
            foreach (var kv in _paths)
            {
                Routes.Add(new Route(kv.Key, kv.Value, _routes[kv.Key]));
            }

            onDone?.Invoke();
        }

        private static void onLocationChanged(Event ev)
        {
            var currenthash = window.location.hash ?? "";

            if(_currentState is object && _currentState.FullPath == currenthash)
            {
                return;
            }

            var p = currenthash.Split(new[] { '?' }, 2); //Do not remove empty entries, as we need the empty entry in the array

            var hash = p.Length == 0 ? "" : p[0].TrimStart('#');
            
            var par = new Dictionary<string, string>();
            var parts = hash.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var r in Routes)
            {
                par.Clear();
                if (r.IsMatch(parts, par))
                {
                    if (p.Length > 1)
                    {
                        //TODO parse query parameters
                        var query = p[1];
                        var queryParts = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach(var qp in queryParts)
                        {
                            var qpp = qp.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            if(qpp.Length == 1)
                            {
                                par[es5.decodeURIComponent(qpp[0])] = "";
                            }
                            else
                            {
                                par[es5.decodeURIComponent(qpp[0])] = es5.decodeURIComponent(qpp[1]);
                            }
                        }
                    }

                    var toState = new State()
                    {
                        Parameters = new Parameters(par),
                        Path = hash,
                        FullPath = window.location.href,
                        RouteName = r.Name
                    };

                    if(onBeforeNavigate is null || onBeforeNavigate(toState, _currentState))
                    {
                        var oldState = _currentState;
                        _currentState = toState;
                        r.Activate(toState.Parameters);
                        onNavigated?.Invoke(toState, oldState);
                        return;
                    }
                    else
                    {
                        if(_currentState is object && !string.IsNullOrEmpty(_currentState.FullPath))
                        {
                            window.location.href = _currentState.FullPath;
                        }
                        return;
                    }
                }
            }
        }
    }

    public class Parameters
    {
        private Dictionary<string, string> _parameters;

        public Parameters(Dictionary<string, string> parameters)
        {
            _parameters = parameters;
        }

        public new string this[string key] => _parameters[key];

        public IEnumerable<string> Keys => _parameters.Keys;

        public IEnumerable<string> Values => _parameters.Values;

        public int Count => _parameters.Count;

        public bool ContainsKey(string key)
        {
            return _parameters.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        public bool TryGetValue(string key, out string value)
        {
            return _parameters.TryGetValue(key, out value);
        }

        public Parameters With(string key, string value)
        {
            _parameters[key] = value;
            return this;
        }
    }
}
