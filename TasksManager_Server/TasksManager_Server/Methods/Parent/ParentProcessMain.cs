using CommonDll.Helps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace TasksManager_Server.MethodsProcess
{
    public class ParentProcessMain
    {
        public ThreadPriority threadPriority { get; set; }

        public ParentProcessMain()
        {
            @WatchObjects = new Dictionary<string, Watcher>();
        }

        #region Watcher
        private Dictionary<string, Watcher> @WatchObjects { get; set; }

        public void Watcher_Watch(string CodeName, string MethodName, string AssemblyQualifiedName, string dir, Tuple<bool, bool, bool, bool> Changed_Created_Deleted_Renamed, List<dynamic> Config)
        {
            Watcher watcher = new Watcher();
            watcher.Watch(MethodName, AssemblyQualifiedName, dir, Changed_Created_Deleted_Renamed, Config);

            @WatchObjects.Add(CodeName, watcher);
        }

        public void Watcher_Turn(bool Turn, string CodeName)
        {
            if (@WatchObjects.ContainsKey(CodeName))
                @WatchObjects[CodeName].Watcher_Turn(Turn);
        }
        #endregion

        #region Excel
        public static string EngAlp
        {
            get
            {
                return ParseExcelDocument.EngAlp;
            }
        }

        #region Read Excel
        public Tuple<DataTable, string> Read(string Path, string LetterStart, string LetterEnd, Dictionary<string, string> PathsAndFields, int startRow)
        {
            return ParseExcelDocument.Read(Path, LetterStart, LetterEnd, PathsAndFields, startRow, out Exception exception);
        }
        #endregion
        #endregion

        #region ConsoleWriteLine
        public void WriteInConsole(string Module, string Process, string Status, string Comment, ConsoleColor consoleColor)
        {
            ConsoleWriteLine.WriteInConsole(Module, Process, Status, Comment, consoleColor);
        }
        #endregion
    }
}
