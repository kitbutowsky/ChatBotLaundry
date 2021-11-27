using System;
using System.Collections.Generic;
using System.Threading;

namespace ChatBotLaundry
{
    class Program
    {
        public static int WashesAmount = 3;
        public static List<int> WashesHours = new List<int> { 10, 14, 18, 20 };

        //словарь статусов пользователей (1 - сск, 2 - админ)
        public static Dictionary<long, int> userStatus = new Dictionary<long, int>{
            {1, 1},
            {2, 2},
            {3, 1}
        };

        //список дней записи
        public static List<Day> Days = new List<Day>{
            new Day(){
                Date = new DateTime(2021, 11, 15),
                HoursWashesTable = new long[,]{ {1, 1, 0}, {1, 2, 3}, { 1, 1, 1 }, { 1, 2, 2} }
            },
            new Day(){
                Date = new DateTime(2021, 11, 16),
                HoursWashesTable = new long[WashesHours.Count, WashesAmount]
            },
            new Day(){
                Date = new DateTime(2021, 11, 17),
                HoursWashesTable = new long[WashesHours.Count, WashesAmount]
            },
            new Day(){
                Date = new DateTime(2021, 11, 18),
                HoursWashesTable = new long[WashesHours.Count, WashesAmount]
            },
            new Day(){
                Date = new DateTime(2021, 11, 19),
                HoursWashesTable = new long[WashesHours.Count, WashesAmount]
            },
            new Day(){
                Date = new DateTime(2021, 11, 20),
                HoursWashesTable = new long[WashesHours.Count, WashesAmount]
            },
            new Day(){
                Date = new DateTime(2021, 11, 21),
                HoursWashesTable = new long[WashesHours.Count, WashesAmount]
            }
        };

        //список записей
        public static List<TimeNote> Notes = new List<TimeNote>();

        static void CheckMessege()
        {
            while (true)
            {
                var user = new User();
                var session = new WebInterface();
                user.ID = session.GetUserId();
                Bot.BotRun( user, session);  
            }
        }

        static void Main()
        {
            var thread = new Thread(() => CheckMessege());
            thread.Start();
            while (true)
            {
                Thread.Sleep(100000);
                //Console.WriteLine("Запись обновилась");
                Days.RemoveAt(0);
                var newDay = new Day
                {
                    Date = DateTime.UtcNow,
                    HoursWashesTable = new long[WashesHours.Count, WashesAmount]
                };
                Days.Add(newDay);
            }
        }
    }
}