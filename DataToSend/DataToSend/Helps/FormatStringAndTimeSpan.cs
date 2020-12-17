using System;

namespace CommonDll.Helps
{
    public class FormatStringAndTimeSpan
    {
        public static TimeSpan Str2TimeSpan(string str)
        {
            string str_buff = str;

            if (string.IsNullOrEmpty(str_buff))
                str_buff = "00:00:00";

            int col_pos = str_buff.IndexOf(':');
            int days = 0, hours = 0, minutes = 0, seconds = 0;

            if (str_buff.IndexOf('.') > 0 & str_buff.IndexOf('.') < col_pos)
            {
                days = Convert.ToInt32(str_buff.Substring(0, str_buff.IndexOf('.')));
                str_buff = str_buff.Substring(str_buff.IndexOf('.') + 1);
            }

            if (col_pos > 0)
            {
                try
                {
                    hours = Convert.ToInt32(str_buff.Substring(0, col_pos));

                    if (hours >= 24)
                    {
                        days = hours / 24;
                        hours %= 24;

                    }

                    str_buff = str_buff.Substring(col_pos + 1);
                    col_pos = str_buff.IndexOf(':');

                    if (col_pos > 0)
                    {
                        minutes = Convert.ToInt32(str_buff.Substring(0, col_pos));
                        str_buff = str_buff.IndexOf(".") == -1 ? str_buff.Substring(col_pos + 1) : str_buff.Substring(col_pos + 1, str_buff.IndexOf("."));
                    }

                    seconds = int.Parse(str_buff);
                }
                catch
                {
                    return TimeSpan.Parse(str);
                }
            }

            if (seconds > 59)
            {
                int i = seconds / 60;
                seconds -= i * 60;
                minutes += i;
            }

            if (minutes > 59)
            {
                int i = minutes / 60;
                minutes -= i * 60;
                hours += i;
            }

            if (hours > 23)
            {
                int i = hours / 24;
                hours -= i * 24;
                days += i;
            }

            string format = days.ToString() + '.' + hours.ToString() + ':' + minutes.ToString() + ':' + seconds.ToString();

            try
            {
                return TimeSpan.Parse(format);
            }
            catch
            {
                try
                {
                    return TimeSpan.Parse(str);
                }
                catch
                {
                    return new TimeSpan();
                }
            }
        }

        public static string TimeSpanToString(TimeSpan ts)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", (ts.Days * 24) + ts.Hours, ts.Minutes, ts.Seconds);
        }
    }
}
