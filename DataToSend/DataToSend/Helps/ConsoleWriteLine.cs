using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace CommonDll.Helps
{
    public static class ConsoleWriteLine
    {
        public static bool WriteInConsole_Default { get; set; } = true;
        public static void WriteInConsole(string Module, string Process, string Status, string Comment, ConsoleColor consoleColor)
        {
            DateTime Now = DateTime.Now;
            string ToWrite = string.Format("\n[{0}] ", Now.ToLongTimeString());

            if (!string.IsNullOrEmpty(Module))
                ToWrite += string.Format("Parent's module: [ {0} ] \n       ", Module);

            if (!string.IsNullOrEmpty(Process))
                ToWrite += string.Format("Child's process: [ {0} ] \n           ", Process);

            if (!string.IsNullOrEmpty(Status))
            {
                ToWrite += string.Format("Status: [ {0} ], \n               ", Status);

                if (Status.ToUpper().Contains("Failed".ToUpper()))
                    consoleColor = ConsoleColor.Red;
            }

            if (!string.IsNullOrEmpty(Comment))
                ToWrite += string.Format("Comment: [ {0} ]", Comment);

            if (ToWrite is null ? false : ToWrite.Length > 0)
                WriteInConsole_Add(ToWrite, Now, consoleColor);
        }
        static Dictionary<string, Tuple<DateTime, ConsoleColor>> ToWriteInConsole = new Dictionary<string, Tuple<DateTime, ConsoleColor>>();
        static void WriteInConsole_Add(string ToWrite, DateTime Now, ConsoleColor consoleColor)
        {
            try
            {
                lock (LockObject_ToRemoveAdd)
                    if (!ToWriteInConsole.ContainsKey(ToWrite))
                        ToWriteInConsole.Add(ToWrite, new Tuple<DateTime, ConsoleColor>(Now, consoleColor));
            }
            catch (IndexOutOfRangeException ex)
            {
                WriteInConsole_ReCreate();
                WriteInConsole_Add(ToWrite, Now, consoleColor);
            }
            catch (Exception ex)
            {
                WriteInConsole_Add(ToWrite, Now, consoleColor);
            }
        }
        static DateTime LastClear { get; set; } = DateTime.Now;
        static Thread Thread { get; set; }
        static void WriteInConsole_Write()
        {
            List<KeyValuePair<string, Tuple<DateTime, ConsoleColor>>> Selected = WriteInConsole_Selected();
            Selected.Sort((a, b) => a.Value is null ? 0 : a.Value.Item1.CompareTo(b.Value is null ? default : b.Value.Item1));

            int i = 0;

            Thread = new Thread(() =>
            {
                try
                {
                    if (Thread.CurrentThread.Name is null)
                        Thread.CurrentThread.Name = nameof(WriteInConsole_Write);

                    foreach (var r in Selected)
                    {
                        i++;

                        if (r.Value.Item2 != ConsoleColor.Black)
                            Console.ForegroundColor = r.Value.Item2; // устанавливаем цвет
                        else
                            Console.ResetColor(); // сбрасываем в стандартный

                        string ToWrite = r.Key;

                        if (ToWrite is null ? false : ToWrite.Length > 0)
                            Console.WriteLine(ToWrite);
                    }

                    Console.ResetColor(); // сбрасываем в стандартный
                }
                catch (ThreadAbortException ex)
                {
                    Thread.ResetAbort();
                }
                catch (Exception ex)
                {

                }
            });
            Thread.IsBackground = true;

            try
            {
                Thread.Start();
                Thread.Join((int)Math.Round(TimeSpan.FromMinutes(2).TotalMilliseconds));
            }
            catch (ThreadStateException ex)
            {
                //Progress.ExceptionMessage = ex.ToString();
            }
            catch (ThreadStartException ex)
            {
                //Progress.ExceptionMessage = ex.ToString();
            }
            catch (NullReferenceException ex)
            {
                //Progress.ExceptionMessage = ex.ToString();
            }

            try
            {
                if (Thread is null ? false : Thread.IsAlive)
                {
                    try
                    {
                        if (Thread is null ? false : Thread.IsAlive)
                            Thread.Abort();
                    }
                    catch
                    {

                    }

                    try
                    {
                        Thread = null;
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }

            if (i > 0 ? i != Selected.Count : false)
                for (int j = 0; j < i; j++)
                    Selected.RemoveAt(Selected.Count - 1);

            WriteInConsole_Remove(Selected.Select(x => x.Key).ToList());

            if (DateTime.Now - LastClear > TimeSpan.FromMinutes(30))
            {
                try
                {
                    Console.Clear();
                }
                catch (IOException ex)
                {

                }

                LastClear = DateTime.Now;
            }
        }
        static List<KeyValuePair<string, Tuple<DateTime, ConsoleColor>>> WriteInConsole_Selected()
        {
            try
            {
                List<KeyValuePair<string, Tuple<DateTime, ConsoleColor>>> Selected = new List<KeyValuePair<string, Tuple<DateTime, ConsoleColor>>>();

                lock (LockObject_ToRemoveAdd)
                    foreach (var r in ToWriteInConsole)
                        Selected.Add(r);

                return Selected;
            }
            catch (IndexOutOfRangeException ex)
            {
                WriteInConsole_ReCreate();

                return WriteInConsole_Selected();
            }
            catch (Exception ex)
            {
                return WriteInConsole_Selected();
            }
        }
        static object LockObject_ToRemoveAdd = new object();
        static void WriteInConsole_ReCreate()
        {
            try
            {
                Dictionary<string, Tuple<DateTime, ConsoleColor>> keyValuePairs = new Dictionary<string, Tuple<DateTime, ConsoleColor>>();

                lock (LockObject_ToRemoveAdd)
                    foreach (var r in ToWriteInConsole)
                        keyValuePairs.Add(r.Key, r.Value);

                ToWriteInConsole = keyValuePairs;
            }
            catch
            {
                WriteInConsole_ReCreate();
            }
        }
        static void WriteInConsole_Remove(List<string> ToRemove)
        {
            try
            {
                lock (LockObject_ToRemoveAdd)
                {
                    foreach (var r in ToRemove)
                        if (ToWriteInConsole.ContainsKey(r))
                            ToWriteInConsole.Remove(r);

                    foreach (var r in ToWriteInConsole)  // Есль все же консоль не справляется, то удалять устарешие консольные логи
                        if (DateTime.Now - r.Value.Item1 > TimeSpan.FromMinutes(10))
                            ToWriteInConsole.Remove(r.Key);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                WriteInConsole_ReCreate();
                WriteInConsole_Remove(ToRemove);
            }
            catch (Exception ex)
            {
                WriteInConsole_Remove(ToRemove);
            }
        }

        #region Timer
        static Timer Timer { get; set; } = TimerStart((int)Math.Round(TimeSpan.FromSeconds(1).TotalMilliseconds));
        private static Timer TimerStart(int milliseconds)
        {
            if (milliseconds > 0)
            {
                Timer Timer = new Timer(milliseconds);
                Timer.Elapsed += Timer_Elapsed;
                Timer.Start();

                return Timer;
            }

            return default;
        }
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (WriteInConsole_Default)
                if (!Helper.IsLocked(Timer))
                    lock (Timer)
                        WriteInConsole_Write();
        }
        #endregion
    }
}
