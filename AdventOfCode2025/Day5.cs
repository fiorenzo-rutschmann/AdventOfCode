
using Xunit.Sdk;

namespace AdventOfCode2025;

public class Day5
{
    [Theory]
    [InlineData("./test/day5.txt", 3)]
    [InlineData("./input/day5.txt", 664)]
    public void Part1(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        List<string> freshRanges = new List<string>();
        List<string> ingredients = new List<string>();

        bool next = false;
        foreach (string line in lines)
        {
            if (line.Length < 1)
            {
                next = true;
                continue;
            }

            if (next)
            {
                ingredients.Add(line);
            }
            else
            {
                freshRanges.Add(line);
            }
        }

        var ranges = freshRanges.Select(line => line.Split('-').Select(long.Parse).ToArray()).ToList();

        foreach(var i in ingredients)
        {
            long ind = long.Parse(i);

            if(ranges.Any( r => r[0] <= ind && r[1] >= ind))
            {
                result++;
            }
        }


        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day5.txt", 14)]
    [InlineData("./input/day5.txt", 350780324308385)]
    public void Part2(string input, long expected)
    {
        string[] lines = File.ReadAllLines(input);

        long result = 0;

        List<string> freshRanges = new List<string>();
        List<string> ingredients = new List<string>();

        bool next = false;
        foreach (string line in lines)
        {
            if (line.Length < 1)
            {
                next = true;
                continue;
            }

            if (next)
            {
                ingredients.Add(line);
            }
            else
            {
                freshRanges.Add(line);
            }
        }

        var ranges = freshRanges.Select(line => line.Split('-').Select(long.Parse).ToArray()).ToList();


        var numbers = new List<long>();

        //foreach (var range in ranges)
        //{
        //    //numbers.AddRange(Enumerable.Range(range[0], (range[1]+1) - range[0]));
        //    for (long i = range[0]; i <= range[1]; i++)
        //    {
        //        numbers.Add(i);
        //    }
        //}

        var naughtyRanges = ranges.Where(r => r[0] > r[1]);


        bool Overlapps(int i, int j, out long x, out long y)
        {

            //any range is [0,0] i/e blanked out
            if (ranges[i][0] + ranges[i][1] == 0 || ranges[j][0] + ranges[j][1] == 0)
            {
                x = 0;
                y = 0;
                return false;
            }

            //range i is equal and in range j
            if (ranges[i][0] == ranges[i][1] && ranges[i][0] <= ranges[j][1] && ranges[i][1] >= ranges[j][0])
            {
                x = ranges[j][0];
                y = ranges[j][1];
                return true;
            }

            //range j is equal and in range i
            if (ranges[j][0] == ranges[j][1] && ranges[j][0] <= ranges[i][1] && ranges[j][1] >= ranges[i][0])
            {
                x = ranges[i][0];
                y = ranges[i][1];
                return true;
            }

            if (ranges[i][0] == ranges[i][1] || ranges[j][0] == ranges[j][1])
            {
                x = 55;
            }

            //range i falls in j or equals
            if (ranges[i][0] >= ranges[j][0] && ranges[i][1] <= ranges[j][1])
            {
                x = ranges[j][0];
                y = ranges[j][1];
                return true;
            }


            //range j falls in i or equals
            if (ranges[j][0] >= ranges[i][0] && ranges[j][1] <= ranges[i][1])
            {
                x = ranges[i][0];
                y = ranges[i][1];
                return true;
            }

            // range i overlapps j
            if (ranges[i][1] >= ranges[j][0] && ranges[i][1] <= ranges[j][1])
            {
                x = ranges[i][0];
                y = ranges[j][1];
                return true;
            }

            // range j overlapps i
            if (ranges[j][1] >= ranges[i][0] && ranges[j][1] <= ranges[i][1])
            {
                x = ranges[j][0];
                y = ranges[i][1];
                return true;
            }

            x = 0;
            y = 0;
            return false;
        }

        bool found = false;

        do
        {
            found = false;
            foreach (var i in Enumerable.Range(0, ranges.Count - 1))
            {
                foreach (var j in Enumerable.Range(i + 1, ranges.Count - 1 - i))
                {
                    if (Overlapps(i, j, out long x, out long y))
                    {
                        ranges[i] = [x, y];
                        ranges[j] = [0, 0];
                        found = true;
                    }
                }
            }

            ranges.RemoveAll(r => r[0] == 0 && r[1] == 0);

        } while (found);

        result = ranges.Sum(r => r[0] == r[1] ? 1 : (r[1] - r[0]) + 1);

        Assert.Equal(expected, result);
    }

    //7:38 start
    //7:53 end

    [Theory]
    [InlineData("./test/day5.txt", 14)]
    [InlineData("./input/day5.txt", 350780324308385)]
    public void Part2_Better(string input, long expected)
    {
        string[] lines = File.ReadAllLines(input);

        long result = 0;

        List<string> freshRanges = new List<string>();
        List<string> ingredients = new List<string>();

        bool next = false;
        foreach (string line in lines)
        {
            if (line.Length < 1)
            {
                next = true;
                continue;
            }

            if (next)
            {
                ingredients.Add(line);
            }
            else
            {
                freshRanges.Add(line);
            }
        }

        var ranges = freshRanges.Select(line => line.Split('-').Select(long.Parse).ToArray()).ToList();

        var sorted = ranges.OrderBy(r => r[0]).ToList();

        long current = sorted.First()[0];

        foreach (var range in sorted)
        {
            if (current >= range[0])
            {
                if (current >= range[1])
                {
                    continue;
                }

                result += range[1] - current;
            }
            else
            {
                result += range[1] - range[0];
                result++;
            }

            current = range[1];
        }

        result++;
        Assert.Equal(expected, result);
    }
}
