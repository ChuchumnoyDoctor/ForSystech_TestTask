using CommonDll.Structs.F_LogFile.F_LogRecord;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CommonDll.Structs.F_HR
{
    [Serializable]
    public class HR : MainParentClass
    {
        #region override basement changes
        internal protected override MainParentClass[] Base_Childs
        {
            get
            {
                return Workers_Hierarchy.Cast<MainParentClass>().Concat(Groups).ToArray();
            }
            set
            {
                Workers_Hierarchy = value is null ? null : value.Where(x => x is HR_Worker).Cast<HR_Worker>().ToList();
                Groups = value is null ? null : value.Where(x => x is HR_Group).Cast<HR_Group>().ToList();
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

        [field: NonSerialized]
        private List<HR_Worker> workers_All;
        public List<HR_Worker> Workers_All
        {
            get
            {
                if (workers_All is null ? true : workers_All.Count == 0)
                {
                    workers_All = new List<HR_Worker>();

                    foreach (var Work in Workers_Hierarchy)
                    {
                        if (workers_All.FirstOrDefault(x => x.Base_Name == Work.Base_Name) is null)
                            workers_All.Add(Work);

                        workers_All = GetAllCollection(Work, workers_All);
                    }

                    workers_All.Sort((a, b) => a.Group.Name.CompareTo(b.Group.Name));
                }

                return workers_All;
            }
        }
        private List<HR_Worker> GetAllCollection(HR_Worker Worker, List<HR_Worker> Workers)
        {
            foreach (var Work in Worker.SubWorkers)
            {
                if (Workers.FirstOrDefault(x => x.Base_Name == Work.Base_Name) is null)
                {
                    HR_Worker HR_Worker = (HR_Worker)Work.Clone();
                    HR_Worker.Parent = Work.Parent;
                    Workers.Add(HR_Worker);
                }

                GetAllCollection(Work, Workers);
            }

            return Workers;
        }
        private List<HR_Worker> workers_Hierarchy;
        public List<HR_Worker> Workers_Hierarchy
        {
            get
            {
                if (workers_Hierarchy is null)
                    workers_Hierarchy = new List<HR_Worker>();

                if (!(workers_Hierarchy is null))
                    foreach (var t in workers_Hierarchy)
                        t.Parent = this;

                return workers_Hierarchy;
            }
            set
            {
                workers_Hierarchy = value;
            }
        }

        private List<HR_Group> groups;
        public List<HR_Group> Groups
        {
            get
            {
                if (groups is null)
                    groups = new List<HR_Group>();

                if (!(groups is null))
                    foreach (var t in groups)
                        t.Parent = this;

                return groups;
            }
            set
            {
                groups = value;
            }
        }
        public float PaymentPeriod(DateTime Start, DateTime End)
        {
            return Workers_All.Select(x => x.PaymentPeriod(Start, End)).Sum();
        }
        public override object ClonePart()
        {
            return new HR
            {
                Workers_Hierarchy = this.Workers_Hierarchy is null ? null : this.Workers_Hierarchy.Select(x => (HR_Worker)x.Clone()).ToList(),
                Groups = this.Groups is null ? null : this.Groups.Select(x => (HR_Group)x.Clone()).ToList(),
            };
        }
    }
}
