using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    class Data
    {
        public static string Info = "Какая-то важная информация... Бла... Бла... Бла...";
        public static string NewInfo;

        public static string Password = "admin";

        /// <summary>
        /// Количество машинок
        /// </summary>
        public static int WashesAmount = 3;
        /// <summary>
        /// Время по которым работают машинки, со сдвигом по Timezone
        /// </summary>
        public static List<int> WashesHours = new List<int> { 5, 8, 11, 14 }; 
        public static List<int> WashesHoursInTimezone
        {
            get
            {
                var washesHoursInTimezone = new List<int>();
                foreach (var time in WashesHours)
                {
                    var newtime = time + StaticDataAndMetods.Timezone;
                    if (newtime >= 24)
                    {
                        newtime -= 24;
                    }
                    washesHoursInTimezone.Add(newtime);
                }
                return washesHoursInTimezone;
            }
        }
        public static string WashesHoursToString()
        {
            var washesHoursData = "";
            foreach (var time in WashesHoursInTimezone)
                washesHoursData += time.ToString() + ":00 \n";
            return washesHoursData;
        }
        public static List<int> WashesOpenerHours
        {
            get
            {
                var washesOpenerHours = new List<int>();
                for (var i = 0; i < WashesHoursInTimezone.Count - 1; i++)
                {
                    washesOpenerHours.Add(WashesHoursInTimezone[i]);
                    if (WashesHoursInTimezone[i + 1] - WashesHoursInTimezone[i] > 2)
                        washesOpenerHours.Add(WashesHoursInTimezone[i] + 2);
                }
                washesOpenerHours.Add(WashesHoursInTimezone[^1]);
                washesOpenerHours.Add(WashesHoursInTimezone[^1] + 2);
                return washesOpenerHours;
            }
        }

        /// <summary>
        ///список всех дней записи
        /// </summary>
        public static List<Day> DaysArhive = new List<Day>();
        /// <summary>
        ///список дней записи недели
        /// </summary>
        public static List<Day> Days = new List<Day>{
            new Day(){
                Date = new DateTime(2021, 11, 15),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            },
            new Day(){
                Date = new DateTime(2021, 11, 16),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            },
            new Day(){
                Date = new DateTime(2021, 11, 17),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            },
            new Day(){
                Date = new DateTime(2021, 11, 18),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            },
            new Day(){
                Date = new DateTime(2021, 11, 19),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            },
            new Day(){
                Date = new DateTime(2021, 11, 20),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            },
            new Day(){
                Date = new DateTime(2021, 11, 21),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            }
        };


        public static string AllNotesToStringList()
        {
            var list = "";
            foreach (var day in Days)
                for (var i = 0; i < day.Notes.Count; i++)
                {
                    list += i.ToString() + ' ' + day.Notes[i].ToString() + "\n";
                }
            if (list == "")
                list = "Нет записей";
            return list;
        }

        readonly public static List<string> Statuses = new List<string> {
            "Клиенты",
            "ССК",
            "Открывающие",
            "Админы",
            "Черный список" };

        /// <summary>
        /// список пользователей 
        /// </summary>
        public static List<User> Users = new List<User>{
            new User{ID = 1, Status = 3},
            new User{ID = 2, Status = 1, Blocked = (true, DateTime.Now)},
            new User{ID = 3, Status = 1},
        };
        /// <summary>
        /// возвращает количество людей определенного статуса (1 - сск, 2 - открывающий, 3 - админ, 4 - заблокированный)
        /// </summary>
        public static int AmountOf(int status)
        { 
            var count = 0;
            foreach (var user in Users)
                if (user.Status == status) count++;
            return count;
        }

    }
}
