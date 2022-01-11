using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    static class StaticDataAndMetods
    {
        public static readonly string[] dayOfWeekRussian = new[] { "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };
        public static readonly int Timezone = 5;

        public static string DayOfWeekR (DateTime date)
        { 
            return dayOfWeekRussian[(int)(date.AddHours(Timezone).DayOfWeek)];  
        }

        readonly public static List<string> Statuses = new List<string> {
            "Клиент",
            "ССК",
            "Открывающий",
            "Администратор"};

        public static int ToTimezone(this int time)
        {
            var newtime = time + Timezone;
            if (newtime >= 24)
                newtime -= 24;
            return newtime;
        }
    }
}
