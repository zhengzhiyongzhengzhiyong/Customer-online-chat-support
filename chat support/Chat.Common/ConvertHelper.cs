using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Common
{
    /// <summary>
    /// 数据类型转换
    /// </summary>
    public static class ConvertHelper
    {
        public static int ToInt_V(this object obj)
        {
            int a = 0;
            int.TryParse(obj.ToString_V(), out a);
            return a;
        }

        public static Dictionary<string, TValue> ToDictionary<TValue>(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);
            return dictionary;
        }

        public static double ToDouble_V(this object obj)
        {
            double a = 0.0;
            double.TryParse(obj.ToString_V(), out a);
            return a;
        }

        /// <summary>
        /// 转换Double类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="digits">保留小数点位数</param>
        /// <returns></returns>
        public static double ToDouble_V(this object obj, int digits)
        {
            var a = obj.ToDouble_V();
            return Math.Round((decimal)a, digits, MidpointRounding.AwayFromZero).ToDouble_V();
        }

        public static string ToString_V(this object obj)
        {
            if (obj == null || DBNull.Value == obj) return "";
            return obj.ToString();
        }
        public static bool ToBoolean_V(this object obj)
        {
            try
            {
                if (obj == null || obj == DBNull.Value) return false;
                return Convert.ToBoolean(obj);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static DateTime ToDateTime_V(this object obj)
        {
            DateTime a = DateTime.MinValue;
            DateTime.TryParse(obj.ToString_V(), out a);
            return a;
        }

        /// <summary>
        /// DateTime转换string类型 默认格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToDateTimeString_V(this object obj)
        {
            return obj.ToDateTime_V().ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// DateTime转换string类型 默认格式yyyy-MM-dd
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToDateString_V(this object obj)
        {
            return obj.ToDateTime_V().ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// DateTime转换string类型 默认格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format">格式化的日期字符串</param>
        /// <returns></returns>
        public static string ToDateTimeString_V(this object obj, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                return obj.ToDateTime_V().ToDateTimeString_V();
            }
            return obj.ToDateTime_V().ToString(format);
        }

        /// <summary>
        /// 将提取Uit类型时间戳
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static uint ToDateTimeToUInt(this DateTime obj)
        {
            var stratime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, 0));
            return ((obj.Ticks - stratime.Ticks) / 10000000).ToUint_V();
        }

        /// <summary>
        /// 转换UInt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static uint ToUint_V(this object obj)
        {
            uint.TryParse(obj.ToString_V(), out uint t);
            return t;
        }
        public static long ToLong_V(this object obj)
        {
            long.TryParse(obj.ToString_V(), out long u);
            return u;
        }
        public static decimal ToDecimal_V(this object obj)
        {
            decimal.TryParse(obj.ToString_V(), out decimal t);
            return t;
        }

        /// <summary>
        /// 时间戳转换为时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToTime_V(this long timeStamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(timeStamp);
            return dt;
        }
        public static string ToProperties_V(this object obj)
        {
            if (obj == null) return "";
            var s = obj.GetType();
            StringBuilder app = new StringBuilder();
            foreach (var item in s.GetProperties())
            {
                app.Append($"{item.Name}:{item.GetValue(obj)}\n");
            }
            return app.ToString().Trim(',');
        }
        public static string ToJson_V(this object obj)
        {
            if (obj == null) return "";
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static T ToObjectByJson_V<T>(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
                return default(T);
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
