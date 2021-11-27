using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    //класс пользователя
    public class User
    {
        public long ID;
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