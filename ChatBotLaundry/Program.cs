using System;
using System.Collections.Generic;
using System.Threading;

namespace ChatBotLaundry
{
    class Program
    {
        static void CheckMessege()
        {
            while (true)
            {
                var user = new User();
                var session = new WebInterface();
                user.ID = session.GetUserId();
                Bot.BotRun(user, session);  
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
                Data.Days.RemoveAt(0);
                var newDay = new Day
                {
                    Date = DateTime.UtcNow,
                    HoursWashesTable = new long[Data.WashesHours.Count, Data.WashesAmount]
                };
                Data.Days.Add(newDay);
            }
        }
    }
}