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
            switch (user.Status)
            {
                case 0:
                    session.SendMessage("Клиент");
                    BotClient(user, session);
                    break;
                case 1:
                    session.SendMessage("ССК");
                    BotClient(user, session);
                    break;
                case 2:
                    session.SendMessage("Открывающий");
                    
                    break;
                case 3:
                    session.SendMessage("Администратор");
                    BotAdmin(user, session);
                    break;
                case 4:
                    session.SendMessage("Вы временно заблокированны");
                    session.SendMessage("Время до конца блокировки");
                    break;
            }
        }

        public static void BotAdmin(User user, WebInterface session)
        {
            session.SendMessage("Выберите действие:");
            while (true)
            {
                session.SendButtons(GetAdminMenuButtons(user));
                var buttonClicked = session.GetButton();
                switch (buttonClicked)
                {
                    case "1":
                        BotClient(user, session);
                        break;
                    case "2":
                        UserAdministrtion(user, session);
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "b":
                        return;
                }
            }
        }

        //ветки диалога
        private static void UserAdministrtion(User user, WebInterface session)
        {
            while (true)
            {
                session.SendButtons(GetAdminUsersButtons(user));
                var buttonClicked1 = session.GetButton();
                if (buttonClicked1 == "b") break;
                while (true)
                {
                    session.SendMessage(Data.Statuses[int.Parse(buttonClicked1)]);
                    var listIds = GetUsersIdsList(buttonClicked1);
                    string stringListIds = GetStringListIds(listIds);
                    session.SendMessage(stringListIds);
                    session.SendButtons(GetAdminStatusButtons(user, int.Parse(buttonClicked1)));
                    var buttonClicked2 = session.GetButton();
                    switch (buttonClicked2)
                    {
                        case "1":
                            session.SendMessage("Введите id пользователя");
                            var id = long.Parse(session.GetMessage());
                            Data.userStatus.Add(id, int.Parse(buttonClicked1));
                            break;
                        case "2":
                            session.SendMessage("Введите номер пользователя");
                            var num = int.Parse(session.GetMessage());
                            Data.userStatus.Remove(listIds[num]);
                            break;
                        case "b":
                            break;
                    }
                    if (buttonClicked2 == "b") break;
                }
            }
        }

        public static void BotClient(User user, WebInterface session)
        {
            session.SendMessage("Выберите действие:");
            while (true)
            {
                session.SendButtons(GetClientMenuButtons(user));
                var buttonClicked = session.GetButton();
                switch (buttonClicked)
                {
                    case "1":
                        session.SendMessage("Запись");
                        session.SendButtons(GetDayButtons(user.Status));
                        var selectedDay = session.GetButton();
                        if (selectedDay == "b") break;  
                        session.SendButtons(GetTimeButtons(selectedDay));
                        var selectedTime = session.GetButton();
                        if (selectedTime == "b") goto case "1";
                        session.SendButtons(GetAmountButtons(selectedDay, selectedTime));
                        var selectedAmount = session.GetButton();
                        if (selectedAmount == "b") goto case "1";
                        MakeNote(user, selectedDay, selectedTime, selectedAmount);
                        break;
                    case "2":
                        session.SendMessage("Отмена записи");
                        session.SendButtons(GetNotesButtons(user));
                        var selectedNote = int.Parse(session.GetButton());
                        RemoveNote(user, selectedNote);
                        break;
                    case "3":
                        user.NotificationStatus = !user.NotificationStatus;
                        if (user.NotificationStatus)
                            session.SendMessage("Уведомления выключены");
                        else
                            session.SendMessage("Уведомления включены");
                        break;
                    case "4":
                        session.SendMessage("Какая-то важная инфа по стирке...");
                        break;
                    case "b":
                        return;
                }
            }
        }

        //методы клиента
        private static void MakeNote(User user, string selectedDay, string selectedTime, string selectedAmount)
        {
            var note = new TimeNote(
                                    user.ID,
                                    int.Parse(selectedDay),
                                    int.Parse(selectedTime),
                                    int.Parse(selectedAmount)
                                    );
            //добавляет запись в список записей
            Data.Notes.Add(note);
            //ищет первую свободную ячейку
            var i = 0;
            while (Data.Days[note.DayForNotation].HoursWashesTable[note.Time, i] != 0) i++;
            //записывает ID note.Amount раз
            for (var j = i; j < note.Amount + i; j++)
                Data.Days[note.DayForNotation].HoursWashesTable[note.Time, j] = note.UserID;
        }

        private static void RemoveNote(User user, int selectedNote)
        {
            var notes = Data.Notes.FindAll(delegate (TimeNote note)
            {
                return note.UserID == user.ID;
            });
            Data.Notes.Remove(notes[selectedNote]);
            var amount = notes[selectedNote].Amount;
            var dayIndex = Data.Days.FindIndex(delegate (Day day)
            {
                return day.Date == notes[selectedNote].Day.Date;
            });
            for (var i = 0; i < Data.WashesAmount; i++)
            {
                if (Data.Days[dayIndex].HoursWashesTable[notes[selectedNote].Time, i] == user.ID
                    && amount != 0)
                {
                    Data.Days[dayIndex].HoursWashesTable[notes[selectedNote].Time, i] = 0;
                    amount -= 1;
                }
            }
        }

        //методы получения кнопок клиента
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

        private static List<string[]> GetClientMenuButtons(User user)
        {
            var clientMenuButtons = new List<string[]>();

            if (Data.Days.FindIndex(delegate (Day day)
            {
                return (day.EmptySpaces != 0) && (day.AvailableForSSK == (user.Status == 1));
            }
            ) != -1)
                clientMenuButtons.Add(new[] { "Записаться в прачечную", "1" });
            if (Data.Notes.FindIndex(delegate (TimeNote note)
            {
                return note.UserID == user.ID;
            }
            ) != -1)
                clientMenuButtons.Add(new[] { "Отмена", "2" });
            clientMenuButtons.Add(new[] { "Выкл уведомления", "3" });
            clientMenuButtons.Add(new[] { "FAQ", "4" });
            if (!user.NotificationStatus)
            {
                clientMenuButtons[^2][0] = "Вкл уведомления";
            }
            else
            {
                clientMenuButtons[^2][0] = "Выкл уведомления";
            }
            if (user.Status != 2)
            {
                clientMenuButtons.Add(new[] { "Выйти", "b" });
            }
            return clientMenuButtons;
        }

        //методы админа
        private static List<string[]> GetAdminMenuButtons(User user)
        {
            var buttons = new List<string[]> {
                new[] { "Функции клиента", "1" },
                new[] { "Администрирование пользователей", "2" },
                new[] { "Администрирование прачечной", "3" },
                new[] { "Отчетность", "4" },
                new[] { "Выйти", "b" }
            };
            return buttons;
        }

        private static List<string[]> GetAdminUsersButtons(User user)
        {
            var buttons = new List<string[]> {
                new[] { "ССК", "1" },
                new[] { "Открывающие", "2" },
                new[] { "Администраторы", "3" },
                new[] { "Черный список", "4" },
            };
            return buttons;
        }

        private static List<string[]> GetAdminStatusButtons(User user, int status)
        {
            var buttons = new List<string[]> {
                new[] { "Добавить", "1" }
            };
            if ((Data.AmountOf(status) != 0 && status != 3) || (Data.AmountOf(status) > 1 && status == 3))
                buttons.Add(new[] { "Удалить", "2" });
            return buttons;
        }

        private static List<long> GetUsersIdsList(string buttonClicked1)
        {
            var listIds = new List<long>();
            foreach (var userCategory in Data.userStatus)
                if (userCategory.Value == int.Parse(buttonClicked1))
                    listIds.Add(userCategory.Key);
            return listIds;
        }

        private static string GetStringListIds(List<long> listIds)
        {
            string stringListIds = "";
            var i = 0;
            foreach (var id in listIds)
            {
                stringListIds += i.ToString() + " " + id.ToString() + "\n";
                i++;
            } 
            return stringListIds;
        }

    }
}

