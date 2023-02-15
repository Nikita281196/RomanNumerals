using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace RomanNumeric
{
    public partial class Calculator
    {

        /// <summary>
        /// Roman equation calculation
        /// </summary>
        /// <param name="input">Roman equation</param>
        /// <returns>Result of equation in roman type </returns>
        /// <exception cref="Exception">roman numeral error</exception>
        public static string Evaluate(string input)
        {
            var arguments = _equrationRegex.Matches(input.ToUpper());
            if (arguments is null || arguments.Count == 0) throw new Exception(nameof(arguments));

            var arabicEquration = new StringBuilder(input.ToUpper());
            foreach (Match argument in arguments.Cast<Match>())
            {
                var romanNumerics = _numberRegex.Matches(argument.Value);
                if (romanNumerics is null || romanNumerics.Count == 0) throw new Exception(nameof(romanNumerics));

                var romanNumericGroups = romanNumerics?.FirstOrDefault()?.Groups;
                if (romanNumericGroups is null) throw new Exception(nameof(romanNumericGroups));

                var arabicNumeric = 0;
                foreach (Group romanNumeric in romanNumericGroups.Cast<Group>())
                {
                    if (romanNumeric.Name == "0") continue;
                    if (romanNumeric.Length == 0) continue;

                    if (_pairsOfNumbers.TryGetValue(romanNumeric.Value, out var value)) arabicNumeric += value;
                    else throw new Exception(nameof(value));
                }
                arabicEquration.Replace(argument.Value,arabicNumeric.ToString());
            }
            var arabicResult = Calculate(arabicEquration.ToString());
            if (arabicResult > 3999) throw new Exception("result more 3999");

            var place = 10;
            var result = new StringBuilder();
            while (arabicResult != 0)
            {
                var number = arabicResult % place;
                place *= 10;
                arabicResult -= number;
                if (number == 0) continue;
                result.Insert(0, _pairsOfNumbers.FirstOrDefault(x => x.Value == number).Key);
            }
            return result.ToString();
        }
        private static double Calculate(string expression)
        {
            DataTable table = new();
            table.Columns.Add("expression", typeof(string), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return double.Parse((string)row["expression"]);
        }

        [GeneratedRegex("\\b[(M{0,3})(CM|CD|D?C{0,3})?(XC|XL|L?X{0,3})?(IX|IV|V?I{0,3})?$]+\\b")]
        private static partial Regex EqurationRegex();

        [GeneratedRegex("^(M{0,3})(CM|CD|D?C{0,3})?(XC|XL|L?X{0,3})?(IX|IV|V?I{0,3})?$")]
        private static partial Regex NumberRegex();
        private static readonly Regex _equrationRegex = EqurationRegex();
        private static readonly Regex _numberRegex = NumberRegex();

        private static readonly Dictionary<string, int> _pairsOfNumbers = new()
        {
            { "I", 1 },
            { "II", 2},
            { "III", 3 },
            { "IV", 4 },
            { "V", 5 },
            { "VI", 6 },
            { "VII", 7 },
            { "VIII", 8 },
            { "IX", 9 },
            { "X", 10 },
            { "XX", 20 },
            { "XXX", 30 },
            { "XL", 40 },
            { "L", 50 },
            { "LX", 60  },
            { "LXX" , 70 },
            { "LXXX", 80  },
            { "XC", 90 },
            { "C", 100 },
            { "CC", 200  },
            { "CCC", 300  },
            { "CD", 400  },
            { "D", 500  },
            { "DC", 600  },
            { "DCC", 700 },
            { "DCCC", 800 },
            { "CM", 900 },
            { "M", 1000 },
            { "MM", 2000 },
            { "MMM", 3000 }
        };
    }
}