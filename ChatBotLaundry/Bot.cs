using System;
using System.Collections.Generic;
using System.Text;
using static Requester.Program;

namespace Requester
{
    class Bot
    {

        public static TimeNote BotRun(User user, WebInterface session)
        {
            if (user.Status != 2)
            {
                if (user.Status == 1)
                    session.SendMessage("ССК");
                else
                    session.SendMessage("Клиент");
                return BotClient(user, session);
            }
            else
            {
                Console.WriteLine("Администратор");
                return new TimeNote();
            }
        }

        public static TimeNote BotClient(User user, WebInterface session)
        {
            List<string[]> GetAmountButtons(string d, string t)
            {
                var time = int.Parse(t);
                var day = int.Parse(d);
                var buttons = new List<string[]>();
                for (var i = 1; i < Days[day].EmptyTimes[time]+1; i++)
                {
                    var button = new string[] { i.ToString() + ". ", i.ToString() };
                    buttons.Add(button);
                }
                return buttons;
            }

            List<string[]> GetTimeButtons(string d)
            {
                var day = int.Parse(d);
                var buttons = new List<string[]>();
                for (var i = 0; i < Days[day].HoursWashesTable.GetLength(0); i++)
                {
                    if(Days[day].EmptyTimes[i] != 0)
                    {
                        var button = new string[] { WashesHours[i].ToString() + ":00 кол-во свободных машинок:" + Days[day].EmptyTimes[i].ToString(), i.ToString() };
                        buttons.Add(button);
                    } 
                }
                return buttons;
            }
            
            List<string[]> GetDayButtons(int status)
            {
                var buttons = new List<string[]>();
                for (var d = 0; d < Days.Count; d++)
                    if (Days[d].IsFree && ((Days[d].AvailableForSSK && status != 0) || (!Days[d].AvailableForSSK && status == 0)))
                    {
                        var button = new string[] { Days[d].DayOfWeekR + " " + Days[d].EmptySpaces.ToString(), d.ToString() };
                        buttons.Add(button);
                    }
                return buttons;
            }

            var clientMenuButtons = new List<string[]> { new []{"Записаться в прачечную", "1" }, new []{ "Отменить запись", "2" } , new []{ "Выкл уведомления", "3"}, new []{"FAQ", "4" }};
            session.SendMessage("Выберите действие:");
            while (true)
            {
                session.SendButtons(clientMenuButtons);
                var buttonClicked = session.GetButton();
                if (buttonClicked == "1")
                {
                    session.SendMessage("Запись");
                    session.SendButtons(GetDayButtons(user.Status));
                    var selectedDay = session.GetButton();
                    session.SendButtons(GetTimeButtons(selectedDay));
                    var selectedTime = session.GetButton();
                    session.SendButtons(GetAmountButtons(selectedDay, selectedTime));
                    var selectedAmount = session.GetButton();
                    return new TimeNote { Day = int.Parse(selectedDay), Time = int.Parse(selectedTime), Amount = int.Parse(selectedAmount) };
                }
                else if (buttonClicked == "2")
                {
                    session.SendMessage("Отмена");
                    return new TimeNote();
                }
                else if (buttonClicked == "3")
                {
                    user.NotificationStatus = !user.NotificationStatus;
                    if (!user.NotificationStatus)
                    {
                        session.SendMessage("Уведомления выключены");
                        clientMenuButtons[2][0] = "Вкл уведомления";
                    }
                    else
                    {
                        session.SendMessage("Уведомления включены"); //метод отправки сообщения
                        clientMenuButtons[2][0] = "Выкл уведомления";
                    }
                }
                else if (buttonClicked == "4")
                    session.SendMessage("Какая-то важная инфа по стирке...");
                else if (buttonClicked == "000")
                {
                    if (!userStatus.ContainsKey(user.ID))
                    {
                        userStatus.Add(user.ID, 1);
                        session.SendMessage("Уровень повышен до ССК");
                    }
                }
            }
        }
    }
}

