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
        private string condition = "st";
        public int[] note = new [] {0, 0, 0};
        public int PasswordTries = 3;

        /// <summary>
        /// содержит список пользователей и статус пользователей для администрирования
        /// </summary>
        public (List<long>, int) adminIdsList;

        /// <summary>
        /// 1 - сск, 2 - открывающий, 3 - админ, 4 - заблокированный, по умолчанию 0 - клиент
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