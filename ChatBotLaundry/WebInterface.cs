using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    static class WebInterface 
    {
        /// <summary>
        /// принимает контент в виде сообщения или нажатой кнопки
        /// </summary>
        /// <returns>id, payload, msg</returns>
        static public (long, string, string) GetContent()
        {
            var strs = new string[2];
            strs = Console.ReadLine().Split();
            var id = long.Parse(strs[0]);
            var button = strs[1];
            var msg = Console.ReadLine();
            return (id, button, msg);
        }


        static public void SendButtons(long id, string message, List<List<(string, string)>> buttons)
        {
            var toId = "Пользователю: " + id.ToString();
            Console.WriteLine(toId + '\n' + message);
            var str = "";
            foreach (var buttns in buttons)
            {
                foreach (var buttn in buttns)
                {
                    str += buttn.Item2 + ". " + buttn.Item1 + " ";
                }
                str += "\n";
            }
            Console.WriteLine("{0}\n{1}", toId, str);
        }


        static public void SendMessage(long id, string message)
        {
            var toId = "Пользователю: " + id.ToString();
            Console.WriteLine(toId + '\n' + message);
        }
    }
}