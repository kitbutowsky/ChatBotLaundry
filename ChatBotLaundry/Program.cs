﻿using System;
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
                var (id, button) = session.GetButton();
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
                BotAsynh.BotRun(user, session, button);  
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