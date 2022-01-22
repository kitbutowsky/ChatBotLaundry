using System;
using System.Collections.Generic;
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
                    DataMethods.Update.Condition(user);
                    return;
                case "op":
                    user.Condition = button;
                    DataMethods.Update.Condition(user);
                    return;
                case "ad":
                    user.Condition = button;
                    DataMethods.Update.Condition(user);
                    return;
                case "re":
                    WebInterface.SendMessage(user.ID, "Отчетность:");
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
                        DataMethods.Update.Condition(user);
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
                                                DataMethods.Update.Condition(us);
                                                WebInterface.SendButtons(us.ID, "Выберите действие:", GetButtons.Cl(user));
                                                break;
                                            case 2:
                                                us.Condition = "op";
                                                DataMethods.Update.Condition(us);
                                                WebInterface.SendButtons(us.ID, "Функции открывающего:", GetButtons.Op(us));
                                                break;
                                            case 3:
                                                us.Condition = "adm";
                                                DataMethods.Update.Condition(us);
                                                WebInterface.SendButtons(us.ID, "Выберите действие:", GetButtons.Adm());
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Data.Users.Add(
                                            new User(
                                                usId, 
                                                TimeSpan.Zero,
                                                DateTime.MinValue,
                                                int.Parse(user.adminIdsList.Item2)
                                                )
                                            );
                                        us = Data.Users[Data.Users.Count - 1];
                                        var info = new List<object> {
                                            us.ID,
                                            us.Status,
                                            us.NotificationStatus,
                                            us.Blocked.Item1,
                                            us.Blocked.Item2,
                                            us.Blocked.Item3,
                                            us.Condition,
                                            us.WashCounter,
                                            us.OpenerTimes,
                                            us.AverageOpenerTime,
                                            us.PasswordTries
                                        };
                                        DataMethods.Update.UserUpdates(us, info, fullUpdate: true);
                                    }
                                        
                                        
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
                                    us.Blocked = (false, DateTime.UtcNow, -1);
                                    WebInterface.SendMessage(us.ID, "Блокировка снята");
                                }
                                if (us.Status != 0)
                                {
                                    us.Status = 0;
                                    WebInterface.SendMessage(us.ID, "Теперь вы клиент");
                                }
                                us.Condition = "cl";
                                DataMethods.Update.Condition(us);
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
                            return;
                        case "pas":
                            user.Condition = button;
                            return;
                        case "w":
                            user.Condition = button;
                            return;
                        case "b":
                            user.Condition = "ad";
                            DataMethods.Update.Condition(user);
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
                        if (int.TryParse(msg, out int selectedNote) && (selectedNote <= user.notes.Count && selectedNote > 0))
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
                                DataMethods.Update.Condition(user);
                                break;
                            case 2:
                                user.Condition = "op";
                                DataMethods.Update.Condition(user);
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
                                DataMethods.MakeNote(user, user.note[0], user.note[1], user.note[2]);
                            }
                            else
                                user.Condition = "clnd";
                        }

                public static void Cldn(User user, string button)
                {
                    if (button != "b")
                    {
                        if (int.TryParse(button, out var selectedNote)){ 
                            DataMethods.RemoveNote(selectedNote, user);
                            if (user.notes.Count == 0)
                            {
                                user.Condition = "cl";
                                WebInterface.SendMessage(user.ID, "Все записи отменены");
                            }   
                        }
                    }
                    else
                        user.Condition = "cl";
                }

        //функции открывающего
        public static void Op(User user, string button)
        {
            switch (button)
            {
                case "opd":
                    user.Condition = button;
                    break;
                case "opt":
                    user.Condition = button;
                    break;
                case "b":
                    if (user.Status == 3)
                    {
                        user.Condition = "adm";
                        DataMethods.Update.Condition(user);
                    };
                    break;
            }
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
                        return;
                    }
                    var id = int.Parse(button);
                    var blU = Data.Users.Find(delegate (User usr) { return usr.ID == id; });
                    blU.Blocked = (true, DateTime.UtcNow.AddDays(7), +1 );
                    user.OpenerIdsList.Remove(id);
                }
            
                                    public static void MakeOpenerNote(User user)
                                    {
                                        foreach (var id in user.OpenerIdsList)
                                            Data.Users.Find(delegate (User usr) { return usr.ID == id; }).WashCounter++;
                                        user.OpenerTimes++;
                                        user.AverageOpenerTime = ((user.AverageOpenerTime * (user.OpenerTimes-1)) + new TimeSpan(0, DateTime.UtcNow.Minute, DateTime.UtcNow.Second))/ user.OpenerTimes;
                                    }
            
            public static void Opt(User user, string button)
            {
                if (button != "b")
                {
                    if (int.TryParse(button, out user.opnote[0]))
                        user.Condition = "optd";
                    else
                        user.Condition = "op";
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