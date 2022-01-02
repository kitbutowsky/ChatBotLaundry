using System;
using System.Collections.Generic;

namespace ChatBotLaundry
{
    //класс получения кнопок
    internal class GetButtons
    {
        internal static List<List<(string, string)>> Adm()
        {
            var buttons = new List<List<(string, string)>> {
                new List<(string, string)>{
                    ("Функции клиента", "cl"),
                    ("Функции открывающего", "op")
                },
                new List<(string, string)>{
                    ("Администрирование", "ad"),
                    ("Отчетность", "re")
                }
            };
            return buttons;
        }

            internal static List<List<(string, string)>> Ad()
            {
                var buttons = new List<List<(string, string)>>{
                    new List<(string, string)>{( "Пользователи", "us" )},
                    new List<(string, string)>{( "Прачка", "l" )},
                    new List<(string, string)>{( "Записи", "n" )},
                    new List<(string, string)>{( "Назад", "b" )}
                };
                return buttons;
            }

                internal static List<List<(string, string)>> Us()
                {
                    var buttons = new List<List<(string, string)>> {
                        new List<(string, string)>{ ( "ССК", "1" ), ( "Открывающие", "2" ) },
                        new List<(string, string)>{ ( "Администраторы", "3" ), ( "Черный список", "bl" ) },
                        new List<(string, string)>{ ( "Назад", "b" ) }
                    };
                    return buttons;
                }

                    internal static List<List<(string, string)>> Usrs(string status)
                    {
                        List<List< (string, string) >> buttons = new List<List<(string, string)>>();
                        //если работа с классами доступа
                        if (int.TryParse(status, out var stts))
                        {
                            buttons.Add( new List<(string, string)>{ ( "Добавить", "add" ) });
                            var amount = 0;
                            foreach (var user in Data.Users)
                                if (user.Status == stts) amount++;
                            if ((amount != 0 && stts != 3) || (amount > 1 && stts == 3))
                                buttons[0].Add(("Удалить", "del"));
                        }
                        //если работа с заблокированными пользователями
                        else
                        {
                            var amount = 0;
                            foreach (var user in Data.Users)
                                if (user.Blocked.Item1) amount++;
                            if (amount != 0)
                                buttons.Add(new List<(string, string)> { ("Удалить", "del") });
                        }
                        buttons.Add(new List<(string, string)> { ("Назад", "b") });
                        return buttons;
                    }

                internal static List<List<(string, string)>> L(User user)
                {
                    var buttons = new List<List<(string, string)>>{
                        new List<(string, string)>
                        {
                            ("Изменить информацию о прачке", "infad")
                        },
                        new List<(string, string)>{
                            ("Изменить время", "time")
                        },
                        new List<(string, string)>
                        {
                            ("Изменить количество машинок", "w")
                        }
                    };
                    if (user.PasswordTries != 0)
                        buttons.Add(new List<(string, string)>{("Изменить пароль", "pas")});            
                    buttons.Add(new List<(string, string)> { ("Назад", "b") });
                    return buttons;
                }

                    internal static List<List<(string, string)>> Infadc()
                    {
                        var buttons = new List<List<(string, string)>>{
                            new List<(string, string)>{
                                ("Отмена", "b")
                            }
                        };
                        if (Data.NewInfo.Length != 0)
                            buttons.Insert(0, new List<(string, string)>
                            { ("Сохранить изменения" , "save")
                            });
                        return buttons;
                    }

                    internal static List<List<(string, string)>> Time()
                    {
                        var buttons = new List<List<(string, string)>> { };
                        var i = -1;
                        for (var t = 8; t < 23; t++)
                        {
                            if ((t - 8) % 5 == 0)
                            {
                                buttons.Add(new List<(string, string)>());
                                i++;
                            }
                            buttons[i].Add((t.ToString() + ":00", t.ToString()));
                        }
                        buttons.Add(new List<(string, string)> { ("Назад", "b") });
                        return buttons;
                    }

                    internal static List<List<(string, string)>> W()
                    {
                        var buttons = new List<List<(string, string)>> { new List<(string, string)>() };
                        for (var t = 1; t < 5; t++)
                        {
                            buttons[0].Add((t.ToString(), t.ToString()));
                        }
                        buttons.Add(new List<(string, string)> { ("Назад", "b") });
                        return buttons;
                    }

        internal static List<List<(string, string)>> Cl(User user)
        {
            var buttons = new List<List<(string, string)>>();
            if (Data.Days.FindIndex(delegate (Day day)
            {
                foreach (var note in day.Notes)
                    if (note.UserID == user.ID && note.Day.Date == DateTime.UtcNow.Date && day.WashesOpenerHours.Contains(DateTime.UtcNow.Hour))
                        return true;
                return false;
            }
            ) != -1)
                buttons.Add(new List<(string, string)> { ("Не открыли", "ddop") });
            if (Data.Days.FindIndex(delegate (Day day)
            {
                return (day.IsEmpty) && (day.AvailableForSSK == !(user.Status == 0));
            }
            ) != -1)
                buttons.Add(new List<(string, string)> { ("Записаться в прачечную", "cln") });
            if (Data.Days.FindIndex(delegate (Day day)
            {
                foreach (var note in day.Notes)
                    if (note.UserID == user.ID)
                        return true;
                return false;
            }
            ) != -1)
                buttons.Add(new List<(string, string)> { ("Отмена", "cldn") });
            if (!user.NotificationStatus)
                buttons.Add(new List<(string, string)> { ("Вкл уведомления", "clnt") });
            else
                buttons.Add(new List<(string, string)> { ("Выкл уведомления", "clnt") });
            buttons.Add(new List<(string, string)> { ("FAQ", "info") });
            if (user.Status == 3 || user.Status == 2)
                buttons.Add(new List<(string, string)> { ("Назад", "b") });
            return buttons;
        }

            internal static List<List<(string, string)>> Cln(int status)
            {
                var buttons = new List<List<(string, string)>>();
                var i = 0;
                for (var d = 0; d < Data.Days.Count; d++)
                    if (Data.Days[d].IsEmpty && (Data.Days[d].AvailableForSSK == (status != 0)))
                    {
                        var button = (StaticDataAndMetods.DayOfWeekR(Data.Days[d].Date) + " " + Data.Days[d].EmptySpaces.ToString(), d.ToString());
                        if (i % 2 == 0)
                            buttons.Add(new List<(string, string)> { button });
                        else
                            buttons[i / 2].Add(button);
                        i++;
                    }
                buttons.Add(new List<(string, string)> { ("Назад", "b") });
                return buttons;
            }

                internal static List<List<(string, string)>> Clnd(int day)
                {
                    var buttons = new List<List<(string, string)>>();
                    var c = 0;
                    for (var i = 0; i < Data.Days[day].HoursWashesTable.GetLength(0); i++)
                        if (Data.Days[day].EmptyTimes[i] != 0)
                        {
                            var button = ((Data.WashesHours[i] + StaticDataAndMetods.Timezone).ToString() + ":00 " + Data.Days[day].EmptyTimes[i].ToString(), i.ToString()) ;
                            if (c % 2 == 0)
                                buttons.Add(new List<(string, string)> { button });
                            else
                                buttons[c / 2].Add(button);
                            c++;
                        }
                    buttons.Add(new List<(string, string)> { ("Назад", "b") });
                    return buttons;
                }

                    internal static List<List<(string, string)>> Clndt(int day, int time)
                    {
                        var buttons = new List<List<(string, string)>>();
                        for (var i = 1; i < Data.Days[day].EmptyTimes[time] + 1; i++)
                        {
                            var button = new List<(string, string)> { (i.ToString() + ". ", i.ToString()) };
                            buttons.Add(button);
                        }
                        buttons.Add(new List<(string, string)> { ("Назад", "b") });
                        return buttons;
                    }

            internal static List<List<(string, string)>> Cldn(User user)
            {
                var buttons = new List<List<(string, string)>>();
                var c = 0;
                for (var i = 0; i < user.notes.Count; i++)
                {
                    var button = (i.ToString(), i.ToString());
                    if (c % 5 == 0)
                        buttons.Add(new List<(string, string)> { button });
                    else
                        buttons[c / 5].Add(button);
                    c++;
                }
                buttons.Add(new List<(string, string)> { ("Назад", "b") });
                return buttons;
            }

        internal static List<List<(string, string)>> Op(User user)
        {
            var buttons = new List<List<(string, string)>>();
            var today = Data.Days.Find(delegate (Day d)
            {
                return d.Date.Date == DateTime.UtcNow.Date;
            });
            //смотрит сегодняшний день и если есть время открытия на это время показывает кнопку
            var now = DateTime.UtcNow.Hour;
            if (today != null)
                if (today.WashesOpenerHours.Contains(now))
                {
                    user.OpenerIdsList = BotAsynh.GetOpenerIdsList();
                    if (user.OpenerIdsList.Count != 0)
                        buttons.Add(new List<(string, string)> { ("Открыл", "opd") });
                }
            if (Data.Days.FindIndex(delegate (Day day)
            {
                return day.IsEmptyOpener;
            }
            ) != -1)
                buttons.Add(new List<(string, string)> { ("Время открытия", "opt") });
            if (user.Status == 2)
                buttons.Add(new List<(string, string)> { ("Функции клиента", "cl") });
            if (user.Status == 3)
            {
                buttons.Add(new List<(string, string)> { ("Выйти", "b") });
            }
            return buttons;
        }

            internal static List<List<(string, string)>> Opd(User user)
            {
                var buttons = new List<List<(string, string)>> { 
                    new List<(string, string)> { ("Да", "allcm"), ("Нет", "ddn") },
                    new List<(string, string)> { ("Назад", "b") }};
                return buttons;
            }

                internal static List<List<(string, string)>> Ddn(User user)
                {
                    //кнопки с номерами и кнопка подтверждения
                    var ids = user.OpenerIdsList;
                    var buttons = new List<List<(string, string)>> {
                        new List<(string, string)> { },
                        new List<(string, string)> { ("Подтвердить", "y") }};
                    for (var i = 0; i < ids.Count; i++)
                    {
                        var button = (i.ToString(), ids[i].ToString());
                        buttons[0].Add(button);
                    }
                    return buttons;
                }


            internal static List<List<(string, string)>> Opt()
            {
                var buttons = new List<List<(string, string)>>();
                var i = 0;
                for (var d = 0; d < Data.Days.Count; d++)
                    if (Data.Days[d].IsEmptyOpener)
                    {
                        var button = (StaticDataAndMetods.DayOfWeekR(Data.Days[d].Date), d.ToString());
                        if (i % 2 == 0)
                            buttons.Add(new List<(string, string)> { button });
                        else
                            buttons[i / 2].Add(button);
                        i++;
                    }
                buttons.Add(new List<(string, string)> { ("Назад", "b") });
                return buttons;
            }

                internal static List<List<(string, string)>> Optd(int day)
                {
                    var buttons = new List<List<(string, string)>>();
                    var c = 0;
                    for (var i = 0; i < Data.Days[day].HoursWashesOpenerTable.Length; i++)
                    {
                        if (Data.Days[day].HoursWashesOpenerTable[i] == 0)
                        {
                            var button = ((Data.WashesOpenerHours[i] + StaticDataAndMetods.Timezone).ToString() + ":00 ", i.ToString());
                            if (c % 2 == 0)
                                buttons.Add(new List<(string, string)> { button });
                            else
                                buttons[c / 2].Add(button);
                            c++;
                        }
                    }
                    buttons.Add(new List<(string, string)> { ("Назад", "b") });
                    return buttons;
                }

        internal static List<List<(string, string)>> Back()
        {
            return new List<List<(string, string)>> { new List<(string, string)> { ("Назад", "b") } };
        }
    }
}