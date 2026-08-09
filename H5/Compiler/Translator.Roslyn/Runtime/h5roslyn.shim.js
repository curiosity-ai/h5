// H5.Translator.Roslyn — thin adapter that exposes the small set of language-level
// helpers the emitter relies on, implemented over the real h5.js runtime primitives.
// This lets emitted code interoperate with h5.js / h5.core.
(function (global) {
    var H5 = global.H5;
    var H5R = global.H5R = global.H5R || {};

    H5R.toStr = function (v) {
        if (v === null || v === undefined) { return ""; }
        if (typeof v === "boolean") { return v ? "True" : "False"; }
        try { return H5.toString(v); } catch (e) { return v.toString ? v.toString() : String(v); }
    };
    H5R.chr = function (code) { return String.fromCharCode(code); };
    H5R.is = function (v, t) { return H5.is(v, t); };
    H5R.as = function (v, t) { return H5.as ? H5.as(v, t) : (H5.is(v, t) ? v : null); };
    H5R.equals = function (a, b) { return H5.equals ? H5.equals(a, b) : a === b; };
    H5R.idiv = (H5.Int && H5.Int.div) ? function (a, b) { return H5.Int.div(a, b); } : function (a, b) { var r = a / b; return r < 0 ? Math.ceil(r) : Math.floor(r); };
    H5R.trunc = function (x) { return x < 0 ? Math.ceil(x) : Math.floor(x); };
    H5R.clone = function (o) {
        if (!o) { return o; }
        if (o.$clone) { return o.$clone(); }
        return Object.assign(Object.create(Object.getPrototypeOf(o)), o);
    };
    H5R.hash = function (v) { return H5.getHashCode ? H5.getHashCode(v) : 0; };
    H5R.getEnumerator = function (src) {
        var wrap = function (e) {
            return { moveNext: function () { return e.moveNext ? e.moveNext() : e.MoveNext(); }, get current() { return e.Current !== undefined ? e.Current : e.current; } };
        };
        if (src != null) {
            // Already an enumerator (pattern-based / extension GetEnumerator result).
            if (typeof src.moveNext === "function" || typeof src.MoveNext === "function") { return wrap(src); }
            // An enumerable with its own GetEnumerator (e.g. H5R.iter iterables).
            if (typeof src.GetEnumerator === "function") { return wrap(src.GetEnumerator()); }
        }
        if (H5.getEnumerator) { return wrap(H5.getEnumerator(src)); }
        var i = -1; return { moveNext: function () { i++; return i < src.length; }, get current() { return src[i]; } };
    };
    H5R.dispose = function (x) { if (x) { if (x.dispose) { x.dispose(); } else if (x.Dispose) { x.Dispose(); } } };
    H5R.array = function (n, d) { var a = new Array(n); for (var i = 0; i < n; i++) { a[i] = d; } return a; };

    // Delegate / event helpers (multicast combine + remove) over h5.js's H5.fn.
    H5R.combine = function (a, b) { return H5.fn.combine(a, b); };
    H5R.remove = function (a, b) { return H5.fn.remove(a, b); };

    // Async interop: adapt a native Promise (produced by an emitted `async` body) into an
    // h5.js Task, so async methods return real Tasks that compose with Task.Run/WhenAll/
    // ContinueWith and route exceptions through the Task (faulted state), matching h5.js.
    H5R.fromPromise = function (p) {
        var tcs = new System.Threading.Tasks.TaskCompletionSource();
        Promise.resolve(p).then(
            function (v) { tcs.setResult(v); },
            function (e) { tcs.setException(System.Exception.create(e)); }
        );
        return tcs.task;
    };

    // Spread source → JS array (arrays pass through; other enumerables are drained).
    H5R.spread = function (x) {
        if (x == null) { return []; }
        if (Array.isArray(x)) { return x; }
        var out = [], e = H5R.getEnumerator(x);
        while (e.moveNext()) { out.push(e.current); }
        return out;
    };
    H5R.formatValue = function (v, fmt) { try { return System.String.format("{0:" + fmt + "}", v); } catch (e) { return H5R.toStr(v); } };

    // Date/TimeSpan arithmetic helpers (best-effort; System types come from h5.js).
    H5R.dtSub = function (a, b) { return System.DateTime.subdd(a, b); };
    H5R.dtSubTs = function (a, b) { return System.DateTime.subdt(a, b); };
    H5R.dtAddTs = function (a, b) { return System.DateTime.adddt(a, b); };
    H5R.tsAdd = function (a, b) { return System.TimeSpan.add(a, b); };
    H5R.tsSub = function (a, b) { return System.TimeSpan.sub(a, b); };

    // Iterator (yield) support: a re-enumerable wrapper around a generator function.
    // Built on h5.js's own GeneratorEnumerable so the result is a real
    // System.Collections.Generic.IEnumerable<object> — recognised by H5.as/H5.getEnumerator
    // AND by System.Linq.Enumerable.from (which checks H5.as(_, IEnumerable) and otherwise
    // treats the source as empty). A plain {GetEnumerator} object satisfies the former but not
    // the latter, so LINQ over an iterator method would silently yield nothing.
    H5R.iter = function (genFn) {
        var T = System.Object;
        return new (H5.GeneratorEnumerable$1(T))(function () {
            var it = genFn();
            var en = new (H5.GeneratorEnumerator$1(T))(function () {
                var r = it.next();
                if (r.done) { return false; }
                en.current = r.value;
                return true;
            });
            return en;
        });
    };
})(typeof globalThis !== "undefined" ? globalThis : this);
