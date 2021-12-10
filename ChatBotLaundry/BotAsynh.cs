using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static ChatBotLaundry.Program;

namespace ChatBotLaundry
{
    class BotAsynh
    {
        public static void BotRun(User user, WebInterface session, string button)
        {
            switch (user.Status)
            {
                case 0:
                    //session.SendMessage("Клиент");
                    BotClient(user, session, button);
                    break;
                case 1:
                    //session.SendMessage("ССК");
                    BotClient(user, session, button);
                    break;
                case 2:
                    //session.SendMessage("Открывающий");
                    break;
                case 3:
                    session.SendMessage("Администратор");
                    //BotAdmin(user, session, button);
                    break;
                case 4:
                    session.SendMessage("Вы временно заблокированны");
                    session.SendMessage("Время до конца блокировки");
                    break;
            }
        }

        //public static void BotAdmin(User user, WebInterface session, string button)
        //{
        //    switch (button)
        //    {
        //        case "clf":
        //            user.Condition = button;
        //            BotClient(user, session, button);
        //            break;
        //        case "adus":
        //            user.Condition = button;
        //            UserAdministrtion(user, session, button);
        //            break;
        //        case "adus1":
        //            user.Condition = button;
        //            UserAdministrtion(user, session, button);
        //            break;
        //        case "adus11":
        //            user.Condition = button;
        //            UserAdministrtion(user, session, button);
        //            break;
        //        case "adus12":
        //            user.Condition = button;
        //            UserAdministrtion(user, session, button);
        //            break;
        //        case "adl":
        //            user.Condition = button;
        //            break;
        //        case "re":
        //            user.Condition = button;
        //            break;
        //        case "re":
        //            break;
        //        case "re":
        //            break;
        //        case "re":
        //            break;

        //        case "b":
        //            return;
        //    }
        //}

        //ветки диалога
        //private static void UserAdministrtion(User user, WebInterface session, string button)
        //{
        //    while (true)
        //    {
        //        session.SendButtons(GetAdminUsersButtons(user));
        //        var buttonClicked1 = session.GetButton();
        //        if (buttonClicked1 == "b") break;
        //        while (true)
        //        {
        //            session.SendMessage(Data.Statuses[int.Parse(buttonClicked1)]);
        //            var listIds = GetUsersIdsList(buttonClicked1);
        //            string stringListIds = GetStringListIds(listIds);
        //            session.SendMessage(stringListIds);
        //            session.SendButtons(GetAdminStatusButtons(user, int.Parse(buttonClicked1)));
        //            var buttonClicked2 = session.GetButton();
        //            switch (buttonClicked2)
        //            {
        //                case "1":
        //                    session.SendMessage("Введите id пользователя");
        //                    var id = long.Parse(session.GetMessage());
        //                    Data.Users.Add(new User { ID = id, Status = int.Parse(buttonClicked1) });
        //                    break;
        //                case "2":
        //                    session.SendMessage("Введите номер пользователя");
        //                    var num = int.Parse(session.GetMessage());
        //                    Data.Users.Find(delegate (User user)
        //                    {
        //                        return user.ID == listIds[num];
        //                    }).Status = 0;
        //                    break;
        //                case "b":
        //                    break;
        //            }
        //            if (buttonClicked2 == "b") break;
        //        }
        //    }
        //}

        public static void BotClient(User user, WebInterface session, string button)
        {
            switch (user.Condition)
            {
                case "st":
                    user.Condition = "cl";
                    session.SendMessage("Выберите действие:");
                    session.SendButtons(GetClientMenuButtons(user));
                    break;
                case "cl":
                    switch (button)
                    {
                        case "cln":
                            user.Condition = button;
                            session.SendMessage("Запись");
                            session.SendButtons(GetDayButtons(user.Status));
                            break;
                        case "cldn":
                            user.Condition = button;
                            session.SendMessage("Отмена записи");
                            session.SendButtons(GetNotesButtons(user));
                            break;
                        case "clnt":
                            user.Condition = "cl";
                            user.NotificationStatus = !user.NotificationStatus;
                            if (user.NotificationStatus)
                                session.SendMessage("Уведомления включены");
                            else
                                session.SendMessage("Уведомления выключены");
                            Thread.Sleep(1000);
                            session.SendMessage("Выберите действие:");
                            session.SendButtons(GetClientMenuButtons(user));
                            break;
                        case "info":
                            user.Condition = "cl";
                            session.SendMessage("Какая-то важная инфа по стирке..."); 
                            Thread.Sleep(1000);
                            session.SendMessage("Выберите действие:");
                            session.SendButtons(GetClientMenuButtons(user));
                            break;
                        case "b":
                            user.Condition = "st";
                            return;
                    };
                    break;
                case "cln":
                    if (button != "b")
                    {
                        user.Condition = "clnd";
                        user.note[0] = int.Parse(button);
                        session.SendButtons(GetTimeButtons(user.note[0]));
                    }
                    else
                    {
                        user.Condition = "cl";
                        session.SendMessage("Выберите действие:");
                        session.SendButtons(GetClientMenuButtons(user));
                    }
                    break;
                case "clnd":
                    if (button != "b")
                    {
                        user.Condition = "clndt";
                        user.note[1] = int.Parse(button);
                        session.SendButtons(GetAmountButtons(user.note[0], user.note[1]));
                        break;
                    }
                    else
                    {
                        user.Condition = "cln";
                        session.SendMessage("Запись");
                        session.SendButtons(GetDayButtons(user.Status));
                        break;
                    }
                case "clndt":
                    if (button != "b")
                    {
                        user.Condition = "cl";
                        user.note[2] = int.Parse(button);
                        MakeNote(user, user.note[0], user.note[1], user.note[2]);
                        session.SendMessage("Выберите действие:");
                        session.SendButtons(GetClientMenuButtons(user));
                        break;
                    }
                    else
                    {
                        user.Condition = "clnd";
                        session.SendButtons(GetTimeButtons(user.note[0]));
                        break;
                    }
                        
                case "cldn":
                    if (button != "b")
                    {
                        user.Condition = "cl";
                        var selectedNote = int.Parse(button);
                        RemoveNote(user, selectedNote);
                        session.SendMessage("Выберите действие:");
                        session.SendButtons(GetClientMenuButtons(user));
                        break;
                    }
                    else
                    {
                        user.Condition = "cl";
                        session.SendMessage("Выберите действие:");
                        session.SendButtons(GetClientMenuButtons(user));
                        break;
                    }
            }
        }

        //методы клиента
        private static void MakeNote(User user, int selectedDay, int selectedTime, int selectedAmount)
        {

            var note = new TimeNote(
                                    user.ID,
                                    selectedDay,
                                    selectedTime,
                                    selectedAmount
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
            Data.Notes.Remove(notes[selectedNote]);
        }

        //методы получения кнопок
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
                var button = new string[] { notes[i].ToString() + "\n", i.ToString() };
                buttons.Add(button);
            }
            return buttons;
        }

        private static List<string[]> GetAmountButtons(int day, int time)
        {
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

        private static List<string[]> GetTimeButtons(int day)
        {
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
                clientMenuButtons.Add(new[] { "Записаться в прачечную", "cln" });
            if (Data.Notes.FindIndex(delegate (TimeNote note)
            {
                return note.UserID == user.ID;
            }
            ) != -1)
                clientMenuButtons.Add(new[] { "Отмена", "cldn" });
            clientMenuButtons.Add(new[] { "Выкл уведомления", "clnt" });
            clientMenuButtons.Add(new[] { "FAQ", "info" });
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
        private static List<long> GetUsersIdsList(string buttonClicked1)
        {
            var listIds = new List<long>();
            foreach (var userCategory in Data.Users)
                if (userCategory.Status == int.Parse(buttonClicked1))
                    listIds.Add(userCategory.ID);
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

        //методы получения кнопок
        private static List<string[]> GetAdminMenuButtons(User user)
        {
            var buttons = new List<string[]> {
                new[] { "Функции клиента", "cl" },
                new[] { "Администрирование пользователей", "adus" },
                new[] { "Администрирование прачечной", "adl" },
                new[] { "Отчетность", "re" },
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

    }
}