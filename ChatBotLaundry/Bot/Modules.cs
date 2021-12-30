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
                    WebInterface.SendMessage(user.ID, "Функции открывающего");
                    return;
                case "ad":
                    user.Condition = button;
                    return;
                case "re":
                    user.Condition = button;
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
                    user.adminIdsList = (BotAsynh.GetUsersIdsList(condition), button);
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
                            if (long.TryParse(msg, out long usId))
                            {
                                user.Condition = "usrs";
                                if (user.adminIdsList.Item2 == "bl")
                                {
                                    Data.Users.Find(delegate (User usr){return usr.ID == usId;}).Blocked = (true, DateTime.Now);
                                } 
                                else
                                    Data.Users.Add(new User { ID = usId, Status = int.Parse(user.adminIdsList.Item2) });
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
                            if (int.TryParse(msg, out int num) && num <= user.adminIdsList.Item1.Count)
                            {
                                if (user.adminIdsList.Item2 == "bl")
                                {
                                    Data.Users.Find(delegate (User usr) { return usr.ID == user.adminIdsList.Item1[num]; }).Blocked = (false, DateTime.Now);
                                }
                                else
                                    Data.Users.Find(delegate (User usr)
                                    {
                                        return usr.ID == user.adminIdsList.Item1[num];
                                    }).Status = 0;
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
                        //case "w":
                        //    user.Condition = button;
                        //    return;
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
                    //todo
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
                        if (int.TryParse(msg, out int selectedNote))
                        {
                            var notes = BotAsynh.GetNotes(user.ID, true);
                            BotAsynh.RemoveNote(user.ID, selectedNote, notes);
                        }
                        else
                            WebInterface.SendMessage(user.ID, "Ошибка ввода!");
                    }
                    else
                        user.Condition = "ad";
                }

            //функции клиента
            public static void Cl(User user, string button)
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

                public static void Cln(User user, string button)
                {
                    if (button != "b")
                    {
                        user.Condition = "clnd";
                        user.note[0] = int.Parse(button);
                    }
                    else
                        user.Condition = "cl";
                }

                    public static void Clnd(User user, string button)
                    {
                        if (button != "b")
                            user.Condition = "clndt";
                        else
                            user.Condition = "cln";
                    }

                        public static void Clndt(User user, string button)
                        {
                            if (button != "b")
                            {
                                user.Condition = "cl";
                                user.note[2] = int.Parse(button);
                                BotAsynh.MakeNote(user, user.note[0], user.note[1], user.note[2]);
                            }
                            else
                                user.Condition = "clnd";
                        }

                public static void Cldn(User user, string button)
                {
                    if (button != "b")
                    {
                        var selectedNote = int.Parse(button);
                        var notes = BotAsynh.GetNotes(user.ID);
                        BotAsynh.RemoveNote(user.ID, selectedNote, notes);
                    }
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
            };
        }

            public static void Opd(User user, string button)
            {
                switch (button)
                {
                    case "allcm":
                        //пришли все создание отметки и времени открытия
                        user.Condition = "op";
                        break;
                    case "ddn":
                        user.Condition = button;
                        user.OpenerIdsList = BotAsynh.GetOpenerIdsList();
                        break;
                    case "b":
                        user.Condition = "op";
                        user.OpenerIdsList.Clear();
                        break;
                };
            }
                //todo
                public static void Ddn(User user, string button)
                {
                    switch (button)
                    {
                        case "op":
                            user.Condition = button;
                            break;
                        case "ddn":
                            user.Condition = button;
                            user.OpenerIdsList = BotAsynh.GetOpenerIdsList();
                            break;
                    };
                }
            //todo
            public static void Opt(User user, string button)
            {
                switch (button)
                {
                    case "allcm":
                        //пришли все создание отметки и времени открытия
                        user.Condition = "op";
                        break;
                    case "ddn":
                        user.Condition = button;
                        user.OpenerIdsList = BotAsynh.GetOpenerIdsList();
                        break;
                    case "b":
                        user.Condition = "op";
                        user.OpenerIdsList.Clear();
                        break;
                };
            }
                //todo
                public static void Optd(User user, string button)
                {
                    switch (button)
                    {
                        case "op":
                            user.Condition = button;
                            break;
                        case "ddn":
                            user.Condition = button;
                            user.OpenerIdsList = BotAsynh.GetOpenerIdsList();
                            break;
                    };
                }
    }
}