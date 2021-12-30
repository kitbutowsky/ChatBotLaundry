using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    //Класс записи в праченую
    public class TimeNote
    {
        public int dayForNotation;
        private DateTime date;
        private long userID;
        private int time;
        private int amount;

        public int DayForNotation { get { return dayForNotation; } }
        public DateTime Day { get { return date; } }
        public long UserID { get { return userID; } }
        public int Time { get { return time; } }
        public int Amount { get { return amount; } }
        /// <summary>
        /// Конструктор класса записи ID, индекс выбранного дня записи в списке дней, выбранный индекс времени записи и количество машинок 
        /// </summary>
        /// <param name=""></param>
        public TimeNote(long userID, int dayForNotation, int time, int amount)
        {
            this.userID = userID;
            this.dayForNotation = dayForNotation;
            this.time = time;
            this.amount = amount;
            this.date = Data.Days[dayForNotation].Date;
        }


        public override string ToString()
        {
            return  "ID: " + userID.ToString() +
                    " День: " + StaticDataAndMetods.DayOfWeekR(date) +
                    " Время: " + Data.WashesHoursInTimezone[time].ToString() + ":00 " +
                    " Количество машинок: " + amount.ToString();
        }
    }
}
