using System;
using System.Threading;

namespace ChatBotLaundry
{
    internal class Modules
    {
        //модули диалогов(модули возвращают предыдущее/следующее состояния, выполняют действия)
        public static void Adm(User user,  string button)
        {
            switch (button)
            {
                case "cl":
                    user.Condition = button;
                    return;
                case "op":
                    user.Condition = button;
                    return;
                case "ad":
                    user.Condition = button;
                    return;
                case "re":
                    WebInterface.SendMessage(user.ID, "Отчетность:");
                    return;
                case "infad":
                    user.Condition = button;
                    return;
                case "b":
                    user.Condition = "st";
                    return;
            };
        }

            public static void Ad(User user, string button)
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
                        user.notes = DataMethods.GetNotes(user.ID, true);
                        return;
                    case "b":
                        user.Condition = "adm";
                        return;
                };
            }

                public static void Us(User user, string button)
                {
                    Predicate<User> condition;
                    switch (button)
                    {
                        case "b":
                            user.Condition = "ad";
                            return;
                        case "bl":
                            user.Condition = "usrs";
                            condition = delegate (User us) { return us.Blocked.Item1; };
                            break;
                        default:
                            user.Condition = "usrs";
                            condition = delegate (User us) { return us.Status == int.Parse(button); };
                            break;
                    };
                    user.adminIdsList = (DataMethods.GetUsersIdsList(condition), button);
                }

                    public static void Usrs(User user, string button)
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
                    }

                        public static void UsrsAdd(User user, string button, string msg)
                        {
                            if (button == "b")
                            {
                                user.Condition = "usrs";
                                return;
                            }
                            if (msg.StartsWith("https://vk.com/im?sel=") && long.TryParse(msg[(msg.IndexOf('=') + 1)..], out long usId))
                            {
                                user.Condition = "usrs";
                                var us = Data.Users.Find(delegate (User usr) { return usr.ID == usId; });
                                if (us.Status == int.Parse(user.adminIdsList.Item2)) 
                                { 
                                    WebInterface.SendMessage(user.ID, "Этот пользаватель уже " + StaticDataAndMetods.Statuses[us.Status]);
                                    return;
                                }
                                if (user.adminIdsList.Item2 != "bl")
                                {
                                    if (us != null)
                                    {
                                        us.Status = int.Parse(user.adminIdsList.Item2);
                                        WebInterface.SendMessage(usId, "Теперь вы " + StaticDataAndMetods.Statuses[us.Status]);
                                        switch (us.Status)
                                        {
                                            case 1:
                                                us.Condition = "cl";
                                                WebInterface.SendButtons(us.ID, "Выберите действие:", GetButtons.Cl(user));
                                                break;
                                            case 2:
                                                us.Condition = "op";
                                                WebInterface.SendButtons(us.ID, "Функции открывающего:", GetButtons.Op(us));
                                                break;
                                            case 3:
                                                us.Condition = "adm";
                                                WebInterface.SendButtons(us.ID, "Выберите действие:", GetButtons.Adm());
                                                break;
                                        }
                                    }
                                    else
                                        Data.Users.Add(new User { ID = usId, Status = int.Parse(user.adminIdsList.Item2) });
                                }
                                user.adminIdsList.Item1.Add(usId);
                            }
                            else
                                WebInterface.SendMessage(user.ID, "Некоректный ввод! повторите попытку");
                        }

                        public static void UsrsDel(User user, string button, string msg)
                        {
                            if (button == "b")
                            {
                                user.Condition = "usrs";
                                return;
                            }
                            if (int.TryParse(msg, out int num) && num >= 0 && num < user.adminIdsList.Item1.Count)
                            {
                                var us = Data.Users.Find(delegate (User usr) { return usr.ID == user.adminIdsList.Item1[num]; });
                                if (user.adminIdsList.Item2 == "bl")
                                {
                                    us.Blocked.Item1 = false;
                                    us.Blocked.Item2 = DateTime.UtcNow; 
                                    us.Blocked.Item3--;
                                    WebInterface.SendMessage(us.ID, "Блокировка снята");
                                }
                                if (us.Status != 0)
                                {
                                    us.Status = 0;
                                    WebInterface.SendMessage(us.ID, "Теперь вы клиент");
                                }
                                us.Condition = "cl";
                                WebInterface.SendButtons(us.ID, "Выберите действие:", GetButtons.Cl(user));
                                user.adminIdsList.Item1.RemoveAt(num);
                                user.Condition = "usrs";
                            }
                            else
                                WebInterface.SendMessage(user.ID, "Некоректный ввод! повторите попытку");
                        }

                public static void L(User user, string button)
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
                        case "w":
                            user.Condition = button;
                            return;
                        case "b":
                            user.Condition = "ad";
                            return;
                    }
                }

                    public static void InfAd(User user, string button, string msg)
                    {
                        if (button == "b")
                            user.Condition = "l";
                        else
                        {
                            user.Condition = "infadc";//Info administration confirmation
                            Data.NewInfo = msg;
                        }
                    }

                        public static void InfAdC(User user, string button)
                        {
                            switch (button)
                            {
                                case "save":
                                    Data.Info = Data.NewInfo;
                                    WebInterface.SendMessage(user.ID, "Информация изменена");
                                    Thread.Sleep(1000);
                                    break;
                                case "b":
                                    break;
                            }
                            user.Condition = "l";
                        }

                    public static void Time(User user, string button)
                    {
                        if (button == "b")
                        {
                            user.Condition = "l";
                            return;
                        }
                        var newtime = int.Parse(button) - StaticDataAndMetods.Timezone;
                        if (newtime <= 0)
                        {
                            newtime += 24;
                        }
                        if (Data.WashesHours.Contains(newtime))
                            Data.WashesHours.Remove(newtime);
                        else
                        {
                            Data.WashesHours.Add(newtime);
                            Data.WashesHours.Sort();
                        }
                    }
                    
        
                    public static void W(User user, string button)
                    {
                        user.Condition = "l";
                        if (button == "b")
                            return;
                        Data.WashesAmount = int.Parse(button);
                        WebInterface.SendMessage(user.ID, "Теперь машинок " + button);
                    }

                    public static void Pas(User user, string button, string msg)
                    {
                        if (button == "b")
                            user.Condition = "l";
                        else if (msg == Data.Password)
                            user.Condition = "newpas";
                        else
                        {
                            user.PasswordTries--;
                            WebInterface.SendMessage(user.ID, "Вы ввели не правильный пароль\nОсталось " + user.PasswordTries.ToString() + " попыток");
                            if (user.PasswordTries == 0)
                                user.Condition = "l";
                        }
                    }

                        public static void NewPas(User user, string msg)
                        {
                            user.Condition = "l";
                            user.PasswordTries = 3;
                            Data.Password = msg;
                            WebInterface.SendMessage(user.ID, "Пароль изменен!");
                            WebInterface.SendMessage(user.ID, "Новый пароль: " + Data.Password);
                            Thread.Sleep(3000);
                        }

                public static void N(User user, string button, string msg)
                {
                    if (button != "b")
                    {
                        if (int.TryParse(msg, out int selectedNote) && (selectedNote < user.notes.Count && selectedNote >= 0))
                        {
                            DataMethods.RemoveNote(selectedNote, user);
                            if (user.notes.Count == 0)
                            {
                                user.Condition = "ad";
                                WebInterface.SendMessage(user.ID, "Все записи отменены");
                            } 
                        }
                        else
                            WebInterface.SendMessage(user.ID, "Ошибка ввода!");
                            Thread.Sleep(1000);
                    }
                    else
                        user.Condition = "ad";
                }

            //функции клиента
            public static void Cl(User user, string button)
            {
                switch (button)
                {
                    case "ddop":
                        WebInterface.SendMessage(user.ID, 
                            "В ближайшее время с вами свяжется администратор\n" +
                            "Если вам не откроют в течении 10 минут просто уходите в комнату\n" +
                            "Хорошего вам дня!"); 
                        Predicate<User> condition;
                        condition = delegate (User us) { return us.Status == 3; };
                        foreach (var admin in DataMethods.GetUsersIdsList(condition))
                            WebInterface.SendMessage(admin,
                            "Пользователю https://vk.com/im?sel=" + user.ID.ToString() + 
                            " не открыли прачку\n" +
                            "Он ждет!"); 
                        break;
                    case "cln":
                        user.Condition = button;
                        break;
                    case "cldn":
                        user.Condition = button;
                        user.notes = DataMethods.GetNotes(user.ID);
                        break;
                    case "clnt":
                        user.NotificationStatus = !user.NotificationStatus;
                        if (user.NotificationStatus)
                            WebInterface.SendMessage(user.ID, "Уведомления включены");
                        else
                            WebInterface.SendMessage(user.ID, "Уведомления выключены");
                        Thread.Sleep(1000);
                        break;
                    case "info":
                        WebInterface.SendMessage(user.ID, Data.Info);
                        Thread.Sleep(1000);
                        break;
                    case "b":
                        switch (user.Status)
                        {
                            case 3:
                                user.Condition = "adm";
                                break;
                            case 2:
                                user.Condition = "op";
                                break;
                        }
                    break;  
                };
            }

                public static void Cln(User user, string button)
                {
                    if (button != "b" && int.TryParse(button, out var note) && (note >= 0 & note < 7 ))
                    {
                        user.Condition = "clnd";
                        user.note[0] = note;
                    }
                    else
                        user.Condition = "cl";
                }

                    public static void Clnd(User user, string button)
                    {
                        if (button != "b" && int.TryParse(button, out var note))
                        {
                            user.Condition = "clndt";
                            user.note[1] = note;
                        }
                        else
                            user.Condition = "cln";
                    }

                        public static void Clndt(User user, string button)
                        {
                            if (button != "b" && int.TryParse(button, out var note))
                            {
                                user.Condition = "cl";
                                user.note[2] = note;
                                MakeNote(user, user.note[0], user.note[1], user.note[2]);
                            }
                            else
                                user.Condition = "clnd";
                        }

                                    internal static void MakeNote(User user, int selectedDay, int selectedTime, int amount)
                                    {
                                        var day = Data.Days[selectedDay];
                                        var note = new TimeNote(
                                                                user.ID,
                                                                day.Date, 
                                                                selectedTime,
                                                                day.WashesHours[selectedTime].ToTimezone(),
                                                                amount
                                                                );
                                        var max = day.EmptyTimes[selectedTime];
                                        if (amount <= max) 
                                        {
                                            //поочередно проверяет ячейки и в свободные записывает id amount раз
                                            for (var j = 0; j < day.WashesAmount; j++)
                                                if (amount != 0 && day.HoursWashesTable[selectedTime, j] == 0)
                                                {
                                                    day.HoursWashesTable[selectedTime, j] = note.UserID;
                                                    amount--;
                                                }
                                            //добавляет запись в список записей
                                            day.Notes.Add(note);
                                            WebInterface.SendMessage(user.ID, "Вы записаны");
                                        }
                                        else 
                                            WebInterface.SendMessage(user.ID,   "Кажется вы слишком долго выбирали, это место уже забронировали\n" +
                                                                                "Не отчаивайтесь! Выберите другое время)");
                                    }

                public static void Cldn(User user, string button)
                {
                    if (button != "b")
                    {
                        var selectedNote = int.Parse(button);
                        DataMethods.RemoveNote(selectedNote, user);
                        if (user.notes.Count == 0)
                        {
                            user.Condition = "cl";
                            WebInterface.SendMessage(user.ID, "Все записи отменены");
                        }   
                    }
                    else
                        user.Condition = "cl";
                }

        //функции открывающего
        public static void Op(User user, string button)
        {
            if (button == "b")
            {
                switch (user.Status)
                {
                    case 3:
                        user.Condition = "adm";
                        break;
                    case 2:
                        user.Condition = "op";
                        break;
                }
            }
            else if (button != "st" && button != "")
                user.Condition = button;
        }

            public static void Opd(User user, string button)
            {
                switch (button)
                {
                    case "allcm":
                        MakeOpenerNote(user);
                        break;
                    case "ddn":
                        user.Condition = "ddn";
                        return;
                    case "b":
                        user.OpenerIdsList.Clear();
                        break;
                };
                user.Condition = "op";
            }
                
                public static void Ddn(User user, string button)
                {
                    if (button == "y")
                    {
                        MakeOpenerNote(user);
                        user.Condition = "op";
                    }
                    var id = int.Parse(button);
                    var blU = Data.Users.Find(delegate (User usr) { return usr.ID == id; });
                    blU.Blocked.Item1 = true;
                    blU.Blocked.Item2 = DateTime.UtcNow.AddDays(7);
                    blU.Blocked.Item3++;
                    user.OpenerIdsList.Remove(id);
                }
            
                                    public static void MakeOpenerNote(User user)
                                    {
                                        foreach (var id in user.OpenerIdsList)
                                            Data.Users.Find(delegate (User usr) { return usr.ID == id; }).WashCounter++;
                                        user.OpenerTime.Add(new TimeSpan(0, DateTime.UtcNow.Minute, DateTime.UtcNow.Second));
                                    }
            
            public static void Opt(User user, string button)
            {
                if (button != "b")
                {
                    user.Condition = "optd";
                    user.opnote[0] = int.Parse(button);
                }
                else
                    user.Condition = "op";
            }
                
                public static void Optd(User user, string button)
                {
                    if (button != "b")
                    {
                        user.Condition = "op";
                        user.opnote[1] = int.Parse(button);
                        Data.Days[user.note[0]].HoursWashesOpenerTable[user.opnote[1]] = user.ID;
                    }
                    else
                        user.Condition = "opt";
                }
    }
}