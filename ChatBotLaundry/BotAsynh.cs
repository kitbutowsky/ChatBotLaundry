﻿using System;
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
                case 1:
                    BotClient(user, session, button);
                    break;
                case 2:
                    break;
                case 3:
                    session.SendMessage("Администратор");
                    BotAdmin(user, session, button);
                    break;
                case 4:
                    session.SendMessage("Вы временно заблокированны");
                    session.SendMessage("Время до конца блокировки");
                    break;
            }
        }

        public static void BotAdmin(User user, WebInterface session, string button)
        {
            switch (user.Condition)
            {
                case "st":
                    user.Condition = "adm";
                    session.SendMessage("Выберите действие:");
                    session.SendButtons(GetAdminMenuButtons());
                    return;
                case "adm":
                    AdmModule(user, session, button);
                    return;
                case "ad":
                    AdModule(user, session, button);
                    return;
                case "us":
                    UsModule(user, session, button);
                    return;
                case "usrs":
                    UsrsModule(user, session, button);
                    return;
                case "l":
                    LModule(user, session, button);
                    return;
                case "info":
                    InfoModule(user, session, button);
                    return;
                case "time":
                    TimeModule(user, session, button);
                    return;
            }
            ClientBrunch(user, session, button);
        }

        public static void BotClient(User user, WebInterface session, string button)
        {
            switch (user.Condition)
            {
                case "st":
                    user.Condition = "cl";
                    session.SendMessage("Выберите действие:");
                    session.SendButtons(GetClientMenuButtons(user));
                    return;

            }
            ClientBrunch(user, session, button);
        }


        //ветки диалогов
        private static void ClientBrunch(User user, WebInterface session, string button)
        {
            switch (user.Condition)
            {
                case "cl":
                    ClModule(user, session, button);
                    return;
                case "cln":
                    ClnModule(user, session, button);
                    return;
                case "clnd":
                    ClndModule(user, session, button);
                    return;
                case "clndt":
                    ClndtModule(user, session, button);
                    return;
                case "cldn":
                    CldnModule(user, session, button);
                    return;
                case "l":
                    LModule(user, session, button);
                    return;
                case "info":
                    InfoModule(user, session, button);
                    return;
                case "time":
                    TimeModule(user, session, button);
                    return;
            }
        }

        //модули диалогов(модули возвращают предыдущее/следующее состояния, выполняют действия)
        private static void AdmModule(User user, WebInterface session, string button)
        {
            switch (button)
            {
                case "cl":
                    user.Condition = button;
                    session.SendMessage("Выберите действие:");
                    session.SendButtons(GetClientMenuButtons(user));
                    return;
                case "op":
                    user.Condition = button;
                    session.SendMessage("Функции открывающего");
                    return;
                case "ad":
                    user.Condition = button;
                    session.SendMessage("Выберите действие:");
                    session.SendButtons(GetAdminButtons());
                    return;
                case "re":
                    user.Condition = button;
                    session.SendMessage("Отчетность:");
                    return;
                case "b":
                    user.Condition = "st";
                    return;
            };
        }
        
            private static void ClModule(User user, WebInterface session, string button)
            {
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
                        session.SendMessage(Data.Info);
                        Thread.Sleep(1000);
                        session.SendMessage("Выберите действие:");
                        session.SendButtons(GetClientMenuButtons(user));
                        break;
                    case "b":
                        user.Condition = "st";
                        break;
                };
            }

                private static void ClnModule(User user, WebInterface session, string button)
                {
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
                }

                    private static void ClndModule(User user, WebInterface session, string button)
                    {
                        if (button != "b")
                        {
                            user.Condition = "clndt";
                            user.note[1] = int.Parse(button);
                            session.SendButtons(GetAmountButtons(user.note[0], user.note[1]));
                        }
                        else
                        {
                            user.Condition = "cln";
                            session.SendMessage("Запись");
                            session.SendButtons(GetDayButtons(user.Status));

                        }
                    }

                        private static void ClndtModule(User user, WebInterface session, string button)
                    {
                        if (button != "b")
                        {
                            user.Condition = "cl";
                            user.note[2] = int.Parse(button);
                            MakeNote(user, user.note[0], user.note[1], user.note[2]);
                            session.SendMessage("Выберите действие:");
                            session.SendButtons(GetClientMenuButtons(user));
                        }
                        else
                        {
                            user.Condition = "clnd";
                            session.SendButtons(GetTimeButtons(user.note[0]));
                        }
                    }
        
                private static void CldnModule(User user, WebInterface session, string button)
                {
                    if (button != "b")
                    {
                        var selectedNote = int.Parse(button);
                        RemoveNote(user, selectedNote);
                    }
                    user.Condition = "cl";
                    session.SendMessage("Выберите действие:");
                    session.SendButtons(GetClientMenuButtons(user));
                }

            private static void AdModule(User user, WebInterface session, string button)
            {
                switch (button)
                {
                    case "us":
                        user.Condition = button;
                        session.SendButtons(GetAdminUsersButtons());
                        return;
                    case "l":
                        user.Condition = button;
                        session.SendMessage("Выберите действие:");
                        session.SendButtons(GetAdminLaundryButtons());
                        return;
                    case "n":
                        session.SendMessage(GetAllNotesList());
                        session.SendMessage("Введите номер записи для отмены или напишите \"назад\":");
                        var msg = session.GetMessage().Item2;
                        int num;
                        while (msg != "назад")
                        {
                            if (int.TryParse(msg, out num))
                            {
                                Data.Notes.RemoveAt(num);
                                session.SendMessage(GetAllNotesList());
                            }
                            session.SendMessage("Введите номер записи для отмены или напишите \"назад\":");
                            msg = session.GetMessage().Item2;//todo
                        }
                        session.SendMessage("Выберите действие:");
                        session.SendButtons(GetAdminButtons());
                        return;
                    case "b":
                        user.Condition = "adm";
                        session.SendMessage("Выберите действие:");
                        session.SendButtons(GetAdminMenuButtons());
                        return;
                };
            }

                private static void UsModule(User user, WebInterface session, string button)
                {
                    switch (button)
                    {
                        case "b":
                            user.Condition = "ad";
                            session.SendMessage("Выберите действие:");
                            session.SendButtons(GetAdminButtons());
                            return;
                        default:
                            user.Condition = "usrs";
                            user.adminIdsList = (GetUsersIdsList(button), int.Parse(button));
                            session.SendMessage(ListToNumerableStringList(user.adminIdsList.Item1));
                            session.SendButtons(GetAdminStatusButtons(user.adminIdsList.Item2));
                            return;
                    };
                }
        
                    private static void UsrsModule(User user, WebInterface session, string button)
                    {
                        switch (button)
                        {
                            case "a":
                                session.SendMessage("Введите id пользователя");
                                var msg = session.GetMessage().Item2;
                                var usId = long.Parse(msg);
                                Data.Users.Add(new User { ID = usId, Status = user.adminIdsList.Item2 });
                                break;
                            case "d":
                                session.SendMessage("Введите номер пользователя");
                                msg = session.GetMessage().Item2;
                                var num = int.Parse(msg);
                                Data.Users.Find(delegate (User user)
                                {
                                    return user.ID == user.adminIdsList.Item1[num];
                                }).Status = 0;
                                break;
                            case "b":
                                user.Condition = "us";
                                session.SendButtons(GetAdminUsersButtons());
                                return;
                            default:
                                return;
                        }
                        user.adminIdsList.Item1 = GetUsersIdsList(user.adminIdsList.Item2.ToString());
                        session.SendMessage(ListToNumerableStringList(user.adminIdsList.Item1));
                        session.SendButtons(GetAdminStatusButtons(user.adminIdsList.Item2));
                    }

                private static void LModule(User user, WebInterface session, string button)
                {
                    switch (button)
                    {
                        case "info":
                            user.Condition = button;
                            session.SendMessage(Data.Info);
                            session.SendMessage("Введите новую информацию о прачке");
                            Data.NewInfo = session.GetMessage().Item2;
                            session.SendButtons(GetСonfirmationButtons());
                            return;
                        case "time":
                            user.Condition = button;
                            session.SendMessage(WashesHoursToString());
                            session.SendButtons(GetTimeAdButtons());
                            break;
                        case "pas":
                            user.Condition = "l";
                            session.SendMessage("Введите пароль или напишите \"отмена\":");
                            var password = session.GetMessage().Item2;
                            for (var i = 3; i > 0; i--)
                                if (password == Data.Password)
                                {
                                    session.SendMessage("Введите новый пароль");
                                    Data.Password = session.GetMessage().Item2;
                                    session.SendMessage("Пароль изменен:");
                                    Thread.Sleep(3000);
                                    return;
                                }
                                else if(password == "отмена")
                                {
                                    session.SendButtons(GetAdminLaundryButtons());
                                    return;
                                }
                                else
                                {
                                    session.SendMessage("Вы ввели не правильный пароль\nОсталось " + i.ToString() + " попыток\nВведите пароль или напишите \"отмена\":");
                                    password = session.GetMessage().Item2;
                                }
                            session.SendButtons(GetAdminLaundryButtons());
                            return;
                        case "b":
                            user.Condition = "ad";
                            session.SendButtons(GetAdminButtons() );
                            return;
                    }
                }

                    private static void TimeModule(User user, WebInterface session, string button)
        {
            switch (button)
            {
                case "b":
                    user.Condition = "l";
                    session.SendMessage("Выберите действие:");
                    session.SendButtons(GetAdminLaundryButtons());
                    return;
            }
            if (Data.WashesHours.Contains(int.Parse(button)))
                Data.WashesHours.Remove(int.Parse(button));
            else
                Data.WashesHours.Add(int.Parse(button));
            session.SendMessage(WashesHoursToString());
            session.SendButtons(GetTimeAdButtons());
        }

                    private static void InfoModule(User user, WebInterface session, string button)
        {
            switch (button)
            {
                case "save":
                    Data.Info = Data.NewInfo;
                    session.SendMessage("Информация изменена");
                    Thread.Sleep(1000);
                    break;
                case "b":
                    break;
                default:
                    return;
            }
            user.Condition = "l";
            session.SendButtons(GetAdminLaundryButtons());
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

        //методы админа
        private static List<long> GetUsersIdsList(string buttonClicked1)
        {
            var listIds = new List<long>();
            foreach (var userCategory in Data.Users)
                if (userCategory.Status == int.Parse(buttonClicked1))
                    listIds.Add(userCategory.ID);
            return listIds;
        }

        private static string ListToNumerableStringList(List<long> list)
        {
            string stringListIds = "";
            var i = 0;
            foreach (var id in list)
            {
                stringListIds += i.ToString() + " " + id.ToString() + "\n";
                i++;
            }
            return stringListIds;
        }

        private static string WashesHoursToString()
        {
            var washesHoursData = "";
            foreach (var id in Data.WashesHours)
                washesHoursData += id.ToString() + ":00 \n";
            return washesHoursData;
        }

        private static string GetAllNotesList()
        {
            var list = "";
            for (var i = 0; i < Data.Notes.Count; i++)
            {
                list += i.ToString() + ' ' + Data.Notes[i].ToString() + "\n";
            }
            if (Data.Notes.Count == 0)
                list = "Нет записей";
            return list;
        }


        //методы получения кнопок
        private static List<string[]> GetСonfirmationButtons()
        {
            var buttons = new List<string[]>();
            if (Data.NewInfo.Length != 0)
                buttons.Add(new string[] { "Сохранить изменения" , "save" });
            buttons.Add(new string[] { "Отмена", "b" });
            return buttons;
        }

        private static List<string[]> GetAdminLaundryButtons()
        {
            var buttons = new List<string[]> {
                new[] { "Изменить информацию о прачке", "info" },
                new[] { "Изменить время", "time" },
                new[] { "Изменить количество машинок", "w" }
            };
            return buttons;
        }

        private static List<string[]> GetAdminButtons()
        {
            var userAdminButtons = new List<string[]>
                            {
                                new[] { "Пользователи", "us" },
                                new[] { "Прачка", "l" }
                            };
            if (Data.Notes.Count != 0)
                userAdminButtons.Add(new[] { "Записи", "n" });
            return userAdminButtons;
        }

        private static List<string[]> GetTimeAdButtons()
        {
            var buttons = new List<string[]>();
            for (var t = 8; t < 23; t++)
                buttons.Add(new[] { t.ToString() + ":00", t.ToString() });
            return buttons;
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

        private static List<string[]> GetAdminMenuButtons()
        {
            var buttons = new List<string[]> {
                new[] { "Функции клиента", "cl" },
                new[] { "Администрирование", "ad" },
                new[] { "Функции открывающего", "op" },
                new[] { "Отчетность", "re" },
                new[] { "Выйти", "b" }
            };
            return buttons;
        }

        private static List<string[]> GetAdminUsersButtons()
        {
            var buttons = new List<string[]> {
                new[] { "ССК", "1" },
                new[] { "Открывающие", "2" },
                new[] { "Администраторы", "3" },
                new[] { "Черный список", "4" },
            };
            return buttons;
        }

        private static List<string[]> GetAdminStatusButtons(int status)
        {
            var buttons = new List<string[]> {
                new[] { "Добавить", "a" }
            };
            if ((Data.AmountOf(status) != 0 && status != 3) || (Data.AmountOf(status) > 1 && status == 3))
                buttons.Add(new[] { "Удалить", "d" });
            return buttons;
        }
    }
}