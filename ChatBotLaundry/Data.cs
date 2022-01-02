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
        public static List<int> WashesOpenerHours
        {
            get
            {
                var washesOpenerHours = new List<int>();
                for (var i = 0; i < WashesHours.Count - 1; i++)
                {
                    washesOpenerHours.Add(WashesHours[i]);
                    if (WashesHours[i + 1] - WashesHours[i] > 2)
                        washesOpenerHours.Add(WashesHours[i] + 2);
                }
                washesOpenerHours.Add(WashesHours[^1]);
                washesOpenerHours.Add(WashesHours[^1] + 2);
                return washesOpenerHours;
            }
        }
        public static List<int> WashesOpenerHoursInTimezone
        {
            get
            {
                var washesOpenerHoursInTimezone = new List<int>();
                foreach (var time in WashesOpenerHours)
                {
                    var newtime = time + StaticDataAndMetods.Timezone;
                    if (newtime >= 24)
                    {
                        newtime -= 24;
                    }
                    washesOpenerHoursInTimezone.Add(newtime);
                }
                return washesOpenerHoursInTimezone;
            }
        }
        public static string WashesHoursToString()
        {
            var washesHoursData = "";
            foreach (var time in WashesHoursInTimezone)
                washesHoursData += time.ToString() + ":00 \n";
            return washesHoursData;
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
                HoursWashesTable = new long[,]{ {0, 0, 0}, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            },
            new Day(){
                Date = new DateTime(2021, 11, 16),
                HoursWashesTable = new long[,]{ { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            },
            new Day(){
                Date = new DateTime(2021, 11, 17),
                HoursWashesTable = new long[,]{ { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }},
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
                Date = new DateTime(2021, 12, 31),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 7, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count]
            }
        };

        /// <summary>
        /// список пользователей 
        /// </summary>
        public static List<User> Users = new List<User>{
            new User{ID = 1, Status = 3},
            new User{ID = 2, Status = 1, Blocked = (true, DateTime.UtcNow, 4)},
            new User{ID = 3, Status = 1},
            new User{ID = 4, Status = 2},
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
