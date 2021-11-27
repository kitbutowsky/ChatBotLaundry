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
        readonly string[] dayOfWeekRussian = new[] {  "Воскресенье","Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };
        public DateTime Date;
        public string DayOfWeekR { get { return dayOfWeekRussian[(int)Date.DayOfWeek]; } }
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
        public bool IsFree
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
        /// Таблица записи одного дня
        /// Принимает значения ID [время, количесво машинок] 
        /// </summary>
        public long[,] HoursWashesTable;
    }
}
