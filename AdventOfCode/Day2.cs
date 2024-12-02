using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

namespace AdventOfCode
{
    public class Day2
    {
        [Theory]
        [InlineData("./test/day2.txt", 2)]
        [InlineData("./input/day2.txt", 282)]
        public void Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);
            int result = 0;

            var levels = lines.Select(a => a.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(a => int.Parse(a)));

            foreach (var level in levels)
            {
                int[] arr = level.ToArray();

                bool increasingnotsafe = false;
                bool decreasingnotsafe = false;

                //increasing
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    int item = arr[i];
                    int next = arr[i + 1];

                    if (next - item <= 3 && next - item > 0)
                    {

                    }
                    else
                    {
                        increasingnotsafe = true;
                        break;
                    }
                }

                //increasing
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    int item = arr[i];
                    int next = arr[i + 1];

                    if (item - next <= 3 && item - next > 0)
                    {

                    }
                    else
                    {
                        decreasingnotsafe = true;
                        break;
                    }
                }


                if (increasingnotsafe && decreasingnotsafe)
                {
                    
                }
                else
                {
                    result += 1;
                }

            }


            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day2.txt", 4)]
        [InlineData("./input/day2.txt", 349)]
        public void Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);
            int result = 0;

            var levels = lines.Select(a => a.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(a => int.Parse(a)).ToList());

            foreach (var level in levels)
            {
                int[] arr = level.ToArray();
                if (IsSafe(arr))
                {
                    result += 1;
                }
                else
                {
                    //remove 1
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (IsSafe(arr.Where((source, index) => index != i).ToArray()))
                        {
                            result += 1;
                            break;
                        }
                    }
                }
            }

            Assert.Equal(expected, result);
        }

        private static bool IsSafe(int[] arr)
        {
            bool increasingnotsafe = false;
            bool decreasingnotsafe = false;

            //increasing
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int item = arr[i];
                int next = arr[i + 1];

                if (next - item <= 3 && next - item > 0)
                {

                }
                else
                {
                    increasingnotsafe = true;
                    break;
                }
            }

            //increasing
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int item = arr[i];
                int next = arr[i + 1];

                if (item - next <= 3 && item - next > 0)
                {

                }
                else
                {
                    decreasingnotsafe = true;
                    break;
                }
            }


            if (increasingnotsafe && decreasingnotsafe)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
