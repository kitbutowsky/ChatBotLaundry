using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    //класс пользователя
    public class User
    {
        public long ID;
        private int status;
        /// <summary>
        /// статус блокировки, время блокировки и количество нарушений
        /// </summary>
        public (bool, DateTime, int) Blocked = (false, DateTime.UtcNow, 0);
        //служебные поля
        private string condition = "st";
        public int[] note = new [] {0, 0, 0};
        public int[] opnote = new[] { 0, 0 };
        public int PasswordTries = 3;
        /// <summary>
        /// содержит список пользователей и статус пользователей для администрирования
        /// </summary>
        public (List<long>, string) adminIdsList;
        /// <summary>
        /// содержит список пользователей и статус пользователей для открывающего
        /// </summary>
        public List<long> OpenerIdsList;

        /// <summary>
        /// 1 - сск, 2 - открывающий, 3 - админ, по умолчанию 0 - клиент
        /// </summary>
        public int Status{ get { return status; } set { status = value; } }

        public string Condition{ get { return condition; } set { condition = value; } }

        public bool NotificationStatus = true;

        public override string ToString()
        {
            return ID.ToString() + ' ' + Status.ToString();
        }
    }

}