using System;

namespace CommonDll.Structs.F_LogFile.F_LogRecord.F_TypeRecord
{
    /// <summary>
    /// Тип записи
    /// </summary>
    [Serializable]
    public class TypeRecord : ICloneable
    {
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value == Insert & text != value)
                    text = value;
                else if (value == Update & text != value)
                    text = value;
                else if (value == Delete & text != value)
                    text = value;
                else if (value == null & text != value)
                    text = value;
            }
        }
        public bool Exproted { get; set; } = true;
        public object Clone()
        {
            return new TypeRecord
            {
                Exproted = this.Exproted,
                Text = this.Text is null ? null : (string)this.Text.Clone(),
            };
        }

        [NonSerialized()]
        public static readonly string Insert = "<Insert>";
        [NonSerialized()]
        public static readonly string Update = "<Update>";
        [NonSerialized()]
        public static readonly string Delete = "<Delete>";
    }
}
