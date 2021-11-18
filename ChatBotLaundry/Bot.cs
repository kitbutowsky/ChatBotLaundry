using System;
using System.Collections.Generic;
using System.Text;
using static Requester.Program;

namespace Requester
{
    class Bot
    {
        public class Note
        {
            public int Time;
            public int Count;
        }

        public static void BotRun(User user, WebInterface session)
        {
            if (user.Status != 2)
            {
                var newNote = new Note();
                if (user.Status == 1)
                    session.SendMessage("ССК");
                else
                    session.SendMessage("Клиент");
                BotClient(user, session);
            }
            else
            {
                var newNote = new Note();
                Console.WriteLine("Администратор");
            }
        }

        public static void BotClient(User user, WebInterface session)
        {

            List<string> GetDayButtons(int status)
            {
                var buttons = new List<string>();
                foreach (var d in Days)
                    if (d.IsFree && ((d.AvailableForSSK && status != 0) || (!d.AvailableForSSK && status == 0)))
                    {
                        buttons.Add(d.DayOfWeekR + " " + d.EmptySpaces.ToString());
                    }
                return buttons;
            }
            var clientMenuButtons = new List<string> { "Записаться в прачечную", "Отменить запись", "Выкл уведомления", "FAQ" };
            session.SendMessage("Выберите действие:");
            while (true)
            {
                session.SendButtons(clientMenuButtons);
                int buttonClicked = session.GetButton();
                if (buttonClicked == 1)
                {
                    session.SendMessage("Запись");
                    session.SendButtons(GetDayButtons(user.Status));
                    break;
                }
                else if (buttonClicked == 2)
                {
                    session.SendMessage("Отмена");
                    break;
                }
                else if (buttonClicked == 3)
                {
                    user.NotificationStatus = !user.NotificationStatus;
                    if (!user.NotificationStatus)
                    {
                        session.SendMessage("Уведомления выключены");
                        clientMenuButtons[2] = "Вкл уведомления";
                    }
                    else
                    {
                        session.SendMessage("Уведомления включены"); //метод отправки сообщения
                        clientMenuButtons[2] = "Выкл уведомления";
                    }
                }
                else if (buttonClicked == 4)
                    session.SendMessage("Какая-то важная инфа по стирке...");
                //else if (buttonClicked == "000")
                //{
                //    if (!userStatus.ContainsKey(user.ID))
                //    {
                //        userStatus.Add(user.ID, 1);
                //        session.SendMessage("Уровень повышен до ССК");
                //    }
                //}
            }

        }
    }
}

