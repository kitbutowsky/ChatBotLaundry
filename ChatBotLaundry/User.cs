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
        public bool NotificationStatus = true;
        public int WashCounter = 0;
        /// <summary>
        /// статус блокировки, время блокировки и количество нарушений
        /// </summary>
        public (bool, DateTime, int) Blocked = (false, DateTime.UtcNow.AddHours(-1), 0);
        /// <summary>
        /// 1 - сск, 2 - открывающий, 3 - админ, по умолчанию 0 - клиент
        /// </summary>
        public int Status{ get { return status; } set { status = value; } }

        //служебные поля
        private string condition = "st";
        public string Condition { get { return condition; } set { condition = value; } }
        public List<TimeNote> notes;

        //поля для клиента
        public int[] note = new [] {0, 0, 0};

        //поля для открывающего
        public int[] opnote = new[] { 0, 0 };
        /// <summary>
        /// список записанных на определенное время пользователей
        /// </summary>
        public List<long> OpenerIdsList;

        /// <summary>
        /// количество открытий и среднее время
        /// </summary>
        public int OpenerTimes = 0;
        public TimeSpan AverageOpenerTime;

        //поля для администратора
        public int PasswordTries = 3;
        /// <summary>
        /// содержит список пользователей и статус пользователей для администрирования
        /// </summary>
        public (List<long>, string) adminIdsList;

        public override string ToString()
        {
            return ID.ToString() + ' ' + Status.ToString();
        }
    }
}