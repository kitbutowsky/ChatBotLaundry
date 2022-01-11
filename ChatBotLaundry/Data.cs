using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    class Data
    {
        public static string Info = 
            @"Если вам не открыли прачку в течении 5 минут - писать:
https://vk.com/im?sel=188713690 
или
https://vk.com/im?sel=149102317 
Открывающий ждет в прачке не более 10 минут после времени записи)";
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

        /// <summary>
        ///список всех дней записи
        /// </summary>
        public static List<Day> DaysArhive = new List<Day>();
        /// <summary>
        ///список дней записи недели
        /// </summary>
        public static List<Day> Days = new List<Day>{
            new Day(){
                Date = new DateTime(2022, 1, 11),
                HoursWashesTable = new long[,]{ {0, 0, 0}, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } },
                WashesHours = new List<int> { 5, 8, 11, 18 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count], 
                WashesAmount = 3
            },
            new Day(){
                Date = new DateTime(2022, 1, 13),
                HoursWashesTable = new long[,]{ { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count],
                WashesAmount = 3
            },
            new Day(){
                Date = new DateTime(2022, 1, 14),
                HoursWashesTable = new long[,]{ { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }},
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count],
                WashesAmount = 3
            },
            new Day(){
                Date = new DateTime(2022, 1, 15),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count],
                WashesAmount = 3
            },
            new Day(){
                Date = new DateTime(2022, 1, 16),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count],
                WashesAmount = 3
            },
            new Day(){
                Date = new DateTime(2022, 1, 17),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} },
                WashesHours = new List<int> { 5, 8, 11, 14 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count],
                WashesAmount = 3
            },
            new Day(){
                Date = new DateTime(2022, 1, 18),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 0} },
                WashesHours = new List<int> { 5, 7, 11, 15 },
                HoursWashesOpenerTable = new long[WashesOpenerHours.Count],
                WashesAmount = 3
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
            new User{ID = 188713690, Status = 3},
            new User{ID = 70259283, Status = 3}
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
