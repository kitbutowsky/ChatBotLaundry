using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    class WebInterface : IWebInterface
    {
        public (long, string) GetButton()
        {
            var strs = Console.ReadLine().Split();
            var id = long.Parse(strs[0]);
            var button = strs[1];
            return (id, button);
        }

        public string GetMessage()
        {
            return Console.ReadLine();
        }

        public long GetUserId()
        {
            return long.Parse(Console.ReadLine());
        }
        
        public void SendButtons(List<string[]> buttons)
        {
            var buttns = new List<string>();
            foreach (var button in buttons)
            {
                buttns.Add(button[1] + ". " + button[0]);
            }
            if (!buttns.Contains("b. Выйти"))
                buttns.Add( "b.  Назад");
            Console.WriteLine(string.Join(" ", buttns));
        }

        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}