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

        public static string info;
        public static string Info { 
            get
            {
                return info;
            }
            set
            {
                info = value;
                DataMethods.Update.Data.Info();
            }
        }
        public static string NewInfo;

        public static string password;
        public static string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                DataMethods.Update.Data.Password();
            }
        }

        /// <summary>
        /// Количество машинок
        /// </summary>
        public static int washesAmount;
        public static int WashesAmount
        {
            get
            {
                return washesAmount;
            }
            set
            {
                washesAmount = value;
                DataMethods.Update.Data.WashesAmount();
            }
        }
        /// <summary>
        /// Время по которым работают машинки, со сдвигом по Timezone
        /// </summary>
        public static List<int> washesHours = new List<int> { };
        public static List<int> WashesHours 
        {
            get
            {
                return washesHours;
            }
            set
            {
                washesHours = value;
                DataMethods.Update.Data.WashesHours();
            }
        }

        public static List<int> WashesOpenerHours
        {
            get
            {
                var washesOpenerHours = new List<int> { };
                if (washesHours != null) 
                { 
                    for (var i = 0; i < washesHours.Count - 1; i++)
                    {
                        washesOpenerHours.Add(washesHours[i]);
                        if (WashesHours[i + 1] - washesHours[i] > 2)
                            washesOpenerHours.Add(washesHours[i] + 2);
                    }
                    washesOpenerHours.Add(washesHours[^1]);
                    washesOpenerHours.Add(washesHours[^1] + 2);
                }
                return washesOpenerHours;
            }
        }
        /// <summary>
        ///список дней записи недели
        /// </summary>
        public static List<Day> Days = new List<Day>();

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
