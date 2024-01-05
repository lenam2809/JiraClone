using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Common
{
    public static class Convertion
    {
        public static T To<T>(this object obj, bool defaultOnFailure = true) where T : struct
        {
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                if (defaultOnFailure)
                {
                    return default;
                }
                else
                {
                    throw;
                }
            }
        }

        public static T? ToNullable<T>(this object obj, bool treatDefaultAsNull = false) where T : struct
        {
            try
            {
                T? result = (T)Convert.ChangeType(obj, typeof(T));
                return treatDefaultAsNull && Equals(result, default(T)) ? null : result;
            }
            catch
            {
                return null;
            }
        }

        public static string ToStringOrDefault(this object obj, string defaultValue = "")
        {
            return obj == null ? defaultValue : obj.ToString();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static DataTable ConvertToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static string ToRoman(this int number)
        {
            if (number < 0 || number > 3999) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + (number - 1000).ToRoman();
            if (number >= 900) return "CM" + (number - 900).ToRoman();
            if (number >= 500) return "D" + (number - 500).ToRoman();
            if (number >= 400) return "CD" + (number - 400).ToRoman();
            if (number >= 100) return "C" + (number - 100).ToRoman();
            if (number >= 90) return "XC" + (number - 90).ToRoman();
            if (number >= 50) return "L" + (number - 50).ToRoman();
            if (number >= 40) return "XL" + (number - 40).ToRoman();
            if (number >= 10) return "X" + (number - 10).ToRoman();
            if (number >= 9) return "IX" + (number - 9).ToRoman();
            if (number >= 5) return "V" + (number - 5).ToRoman();
            if (number >= 4) return "IV" + (number - 4).ToRoman();
            if (number >= 1) return "I" + (number - 1).ToRoman();
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        public static string Quote(this string str)
        {
            if (str == null)
                return null;
            return "\"" + str + "\"";
        }

        public static List<string> SplitToList(this string str, char separator = ',')
        {
            return str == null ? new List<string>() : str.Split(separator).ToList();
        }

        public static string ToOracleDateQuery(this DateTime date)
        {
            return string.Format("TO_DATE('{0}' , 'YYYY-MM-DD')", date.ToString("yyyy-MM-dd"));
        }
        private static string DatetimeISO8601(DateTime date)
        {
            //return date.ToString("o", CultureInfo.InvariantCulture);
            return date.Date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.sssZ");
        }
        private static string DatetimeNoTimeISO8601(DateTime date)
        {
            //return date.ToString("o", CultureInfo.InvariantCulture);
            DateTime newDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            return newDate.Date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.sssZ");
        }
        public static string GetQueryDateTimeSolrEq(string FieldName, DateTime valueDate, bool includeTime = false)
        {
            if (includeTime)
                return string.Format("{0}:\"{1}\"", FieldName, DatetimeISO8601(valueDate));
            else
                return string.Format("{0}:[\"{1}\" TO \"{2}\"]", FieldName, DatetimeISO8601(valueDate), DatetimeISO8601(valueDate.AddDays(1).AddTicks(-1)));
        }
        public static DateTime convertStringToDate(string valueDate, string fstr = "yyyy-MM-dd")
        {
            DateTime dateReturn;
            DateTime.TryParseExact(valueDate, fstr, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateReturn);
            return dateReturn;

        }

        public static string GetStringDateSolrInRange(string FieldName, string valuefrom, string valueto, bool isAdday = true)
        {

            return string.Format("{0}:[{1} TO {2}]", FieldName, DatetimeISO8601(convertStringToDate(valuefrom)), DatetimeISO8601(convertStringToDate(valueto).AddDays(1)));
        }
        public static string GetStringDateSolrFrom(string FieldName, string valuefrom)
        {
            return string.Format("{0}:[{1} TO *]", FieldName, DatetimeISO8601(convertStringToDate(valuefrom)));
        }
        public static string GetStringDateSolrFrom(string FieldName, DateTime valuefrom)
        {
            return string.Format("{0}:[{1} TO *]", FieldName, DatetimeISO8601(valuefrom));
        }
        public static string GetStringDateSolrEq(string FieldName, string valuefrom)
        {
            return string.Format("{0}:[{1} TO {2}]", FieldName, DatetimeNoTimeISO8601(convertStringToDate(valuefrom)), DatetimeNoTimeISO8601(convertStringToDate(valuefrom).AddDays(1)));
        }
        public static string GetStringDateSolrNotEq(string FieldName, string valuefrom)
        {
            return string.Format("!{0}:[{1} TO {2}]", FieldName, DatetimeISO8601(convertStringToDate(valuefrom)), DatetimeISO8601(convertStringToDate(valuefrom).AddDays(1)));
        }
        public static string GetStringDateSolrTo(string FieldName, string valueto, bool isAdday = true)
        {
            //Thêm 1 ngày với giá trị ngày tháng truyền vào không có thời gian
            var date = convertStringToDate(valueto);
            if (isAdday)
                date = date.AddDays(1);
            return string.Format("{0}:[* TO {1}]", FieldName, DatetimeISO8601(date));
        }

    }
}
