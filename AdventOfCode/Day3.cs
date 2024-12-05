using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day3
    {
        //private const string MulRegex = @"mul\((?<first>[^\]\)]*?[0-9]{1,3}[^\]\)]*?),(?<second>[^\]\)]*?[0-9]{1,3}[^\]]*?)\)";
        private const string MulRegex = @"mul\((?<first>[0-9]{1,3}),(?<second>[0-9]{1,3})\)";

        [Theory]
        [InlineData("./test/day3.txt", 161)]
        [InlineData("./input/day3.txt", 157621318)]
        public void Part1(string input, int expected)
        {
            string inputString = File.ReadAllText(input);
            long result = 0;

            Regex rgx = new Regex(MulRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection matches = rgx.Matches(inputString);

            foreach (Match match in matches)
            {
                int first = int.Parse(match.Groups["first"].Value.Replace('-', ' ').Where(c => char.IsDigit(c)).ToArray());
                int second = int.Parse(match.Groups["second"].Value.Replace('-', ' ').Where(c => char.IsDigit(c)).ToArray());

                result += first * second;
            }

           
            Assert.Equal(expected, result);
        }

        private const string MulRegex2 = @"(?<do>do\(\))|(?<dont>don\'t\(\))|mul\((?<first>[0-9]{1,3}),(?<second>[0-9]{1,3})\)";
        //private const string MulRegex2 = @"[d]{1}[o]{1}\({1}\){1}|\don't\(\)";

        [Theory]
        [InlineData("./test/day3_2.txt", 48)]
        [InlineData("./input/day3.txt", 79845780)]
        public void Part2(string input, int expected)
        {
            string inputString = File.ReadAllText(input);
            long result = 0;

            Regex rgx2 = new Regex(MulRegex2, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection matches2 = rgx2.Matches(inputString);

            bool dodo = true;

            foreach (Match match in matches2)
            {

                if (match.Groups["dont"].Length > 0)
                {
                    dodo = false;
                    continue;
                }

                if (match.Groups["do"].Length > 0)
                {
                    dodo = true;
                    continue;
                }

                if (!dodo)
                {
                    continue;
                }

                int first = int.Parse(match.Groups["first"].Value.Replace('-', ' ').Where(c => char.IsDigit(c)).ToArray());
                int second = int.Parse(match.Groups["second"].Value.Replace('-', ' ').Where(c => char.IsDigit(c)).ToArray());

                result += first * second;
            }

            Assert.Equal(expected, result);
        }
    }
}
