using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Klab.System.Code
{
    /// <summary>
    /// ASCII code generator.
    /// </summary>
    public class Code
    {
        private Code() { }

        public static string GetStatical(string seed, IEnumerable<char> enableSpecialASCIISet = null)
        {
            return Get(seed, enableSpecialASCIISet);
        }

        public static string GetRandomly(string seed, IEnumerable<char> enableSpecialASCIISet = null)
        {
            var seedNum = new Random().Next();
            var seedTime = DateTime.Now;
            return Get($"{seedNum}{seedTime.Ticks}{seed}", enableSpecialASCIISet);
        }

        private static string Get(string seed, IEnumerable<char> enableSpecialASCIISet = null)
        {
            var enableASCIISet = EnableDefaultASCIISet;
            if (enableSpecialASCIISet != null)
            {
                foreach (var c in enableSpecialASCIISet.Where(x => x >= 0 && x <= 126))
                    enableASCIISet.Add(c);
            }

            var calc = new SHA512Managed();
            var raw = calc.ComputeHash(Encoding.UTF8.GetBytes(seed));
            calc.Initialize();
            raw = raw.Select(x => (byte)(255 - x)).ToArray();
            raw = calc.ComputeHash(raw);
            calc.Dispose();

            var hexFormat = "x2";
            var sb = new StringBuilder();
            foreach (var c in raw.Select(x => (int)x))
            {
                sb.Append(enableASCIISet.Contains(c)
                    ? (char)c
                    : c.ToString(hexFormat)[1]
                );
            }

            return sb.ToString();
        }

        private static HashSet<int> EnableDefaultASCIISet = new HashSet<int>(new[]
        {
            // 0 to 9
            48, 49, 50, 51, 52, 53, 54, 55, 56, 57

            // A to Z
            , 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90

            // a to z
            , 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122
        });

        public static HashSet<char> PrintableSpecialASCIISet = new HashSet<char>(new[]
        {
            '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/'
            , ':', ';', '<', '=', '>', '?', '@'
            , '[', '\\', ']', '^', '_', '`'
            , '{', '|', '}', '~'
        });
    }
}
