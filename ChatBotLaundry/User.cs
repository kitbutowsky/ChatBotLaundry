using System;
using System.Collections.Generic;
using System.Text;

namespace Requester
{
    //класс пользователя
    public class User
    {
        public long ID;
        public int Status
        {
            get
            {
                if (!Program.userStatus.ContainsKey(ID))
                    return 0;
                return Program.userStatus[ID];
            }
        }

        public bool NotificationStatus = true;

        public override string ToString()
        {
            return ID.ToString() + ' ' + Status.ToString();
        }
    }
}