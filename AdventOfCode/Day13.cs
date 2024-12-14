using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode
{
    public class Day13
    {
        private readonly ITestOutputHelper output;

        public Day13(ITestOutputHelper output)
        {
            this.output = output;
        }

        Regex rgx = new Regex(@"Button A: X\+(?<AX>\d*), Y\+(?<AY>\d*)(\n|\r|\r\n)Button B: X\+(?<BX>\d*), Y\+(?<BY>\d*)(\n|\r|\r\n)Prize: X=(?<PX>\d*), Y=(?<PY>\d*)", RegexOptions.Multiline);

        private struct ClawMachine {
            public long ax, ay, bx, by, px, py;
        }

        [Theory]
        [InlineData("./test/day13.txt", 480)]
        [InlineData("./input/day13.txt", 35255)]
        public void Part1(string file, long expected)
        {
            string input = File.ReadAllText(file);
            long result = 0;

            const int ATOKENCOST = 3;
            const int BTOKENCOST = 1;

            //parse input
            var matches = rgx.Matches(input);

            List<ClawMachine> claws = new List<ClawMachine>();

            foreach (Match match in matches)
            {
                var newclaw = new ClawMachine()
                {
                    ax = int.Parse(match.Groups["AX"].Value),
                    ay = int.Parse(match.Groups["AY"].Value),
                    bx = int.Parse(match.Groups["BX"].Value),
                    by = int.Parse(match.Groups["BY"].Value),
                    px = int.Parse(match.Groups["PX"].Value),
                    py = int.Parse(match.Groups["PY"].Value)
                };

                claws.Add(newclaw);
            }

            ////b pushes first
            //foreach (ClawMachine claw in claws)
            //{
            //    //push b until its close, the rewind whilst pushing a
            //    int bpushes = 0;
            //    int apushes = 0;

            //    while (ClawIsLessThanPrize(claw, apushes, bpushes) && bpushes < 100)
            //    {
            //        bpushes++;
            //    }

            //    //check
            //    if (ClawIsEqualToPrize(claw, apushes, bpushes))
            //    {
            //        result += (bpushes * BTOKENCOST) + (apushes * ATOKENCOST);
            //    }

            //    for (; bpushes >= 0; bpushes--)
            //    {
            //        while (ClawIsLessThanPrize(claw, apushes, bpushes) && bpushes < 100 && apushes < 100)
            //        {
            //            apushes++;
            //        }

            //        if (ClawIsEqualToPrize(claw, apushes, bpushes))
            //        {
            //            result += (bpushes * BTOKENCOST) + (apushes * ATOKENCOST);
            //            break;
            //        }
            //    }
            //}

            foreach (ClawMachine claw in claws)
            {
                int minimumspend = int.MaxValue;
                for (int b = 0; b <= 100; b++)
                {
                    for (int a = 0; a <= 100; a++)
                    {
                        if (ClawIsEqualToPrize(claw, a, b))
                        {
                            int spend = (b * BTOKENCOST) + (a * ATOKENCOST);

                            if (spend < minimumspend)
                                minimumspend = spend;
                        }
                    }
                }

                result = minimumspend < int.MaxValue ? result + minimumspend : result;
            }

            Assert.Equal(expected, result);
        }

        private bool ClawIsLessThanPrize(ClawMachine claw, int a, int b)
        {
            if (
                    ((a * claw.ax) + (b * claw.bx)) > claw.px
                || ((a * claw.ay) + (b * claw.by)) > claw.py
            )
            {
                return false;
            }

            return true;
        }

        private bool ClawIsEqualToPrize(ClawMachine claw, int a, int b)
        {
            if (
                    ((a * claw.ax) + (b * claw.bx)) == claw.px
                && ((a * claw.ay) + (b * claw.by)) == claw.py
            )
            {
                return true;
            }

            return false;
        }


        [Theory]
        [InlineData("./test/day13.txt", 80)]
        [InlineData("./input/day13.txt", 276661131175807)]
        public void Part2(string file, long expected)
        {
            string input = File.ReadAllText(file);
            long result = 0;

            const int ATOKENCOST = 3;
            const int BTOKENCOST = 1;

            //parse input
            var matches = rgx.Matches(input);

            List<ClawMachine> claws = new List<ClawMachine>();

            foreach (Match match in matches)
            {
                var newclaw = new ClawMachine()
                {
                    ax = int.Parse(match.Groups["AX"].Value),
                    ay = int.Parse(match.Groups["AY"].Value),
                    bx = int.Parse(match.Groups["BX"].Value),
                    by = int.Parse(match.Groups["BY"].Value),
                    px = int.Parse(match.Groups["PX"].Value) + 10000000000000,
                    py = int.Parse(match.Groups["PY"].Value) + 10000000000000
                };

                claws.Add(newclaw);
            }




            foreach (ClawMachine claw in claws)
            {


                //find cheapest
                long aValue = (claw.ax / ATOKENCOST) + (claw.ay / ATOKENCOST);
                long bValue = (claw.bx / BTOKENCOST) + (claw.by / BTOKENCOST);

                if (aValue >= bValue)
                {
                    long dx = claw.px / claw.ax;
                    long dy = claw.px / claw.ay;

                    //push a until its close, the rewind whilst pushing b
                    long apushes = 0;

                    apushes = dx > dy ? dy : dx;

                    while (apushes > 0)
                    {
                        long rx = claw.px - (apushes * claw.ax);
                        long ry = claw.py - (apushes * claw.ay);

                        if (rx % claw.bx == 0 && ry % claw.by == 0 && ((rx / claw.bx) == (ry / claw.by)))
                        {
                            long bpushes = (ry / claw.by);
                            long cost = (bpushes * BTOKENCOST) + (apushes * ATOKENCOST);
                            result += cost;

                            output.WriteLine($"Button A: X+{claw.ax} Y+{claw.ay}\nButton B: X+{claw.bx} Y+{claw.by}\nPrize: X={claw.px} Y={claw.py}\n\n A Pressed = {apushes}\n B Pressed {bpushes}\n");
                            break;
                        }

                        apushes--;
                    }
                }
                else if (aValue < bValue)
                {
                    long dx = claw.px / claw.bx;
                    long dy = claw.py / claw.by;

                    //push b until its close, the rewind whilst pushing a
                    long bpushes = 0;

                    bpushes = dx > dy ? dy : dx;

                    while (bpushes > 0)
                    {
                        long rx = claw.px - (bpushes * claw.bx);
                        long ry = claw.py - (bpushes * claw.by);

                        if (rx % claw.ax == 0 && ry % claw.ay == 0 && ((rx / claw.ax) == (ry / claw.ay)))
                        {
                            long apushes = (ry / claw.ay);
                            long  cost = (bpushes * BTOKENCOST) + (apushes * ATOKENCOST);
                            result += cost;

                            output.WriteLine($"Button A: X+{claw.ax} Y+{claw.ay}\nButton B: X+{claw.bx} Y+{claw.by}\nPrize: X={claw.px} Y={claw.py}\n\n A Pressed = {apushes}\n B Pressed {bpushes}\n");
                            break;
                        }

                        bpushes--;
                    }
                }
            }

            Assert.Equal(expected, result);
        }

    }
}
