using System;
using System.Collections.Generic;
using System.Text;

namespace Requester
{
    interface IWebInterface
    {
        long GetUserId();
        void SendButtons(List<string> buttons);
        void SendMessage(string message);
        int GetButton();
    }
}
