using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace AdventOfCode2025;

public class Day6
{
    [Theory]
    [InlineData("./test/day6.txt", 4277556)]
    [InlineData("./input/day6.txt", 6757749566978)]
    public void Part1(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        var splitLines = lines.Select(l => l.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)).ToList();

        foreach (var i in Enumerable.Range(0, splitLines[0].Count()))
        {
            var math = splitLines.Last()[i];
            var numbers = splitLines.Select(l => l[i]);
            var longs = numbers.Take(numbers.Count() - 1).Select(long.Parse);
            
            if (math == "*")
            {
                result += longs.Aggregate((a, x) => a * x);
            }
            else
            {
                result += longs.Sum();
            }
            
            //.Aggregate(0, (accumulator, current) => { current is not "*" or "+" ? long.Parse(current) : accumulator });
        }


        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day6.txt", 3263827)]
    [InlineData("./input/day6.txt", 10603075273949)]
    public void Part2(string input, long expected)
    {
        string[] lines = File.ReadAllLines(input);

        long result = 0;

        //var splitLines = lines.Select(l => l.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)).ToList();

        //foreach (var i in Enumerable.Range(0, splitLines[0].Count()))
        //{
        //    var math = splitLines.Last()[i];
        //    var numbers = splitLines.Select(l => l[i]);
        //    var longs = numbers.Take(numbers.Count() - 1);


        //    List<long> verticals = new();

        //    int index = 0;

        //    do
        //    {
        //        string numby = string.Concat(longs.Select(s => s.Length > index ? s[index].ToString() : ""));

        //        if (String.IsNullOrWhiteSpace(numby))
        //        {
        //            break;
        //        }

        //        verticals.Add(long.Parse(numby));
        //    }
        //    while (true);


        //    if (math == "*")
        //    {
        //        result += verticals.Aggregate((a, x) => a * x);
        //    }
        //    else
        //    {
        //        result += verticals.Sum();
        //    }

        //    //.Aggregate(0, (accumulator, current) => { current is not "*" or "+" ? long.Parse(current) : accumulator });
        //}


        bool add = true;

        long runningTotal = 0;

        foreach(var (index, item) in lines.Last().Index())
        {
            if (item == '*')
            {
                result += runningTotal;
                
                add = false;
                runningTotal = 1;
            }

            if (item == '+')
            {
               result += runningTotal;

                runningTotal = 0;
                add = true;
            }

            string numby = string.Concat(lines.Select(s => char.IsNumber(s[index]) ? s[index].ToString() : "" ));

            if (String.IsNullOrWhiteSpace(numby))
            {
                continue;
            }

            if (add)
            {
                runningTotal += long.Parse(numby);
            }
            else
            {
                runningTotal *= long.Parse(numby);
            }
        }
        
        result += runningTotal;

        Assert.Equal(expected, result);
    }
}
