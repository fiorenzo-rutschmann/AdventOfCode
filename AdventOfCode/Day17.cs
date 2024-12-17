using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Diagnostics.Metrics;
using Xunit.Abstractions;

namespace AdventOfCode
{
    public class Day17
    {
        private readonly ITestOutputHelper output;

        public Day17(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day17.txt", "4,6,3,5,6,3,5,2,1,0")]
        [InlineData("./input/day17.txt", "1,6,7,4,3,0,5,0,6")]
        public void Part1(string file, string expected)
        {
            string[] input = File.ReadAllLines(file);
            string result = "";

            uint A = uint.Parse(input[0]["Register A: ".Length..]);
            uint B = uint.Parse(input[1]["Register B: ".Length..]);
            uint C = uint.Parse(input[2]["Register C: ".Length..]);

            uint[] program = input[4]["Program: ".Length..].Split(",").Select(a => uint.Parse(a)).ToArray();

            uint IP = 0;

            List<uint> outp = new(); 

            while (IP < program.Length)
            {
                switch (program[IP])
                {
                    case 0:
                        {
                            //adv
                            uint combo = GetComboValue(program[IP + 1], A, B, C);
                            uint output = A / (uint)Math.Pow(2.0, (double)combo); ;
                            A = output;
                            IP += 2;
                            break;
                        }
                    case 1:
                        {
                            //bxl
                            B = B ^ program[IP + 1];
                            IP += 2;
                            break;
                        }
                    case 2:
                        {
                            //bst
                            uint combo = GetComboValue(program[IP + 1], A, B, C);
                            B = combo % 8;
                            IP += 2;
                            break;
                        }
                    case 3:
                        {
                            //jnz
                            if (A != 0)
                            {
                                IP = program[IP + 1];
                            }
                            else
                            {
                                IP += 2;
                            }
                            break;
                        }
                    case 4:
                        {
                            //bxc
                            B = B ^ C;
                            IP += 2;
                            break;
                        }
                    case 5:
                        {
                            //out
                            uint combo = GetComboValue(program[IP + 1], A, B, C);
                            outp.Add(combo % 8);
                            IP += 2;
                            break;
                        }
                    case 6:
                        {
                            //bdv
                            uint combo = GetComboValue(program[IP + 1], A, B, C);
                            uint output = A / (uint)Math.Pow(2.0, (double)combo); ;
                            B = output;
                            IP += 2;
                            break;
                        }
                    case 7:
                        {
                            //cdv
                            uint combo = GetComboValue(program[IP + 1], A, B, C);
                            uint output = A / (uint)Math.Pow(2.0, (double)combo); ;
                            C = output;
                            IP += 2;
                            break;
                        }
                    default: Assert.Fail("not an operator"); break;
                }
            }

            result = string.Join(",", outp);


            Assert.Equal(expected, result);
        }

        private uint GetComboValue(uint v, uint a, uint b, uint c)
        {
            switch (v)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return v;
                case 4:
                    return a;
                case 5:
                    return b;
                case 6:
                    return c;
                case 7:
                default:
                    Assert.Fail("not an combo value!!!");
                    return 0;
            }
        }

        private ulong GetComboValue(ulong v, ulong a, ulong b, ulong c)
        {
            switch (v)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return v;
                case 4:
                    return a;
                case 5:
                    return b;
                case 6:
                    return c;
                case 7:
                default:
                    Assert.Fail("not an combo value!!!");
                    return 0;
            }
        }


        private string ELFCPU(uint[] program, ulong A, ulong B, ulong C)
        {

            //A = 0
            //B = 0
            //C = 0

            //2,4,1,3,7,5,0,3,1,5,4,1,5,5,3,0

            ulong inp = A;

            int PP = 0;

            uint IP = 0;

            List<ulong> outp = new();

            uint ticks = 0;

            while (IP < program.Length)
            {
                switch (program[IP])
                {
                    case 0:
                        {
                            //adv
                            ulong combo = GetComboValue(program[IP + 1], A, B, C);
                            ulong output = A / ULongPow(2, combo);//(ulong)Math.Pow(2.0, (double)combo);
                            A = output;
                            IP += 2;
                            break;
                        }
                    case 1:
                        {
                            //bxl
                            B = B ^ program[IP + 1];
                            IP += 2;
                            break;
                        }
                    case 2:
                        {
                            //bst
                            ulong combo = GetComboValue(program[IP + 1], A, B, C);
                            B = combo % 8;
                            IP += 2;
                            break;
                        }
                    case 3:
                        {
                            //jnz
                            if (A != 0)
                            {
                                IP = program[IP + 1];
                            }
                            else
                            {
                                IP += 2;
                            }
                            break;
                        }
                    case 4:
                        {
                            //bxc
                            B = B ^ C;
                            IP += 2;
                            break;
                        }
                    case 5:
                        {
                            //out
                            ulong combo = GetComboValue(program[IP + 1], A, B, C);

                            ulong res = combo % 8;
                            //return early
                            if (program[PP++] != res)
                            {
                                return "";
                            }

                            outp.Add(res);

                            //output.WriteLine($"{inp}");

                            IP += 2;
                            break;
                        }
                    case 6:
                        {
                            //bdv
                            ulong combo = GetComboValue(program[IP + 1], A, B, C);
                            ulong output = A / ULongPow(2, combo);//(ulong)Math.Pow(2.0, (double)combo); ;
                            B = output;
                            IP += 2;
                            break;
                        }
                    case 7:
                        {
                            //cdv
                            ulong combo = GetComboValue(program[IP + 1], A, B, C);
                            ulong output = A / ULongPow(2, combo); //(ulong)Math.Pow(2.0, (double)combo); ;
                            C = output;
                            IP += 2;
                            break;
                        }
                    default: Assert.Fail("not an operator"); break;
                }

                //infinite loop
                if (ticks++ > 10000)
                {
                    break;
                }
            }

            return string.Join(",", outp);
        }


        ulong ULongPow(ulong x, ulong pow)
        {
            ulong ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
        }



        [Theory]
        [InlineData("./test/day17_1.txt", 117440)]
        [InlineData("./input/day17.txt", 0)]
        public void Part2(string file, ulong expected)
        {
            ulong result = 0;
            string[] input = File.ReadAllLines(file);

            //uint A = uint.Parse(input[0]["Register A: ".Length..]);
            uint B = uint.Parse(input[1]["Register B: ".Length..]);
            uint C = uint.Parse(input[2]["Register C: ".Length..]);

            uint[] program = input[4]["Program: ".Length..].Split(",").Select(a => uint.Parse(a)).ToArray();

            string programString = input[4]["Program: ".Length..];


            ulong max = ulong.MaxValue - 1000;

            ulong A = 0;//122096000673
            string outp = "";

            while(!outp.Equals(programString) && A < max)
            {
                //ulong A1 = A;
                //ulong B1 = 0;
                //ulong C1 = 0;

                //B1 = A1 % 8;
                //B1 = B1 ^ 3;
                //C1 = A1 / (ulong)Math.Pow(2.0, (double)B1);
                //A1 = A1 / (ulong)Math.Pow(2.0, (double)3);
                //B1 = B1 ^ 5;
                //B1 = B1 ^ C1;

                //if (B1 == 2)
                //{
                //    outp = ELFCPU(program, A, B, C);
                //}

                outp = ELFCPU(program, A, B, C);

                A++;
            } 

            result = A-1;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FirstNumber()
        {
            List<uint> AList = new();

            uint counter = 0;

            for (counter = 0; counter < 1_000_000_000; counter++)
            {
                ulong A1 = counter;
                ulong B1 = 0;
                ulong C1 = 0;

                B1 = A1 % 8;
                B1 = B1 ^ 3;
                C1 = A1 / (ulong)Math.Pow(2.0, (double)B1);
                A1 = A1 / (ulong)Math.Pow(2.0, (double)3);
                B1 = B1 ^ 5;
                B1 = B1 ^ C1;

                if (B1 == 2)
                {
                    AList.Add(counter);
                }
            }

            Assert.Fail();
        }
    }
}
