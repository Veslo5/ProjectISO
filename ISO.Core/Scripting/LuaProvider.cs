using ISO.Core.DataLoader.SqliteClient;
using ISO.Core.DataLoader.SqliteClient.Contracts;
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


        public bool IsEnabled { get; set; }
        private string Path { get; }
        private int Id { get; }

        public LuaProvider(string path, int id, bool enabled = false)
        {
            ScriptHolder = new List<IsoScript>();
            IsEnabled = enabled;
            Path = path;
            Id = id;
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

            SCRIPT dbScript = null;

            using (var context = new ISODbContext(Path))
            {
                dbScript = context.LoadTForMap<SCRIPT>(Id);
            }

            if (dbScript == null)
            {
                Log.Warning("Script not found... continuing without script");
                return;
            }

            script.LastLoadedScriptValue = script.LoadString(dbScript.DATA).Function.Call();

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

            SCRIPT dbScript = null;

            using (var context = new ISODbContext(Path))
            {
                dbScript = context.LoadTForMap<SCRIPT>(Id);
            }

            script.LastLoadedScriptValue = script.LoadString(dbScript.DATA).Function.Call();
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
