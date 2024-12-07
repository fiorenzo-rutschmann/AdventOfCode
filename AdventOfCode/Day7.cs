using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day7
    {
        [Theory]
        [InlineData("./test/day7.txt", 3749)]
        [InlineData("./input/day7.txt", 3351424677624)]
        public void Part1(string input, long expected)
        {
            string[] inputString = File.ReadAllLines(input);
            long result = 0;

            foreach (string line in inputString) 
            {
                string[] split = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                long testValue = long.Parse(split[0]);
                uint[] numbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(a => uint.Parse(a)).ToArray();


                //brute force

                uint operators = 0;

                for (operators = 0; operators <= Math.Pow(2, (numbers.Length - 1)); operators++)
                {
                    long rolling = numbers[0];
                    for (int i = 1; i < numbers.Length; i++)
                    {
                        if (((operators >> (i - 1)) & 1) == 1)
                        {
                            rolling *= numbers[i];
                        }
                        else
                        {
                            rolling += numbers[i];
                        }

                        if (rolling > testValue)
                        {
                            break;
                        }
                    }

                    if (testValue == rolling)
                    {
                        result += testValue;
                        break;
                    }
                }
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day7.txt", 11387)]
        [InlineData("./input/day7.txt", 204976636995111)]
        public void Part2(string input, long expected)
        {
            string[] inputString = File.ReadAllLines(input);
            long result = 0;


            foreach (string line in inputString)
            {
                string[] split = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                long testValue = long.Parse(split[0]);
                uint[] numbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(a => uint.Parse(a)).ToArray();


                //brute force //part 1
                long operators = 0;
                bool found = false;
                for (operators = 0; operators <= Math.Pow(2, (numbers.Length - 1)); operators++)
                {
                    long rolling = numbers[0];
                    for (int i = 1; i < numbers.Length; i++)
                    {
                        if (((operators >> (i - 1)) & 1) == 1)
                        {
                            rolling *= numbers[i];
                        }
                        else
                        {
                            rolling += numbers[i];
                        }

                        if (rolling > testValue)
                        {
                            break;
                        }
                    }

                    if (testValue == rolling)
                    {
                        found = true;
                        result += testValue;
                        break;
                    }
                }

                //satisfies part 1 
                if (found)
                {
                    continue;
                }

                //part 2
                long ternarymax = Convert.ToInt64(Math.Pow(3, (numbers.Length - 1)));
                for (operators = 0; operators <= ternarymax; operators++)
                {
                    long rolling = numbers[0];

                    long dividend = operators;
                    for (int i = 1; i < numbers.Length; i++)
                    {
                        long place = dividend % 3;
                        dividend -= (dividend - place) / 3;

                        if (place == 0)
                        {
                            rolling *= numbers[i];
                        }
                        else if (place == 1)
                        {
                            rolling += numbers[i];
                        }
                        else if (place == 2)
                        {
                            //concat
                            rolling = long.Parse(rolling.ToString() + numbers[i].ToString());

                        }

                        if (rolling > testValue)
                        {
                            break;
                        }
                    }

                    if (testValue == rolling)
                    {
                        result += testValue;
                        break;
                    }
                }
            }

            Assert.Equal(expected, result);
        }
    }
}
