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
        /// Время по которым работают машинки
        /// </summary>
        public static List<int> WashesHours = new List<int> { 10, 14, 18, 20 };
        public static List<int> WashesOpenerHours{
            get
            {
                var washesOpenerHours = new List<int>();
                for (var i = 0; i < WashesHours.Count - 1; i++)
                {
                    washesOpenerHours.Add(WashesHours[i]);
                    if (WashesHours[i + 1] - WashesHours[i] > 2)
                        washesOpenerHours.Add(WashesHours[i]+2); 
                }
                washesOpenerHours.Add(WashesHours[^1]);
                washesOpenerHours.Add(WashesHours[^1] + 2);
                return washesOpenerHours;
            }
        }

        public static string WashesHoursToString()
        {
            var washesHoursData = "";
            foreach (var id in Data.WashesHours)
                washesHoursData += id.ToString() + ":00 \n";
            return washesHoursData;
        }

        /// <summary>
        ///список дней записи
        /// </summary>
        public static List<Day> Days = new List<Day>{
            new Day(){
                Date = new DateTime(2021, 11, 15),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} }
            },
            new Day(){
                Date = new DateTime(2021, 11, 16),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} }
            },
            new Day(){
                Date = new DateTime(2021, 11, 17),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} }
            },
            new Day(){
                Date = new DateTime(2021, 11, 18),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} }
            },
            new Day(){
                Date = new DateTime(2021, 11, 19),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} }
            },
            new Day(){
                Date = new DateTime(2021, 11, 20),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} }
            },
            new Day(){
                Date = new DateTime(2021, 11, 21),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} }
            }
        };

        //список записей
        public static List<TimeNote> Notes = new List<TimeNote>();

        public static string AllNotesToStringList()
        {
            var list = "";
            for (var i = 0; i < Data.Notes.Count; i++)
            {
                list += i.ToString() + ' ' + Data.Notes[i].ToString() + "\n";
            }
            if (Data.Notes.Count == 0)
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
