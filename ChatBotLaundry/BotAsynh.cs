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
                case 1:
                    BotClient(user, session, button);
                    break;
                case 2:
                    break;
                case 3:
                    BotAdmin(user, session, button);
                    break;
                case 4:
                    session.SendMessage(user.ID, "Вы временно заблокированны");
                    session.SendMessage(user.ID, "Время до конца блокировки");
                    break;
            }
        }

        public static void BotAdmin(User user, WebInterface session, string button)
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
                case "l":
                    LModule(user, session, button);
                    break;
                case "info":
                    InfoModule(user, session, button);
                    break;
                case "time":
                    TimeModule(user, session, button);
                    break;
            }
            ClientActions(user, session, button);
            //отправление сообщений и кнопок по состоянию
            switch (user.Condition)
            {
                case "adm":
                    session.SendMessage(user.ID, "Выберите действие:");
                    session.SendButtons(user.ID, GetAdminMenuButtons());
                    return;
                case "ad":
                    session.SendMessage(user.ID, "Выберите действие:");
                    session.SendButtons(user.ID, GetAdminButtons());
                    return;
                case "us":
                    session.SendButtons(user.ID, GetAdminUsersButtons());
                    return;
                case "usrs":
                    session.SendMessage(user.ID, ListToNumerableStringList(user.adminIdsList.Item1));
                    session.SendButtons(user.ID, GetAdminStatusButtons(user.adminIdsList.Item2));
                    return;
                case "l":
                    session.SendMessage(user.ID, "Выберите действие:");
                    session.SendButtons(user.ID, GetAdminLaundryButtons());
                    return;
                case "info":
                    InfoModule(user, session, button);
                    return;
                case "time":
                    session.SendMessage(user.ID, Data.WashesHoursToString());
                    session.SendButtons(user.ID, GetTimeAdButtons());
                    return;
            }
            //для клиента
            ClientButtonsSender(user, session);
        }


        public static void BotClient(User user, WebInterface session, string button)
        {
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
                    session.SendButtons(user.ID, GetClientMenuButtons(user));
                    return;
                case "cln":
                    session.SendMessage(user.ID, "Запись");
                    session.SendButtons(user.ID, GetDayButtons(user.Status));
                    return;
                case "cldn":
                    session.SendMessage(user.ID, "Отмена записи");
                    session.SendButtons(user.ID, GetNotesButtons(user));
                    break;
                case "clnd":
                    session.SendButtons(user.ID, GetTimeButtons(user.note[0]));
                    break;
                case "clndt":
                    session.SendButtons(user.ID, GetAmountButtons(user.note[0], user.note[1]));
                    break;
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
                    //todo
                    case "n":
                        session.SendMessage(user.ID, Data.AllNotesToStringList());
                        session.SendMessage(user.ID, "Введите номер записи для отмены или напишите \"назад\":");
                        var msg = session.GetMessage().Item2;
                        int num;
                        while (msg != "назад")
                        {
                            if (int.TryParse(msg, out num))
                            {
                                Data.Notes.RemoveAt(num);
                                session.SendMessage(user.ID, Data.AllNotesToStringList());
                            }
                            session.SendMessage(user.ID, "Введите номер записи для отмены или напишите \"назад\":");
                            msg = session.GetMessage().Item2;//todo
                        }
                        session.SendMessage(user.ID, "Выберите действие:");
                        session.SendButtons(user.ID, GetAdminButtons());
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
                            //todo
                            case "a":
                                session.SendMessage(user.ID, "Введите id пользователя");
                                var msg = session.GetMessage().Item2;
                                long usId;
                                if (long.TryParse(msg, out usId))
                                    Data.Users.Add(new User { ID = usId, Status = user.adminIdsList.Item2 });
                                else
                                    session.SendMessage(user.ID, "Некоректный ввод!");
                                break;
                            case "d":
                                session.SendMessage(user.ID, "Введите номер пользователя");
                                msg = session.GetMessage().Item2;
                                int num;
                                if (int.TryParse(msg, out num) && num <= user.adminIdsList.Item1.Count) 
                                    Data.Users.Find(delegate (User user)
                                    {
                                        return user.ID == user.adminIdsList.Item1[num];
                                    }).Status = 0;
                                else
                                    session.SendMessage(user.ID, "Некоректный ввод!");
                                break;
                            case "b":
                                user.Condition = "us";
                                return;
                        }
                        user.adminIdsList.Item1 = GetUsersIdsList(user.adminIdsList.Item2.ToString());
                    }

                private static void LModule(User user, WebInterface session, string button)
                {
                    switch (button)
                    {
                        //todo
                        case "info":
                            user.Condition = button;
                            session.SendMessage(user.ID, Data.Info);
                            session.SendMessage(user.ID, "Введите новую информацию о прачке");
                            Data.NewInfo = session.GetMessage().Item2;
                            session.SendButtons(user.ID, GetСonfirmationButtons());
                            return;
                        case "time":
                            user.Condition = button;
                            break;
                        //todo
                        case "pas":
                            session.SendMessage(user.ID, "Введите пароль или напишите \"отмена\":");
                            var password = session.GetMessage().Item2;
                            for (var i = 3; i > 0; i--)
                                if (password == Data.Password)
                                {
                                    session.SendMessage(user.ID, "Введите новый пароль");
                                    Data.Password = session.GetMessage().Item2;
                                    session.SendMessage(user.ID, "Пароль изменен:");
                                    Thread.Sleep(3000);
                                    return;
                                }
                                else if(password == "отмена")
                                {
                                    session.SendButtons(user.ID, GetAdminLaundryButtons());
                                    return;
                                }
                                else
                                {
                                    session.SendMessage(user.ID, "Вы ввели не правильный пароль\nОсталось " + i.ToString() + " попыток\nВведите пароль или напишите \"отмена\":");
                                    password = session.GetMessage().Item2;
                                }
                            return;
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

                    private static void InfoModule(User user, WebInterface session, string button)
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



        //методы получения кнопок
        private static List<List<(string, string)>> GetСonfirmationButtons()
        {
            var buttons = new List<List<(string, string)>>{
                new List<(string, string)>{
                    ("Отмена", "b")
                }
            };
            if (Data.NewInfo.Length != 0)
                buttons.Insert(0, new List<(string, string)>
                { ("Сохранить изменения" , "save")
                });
            return buttons;
        }

        private static List<List<(string, string)>> GetAdminLaundryButtons()
        {
            var buttons = new List<List<(string, string)>>{
                new List<(string, string)>
                {
                    ("Изменить информацию о прачке", "info")
                },
                new List<(string, string)>{
                    ("Изменить время", "time")
                },
                new List<(string, string)>
                {
                    ("Изменить количество машинок", "w")
                }
            }; 
            return buttons;
        }

        private static List<List<(string, string)>> GetAdminButtons()
        {
            var userAdminButtons = new List<List<(string, string)>>{
                new List<(string, string)>
                {
                    ("Пользователи", "us")
                },
                new List<(string, string)>{
                    ("Прачка", "l")
                }
            };
            if (Data.Notes.Count != 0)
                userAdminButtons.Add(new List<(string, string)>
                {
                    ("Записи", "n" )
                });
            return userAdminButtons;
        }

        private static List<List<(string, string)>> GetTimeAdButtons()
        {
            var buttons = new List<List<(string, string)>> {};
            var i = -1;
            for (var t = 8; t < 23; t++)
            {
                if ((t - 8) % 5 == 0)
                {
                    buttons.Add(new List<(string, string)>());
                    i++;
                }
                buttons[i].Add((t.ToString() + ":00", t.ToString()));
            }
            return buttons;
        }

        private static List<List<(string, string)>> GetNotesButtons(User user)
        {
            var notes = Data.Notes.FindAll(delegate (TimeNote note)
            {
                return note.UserID == user.ID;
            }
            );
            var buttons = new List<List<(string, string)>>();
            for (var i = 0; i < notes.Count; i++)
            {
                var button = new List<(string, string)> { (notes[i].ToString() + "\n", i.ToString()) };
                buttons.Add(button);
            }
            return buttons;
        }

        private static List<List<(string, string)>> GetAmountButtons(int day, int time)
        {
            var buttons = new List<List<(string, string)>>();
            for (var i = 1; i < Data.Days[day].EmptyTimes[time] + 1; i++)
            {
                var button = new List<(string, string)> { (i.ToString() + ". ", i.ToString()) };
                buttons.Add(button);
            }
            return buttons;
        }

        private static List<List<(string, string)>> GetDayButtons(int status)
        {
            var buttons = new List<List<(string, string)>>();
            for (var d = 0; d < Data.Days.Count; d++)
                if (Data.Days[d].IsFree && ((Data.Days[d].AvailableForSSK && status != 0) || (!Data.Days[d].AvailableForSSK && status == 0)))
                {
                    var button = new List<(string, string)> { (Data.Days[d].DayOfWeekR + " " + Data.Days[d].EmptySpaces.ToString(), d.ToString()) };
                    buttons.Add(button);
                }
            return buttons;
        }

        private static List<List<(string, string)>> GetTimeButtons(int day)
        {
            var buttons = new List<List<(string, string)>>();
            for (var i = 0; i < Data.Days[day].HoursWashesTable.GetLength(0); i++)
            {
                if (Data.Days[day].EmptyTimes[i] != 0)
                {
                    var button = new List<(string, string)> { (Data.WashesHours[i].ToString() + ":00 кол-во свободных машинок:" + Data.Days[day].EmptyTimes[i].ToString(), i.ToString() )};
                    buttons.Add(button);
                }
            }
            return buttons;
        }

        private static List<List<(string, string)>> GetClientMenuButtons(User user)
        {
            var clientMenuButtons = new List<List<(string, string)>> {
                new List<(string, string)>{
                    ("Выкл уведомления", "clnt"),
                    ("FAQ", "info")
                }
            };

            if (Data.Notes.FindIndex(delegate (TimeNote note)
            {
                return note.UserID == user.ID;
            }
            ) != -1)
                clientMenuButtons.Insert(0, new List<(string, string)> { ("Отмена", "cldn") });
            if (Data.Days.FindIndex(delegate (Day day)
            {
                return (day.EmptySpaces != 0) && (day.AvailableForSSK == (user.Status == 1));
            }
            ) != -1)
                clientMenuButtons.Insert(0, new List<(string, string)>{("Записаться в прачечную", "cln")} );
            if (!user.NotificationStatus)
            {
                var repl = clientMenuButtons[^1][0];
                repl.Item1 = "Вкл уведомления";
                clientMenuButtons[^1][0] = repl;
            }
            else
            {
                var repl = clientMenuButtons[^1][0];
                repl.Item1 = "Выкл уведомления";
                clientMenuButtons[^1][0] = repl;
            }
            if (user.Status != 3 && user.Status != 2)
            {
                clientMenuButtons.Add(new List<(string, string)>{("Выйти", "b")});
            }
            return clientMenuButtons;
        }

        private static List<List<(string, string)>> GetAdminMenuButtons()
        {
            var buttons = new List<List<(string, string)>> {
                new List<(string, string)>{
                    ("Функции клиента", "cl"),
                    ("Функции открывающего", "op")
                },
                new List<(string, string)>{
                    ("Администрирование", "ad"),
                    ("Отчетность", "re")
                },
                new List<(string, string)>{
                    ("Выйти", "b")
                }
            };
            return buttons;
        }

        private static List<List<(string, string)>> GetAdminUsersButtons()
        {
            var buttons = new List<List<(string, string)>> { 
                new List<(string, string)>{
                    ("ССК", "1"),
                    ("Открывающие", "2")
                },
                new List<(string, string)>{
                    ("Администраторы", "3"),
                    ("Черный список", "4"),
                }
            };
            return buttons;
        }

        private static List<List<(string, string)>> GetAdminStatusButtons(int status)
        {
            var buttons = new List<List<(string, string)>> {
                new List<(string, string)>{("Добавить", "a")}
            };
            if ((Data.AmountOf(status) != 0 && status != 3) || (Data.AmountOf(status) > 1 && status == 3))
                buttons[0].Add(("Удалить", "d"));
            return buttons;
        }
    }
}