using System;
using System.Collections.Generic;
using System.Text;

namespace Requester
{
    class WebInterface : IWebInterface
    {
        public int GetButton()
        {
            return int.Parse(Console.ReadLine());
        }

        public long GetUserId()
        {
            return long.Parse(Console.ReadLine());
        }

        public void SendButtons(List<string> buttons)
        {
            var buttns = new List<string>();
            var num = 1;
            foreach (var button in buttons)
            {
                buttns.Add(num.ToString() + ". " + button);
                num++;
            }
            Console.WriteLine(string.Join(" ", buttns));
        }

        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}