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
                },
                new List<(string, string)>{
                    ("Выйти", "b")
                }
            };
            return buttons;
        }

            internal static List<List<(string, string)>> Ad()
            {
                var buttons = new List<List<(string, string)>>{
                    new List<(string, string)>
                    {
                        ("Пользователи", "us")
                    },
                    new List<(string, string)>{
                        ("Прачка", "l")
                    }
                };
                if (Data.Notes.Count != 0)
                    buttons.Add(new List<(string, string)>{("Записи", "n" )});
                buttons.Add(new List<(string, string)>{("Назад", "b" )});
                return buttons;
            }

                internal static List<List<(string, string)>> Us()
                {
                    var buttons = new List<List<(string, string)>> {
                        new List<(string, string)>{
                            ("ССК", "1"),
                            ("Открывающие", "2")
                        },
                        new List<(string, string)>{
                            ("Администраторы", "3"),
                            ("Черный список", "4"),
                        },
                        new List<(string, string)>{("Назад", "b" )}
                    };
                    return buttons;
                }

                    internal static List<List<(string, string)>> Usrs(int status)
                    {
                        var buttons = new List<List<(string, string)>> {
                            new List<(string, string)>{("Добавить", "add") }
                        };
                        if ((Data.AmountOf(status) != 0 && status != 3) || (Data.AmountOf(status) > 1 && status == 3))
                            buttons[0].Add(("Удалить", "del"));
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
                        buttons.Add(new List<(string, string)> { ("Назад", "b") });
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

        internal static List<List<(string, string)>> Cl(User user)
        {
            var buttons = new List<List<(string, string)>> {
                new List<(string, string)>{
                    ("Выкл уведомления", "clnt"),
                    ("FAQ", "info")
                }
            };

            if (Data.Notes.FindIndex(delegate (TimeNote note)
            {
                return note.UserID == user.ID;
            }
            ) != -1)
                buttons.Insert(0, new List<(string, string)> { ("Отмена", "cldn") });
            if (Data.Days.FindIndex(delegate (Day day)
            {
                return (day.EmptySpaces != 0) && (day.AvailableForSSK == (user.Status == 1));
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
            if (user.Status != 3 && user.Status != 2)
            {
                buttons.Add(new List<(string, string)> { ("Выйти", "b") });
            }
            buttons.Add(new List<(string, string)> { ("Назад", "b") });
            return buttons;
        }

            internal static List<List<(string, string)>> Cln(int status)
            {
                var buttons = new List<List<(string, string)>>();
                for (var d = 0; d < Data.Days.Count; d++)
                    if (Data.Days[d].IsFree && ((Data.Days[d].AvailableForSSK && status != 0) || (!Data.Days[d].AvailableForSSK && status == 0)))
                    {
                        var button = new List<(string, string)> { (Data.Days[d].DayOfWeekR + " " + Data.Days[d].EmptySpaces.ToString(), d.ToString()) };
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
                            var button = new List<(string, string)> { (Data.WashesHours[i].ToString() + ":00 кол-во свободных машинок:" + Data.Days[day].EmptyTimes[i].ToString(), i.ToString()) };
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
                var notes = Data.Notes.FindAll(delegate (TimeNote note)
                {
                    return note.UserID == user.ID;
                }
                );
                var buttons = new List<List<(string, string)>>();
                for (var i = 0; i < notes.Count; i++)
                {
                    var button = new List<(string, string)> { (notes[i].ToString() + "\n", i.ToString()) };
                    buttons.Add(button);
                }
                buttons.Add(new List<(string, string)> { ("Назад", "b") });
                return buttons;
            }

        internal static List<List<(string, string)>> Back()
        {
            return new List<List<(string, string)>> { new List<(string, string)>{ ("Назад", "b") } };
        }
    }
}