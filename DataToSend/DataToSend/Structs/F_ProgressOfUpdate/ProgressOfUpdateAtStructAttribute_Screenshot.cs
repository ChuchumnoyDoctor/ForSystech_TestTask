using System;
using System.Collections.Generic;

namespace CommonDll.Structs.F_ProgressOfUpdate
{
    [Serializable]
    public class ProgressOfUpdateAtStructAttribute_Screenshot : ProgressOfUpdateAtStructAttribute
    {
        public override string ExceptionMessage { get; set; }
        public override DateTime LastChange_DateTime { get; set; }
        public override TimeSpan TimeSpend { get; internal set; }
        public override string Status { get; private protected set; }
        public override int Progress { get; private protected set; }
        public static List<ProgressOfUpdateAtStructAttribute> CreateScreenShot(List<ProgressOfUpdateAtStructAttribute> Submodules, bool ToAllSubmodulesToScreenshot)
        {
            if (Submodules.Count > 0)
            {
                List<ProgressOfUpdateAtStructAttribute> Returned = new List<ProgressOfUpdateAtStructAttribute>();

                foreach (var r in Submodules)
                    Returned.Add(CreateScreenShot(r, ToAllSubmodulesToScreenshot));

                return Returned;
            }
            else
                return null;
        }
        public static ProgressOfUpdateAtStructAttribute CreateScreenShot(ProgressOfUpdateAtStructAttribute Submodule, bool ToAllSubmodulesToScreenshot)
        {
            if (Submodule is null)
                return null;
            else
                return new ProgressOfUpdateAtStructAttribute_Screenshot()
                {
                    Name = Submodule.Name,
                    Name_RussianEquivalent = Submodule.Name_RussianEquivalent,
                    ExceptionMessage = Submodule.ExceptionMessage,
                    Submodules = ToAllSubmodulesToScreenshot ? CreateScreenShot(Submodule.Submodules_Get(), ToAllSubmodulesToScreenshot) : Submodule.Submodules_Get(),
                    LastChange_DateTime = Submodule.LastChange_DateTime,
                    TimeSpend = Submodule.TimeSpend,
                    Status = Submodule.Status,
                    Progress = Submodule.Progress,
                    NonSerializedConfig = Submodule.NonSerializedConfig,
                    Parent = Submodule.Parent,
                    Count = Submodule.Count
                };
        }
    }
}
