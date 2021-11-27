using System;
using System.Collections.Generic;
using System.Text;
using static ChatBotLaundry.Program;

namespace ChatBotLaundry
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
                    var note = new TimeNote(
                        user.ID, 
                        int.Parse(selectedDay), 
                        int.Parse(selectedTime), 
                        int.Parse(selectedAmount)
                        );
                    MakeNote(note);
                    break;
                }
                else if (buttonClicked == "2")
                {
                    session.SendMessage("Отмена записи");
                    session.SendButtons(GetNotesButtons(user));
                    var selectedDay = session.GetButton();
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
                    if (!Data.userStatus.ContainsKey(user.ID))
                    {
                        Data.userStatus.Add(user.ID, 1);
                        session.SendMessage("Уровень повышен до ССК");
                    }
                }
            }
        }

        private static List<string[]> GetNotesButtons(User user)
        {
            var notes = Data.Notes.FindAll(delegate (TimeNote note)
            {
                return note.UserID == user.ID;
            }
            ); 
            var buttons = new List<string[]>();
            for (var i = 0; i < notes.Count; i++)
            {
                var button = new string[] { notes[i].ToString() + "\n",  i.ToString()};
                buttons.Add(button);
            }
            return buttons;
        }

        private static List<string[]> GetAmountButtons(string d, string t)
        {
            var time = int.Parse(t);
            var day = int.Parse(d);
            var buttons = new List<string[]>();
            for (var i = 1; i < Data.Days[day].EmptyTimes[time] + 1; i++)
            {
                var button = new string[] { i.ToString() + ". ", i.ToString() };
                buttons.Add(button);
            }
            return buttons;
        }

        private static List<string[]> GetDayButtons(int status)
        {
            var buttons = new List<string[]>();
            for (var d = 0; d < Data.Days.Count; d++)
                if (Data.Days[d].IsFree && ((Data.Days[d].AvailableForSSK && status != 0) || (!Data.Days[d].AvailableForSSK && status == 0)))
                {
                    var button = new string[] { Data.Days[d].DayOfWeekR + " " + Data.Days[d].EmptySpaces.ToString(), d.ToString() };
                    buttons.Add(button);
                }
            return buttons;
        }

        private static List<string[]> GetTimeButtons(string d)
        {
            var day = int.Parse(d);
            var buttons = new List<string[]>();
            for (var i = 0; i < Data.Days[day].HoursWashesTable.GetLength(0); i++)
            {
                if (Data.Days[day].EmptyTimes[i] != 0)
                {
                    var button = new string[] { Data.WashesHours[i].ToString() + ":00 кол-во свободных машинок:" + Data.Days[day].EmptyTimes[i].ToString(), i.ToString() };
                    buttons.Add(button);
                }
            }
            return buttons;
        }

        private static void MakeNote(TimeNote note)
        {
            //добавляет запись в список записей
            Data.Notes.Add(note);
            //ищет первую свободную ячейку
            var i = 0;
            while (Data.Days[note.DayForNotation].HoursWashesTable[note.Time, i] != 0) i++;
            //записывает ID note.Amount раз
            for (var j = i; j < note.Amount + i; j++)
                Data.Days[note.DayForNotation].HoursWashesTable[note.Time, j] = note.UserID;
        }
    }
}

