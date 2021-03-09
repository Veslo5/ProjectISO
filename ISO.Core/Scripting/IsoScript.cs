using MoonSharp.Interpreter;

namespace ISO.Core.Scripting
{
    public class IsoScript : Script
    {
        /// <summary>
        /// ScriptName
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loaded Script values
        /// </summary>
        public DynValue LastLoadedScriptValue { get; set; }
    }
}
