using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace SodukoSolver
{
    public class Worker
    {
        #region Sudokus
        ////easy
        //private int[,] _grid = new int[,]
        //{
        //{0,2,0,0,0,0,0,0,0},
        //{1,5,8,0,0,0,0,3,0},
        //{3,4,0,1,6,0,9,0,2},
        //{0,0,9,2,0,8,1,7,5},
        //{0,0,0,0,4,0,0,0,0},
        //{0,3,5,6,0,1,0,0,0},
        //{0,0,0,3,0,0,5,9,4},
        //{5,1,3,4,8,0,0,0,7},
        //{6,9,0,7,5,2,0,0,3}
        //};

        ////medium
        //private int[,] _grid = new int[,]
        //{
        //    {0,0,7,0,0,3,0,0,2},
        //    {0,9,4,0,0,0,0,6,0},
        //    {0,0,0,7,2,0,0,1,0},
        //    {4,0,0,0,0,1,0,0,9},
        //    {0,0,0,0,4,0,8,0,1},
        //    {9,5,0,0,3,0,0,0,0},
        //    {0,4,0,0,8,0,0,2,6},
        //    {7,0,0,9,0,2,1,0,0},
        //    {6,0,2,0,0,0,5,0,3}
        //};

        //hard
        private int[,] _grid = new int[,]
        {
            {0,0,0,0,6,2,3,4,0},
            {0,0,6,0,3,0,0,0,1},
            {0,4,9,0,1,0,0,2,0},
            {0,0,0,0,0,0,1,0,0},
            {0,0,2,0,0,8,0,7,9},
            {7,0,0,0,0,0,0,0,0},
            {0,2,7,0,0,0,0,0,0},
            {6,0,3,1,0,0,0,0,0},
            {0,0,0,0,0,4,6,8,0}
        };

        ////expert
        //private int[,] _grid = new int[,]
        //{
        //{6,0,0,1,7,0,0,0,5},
        //{0,0,0,0,4,0,0,2,0},
        //{0,0,0,0,0,0,8,9,0},
        //{0,3,7,8,0,0,0,0,2},
        //{5,0,0,0,0,1,0,0,9},
        //{0,0,2,0,0,0,0,0,0},
        //{0,0,5,0,2,4,0,0,0},
        //{0,0,0,0,1,0,6,0,0},
        //{7,0,0,3,0,0,0,0,0}
        //};
        #endregion

        private bool _isReady = false;

        public int[,] Grid
        {
            get { return _grid; }
            set { _grid = value; }
        }

        public void SetDataInGrid(int y, int x, int value)
        {
            _grid[y - 1, x - 1] = value;
        }
        public void Start()
        {
            Print();

            CollectInformation();

            Print();

            Solve();

            Print();
        }

        public void Solve()
        {
            bool square1 = false;
            bool square2 = false;
            bool square3 = false;
            bool square4 = false;
            bool square5 = false;
            bool square6 = false;
            bool square7 = false;
            bool square8 = false;
            bool square9 = false;

            bool row1 = false;
            bool row2 = false;
            bool row3 = false;
            bool row4 = false;
            bool row5 = false;
            bool row6 = false;
            bool row7 = false;
            bool row8 = false;
            bool row9 = false;

            bool column1 = false;
            bool column2 = false;
            bool column3 = false;
            bool column4 = false;
            bool column5 = false;
            bool column6 = false;
            bool column7 = false;
            bool column8 = false;
            bool column9 = false;


            while (!square1 || !square2 || !square3 || !square4 || !square5 || !square6 || !square7 || !square8 || !square9)
            {
                for (int i = 0; i < 100; i++)
                {
                    if (!square1) square1 = Solve3By3(0, 1, 2, 0, 1, 2); //Console.WriteLine("sq1 " + square1);
                    if (!square2) square2 = Solve3By3(0, 1, 2, 3, 4, 5); //Console.WriteLine("sq2 " + square2);
                    if (!square3) square3 = Solve3By3(0, 1, 2, 6, 7, 8); //Console.WriteLine("sq3 " + square3);

                    if (!square4) square4 = Solve3By3(3, 4, 5, 0, 1, 2); //Console.WriteLine("sq4 " + square4);
                    if (!square5) square5 = Solve3By3(3, 4, 5, 3, 4, 5); //Console.WriteLine("sq5 " + square5);
                    if (!square6) square6 = Solve3By3(3, 4, 5, 6, 7, 8); //Console.WriteLine("sq6 " + square6);

                    if (!square7) square7 = Solve3By3(6, 7, 8, 0, 1, 2); //Console.WriteLine("sq7 " + square7);
                    if (!square8) square8 = Solve3By3(6, 7, 8, 3, 4, 5); //Console.WriteLine("sq8 " + square8);
                    if (!square9) square9 = Solve3By3(6, 7, 8, 6, 7, 8); //Console.WriteLine("sq9 " + square9);

                    //for (int ii = 0; ii < 30; ii++)
                    //{
                    //if (!row1) row1 = SolveByRow(0);
                    //if (!row2) row2 = SolveByRow(1);
                    //if (!row3) row3 = SolveByRow(2);
                    //if (!row4) row4 = SolveByRow(3);
                    //if (!row5) row5 = SolveByRow(4);
                    //if (!row6) row6 = SolveByRow(5);
                    //if (!row7) row7 = SolveByRow(6);
                    //if (!row8) row8 = SolveByRow(7);
                    //if (!row9) row9 = SolveByRow(8);

                    //if (!column1) column1 = SolveByColumn(0);
                    //if (!column2) column2 = SolveByColumn(1);
                    //if (!column3) column3 = SolveByColumn(2);
                    //if (!column4) column4 = SolveByColumn(3);
                    //if (!column5) column5 = SolveByColumn(4);
                    //if (!column6) column6 = SolveByColumn(5);
                    //if (!column7) column7 = SolveByColumn(6);
                    //if (!column8) column8 = SolveByColumn(7);
                    //if (!column9) column9 = SolveByColumn(8);
                    //}
                }
                break;
            }


            //Solve3By3(3, 4, 5, 0, 1, 2);
        }

        List<int> GetNumbersCompletedIn3By3(int y1, int y2, int y3, int x1, int x2, int x3)
        {
            List<int> toReturn = new List<int>();

            toReturn.Add(_grid[y1, x1]);
            toReturn.Add(_grid[y1, x2]);
            toReturn.Add(_grid[y1, x3]);

            toReturn.Add(_grid[y2, x1]);
            toReturn.Add(_grid[y2, x2]);
            toReturn.Add(_grid[y2, x3]);

            toReturn.Add(_grid[y3, x1]);
            toReturn.Add(_grid[y3, x2]);
            toReturn.Add(_grid[y3, x3]);

            toReturn.RemoveAll(i => i == 0);

            return toReturn;
        }


        private bool Solve3By3(int y1, int y2, int y3, int x1, int x2, int x3)
        {
            bool isDone = false;
            List<int[]> allGridIndexes = new List<int[]>();
            allGridIndexes.Add(new[] { y1, x1 });
            allGridIndexes.Add(new[] { y1, x2 });
            allGridIndexes.Add(new[] { y1, x3 });

            allGridIndexes.Add(new[] { y2, x1 });
            allGridIndexes.Add(new[] { y2, x2 });
            allGridIndexes.Add(new[] { y2, x3 });

            allGridIndexes.Add(new[] { y3, x1 });
            allGridIndexes.Add(new[] { y3, x2 });
            allGridIndexes.Add(new[] { y3, x3 });

            while (!isDone)
            {
                for (int jj = 0; jj < 5; jj++) //it can max go though the process without coming any further 5 times
                {
                    List<int[]> completedIndexes = new List<int[]>();
                    List<int[]> notCompletedIndexes = new List<int[]>();

                    foreach (int[] i in allGridIndexes)
                    {
                        if (_grid[i[0], i[1]] != 0) completedIndexes.Add(i);
                        else notCompletedIndexes.Add(i);
                    }

                    if (completedIndexes.Count == 9)
                    {
                        isDone = true;
                        Console.WriteLine("3x3 done");
                        return true;
                    }

                    foreach (int[] iSet in notCompletedIndexes) //goes though the incomplete fields
                    {
                        int row = iSet[0];
                        int column = iSet[1];
                        for (int i = 1; i <= 9; i++)
                        {
                            //Console.WriteLine($"Starting to check number: {i}, in the box: {y1} {y2} {y3} {x1} {x2} {x3}");
                            //location y1, y2....
                            //list of incomplete indexes
                            //index we are checking right now
                            //number we are looking for
                            bool numberGoesHere = false;
                            int numberGoesHere2 = 0;
                            int rowThatNeedsToBeChecked = -1;
                            int columnThatNeedsToBeChecked = -1;
                            List<int[]> numberCanGoHere = new List<int[]>();
                            if (GetNumbersCompletedIn3By3(y1, y2, y3, x1, x2, x3).Contains(i)) continue; //checks if the number already exists in the 3x3 area
                            if (!CheckColumn(column, i) && !CheckRow(row, i)) //if the number is NOT present on the current row or column
                            {
                                numberCanGoHere = new List<int[]>();
                                foreach (int[] incompleteFields in notCompletedIndexes)
                                {
                                    if (incompleteFields == iSet)
                                    {
                                        numberCanGoHere.Add(incompleteFields);
                                        continue;
                                    }
                                    if (CheckColumn(incompleteFields[1], i) || CheckRow(incompleteFields[0], i))
                                    {
                                        numberGoesHere2++;
                                    }
                                    else
                                    {
                                        numberCanGoHere.Add(incompleteFields);
                                        numberGoesHere = false;
                                    }
                                }

                                //Console.WriteLine("number: " + i + ", can be " + numberCanGoHere.Count + " different places");
                                //WAS HERE

                                //if (numberGoesHere2 == otherIncompleteIndexes.Count) numberGoesHere = true;
                                if (numberGoesHere2 == notCompletedIndexes.Count - 1) numberGoesHere = true;
                            }
                            if (numberGoesHere)
                            {
                                _grid[row, column] = i;
                                //Console.WriteLine("3x3 SOLVED " + row + " , " + iSet[1] + " - " + i);
                            }
                            else if (numberCanGoHere.Count >= 2 && numberCanGoHere.Count <= 3)
                            {
                                bool needToCheckOther3By3 = false;
                                int needToCheckOther3By32 = 0;
                                rowThatNeedsToBeChecked = -1;
                                //Console.WriteLine("number: " + i + ", can be " + numberCanGoHere.Count + " different places");
                                
                                //checks if the avaiable spots are on the same row
                                for (int j = 0; j < 9; j++)
                                {
                                    foreach (int[] ints in numberCanGoHere)
                                    {
                                        if (ints[0] == j)
                                        {
                                            needToCheckOther3By32++;
                                            needToCheckOther3By3 = true;
                                        }
                                        else needToCheckOther3By3 = false;
                                    }

                                    if (needToCheckOther3By3 && needToCheckOther3By32 == numberCanGoHere.Count)
                                    {
                                        rowThatNeedsToBeChecked = j;
                                        break;
                                    }

                                    needToCheckOther3By32 = 0;
                                }

                                needToCheckOther3By3 = false;
                                needToCheckOther3By32 = 0;
                                columnThatNeedsToBeChecked = -1;


                                //checks if the avaiable spots are on the same column
                                for (int j = 0; j < 9; j++)
                                {
                                    foreach (int[] ints in numberCanGoHere)
                                    {
                                        if (ints[1] == j)
                                        {
                                            needToCheckOther3By32++;
                                            needToCheckOther3By3 = true;
                                        }
                                        else needToCheckOther3By3 = false;
                                    }

                                    if (needToCheckOther3By3 && needToCheckOther3By32 == numberCanGoHere.Count)
                                    {
                                        columnThatNeedsToBeChecked = j;
                                        break;
                                    }

                                    needToCheckOther3By32 = 0;
                                }
                            }

                            /*xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                                        HERE WE CHECK THE VERTICAL OTHER 3X3 WITH THIS CROSS INFORMATION
                            xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx*/


                            #region Looks at other horizontional 3x3's
                            //if (rowThatNeedsToBeChecked != -1)
                            //{
                            //    //Console.WriteLine("row: " + rowThatNeedsToBeChecked + " contains: " + i);
                            //    //now we need to check the other 2x 3by3's with this knowledge
                            //    List<int[]> theOther23By3 = new List<int[]>();
                            //    int[] square1 = Get3By3FromIndex(rowThatNeedsToBeChecked, 0);
                            //    int[] square2 = Get3By3FromIndex(rowThatNeedsToBeChecked, 3);
                            //    int[] square3 = Get3By3FromIndex(rowThatNeedsToBeChecked, 6);
                            //    int[] squareNotToCheck = Get3By3FromIndex(iSet[0], iSet[1]);
                            //    if (!Enumerable.SequenceEqual(square1, squareNotToCheck)) theOther23By3.Add(square1);
                            //    if (!Enumerable.SequenceEqual(square2, squareNotToCheck)) theOther23By3.Add(square2);
                            //    if (!Enumerable.SequenceEqual(square3, squareNotToCheck)) theOther23By3.Add(square3);
                            //    foreach (int[] ints in theOther23By3)
                            //    {
                            //        if (!GetNumbersCompletedIn3By3(ints[0], ints[1], ints[2], ints[3], ints[4], ints[5]).Contains(i))
                            //        {
                            //            //if the 3by3 field dosent contain the number, we find witch rows that contains the number,
                            //            //here it also takes the information about the previous aquired data
                            //            bool row1AlreadyGotNumber = false;
                            //            bool row2AlreadyGotNumber = false;
                            //            bool row3AlreadyGotNumber = false;
                            //            if (ints[0] == rowThatNeedsToBeChecked || CheckRow(ints[0], i)) row1AlreadyGotNumber = true;
                            //            if (ints[1] == rowThatNeedsToBeChecked || CheckRow(ints[1], i)) row2AlreadyGotNumber = true;
                            //            if (ints[2] == rowThatNeedsToBeChecked || CheckRow(ints[2], i)) row3AlreadyGotNumber = true;

                            //            //Console.WriteLine("box: " + ints[0] + ints[1] + ints[2] + ints[3] + ints[4] + ints[5]);
                            //            //Console.WriteLine("row1="+row1AlreadyGotNumber + " , " + "row2=" + row2AlreadyGotNumber + " , " + "row3=" + row3AlreadyGotNumber);

                            //            //now we got the 3x3 square, and we got the rows that already got a number on it
                            //            List<int[]> indexesThatNeedsToBeCheked = new List<int[]>();
                            //            List<int[]> indexesThatNeedsToBeCheked2 = new List<int[]>();
                            //            if (!row1AlreadyGotNumber)
                            //            {
                            //                indexesThatNeedsToBeCheked.Add(new[] { ints[0], ints[3] });
                            //                indexesThatNeedsToBeCheked.Add(new[] { ints[0], ints[4] });
                            //                indexesThatNeedsToBeCheked.Add(new[] { ints[0], ints[5] });
                            //            }
                            //            if (!row2AlreadyGotNumber)
                            //            {
                            //                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[3] });
                            //                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[4] });
                            //                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[5] });
                            //            }
                            //            if (!row3AlreadyGotNumber)
                            //            {
                            //                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[3] });
                            //                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[4] });
                            //                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[5] });
                            //            }



                            //            //removes all the fields where there are allready numbers
                            //            foreach (int[] intse in indexesThatNeedsToBeCheked)
                            //            {
                            //                if (_grid[intse[0], intse[1]] == 0) indexesThatNeedsToBeCheked2.Add(intse);
                            //            }

                            //            //now we only have the avibale spots that are free, and we just need to run a checkcolumn
                            //            foreach (int[] intse in indexesThatNeedsToBeCheked2)
                            //            {
                            //                //Console.WriteLine("suitable spot: " + intse[0] + " , " + intse[1]);
                            //                numberGoesHere2 = 0;
                            //                numberGoesHere = false;
                            //                if (!CheckColumn(intse[1], i) && !CheckRow(intse[0], i)) //if the number is NOT present on the current row or column
                            //                {
                            //                    numberCanGoHere = new List<int[]>();
                            //                    foreach (int[] incompleteFields in indexesThatNeedsToBeCheked2)
                            //                    {
                            //                        if (incompleteFields == intse)
                            //                        {
                            //                            numberCanGoHere.Add(incompleteFields);
                            //                            continue;
                            //                        }
                            //                        if (CheckColumn(incompleteFields[1], i) || CheckRow(incompleteFields[0], i))
                            //                        {
                            //                            numberGoesHere2++;
                            //                            continue;
                            //                        }
                            //                        else
                            //                        {
                            //                            numberCanGoHere.Add(incompleteFields);
                            //                            numberGoesHere = false;
                            //                            //break;
                            //                        }
                            //                    }

                            //                    //Console.WriteLine("number: " + i + ", can be " + numberCanGoHere.Count + " different places");

                            //                    if (numberGoesHere2 == indexesThatNeedsToBeCheked2.Count - 1) numberGoesHere = true;
                            //                }
                            //                if (numberGoesHere)
                            //                {
                            //                    _grid[intse[0], intse[1]] = i;
                            //                    Console.WriteLine("3x3 ROW SOLVED " + intse[0] + " , " + intse[1] + " - " + i);
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            //Console.WriteLine("The other 3by3 already had a: " + i);
                            //        }
                            //    }
                            //}
                            #endregion


                            #region Looks at other vertical 3x3's
                            if (columnThatNeedsToBeChecked != -1)
                            {
                                //now we need to check the other 2x 3by3's with this knowledge
                                List<int[]> theOther23By3 = new List<int[]>();
                                int[] square1 = Get3By3FromIndex(0, columnThatNeedsToBeChecked);
                                int[] square2 = Get3By3FromIndex(3, columnThatNeedsToBeChecked);
                                int[] square3 = Get3By3FromIndex(6, columnThatNeedsToBeChecked);
                                int[] squareNotToCheck = Get3By3FromIndex(iSet[0], iSet[1]);
                                if (!Enumerable.SequenceEqual(square1, squareNotToCheck)) theOther23By3.Add(square1);
                                if (!Enumerable.SequenceEqual(square2, squareNotToCheck)) theOther23By3.Add(square2);
                                if (!Enumerable.SequenceEqual(square3, squareNotToCheck)) theOther23By3.Add(square3);
                                foreach (int[] ints in theOther23By3)
                                {
                                    if (!GetNumbersCompletedIn3By3(ints[0], ints[1], ints[2], ints[3], ints[4], ints[5]).Contains(i))
                                    {
                                        //if the 3by3 field dosent contain the number, we find witch columns that contains the number,
                                        //here it also takes the information about the previous acquired data
                                        bool column1AlreadyGotNumber = false;
                                        bool column2AlreadyGotNumber = false;
                                        bool column3AlreadyGotNumber = false;
                                        if (ints[3] == columnThatNeedsToBeChecked || CheckColumn(ints[3], i)) column1AlreadyGotNumber = true;
                                        if (ints[4] == columnThatNeedsToBeChecked || CheckColumn(ints[4], i)) column2AlreadyGotNumber = true;
                                        if (ints[5] == columnThatNeedsToBeChecked || CheckColumn(ints[5], i)) column3AlreadyGotNumber = true;

                                        //Console.WriteLine("box: " + ints[0] + ints[1] + ints[2] + ints[3] + ints[4] + ints[5]);
                                        //Console.WriteLine("column1="+column1AlreadyGotNumber + " , " + "column2=" + column2AlreadyGotNumber + " , " + "column3=" + column3AlreadyGotNumber);

                                        //now we got the 3x3 square, and we got the columns that already got a number on it
                                        List<int[]> indexesThatNeedsToBeCheked = new List<int[]>();
                                        List<int[]> indexesThatNeedsToBeCheked2 = new List<int[]>();
                                        if (!column1AlreadyGotNumber)
                                        {
                                            indexesThatNeedsToBeCheked.Add(new[] { ints[0], ints[3] });
                                            indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[3] });
                                            indexesThatNeedsToBeCheked.Add(new[] { ints[2], ints[3] });
                                        }
                                        if (!column2AlreadyGotNumber)
                                        {
                                            indexesThatNeedsToBeCheked.Add(new[] { ints[0], ints[4] });
                                            indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[4] });
                                            indexesThatNeedsToBeCheked.Add(new[] { ints[2], ints[4] });
                                        }
                                        if (!column3AlreadyGotNumber)
                                        {
                                            indexesThatNeedsToBeCheked.Add(new[] { ints[0], ints[5] });
                                            indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[5] });
                                            indexesThatNeedsToBeCheked.Add(new[] { ints[2], ints[5] });
                                        }



                                        //removes all the fields where there are already numbers
                                        foreach (int[] intse in indexesThatNeedsToBeCheked)
                                        {
                                            if (_grid[intse[0], intse[1]] == 0) indexesThatNeedsToBeCheked2.Add(intse);
                                        }

                                        //now we only have the avibale spots that are free, and we just need to run a checkrow
                                        foreach (int[] intse in indexesThatNeedsToBeCheked2)
                                        {
                                            //Console.WriteLine("suitable spot: " + intse[0] + " , " + intse[1]);
                                            numberGoesHere2 = 0;
                                            numberGoesHere = false;
                                            if (!CheckColumn(intse[1], i) && !CheckRow(intse[0], i)) //if the number is NOT present on the current row or column
                                            {
                                                numberCanGoHere = new List<int[]>();
                                                foreach (int[] incompleteFields in indexesThatNeedsToBeCheked2)
                                                {
                                                    if (incompleteFields == intse)
                                                    {
                                                        numberCanGoHere.Add(incompleteFields);
                                                        continue;
                                                    }
                                                    if (CheckColumn(incompleteFields[1], i) || CheckRow(incompleteFields[0], i))
                                                    {
                                                        numberGoesHere2++;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        numberCanGoHere.Add(incompleteFields);
                                                        numberGoesHere = false;
                                                        //break;
                                                    }
                                                }

                                                //Console.WriteLine("number: " + i + ", can be " + numberCanGoHere.Count + " different places");

                                                if (numberGoesHere2 == indexesThatNeedsToBeCheked2.Count - 1) numberGoesHere = true;
                                            }
                                            if (numberGoesHere)
                                            {
                                                _grid[intse[0], intse[1]] = i;
                                                Console.WriteLine("3x3 COLUMN SOLVED " + intse[0] + " , " + intse[1] + " - " + i);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Console.WriteLine("The other 3by3 already had a: " + i);
                                    }
                                }
                            }
                            #endregion


                        }
                    }
                }

                return isDone;
            }

            return isDone;

        }

        //WORKING ON THIS
        public void SolveAndSetNumberIn3x3(int y1, int y2, int y3, int x1, int x2, int x3, List<int[]> notCompletedIndexes, int[] iSet, int i, 
            bool row1AlreadyGotNumber, bool row2AlreadyGotNumber, bool row3AlreadyGotNumber, bool column1AlreadyGotNumber, bool column2AlreadyGotNumber, bool column3AlreadyGotNumber)
        {
            //location y1, y2....
            //list of incomplete indexes
            //index we are checking right now
            //number we are looking for
            int row = iSet[0];
            int column = iSet[1];
            bool numberGoesHere = false;
            int numberGoesHere2 = 0;
            int rowThatNeedsToBeChecked = -1;
            List<int[]> numberCanGoHere = new List<int[]>();
            if (!GetNumbersCompletedIn3By3(y1, y2, y3, x1, x2, x3).Contains(i)) //checks if the number already exists in the 3x3 area
            {
                //copy from where
                if (!CheckColumn(column, i) && !CheckRow(row, i)) //if the number is NOT present on the current row or column
                {
                    numberCanGoHere = new List<int[]>();
                    foreach (int[] incompleteFields in notCompletedIndexes)
                    {
                        if (incompleteFields == iSet)
                        {
                            numberCanGoHere.Add(incompleteFields);
                            continue;
                        }
                        if (CheckColumn(incompleteFields[1], i) || CheckRow(incompleteFields[0], i))
                        {
                            numberGoesHere2++;
                            continue;
                        }
                        else
                        {
                            numberCanGoHere.Add(incompleteFields);
                            numberGoesHere = false;
                            //break;
                        }
                    }

                    //Console.WriteLine("number: " + i + ", can be " + numberCanGoHere.Count + " different places");
                    
                    if (numberGoesHere2 == notCompletedIndexes.Count - 1) numberGoesHere = true;
                }
                if (numberGoesHere)
                {
                    _grid[row, column] = i;
                    Console.WriteLine("3x3 SOLVED " + row + " , " + iSet[1] + " - " + i);
                }
                //too here
                else if (numberCanGoHere.Count >= 2 && numberCanGoHere.Count <= 3)
                {
                    bool needToCheckOther3By3 = false;
                    int needToCheckOther3By32 = 0;
                    rowThatNeedsToBeChecked = -1;
                    //checks if the aviable spots are on the same row
                    Console.WriteLine("number: " + i + ", can be " + numberCanGoHere.Count + " different places");
                    for (int j = 0; j < 9; j++)
                    {
                        foreach (int[] ints in numberCanGoHere)
                        {
                            if (ints[0] == j)
                            {
                                needToCheckOther3By32++;
                                needToCheckOther3By3 = true;
                            }
                            else needToCheckOther3By3 = false;
                        }

                        if (needToCheckOther3By3 && needToCheckOther3By32 == numberCanGoHere.Count)
                        {
                            rowThatNeedsToBeChecked = j;
                            break;
                        }

                        needToCheckOther3By32 = 0;
                    }
                }
                if (rowThatNeedsToBeChecked != -1)
                {
                    Console.WriteLine("row: " + rowThatNeedsToBeChecked + " contains: " + i);
                    //now we need to check the other 2x 3by3's with this knowledge
                    List<int[]> theOther23By3 = new List<int[]>();
                    int[] square1 = Get3By3FromIndex(rowThatNeedsToBeChecked, 0);
                    int[] square2 = Get3By3FromIndex(rowThatNeedsToBeChecked, 3);
                    int[] square3 = Get3By3FromIndex(rowThatNeedsToBeChecked, 6);
                    int[] squareNotToCheck = Get3By3FromIndex(iSet[0], iSet[1]);
                    if (!Enumerable.SequenceEqual(square1, squareNotToCheck)) theOther23By3.Add(square1);
                    if (!Enumerable.SequenceEqual(square2, squareNotToCheck)) theOther23By3.Add(square2);
                    if (!Enumerable.SequenceEqual(square3, squareNotToCheck)) theOther23By3.Add(square3);
                    Console.WriteLine(theOther23By3.Count);
                    foreach (int[] ints in theOther23By3)
                    {
                        if (!GetNumbersCompletedIn3By3(ints[0], ints[1], ints[2], ints[3], ints[4], ints[5]).Contains(i))
                        {
                            //if the 3by3 field dosent contain the number, we find witch rows that contains the number,
                            //here it also takes the information about the previous aquired data
                            row1AlreadyGotNumber = false;
                            row2AlreadyGotNumber = false;
                            row3AlreadyGotNumber = false;
                            if (ints[0] == rowThatNeedsToBeChecked || CheckRow(ints[0], i)) row1AlreadyGotNumber = true;
                            if (ints[1] == rowThatNeedsToBeChecked || CheckRow(ints[1], i)) row2AlreadyGotNumber = true;
                            if (ints[2] == rowThatNeedsToBeChecked || CheckRow(ints[2], i)) row3AlreadyGotNumber = true;

                            Console.WriteLine("box: " + ints[0] + ints[1] + ints[2] + ints[3] + ints[4] + ints[5]);
                            Console.WriteLine("row1=" + row1AlreadyGotNumber + " , " + "row2=" + row2AlreadyGotNumber + " , " + "row3=" + row3AlreadyGotNumber);

                            //now we got the 3x3 square, and we got the rows that already got a number on it
                            List<int[]> indexesThatNeedsToBeCheked = new List<int[]>();
                            List<int[]> indexesThatNeedsToBeCheked2 = new List<int[]>();
                            if (!row1AlreadyGotNumber)
                            {
                                indexesThatNeedsToBeCheked.Add(new[] { ints[0], ints[3] });
                                indexesThatNeedsToBeCheked.Add(new[] { ints[0], ints[4] });
                                indexesThatNeedsToBeCheked.Add(new[] { ints[0], ints[5] });
                            }
                            if (!row2AlreadyGotNumber)
                            {
                                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[3] });
                                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[4] });
                                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[5] });
                            }
                            if (!row3AlreadyGotNumber)
                            {
                                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[3] });
                                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[4] });
                                indexesThatNeedsToBeCheked.Add(new[] { ints[1], ints[5] });
                            }



                            //removes all the fields where there are allready numbers
                            foreach (int[] intse in indexesThatNeedsToBeCheked)
                            {
                                if (_grid[intse[0], intse[1]] == 0) indexesThatNeedsToBeCheked2.Add(intse);
                            }

                            //now we only have the avibale spots that are free, and we just need to run a checkcolumn
                            foreach (int[] intse in indexesThatNeedsToBeCheked2)
                            {
                                //Console.WriteLine("suitable spot: " + intse[0] + " , " + intse[1]);
                                //if (!CheckColumn(intse[1], i))
                            }
                        }
                        else
                        {
                            Console.WriteLine("The other 3by3 already had a: " + i);
                        }
                    }
                }
                //bottom of this number in the first unsolved field
            }

        }

        private bool CheckColumn(int column, int number)
        {
            bool toReturn = false;

            for (int i = 0; i < 9; i++)
            {
                if (_grid[i, column] == number) return true;
            }

            return toReturn;
        }

        private bool CheckRow(int row, int number)
        {
            bool toReturn = false;

            for (int i = 0; i < 9; i++)
            {
                if (_grid[row, i] == number) return true;
            }

            return toReturn;
        }

        private int[] Get3By3FromIndex(int y, int x)
        {
            int[] toReturn = new[] {0, 0, 0, 0, 0, 0};

            if (y >= 0 && y <= 2 && x >= 0 && x <= 2) toReturn = new[] {0, 1, 2, 0, 1, 2};
            else if (y >= 0 && y <= 2 && x >= 3 && x <= 5) toReturn = new[] {0, 1, 2, 3, 4, 5};
            else if (y >= 0 && y <= 2 && x >= 6 && x <= 8) toReturn = new[] {0, 1, 2, 6, 7, 8};

            else if (y >= 3 && y <= 5 && x >= 0 && x <= 2) toReturn = new[] { 3, 4, 5, 0, 1, 2 };
            else if (y >= 3 && y <= 5 && x >= 3 && x <= 5) toReturn = new[] { 3, 4, 5, 3, 4, 5 };
            else if (y >= 3 && y <= 5 && x >= 6 && x <= 8) toReturn = new[] { 3, 4, 5, 6, 7, 8 };

            else if (y >= 6 && y <= 8 && x >= 0 && x <= 2) toReturn = new[] { 6, 7, 8, 0, 1, 2 };
            else if (y >= 6 && y <= 8 && x >= 3 && x <= 5) toReturn = new[] { 6, 7, 8, 3, 4, 5 };
            else if (y >= 6 && y <= 8 && x >= 6 && x <= 8) toReturn = new[] { 6, 7, 8, 6, 7, 8 };

            return toReturn;
        }

        public bool SolveByRow(int row)
        {
            bool isDone = false;

            while (!isDone)
            {
                for (int ii = 0; ii < 5; ii++)
                {
                    List<int[]> allIndexes = new List<int[]>();
                    List<int[]> completedIndexes = new List<int[]>();
                    List<int[]> incompletedIndexes = new List<int[]>();
                    List<int> completedNumbers = new List<int>();

                    for (int i = 0; i < 9; i++)
                    {
                        allIndexes.Add(new[] {row, i});
                        if (_grid[row, i] != 0)
                        {
                            completedIndexes.Add(new[] {row, i});
                            completedNumbers.Add(_grid[row, i]);
                        }
                        else incompletedIndexes.Add(new[] {row, i});
                    }

                    if (completedIndexes.Count == 9)
                    {
                        isDone = true;
                        Console.WriteLine("row " + row + " done");
                        return isDone;
                    }

                    for (int notSolvedNumber = 1; notSolvedNumber <= 9; notSolvedNumber++)
                    {

                        if (completedNumbers.Contains(notSolvedNumber)) continue;

                        foreach (int[] iSet in incompletedIndexes)
                        {
                            bool numberGoesHere = false;
                            int numberGoesHere2 = 0;
                            if (CheckColumn(iSet[1], notSolvedNumber)) continue;
                            else
                            {
                                int[] a = Get3By3FromIndex(iSet[0], iSet[1]);

                                if (!GetNumbersCompletedIn3By3(a[0], a[1], a[2], a[3], a[4], a[5])
                                    .Contains(notSolvedNumber))
                                {
                                    foreach (int[] iSet2 in incompletedIndexes)
                                    {
                                        if (iSet2 == iSet) continue;

                                        a = Get3By3FromIndex(iSet2[0], iSet2[1]);
                                        //List<int> hej = GetNumbersCompletedIn3By3(a[0], a[1], a[2], a[3], a[4], a[5]);
                                        //bool test = hej.Contains(notSolvedNumber);
                                        if (CheckColumn(iSet2[1], notSolvedNumber) ||
                                            GetNumbersCompletedIn3By3(a[0], a[1], a[2], a[3], a[4], a[5])
                                                .Contains(notSolvedNumber))
                                        {
                                            numberGoesHere2++;
                                            continue;
                                        }
                                        else
                                        {
                                            numberGoesHere = false;
                                            break;
                                        }
                                    }
                                }

                            }

                            if (numberGoesHere2 == incompletedIndexes.Count - 1) numberGoesHere = true;
                            if (numberGoesHere)
                            {
                                _grid[row, iSet[1]] = notSolvedNumber;
                                Console.WriteLine("ROW SOLVED " + row + " , " + iSet[1] + " - " + notSolvedNumber);
                            }
                        }
                    }

                    //return isDone;
                }

                return isDone;
            }

            return isDone;
        }

        public bool SolveByColumn(int column)
        {
            bool isDone = false;

            while (!isDone)
            {
                for (int ii = 0; ii < 5; ii++)
                {
                    List<int[]> allIndexes = new List<int[]>();
                    List<int[]> completedIndexes = new List<int[]>();
                    List<int[]> incompletedIndexes = new List<int[]>();
                    List<int> completedNumbers = new List<int>();

                    for (int i = 0; i < 9; i++)
                    {
                        allIndexes.Add(new[] { i, column });
                        if (_grid[i, column] != 0)
                        {
                            completedIndexes.Add(new[] { i, column });
                            completedNumbers.Add(_grid[i, column]);
                        }
                        else incompletedIndexes.Add(new[] { i, column });
                    }

                    if (completedIndexes.Count == 9)
                    {
                        isDone = true;
                        Console.WriteLine("column " + column + " done");
                        return isDone;
                    }

                    for (int notSolvedNumber = 1; notSolvedNumber <= 9; notSolvedNumber++)
                    {

                        if (completedNumbers.Contains(notSolvedNumber)) continue;

                        foreach (int[] iSet in incompletedIndexes)
                        {
                            bool numberGoesHere = false;
                            int numberGoesHere2 = 0;
                            if (CheckRow(iSet[0], notSolvedNumber)) continue;
                            else
                            {
                                int[] a = Get3By3FromIndex(iSet[0], iSet[1]);

                                if (!GetNumbersCompletedIn3By3(a[0], a[1], a[2], a[3], a[4], a[5])
                                    .Contains(notSolvedNumber))
                                {
                                    foreach (int[] iSet2 in incompletedIndexes)
                                    {
                                        if (iSet2 == iSet) continue;

                                        a = Get3By3FromIndex(iSet2[0], iSet2[1]);
                                        //List<int> hej = GetNumbersCompletedIn3By3(a[0], a[1], a[2], a[3], a[4], a[5]);
                                        //bool test = hej.Contains(notSolvedNumber);
                                        if (CheckRow(iSet2[0], notSolvedNumber) ||
                                            GetNumbersCompletedIn3By3(a[0], a[1], a[2], a[3], a[4], a[5])
                                                .Contains(notSolvedNumber))
                                        {
                                            numberGoesHere2++;
                                            continue;
                                        }
                                        else
                                        {
                                            numberGoesHere = false;
                                            break;
                                        }
                                    }
                                }

                            }

                            if (numberGoesHere2 == incompletedIndexes.Count - 1) numberGoesHere = true;
                            if (numberGoesHere)
                            {
                                _grid[iSet[0], column] = notSolvedNumber;
                                Console.WriteLine("COLUMN SOLVED " + column + " , " + iSet[0] + " - " + notSolvedNumber);
                            }
                        }
                    }
                    //return isDone;
                }
                return isDone;
            }
            return isDone;
        }



        public void CollectInformation()
        {
            while (!_isReady)
            {
                Console.WriteLine("write the information you have, ex: 3 4 8, that would mean that row number 3 on the 4th place is an number 8");
                Console.WriteLine("you can write \"show\", to see the information provided, and when you are done, write \"done\"");
                while (!_isReady)
                {
                    Console.Write("numbers: ");
                    string s = Console.ReadLine();
                    if (s == "done")
                    {
                        _isReady = true;
                        break;
                    }
                    else if (s == "show")
                    {
                        Print();
                        break;
                    }
                    var ss = s.Split(" ");
                    if (ss.Length == 3)
                    {
                        int i1 = 0;
                        int i2 = 0;
                        int i3 = 0;
                        try
                        {
                            i1 = int.Parse(ss[0]);
                            i2 = int.Parse(ss[1]);
                            i3 = int.Parse(ss[2]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        if ((i1 >= 1 && i1 <= 9) && (i2 >= 1 && i2 <= 9) && (i3 >= 1 && i3 <= 9))
                        {
                            SetDataInGrid(i1, i2, i3);
                        }
                        else
                        {
                            Console.WriteLine("data must be between 1 and 9");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void Print()
        {
            Console.WriteLine(" ");
            int j = 0;
            int l = 0;
            int k = 0;
            foreach (int i in _grid)
            {
                if (j % 9 == 0 && j != 0) { Console.WriteLine(); k++; }
                if (l % 3 == 0 && l % 9 != 0) Console.Write("| ");
                if (k == 3) {Console.WriteLine("---------------------"); k = 0; }
                Console.Write(i + " ");
                j++;
                l++;
            }

            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }
    }
}
