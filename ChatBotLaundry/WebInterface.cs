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

        public (long, string) GetMessage()
        {
            var strs = Console.ReadLine().Split(separator: ' ');
            var id = long.Parse(strs[0]);
            var message = strs[1];
            for (var i = 2; i < strs.Length; i++)
                message += ' ' + strs[i];
            return (id, message);
        }
        
        public void SendButtons(long id, List<List<(string, string)>> buttons)
        {
            var toId = "Пользователю: " + id.ToString();
            var str = "";
            foreach (var buttns in buttons)
            {
                foreach (var buttn in buttns)
                {
                    str += buttn.Item2 + ". " + buttn.Item1 + " ";
                }
                str += "\n";
            }
            if (!str.Contains("b. Выйти") && !str.Contains("b. Отмена"))
                str += "b.  Назад";
            Console.WriteLine("{0}\n{1}", toId, str);
        }

        public void SendInlineButtons(long id, List<List<(string, string)>> buttons)
        {
            var toId = "Пользователю: " + id.ToString();
            var str = "";
            foreach (var buttns in buttons)
            {
                foreach (var buttn in buttns)
                {
                    str += buttn.Item2 + ". " + buttn.Item1;
                }
                str += "\n";
            }
            if (!str.Contains("b. Выйти") && !str.Contains("b. Отмена"))
                str += "b.  Назад";
            Console.WriteLine("{0}\n{1}", toId, str);
        }

        public void SendMessage(long id, string message)
        {
            var toId = "Пользователю: " + id.ToString();
            Console.WriteLine(toId + '\n' + message);
        }
    }
}