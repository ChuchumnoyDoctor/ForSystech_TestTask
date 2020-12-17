using CommonDll.Structs;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace CommonDll.Helps
{
    public static class Helper
    {
        public static void CheckDirectories()
        {
            // Start with drives if you have to search the entire computer.
            string[] drives = System.Environment.GetLogicalDrives();

            foreach (string dr in drives)
            {
                System.IO.DriveInfo di = new System.IO.DriveInfo(dr);
                Console.WriteLine("{0} IsReady: {1}", di.Name, di.IsReady);
            }
        }
        public static T InheritanceClone<T>(MainParentClass From, T To) where T : MainParentClass
        {
            if (!(To is null) & !(From is null))
            {
                var Properties_From = From.GetType().GetProperties().ToList();
                var PropertiesToRemove = typeof(MainParentClass).GetProperties().ToList();

                foreach (var Property in PropertiesToRemove)
                {
                    int Index = Properties_From.FindIndex(x => x.Name == Property.Name);

                    if (Index > -1)
                        Properties_From.RemoveAt(Index);
                }

                {
                    int Index = Properties_From.FindIndex(x => x.Name == "UpdateObject_Original");

                    if (Index > -1)
                        Properties_From.RemoveAt(Index);
                }

                var Properties_To = To.GetType().GetProperties().ToList();

                foreach (var Property in Properties_From)
                {
                    var Finded = Properties_To.FirstOrDefault(x => x.Name == Property.Name);

                    if (!(Finded is null))
                        try
                        {
                            var GetValue = Property.GetValue(From);
                            Finded.SetValue(To, GetValue);
                        }
                        catch (Exception ex)
                        {

                        }
                }
            }

            return To;
        }
        /// <summary>
        /// Получить все свойства объекта данного класса
        /// </summary>
        /// <typeparam name="T">Тип выходящих аргументов</typeparam>
        /// <typeparam name="T2">Тип входящего аргумента</typeparam>
        /// <param name="SettedArgument">Входящий аргумент</param>
        /// <returns></returns>
        public static Dictionary<string, T> GetProperties<T, T2>(T2 SettedArgument) where T2 : class
        {
            Dictionary<string, T> Properties = new Dictionary<string, T>();
            PropertyInfo[] properties = SettedArgument.GetType().GetProperties();

            foreach (var r in properties)
            {
                var Name = r.Name;
                var Value = r.GetValue(SettedArgument);
                var PropertyType = r.PropertyType;
                var BaseType = PropertyType.BaseType;

                if (((BaseType == typeof(T)) || (PropertyType == typeof(T))) & !(Value is null) & !string.IsNullOrEmpty(Name))
                    Properties.Add(Name, (T)Value);
            }

            return Properties;
        }
        public static bool IsDriveNetwork(string drive)
        {
            if (string.IsNullOrEmpty(drive))
                return false;

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo d in drives)
                if (d.DriveType == DriveType.Network && d.Name.Equals(drive, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }

        [DllImport("mpr.dll")]
        static extern int WNetGetUniversalNameA(
            string lpLocalPath, int dwInfoLevel, IntPtr lpBuffer, ref int lpBufferSize
        );

        // I think max length for UNC is actually 32,767
        public static string LocalToUNC(string localPath, int maxLen = 2000)
        {
            IntPtr lpBuff;

            // Allocate the memory
            try
            {
                lpBuff = System.Runtime.InteropServices.Marshal.AllocHGlobal(maxLen);
            }
            catch (OutOfMemoryException)
            {
                return null;
            }

            try
            {
                int res = WNetGetUniversalNameA(localPath, 1, lpBuff, ref maxLen);

                if (res != 0)
                    return null;

                // lpbuff is a structure, whose first element is a pointer to the UNC name (just going to be lpBuff + sizeof(int))
                return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(System.Runtime.InteropServices.Marshal.ReadIntPtr(lpBuff));
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(lpBuff);
            }
        }
        public static object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType)
                return Activator.CreateInstance(type);

            if (value == null)
                return null;

            if (type == value.GetType())
                return value;

            if (type.IsEnum)
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);

            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);

                return Activator.CreateInstance(type, new object[] { innerValue });
            }

            if (value is string && type == typeof(Guid))
                return new Guid(value as string);

            if (value is string && type == typeof(System.Version))
                return new System.Version(value as string);

            if (!(value is IConvertible))
                return value;

            return Convert.ChangeType(value, type);
        }
        public static void RemoveBackupFiles(string ApplicationStartupPath, string format)
        {
            if (Directory.Exists(ApplicationStartupPath))
            {
                string[] files_to_remove = Directory.GetFiles(ApplicationStartupPath, format);

                if (files_to_remove.Count() > 0)
                    foreach (string file in files_to_remove)
                        if (File.Exists(file))
                            try
                            {
                                File.Delete(file);
                            }
                            catch
                            {

                            }
            }
        }
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (!target.FullName.Contains("Z:"))
            {
                if (!Directory.Exists(target.FullName))
                    Directory.CreateDirectory(target.FullName);

                // ToCopy each file into the new directory.
                foreach (FileInfo fi in source.GetFiles())
                {
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                    ConsoleWriteLine.WriteInConsole(null, null, "Done", string.Format(@"Copying {0}\{1}", target.FullName, fi.Name), ConsoleColor.White);
                }

                // ToCopy each subdirectory using recursion.
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                        target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
            }
        }
        public static void DeleteAll(DirectoryInfo source)
        {
            if (!source.FullName.Contains("Z:"))
            {
                FileInfo[] GetFiles = default;

                try
                {
                    GetFiles = source.GetFiles();
                }
                catch (UnauthorizedAccessException ex)
                {

                }

                if (GetFiles is null ? false : GetFiles.Count() > 0) // Delete each file into the new directory.
                    foreach (FileInfo fi in GetFiles)
                    {
                        ConsoleWriteLine.WriteInConsole(null, null, "Done", string.Format(@"Delete {0}\{1}", source.FullName, fi.Name), ConsoleColor.White);

                        try
                        {
                            fi.Delete();
                        }
                        catch { }
                    }

                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories()) // Delete each subdirectory using recursion.
                {
                    DeleteAll(diSourceSubDir);

                    try
                    {
                        diSourceSubDir.Delete();
                    }
                    catch
                    {
                        DeleteAll(diSourceSubDir);
                    }
                }
            }
        }
        public static bool PrepaerPathToCopy(out string ExceptionMessage, string BeforePath, string NewBasePath, out string NewPath)
        {
            bool Done = false;
            ExceptionMessage = null;
            NewPath = NewBasePath;

            if (!NewBasePath.Contains("Z:"))
                if (File.Exists(BeforePath))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(NewBasePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(NewBasePath));

                    if (File.Exists(NewBasePath))
                        try
                        {
                            File.Delete(NewBasePath);
                        }
                        catch (Exception ex)
                        {
                            Process[] runningProcesses = Process.GetProcesses();

                            foreach (Process process in runningProcesses)
                                try
                                {
                                    if (!(process.MainModule is null))
                                        if (string.Compare(process.MainModule.FileName, NewBasePath, StringComparison.InvariantCultureIgnoreCase) == 0)
                                            process.Kill();
                                }
                                catch
                                {
                                    ExceptionMessage = ex.Message.ToString();
                                }
                        }

                    if (!File.Exists(NewBasePath))
                        try
                        {
                            File.Copy(BeforePath, NewBasePath);
                            Done = true;
                        }
                        catch (FileNotFoundException ex)
                        {
                            string Format = NewBasePath.Substring(NewBasePath.LastIndexOf("."), NewBasePath.Length - NewBasePath.LastIndexOf("."));
                            string NewPat = NewBasePath.Substring(0, NewBasePath.LastIndexOf("."));
                            NewPat += "1" + Format;

                            if (PrepaerPathToCopy(out ExceptionMessage, BeforePath, NewPat, out NewPath))
                                Done = true;
                            else
                            {

                            }
                        }
                    else
                    {
                        string Format = NewBasePath.Substring(NewBasePath.LastIndexOf("."), NewBasePath.Length - NewBasePath.LastIndexOf("."));
                        string NewPat = NewBasePath.Substring(0, NewBasePath.LastIndexOf("."));
                        NewPat += "1" + Format;

                        if (PrepaerPathToCopy(out ExceptionMessage, BeforePath, NewPat, out NewPath))
                            Done = true;
                        else
                        {

                        }
                    }
                }
                else
                    ExceptionMessage = string.Format("\n\n{0}: file not exists({1})", Path.GetFileName(BeforePath), BeforePath);

            return Done;
        }
        public static string TransformDetailName(string name)
        {
            if (name.LastIndexOf('.') > 0)
                name = name.Substring(0, name.LastIndexOf('.'));

            if (name.IndexOf("_razv") > 0)
                name = name.Substring(0, name.IndexOf("_razv"));

            return name;
        }

        /// <summary>
        /// To know that object is locking
        /// </summary>
        public static bool IsLocked(object o)
        {
            if (!(o is null))
            {
                if (!Monitor.TryEnter(o))
                    return true;

                Monitor.Exit(o);
            }
            else
            {

            }

            return false;
        }
        public static string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties adapterProperties = item.GetIPProperties();

                    if (adapterProperties.GatewayAddresses.FirstOrDefault() != null)
                        foreach (UnicastIPAddressInformation ip in adapterProperties.UnicastAddresses)
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                output = ip.Address.ToString();
                }

            if (string.IsNullOrEmpty(output))
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (var ip in host.AddressList)
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        output = ip.ToString();
            }

            return output;
        }

        /// <summary>
        /// From TimeSpan to string without Days(Days => Hours)
        /// </summary>
        public static string ToString(this TimeSpan Time)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", (Time.Days * 24) + Time.Hours, Time.Minutes, Time.Seconds);
        }
        public static List<int> GetNumbersFromString(string name)
        {
            List<int> vs_name = new List<int>();
            string buff_name = "";

            if (!string.IsNullOrEmpty(name))
                foreach (var ch in name)
                    if (int.TryParse(ch + "", out int x))
                        buff_name += x;
                    else if (!string.IsNullOrEmpty(buff_name))
                    {
                        if (int.TryParse(buff_name, out int jj))
                            vs_name.Add(jj);

                        buff_name = "";
                    }

            if (!string.IsNullOrEmpty(buff_name))
                if (int.TryParse(buff_name, out int jj))
                    vs_name.Add(jj);

            return vs_name;
        }

        #region Default value
        public static dynamic GetOrNull(dynamic dynamicObject) // For views on client
        {
            if (dynamicObject is null)
                return null;
            else if (((object)dynamicObject).Equals(ReturnDefault(dynamicObject)))
                return null;
            else
                return dynamicObject;
        }
        private static Dictionary<Type, dynamic> DefaultValue { get; set; } = new Dictionary<Type, dynamic>();
        public static T ReturnDefault<T>(T TObject)
        {
            lock (DefaultValue)
            {
                T Returned = default;

                if (!(TObject is null))
                {
                    Type Type = TObject.GetType();
                    var Finded = DefaultValue.FirstOrDefault(x => x.Key == Type || x.Key.Equals(Type));

                    if (Finded.Key is null)
                    {
                        if (Returned is null)
                            try
                            {
                                Returned = (T)Activator.CreateInstance(Type);
                            }
                            catch
                            {

                            }

                        if (Returned is null)
                            Returned = (T)default(T);

                        DefaultValue.Add(Type, Returned);
                    }
                    else
                        Returned = Finded.Value;
                }

                return Returned;
            }
        }
        #endregion
    }

    #region Applications
    public static class Helper_WINWORD
    {
        public static object LockObject_ForCreateProcessKill = new object();
        private static object threadLockHighPriority = new object();
        private static Microsoft.Office.Interop.Word.Application GetApp(int RecursTry)
        {
            if (RecursTry == 3)
                return default;
            else
                try
                {
                    return new Microsoft.Office.Interop.Word.Application();
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine.WriteInConsole(null, null, "Failed", ex.Message.ToString(), ConsoleColor.Red);

                    return GetApp(++RecursTry);
                }
        }
        public static Document GetDocument(Microsoft.Office.Interop.Word.Application Application, string path, int RecursTry)
        {
            if (RecursTry == 3)
                return default;
            else
                try
                {
                    Document document = Application.Documents.OpenNoRepairDialog(path, ReadOnly: true);
                    document.Activate();

                    return document;
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine.WriteInConsole(null, null, "Failed", ex.Message.ToString(), ConsoleColor.Red);

                    return GetDocument(Application, path, ++RecursTry);
                }
        }
        /// <summary>
        /// Get 'WINWORD' application and Id of that process
        /// </summary>
        public static Microsoft.Office.Interop.Word.Application GetApp(out int IdOfProcess, bool WriteInConsole)
        {
            lock (threadLockHighPriority)
            {
                Microsoft.Office.Interop.Word.Application Application;

                Process[] ProcessBefore = Process.GetProcessesByName("WINWORD");
                Application = GetApp(0);
                Process[] ProcessAfter = Process.GetProcessesByName("WINWORD");

                while (ProcessBefore.Length == ProcessAfter.Length)
                {
                    ProcessAfter = Process.GetProcessesByName("WINWORD");

                    if ((ProcessAfter.Length - ProcessBefore.Length) != 1)
                    {
                        ProcessBefore = Process.GetProcessesByName("WINWORD");
                        Application = GetApp(0);
                    }
                }

                IdOfProcess = default;

                if (ProcessAfter.Length - ProcessBefore.Length == 1)
                    foreach (var a in ProcessAfter)
                    {
                        bool i = false;

                        foreach (var b in ProcessBefore)
                            if (a.Id == b.Id)
                                i = true;

                        if (!i)
                        {
                            IdOfProcess = a.Id;

                            if (WriteInConsole)
                                ConsoleWriteLine.WriteInConsole("Threads", "GetApplication", "End", "All threads:" + ProcessAfter.Length, ConsoleColor.Red);

                            return Application;
                        }
                    }

                if (WriteInConsole)
                    ConsoleWriteLine.WriteInConsole("Threads", "GetApplication", "Failed", "All threads:" + ProcessAfter.Length, ConsoleColor.Red);

                return Application;
            }
        }
        /// <summary>
        /// Kill 'WINWORD' process by Id
        /// </summary>
        public static void KillApp(int Id, bool WriteInConsole)
        {
            lock (threadLockHighPriority)
            {
                Process[] Processs = Process.GetProcessesByName("WINWORD");
                var r = Processs.FirstOrDefault(x => x.Id == Id);

                if (!(r is null))
                    try
                    {
                        r.Kill();
                    }
                    catch { }
                    finally { if (WriteInConsole) ConsoleWriteLine.WriteInConsole("Threads", "KillApplication", "End", "All threads:" + (Processs.Length - 1), ConsoleColor.Red); }
            }
        }
        /// <summary>
        /// Kill all 'WINWORD' processes
        /// </summary>
        public static void Clear()
        {
            var Processs = Process.GetProcessesByName("WINWORD");

            for (int i = 0; i < Processs.Length; i++)
                try
                {
                    Processs[i].Kill();
                }
                catch { }
        }
    }
    public static class Helper_EXCEL
    {
        public static object LockObject_ForCreateProcessKill = new object();
        private static object threadLockHighPriority = new object();
        private static Microsoft.Office.Interop.Excel.Application GetApp(int RecursTry)
        {
            if (RecursTry == 3)
                return default;
            else
                try
                {
                    return new Microsoft.Office.Interop.Excel.Application();
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine.WriteInConsole(null, null, "Failed", ex.Message.ToString(), ConsoleColor.Red);

                    return GetApp(++RecursTry);
                }
        }
        public static Workbook GetWorkbook(Microsoft.Office.Interop.Excel.Application Application, string path, out Exception Exception, int RecursTry)
        {
            Exception = null;

            if (RecursTry == 3)
                return Application.ThisWorkbook;
            else
                try
                {
                    Workbook document = Application.Workbooks.Open(path, ReadOnly: true);
                    document.Activate();

                    return document;
                }
                catch (Exception Ex)
                {
                    Exception = Ex;
                    ConsoleWriteLine.WriteInConsole(null, null, "Failed", Ex.Message.ToString(), ConsoleColor.Red);

                    return GetWorkbook(Application, path, out Exception, ++RecursTry);
                }
        }
        /// <summary>
        /// Get 'EXCEL' application and Id of that process
        /// </summary>
        public static Microsoft.Office.Interop.Excel.Application GetApp(out int IdOfProcess, bool WriteInConsole)
        {
            lock (threadLockHighPriority)
            {
                Microsoft.Office.Interop.Excel.Application Application;
                Process[] ProcessBefore = Process.GetProcessesByName("EXCEL");
                Application = GetApp(0);
                Process[] ProcessAfter = Process.GetProcessesByName("EXCEL");

                while (ProcessBefore.Length == ProcessAfter.Length)
                {
                    ProcessAfter = Process.GetProcessesByName("EXCEL");

                    if ((ProcessAfter.Length - ProcessBefore.Length) != 1)
                    {
                        ProcessBefore = Process.GetProcessesByName("EXCEL");

                        Application = GetApp(0);
                    }
                }

                IdOfProcess = default;

                if (ProcessAfter.Length - ProcessBefore.Length == 1)
                    foreach (var a in ProcessAfter)
                    {
                        bool i = false;

                        foreach (var b in ProcessBefore)
                            if (a.Id == b.Id)
                                i = true;

                        if (!i)
                        {
                            IdOfProcess = a.Id;

                            if (WriteInConsole)
                                ConsoleWriteLine.WriteInConsole("Threads", "GetApplication", "End", "All threads:" + ProcessAfter.Length, ConsoleColor.Red);

                            return Application;
                        }
                    }

                if (WriteInConsole)
                    ConsoleWriteLine.WriteInConsole("Threads", "GetApplication", "Failed", "All threads:" + ProcessAfter.Length, ConsoleColor.Red);

                return Application;
            }
        }
        /// <summary>
        /// Kill 'EXCEL' process by Id
        /// </summary>
        public static void KillApp(int Id, bool WriteInConsole)
        {
            lock (threadLockHighPriority)
            {
                Process[] Processs = Process.GetProcessesByName("EXCEL");
                var r = Processs.FirstOrDefault(x => x.Id == Id);

                if (!(r is null))
                    try
                    {
                        r.Kill();
                    }
                    catch { }
                    finally { if (WriteInConsole) ConsoleWriteLine.WriteInConsole("Threads", "KillApplication", "End", "All threads:" + (Processs.Length - 1), ConsoleColor.Red); }
            }
        }
        /// <summary>
        /// Kill all 'EXCEL' processes
        /// </summary>
        public static void Clear()
        {
            var Processs = Process.GetProcessesByName("EXCEL");

            for (int i = 0; i < Processs.Length; i++)
                try
                {
                    Processs[i].Kill();
                }
                catch { }
        }
    }
    #endregion
}
