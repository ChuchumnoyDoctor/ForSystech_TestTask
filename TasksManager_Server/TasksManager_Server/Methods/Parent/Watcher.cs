using CommonDll.Helps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Threading;

namespace TasksManager_Server
{
    public class Watcher // Просмотр папки на изменение
    {
        private FileSystemWatcher PathFolder { get; set; }
        private string MethodName { get; set; }
        private string ClassName { get; set; }
        private List<dynamic> Config { get; set; }
        private int Times { get; set; }
        private static List<Thread> threads = new List<Thread>();
        private static int time { get; set; }
        private static readonly object lockObject = new object(); // очередь

        public void Watch(string MethodName, string ClassName, string dir, Tuple<bool, bool, bool, bool> Changed_Created_Deleted_Renamed, List<dynamic> Config)
        {
            if (!string.IsNullOrEmpty(dir))
            {
                Times = 0;

                this.Config = Config;
                this.MethodName = MethodName;
                this.ClassName = ClassName;
                time = 5000;

                this.PathFolder = RunForFolders(dir, Changed_Created_Deleted_Renamed.Item1, Changed_Created_Deleted_Renamed.Item2, Changed_Created_Deleted_Renamed.Item3, Changed_Created_Deleted_Renamed.Item4);

                ConsoleWriteLine.WriteInConsole(this.MethodName, "Watch", "Start", "", ConsoleColor.White);
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private FileSystemWatcher RunForFolders(string Path, bool Changed, bool Created, bool Deleted, bool Renamed)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();

            try
            {
                watcher.Path = Path;
            }
            catch (Exception ex)
            {
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", ex.Message.ToString(), ConsoleColor.Red);

                return null;
            }

            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size | NotifyFilters.Attributes;

            // Add event handlers.
            if (Changed)
                watcher.Changed += new FileSystemEventHandler(Watcher_Process);

            if (Created)
                watcher.Created += new FileSystemEventHandler(Watcher_Process);

            if (Deleted)
                watcher.Deleted += new FileSystemEventHandler(Watcher_Process);

            if (Renamed)
                watcher.Renamed += new RenamedEventHandler(Watcher_Process);

            return watcher;
        }

        private void Watcher_Process(object sender, FileSystemEventArgs e)
        {
            lock (lockObject)
            {
                Watcher_Turn(false);

                Thread thread_Watcher_Process = new Thread(() =>
                {
                    lock (threads)
                    {
                        foreach (var thr in threads)
                            if (thr != null)
                                thr.Abort();

                        threads = new List<Thread>();

                        Thread.CurrentThread.Name = "Watcher_" + this.MethodName;
                        threads.Add(Thread.CurrentThread);
                    }

                    Thread.Sleep(time);

                    Thread threadCallProcess = new Thread(() =>
                    {
                        DelegateToMethod.GetTypeOfDelegate(MethodName, ClassName).Item1.DynamicInvoke(Config.ToArray());
                    });
                    threadCallProcess.Start();
                    threadCallProcess.Join();

                    threads.Remove(Thread.CurrentThread);

                    Times++;
                });
                thread_Watcher_Process.Start();
                thread_Watcher_Process.Join();

                Watcher_Turn(true);
            }
        }

        public void Watcher_Turn(bool Turn)
        {
            if (PathFolder != null)
            {
                PathFolder.EnableRaisingEvents = Turn;
                ConsoleWriteLine.WriteInConsole(null, null, null, string.Format("\nClassName: {0}\nPathFolder: {1}\nTurn: {2}\nTimes: {3}\n", ClassName, PathFolder.Path, Turn, Times), ConsoleColor.White);
            }
            else
            {
                ConsoleWriteLine.WriteInConsole(null, null, null, string.Format("\nClassName: {0}\nPathFolder: {1}\nTurn: {2}\nTimes: {3}\n", ClassName, "NULL", Turn, Times), ConsoleColor.White);
            }
        }
    }
}
