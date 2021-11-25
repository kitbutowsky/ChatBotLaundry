using System;
using System.Collections.Generic;
using System.Text;
using static Requester.Program;

namespace Requester
{
    class Bot
    {
        public static void BotRun( User user, WebInterface session)
        {
            if (user.Status != 2)
            {
                if (user.Status == 1)
                    session.SendMessage("ССК");
                else
                    session.SendMessage("Клиент");
                BotClient(user, session);
            }
            else
            {
                Console.WriteLine("Администратор");
            }
        }

        public static void BotClient(User user, WebInterface session)
        {
            var clientMenuButtons = new List<string[]> {
                new []{"Записаться в прачечную", "1" }, 
                new []{ "Отменить запись", "2" } , 
                new []{ "Выкл уведомления", "3"}, 
                new []{"FAQ", "4" }};
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
                    var note = new TimeNote { UserID = user.ID, Day = int.Parse(selectedDay), Time = int.Parse(selectedTime), Amount = int.Parse(selectedAmount) };
                    MakeNote(note);
                    break;
                }
                else if (buttonClicked == "2")
                {
                    session.SendMessage("Отмена");
                    break;
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

        private static List<string[]> GetAmountButtons(string d, string t)
        {
            var time = int.Parse(t);
            var day = int.Parse(d);
            var buttons = new List<string[]>();
            for (var i = 1; i < Days[day].EmptyTimes[time] + 1; i++)
            {
                var button = new string[] { i.ToString() + ". ", i.ToString() };
                buttons.Add(button);
            }
            return buttons;
        }

        private static List<string[]> GetDayButtons(int status)
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

        private static List<string[]> GetTimeButtons(string d)
        {
            var day = int.Parse(d);
            var buttons = new List<string[]>();
            for (var i = 0; i < Days[day].HoursWashesTable.GetLength(0); i++)
            {
                if (Days[day].EmptyTimes[i] != 0)
                {
                    var button = new string[] { WashesHours[i].ToString() + ":00 кол-во свободных машинок:" + Days[day].EmptyTimes[i].ToString(), i.ToString() };
                    buttons.Add(button);
                }
            }
            return buttons;
        }

        private static void MakeNote(TimeNote note)
        {
            //добавляет запись в список записей
            Notes.Add(note);
            //ищет первую свободную ячейку
            var i = 0;
            while (Days[note.Day].HoursWashesTable[note.Time, i] != 0) i++;
            //записывает ID note.Amount раз
            for (var j = i; j < note.Amount + i; j++)
                Days[note.Day].HoursWashesTable[note.Time, j] = note.UserID;
        }
    }
}

