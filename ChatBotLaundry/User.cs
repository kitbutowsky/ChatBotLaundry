using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    //класс пользователя
    public class User
    {   
        //общие поля
        public long ID;
        private int status;
        private bool notificationStatus;
        private (bool, DateTime, int) blocked;
        private int washCounter = 0;
        private string condition = "st";
        public List<TimeNote> notes;

        /// <summary>
        /// 1 - сск, 2 - открывающий, 3 - админ, по умолчанию 0 - клиент
        /// </summary>
        public int Status{ 
            get { return status; } 
            set { 
                status = value;
                DataMethods.Update.User.Status(this, value);
            } }

        public bool NotificationStatus
        {
            get { return notificationStatus; }
            set
            {
                notificationStatus = value;
                DataMethods.Update.User.NotificationStatus(this, value);
            }
        }
        /// <summary>
        /// статус блокировки, время блокировки и количество нарушений
        /// </summary>
        public (bool, DateTime, int) Blocked{
            get { return blocked; }
            set
            {
                blocked = value;
                DataMethods.Update.User.Blocked(this, value.Item1);
                DataMethods.Update.User.BlockingTime(this, value.Item2);
                DataMethods.Update.User.BlockingCount(this, value.Item3);
            }
        }
        public string Condition { 
            get { return condition; } 
            set { condition = value; } }
        public int WashCounter
        {
            get { return washCounter; }
            set
            {
                washCounter = value;
                DataMethods.Update.User.WashCounter(this, value);
            }
        }

        //поля для клиента
        public int[] note = new [] {7, 7, 7};

        //поля для открывающего
        public int[] opnote = new[] { 7, 7 };

        /// <summary>
        /// количество открытий и среднее время
        /// </summary>
        private int openerTimes = 0;
        private TimeSpan averageOpenerTime;

        public int OpenerTimes
        {
            get { return openerTimes; }
            set
            {
                openerTimes = value;
                DataMethods.Update.User.OpenerTimes(this, value);
            }
        }

        public TimeSpan AverageOpenerTime
        {
            get { return averageOpenerTime; }
            set
            {
                averageOpenerTime = value;
                DataMethods.Update.User.AverageOpenerTime(this, value);
            }
        }
        //поля для администратора
        private int passwordTries = 3;
        public int PasswordTries
        {
            get { return passwordTries; }
            set
            {
                passwordTries = value;
                DataMethods.Update.User.PasswordTries(this, value);
            }
        }
        /// <summary>
        /// содержит список пользователей и статус пользователей для администрирования
        /// </summary>
        public (List<long>, string) adminIdsList;

        public override string ToString()
        {
            return ID.ToString() + ' ' + Status.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="status"></param>
        /// <param name="averageOpenerTime">если openerTimes = 0 -> TimeSpan.Min</param>
        /// <param name="blockingTime">если пользователь не заблокирован -> DateTime.Min</param>
        /// <param name="notificationStatus"></param>
        /// <param name="blocked"></param>
        /// <param name="blockingCount"></param>
        /// <param name="washCounter"></param>
        /// <param name="condition"></param>
        /// <param name="openerTimes"></param>
        public User(
            long ID,
            TimeSpan averageOpenerTime,
            DateTime blockingTime,
            int status = 0, 
            bool notificationStatus = true, 
            bool blocked = false,
            int blockingCount = 0,
            int washCounter = 0,
            string condition = "st",
            int openerTimes = 0,
            int passwordTries = 0)
        {
            this.ID = ID;
            this.status = status;
            this.notificationStatus = notificationStatus;
            this.blocked = (blocked, blockingTime, blockingCount);
            this.washCounter = washCounter;
            this.openerTimes = openerTimes;
            this.averageOpenerTime = averageOpenerTime;
            this.condition = condition;
            this.passwordTries = passwordTries;
        }
    }
}