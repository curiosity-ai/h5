namespace H5.Contract
{
    /// <summary>
    /// Controls how generated JavaScript output is packaged.
    /// </summary>
    public enum OutputModuleType
    {
        /// <summary>
        /// Default H5 output: every type is registered through global <c>H5.define</c> /
        /// <c>H5.assembly</c> calls. Files are emitted with a <c>.js</c> extension.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Modern ES module output. Files are emitted with a <c>.mjs</c> extension and
        /// the generated assembly registration is wrapped in a top-level
        /// <c>import { H5 } from "./h5.mjs"</c> / <c>export { H5 }</c> pair so the
        /// resulting files can be consumed directly by browsers or bundlers as ES
        /// modules.
        /// </summary>
        ESM = 1,
    }
}
