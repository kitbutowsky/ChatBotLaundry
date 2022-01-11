using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    //Класс записи в праченую
    public class TimeNote
    {
        private long userID;
        public long UserID { get { return userID; } }
        private DateTime date;
        public DateTime Day { get { return date; } }
        private int time;
        public int Time { get { return time; } }
        private int timeIndex;
        public int TimeIndex { get { return timeIndex; } }
        private int amount;
        public long Amount { get { return amount; } }
        /// <summary>
        /// Конструктор класса записи ID, день записи, времени записи и количество машинок 
        /// </summary>
        /// <param name=""></param>
        public TimeNote(long userID, DateTime day, int timeIndex, int time, int amount)
        {
            this.userID = userID;
            this.date = day; 
            this.timeIndex = timeIndex;
            this.time = time;
            this.amount = amount;
        }


        public override string ToString()
        {
            return  "ID: https://vk.com/im?sel=" + userID.ToString() +
                    " День: " + StaticDataAndMetods.DayOfWeekR(date) +
                    " Время: " + time.ToString() + ":00 " +
                    " Количество машинок: " + amount.ToString();
        }
    }
}
