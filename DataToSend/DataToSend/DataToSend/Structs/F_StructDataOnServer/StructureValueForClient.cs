using CommonDll.Client_Server;
using CommonDll.Structs.F_HR;
using CommonDll.Structs.F_ProgressOfUpdate;
using CommonDll.Structs.F_Users;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace CommonDll.Structs.F_StructDataOnServer
{
    [Serializable]
    public class StructureValueForClient : MainParentClass
    {
        #region override basement changes
        internal protected override MainParentClass[] Base_Childs
        {
            get
            {
                return default;
            }
            set
            {

            }
        }
        private protected override MainParentClass Base_UpdateObject_Original { get { return default; } set { } }
        public override string Base_Name { get { return default; } }

        #region Server Export Part
        [field: NonSerialized()]
        public override KeyValuePair<string, DbParameter[]> Select { get; set; }
        public override string Select_WHERE { get { return ""; } }

        [field: NonSerialized()]
        public override KeyValuePair<string, DbParameter[]> Select_FullMatch { get; set; } // Переопределить

        [field: NonSerialized()]
        internal protected override KeyValuePair<string, DbParameter[]> Insert { get; set; }

        [field: NonSerialized()]
        internal protected override string Update { get; set; }

        [field: NonSerialized()]
        internal protected override string Delete { get; set; }

        #endregion

        public override object ClonePart()
        {
            throw new NotImplementedException();
        }
        #endregion

        public static bool IsServer { get; set; } = false;  // то, что происходит на сервере, но не должно срабатывать на клиенте.

        #region HR
        public DateTime updateTime_Module_HR;
        public DateTime UpdateTime_Module_HR
        {
            get
            {
                return updateTime_Module_HR;
            }
            set
            {
                updateTime_Module_HR = value;

                if (IsServer)
                    if (HR.Workers_Hierarchy.Count > 0)
                        DataToBinary.SerializeObject_ToFile(HR, nameof(HR), out TimeSpan SpendOnIt_MainPlanModule);
            }
        }

        private HR hR;
        public HR HR
        {
            get
            {
                if (hR is null)
                    hR = new HR();

                if (!(hR is null))
                    hR.Parent = this;

                return hR;
            }
            set
            {
                hR = value;
            }
        }

        [field: NonSerialized()]
        public object HR_LockObject { get; set; } = new object();
        #endregion

        #region Users
        public DateTime updateTime_Module_Users;
        public DateTime UpdateTime_Module_Users
        {
            get
            {
                return updateTime_Module_Users;
            }
            set
            {
                updateTime_Module_Users = value;

                if (IsServer)
                    if (Users.User_s.Count > 0)
                        DataToBinary.SerializeObject_ToFile(Users, nameof(Users), out TimeSpan SpendOnIt_MainPlanModule);
            }
        }

        private Users users;
        public Users Users
        {
            get
            {
                if (users is null)
                    users = new Users();

                if (!(users is null))
                    users.Parent = this;

                return users;
            }
            set
            {
                users = value;
            }
        }

        [field: NonSerialized()]
        public object Users_LockObject { get; set; } = new object();
        #endregion

        #region ProgressOfUpdateAtStruct: Update structs
        public DateTime updateTime_Module_ProgressOfUpdate;
        public DateTime UpdateTime_Module_ProgressOfUpdate
        {
            get
            {
                return updateTime_Module_ProgressOfUpdate;
            }
            set
            {
                updateTime_Module_ProgressOfUpdate = value;
            }
        }
        public ProgressOfUpdate ProgressOfUpdate { get; set; }

        [field: NonSerialized()]
        public object ProgressOfUpdate_LockObject { get; set; } = new object();
        #endregion

        #region AutoRefresh
        public Dictionary<string, DateTime> CheckProperties_With_TimeLastUpdate { get; set; }
        public TimeSpan TimeServerOnGetProperties { get; set; }
        public TimeSpan TimeServerWhenGetProperties { get; set; }
        #endregion
    }
}
