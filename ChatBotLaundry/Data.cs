using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    class Data
    {
        /// <summary>
        /// ID таблиц которую мы используем как базу данных
        /// </summary>
        public static readonly string SpreadsheetDBID = "1iTGZEAbp5AKaEaH3GgVm591w-bvDIYY5b2N9BYK5xfk";

        public static string Info;
        public static string NewInfo;

        public static string Password;

        /// <summary>
        /// Количество машинок
        /// </summary>
        public static int WashesAmount;
        /// <summary>
        /// Время по которым работают машинки, со сдвигом по Timezone
        /// </summary>
        public static List<int> WashesHours = new List<int> { }; 
        public static List<int> WashesOpenerHours
        {
            get
            {
                var washesOpenerHours = new List<int> { };
                if (WashesHours != null) 
                { 
                    for (var i = 0; i < WashesHours.Count - 1; i++)
                    {
                        washesOpenerHours.Add(WashesHours[i]);
                        if (WashesHours[i + 1] - WashesHours[i] > 2)
                            washesOpenerHours.Add(WashesHours[i] + 2);
                    }
                    washesOpenerHours.Add(WashesHours[^1]);
                    washesOpenerHours.Add(WashesHours[^1] + 2);
                }
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
        };

        /// <summary>
        /// список пользователей 
        /// </summary>
        public static List<User> Users = new List<User>();
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
