using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommonDll.Client_Server.Server
{
    public static class AutoRefresh_ServerPart
    {
        /// <summary>
        /// Модуль автообновления данных клиента. Серверная часть
        /// </summary>   
        public static StructureValueForClient CheckAutoRefresh(StructureValueForClient StructureValueForClient)
        {
            Dictionary<string, DateTime> CheckProperties_With_TimeLastUpdate = StructureValueForClient.CheckProperties_With_TimeLastUpdate;
            Dictionary<string, DateTime> Return_CheckProperties_With_TimeLastUpdate = new Dictionary<string, DateTime>();

            foreach (var name in CheckProperties_With_TimeLastUpdate)
            {
                DateTime valueNameOfTimeUpdated = default(DateTime);
                PropertyInfo propertiNameOfTimeUpdated = default(PropertyInfo);
                propertiNameOfTimeUpdated = default(PropertyInfo);
                string NameOfTimeUpdated = "UpdateTime_Module_" + name.Key;
                propertiNameOfTimeUpdated = ValueForClient.ReadyStructure.GetType().GetProperty(NameOfTimeUpdated);
                valueNameOfTimeUpdated = propertiNameOfTimeUpdated is null ? default(DateTime) : (DateTime)propertiNameOfTimeUpdated.GetValue(ValueForClient.ReadyStructure);

                if (valueNameOfTimeUpdated < name.Value)
                    valueNameOfTimeUpdated = propertiNameOfTimeUpdated is null ? new DateTime() : (DateTime)propertiNameOfTimeUpdated.GetValue(ValueForClient.ReadyStructure);

                Return_CheckProperties_With_TimeLastUpdate.Add(name.Key, valueNameOfTimeUpdated);
            }

            return new StructureValueForClient() { CheckProperties_With_TimeLastUpdate = Return_CheckProperties_With_TimeLastUpdate }; // Return name of modules
        }

        /// <summary>
        /// Получить параметры по имени параметров
        /// </summary> 
        public static StructureValueForClient GetParamatersForClientReady(List<string> ListNameOfValue) // Для запроса по нажатию, получает уже готовый результат
        {
            StructureValueForClient StructureReturn = new StructureValueForClient();
            PropertyInfo propertyStructure = typeof(ValueForClient).GetProperty("ReadyStructure"); // Получил структуру
            StructureValueForClient Structure = (StructureValueForClient)propertyStructure.GetValue(propertyStructure.Name); // Получил значение структуры  

            foreach (string name in ListNameOfValue)
            {
                PropertyInfo property = Structure.GetType().GetProperty(name);

                if (property != null)
                    property.SetValue(StructureReturn, property.GetValue(Structure));

                string UpdateTime_Module_ = "updateTime_Module_";
                var updateTime_Module_property = Structure.GetType().GetField(UpdateTime_Module_ + name);

                if (updateTime_Module_property != null)
                    updateTime_Module_property.SetValue(StructureReturn, updateTime_Module_property.GetValue(Structure));
            }

            return StructureReturn;
        }
    }
}
