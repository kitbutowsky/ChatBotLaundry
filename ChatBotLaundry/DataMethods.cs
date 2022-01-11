using System;
using System.Collections.Generic;
using System.Threading;

namespace ChatBotLaundry
{
    class DataMethods
    {
        public static void DayUpdate()
        {
            while (true)
            {
                var time = new TimeSpan(1, 0, 0, 0);
                Thread.Sleep(time);
                Data.DaysArhive.Add(Data.Days[0]);
                Data.Days.RemoveAt(0);
                var newDay = new Day
                {
                    Date = DateTime.UtcNow,
                    HoursWashesTable = new long[Data.WashesHours.Count, Data.WashesAmount],
                    HoursWashesOpenerTable = new long[Data.WashesOpenerHours.Count],
                    WashesHours = Data.WashesHours
                };
                Data.Days.Add(newDay);
                foreach (var us in Data.Users)
                    if (us.Status == 3 || us.Status == 2)
                        WebInterface.SendMessage(us.ID, "Обновилась запись");
            }
        }

        internal static void RemoveNote(int selectedNote, User user)
        {
            var amount = user.notes[selectedNote].Amount;
            //ищет индекс дня в списке дней для удаления записи
            var index = Data.Days.FindIndex(delegate (Day day)
            {
                return day.Date == user.notes[selectedNote].Day.Date;
            });
            for (var i = 0; i < Data.WashesAmount; i++)
            {
                if (Data.Days[index].HoursWashesTable[user.notes[selectedNote].TimeIndex, i] == user.notes[selectedNote].UserID
                    && amount != 0)
                {
                    Data.Days[index].HoursWashesTable[user.notes[selectedNote].TimeIndex, i] = 0;
                    amount -= 1;
                }
            }
            Data.Days[index].Notes.Remove(user.notes[selectedNote]);
            user.notes.RemoveAt(selectedNote);
        }

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
                    if (note.UserID == id || all)
                        notes.Add(note);
                dayIndex++;
            }
            return notes;
        }
    }
}
