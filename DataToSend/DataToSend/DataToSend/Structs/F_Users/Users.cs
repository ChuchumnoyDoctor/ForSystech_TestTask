using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDll.Structs.F_Users
{
    [Serializable]
    public class Users : MainParentClass
    {
        #region override basement changes
        internal protected override MainParentClass[] Base_Childs
        {
            get
            {
                return User_s.Cast<MainParentClass>().ToArray();
            }
            set
            {
                User_s = value is null ? null : value.Where(x => x is User).Cast<User>().ToList();
            }
        }
        private protected override MainParentClass Base_UpdateObject_Original { get { return default; } set { } }
        public override string Base_Name { get { return ClassName; } }

        #region Server Export Part
        [field: NonSerialized()]
        public override KeyValuePair<string, DbParameter[]> Select { get; set; }
        public override string Select_WHERE { get { return ""; } }

        [field: NonSerialized()]
        public override KeyValuePair<string, DbParameter[]> Select_FullMatch { get; set; }

        [field: NonSerialized()]
        internal protected override KeyValuePair<string, DbParameter[]> Insert { get; set; }

        [field: NonSerialized()]
        internal protected override string Update { get; set; }

        [field: NonSerialized()]
        internal protected override string Delete { get; set; }
        #endregion
        #endregion
        private List<User> users;
        public List<User> User_s
        {
            get
            {
                if (users is null)
                    users = new List<User>();

                if (!(users is null))
                    foreach (var t in users)
                        t.Parent = this;

                return users;
            }
            set
            {
                users = value;
            }
        }
        public override object ClonePart()
        {
            return new Users
            {
                User_s = this.User_s is null ? null : this.User_s.Select(x => (User)x.Clone()).ToList(),
            };
        }
    }
}
