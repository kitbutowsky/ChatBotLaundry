using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    class WebInterface : IWebInterface
    {
        public (long, string, string) GetContent()
        {
            var strs = new string[2];
            strs = Console.ReadLine().Split();
            var id = long.Parse(strs[0]);
            
            var button = strs[1];
            var msg = Console.ReadLine();
            return (id, button, msg);
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