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
                var session = new WebInterface();
                var id = session.GetUserId();
                User user;
                if (Data.Users.FindIndex(delegate (User usr)
                {
                    return usr.ID == id;
                }) == -1)
                    user = new User(); 
                else
                    user = Data.Users.Find(delegate (User usr)
                    {
                        return usr.ID == id;
                    });
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