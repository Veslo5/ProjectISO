using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Data.DataLoader.SqliteClient.Contracts;
using ISO.Core.Engine;
using ISO.Core.Engine.Helpers.Extensions.ErrorHandling;
using ISO.Core.Engine.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISO.Core.Scripting
{
    public class LuaManager : Manager
    {
        /// <summary>
        /// Init constant for script function
        /// </summary>
        private const string INIT_NAME = "Initialize";

        /// <summary>
        /// LoadContent constant for script function
        /// </summary>
        private const string LOADCONTENT_NAME = "LoadContent";

        /// <summary>
        /// Update constant for script function
        /// </summary>
        private const string UPDATE_NAME = "Update";

        /// <summary>
        /// Draw constant for script function
        /// </summary>
        private const string DRAW_NAME = "Draw";


        /// <summary>
        /// ScriptHolder
        /// </summary>
        public List<IsoScript> ScriptHolder { get; set; }


        public bool IsEnabled { get; set; }
        private string Path { get; }
        private int Id { get; }

        public LuaManager(string path, int id, bool enabled = false)
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
                Log.Warning("Scripting is disabled for this scene.", LogModule.SC);
                return;
            }


            Log.Info("loading script " + name, LogModule.SC);

            var script = new IsoScript();
            script.Name = name;
            script.Options.DebugPrint = output => Log.Unique(output);

            SCRIPT dbScript = null;

            using (var context = new ISODbContext(Path))
            {
                dbScript = context.LoadTForMap<SCRIPT>(Id);
            }

            if (dbScript == null)
            {
                Log.Warning("Script not found... continuing without script", LogModule.SC);
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
            Log.Warning("reloading script " + scriptName, LogModule.SC);

            var script = ScriptHolder.FirstOrDefault(x => x.Name == scriptName);

            SCRIPT dbScript = null;

            using (var context = new ISODbContext(Path))
            {
                dbScript = context.LoadTForMap<SCRIPT>(Id);
            }

            script.LastLoadedScriptValue = script.LoadString(dbScript.DATA).Function.Call();
        }


        public void InvokeUpdate(string name) => InvokeFunctionFromScript(name, UPDATE_NAME);
        public void InvokeInit(string name) => InvokeFunctionFromScript(name, INIT_NAME);
        public void InvokeDraw(string name) => InvokeFunctionFromScript(name, DRAW_NAME);
        public void InvokeLoad(string name) => InvokeFunctionFromScript(name, LOADCONTENT_NAME);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        private void InvokeFunctionFromScript(string scriptName, string functionName, params object[] functionArgs)
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
                Log.Error(ex.GetAllInnerExceptions(), LogModule.SC);
            }
        }

    }
}
