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
                    user = new User(
                        id,
                        TimeSpan.Zero,
                        DateTime.MinValue
                        );
                    Data.Users.Add(user); 
                    var info = new List<object>{
                        user.ID,
                        user.Status,
                        user.NotificationStatus,
                        user.Blocked.Item1,
                        user.Blocked.Item2,
                        user.Blocked.Item3,
                        user.Condition,
                        user.WashCounter,
                        user.OpenerTimes,
                        user.AverageOpenerTime,
                        user.PasswordTries
                    };
                    DataMethods.Update.UserUpdates(user, info, fullUpdate: true);
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
                    WebInterface.SendMessage(user.ID, "Время до конца блокировки: " + ((int)((user.Blocked.Item2 - DateTime.UtcNow).TotalHours)).ToString() + " часов");
                }
                else
                    BotAsynh.Run(user, button, msg);
            }
        }



        static void Main()
        {
            DataMethods.GetData();
            var thread = new Thread(() => CheckMessege());
            thread.Start();
            var updater = new Thread(() => DataMethods.NewDay());
            updater.Start();
            var noticer = new Thread(() => Notifications.Run());
            noticer.Start();
        }
    }
}