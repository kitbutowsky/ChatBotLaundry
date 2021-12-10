using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    interface IWebInterface
    {
        /// <summary>
        /// отправляет кнопки и их значения
        /// принимает список массивов строк где первое значение - текст кнопки, второе значение - то что кнопка возрващает при нажатии
        /// </summary>
        /// <param name="buttons"></param>
        void SendButtons(List<string[]> buttons);
        void SendMessage(string message);
        (long, string) GetButton();
        string GetMessage();
    }
}
