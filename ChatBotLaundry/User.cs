﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    //класс пользователя
    public class User
    {
        public long ID;
        /// <summary>
        /// 1 - сск, 2 - админ, 3 - сск, 4 - заблокированный, по умолчанию 0 - клиент
        /// </summary>
        public int Status
        {
            get
            {
                if (!Data.userStatus.ContainsKey(ID))
                    return 0;
                return Data.userStatus[ID];
            }
        }

        public bool NotificationStatus = true;

        public override string ToString()
        {
            return ID.ToString() + ' ' + Status.ToString();
        }
    }
}