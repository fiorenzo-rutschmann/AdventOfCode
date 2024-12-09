using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit.Abstractions;

namespace AdventOfCode
{
    public class Day9
    {
        private readonly ITestOutputHelper output;

        public Day9(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day9.txt", 1928)]
        [InlineData("./input/day9.txt", 6323641412437)]
        public void Part1(string file, long expected)
        {
            string input = File.ReadAllText(file);

            int[] ints = input.ToList().ConvertAll(x => int.Parse(x.ToString())).ToArray();
            long result = 0;

            int currentIndex = 0;
            for (int i = 0; i < ints.Length; i++)
            {
                if (i % 2 == 0) //even
                {
                    int id = i / 2;
                    //result += Enumerable.Range(currentIndex, ints[i]).Sum(x => x * id);
                    for (int k = currentIndex; k < currentIndex + ints[i]; k++)
                    {
                        result += k * id;
                    }

                    currentIndex += ints[i];
                }
                else //odd
                {
                    int space = ints[i];

                    //take space number from last item
                    for (int j = ints.Length-1; j > i-1 ; j--)
                    {
                        if (j % 2 == 0)
                        {
                            if (space > 0)
                            {
                                if(space > ints[j])
                                {
                                    space -= ints[j];

                                    //result += Enumerable.Range(currentIndex, ints[j]).Sum(x => x * (j/2));

                                    for (int k = currentIndex; k < currentIndex + ints[j]; k++)
                                    {
                                        result += k * (j / 2);
                                    }

                                    currentIndex += ints[j];

                                    ints[j] = 0;
                                }
                                else
                                {
                                    ints[j] -= space;

                                    //result += Enumerable.Range(currentIndex, space).Sum(x => x * (j / 2));
                                    for(int k = currentIndex; k < currentIndex+space; k++)
                                    {
                                        result += k * (j / 2);
                                    }

                                    currentIndex += space;

                                    space = 0;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day9.txt", 2858)]
        [InlineData("./input/day9.txt", 6351801932670)]
        public void Part2(string file, long expected)
        {
            string input = File.ReadAllText(file);

            int[] ints = input.ToList().ConvertAll(x => int.Parse(x.ToString())).ToArray();
            long result = 0;

            List<(int, int)> tuple = new();

            for (int i = 0; i < ints.Length; i++)
            {
                if (i % 2 == 0) //even
                {
                    int id = i / 2;

                    tuple.Add((id, ints[i]));
                }
                else
                {
                    tuple.Add((-1, ints[i]));
                }
            }

            tuple.Reverse();
            (int, int)[] revtuple = tuple.ToArray();

            bool more = true;
            int index = 0;
            int indexInsert = 0;
            while (more)
            {
                if (index >= revtuple.Length)
                    break;

                more = false;

                
                (int, int) tupleInsert = (0,0); 

                for (int i = index; i < revtuple.Length; i++)
                {
                    var item = revtuple[i];
                    if (item.Item1 != -1 && item.Item1 != -2)
                    {
                        int count = item.Item2;
                        int foundIndex = revtuple.ToList().FindLastIndex(a => a.Item1 == -1 && a.Item2 >= count);
                        if (foundIndex != -1 && foundIndex > i)
                        {
                            revtuple[foundIndex].Item2 -= count;
                            indexInsert = foundIndex+1;
                            tupleInsert = revtuple[i];
                            revtuple[i].Item1 = -2; //maybe still -1?
                            //revtuple[i].Item2 = 0;
                            more = true;
                            index = i + 1;
                            break;
                        }
                    }
                }

                if (more)
                {
                    var tempList = revtuple.ToList();
                    tempList.Insert(indexInsert, tupleInsert);
                    revtuple = tempList.ToArray();

                    //print
                    //StringBuilder sb = new();
                    //for (int i = 0; i < revtuple.Length; i++)
                    //{
                    //    var item = revtuple[i];
                    //    char x = ' ';
                    //    if (item.Item1 == -1 || item.Item1 == -2)
                    //    {
                    //        x = '.';
                    //    }
                    //    else
                    //    {
                    //        x = item.Item1.ToString()[0];
                    //    }

                    //    for (int j = 0; j < revtuple[i].Item2; j++)
                    //    {
                    //        sb.Append(x);
                    //    }
                    //}

                    //output.WriteLine(new string(sb.ToString().Reverse().ToArray()));
                }
            }
            
            var outTuple = revtuple.Reverse().ToArray();

            int currentIndex = 0;
            for (int i = 0; i < outTuple.Length; i++)
            {
                (int, int) item = outTuple[i];
                if (item.Item1 != -1 && item.Item1 != -2)
                {
                    for (int k = currentIndex; k < currentIndex + item.Item2; k++)
                    {
                        result += k * item.Item1;
                    }

                    currentIndex += item.Item2;
                }
                else
                {
                    currentIndex += item.Item2;
                }
            }


            //foreach(var i in revtuple.Index())
            //{
            //    if (i.Item1 != -1)
            //    {
            //        int count = i.Item.Item2;
            //        var foundIndex = revtuple.FindLastIndex(a => a.Item1 == -1 && a.Item2 < count);

            //        if (foundIndex != -1)
            //        {
            //            revtuple[foundIndex].Item2 -= count;

                        
            //        }
            //    }
            //}

            //(int,int)[] tupleArray = tuple.ToArray();

            //for (int j = tupleArray.Length - 1; j > 0; j--)
            //{
            //    if (j % 2 == 0)
            //    {
            //        for (int i = 0; i < j; i++)
            //        {
            //            var iv = tupleArray[i];
            //            var jv = tupleArray[j];

            //            if (iv.Item1 == -1 && iv.Item2 < )
            //            {

            //            }
            //        }
            //    }
            //}


             Assert.Equal(expected, result);
        }
    }
}
