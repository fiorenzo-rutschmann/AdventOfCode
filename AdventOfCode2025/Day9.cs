namespace AdventOfCode2025;

public class Day9
{
    [Theory]
    [InlineData("./test/day9.txt", 0)]
    [InlineData("./input/day9.txt", 0)]
    public void Part1(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day9.txt", 0)]
    [InlineData("./input/day9.txt", 0)]
    public void Part2(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        Assert.Equal(expected, result);
    }
}