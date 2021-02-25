using ISO.Core.ErrorHandling;
using ISO.Core.Logging;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO.Core.Scripting
{
    public class LuaProvider
    {
        /// <summary>
        /// ScriptHolder
        /// </summary>
        public List<IsoScript> ScriptHolder { get; set; }

        /// <summary>
        /// Default path to scripts
        /// </summary>
        public string DefaultPath { get; private set; }

        public bool IsEnabled { get; set; }


        public LuaProvider(string path, bool enabled = false)
        {
            ScriptHolder = new List<IsoScript>();
            IsEnabled = enabled;            

            //default path system
            if (string.IsNullOrEmpty(path))
            {
                DefaultPath = AppDomain.CurrentDomain.BaseDirectory + "\\scripts";
            }
            else
            {
                DefaultPath = path;
            }
        }

        /// <summary>
        /// Adds script into script stack
        /// </summary>
        /// <param name="name"></param>
        public void AddScript(string name)
        {
            if (IsEnabled == false)
            {
                Log.Warning("Scripting is disabled for this scene.");
                return;
            }


            Log.Info("loading script " + name);

            var script = new IsoScript();
            script.Name = name;
            script.Options.DebugPrint = output => Log.Script(output);

            script.LastLoadedScriptValue = script.LoadFile(DefaultPath + "\\" + name + ".lua").Function.Call();

            ScriptHolder.Add(script);
        }

        /// <summary>
        /// Reload script
        /// </summary>
        /// <param name="scriptName"></param>
        public void ReloadScript(string scriptName)
        {
            Log.Warning("reloading script " + scriptName);

            var script = ScriptHolder.FirstOrDefault(x => x.Name == scriptName);
            script.LastLoadedScriptValue = script.LoadFile(DefaultPath + "\\" + scriptName + ".lua").Function.Call();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        public void InvokeFunctionFromScript(string scriptName, string functionName, params object[] functionArgs)
        {
            if (IsEnabled == false)
                return;

            try
            {
                var script = ScriptHolder.FirstOrDefault(x => x.Name == scriptName);

                var scriptFunction = script.LastLoadedScriptValue.Table.Get(functionName);
                scriptFunction.Function.Call(functionArgs);
            }
            catch (Exception ex)
            {
                Log.Error(ex.GetAllInnerExceptions());
            }
        }

    }
}
