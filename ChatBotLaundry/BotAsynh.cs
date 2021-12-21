using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static ChatBotLaundry.Program;

namespace ChatBotLaundry
{
    class BotAsynh
    {
        public static void BotRun(User user, WebInterface session, string button, string msg)
        {
            switch (user.Status)
            {
                case 0:
                case 1:
                    BotClient(user, session, button, msg);
                    break;
                case 2:
                    break;
                case 3:
                    BotAdmin(user, session, button, msg);
                    break;
                case 4:
                    session.SendMessage(user.ID, "Вы временно заблокированны");
                    session.SendMessage(user.ID, "Время до конца блокировки");
                    break;
            }
        }

        public static void BotAdmin(User user, WebInterface session, string button, string msg)
        {
            //Выполнение действий по нажатию кнопки
            switch (user.Condition)
            {
                case "st":
                    user.Condition = "adm";
                    break;
                case "adm":
                    AdmModule(user, session, button);
                    break;
                case "ad":
                    AdModule(user, session, button);
                    break;
                case "us":
                    UsModule(user, session, button);
                    break;
                case "usrs":
                    UsrsModule(user, session, button);
                    break;
                case "add":
                    UsrsAddModule(user, session, button, msg);
                    break;
                case "del":
                    UsrsDelModule(user, session, button, msg);
                    break;
                case "l":
                    LModule(user, session, button);
                    break;
                case "pas":
                    PasModule(user, session, button, msg);
                    break;
                case "newpas":
                    NewPasModule(user, session, msg);
                    break;
                case "infad":
                    InfAdModule(user, session, button, msg);
                    break;
                case "infadc":
                    InfAdCModule(user, session, button);
                    break;
                case "time":
                    TimeModule(user, session, button);
                    break;
                case "n":
                    NModule(user, session, msg);
                    break;
            }
            ClientActions(user, session, button);
            //отправление сообщений и кнопок по состоянию
            switch (user.Condition)
            {
                case "adm":
                    session.SendMessage(user.ID, "Выберите действие:");
                    session.SendInlineButtons(user.ID, GetButtons.Adm());
                    return;
                case "ad":
                    session.SendMessage(user.ID, "Выберите действие:");
                    session.SendInlineButtons(user.ID, GetButtons.Ad());
                    return;
                case "us":
                    session.SendInlineButtons(user.ID, GetButtons.Us());
                    return;
                case "usrs":
                    session.SendMessage(user.ID, ListToNumerableStringList(user.adminIdsList.Item1));
                    session.SendInlineButtons(user.ID, GetButtons.Usrs(user.adminIdsList.Item2));
                    return;
                case "add":
                    session.SendMessage(user.ID, ListToNumerableStringList(user.adminIdsList.Item1));
                    session.SendMessage(user.ID, "Введите id пользователя");
                    session.SendButtons(user.ID, new List<List<(string, string)>>());
                    return;
                case "del":
                    session.SendMessage(user.ID, ListToNumerableStringList(user.adminIdsList.Item1));
                    session.SendMessage(user.ID, "Введите номер пользователя");
                    session.SendButtons(user.ID, new List<List<(string, string)>>());
                    return;
                case "l":
                    session.SendMessage(user.ID, "Выберите действие:");
                    session.SendInlineButtons(user.ID, GetButtons.L());
                    return;
                case "pas":
                    session.SendButtons(user.ID, new List<List<(string, string)>>());
                    session.SendMessage(user.ID, "Введите пароль:");
                    return;
                case "newpas":
                    session.SendMessage(user.ID, "Введите новый пароль");
                    return;
                case "infad":
                    session.SendMessage(user.ID, Data.Info);
                    session.SendMessage(user.ID, "Введите новую информацию о прачке");
                    session.SendButtons(user.ID, new List<List<(string, string)>>());
                    return;
                case "infadc":
                    session.SendInlineButtons(user.ID, GetButtons.Infadc());
                    return;
                case "time":
                    session.SendMessage(user.ID, Data.WashesHoursToString());
                    session.SendButtons(user.ID, GetButtons.Time());
                    return;
                case "n":
                    session.SendMessage(user.ID, Data.AllNotesToStringList());
                    session.SendMessage(user.ID, "Введите номер записи для отмены или напишите \"назад\":");
                    return;
            }
            //для клиента
            ClientButtonsSender(user, session);
        }


        public static void BotClient(User user, WebInterface session, string button, string msg)
        {
            if (user.Status == 0 && user.Condition == "cl" && msg == Data.Password)
            {
                user.Status = 1;
                session.SendMessage(user.ID, "Теперь вы сск!");
            }
            switch (user.Condition)
            {
                case "st":
                    user.Condition = "cl";
                    break;
            }
            ClientActions(user, session, button);
            ClientButtonsSender(user, session);
        }


        //Модули действий(реакции на нажатые кнопки)
        private static void ClientActions(User user, WebInterface session, string button)
        {
            switch (user.Condition)
            {
                case "cl":
                    ClModule(user, session, button);
                    break;
                case "cln":
                    ClnModule(user, session, button);
                    break;
                case "clnd":
                    ClndModule(user, session, button);
                    break;
                case "clndt":
                    ClndtModule(user, session, button);
                    break;
                case "cldn":
                    CldnModule(user, session, button);
                    break;
            }
        }

        //Модули отправки кнопок(отправка кнопок в зависимости от состояния)
        private static void ClientButtonsSender(User user, WebInterface session)
        {
            switch (user.Condition)
            {
                case "st":
                    session.SendButtons(user.ID, new List<List<(string, string)>> { new List<(string, string)> { ("Начать", "st") } });
                    return;
                case "cl":
                    session.SendMessage(user.ID, "Выберите действие:");
                    session.SendButtons(user.ID, GetButtons.Cl(user));
                    return;
                case "cln":
                    session.SendMessage(user.ID, "Запись");
                    session.SendButtons(user.ID, GetButtons.Cln(user.Status));
                    return;
                case "clnd":
                    session.SendButtons(user.ID, GetButtons.Clnd(user.note[0]));
                    return;
                case "clndt":
                    session.SendButtons(user.ID, GetButtons.Clndt(user.note[0], user.note[1]));
                    return;
                case "cldn":
                    session.SendMessage(user.ID, "Отмена записи");
                    session.SendButtons(user.ID, GetButtons.Cldn(user));
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
                    return;
                case "op":
                    session.SendMessage(user.ID, "Функции открывающего");
                    return;
                case "ad":
                    user.Condition = button;
                    return;
                case "re":
                    user.Condition = button;
                    session.SendMessage(user.ID, "Отчетность:");
                    return;
                case "infad":
                    user.Condition = button;
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
                        break;
                    case "cldn":
                        user.Condition = button;
                        break;
                    case "clnt":
                        user.NotificationStatus = !user.NotificationStatus;
                        if (user.NotificationStatus)
                            session.SendMessage(user.ID, "Уведомления включены");
                        else
                            session.SendMessage(user.ID, "Уведомления выключены");
                        Thread.Sleep(1000);
                        break;
                    case "info":
                        session.SendMessage(user.ID, Data.Info);
                        Thread.Sleep(1000);
                        break;
                    case "b":
                        if (user.Status == 3)
                        {
                            user.Condition = "adm";
                        }
                        else
                        {
                            user.Condition = "st";
                        } 
                        break;
                };
            }

                private static void ClnModule(User user, WebInterface session, string button)
                {
                    if (button != "b")
                    {
                        user.Condition = "clnd";
                        user.note[0] = int.Parse(button);
                    }
                    else
                        user.Condition = "cl";
                }

                    private static void ClndModule(User user, WebInterface session, string button)
                    {
                        if (button != "b")
                            user.Condition = "clndt";
                        else
                            user.Condition = "cln";
                    }

                        private static void ClndtModule(User user, WebInterface session, string button)
                        {
                            if (button != "b")
                            {
                                user.Condition = "cl";
                                user.note[2] = int.Parse(button);
                                MakeNote(user, user.note[0], user.note[1], user.note[2]);
                            }
                            else
                                user.Condition = "clnd";
                        }
        
                private static void CldnModule(User user, WebInterface session, string button)
                {
                    if (button != "b")
                    {
                        var selectedNote = int.Parse(button);
                        RemoveNote(user, selectedNote);
                    }
                    user.Condition = "cl";
                }

            private static void AdModule(User user, WebInterface session, string button)
            {
                switch (button)
                {
                    case "us":
                        user.Condition = button;
                        return;
                    case "l":
                        user.Condition = button;
                        return;
                    case "n":
                        user.Condition = button;
                        return;
                    case "b":
                        user.Condition = "adm";
                        return;
                };
            }

                private static void UsModule(User user, WebInterface session, string button)
                {
                    switch (button)
                    {
                        case "b":
                            user.Condition = "ad";
                            return;
                        default:
                            user.Condition = "usrs";
                            user.adminIdsList = (GetUsersIdsList(button), int.Parse(button));
                            return;
                    };
                }
        
                    private static void UsrsModule(User user, WebInterface session, string button)
                    {
                        switch (button)
                        {
                            case "add":
                                user.Condition = "add";
                                break;
                            case "del":
                                user.Condition = "del";
                                break;
                            case "b":
                                user.Condition = "us";
                                return;
                        }
                        user.adminIdsList.Item1 = GetUsersIdsList(user.adminIdsList.Item2.ToString());
                    }

                        private static void UsrsAddModule(User user, WebInterface session, string button, string msg)
                        {
                            if (button == "b")
                            {
                                user.Condition = "usrs";
                                return;
                            }
                            if (long.TryParse(msg, out long usId)) 
                            { 
                                Data.Users.Add(new User { ID = usId, Status = user.adminIdsList.Item2 });
                                user.Condition = "usrs";
                                user.adminIdsList.Item1.Add(usId);
                            }
                            else
                                session.SendMessage(user.ID, "Некоректный ввод! повторите попытку");
                        }

                        private static void UsrsDelModule(User user, WebInterface session, string button, string msg)
                        {
                            if (button == "b")
                            {
                                user.Condition = "usrs";
                                return;
                            }
                            if (int.TryParse(msg, out int num) && num <= user.adminIdsList.Item1.Count)
                            {
                                Data.Users.Find(delegate (User usr)
                                {
                                    return usr.ID == user.adminIdsList.Item1[num];
                                }).Status = 0;
                                user.adminIdsList.Item1.RemoveAt(num);
                                user.Condition = "usrs";
                            }
                            else
                                session.SendMessage(user.ID, "Некоректный ввод! повторите попытку");
                        }
                    

                private static void LModule(User user, WebInterface session, string button)
                {
                    switch (button)
                    {
                        case "infad":
                            user.Condition = button;
                            return;
                        case "time":
                            user.Condition = button;
                            break;
                        case "pas":
                            user.Condition = button;
                            return;
                        //case "w":
                        //    user.Condition = button;
                        //    return;
                        case "b":
                            user.Condition = "ad";
                            return;
                    }
                }

                    private static void TimeModule(User user, WebInterface session, string button)
                    {
                        if (button == "b")
                        {
                            user.Condition = "l";
                            return;
                        }
                        if (Data.WashesHours.Contains(int.Parse(button)))
                            Data.WashesHours.Remove(int.Parse(button));
                        else
                        {
                            Data.WashesHours.Add(int.Parse(button));
                            Data.WashesHours.Sort();
                        }
                    }

                    private static void InfAdModule(User user, WebInterface session, string button, string msg)
                    {
                        if (button == "b")
                            user.Condition = "l";
                        else
                        {
                            user.Condition = "infadc";//Info administration confirmation
                            Data.NewInfo = msg;
                        }
                    }

                        private static void InfAdCModule(User user, WebInterface session, string button)
                        {
                            switch (button)
                            {
                                case "save":
                                    Data.Info = Data.NewInfo;
                                    session.SendMessage(user.ID, "Информация изменена");
                                    Thread.Sleep(1000);
                                    break;
                                case "b":
                                    break;
                            }
                            user.Condition = "l";
                        }
        //todo
                    private static void PasModule(User user, WebInterface session, string button, string msg)
                    {
                        if (button == "b")
                            user.Condition = "l";
                        else if (msg == Data.Password)
                            user.Condition = "newpas";
                        else
                        {
                            Data.PasswordTries--;
                            session.SendMessage(user.ID, "Вы ввели не правильный пароль\nОсталось " + Data.PasswordTries.ToString() + " попыток");
                        }
                    }

                        private static void NewPasModule(User user, WebInterface session, string msg)
                        {
                            user.Condition = "l";
                            Data.PasswordTries = 3;
                            Data.Password = msg;
                            session.SendMessage(user.ID, "Пароль изменен!");
                            session.SendMessage(user.ID, "Новый пароль: " + Data.Password.ToString());
                            Thread.Sleep(3000);
                        }

        private static void NModule(User user, WebInterface session, string msg)
                {
                    int num;
                    if (msg != "назад")
                    {
                        if (int.TryParse(msg, out num))
                        {
                            Data.Notes.RemoveAt(num);
                            session.SendMessage(user.ID, Data.AllNotesToStringList());
                        }
                        else
                            session.SendMessage(user.ID, Data.AllNotesToStringList());
                    }
                    else 
                        user.Condition = "ad";
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
    }
}