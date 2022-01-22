using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBotLaundry
{
    static class StaticDataAndMetods
    {
        /// <summary>
        /// данные для авторизации в гугл таблицах
        /// </summary>
        internal static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        internal static readonly string ApplicationName = "Bot";

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

        public static string ToLetterColumn(int column)
        {
            column++;
            string columnIndex = "";
            var abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            while (column > 0)
            {
                columnIndex += abc[column % abc.Length - 1];
                column /= abc.Length;
            };
            var newshit = columnIndex.ToCharArray().Reverse().ToArray();
            columnIndex = string.Join("", newshit);
            return columnIndex;
        }
    }
}
