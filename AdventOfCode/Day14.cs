using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Xunit.Abstractions;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode
{
    public class Day14
    {
        private readonly ITestOutputHelper output;

        public Day14(ITestOutputHelper output)
        {
            this.output = output;
        }

        Regex rgx = new Regex(@"p=(?<x>-?\d*),(?<y>-?\d*) v=(?<vx>-?\d*),(?<vy>-?\d*)");

        private class Robot
        {
            public long x, y, vx, vy;
        }

        [Theory]
        [InlineData("./test/day14.txt", 12, 11, 7)]
        [InlineData("./input/day14.txt", 214400550, 101, 103)]
        public void Part1(string file, long expected, int width, int height)
        {
            string input = File.ReadAllText(file);
            long result = 0;

            //parse input
            var matches = rgx.Matches(input);

            List<Robot> robots = new List<Robot>();

            foreach (Match match in matches)
            {
                var newrobot = new Robot()
                {
                    x = int.Parse(match.Groups["x"].Value),
                    y = int.Parse(match.Groups["y"].Value),
                    vx = int.Parse(match.Groups["vx"].Value),
                    vy = int.Parse(match.Groups["vy"].Value),
                };

                robots.Add(newrobot);
            }

            int seconds = 100;

            foreach (Robot robot in robots)
            {
                robot.x += seconds * robot.vx;
                robot.y += seconds * robot.vy;

                robot.x = mod(robot.x, width);
                robot.y = mod(robot.y, height);
            }

            int q1 = 0, q2 = 0, q3 = 0, q4 = 0;

            foreach (Robot robot in robots)
            {
                if (robot.x < width / 2 && robot.y < height / 2)
                {
                    q1++;
                }

                if (robot.x > width / 2 && robot.y < height / 2)
                {
                    q2++;
                }

                if (robot.x < width / 2 && robot.y > height / 2)
                {
                    q3++;
                }

                if (robot.x > width / 2 && robot.y > height / 2)
                {
                    q4++;
                }
            }

            result = q1 * q2 * q3 * q4;

            Assert.Equal(expected, result);
        }

        private long mod(long x, long m)
        {
            return (x % m + m) % m;
        }

        [Theory]
        //[InlineData("./test/day14.txt", 80, 11, 7)]
        [InlineData("./input/day14.txt", 214400550, 101, 103)]
        public void Part2(string file, long expected, int width, int height)
        {
            string input = File.ReadAllText(file);
            long result = 0;

            //parse input
            var matches = rgx.Matches(input);

            List<Robot> robots = new List<Robot>();

            foreach (Match match in matches)
            {
                var newrobot = new Robot()
                {
                    x = int.Parse(match.Groups["x"].Value),
                    y = int.Parse(match.Groups["y"].Value),
                    vx = int.Parse(match.Groups["vx"].Value),
                    vy = int.Parse(match.Groups["vy"].Value),
                };

                robots.Add(newrobot);
            }

            int seconds = 9149;

            for (int i = 0; i < seconds; i++)
            {
                foreach (Robot robot in robots)
                {
                    robot.x += robot.vx;
                    robot.y += robot.vy;

                    robot.x = mod(robot.x, width);
                    robot.y = mod(robot.y, height);
                }

                Bitmap image = new Bitmap(width, height);

                Graphics g = Graphics.FromImage(image);
                //blank
                //g.DrawRectangle(new Pen(Color.Black), 0, 0, width, height);

                foreach (Robot robot in robots)
                {
                    g.DrawRectangle(new Pen(Color.White), (int)robot.x, (int)robot.y, (int)1, (int)1);
                }

                image.Save($"output1\\{i}.jpeg");
                g.Dispose();
                image.Dispose();
            } 
        
            Assert.Equal(expected, result);
        }

    }
}
