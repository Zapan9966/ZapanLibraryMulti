using DevZapanLibraryMulti_NETCOREAPP3_1.Models;
using RandomNameGeneratorNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DevZapanLibraryMulti_NETCOREAPP3_1.Helpers
{
    internal static class DevHelpers
    {
        private static Random _rand = new Random();

        internal static IEnumerable<ListViewDataModel> GenerateListViewData()
        {
            var datas = new List<ListViewDataModel>();
            var personGenerator = new PersonNameGenerator();

            var max = 1000;
            for (int i = 0; i < max; i++)
            {
                var data = new ListViewDataModel
                {
                    Id = GetRandomInt(1, int.MaxValue),
                    FirstName = personGenerator.GenerateRandomFirstName(),
                    LastName = personGenerator.GenerateRandomLastName(),
                    Text = GenerateRandomString(),
                    DateTime = GenerateRandomDateTime(),
                    IsActrive = GenerateRandomBoolean()
                };

                while (datas.Any(d => d.Id == data.Id))
                {
                    data.Id = GetRandomInt();
                }

                datas.Add(data);
            }
            return datas;
        }

        private static int GetRandomInt()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] rno = new byte[5];
            rng.GetBytes(rno);
            return BitConverter.ToInt32(rno, 0);
        }

        private static int GetRandomInt(int max)
        {
            RandomNumberGenerator.Create();
            return RandomNumberGenerator.GetInt32(max);
        }

        private static int GetRandomInt(int from, int to)
        {
            RandomNumberGenerator.Create();
            return RandomNumberGenerator.GetInt32(from, to);
        }

        private static string GenerateRandomString()
        {
            var length = GetRandomInt(500);
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                double flt = _rand.NextDouble();
                var shift = Convert.ToInt32(Math.Floor(25 * flt));
                var letter = Convert.ToChar(shift + 65);
                builder.Append(letter);
            }
            return builder.ToString();
        }

        private static DateTime GenerateRandomDateTime()
        {
            var start = new DateTime(1970, 1, 1);
            var range = (DateTime.Today - start).Days;

            return start
                .AddDays(GetRandomInt(range))
                .AddHours(GetRandomInt(0, 24))
                .AddMinutes(GetRandomInt(0, 60))
                .AddSeconds(GetRandomInt(0, 60));
        }

        private static bool GenerateRandomBoolean()
        {
            return _rand.NextDouble() >= 0.5;
        }
    }
}
