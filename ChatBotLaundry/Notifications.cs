using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChatBotLaundry
{
    class Notifications
    {
        public static void Run()
        {
            while (true)
            {
                var day = Data.Days[0];
                var time = new TimeSpan(0, 1, 0);
                ClientNotice(day);
                Thread.Sleep(time);
            }
        }
        /// <summary>
        /// отправляет уведомления за 30 и 5 минут до стирки
        /// </summary>
        /// <param name="day"></param>
        public static void ClientNotice(Day day)
        {
            foreach (var n in day.Notes)
            {
                if (Data.Users.Find(delegate (User usr) { return usr.ID == n.UserID; }).NotificationStatus)
                {
                    var m = DateTime.UtcNow.Minute;
                    var h = DateTime.UtcNow.Hour.ToTimezone();
                    if (n.Time - 1 == h)
                    {
                        if (m == 30)
                            WebInterface.SendMessage(n.UserID, "Через 30 минут стирка");
                        if (m == 55)
                            WebInterface.SendMessage(n.UserID, "Через 5 минут стирка");
                    }
                    if (n.Time + 1 == h)
                    {
                        if (m == 30)
                            WebInterface.SendMessage(n.UserID, "Не забудьте забрать вещи через 30 минут");
                        if (m == 55)
                            WebInterface.SendMessage(n.UserID, "Не забудьте забрать вещи через 5 минут");
                    }
                }
            }       
        }

        public static void OpenerNotice(Day day)
        {
            foreach (var n in day.Notes)
            {
                if (Data.Users.Find(delegate (User usr) { return usr.ID == n.UserID; }).NotificationStatus)
                {
                    var m = DateTime.UtcNow.Minute;
                    if (n.Time == DateTime.UtcNow.Hour - 1)
                    {
                        if (m > 27 && m < 33)
                            WebInterface.SendMessage(n.UserID, "Через 30 минут стирка");
                        if (m > 52 && m < 58)
                            WebInterface.SendMessage(n.UserID, "Через 5 минут стирка");
                    }
                    if (n.Time == DateTime.UtcNow.Hour + 1)
                    {
                        if (m > 27 && m < 33)
                            WebInterface.SendMessage(n.UserID, "Не забудьте забрать вещи через 30 минут");
                        if (m > 52 && m < 58)
                            WebInterface.SendMessage(n.UserID, "Не забудьте забрать вещи через 5 минут");
                    }
                }
            }
        }
    }
}
