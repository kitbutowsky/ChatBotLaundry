using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    /// <summary>
    /// класс дня записи содержит дату, день недели, статус доступности, статус пользователя, таблицу ячеек записи
    /// </summary>
    public class Day
    {
        /// <summary>
        /// Количество машинок
        /// </summary>
        public int WashesAmount;
        //время записи для этого дня
        public List<int> WashesHours;
        public List<int> WashesOpenerHours
        {
            get
            {
                var washesOpenerHours = new List<int>();
                for (var i = 0; i < WashesHours.Count - 1; i++)
                {
                    washesOpenerHours.Add(WashesHours[i]);
                    if (WashesHours[i + 1] - WashesHours[i] > 2)
                        washesOpenerHours.Add(WashesHours[i] + 2);
                }
                washesOpenerHours.Add(WashesHours[^1]);
                washesOpenerHours.Add(WashesHours[^1] + 2);
                return washesOpenerHours;
            }
        }

        public DateTime Date;
        public bool AvailableForSSK { get { return Date.DayOfWeek == DayOfWeek.Sunday || Date.DayOfWeek == DayOfWeek.Wednesday; } }
        /// <summary>
        /// возвращает количество свободных ячеек
        /// </summary>
        public int EmptySpaces
        {
            get
            {
                var count = 0;
                foreach (var a in HoursWashesTable)
                    if (a == 0)
                        count++;
                return count;
            }
        }
        /// <summary>
        /// возвращает массив количества свободных ячеек для каждого времени
        /// </summary>
        public int[] EmptyTimes
        {
            get
            {
                var counter = new int[HoursWashesTable.GetLength(0)];
                for (var i = 0; i < HoursWashesTable.GetLength(0); i++)
                    for (var j = 0; j < HoursWashesTable.GetLength(1); j++)
                        if (HoursWashesTable[i,j] == 0)
                            counter[i]++;
                return counter;
            }
        }
        /// <summary>
        /// если есть хотябы одна свободная ячейка возвращает true
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                foreach (var a in HoursWashesTable)
                    if (a == 0)
                        return true;
                return false;
            }
        }
        /// <summary>
        /// если есть хотябы одна свободная ячейка возвращает true
        /// </summary>
        public bool IsEmptyOpener
        {
            get
            {
                foreach (var a in HoursWashesOpenerTable)
                    if (a == 0)
                        return true;
                return false;
            }
        }

        /// <summary>
        /// список записей
        /// </summary>
        public List<TimeNote> Notes = new List<TimeNote>();
        /// <summary>
        /// Таблица записи одного дня
        /// ID[время, количесво машинок] 
        /// </summary>
        public long[,] HoursWashesTable;
        /// <summary>
        /// Таблица распределений открытий прачки одного дня
        /// ID[время] 
        /// </summary>
        public long[] HoursWashesOpenerTable;

        public Day(DateTime Date, int WashesAmount, List<int> WashesHours)
        {
            this.Date = Date;
            this.WashesAmount = WashesAmount;
            this.WashesHours = WashesHours;
            this.HoursWashesTable = new long[this.WashesHours.Count, WashesAmount];
            this.HoursWashesOpenerTable = new long[this.WashesOpenerHours.Count];
        }
    }
}
