using CommonDll.Client_Server.Server;
using CommonDll.Structs;
using CommonDll.Structs.F_ProgressOfUpdate;
using System;
using System.Threading.Tasks;

namespace TasksManager_Server.Methods.MethodsProcess
{
    public class ListenSockets : ParentMethods
    {

        public override MainParentClass Menu_Virtual(ProgressOfUpdateAtStructAttribute Parent)
        {
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(ListenSockets));
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                Task.Run(() =>
                {
                    ConnectFromServer.StartListening(typeof(MethodsCall).AssemblyQualifiedName, 11000);
                });
            });
            Progress.Start();

            return default;
        }
        #region Timer   
        public override bool Timer_Elapsed_ToStartAgain()
        {
            return true;
        }
        #endregion
    }
}
