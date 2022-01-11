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
                var (id, button, msg) = WebInterface.GetContent();
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
                //todo
                if (user.Blocked.Item1)
                {
                    WebInterface.SendMessage(user.ID, "Вы временно заблокированны");
                    var time = DateTime.UtcNow - user.Blocked.Item2;
                    WebInterface.SendMessage(user.ID, "Время до конца блокировки: " + time.TotalHours.ToString() + " часов");
                }
                else
                    BotAsynh.Run(user, button, msg);  
            }
        }



        static void Main()
        {
            var thread = new Thread(() => CheckMessege());
            thread.Start();
            var updater = new Thread(() => DataMethods.DayUpdate());
            updater.Start();
            var noticer = new Thread(() => Notifications.Run());
            noticer.Start();
        }
    }
}