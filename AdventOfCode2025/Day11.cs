using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System.ComponentModel;
using Xunit.Abstractions;

namespace AdventOfCode2025;

public class Day11(ITestOutputHelper output)
{
    [Theory]
    [InlineData("./test/day11.txt", 5)]
    [InlineData("./input/day11.txt", 506)]
    public void Part1(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        Dictionary<string, string[]> blocks = new Dictionary<string, string[]>();

        foreach (string line in lines)
        {
            var split = line.Split(':');

            blocks.Add(split[0], split[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
        }

        void TraceBlock(string block)
        {
            if (blocks[block][0] == "out")
            {
                result += 1;
                return;
            }

            foreach(var item in blocks[block])
            {
                TraceBlock(item);
            }
        }

        TraceBlock("you");

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day11_2.txt", 2)]
    [InlineData("./input/day11.txt", 385912350172800)]
    public void Part2(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        Dictionary<string, string[]> blocks = new Dictionary<string, string[]>();

        foreach (string line in lines)
        {
            var split = line.Split(':');

            blocks.Add(split[0], split[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
        }

        HashSet<string> blocksDone = new();

        Dictionary<(string, bool, bool), long> computedBlocks = new();

        long TraceBlock(string block, bool dac, bool fft)
        {
            if (blocks[block][0] == "out")
            {
                return dac && fft ? 1 : 0;
            }

            if (block == "dac")
            {
                dac = true;
            }

            if (block == "fft")
            {
                fft = true;
            }

            var memo = (block, dac, fft);

            //memo
            if (computedBlocks.TryGetValue(memo, out long computed))
            {
                return computed;
            }

            long output = 0;
            foreach (var item in blocks[block])
            {
                output += TraceBlock(item, dac, fft);
            }

            computedBlocks.Add(memo, output);
            return output;
        }

        var res = TraceBlock("svr", false, false);

        result = res;

        Assert.Equal(expected, result);
    }
}