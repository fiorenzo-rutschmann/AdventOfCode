

namespace AdventOfCode2025;

public class Day2
{
    [Theory]
    [InlineData("./test/day2.txt", 1227775554)]
    [InlineData("./input/day2.txt", 0)]
    public void Part1(string input, long expected)
    {
        string[] lines = File.ReadAllLines(input);

        string line1 = lines[0]; 

        long result = 0;

        var splitlines = line1.Split(',');

        foreach (var line in splitlines)
        {
            var x = line.Split('-');

            for (long i = long.Parse(x[0]); i <= long.Parse(x[1]); i++ )
            {
                result += IsInvalidNumber(i.ToString())? i : 0;
            }
        }

        Assert.Equal(expected, result);
    }

    private bool IsInvalidNumber(string v)
    {
        //split string in mid point.
        if (v.Length % 2 != 0)
            return false; //valid if odd length

        if (v.StartsWith(v.Substring(v.Length / 2)))
        {
            return true;
        }
        else
        {             
            return false;
        }
    }

    [Theory]
    [InlineData("./test/day2.txt", 4174379265)]
    [InlineData("./input/day2.txt", 0)]
    public void Part2(string input, int expected)
    {
        string[] lines = File.ReadAllLines(input);

        string line1 = lines[0];

        long result = 0;

        var splitlines = line1.Split(',');

        foreach (var line in splitlines)
        {
            var x = line.Split('-');

            for (long i = long.Parse(x[0]); i <= long.Parse(x[1]); i++)
            {
                result += IsInvalidNumber2(i.ToString()) ? i : 0;
            }
        }

        Assert.Equal(expected, result);
    }

    private bool IsInvalidNumber2(string v)
    {
        int halfLen = v.Length / 2;
        foreach ( int r in Enumerable.Range(1, halfLen))
        {
            if (v.Length % r != 0)
                continue;

            var pattern = v.Substring(0, r);
            for (int skip = r; skip < v.Length; skip += r)
            {
                if (!v.Substring(skip, r).Equals(pattern))
                {
                    break;
                }
                
                if (skip == v.Length - r)
                    return true;
            }
        }

        return false;
    }
}
