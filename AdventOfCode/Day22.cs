using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Xunit.Abstractions;

namespace AdventOfCode
{
    public class Day22
    {
        private readonly ITestOutputHelper output;

        public Day22(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day22.txt", 37327623)]
        [InlineData("./test/day22_1.txt", 1110806)]
        [InlineData("./input/day22.txt", 14726157693)]
        public void Part1(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            //read all secret numbers
            var secretInputs = input.Select(a => long.Parse(a)).ToList();

            List<long> results = new();

            for (var i = 0; i < secretInputs.Count; i++)
            {
                long current = secretInputs[i];
                long accumulator = 0;
                for (var j = 0; j < 2000; j++)
                {
                    //*64
                    accumulator = current * 64;

                    //mix;
                    current = current ^ accumulator;

                    //prune
                    current = current % 16777216;

                    //divide rounding down (integer division)
                    accumulator = current / 32;

                    //mix;
                    current = current ^ accumulator;

                    //prune
                    current = current % 16777216;

                    //* 2048;
                    accumulator = current * 2048;

                    //mix;
                    current = current ^ accumulator;

                    //prune
                    current = current % 16777216;

                }

                results.Add(current);
            }

            result = results.Sum();

            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("./test/day22_2.txt", 23)]
        [InlineData("./input/day22.txt", 1614)]
        public void Part2(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            //read all secret numbers
            var secretInputs = input.Select(a => long.Parse(a)).ToList();

            List<long> results = new();

            List<Dictionary<UInt32, int>> dictionaries = new();

            for (var i = 0; i < secretInputs.Count; i++)
            {
                UInt32 last4 = 0;
                long lastDigit = secretInputs[i] % 10;
                Dictionary<UInt32, int> currentDictionary = new();

                long current = secretInputs[i];
                long accumulator = 0;
                for (var j = 0; j < 2000; j++)
                {
                    //*64
                    accumulator = current * 64;

                    //mix;
                    current = current ^ accumulator;

                    //prune
                    current = current % 16777216;

                    //divide rounding down (integer division)
                    accumulator = current / 32;

                    //mix;
                    current = current ^ accumulator;

                    //prune
                    current = current % 16777216;

                    //* 2048;
                    accumulator = current * 2048;

                    //mix;
                    current = current ^ accumulator;

                    //prune
                    current = current % 16777216;


                    //--------------------------------------------
                    //get last digit
                    long digit = current % 10;

                    //get delta, converting to positive where negative is the abs value + 10
                    byte delta = Convert.ToByte((digit - lastDigit) >= 0 ? (digit - lastDigit) : ((digit - lastDigit) * -1) + 10 );

                    //dont forget to update last digit!!
                    lastDigit = digit;

                    //add to last4
                    //last4 <<= 8;
                    //last4 |= delta;
                    byte[] Last4Bytes = BitConverter.GetBytes(last4);
                    last4 = BitConverter.ToUInt32([Last4Bytes[1], Last4Bytes[2], Last4Bytes[3], delta]);

                    //save in dictionary
                    if (j >= 3)
                        currentDictionary.TryAdd(last4, (int)digit);
                }

                dictionaries.Add(currentDictionary);
            }

            //UInt32 currentMaxlast4 = 0;
            //long currentMaxCount = 0;

            //foreach (var d in dictionaries) 
            //{
            //    var notd = dictionaries.Where(a => a != d).ToList();

            //    foreach (var kd in d.Keys)
            //    {
            //        long currentCount = d[kd];

            //        foreach (var ndd in notd)
            //        {
            //            if (ndd.TryGetValue(kd, out int value))
            //            {
            //                currentCount += value;
            //            }
            //        }

            //        if (currentCount > currentMaxCount)
            //        {
            //            currentMaxCount = currentCount;
            //            currentMaxlast4 = kd;
            //        }
            //    }
            //}

            //merge the dictionaries

            Dictionary<UInt32, int> outputDict = new();
            foreach (var d in dictionaries)
            {
                foreach (var key in d.Keys)
                {
                    var value = d[key];
                    
                    if (!outputDict.TryAdd(key, value))
                    {
                        outputDict[key] += value;
                    }
                }
            }

            result = outputDict.Max(d => d.Value);

            var test = outputDict.First(d => d.Value == result);
            byte[] Last4Bytes1 = BitConverter.GetBytes(test.Key); 

            Assert.Equal(expected, result);
        }
    }
}
