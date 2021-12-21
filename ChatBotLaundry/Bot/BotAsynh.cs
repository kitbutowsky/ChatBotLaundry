using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static ChatBotLaundry.Program;

namespace ChatBotLaundry
{
    class BotAsynh : Modules
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
                    NModule(user, session, button, msg);
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
                    session.SendButtons(user.ID, GetButtons.Back());
                    return;
                case "del":
                    session.SendMessage(user.ID, ListToNumerableStringList(user.adminIdsList.Item1));
                    session.SendMessage(user.ID, "Введите номер пользователя");
                    session.SendButtons(user.ID, GetButtons.Back());
                    return;
                case "l":
                    session.SendMessage(user.ID, "Выберите действие:");
                    session.SendInlineButtons(user.ID, GetButtons.L());
                    return;
                case "pas":
                    session.SendMessage(user.ID, "Введите пароль:");
                    session.SendButtons(user.ID, GetButtons.Back());
                    return;
                case "newpas":
                    session.SendMessage(user.ID, "Введите новый пароль");
                    return;
                case "infad":
                    session.SendMessage(user.ID, Data.Info);
                    session.SendMessage(user.ID, "Введите новую информацию о прачке");
                    session.SendButtons(user.ID, GetButtons.Back());
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
                    session.SendMessage(user.ID, "Введите номер записи для отмены:");
                    session.SendButtons(user.ID, GetButtons.Back());
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
        internal static void ClientButtonsSender(User user, WebInterface session)
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

        //методы клиента
        internal static void MakeNote(User user, int selectedDay, int selectedTime, int selectedAmount)
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

        internal static void RemoveNote(User user, int selectedNote)
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
        internal static List<long> GetUsersIdsList(string buttonClicked1)
        {
            var listIds = new List<long>();
            foreach (var userCategory in Data.Users)
                if (userCategory.Status == int.Parse(buttonClicked1))
                    listIds.Add(userCategory.ID);
            return listIds;
        }

        internal static string ListToNumerableStringList(List<long> list)
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