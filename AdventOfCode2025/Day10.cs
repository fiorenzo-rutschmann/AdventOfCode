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
    [InlineData("./test/day10.txt", 0)]
    [InlineData("./input/day10.txt", 0)]
    public void Part2(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        Assert.Equal(expected, result);
    }
}