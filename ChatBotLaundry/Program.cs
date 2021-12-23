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
                var (id, button, msg) = session.GetContent();
                User user;
                if (Data.Users.FindIndex(delegate (User usr)
                {
                    return usr.ID == id;
                }) == -1)
                {
                    user = new User() { ID = id };
                    Data.Users.Add(user);
                }
                else
                    user = Data.Users.Find(delegate (User usr)
                    {
                        return usr.ID == id;
                    });
                if (user.Blocked.Item1)
                {
                    session.SendMessage(user.ID, "Вы временно заблокированны");
                    session.SendMessage(user.ID, "Время до конца блокировки");
                }
                else
                    BotAsynh.BotRun(user, session, button, msg);  
            }
        }

        static void Main()
        {
            var thread = new Thread(() => CheckMessege());
            thread.Start();
            while (true)
            {
                Thread.Sleep(1000000);
                //Console.WriteLine("Запись обновилась");
                Data.Days.RemoveAt(0);
                var newDay = new Day
                {
                    Date = DateTime.UtcNow,
                    HoursWashesTable = new long[Data.WashesHours.Count, Data.WashesAmount],
                    HoursWashesOpenerTable = new long[Data.WashesOpenerHours.Count]
                };
                Data.Days.Add(newDay);
            }
        }
    }
}