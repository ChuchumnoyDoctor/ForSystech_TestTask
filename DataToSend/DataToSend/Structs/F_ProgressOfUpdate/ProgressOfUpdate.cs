using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CommonDll.Structs.F_ProgressOfUpdate
{
    [Serializable]
    public class ProgressOfUpdate : MainParentClass
    {
        #region override basement changes
        internal protected override MainParentClass[] Base_Childs
        {
            get
            {
                return List_ProgressOfUpdateAtStructAttribute.ToArray();
            }
            set
            {
                List_ProgressOfUpdateAtStructAttribute = value is null ? null : value.Where(x => x is ProgressOfUpdateAtStructAttribute).Cast<ProgressOfUpdateAtStructAttribute>().ToList();
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
        public override object ClonePart()
        {
            return new ProgressOfUpdate
            {
                List_ProgressOfUpdateAtStructAttribute = this.List_ProgressOfUpdateAtStructAttribute is null ? new List<ProgressOfUpdateAtStructAttribute>() : this.List_ProgressOfUpdateAtStructAttribute.Select(x => (ProgressOfUpdateAtStructAttribute)x.Clone()).ToList(),
            };
        }
        public List<ProgressOfUpdateAtStructAttribute> List_ProgressOfUpdateAtStructAttribute { get; set; }
    }
}
