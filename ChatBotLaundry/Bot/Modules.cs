using System;
using System.Threading;

namespace ChatBotLaundry
{
    internal class Modules
    {
        //модули диалогов(модули возвращают предыдущее/следующее состояния, выполняют действия)
        internal static void AdmModule(User user, WebInterface session, string button)
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

        internal static void AdModule(User user, WebInterface session, string button)
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

        internal static void CldnModule(User user, WebInterface session, string button)
        {
            if (button != "b")
            {
                var selectedNote = int.Parse(button);
                BotAsynh.RemoveNote(user, selectedNote);
            }
            user.Condition = "cl";
        }

        public static void ClModule(User user, WebInterface session, string button)
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

        public static void ClndModule(User user, WebInterface session, string button)
        {
            if (button != "b")
                user.Condition = "clndt";
            else
                user.Condition = "cln";
        }

        public static void ClndtModule(User user, WebInterface session, string button)
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

        public static void ClnModule(User user, WebInterface session, string button)
        {
            if (button != "b")
            {
                user.Condition = "clnd";
                user.note[0] = int.Parse(button);
            }
            else
                user.Condition = "cl";
        }

        public static void InfAdCModule(User user, WebInterface session, string button)
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

        public static void InfAdModule(User user, WebInterface session, string button, string msg)
        {
            if (button == "b")
                user.Condition = "l";
            else
            {
                user.Condition = "infadc";//Info administration confirmation
                Data.NewInfo = msg;
            }
        }


        public static void LModule(User user, WebInterface session, string button)
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

        public static void NewPasModule(User user, WebInterface session, string msg)
        {
            user.Condition = "l";
            user.PasswordTries = 3;
            Data.Password = msg;
            session.SendMessage(user.ID, "Пароль изменен!");
            session.SendMessage(user.ID, "Новый пароль: " + Data.Password.ToString());
            Thread.Sleep(3000);
        }

        public static void NModule(User user, WebInterface session, string button, string msg)
        {
            int num;
            if (button != "b")
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
        //todo
        public static void PasModule(User user, WebInterface session, string button, string msg)
        {
            if (button == "b")
                user.Condition = "l";
            else if (msg == Data.Password)
                user.Condition = "newpas";
            else
            {
                user.PasswordTries--;
                session.SendMessage(user.ID, "Вы ввели не правильный пароль\nОсталось " + user.PasswordTries.ToString() + " попыток");
            }
        }

        public static void TimeModule(User user, WebInterface session, string button)
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

        public static void UsModule(User user, WebInterface session, string button)
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

        public static void UsrsModule(User user, WebInterface session, string button)
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

        public static void UsrsAddModule(User user, WebInterface session, string button, string msg)
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
                session.SendMessage(user.ID, "Некоректный ввод! повторите попытку");
        }

        public static void UsrsDelModule(User user, WebInterface session, string button, string msg)
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
                session.SendMessage(user.ID, "Некоректный ввод! повторите попытку");
        }
    }
}