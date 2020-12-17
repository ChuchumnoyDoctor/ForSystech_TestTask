using CommonDll.Helps;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CommonDll.Client_Server
{
    /// <summary>
    /// Конвертация структур данных
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Получить параметры из DataSet'а
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static Tuple<List<dynamic>, string, List<dynamic>> GetParametersFromDataSet(DataSet dataSet)
        {
            Tuple<List<dynamic>, List<dynamic>, string> instructionsForServer = Instructions.ReadInstructions(dataSet); // Прочитали инструкцию, где каждый элемент листа - это возвращаемые; 'true' - делиметер между возвращаемые и передаваемыми; передаваемые типы данных, порядок и в самом конце - название обрабатываемого метода
            List<dynamic> SetParametrs = instructionsForServer.Item1; // Количество поступающих параметров
            List<dynamic> ReturnParametrs = instructionsForServer.Item2; // Кол-во возращаемых параметров.            
            string NameOfCallMethod = instructionsForServer.Item3; // Имя вызываемого метода
            Tuple<List<dynamic>, int> SetDynamics = GetParametersByNameOfParameters(SetParametrs, dataSet, 0);
            Tuple<List<dynamic>, int> ReturnDynamics = GetParametersByNameOfParameters(ReturnParametrs, dataSet, SetDynamics.Item2);

            return new Tuple<List<dynamic>, string, List<dynamic>>(SetDynamics.Item1, NameOfCallMethod, ReturnDynamics.Item1);
        }

        /// <summary>
        /// Получить параметры, основываясь на их типе данных, записанного в string-формате
        /// </summary>
        /// <param name="ParametersSet"></param>
        /// <param name="dataSet"></param>
        /// <param name="NumbOfStartTable"></param>
        /// <returns></returns>
        private static Tuple<List<dynamic>, int> GetParametersByNameOfParameters(List<dynamic> ParametersSet, DataSet dataSet, int NumbOfStartTable)
        {
            List<dynamic> typeParameterReturns = new List<dynamic>();
            List<dynamic> listDynamic = new List<dynamic>();
            string ExceptionMessage = "";

            foreach (dynamic parameter in ParametersSet)
                if ((parameter).GetType() == (new List<string>().GetType()))
                {
                    foreach (string s in parameter)
                    {
                        Tuple<dynamic, string> Item = GetInstenceFromThisNamespace(s);
                        var someItem = Item.Item1;
                        DataTable dt = dataSet.Tables[NumbOfStartTable];
                        var r = ConvertFrom(someItem, dt);
                        listDynamic.Add(r);
                        NumbOfStartTable++;
                    }

                    typeParameterReturns.Add(listDynamic);
                    listDynamic = new List<dynamic>();
                }
                else
                {
                    var someItem = default(dynamic);

                    Tuple<dynamic, string> Item = GetInstenceFromThisNamespace(parameter);
                    someItem = Item.Item1;

                    var r = default(dynamic);

                    DataTable dt = dataSet.Tables[NumbOfStartTable];
                    r = ConvertFrom(someItem, dt);

                    typeParameterReturns.Add(r); // Преобразованный экземпляр из dataSet

                    NumbOfStartTable++;
                }

            if (ExceptionMessage != "")
                ConsoleWriteLine.WriteInConsole(nameof(GetParametersByNameOfParameters), "", "Failed", string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ExceptionMessage), default);

            return new Tuple<List<dynamic>, int>(typeParameterReturns, NumbOfStartTable);
        }

        /// <summary>
        /// Получить дефолтный экземпляр
        /// </summary>
        /// <param name="NameOfType">Формат данных переменной, записанной в string-формате</param>
        /// <returns></returns>
        private static Tuple<dynamic, string> GetInstenceFromThisNamespace(string NameOfType)
        {
            Type SomeType = Type.GetType(NameOfType);
            dynamic Instance = CreateInstance(SomeType);

            return new Tuple<dynamic, string>(Instance, "");
        }

        #region Создание пустой переменной на основании её типа.
        /// <summary>
        /// Получить дефолтный экземпляр
        /// </summary>
        /// <param name="Item">Тип данных</param>
        /// <returns></returns>
        private static dynamic CreateInstance(Type Item)
        {
            try
            {
                if (Item == typeof(string))
                {
                    var sz = Activator.CreateInstance("".GetType(), "".ToCharArray());
                    return sz as string;
                }
                else if (Item == typeof(Dictionary<string, string>))
                {
                    Type d1 = typeof(Dictionary<,>);  //Creating the Dictionary Type.

                    Type[] typeArgs = { typeof(string), typeof(string) };  //Creating KeyValue Type for Dictionary.

                    Type makeme = d1.MakeGenericType(typeArgs); //Passing the Type and create Dictionary Type.

                    return Activator.CreateInstance(makeme);
                }
                else if (Item == typeof(Dictionary<string, int>))
                {
                    Type d1 = typeof(Dictionary<,>);  //Creating the Dictionary Type.

                    Type[] typeArgs = { typeof(string), typeof(int) };  //Creating KeyValue Type for Dictionary.

                    Type makeme = d1.MakeGenericType(typeArgs); //Passing the Type and create Dictionary Type.

                    return Activator.CreateInstance(makeme);
                }
                else
                    return Activator.CreateInstance(Item);
            }
            catch (Exception ex)
            {
                string except = ex.Message.ToString();
                return null;
            }
        }
        public static dynamic CreateInstance<T>()
        {
            try
            {
                if (typeof(T) == typeof(string))
                {
                    var sz = Activator.CreateInstance("".GetType(), "".ToCharArray());
                    return sz as string;
                }
                else if (typeof(T) == typeof(Dictionary<string, string>))
                {
                    Type d1 = typeof(Dictionary<,>);  //Creating the Dictionary Type.

                    Type[] typeArgs = { typeof(string), typeof(string) };  //Creating KeyValue Type for Dictionary.

                    Type makeme = d1.MakeGenericType(typeArgs); //Passing the Type and create Dictionary Type.

                    return Activator.CreateInstance(makeme);
                }
                else if (typeof(T) == typeof(Dictionary<string, int>))
                {
                    Type d1 = typeof(Dictionary<,>);  //Creating the Dictionary Type.

                    Type[] typeArgs = { typeof(string), typeof(int) };  //Creating KeyValue Type for Dictionary.

                    Type makeme = d1.MakeGenericType(typeArgs); //Passing the Type and create Dictionary Type.

                    return Activator.CreateInstance(makeme);
                }
                else return Activator.CreateInstance<T>();
            }
            catch (Exception ex)
            {
                string except = ex.Message.ToString();

                return default(T);
            }
        }
        #endregion

        #region Перегрузка метода ConvertFrom
        /// <summary>
        /// Конверт из DataSet to T[], T[] to DataSet
        /// </summary>
        /// <param name="SetItems"></param>
        /// <param name="ReturnItems"></param>
        /// <param name="instructionsForServer"></param>
        /// <returns></returns>
        public static DataSet ConvertFrom(List<dynamic> SetItems, List<dynamic> ReturnItems, string instructionsForServer)
        {
            DataSet ds = new DataSet();

            if (SetItems is null ? false : SetItems.Count > 0)
                foreach (dynamic setItem in SetItems.Where(x => !(x is null))) // Поступающие параметры
                    if (setItem.GetType() == typeof(List<dynamic>))
                        foreach (dynamic Item in setItem)
                        {
                            DataTable dt = new DataTable();

                            dt = CommonConvert(Item);

                            ds.Tables.Add(dt);
                        }
                    else
                        ds.Tables.Add(CommonConvert(setItem));

            if (ReturnItems is null ? false : ReturnItems.Count > 0)
                foreach (dynamic returnItem in ReturnItems) // Возвращаемые параметры
                    if (returnItem.GetType() == typeof(List<dynamic>))
                        foreach (dynamic Item in returnItem)
                            ds.Tables.Add(CommonConvert(Item));
                    else
                        ds.Tables.Add(CommonConvert(returnItem));

            ds.Tables.Add(CommonConvert(instructionsForServer)); // Добавили инструцкцию для сервера по обработке посланных данных      

            return ds; // Вернули DataSet
        }
        public static StructureValueForClient ConvertFrom(StructureValueForClient DefaultItem, DataTable dataTable) // DefaultItem - пустой экземпляр для динамической передачи типа/структуры данных 
        {
            return CommonConvert(DefaultItem, dataTable);
        }
        public static DataTable ConvertFrom(DataTable DefaultItem, DataTable dataTable) // DefaultItem - пустой экземпляр для динамической передачи типа/структуры данных 
        {
            return CommonConvert(DefaultItem, dataTable);
        }
        public static int ConvertFrom(int DefaultItem, DataTable dataTable) // DefaultItem - пустой экземпляр для динамической передачи типа/структуры данных 
        {
            return CommonConvert(DefaultItem, dataTable);
        }
        public static List<string> ConvertFrom(List<string> DefaultItem, DataTable dataTable) // DefaultItem - пустой экземпляр для динамической передачи типа/структуры данных 
        {
            return CommonConvert(DefaultItem, dataTable);
        }
        public static string ConvertFrom(string DefaultItem, DataTable dataTable)
        {
            return CommonConvert(DefaultItem, dataTable);
        }
        public static TimeSpan ConvertFrom(TimeSpan DefaultItem, DataTable dataTable)
        {
            return CommonConvert(DefaultItem, dataTable);
        }
        public static HashSet<string> ConvertFrom(HashSet<string> DefaultItem, DataTable dataTable)
        {
            return CommonConvert(DefaultItem, dataTable);
        }
        public static Dictionary<string, string> ConvertFrom(Dictionary<string, string> DefaultItem, DataTable dataTable)
        {
            return CommonConvert(DefaultItem, dataTable);
        }
        public static Dictionary<string, int> ConvertFrom(Dictionary<string, int> DefaultItem, DataTable dataTable)
        {
            return CommonConvert(DefaultItem, dataTable);
        }
        #endregion

        #region Перегрузка метода CommonConvert
        /// <summary>
        /// Конверт из DataTable to T, T to DataTable
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static DataTable CommonConvert(string item)
        {
            return stringToData(item);
        }
        #region StructureValueForClient - костыль
        private static DataTable CommonConvert(StructureValueForClient item)
        {
            return new DataTable();
        }
        private static StructureValueForClient CommonConvert(StructureValueForClient Item, DataTable dataTable)
        {
            return new StructureValueForClient();
        }
        #endregion

        private static DataTable CommonConvert(int item)
        {
            return stringToData(item.ToString());
        }
        private static DataTable CommonConvert(DataTable item)
        {
            return item;
        }
        private static DataTable CommonConvert(TimeSpan item)
        {
            string LikeAString = string.Format("{0:00}:{1:00}:{2:00}", (item.Days * 24) + item.Hours, item.Minutes, item.Seconds);
            return stringToData(LikeAString);
        }
        private static DataTable CommonConvert(Dictionary<string, int> item)
        {
            return ChangeDictionaryToDataTable(item);
        }
        private static DataTable CommonConvert(Dictionary<string, string> item)
        {
            return ChangeDictionaryToDataTable(item);
        }
        private static DataTable CommonConvert(List<string> item)
        {
            return ConvertListToDataTable(item);
        }
        private static DataTable CommonConvert(HashSet<string> item)
        {
            DataTable dtt = ConvertHashSetToDataTable(item);

            return dtt;
        }
        private static DataTable CommonConvert<T>(this IEnumerable<T> item) where T : class
        {
            return AsDataTable(item); // Конверт листа;
        }
        private static TimeSpan CommonConvert(TimeSpan Item, DataTable dataTable)
        {
            dynamic t = dataTable.Rows[0][0];
            TimeSpan ts = FormatStringAndTimeSpan.Str2TimeSpan(t);

            return ts;
        }
        private static DataTable CommonConvert(DataTable item, DataTable dataTable)
        {
            return dataTable;
        }
        private static string CommonConvert(string Item, DataTable dataTable)
        {
            dynamic t = dataTable.Rows[0][0];

            return t;
        }
        private static int CommonConvert(int Item, DataTable dataTable)
        {
            dynamic t = dataTable.Rows[0][0];

            return Int32.Parse(t.ToString());
        }
        private static Dictionary<string, T> CommonConvert<T>(Dictionary<string, T> Item, DataTable dataTable)
        {
            dynamic t = GetDict<T>(dataTable);

            return t;
        }
        private static List<string> CommonConvert(List<string> Item, DataTable dataTable)
        {
            return AddToList<string>(dataTable);
        }
        private static HashSet<string> CommonConvert(HashSet<string> Item, DataTable dataTable)
        {
            IEnumerable<string> values =
               dataTable
               .Rows
               .Cast<DataRow>()
               //.Select(row => row["ColumnName"])
               .Select(row => row[dataTable.Columns[0].ColumnName])
               .Cast<string>();
            HashSet<string> hashSet = new HashSet<string>(values);
            return hashSet;
        }
        #endregion

        #region Вспомогательные конверторы по разным типам данных
        public static List<T> BindList<T>(DataTable dt)
        {
            try
            {
                // Example 1:
                // Get private fields + non properties
                //var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                // Example 2: Your case
                // Get all public fields
                var fields = typeof(T).GetFields();

                List<T> lst = new List<T>();

                foreach (DataRow dr in dt.Rows)
                {
                    // Create the object of T
                    var ob = Activator.CreateInstance<T>();

                    foreach (var fieldInfo in fields)
                        foreach (DataColumn dc in dt.Columns)
                            if (fieldInfo.Name == dc.ColumnName) // Matching the columns with fields
                            {
                                // Get the value from the datatable cell
                                object value = dr[dc.ColumnName];

                                // Set the value into the object
                                fieldInfo.SetValue(ob, value);
                                break;
                            }

                    lst.Add(ob);
                }

                return lst;
            }
            catch
            {
                return BindList<T>(dt);
            }
        }
        static DataTable ConvertHashSetToDataTable(HashSet<string> list)
        {
            try
            {
                // New table.
                DataTable table = new DataTable();

                // Get max columns.
                int columns = 0;

                foreach (var array in list)
                    if (array.Length > columns)
                        columns = array.Length;

                // Add columns.
                for (int i = 0; i < columns; i++)
                    table.Columns.Add();

                // Add rows.
                foreach (var array in list)
                    table.Rows.Add(array);

                return table;
            }
            catch
            {
                return ConvertHashSetToDataTable(list);
            }
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            try
            {
                DataTable dt = new DataTable();
                Type t = typeof(T);
                PropertyInfo[] pia = t.GetProperties();

                foreach (PropertyInfo pi in pia)   //Create the columns in the DataTable
                    dt.Columns.Add(pi.Name, pi.PropertyType);

                foreach (T item in collection)   //Populate the table
                {
                    DataRow dr = dt.NewRow();
                    dr.BeginEdit();

                    foreach (PropertyInfo pi in pia)
                    {
                        if (!(pi.GetIndexParameters().Length == 0))
                            dr[pi.Name] = pi.GetValue(item, new object[] { 0, 1, 2 });
                        // dr[pi.Name] = pi.GetValue(item, returnIndexes(item.ToString().Length));
                    }

                    dr.EndEdit();
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch
            {
                return collection.ToDataTable<T>();
            }
        }
        private static DataTable HashSetToDataTable<T>(HashSet<T> items)
        {
            try
            {
                DataTable tb = new DataTable();
                Type t = typeof(T).ReflectedType;
                PropertyInfo prop = t.GetProperties()[0];
                tb.Columns.Add(prop.Name, t);

                foreach (T item in items)
                {
                    var values = prop.GetValue(item, null);
                    tb.Rows.Add(values);
                }

                return tb;
            }
            catch
            {
                return HashSetToDataTable<T>(items);
            }
        }
        private static Dictionary<string, T> GetDict<T>(DataTable dt)
        {
            try
            {
                Dictionary<String, T> dic = dt.AsEnumerable().ToDictionary(row => row.Field<String>(0), row => row.Field<T>(1));

                return dic;
            }
            catch
            {
                return GetDict<T>(dt);
            }
        }
        static DataTable ChangeDictionaryToDataTable<T>(IDictionary<string, T> dict)
        {
            try
            {
                using (DataTable dataTable = new DataTable())
                {
                    if (dict.Count > 0)
                    {
                        dataTable.Columns.Add("CategoryID", typeof(string));
                        dataTable.Columns.Add("CategoryName", typeof(T));

                        foreach (KeyValuePair<string, T> category in dict)
                            dataTable.Rows.Add(category.Key, category.Value);
                    }
                    return dataTable;
                }
            }
            catch
            {
                return ChangeDictionaryToDataTable(dict);
            }
        }
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection properties =
                 TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();

                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }

                return table;
            }
            catch
            {
                return ConvertToDataTable(data);
            }
        }
        private static T ToObject<T>(this DataRow dataRow) where T : new() // Из строки в объект
        {
            try
            {
                T item = new T();

                foreach (DataColumn column in dataRow.Table.Columns)
                {
                    PropertyInfo property = item.GetType().GetProperty(column.ColumnName);

                    if (property != null && dataRow[column] != DBNull.Value)
                    {
                        object result = Convert.ChangeType(dataRow[column], property.PropertyType);
                        property.SetValue(item, result, null);
                    }
                }

                return item;
            }
            catch
            {
                return dataRow.ToObject<T>();
            }
        }
        public static DataTable AsDataTable<T>(this IEnumerable<T> list) where T : class
        {
            try
            {
                DataTable dtOutput = new DataTable();

                //if the list is empty, return empty data table
                if (list.Count() == 0)
                    return dtOutput;

                //get the list of  public properties and add them as columns to the
                //output table           
                PropertyInfo[] properties = list.FirstOrDefault().GetType().
                    GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo propertyInfo in properties)
                    dtOutput.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);

                //populate rows
                DataRow dr;
                //iterate through all the objects in the list and add them
                //as rows to the table
                foreach (T t in list)
                {
                    dr = dtOutput.NewRow();
                    //iterate through all the properties of the current object
                    //and set their values to data row
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        dr[propertyInfo.Name] = propertyInfo.GetValue(t, null);
                    }
                    dtOutput.Rows.Add(dr);
                }

                return dtOutput;
            }
            catch
            {
                return list.AsDataTable<T>();
            }
        }
        private static DataTable ConvertListToDataTable<T>(List<T> list)
        {
            try
            {
                // New table.
                DataTable table = new DataTable();

                table.Columns.Add();

                // Add rows.
                foreach (var array in list)
                    table.Rows.Add(array);

                return table;
            }
            catch
            {
                return ConvertListToDataTable<T>(list);
            }
        }
        private static List<T> AddToList<T>(DataTable dtData)
        {
            try
            {
                List<string> str = new List<string>();

                foreach (DataRow row in dtData.Rows)
                    foreach (DataColumn Col in dtData.Columns)
                        str.Add(row[Col].ToString());

                return str as List<T>;
            }
            catch
            {
                return AddToList<T>(dtData);
            }
        }
        private static List<T> ToListof<T>(this DataTable dt)
        {
            try
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    ;
                var objectProperties = typeof(T).GetProperties(flags);
                var targetList = dt.AsEnumerable().Select(dataRow =>
                {
                    T instanceOfT;

                    if (typeof(T) == typeof(string))
                        instanceOfT = (T)Activator.CreateInstance("".GetType(), "".ToCharArray());
                    else
                        instanceOfT = (T)Activator.CreateInstance<T>();

                    try
                    {
                        foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                            properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                    }
                    catch (Exception ex)
                    {
                        ConsoleWriteLine.WriteInConsole(nameof(ToListof), "", "Failed", ex.ToString(), default);
                    }

                    return instanceOfT;

                }).ToList();

                return targetList;
            }
            catch
            {
                return dt.ToListof<T>();
            }
        }
        private static List<T> DataTableToList<T>(this DataTable table) where T : new()
        {
            try
            {
                List<T> list = new List<T>();
                var typeProperties = typeof(T).GetProperties().Select(propertyInfo => new
                {
                    PropertyInfo = propertyInfo,
                    Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
                });

                foreach (var row in table.Rows.Cast<DataRow>())
                {
                    T obj = new T();

                    foreach (var typeProperty in typeProperties)
                    {
                        object value = row[typeProperty.PropertyInfo.Name];
                        object safeValue = value == null || DBNull.Value.Equals(value)
                            ? null
                            : Convert.ChangeType(value, typeProperty.Type);

                        typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
                    }
                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return table.DataTableToList<T>();
            }
        }
        private static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            try
            {
                Type type = typeof(T);
                var properties = type.GetProperties();

                DataTable dataTable = new DataTable();

                foreach (PropertyInfo info in properties)
                    dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));

                foreach (T entity in list)
                {
                    object[] values = new object[properties.Length];

                    for (int i = 0; i < properties.Length; i++)
                        values[i] = properties[i].GetValue(entity, null);

                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
            catch
            {
                return CreateDataTable<T>(list);
            }
        }
        private static DataTable stringToData(string SomeString)
        {
            try
            {
                DataTable dt = new DataTable();
                string columnName = SomeString.GetType().GetProperties()[0].Name;
                dt.Columns.Add(columnName);
                DataRow dr = dt.NewRow();
                dr[columnName] = SomeString;
                dt.Rows.Add(dr);

                return dt;
            }
            catch
            {
                return stringToData(SomeString);
            }
        }
        private static List<T> ToList<T>(this DataTable table) where T : new() // из таблицы в лист, вспомогательная
        {
            try
            {
                IList<PropertyInfo> properties = typeof(T).GetProperties();
                List<T> result = new List<T>();

                foreach (var row in table.Rows)
                {
                    var item = CreateItemFromRow<T>((DataRow)row, properties);
                    result.Add(item);
                }

                return result;
            }
            catch
            {
                return table.ToList<T>();
            }
        }
        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new() // Вспомогательная для сбора таблицы в лист
        {
            try
            {
                T item = new T();

                foreach (var property in properties)
                    if (property.PropertyType == typeof(System.DayOfWeek))
                    {
                        DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                        property.SetValue(item, day, null);
                    }
                    else
                    {
                        if (row[property.Name] == DBNull.Value)
                            property.SetValue(item, null, null);
                        else
                            property.SetValue(item, row[property.Name], null);
                    }

                return item;
            }
            catch
            {
                return CreateItemFromRow<T>(row, properties);
            }
        }
        #endregion        
    }
}
