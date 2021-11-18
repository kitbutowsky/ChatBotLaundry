using System;
using System.Collections.Generic;
using System.Text;

namespace Requester
{
    class WebInterface : IWebInterface
    {
        public string GetButton()
        {
            return Console.ReadLine();
        }

        public string GetMessage()
        {
            return Console.ReadLine();
        }

        public long GetUserId()
        {
            return long.Parse(Console.ReadLine());
        }
        /// <summary>
        /// отправляет кнопки и их значения
        /// принимает список массивов строк где первое значение - текст кнопки, второе значение - то что кнопка возрващает при нажатии
        /// </summary>
        /// <param name="buttons"></param>
        public void SendButtons(List<string[]> buttons)
        {
            var buttns = new List<string>();
            foreach (var button in buttons)
            {
                buttns.Add(button[1] + ". " + button[0]);
            }
            Console.WriteLine(string.Join(" ", buttns));
        }

        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}