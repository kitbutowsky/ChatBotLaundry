using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ChatBotLaundry
{
    class DataMethods
    {
        static List<int> metaIndexes = new List<int>();
        static SheetsService service;
        static GoogleCredential credential;

        /// <summary>
        /// Загружает данные в программу перед запуском бота
        /// </summary>
        public static void GetData()
        {
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
            var usersRequest = service.Spreadsheets.Values.Get(Data.SpreadsheetDBID, usersRange);
            var users = usersRequest.Execute().Values;
            if (users != null & users.Count > 0)
                foreach (var row in users)
                    Data.Users.Add(
                        new User(
                            long.Parse(row[0].ToString()),
                            TimeSpan.Parse(row[9].ToString()),
                            DateTime.Parse(row[4].ToString()),
                            int.Parse(row[1].ToString()),
                            bool.Parse(row[2].ToString()),
                            bool.Parse(row[3].ToString()),  
                            int.Parse(row[5].ToString()),
                            int.Parse(row[7].ToString()),
                            row[6].ToString(),
                            int.Parse(row[8].ToString()),
                            int.Parse(row[10].ToString())
                            ));

            //получение общей информации прачки
            var dataRange = $"DataSetings!b1:4";
            var dataRequest = service.Spreadsheets.Values.Get(Data.SpreadsheetDBID, dataRange);
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
            var daysMetaRequest = service.Spreadsheets.Values.Get(Data.SpreadsheetDBID, daysMetaRange);
            var daysMeta = daysMetaRequest.Execute().Values;
            if (daysMeta != null & daysMeta.Count > 0)
                foreach (var index in daysMeta[0])
                    metaIndexes.Add(int.Parse(index.ToString()));

            for (var i = 0; i < 7; i++)
            {
                var dayRange = $"Days!a{metaIndexes[i]}:{metaIndexes[i+1]-1}";
                var dayRequest = service.Spreadsheets.Values.Get(Data.SpreadsheetDBID, dayRange);
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
                    if (int.Parse(day[0][3].ToString()) != 0)
                    {
                        for (var j = 0; j < int.Parse(day[0][3].ToString()); j++)
                            Data.Days[i].Notes.Add(
                                new TimeNote(
                                    long.Parse(day[Data.Days[i].WashesHours.Count + 2][j].ToString()),
                                    DateTime.Parse(day[0][0].ToString()),
                                    int.Parse(day[Data.Days[i].WashesHours.Count + 4][j].ToString()),
                                    int.Parse(day[Data.Days[i].WashesHours.Count + 3][j].ToString()),
                                    int.Parse(day[Data.Days[i].WashesHours.Count + 5][j].ToString())
                                    )
                            );
                    }
                }    

            }
        }

        public class Update
        {
            //обновление полей таблицы 
            public static void NoteAmount(int day)
            {
                var cell = $"Days!D{metaIndexes[day]}";
                var valueRange = new ValueRange();
                valueRange.Values = new List<IList<object>> { new List<object> { Data.Days[day].Notes.Count } };
                var updateRequest = service.Spreadsheets.Values.Update(valueRange, Data.SpreadsheetDBID, cell);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                var updateResponse = updateRequest.Execute();
            }

            public static void MakeTableNote(int day, int time, int slot, long id)
            {
                var timeIndex = metaIndexes[day] + 1 + time;
                var slotIndex = StaticDataAndMetods.ToLetterColumn(slot+1);
                var cell = $"Days!{slotIndex}{timeIndex}";
                var valueRange = new ValueRange();
                valueRange.Values = new List<IList<object>> { new List<object> { id } };
                var updateRequest = service.Spreadsheets.Values.Update(valueRange, Data.SpreadsheetDBID, cell);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                var updateResponse = updateRequest.Execute();
            }

            public class TimeNote
            {
                public static void Add(int day, int noteIndex)
                {
                    NoteAmount(day);
                    var stNumIndex = metaIndexes[day] + 1 + Data.Days[day].WashesHours.Count + 1;
                    var slotIndex = StaticDataAndMetods.ToLetterColumn(noteIndex);
                    var note = Data.Days[day].Notes[noteIndex];
                    var range = $"Days!{slotIndex}{stNumIndex}:{slotIndex}{stNumIndex + 3}";
                    var valueRange = new ValueRange();
                    valueRange.Values = new List<IList<object>> {
                    new List<object> { note.UserID },
                    new List<object> { note.Time },
                    new List<object> { note.TimeIndex },
                    new List<object> { note.Amount },
                };
                    var updateRequest = service.Spreadsheets.Values.Update(valueRange, Data.SpreadsheetDBID, range);
                    updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                    var updateResponse = updateRequest.Execute();
                }

                public static void Delete(int day, int note)
                {
                    NoteAmount(day);
                    var deleteRequest = new Request();
                    deleteRequest.DeleteRange = new DeleteRangeRequest();
                    deleteRequest.DeleteRange.Range = new GridRange()
                    {
                        SheetId = 722513283,
                        StartColumnIndex = note,
                        StartRowIndex = metaIndexes[day] + 1 + Data.Days[day].WashesHours.Count,
                        EndColumnIndex = note + 1,
                        EndRowIndex = metaIndexes[day] + 1 + Data.Days[day].WashesHours.Count + 4
                    };
                    deleteRequest.DeleteRange.ShiftDimension = "COLUMNS";

                    BatchUpdateSpreadsheetRequest requestBody = new BatchUpdateSpreadsheetRequest();
                    requestBody.Requests = new List<Request> {
                        deleteRequest
                    }; ;

                    SpreadsheetsResource.BatchUpdateRequest request = service.Spreadsheets.BatchUpdate(requestBody, Data.SpreadsheetDBID);
                    BatchUpdateSpreadsheetResponse response = request.Execute();
                }
            }

            //Обновление полей User(a)
            public class User
            {
                public static void Status(ChatBotLaundry.User user, int status) => UserUpdates(user, new List<object> { status }, "B");public static void NotificationStatus(ChatBotLaundry.User user, bool status) => UserUpdates(user, new List<object> { status }, "C");
                public static void Blocked(ChatBotLaundry.User user, bool status) => UserUpdates(user, new List<object> { status }, "D");
                public static void BlockingTime(ChatBotLaundry.User user, DateTime datetime) => UserUpdates(user, new List<object> { datetime }, "E");
                public static void BlockingCount(ChatBotLaundry.User user, int count) => UserUpdates(user, new List<object> { count }, "F");
                public static void Condition(ChatBotLaundry.User user) => UserUpdates(user, new List<object> { user.Condition }, "G");
                public static void WashCounter(ChatBotLaundry.User user, int count) => UserUpdates(user, new List<object> { count }, "H");
                public static void OpenerTimes(ChatBotLaundry.User user, int count) => UserUpdates(user, new List<object> { count }, "I");
                public static void AverageOpenerTime(ChatBotLaundry.User user, TimeSpan time) => UserUpdates(user, new List<object> { time }, "J");
                public static void PasswordTries(ChatBotLaundry.User user, int count) => UserUpdates(user, new List<object> { count }, "K");
            }

            public static void UserUpdates(ChatBotLaundry.User user, List<object> info, string column = "A", bool fullUpdate = false)
            {
                var index = Data.Users.FindIndex(delegate (ChatBotLaundry.User us) { return us == user; }) + 2;
                var userRange = "";
                if (fullUpdate)
                    userRange = $"Users!{column}{index}:{index}";
                else 
                    userRange = $"Users!{column}{index}"; 
                var valueRange = new ValueRange();
                valueRange.Values = new List<IList<object>> { info };
                var updateRequest = service.Spreadsheets.Values.Update(valueRange, Data.SpreadsheetDBID, userRange);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                var updateResponse = updateRequest.Execute();
            }
        }


        public static void NewDay()
        {
            while (true)
            {
                var time = new TimeSpan(0, 1, 0, 0);
                if (Data.Days[6].Date.Date != DateTime.Now.Date)
                {
                    Data.DaysArhive.Add(Data.Days[0]);
                    Data.Days.RemoveAt(0);
                    var newDay = new Day
                    (
                        DateTime.UtcNow,
                        Data.WashesAmount,
                        Data.WashesHours
                    );
                    Data.Days.Add(newDay);
                    ////меняем метаиндексы
                    //var newMetaIndexes = new List<int>();
                    //var dmetaIndexes = metaIndexes[1] - metaIndexes[0];
                    //for (var mmi = 1; mmi < metaIndexes.Count; mmi++)
                    //    newMetaIndexes.Add(metaIndexes[mmi] - dmetaIndexes);
                    //newMetaIndexes.Add(newMetaIndexes[5] + Data.Days[6].WashesHours.Count + 6);
                    ////копируем информацию из 6 дней
                    //var dataRange = $"Days!A{}:4";
                    //var dataRequest = service.Spreadsheets.Values.Get(Data.SpreadsheetDBID, dataRange);
                    //var data = dataRequest.Execute().Values;
                    ////очищаем таблицу
                    ////вставляем информацию 6ти дней
                    ////создаем новый день для таблицы
                    ////оправляем этот день
                    foreach (var us in Data.Users)
                        if (us.Status == 3 || us.Status == 2)
                            WebInterface.SendMessage(us.ID, "Обновилась запись");
                }
                Thread.Sleep(time);
                
            }   
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
                        Update.MakeTableNote(selectedDay, selectedTime, j, note.UserID);
                        amount--;
                    }
                //добавляет запись в список записей
                day.Notes.Add(note);
                WebInterface.SendMessage(user.ID, "Вы записаны");
                Update.TimeNote.Add(selectedDay, day.Notes.Count-1);
            }
            else
                WebInterface.SendMessage(user.ID, "Кажется вы слишком долго выбирали, это место уже забронировали\n" +
                                                    "Не отчаивайтесь! Выберите другое время)");
        }

        internal static void RemoveNote(int selectedNote, User user)
        {
            //превращаем номер выбранной записи в индекс
            selectedNote--;
            var amount = user.notes[selectedNote].Amount;
            //ищет индекс дня в списке дней для удаления записи
            var selectedDay = Data.Days.FindIndex(delegate (Day day)
            {
                return day.Date == user.notes[selectedNote].Day.Date;
            });
            var selectedDBNote = Data.Days[selectedDay].Notes.FindIndex(delegate (TimeNote note)
            {
                return note == user.notes[selectedNote];
            });
            for (var i = 0; i < Data.WashesAmount; i++)
            {
                var selectedTime = user.notes[selectedNote].TimeIndex;
                if (Data.Days[selectedDay].HoursWashesTable[selectedTime, i] == user.notes[selectedNote].UserID
                    && amount != 0)
                {
                    Data.Days[selectedDay].HoursWashesTable[selectedTime, i] = 0;
                    Update.MakeTableNote(selectedDay, selectedTime, i, 0);
                    amount -= 1;
                }
            }
            Data.Days[selectedDay].Notes.Remove(user.notes[selectedNote]);
            Update.TimeNote.Delete(selectedDay, selectedDBNote);
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
