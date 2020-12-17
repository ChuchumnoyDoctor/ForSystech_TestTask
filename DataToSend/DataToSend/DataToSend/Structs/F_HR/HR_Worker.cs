using CommonDll.Client_Server;
using CommonDll.Structs.F_LogFile.F_LogRecord;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CommonDll.Structs.F_HR
{
    [Serializable]
    public class HR_Worker : MainParentClass, IUpdateable_Helper<HR_Worker>
    {
        #region override basement changes
        internal protected override MainParentClass[] Base_Childs
        {
            get
            {
                return SubWorkers.ToArray();
            }
            set
            {
                SubWorkers = value is null ? null : value.Where(x => x is HR_Worker).Cast<HR_Worker>().ToList();
            }
        }
        private protected override MainParentClass Base_UpdateObject_Original { get { return UpdateObject_Original; } set { UpdateObject_Original = (HR_Worker)value; } }
        public override string Base_Name { get { return Name; } }

        #region Server Export Part
        [field: NonSerialized()]
        KeyValuePair<string, DbParameter[]> select;
        public override KeyValuePair<string, DbParameter[]> Select
        {
            get
            {
                if (select.Key is null || select.Value is null)
                {
                    return new KeyValuePair<string, DbParameter[]>
                          ("SELECT Count(*) FROM [HR_Worker] " +
                          Select_WHERE,
                          new DbParameter[]
                          {
                              ConnectToDataBase.GetParammeter("@Name", Name, DbConnection),
                          });
                }

                return select;
            }
            set
            {
                select = value;
            }
        }
        public override string Select_WHERE { get { return "WHERE [Name] = @Name"; } }

        [field: NonSerialized()]
        KeyValuePair<string, DbParameter[]> select_FullMatch;
        public override KeyValuePair<string, DbParameter[]> Select_FullMatch
        {
            get
            {
                if (select_FullMatch.Key is null || select_FullMatch.Value is null)
                {
                    string s1 = string.Join(" AND ", Insert.Value.Where(x =>
                    !x.ParameterName.Contains("@FkId_Group") &
                    !x.ParameterName.Contains("@FkId_Chief")).Select(x => string.Format("[{0}] = @{0}", x.ParameterName.Replace("@", ""))));

                    return new KeyValuePair<string, DbParameter[]>
                               (string.Format("SELECT Count(*) FROM [HR_Worker] WHERE {0} AND [FkId_Group] = (SELECT [Id] FROM [HR_Group] WHERE [Name] == @FkId_Group) " +
                               " AND [FkId_Chief] = (SELECT [Id] FROM [HR_Worker] WHERE [Name] == @FkId_Chief)", s1),
                               insert.Value);
                }

                return select_FullMatch;
            }
            set
            {
                select_FullMatch = value;
            }
        }

        [field: NonSerialized()]
        KeyValuePair<string, DbParameter[]> insert;
        internal protected override KeyValuePair<string, DbParameter[]> Insert
        {
            get
            {
                if (insert.Key is null || insert.Value is null)
                {
                    var Value = new DbParameter[]
                    {
                        ConnectToDataBase.GetParammeter("@" + nameof(Name), Name, DbConnection),
                        ConnectToDataBase.GetParammeter("@" + nameof(EnrollmentDate), EnrollmentDate, DbConnection),
                        ConnectToDataBase.GetParammeter("@" + nameof(BaseRate), BaseRate, DbConnection),
                        ConnectToDataBase.GetParammeter("@" + nameof(EnrollmentDate), EnrollmentDate, DbConnection),
                    };

                    string s1 = string.Join(", ", Value.Select(x => string.Format("[{0}]", x.ParameterName.Replace("@", ""))));
                    string s2 = string.Join(", ", Value.Select(x => string.Format("@{0}", x.ParameterName.Replace("@", ""))));

                    Value = Value.Concat(new DbParameter[]
                       {
                            ConnectToDataBase.GetParammeter("@FkId_Group", Group.Name, DbConnection),
                            ConnectToDataBase.GetParammeter("@FkId_Chief", Parent is null ? null : ((HR_Worker)Parent).Name, DbConnection),
                       }).ToArray();

                    return new KeyValuePair<string, DbParameter[]>
                          (string.Format("INSERT INTO [HR_Worker] ({0},[FkId_Group],[FkId_Chief]) VALUES ({1},(SELECT [Id] FROM [HR_Group] WHERE [Name] == @FkId_Group)," +
                          "(SELECT [Id] FROM [HR_Worker] WHERE [Name] == @FkId_Chief))", s1, s2),
                          Value);
                }

                return insert;
            }
            set
            {
                insert = value;
            }
        }

        [field: NonSerialized()]
        string update_Query;
        internal protected override string Update
        {
            get
            {
                if (update_Query is null)
                {
                    string s1 = string.Join(", ", Insert.Value.Where(x =>
                    !x.ParameterName.Contains("@FkId_Group") &
                    !x.ParameterName.Contains("@FkId_Chief")).Select(x => string.Format("[{0}] = @{0}", x.ParameterName.Replace("@", ""))));

                    return string.Format("UPDATE [HR_Worker] SET {1}, " +
                        "[FkId_Group] = (SELECT [Id] FROM [HR_Group] WHERE [Name] == @{0}FkId_Group), " +
                        "[FkId_Chief] = (SELECT [Id] FROM [HR_Worker] WHERE [Name] == @{0}FkId_Chief) " +
                        "WHERE [Name] = @{0}Name", WHERE_Reference, s1);
                }

                return update_Query;
            }
            set
            {
                update_Query = value;
            }
        }

        [field: NonSerialized()]
        string delete;
        internal protected override string Delete
        {
            get
            {
                if (delete is null)
                    return "DELETE FROM [HR_Worker] " +
                        Select_WHERE;

                return delete;
            }
            set
            {
                delete = value;
            }
        }
        #endregion
        #endregion

        public string Name { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public float BaseRate { get; set; }
        private int Years(DateTime start, DateTime end)
        {
            return (end.Year - start.Year - 1) +
                (((end.Month > start.Month) ||
                ((end.Month == start.Month) && (end.Day >= start.Day))) ? 1 : 0);
        }
        public int CurrentPercent(DateTime PaydayDate)
        {
            int PercentGroup = default;
            int Years = EnrollmentDate.Date > PaydayDate.Date ? default : this.Years(EnrollmentDate.Date, PaydayDate.Date);

            if (!(Group is null))
            {
                PercentGroup = Group.Percent * Years;

                if (PercentGroup > Group.MaxPercent)
                    PercentGroup = Group.MaxPercent;
            }

            return PercentGroup;
        }
        public float FinalRate(DateTime PaydayDate, out string Exception, int DeepLevelSubWorkers = 0)
        {
            float Final = default;
            Exception = default;

            if (!(Group is null))
            {
                int PercentGroup = CurrentPercent(PaydayDate);               
                float SubRate = DeepLevelSubWorkers == 0 ? default : SubWorkers.Select(x => x.FinalRate(PaydayDate.Date, out string Except, DeepLevelSubWorkers - 1)).Sum();
                SubRate = SubRate * Group.PercentSubWorkers / 100;
                Final = BaseRate * (((float)PercentGroup / 100) + 1) + SubRate;
            }
            else
                Exception = "Group is null";

            return Final;
        }
        public float PaymentPeriod(DateTime Start, DateTime End)
        {
            if (Start.Date == default(DateTime))
                Start = EnrollmentDate;

            if (End.Date == default(DateTime))
                End = DateTime.Now;

            Start = Start.Date;
            End = End.Date;
            float PaymentRate = default;

            DateTime DateStart = Start;

            while (DateStart < End)
            {
                DateStart = DateStart.AddMonths(1);

                if (DateStart <= End)
                {
                    PaymentRate += FinalRate(DateStart, out string Exception, Group is null ? 0 : Group.DeepLevelSubWorkers);
                }
                else
                {
                    float Coef = (float)((End - DateStart.AddMonths(-1)).TotalDays / (DateStart - DateStart.AddMonths(-1)).TotalDays); // not completed part of month
                    PaymentRate += Coef * FinalRate(End, out string Exception, Group is null ? 0 : Group.DeepLevelSubWorkers);
                }
            }

            return (float)Math.Round(PaymentRate * 100) / 100;
        }
        public HR_Group Group { get; set; }

        private List<HR_Worker> subWorkers;
        public List<HR_Worker> SubWorkers
        {
            get
            {
                if (subWorkers is null)
                    subWorkers = new List<HR_Worker>();

                if (!(subWorkers is null))
                    foreach (var t in subWorkers)
                        t.Parent = this;

                if (!(Group is null))
                    if (Group.PercentSubWorkers == default(float))
                        if (subWorkers.Count > 0)
                            subWorkers = new List<HR_Worker>();

                return subWorkers;
            }
            set
            {
                subWorkers = value;
            }
        }
        public HR_Worker UpdateObject_Original { get; set; }
        public override object ClonePart()
        {
            return new HR_Worker
            {
                Name = this.Name,
                EnrollmentDate = this.EnrollmentDate,
                BaseRate = this.BaseRate,
                Group = this.Group is null ? null : (HR_Group)this.Group.Clone(),
                SubWorkers = this.SubWorkers is null ? null : this.SubWorkers.Select(x => (HR_Worker)x.Clone()).ToList(),

                Log = this.Log is null ? null : (LogRecord)this.Log.Clone(),
                UpdateObject_Original = this.UpdateObject_Original is null ? null : (HR_Worker)this.UpdateObject_Original.Clone(),
            };
        }
    }
}
