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

                internal static List<List<(string, string)>> L()
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
                        },
                        new List<(string, string)>
                        {
                            ("Изменить пароль", "pas")
                        }
                    };
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
            var buttons = new List<List<(string, string)>> {
                new List<(string, string)>{
                    ("Выкл уведомления", "clnt"),
                    ("FAQ", "info")
                }
            };

            if (Data.Days.FindIndex(delegate (Day day)
            {
                foreach (var note in day.Notes)
                    if (note.UserID == user.ID)
                        return true;
                return false;
            }
            ) != -1)
                buttons.Insert(0, new List<(string, string)> { ("Отмена", "cldn") });
            if (Data.Days.FindIndex(delegate (Day day)
            {
                return (day.IsEmpty) && (day.AvailableForSSK == !(user.Status == 0));
            }
            ) != -1)
                buttons.Insert(0, new List<(string, string)> { ("Записаться в прачечную", "cln") });
            if (!user.NotificationStatus)
            {
                var repl = buttons[^1][0];
                repl.Item1 = "Вкл уведомления";
                buttons[^1][0] = repl;
            }
            else
            {
                var repl = buttons[^1][0];
                repl.Item1 = "Выкл уведомления";
                buttons[^1][0] = repl;
            }
            if (user.Status == 3 || user.Status == 2)
            {
                buttons.Add(new List<(string, string)> { ("Назад", "b") });
            }
            return buttons;
        }

            internal static List<List<(string, string)>> Cln(int status)
            {
                var buttons = new List<List<(string, string)>>();
                for (var d = 0; d < Data.Days.Count; d++)
                    if (Data.Days[d].IsEmpty && (Data.Days[d].AvailableForSSK == (status != 0)))
                    {
                        var button = new List<(string, string)> { (StaticDataAndMetods.DayOfWeekR(Data.Days[d].Date) + " " + Data.Days[d].EmptySpaces.ToString(), d.ToString()) };
                        buttons.Add(button);
                    }
                buttons.Add(new List<(string, string)> { ("Назад", "b") });
                return buttons;
            }

                internal static List<List<(string, string)>> Clnd(int day)
                {
                    var buttons = new List<List<(string, string)>>();
                    for (var i = 0; i < Data.Days[day].HoursWashesTable.GetLength(0); i++)
                    {
                        if (Data.Days[day].EmptyTimes[i] != 0)
                        {
                            var button = new List<(string, string)> { ((Data.WashesHours[i] + StaticDataAndMetods.Timezone).ToString() + ":00 кол-во свободных машинок:" + Data.Days[day].EmptyTimes[i].ToString(), i.ToString()) };
                            buttons.Add(button);
                        }
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
                var notes = BotAsynh.GetNotes(user.ID, true);
                var buttons = new List<List<(string, string)>>();
                for (var i = 0; i < notes.Count; i++)
                {
                    var button = new List<(string, string)> { (notes[i].ToString() + "\n", i.ToString()) };
                    buttons.Add(button);
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
                    buttons.Add(new List<(string, string)> { ("Открыл", "opd") });
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

            internal static List<List<(string, string)>> Opt(User user)
            {
                var buttons = new List<List<(string, string)>> { new List<(string, string)> { ("Да", "op"), ("Открыл", "ddn") } };
                return buttons;
            }

                internal static List<List<(string, string)>> Optd(User user)
                {
                    var buttons = new List<List<(string, string)>> { new List<(string, string)> { ("Да", "op"), ("Открыл", "ddn") } };
                    return buttons;
                }

        internal static List<List<(string, string)>> Back()
        {
            return new List<List<(string, string)>> { new List<(string, string)>{ ("Назад", "b") } };
        }
    }
}