using CommonDll.Helps;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CommonDll.Client_Server
{
    /// <summary>
    /// Сериализуемая структура
    /// </summary>
    [Serializable]
    public class DataToSerialize
    {
        public StructureValueForClient ReadyStructure { get; set; }
        public DataSet dataset { get; set; }
        public string message { get; set; }
    }
    /// <summary>
    /// Сериализация и десериализация
    /// </summary>
    public class DataToBinary
    {
        #region Constructor
        public static byte[] Convert(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            return bytes;
        }

        public static void Convert(DataSet dataset, out byte[] bytes, out string ExceptionMessage)
        {
            bytes = default;
            byte[] local = default;
            ExceptionMessage = null;
            string ExceptionMessage_Local = null;
            {
                Convert(new DataToSerialize() { dataset = dataset }, out local, out ExceptionMessage_Local);
            }
            ExceptionMessage = ExceptionMessage_Local;
            bytes = local;
        }

        public static void Convert(DataSet dataset, StructureValueForClient structure, out byte[] bytes, out string ExceptionMessage)
        {
            byte[] local = default;
            string ExceptionMessage_Local = null;
            {
                Convert(new DataToSerialize() { dataset = dataset, ReadyStructure = structure }, out local, out ExceptionMessage_Local);
            }
            bytes = local;
            ExceptionMessage = ExceptionMessage_Local;
            bytes = local;
        }

        public static void Convert(DataToSerialize dataToSerialize, out byte[] bytes, out string ExceptionMessage)
        {
            bytes = default;
            byte[] local = default;
            ExceptionMessage = null;
            {
                local = SerializeState_ToNetWork(dataToSerialize, out string ExceptionMessage_Local, out TimeSpan TimeSpend_OnSerialize);
            }
            bytes = local;
        }

        public static void Convert(byte[] bytes, out DataToSerialize DataToSerialize, out string ExceptionMessage)
        {
            DataToSerialize = default;
            DataToSerialize local = default;
            ExceptionMessage = null;
            string ExceptionMessage_Local = null;
            {
                if (bytes is null ? false : bytes.Length != 0)
                {
                    local = DeserializeState_ToNetWork<DataToSerialize>(bytes, out ExceptionMessage_Local, out TimeSpan TimeSpend);
                }
            }
            ExceptionMessage = ExceptionMessage_Local;
            DataToSerialize = local;
        }
        public static string Convert(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
        #endregion

        #region Serialize/Deserialize
        #region To network
        public static byte[] SerializeState_ToNetWork<T>(T obj, out string Exception, out TimeSpan TimeSpend) // Сериализация 
        {
            DateTime Start = DateTime.Now;
            Exception = "";
            byte[] bytes = default;

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (var deflateStream = new DeflateStream(stream, CompressionMode.Compress, true))
                        try
                        {
                            new BinaryFormatter().Serialize(deflateStream, obj);
                        }
                        catch (Exception ex)
                        {
                            ConsoleWriteLine.WriteInConsole(nameof(SerializeState_ToNetWork), "", "Failed", ex.Message.ToString(), default);
                            Exception = ex.Message.ToString();
                        }

                    stream.Position = 0;
                    bytes = stream.GetBuffer();
                }
            }
            catch (Exception ex)
            {
                ConsoleWriteLine.WriteInConsole(nameof(SerializeState_ToNetWork), "", "Failed", ex.Message.ToString(), default);
                Exception = ex.Message.ToString();
            }

            TimeSpend = DateTime.Now - Start;

            return bytes;
        }
        public static T DeserializeState_ToNetWork<T>(byte[] bytes, out string Exception, out TimeSpan TimeSpend) // Десериализация
        {
            DateTime Start = DateTime.Now;
            Exception = "";
            T Returned = default;

            using (var ms = new MemoryStream(bytes))
            using (var deflateStream = new DeflateStream(ms, CompressionMode.Decompress, true))
                try
                {
                    Returned = (T)new BinaryFormatter().Deserialize(deflateStream);
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine.WriteInConsole(nameof(DeserializeState_ToNetWork), "", "Failed", ex.Message.ToString(), default);
                    Exception = ex.Message.ToString();
                }

            bytes = null;
            TimeSpend = DateTime.Now - Start;

            return Returned;
        }
        #endregion

        #region To file
        public static TimeSpan TimeOut = new TimeSpan(0, 20, 0);
        public static bool IsDeSerializeObject { get; set; }
        public static readonly string ApplicationBasePath = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string DirectoryPath = ApplicationBasePath + "LocalData\\";
        static string Format = ".dat";

        public static void SerializeObject_ToFile<T>(string FullPath, T serializableObject, out TimeSpan SpendOnIt) // Chosen path
        {
            SpendOnIt = default;
            string fileName = FullPath + (Format);
            string fileName_Temp = FullPath + "_Temp" + (Format);

            if (!IsDeSerializeObject)
            {
                if (serializableObject == null)
                    return;

                lock (serializableObject)
                {
                    TimeSpan timeStart = DateTime.Now.TimeOfDay;

                    try
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                            Directory.CreateDirectory(Path.GetDirectoryName(fileName));

                        using (FileStream fs = new FileStream(fileName_Temp, FileMode.OpenOrCreate))
                            new BinaryFormatter().Serialize(fs, serializableObject);

                        ConsoleWriteLine.WriteInConsole(string.Format("SerializeObject: {0}", Path.GetFileName(fileName)), "", "Done", "Time on it: " + (SpendOnIt = (DateTime.Now.TimeOfDay - timeStart)), ConsoleColor.DarkMagenta);
                    }
                    catch (Exception ex)
                    {
                        ConsoleWriteLine.WriteInConsole(string.Format("SerializeObject: {0}", Path.GetFileName(fileName)), "", "Failed", "Time on it: " + (SpendOnIt = (DateTime.Now.TimeOfDay - timeStart)) + "; Exception: " + ex.Message.ToString(), ConsoleColor.DarkMagenta);

                        if (File.Exists(fileName))
                            File.Delete(fileName);
                    }

                    try
                    {
                        if (File.Exists(fileName))
                            File.Delete(fileName);

                        if (!File.Exists(fileName))
                            File.Copy(fileName_Temp, fileName);
                    }
                    catch (Exception ex)
                    {
                        ConsoleWriteLine.WriteInConsole(nameof(SerializeObject_ToFile), "", "Failed", ex.Message.ToString(), default);

                        if (File.Exists(fileName_Temp))
                            File.Delete(fileName_Temp);

                        if (File.Exists(fileName))
                            File.Delete(fileName);
                    }
                }

                FullPath = null;
            }
        }
        public static void SerializeObject_ToFile<T>(T serializableObject, string OriginalFileName, out TimeSpan SpendOnIt) // Standart path
        {
            SerializeObject_ToFile(DirectoryPath + OriginalFileName, serializableObject, out SpendOnIt);
        }
        public static T DeSerializeObject_ToFile<T>(out TimeSpan SpendOnIt, string FullPath) // Chosen path
        {
            SpendOnIt = default(TimeSpan);
            string fileName = FullPath + (Format);
            string fileName_Temp = FullPath + "_Temp" + (Format);

            if (string.IsNullOrEmpty(fileName) ? true : !File.Exists(fileName))
            {
                ConsoleWriteLine.WriteInConsole(nameof(DeSerializeObject_ToFile), "", "Failed", "File not exist", ConsoleColor.Red);

                return Converter.CreateInstance<T>();
            }

            T objectOut = Converter.CreateInstance<T>();
            TimeSpan timeStart = DateTime.Now.TimeOfDay;

            try
            {
                ConsoleWriteLine.WriteInConsole(string.Format("DeSerializeObject: {0}", Path.GetFileName(fileName)), "", "Started", "At: " + (DateTime.Now.TimeOfDay), ConsoleColor.DarkMagenta);

                try
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                        objectOut = (T)new BinaryFormatter().Deserialize(fs);
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine.WriteInConsole(nameof(DeSerializeObject_ToFile), "", "Failed", ex.Message.ToString(), default);

                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }

                ConsoleWriteLine.WriteInConsole(string.Format("DeSerializeObject: {0}", Path.GetFileName(fileName)), "", "Done", "Time on it: " + (SpendOnIt = (DateTime.Now.TimeOfDay - timeStart)), ConsoleColor.DarkMagenta);
            }
            catch (Exception ex)
            {
                ConsoleWriteLine.WriteInConsole(nameof(DeSerializeObject_ToFile), "", "Failed", ex.Message.ToString(), default);

                if (File.Exists(fileName))
                    File.Delete(fileName);

                ConsoleWriteLine.WriteInConsole(string.Format("DeSerializeObject: {0}", Path.GetFileName(fileName)), "", "Failed", "Time on it: " + (SpendOnIt = (DateTime.Now.TimeOfDay - timeStart)) + "; Exception: " + ex.Message.ToString(), ConsoleColor.DarkMagenta);

                if (File.Exists(fileName_Temp))
                {
                    try
                    {
                        using (FileStream fs = new FileStream(fileName_Temp, FileMode.OpenOrCreate))
                            objectOut = (T)new BinaryFormatter().Deserialize(fs);
                    }
                    catch (Exception exx)
                    {
                        ConsoleWriteLine.WriteInConsole(nameof(DeSerializeObject_ToFile), "", "Failed", ex.Message.ToString(), default);

                        if (File.Exists(fileName_Temp))
                            File.Delete(fileName_Temp);
                    }
                }
            }

            FullPath = null;

            return objectOut;
        }
        public static T DeSerializeObject_ToFile<T>(string OriginalFileName, out TimeSpan SpendOnIt) // Standart path
        {
            return DeSerializeObject_ToFile<T>(out SpendOnIt, DirectoryPath + OriginalFileName);
        }
        #endregion
        #endregion
    }
}
