using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ChatBotLaundry
{
    class DataMethods
    {
        static SheetsService service;

        /// <summary>
        /// Загружает данные в программу перед запуском бота
        /// </summary>
        public static void GetData()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(StaticDataAndMetods.Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = StaticDataAndMetods.ApplicationName
            });

            //Получение пользователей
            var usersRange = $"Users!A2:K";
            var usersRequest = service.Spreadsheets.Values.Get(Data.SpreadsheetID, usersRange);
            var users = usersRequest.Execute().Values;
            if (users != null & users.Count > 0)
                foreach (var row in users)
                    Data.Users.Add(
                        new User
                        {
                            ID = long.Parse(row[0].ToString()),
                            Status = int.Parse(row[1].ToString()),
                            NotificationStatus = bool.Parse(row[2].ToString()),
                            Blocked = (bool.Parse(row[3].ToString()), DateTime.Parse(row[4].ToString()), int.Parse(row[5].ToString())),
                            Condition = row[6].ToString(),
                            WashCounter = int.Parse(row[7].ToString()),
                            OpenerTimes = int.Parse(row[8].ToString()),
                            AverageOpenerTime = TimeSpan.Parse(row[9].ToString()),
                            PasswordTries = int.Parse(row[10].ToString())
                        });

            //получение общей информации прачки
            var dataRange = $"DataSetings!b1:4";
            var dataRequest = service.Spreadsheets.Values.Get(Data.SpreadsheetID, dataRange);
            var data = dataRequest.Execute().Values;
            if (data != null & data.Count > 0)
            {
                Data.Info = data[0][0].ToString();
                Data.Password = data[1][0].ToString();
                Data.WashesAmount = int.Parse(data[2][0].ToString());
                foreach (var time in data[3])
                    Data.WashesHours.Add(int.Parse(time.ToString()));
            }

            //получение дней записи
            //получение индексов таблиц дней
            var daysMetaRange = $"Days!a1:1";
            var daysMetaRequest = service.Spreadsheets.Values.Get(Data.SpreadsheetID, daysMetaRange);
            var daysMeta = daysMetaRequest.Execute().Values;
            var metaIndexes = new List<int>();
            if (daysMeta != null & daysMeta.Count > 0)
                foreach (var index in daysMeta[0])
                    metaIndexes.Add(int.Parse(index.ToString()));

            for (var i = 0; i < 7; i++)
            {
                var dayRange = $"Days!a{metaIndexes[i]}:{metaIndexes[i+1]-1}";
                var dayRequest = service.Spreadsheets.Values.Get(Data.SpreadsheetID, dayRange);
                var day = dayRequest.Execute().Values;
                if (day != null & day.Count > 0)
                {
                    var washesHours = new List<int>();
                    //заполняем таблицу времени записи в зависимости от количества слотов времени записи 
                    for (var it = 1; it <= int.Parse(day[0][2].ToString()); it++)
                        washesHours.Add(int.Parse(day[it][0].ToString()));
                    //создаем обьект дня записи
                    Data.Days.Add(
                        new Day(
                                DateTime.Parse(day[0][0].ToString()),
                                int.Parse(day[0][1].ToString()),
                                washesHours
                                )
                    );
                    //добавляем таблицу записей
                    for (var ty = 1; ty <= int.Parse(day[0][2].ToString()); ty++)
                        for (var tx = 1; tx < 1 + Data.Days[i].WashesAmount; tx++)
                            Data.Days[i].HoursWashesTable[ty-1, tx-1] = long.Parse(day[ty][tx].ToString());
                    //добавляем таблицу открывающих
                    for (var j = 0; j < day[Data.WashesHours.Count + 1].Count; j++  )
                        Data.Days[i].HoursWashesOpenerTable[j] = long.Parse(day[Data.WashesHours.Count + 1][j].ToString());
                    //добавляем список записей
                    if (int.Parse(day[0][4].ToString()) != 0)
                    {
                        for (var j = 0; j < int.Parse(day[0][4].ToString()); j++)
                            Data.Days[i].Notes.Add(
                                new TimeNote(
                                    long.Parse(day[Data.WashesHours.Count + 2][j].ToString()),
                                    DateTime.Parse(day[0][0].ToString()),
                                    int.Parse(day[Data.WashesHours.Count + 4][j].ToString()),
                                    int.Parse(day[Data.WashesHours.Count + 3][j].ToString()),
                                    int.Parse(day[Data.WashesHours.Count + 5][j].ToString())
                                    )
                            );
                    }
                }    

            }
        }

        /// <summary>
        /// Обновляет данные в гугл таблицах раз в 5 минут
        /// </summary>
        public static void UpdateData()
        {

        }

        public static void DayUpdate()
        {
            while (true)
            {
                var time = new TimeSpan(1, 0, 0, 0);
                Thread.Sleep(time);
                Data.DaysArhive.Add(Data.Days[0]);
                Data.Days.RemoveAt(0);
                var newDay = new Day
                (
                    DateTime.UtcNow,
                    Data.WashesAmount,
                    Data.WashesHours
                );
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
