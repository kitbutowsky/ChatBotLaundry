using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLaundry
{
    interface IWebInterface
    {
        /// <summary>
        /// отправляет стандартное отображение кнопок и их значения
        /// принимает id пользователя и список списков кортежей
        /// внутренний список - кнопки в ряд
        /// внешний список - ряды кнопок 
        /// или по нормальному список рядов кнопок
        /// в кортеже(кнопке)) первое значение - текст кнопки(label), второе значение - то что кнопка возрващает при нажатии(payload)
        /// https://dev.vk.com/api/bots/development/keyboard
        /// </summary>
        /// <param name="buttons"></param>
        void SendButtons(long id, List<List<(string, string)>> buttons);
        /// <summary>
        /// отправляет In-line кнопки и их значения
        /// принимает id пользователя и список рядов кнопок
        /// в кнопке первое значение - текст кнопки(label), второе значение - то что кнопка возрващает при нажатии(payload)
        /// https://dev.vk.com/api/bots/development/keyboard
        /// </summary>
        /// <param name="buttons"></param>
        void SendInlineButtons(long id, List<List<(string, string)>> buttons);


        /// <summary>
        /// отправляет сообщение
        /// принимает id пользователя и сообщение
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        void SendMessage(long id, string message);

        /// <summary>
        /// принимает контент в виде сообщения или нажатой кнопки
        /// </summary>
        /// <returns>id, payload, msg</returns>
        (long, string, string) GetContent();
    }
}
