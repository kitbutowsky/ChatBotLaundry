﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static ChatBotLaundry.Program;

namespace ChatBotLaundry
{
    class BotAsynh 
    {
        public static void BotRun(User user, string button, string msg)
        {
            
            switch (user.Status)
            {
                case 0:
                case 1:
                    BotClient(user, button, msg);
                    break;
                case 2:
                    BotOpener(user, button, msg);
                    break;
                case 3:
                    BotAdmin(user, button, msg);
                    break;
            }
        }

        public static void BotAdmin(User user, string button, string msg)
        {
            //Выполнение действий по нажатию кнопки
            switch (user.Condition)
            {
                case "st":
                    user.Condition = "adm";
                    break;
                case "adm":
                    Modules.Adm(user, button);
                    break;
                case "ad":
                    Modules.Ad(user, button);
                    break;
                case "us":
                    Modules.Us(user, button);
                    break;
                case "usrs":
                    Modules.Usrs(user, button);
                    break;
                case "add":
                    Modules.UsrsAdd(user, button, msg);
                    break;
                case "del":
                    Modules.UsrsDel(user, button, msg);
                    break;
                case "l":
                    Modules.L(user, button);
                    break;
                case "pas":
                    Modules.Pas(user, button, msg);
                    break;
                case "newpas":
                    Modules.NewPas(user, msg);
                    break;
                case "infad":
                    Modules.InfAd(user, button, msg);
                    break;
                case "infadc":
                    Modules.InfAdC(user, button);
                    break;
                case "time":
                    Modules.Time(user, button);
                    break;
                case "w":
                    Modules.W(user, button);
                    break;
                case "n":
                    Modules.N(user, button, msg);
                    break;
            }
            ClientActions(user, button);
            OpenerActions(user, button);
            //отправление сообщений и кнопок по состоянию
            switch (user.Condition)
            {
                case "adm":
                    WebInterface.SendButtons(user.ID, "Выберите действие:", GetButtons.Adm());
                    return;
                case "ad":
                    WebInterface.SendButtons(user.ID, "Выберите действие:", GetButtons.Ad());
                    return;
                case "us":
                    WebInterface.SendButtons(user.ID, "Выберите класс пользователя:", GetButtons.Us());
                    return;
                case "usrs":
                    WebInterface.SendButtons(user.ID, ListToNumerableStringIdsList(user.adminIdsList.Item1, " пользователей"), GetButtons.Usrs(user.adminIdsList.Item2));
                    return;
                case "add":
                    WebInterface.SendMessage(user.ID, ListToNumerableStringIdsList(user.adminIdsList.Item1, " пользователей"));
                    WebInterface.SendButtons(user.ID, "Введите id пользователя", GetButtons.Back());
                    return;
                case "del":
                    WebInterface.SendMessage(user.ID, ListToNumerableStringIdsList(user.adminIdsList.Item1, " пользователей"));
                    WebInterface.SendButtons(user.ID, "Введите номер пользователя", GetButtons.Back());
                    return;
                case "l":
                    WebInterface.SendButtons(user.ID, "Выберите действие:", GetButtons.L());
                    return;
                case "pas":
                    WebInterface.SendButtons(user.ID, "Введите пароль:", GetButtons.Back());
                    return;
                case "newpas":
                    WebInterface.SendMessage(user.ID, "Введите новый пароль");
                    return;
                case "infad":
                    WebInterface.SendMessage(user.ID, Data.Info);
                    WebInterface.SendButtons(user.ID, "Введите новую информацию о прачке", GetButtons.Back());
                    return;
                case "infadc":
                    WebInterface.SendButtons(user.ID, "Подтверждение", GetButtons.Infadc());
                    return;
                case "time":
                    WebInterface.SendButtons(user.ID, Data.WashesHoursToString(), GetButtons.Time());
                    return;
                case "w":
                    WebInterface.SendButtons(user.ID, "Выберите количество машинок:", GetButtons.W());
                    return;
                case "n":
                    WebInterface.SendMessage(user.ID, ListToNumerableNotesStringList(user.notes, " всех записей"));
                    WebInterface.SendButtons(user.ID, "Введите номер записи для отмены:", GetButtons.Back());
                    return;
            }
            //для клиента
            OpenerButtonsSender(user);
            ClientButtonsSender(user);
        }

        public static void BotOpener(User user, string button, string msg)
        {
            //todo
            if (user.Status == 2 && user.Condition == "cl" && msg == "reset")
            {
                user.Condition = "op";
                WebInterface.SendMessage(user.ID, "Теперь вам доступны функции отрывающего!");
            }
            //Выполнение действий по нажатию кнопки
            switch (user.Condition)
            {
                case "st":
                    user.Condition = "op";
                    break;
            }
            OpenerActions(user, button);
            ClientActions(user, button);
            //отправление сообщений и кнопок по состоянию
            OpenerButtonsSender(user);
            ClientButtonsSender(user);
        }

        public static void BotClient(User user, string button, string msg)
        {
            if (user.Status == 0 && user.Condition == "cl" && msg == Data.Password)
            {
                user.Status = 1;
                WebInterface.SendMessage(user.ID, "Теперь вы сск!");
            }
            switch (user.Condition)
            {
                case "st":
                    user.Condition = "cl";
                    break;
            }
            ClientActions(user, button);
            ClientButtonsSender(user);
        }


        //Модули действий(реакции на нажатые кнопки)
        private static void OpenerActions(User user, string button)
        {
            switch (user.Condition)
            {
                case "op":
                    Modules.Op(user, button);
                    break;
                case "opd":
                    Modules.Opd(user, button);
                    break;
                case "ddn":
                    Modules.Ddn(user, button);
                    break;
                case "opt":
                    Modules.Opt(user, button);
                    break;
                case "optd":
                    Modules.Optd(user, button);
                    break;
            }
        }

        private static void ClientActions(User user, string button)
        {
            switch (user.Condition)
            {
                case "cl":
                    Modules.Cl(user, button);
                    break;
                case "cln":
                    Modules.Cln(user, button);
                    break;
                case "clnd":
                    Modules.Clnd(user, button);
                    break;
                case "clndt":
                    Modules.Clndt(user, button);
                    break;
                case "cldn":
                    Modules.Cldn(user, button);
                    break;
            }
        }

        //Модули отправки кнопок(отправка кнопок в зависимости от состояния)
        private static void OpenerButtonsSender(User user)
        {
            switch (user.Condition)
            {
                case "op":
                    WebInterface.SendButtons(user.ID, "Функции открывающего:", GetButtons.Op(user));
                    return;
                case "opd":
                    WebInterface.SendButtons(user.ID, "Все ли пришли?", GetButtons.Opd(user));
                    return;
                case "ddn":
                    WebInterface.SendButtons(user.ID, ListToNumerableStringIdsList(user.OpenerIdsList, " пришедших") + "\n Выберите непришедших", GetButtons.Ddn(user));
                    return;
                case "opt":
                    WebInterface.SendButtons(user.ID, "Выберите удобный день открытия", GetButtons.Opt());
                    return;
                case "optd":
                    WebInterface.SendButtons(user.ID, "Выберите удобное время открытия", GetButtons.Optd(user.opnote[0]));
                    return;
            }
        }

        internal static void ClientButtonsSender(User user)
        {
            switch (user.Condition)
            {
                case "cl":
                    WebInterface.SendButtons(user.ID, "Выберите действие:", GetButtons.Cl(user));
                    return;
                case "cln":
                    WebInterface.SendButtons(user.ID, "Выберите день недели:", GetButtons.Cln(user.Status));
                    return;
                case "clnd":
                    WebInterface.SendButtons(user.ID, "Выберите время:", GetButtons.Clnd(user.note[0]));
                    return;
                case "clndt":
                    WebInterface.SendButtons(user.ID, "Выберите количество машинок:", GetButtons.Clndt(user.note[0], user.note[1]));
                    return;
                case "cldn":
                    WebInterface.SendButtons(user.ID, ListToNumerableNotesStringList(user.notes, " записей") + "\nВыберите запись для отмены", GetButtons.Cldn(user));
                    return;
            }
        }

        //методы клиента
        internal static void RemoveNote( int selectedNote, List<TimeNote> notes)
        {
            var amount = notes[selectedNote].Amount;
            //ищет индекс дня в списке дней для удаления записи
            var index = Data.Days.FindIndex(delegate (Day day)
            {
                return day.Date == notes[selectedNote].Day.Date;
            });
            for (var i = 0; i < Data.WashesAmount; i++)
            {
                if (Data.Days[index].HoursWashesTable[notes[selectedNote].Time, i] == notes[selectedNote].UserID
                    && amount != 0)
                {
                    Data.Days[index].HoursWashesTable[notes[selectedNote].Time, i] = 0;
                    amount -= 1;
                }
            }
            Data.Days[index].Notes.Remove(notes[selectedNote]);
        }

        //методы админа
        internal static List<long> GetUsersIdsList(Predicate<User> condition)
        {
            var listUsers = Data.Users.FindAll(condition);
            List<long> listIds = new List<long>();
            foreach (var usr in listUsers)
                listIds.Add(usr.ID);
            return listIds;
        }

        internal static List<long> GetOpenerIdsList()
        {
            var day = Data.Days.Find(delegate (Day day)
            {
                return day.Date.Date == DateTime.Now.Date;
            });
            var timeIndex = day.WashesHours.FindIndex(delegate (int time)
            {
                var now = DateTime.UtcNow.Hour;
                for (var i = 0; i < 3; i++)
                    if (time == now - i)
                        return true;
                return false;
            });
            List<long> listIds = new List<long>();
            for (var a = 0; a < Data.WashesAmount; a++)
                if (!listIds.Contains(day.HoursWashesTable[timeIndex, a]))
                    listIds.Add(day.HoursWashesTable[timeIndex, a]);
            return listIds;
        }

        internal static string ListToNumerableStringIdsList(List<long> list, string ob)
        {
            string stringListIds = "Список" + ob + "\n";
            var i = 0;
            foreach (var item in list)
            {
                stringListIds += i.ToString() + ". https://vk.com/im?sel=" + item.ToString() + "\n";
                i++;
            }
            return stringListIds;
        }

        internal static string ListToNumerableNotesStringList(List<TimeNote> list, string ob)
        {
            string stringListIds = "Список" + ob + "\n";
            var i = 0;
            foreach (var item in list)
            {
                stringListIds += i.ToString() + ". " + item.ToString() + "\n";
                i++;
            }
            if (list.Count == 0)
                stringListIds = "Нет записей";
            return stringListIds;
        }
        /// <summary>
        /// возвращает список записей для пользователя id или все записи 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        internal static List<TimeNote> GetNotes(long id, bool all = false)
        {
            var notes = new List<TimeNote>();
            var dayIndex = 0;
            foreach (var day in Data.Days)
            {
                foreach (var note in day.Notes)
                {
                    note.dayForNotation = dayIndex;
                    if (note.UserID == id || all)
                    {
                        notes.Add(note);
                    }
                }
                dayIndex++;
            }
            return notes;
        }

    }
}