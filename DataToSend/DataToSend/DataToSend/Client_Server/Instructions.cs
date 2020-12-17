using CommonDll.Helps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CommonDll.Client_Server
{
    /// <summary>
    /// Конвертация входных параметров в string-строку и наоборот
    /// </summary>
    public static class Instructions
    {
        /// <summary>
        /// Возвращает тип параметра в виде строкового формата
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Item"></param>
        /// <returns></returns>
        private static string returnType<T>(T Item)
        {
            return Item.GetType().AssemblyQualifiedName.ToString();
        }
        /// <summary>
        /// Создание инструкции
        /// </summary>
        /// <param name="SetItems"></param>
        /// <param name="ReturnItems"></param>
        /// <param name="NameOfMethod"></param>
        /// <param name="Parent"></param>
        /// <returns></returns>

        public static string CreateInstructions(List<dynamic> SetItems, List<dynamic> ReturnItems, string NameOfMethod) // Передаваемые параметры и название метода, что будет их использовать.
        {
            string instructions = "";
            {
                instructions = instructions + NameOfMethod + ";"; //  ; - делимитер, с помощью него буду парсить инструкцию

                foreach (dynamic setItem in SetItems.Where(x => !(x is null))) // Поступающие параметры
                    if (setItem.GetType() == typeof(List<dynamic>))
                    {
                        instructions = instructions + "<Start: List <dynamic>>" + ";";

                        foreach (dynamic Item in setItem)
                            instructions = instructions + returnType(Item) + ";";

                        instructions = instructions + "<End: List <dynamic>>" + ";";
                    }
                    else
                        instructions = instructions + returnType(setItem) + ";";

                instructions = instructions + SetItems.Count + ";"; // Количество поступающим параметров
                instructions = instructions + ReturnItems.Count + ";"; // Количество возвращаемых параметров 

                foreach (dynamic returnItem in ReturnItems) // Возвращаемые параметры
                    if (returnItem.GetType() == typeof(List<dynamic>))
                    {
                        instructions = instructions + "<Start: List <dynamic>>" + ";";

                        foreach (dynamic Item in returnItem)
                            instructions = instructions + returnType(Item) + ";";

                        instructions = instructions + "<End: List <dynamic>>" + ";";
                    }
                    else
                        instructions = instructions + returnType(returnItem) + ";";

                instructions = instructions.Substring(0, instructions.Length - 1);
            }

            return instructions;
        }
        /// <summary>
        /// Чтение инструкции
        /// </summary>
        /// <param name="dataSetWithInstruction"></param>
        /// <returns></returns>
        public static Tuple<List<dynamic>, List<dynamic>, string> ReadInstructions(DataSet dataSetWithInstruction)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = dataSetWithInstruction.Tables[dataSetWithInstruction.Tables.Count - 1];
            }
            catch (Exception ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);

                return new Tuple<List<dynamic>, List<dynamic>, string>(new List<dynamic>(), new List<dynamic>(), "");
            }

            string[] datatableStrings = (dt.Rows[0][0].ToString()).Split(';');
            List<dynamic> ItemsSet = new List<dynamic>();
            List<dynamic> ItemsReturn = new List<dynamic>();
            bool isReturnItem = false;
            int NumbItemReturns = 0;
            string NameOfCallMethod = "";
            bool isNameOfCallMethod = true;
            List<string> dynamics = new List<string>();
            bool isListDynamic = false;

            foreach (string parameter in datatableStrings)
            {
                if (isNameOfCallMethod)
                {
                    NameOfCallMethod = parameter;
                    isNameOfCallMethod = false;
                }
                else
                {
                    if (parameter == "<Start: List <dynamic>>")
                    {
                        isListDynamic = true;
                    }
                    else if (parameter == "<End: List <dynamic>>")
                    {
                        isListDynamic = false;

                        if (isReturnItem) // В первую очередь считываются поступающие параметры
                            ItemsReturn.Add(dynamics); // Параметры на возврат
                        else ItemsSet.Add(dynamics); // Поступающие параметры

                        dynamics = new List<string>();
                    }
                    else if (isListDynamic)
                        dynamics.Add(parameter);
                    else if (Int32.TryParse(parameter, out NumbItemReturns))
                        isReturnItem = true;
                    else
                    {
                        if (isReturnItem) // В первую очередь считываются поступающие параметры
                            ItemsReturn.Add(parameter); // Параметры на возврат
                        else
                            ItemsSet.Add(parameter); // Поступающие параметры
                    }
                }
            }
            return new Tuple<List<dynamic>, List<dynamic>, string>(ItemsSet, ItemsReturn, NameOfCallMethod);
        }
    }
}
