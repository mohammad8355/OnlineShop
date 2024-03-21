using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Utility.PersianDate
{
    public static class PersianDateExtension
    {
        public static string ToPersianDate(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(date);
            int month = pc.GetMonth(date);
            int day = pc.GetDayOfMonth(date);

            string persianDate = $"{day} {GetPersianMonthName(month)} {year}";
            return persianDate;
        }
        private static string GetPersianMonthName(int month)
        {
            string[] monthName = new string[]
            {
                "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر",
                "آبان",
                "آذر",
                "دی",
                "بهمن",
                "اسفند",
            };
            return monthName[month - 1];
        }
    }
}
