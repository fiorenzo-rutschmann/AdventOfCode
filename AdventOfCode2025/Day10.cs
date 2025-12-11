using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode2025;

public class Day10
{
    [Theory]
    [InlineData("./test/day10.txt", 7)]
    [InlineData("./input/day10.txt", 459)]
    public void Part1(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        Regex regx = new Regex("\\[(?<goal>[\\.#]*)\\]( \\((?<buttons>[0-9,]*)\\))*");


        foreach (var line in lines)
        {
            var matches = regx.Match(line);

            var buttons = matches.Groups["buttons"].Captures.Select(c => c.Value);

            List<ulong> longButtons = new();
            foreach (var b in buttons)
            {
                ulong bulong = 0x0;

                foreach (var position in b.Split(',').Select(int.Parse))
                {
                    bulong |= BitOperations.RotateLeft(1, position);
                }

                longButtons.Add(bulong);
            }

            var goal = matches.Groups["goal"].Value;
            ulong longGoal = 0x0;
            foreach (var i in goal.Index())
            {
                if (i.Item == '#')
                    longGoal |= BitOperations.RotateLeft(1, i.Index);
            }

            if (longButtons.Contains(longGoal))
            {
                result += 1;
                continue;
            }

            //2
            foreach(var num1 in longButtons.Index())
            {
                foreach(var num2 in longButtons.Skip(num1.Index))
                {
                    if ((num1.Item ^ num2) == longGoal)
                    {
                        result += 2;
                        goto next;
                    }
                }
            }

            bool IsBitSet(int number, int bitPosition)
            { 
                int mask = 1 << bitPosition;
                return (number & mask) != 0;
            }

            long lowest = long.MaxValue;
            for (int i = 1; i < 1 << longButtons.Count(); i++)
            {
                ulong test = 0;
                int pushes = 0;
                for(int b = 0; b < longButtons.Count(); b++)
                {
                    if (IsBitSet(i, b))
                    {
                        pushes++;
                        test ^= longButtons[b];
                    }
                }

                if (test == longGoal && pushes < lowest)
                {
                    lowest = pushes;
                }
            }

            result += lowest;

            next:
                continue;

        }
        
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day10.txt", 33)]
    [InlineData("./input/day10.txt", 0)]
    public void Part2(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        Regex regx = new Regex("""\[(?<goal>[\.#]*)\]( \((?<buttons>[0-9,]*)\))* {(?<joltage>[0-9,]+)}""");

        foreach (var line in lines)
        {
            var matches = regx.Match(line);

            var buttons = matches.Groups["buttons"].Captures.Select(c => c.Value);

            List<UInt128> longButtons = new();
            foreach (var b in buttons)
            {
                UInt128 bulong = 0x0;

                foreach (var position in b.Split(',').Select(int.Parse))
                {
                    bulong |= UInt128.RotateLeft(1, position*9);
                }

                longButtons.Add(bulong);
            }

            var joltage = matches.Groups["joltage"].Value;
            UInt128 longGoal = 0x0;

            //int GCD_Iterative(int a, int b)
            //{
            //    // The loop continues until one number becomes zero
            //    while (b != 0)
            //    {
            //        int temp = b;
            //        b = a % b; // 'b' becomes the remainder of 'a' divided by 'b'
            //        a = temp;  // 'a' becomes the old 'b'
            //    }
            //    return Math.Abs(a); // When b is 0, a is the GCD
            //}

            //int GCD_Array(byte[] numbers)
            //{
            //    if (numbers == null || numbers.Length == 0)
            //    {
            //        return 0; // Or throw an exception
            //    }

            //    int result = numbers[0];
            //    for (int i = 1; i < numbers.Length; i++)
            //    {
            //        // Calculate GCD of the current result and the next number
            //        result = GCD_Iterative(result, numbers[i]);
            //    }
            //    return result;
            //}

            byte[] jolts = joltage.Split(',').Select(byte.Parse).ToArray();

            //int divisor = GCD_Array(jolts);

            byte lowestNumber = jolts.Min();

            var divJolts = jolts.Select(j => (byte)(j - lowestNumber));

            foreach (var jolt in divJolts.Index())
            {
                longGoal |= UInt128.RotateLeft(jolt.Item, jolt.Index*9);
            }

            //UInt128 mask = UInt128.Parse("320265757102059730318470218759311257840"); // (0b1111000011110000111100001111000011110000111100001111000011110000 << 64) | 0b1111000011110000111100001111000011110000111100001111000011110000;
            UInt128 mask = UInt128.Parse("42618535191663525756665312868166664448"); //0b100000000100000000100000000100000000100000000100000000100000000100000000100000000100000000100000000100000000100000000100000000
            Dictionary<UInt128, long> computedBlocks = new();

            bool RecursiveBS(UInt128 block, out long times)
            {
                //memo
                if (computedBlocks.TryGetValue(block, out long computed))
                {
                    times = computed;
                    return times == -1? false : true;
                }

                List<long> ret = new();

                foreach(UInt128 button in longButtons)
                {
                    if (block - button == 0)
                    {
                        times = 1;
                        return true;
                    }

                    UInt128 cand = (block - button);
                    if ((cand & mask) == 0)
                    {
                        if (RecursiveBS(cand, out long candidate))
                        {
                            ret.Add(candidate);
                        }
                    }
                }

                if (ret.Count > 0)
                {
                    times = ret.Min()+1;
                    computedBlocks.Add(block, times);
                    return true;
                }
                else
                {
                    times = -1;
                    computedBlocks.Add(block, times);
                    return false;
                }
            }

            if (RecursiveBS(longGoal, out long minimumPresses))
            {
                result += (minimumPresses + lowestNumber);
            }
            else
            {
                //WTF>!
                Assert.Fail("you lose.");
            }
        }

        Assert.Equal(expected, result);
    }
}