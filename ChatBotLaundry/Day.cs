using System;
using System.Collections.Generic;
using System.Text;

namespace Requester
{
    /// <summary>
    /// класс дня записи содержит дату, день недели, статус доступности, статус пользователя, таблицу ячеек записи
    /// </summary>
    public class Day
    {
        string[] dayOfWeekRussian = new[] { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
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
        /// Принимает значения ID [количесво машинок, время] 
        /// </summary>
        public int[,] HoursWashesTable;

        public override string ToString()
        {
            return Date.ToString() + ' ';
        }
    }
}
