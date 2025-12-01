using System.Diagnostics;

namespace AdventOfCode2025
{
    public class Day1
    {
        [Theory]
        [InlineData("./test/day1.txt", 3)]
        [InlineData("./input/day1.txt", 1177)]
        public void Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            int result = 0;

            int dial = 5050;

            foreach (string line in lines)
            {
                int number = int.Parse(line.Substring(1));
                if (line.StartsWith("L"))
                {
                    dial -= number;
                }
                else
                    dial += number;

                if (dial % 100 == 0)
                    result++;
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day1.txt", 6)]
        [InlineData("./input/day1.txt", 6768)]
        public void Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            int result = 0;

            int dial = 50;

            foreach (string line in lines)
            {
                int number = int.Parse(line.Substring(1));
                if (line.StartsWith("L"))
                {
                    for(int i = 0; i < number; i++)
                    {
                        dial = dial == 0 ? 100 : dial;
                        dial -= 1;
                        if (dial == 0)
                            result++;
                    }
                }
                else
                {
                    for (int i = 0; i < number; i++)
                    {
                        dial = dial == 99 ? -1 : dial;
                        dial += 1;
                        if (dial == 0)
                            result++;
                    }
                }
            }

            Assert.Equal(expected, result);
        }

        
    }
}
