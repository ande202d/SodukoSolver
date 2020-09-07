using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SodukoSolver
{
    public class WorkerV3
    {
        #region InstanceFields and stuff

        private int[,] _grid; //to solve
        private int[,] _backup; //back up when math have been applied
        private int[,] _originalCopy; //copy of the original


        public enum Difficulty
        {
            Clear, Easy, Medium, Hard, VeryHard, Expert
        }

        private Difficulty _difficulty;
        private bool _isReadyToBeSolved = false;

        private List<int[]> _allIndexes = new List<int[]>();
        private List<List<int[]>> _allSquaresInOneList = new List<List<int[]>>();

        private List<int[]> _square1Indexes = new List<int[]>();
        private List<int[]> _square2Indexes = new List<int[]>();
        private List<int[]> _square3Indexes = new List<int[]>();
        private List<int[]> _square4Indexes = new List<int[]>();
        private List<int[]> _square5Indexes = new List<int[]>();
        private List<int[]> _square6Indexes = new List<int[]>();
        private List<int[]> _square7Indexes = new List<int[]>();
        private List<int[]> _square8Indexes = new List<int[]>();
        private List<int[]> _square9Indexes = new List<int[]>();
        private bool _isSolved;
        private DateTime _timeStarted;
        private DateTime _timeFinished;
        private int _solvedByMathUses = 0;
        private int _algorithmUses = 0;


        public bool IsSolved
        {
            get { return _isSolved; }
            set { _isSolved = value; }
        }

        #endregion



        #region Methods coppied from V2

        private List<int> GetNumbersInArea(List<int[]> area)
        {
            List<int> toReturn = new List<int>();

            foreach (int[] box in area)
            {
                int i = GetNumberFromIndex(box);
                if (i != 0) toReturn.Add(i);
            }

            return toReturn;
        }

        private int GetNumberFromIndex(int[] i)
        {
            return _grid[i[0], i[1]];
        }

        private void SetDifficulty()
        {
            if (_difficulty == Difficulty.Clear)
            {
                _grid = new int[,]
                {
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0}
                };
                _originalCopy = (int[,])_grid.Clone();
            }
            else if (_difficulty == Difficulty.Easy)
            {
                _grid = new int[,]
                {
                    {0,2,0,0,0,0,0,0,0},
                    {1,5,8,0,0,0,0,3,0},
                    {3,4,0,1,6,0,9,0,2},
                    {0,0,9,2,0,8,1,7,5},
                    {0,0,0,0,4,0,0,0,0},
                    {0,3,5,6,0,1,0,0,0},
                    {0,0,0,3,0,0,5,9,4},
                    {5,1,3,4,8,0,0,0,7},
                    {6,9,0,7,5,2,0,0,3}
                };
                _originalCopy = (int[,])_grid.Clone();
            }
            else if (_difficulty == Difficulty.Medium)
            {
                _grid = new int[,]
                {
                    {0,0,7,0,0,3,0,0,2},
                    {0,9,4,0,0,0,0,6,0},
                    {0,0,0,7,2,0,0,1,0},
                    {4,0,0,0,0,1,0,0,9},
                    {0,0,0,0,4,0,8,0,1},
                    {9,5,0,0,3,0,0,0,0},
                    {0,4,0,0,8,0,0,2,6},
                    {7,0,0,9,0,2,1,0,0},
                    {6,0,2,0,0,0,5,0,3}
                };
                _originalCopy = (int[,])_grid.Clone();
            }
            else if (_difficulty == Difficulty.Hard)
            {
                //_grid = new int[,]
                //{
                //    {0,0,0,0,6,2,3,4,0},
                //    {0,0,6,0,3,0,0,0,1},
                //    {0,4,9,0,1,0,0,2,0},
                //    {0,0,0,0,0,0,1,0,0},
                //    {0,0,2,0,0,8,0,7,9},
                //    {7,0,0,0,0,0,0,0,0},
                //    {0,2,7,0,0,0,0,0,0},
                //    {6,0,3,1,0,0,0,0,0},
                //    {0,0,0,0,0,4,6,8,0}
                //};
                _grid = new int[,]
                {
                    {0,2,4,0,1,0,0,0,6},
                    {6,0,0,4,0,0,0,0,8},
                    {0,5,0,0,0,0,9,0,0},
                    {3,0,0,7,0,0,4,0,0},
                    {0,6,2,0,0,9,0,3,5},
                    {0,0,0,0,0,3,0,0,0},
                    {0,0,0,0,9,0,7,5,0},
                    {9,3,0,0,5,0,0,0,0},
                    {0,0,1,8,0,0,6,0,0}
                };
                _originalCopy = (int[,])_grid.Clone();
            }
            else if (_difficulty == Difficulty.VeryHard)
            {
                _grid = new int[,]
                {
                    {0,0,0,0,1,0,0,0,6},
                    {0,0,9,0,0,7,1,0,8},
                    {8,0,4,0,0,0,0,0,0},
                    {0,0,0,0,3,8,0,0,0},
                    {6,0,0,0,0,0,0,5,0},
                    {0,0,5,1,7,0,0,0,0},
                    {1,0,0,0,8,0,0,2,0},
                    {0,5,0,4,0,9,0,0,0},
                    {0,2,0,0,0,0,3,9,0}
                };
                _originalCopy = (int[,])_grid.Clone();
            }
            else if (_difficulty == Difficulty.Expert)
            {
                _grid = new int[,]
                {
                    {6,0,0,1,7,0,0,0,5},
                    {0,0,0,0,4,0,0,2,0},
                    {0,0,0,0,0,0,8,9,0},
                    {0,3,7,8,0,0,0,0,2},
                    {5,0,0,0,0,1,0,0,9},
                    {0,0,2,0,0,0,0,0,0},
                    {0,0,5,0,2,4,0,0,0},
                    {0,0,0,0,1,0,6,0,0},
                    {7,0,0,3,0,0,0,0,0}
                };
                _originalCopy = (int[,])_grid.Clone();
            }
            else
            {
                _grid = new int[,]
                {
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0}
                };
                _originalCopy = (int[,])_grid.Clone();
            }
        }

        private void getAndsetAllIndexes()
        {
            int j1 = 0;
            for (int i = 0; i < _grid.Length; i++)
            {
                if (j1 == 9) j1 = 0;

                if (i < 9 * 1) { _allIndexes.Add(new[] { 0, j1 }); j1++; }
                else if (i < 9 * 2) { _allIndexes.Add(new[] { 1, j1 }); j1++; }
                else if (i < 9 * 3) { _allIndexes.Add(new[] { 2, j1 }); j1++; }
                else if (i < 9 * 4) { _allIndexes.Add(new[] { 3, j1 }); j1++; }
                else if (i < 9 * 5) { _allIndexes.Add(new[] { 4, j1 }); j1++; }
                else if (i < 9 * 6) { _allIndexes.Add(new[] { 5, j1 }); j1++; }
                else if (i < 9 * 7) { _allIndexes.Add(new[] { 6, j1 }); j1++; }
                else if (i < 9 * 8) { _allIndexes.Add(new[] { 7, j1 }); j1++; }
                else if (i < 9 * 9) { _allIndexes.Add(new[] { 8, j1 }); j1++; }
            }

            _square1Indexes.AddRange(_allIndexes.GetRange(9 * 0 + 0, 3));
            _square1Indexes.AddRange(_allIndexes.GetRange(9 * 1 + 0, 3));
            _square1Indexes.AddRange(_allIndexes.GetRange(9 * 2 + 0, 3));

            _square2Indexes.AddRange(_allIndexes.GetRange(9 * 0 + 3, 3));
            _square2Indexes.AddRange(_allIndexes.GetRange(9 * 1 + 3, 3));
            _square2Indexes.AddRange(_allIndexes.GetRange(9 * 2 + 3, 3));

            _square3Indexes.AddRange(_allIndexes.GetRange(9 * 0 + 6, 3));
            _square3Indexes.AddRange(_allIndexes.GetRange(9 * 1 + 6, 3));
            _square3Indexes.AddRange(_allIndexes.GetRange(9 * 2 + 6, 3));

            _square4Indexes.AddRange(_allIndexes.GetRange(9 * 3 + 0, 3));
            _square4Indexes.AddRange(_allIndexes.GetRange(9 * 4 + 0, 3));
            _square4Indexes.AddRange(_allIndexes.GetRange(9 * 5 + 0, 3));

            _square5Indexes.AddRange(_allIndexes.GetRange(9 * 3 + 3, 3));
            _square5Indexes.AddRange(_allIndexes.GetRange(9 * 4 + 3, 3));
            _square5Indexes.AddRange(_allIndexes.GetRange(9 * 5 + 3, 3));

            _square6Indexes.AddRange(_allIndexes.GetRange(9 * 3 + 6, 3));
            _square6Indexes.AddRange(_allIndexes.GetRange(9 * 4 + 6, 3));
            _square6Indexes.AddRange(_allIndexes.GetRange(9 * 5 + 6, 3));

            _square7Indexes.AddRange(_allIndexes.GetRange(9 * 6 + 0, 3));
            _square7Indexes.AddRange(_allIndexes.GetRange(9 * 7 + 0, 3));
            _square7Indexes.AddRange(_allIndexes.GetRange(9 * 8 + 0, 3));

            _square8Indexes.AddRange(_allIndexes.GetRange(9 * 6 + 3, 3));
            _square8Indexes.AddRange(_allIndexes.GetRange(9 * 7 + 3, 3));
            _square8Indexes.AddRange(_allIndexes.GetRange(9 * 8 + 3, 3));

            _square9Indexes.AddRange(_allIndexes.GetRange(9 * 6 + 6, 3));
            _square9Indexes.AddRange(_allIndexes.GetRange(9 * 7 + 6, 3));
            _square9Indexes.AddRange(_allIndexes.GetRange(9 * 8 + 6, 3));

            _allSquaresInOneList.Add(_square1Indexes);
            _allSquaresInOneList.Add(_square2Indexes);
            _allSquaresInOneList.Add(_square3Indexes);
            _allSquaresInOneList.Add(_square4Indexes);
            _allSquaresInOneList.Add(_square5Indexes);
            _allSquaresInOneList.Add(_square6Indexes);
            _allSquaresInOneList.Add(_square7Indexes);
            _allSquaresInOneList.Add(_square8Indexes);
            _allSquaresInOneList.Add(_square9Indexes);

        }

        private string GetSquareName(List<int[]> square)
        {
            string toReturn = "";

            if (square == _square1Indexes) toReturn = "Square1";
            else if (square == _square2Indexes) toReturn = "Square2";
            else if (square == _square3Indexes) toReturn = "Square3";
            else if (square == _square4Indexes) toReturn = "Square4";
            else if (square == _square5Indexes) toReturn = "Square5";
            else if (square == _square6Indexes) toReturn = "Square6";
            else if (square == _square7Indexes) toReturn = "Square7";
            else if (square == _square8Indexes) toReturn = "Square8";
            else if (square == _square9Indexes) toReturn = "Square9";

            return toReturn;
        }

        private List<int[]> GetSquareFromIndex(int[] i)
        {
            List<int[]> toReturn = new List<int[]>();

            foreach (List<int[]> i2 in _allSquaresInOneList)
            {
                if (i2.Exists(x => x.SequenceEqual(i)))
                {
                    toReturn = i2;
                    break;
                }
            }

            return toReturn;
        }

        private bool CanNumberBeHere(int[] location, int number)
        {
            bool toReturn = false;

            bool Column = ColumnContainsTheNumber(location[1], number);
            bool Row = RowContainsTheNumber(location[0], number);
            bool Square = SquareContainsTheNumber(GetSquareFromIndex(location), number);

            if (!Column && !Row && !Square)
            {
                //if neither column, row or square got the number
                if (GetNumberFromIndex(location) == 0) toReturn = true;
            }

            return toReturn;
        }

        private bool ColumnContainsTheNumber(int column, int number)
        {
            bool toReturn = false;

            for (int i = 0; i < 9; i++)
            {
                if (_grid[i, column] == number) return true;
            }

            return toReturn;
        }

        private bool RowContainsTheNumber(int row, int number)
        {
            bool toReturn = false;

            for (int i = 0; i < 9; i++)
            {
                if (_grid[row, i] == number) return true;
            }

            return toReturn;
        }

        private bool SquareContainsTheNumber(List<int[]> square, int number)
        {
            bool toReturn = false;
            foreach (int[] i in square)
            {
                if (GetNumberFromIndex(i) == number) toReturn = true;
            }
            return toReturn;
        }

        private List<List<int[]>> FindAffectedSquares(List<int[]> area)
        {
            List<List<int[]>> toReturn = new List<List<int[]>>();

            foreach (List<int[]> squares in _allSquaresInOneList)
            {
                foreach (int[] box in GetFreeSpotsInList(squares))
                {
                    if (area.Exists(x => x.SequenceEqual(box)))
                    {
                        toReturn.Add(squares);
                        break;
                    }
                }
            }

            return toReturn;
        }

        private List<int[]> GetRow(int row)
        {
            List<int[]> toReturn = new List<int[]>();

            toReturn = _allIndexes.FindAll(x => x[0] == row);

            return toReturn;
        }

        private List<int[]> GetColumn(int column)
        {
            List<int[]> toReturn = new List<int[]>();

            toReturn = _allIndexes.FindAll(x => x[1] == column);

            return toReturn;
        }

        private List<int[]> GetFreeSpotsInList(List<int[]> area)
        {
            List<int[]> toReturn = new List<int[]>();
            foreach (int[] i in area)
            {
                if (GetNumberFromIndex(i) == 0) toReturn.Add(i);
            }

            return toReturn;
        }

        public void IsItCorrect(bool info = false)
                {
                    Console.WriteLine("CHECKING THE RESULT");
                    bool toReturn = true;
                    int grandTotal = 0;

                    foreach (List<int[]> square in _allSquaresInOneList)
                    {
                        int total = 0;
                        foreach (int[] box in square)
                        {
                            total += GetNumberFromIndex(box);
                        }

                        if (info)
                        {
                            Console.WriteLine($"ERROR - {GetSquareName(square)} had a total of {total}");
                            if (total != 45) toReturn = false;
                        }
                        else if (total != 45) { toReturn = false; Console.WriteLine($"ERROR - {GetSquareName(square)} had a total of {total}"); }

                        grandTotal += total;
                        total = 0;
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        int total = 0;
                        foreach (int[] box in GetRow(i))
                        {
                            total += GetNumberFromIndex(box);
                        }

                        if (info)
                        {
                            Console.WriteLine($"ERROR - Row: {i}, had a total of {total}");
                            if (total != 45) toReturn = false;
                        }
                        else if (total != 45) { toReturn = false; Console.WriteLine($"ERROR - Row: {i}, had a total of {total}"); }

                        total = 0;

                        foreach (int[] box in GetColumn(i))
                        {
                            total += GetNumberFromIndex(box);
                        }

                        if (info)
                        {
                            Console.WriteLine($"ERROR - Column: {i}, had a total of {total}");
                            if (total != 45) toReturn = false;
                        }
                        else if (total != 45) { toReturn = false; Console.WriteLine($"ERROR - Column: {i}, had a total of {total}"); }

                        total = 0;
                    }

                    if (!toReturn)
                    {
                        Console.WriteLine("ITS NOT DONE");
                    }
                    else
                    {
                        Console.WriteLine("THE RESULT IS CORRECT");
                    }
                    Console.WriteLine($"{grandTotal} / 405");
                }

        private int GetTotal()
        {
            int toReturn = 0;
            foreach (int[] i in _allIndexes)
            {
                toReturn += GetNumberFromIndex(i);
            }

            return toReturn;
        }

        private bool Check()
        {
            if (GetTotal() == 405) IsSolved = true;
            else IsSolved = false;
            return IsSolved;
        }

        private void TakeBackup()
        {
            _backup = (int[,])_grid.Clone();
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        public void PrintTime()
        {
            TimeSpan ts = _timeFinished - _timeStarted;
            Console.WriteLine($"FINISHED IN: [{ts}]");
        }

        public void Print()
        {
            Console.WriteLine(" ");
            int j = 0;
            int l = 0;
            int k = 0;
            foreach (int[] index in _allIndexes)
            {
                int number = GetNumberFromIndex(index);
                if (j % 9 == 0 && j != 0) { Console.WriteLine(); k++; }
                if (l % 3 == 0 && l % 9 != 0) Console.Write("| ");
                if (k == 3) { Console.WriteLine("---------------------"); k = 0; }

                if (number == 0) Console.Write(" " + " ");
                else if (number == _originalCopy[index[0], index[1]])
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(number + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else Console.Write(number + " ");
                j++;
                l++;
            }

            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }



        #endregion

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        //New in this worker
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        private List<List<int[]>> _listOfAllCandidates;

        public WorkerV3(Difficulty difficulty)
        {
            _difficulty = difficulty;
            SetDifficulty();
            getAndsetAllIndexes();
        }

        public void Start()
        {
            Print();

            //collect information

            _timeStarted = DateTime.Now;

            Solve();
        }

        public void Solve()
        {
            ReloadAllCandidates();
            for (int i = 0; i < 10; i++)
            {
                RemoveCandidates();
                SolveSingleCandidates();
                SolveHiddenSingleCandidates();
                if (Check()) break;
            }
            Print();

            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.WriteLine($"---------------------------------------{GetFreeSpotsInList(_allIndexes).Count}------------------------------------");
            Console.WriteLine("-----------------------------------------------------------------------------");
            //SolveSingleCandidates();
            foreach (int[] box in _allIndexes)
            {
                //PrintCandidatesForIndex(box);
            }
            Console.WriteLine("-----------------------------------------------------------------------------");
            PrintCandidates();
            Print();
            PrintTotalCandidates();

            //PrintCandidatesInArea(_allSquaresInOneList[2], 7); //square 3
            //PrintCandidatesInArea(GetRow(2), 7); //row 3

            //RemoveCandidatesClaimingPair();
            //PrintCandidatesInArea(_allSquaresInOneList[2], 7); //square 3

            if (Check())
            {
                _timeFinished = DateTime.Now;
                PrintTime();
            }
            IsItCorrect();
        }

        private void SolveSingleCandidates()
        {
            foreach (int[] box in GetFreeSpotsInList(_allIndexes))
            {
                List<int> numbers = GetCandidatesForIndex(box);
                if (numbers.Count == 1) SetData(box, numbers.First());
            }
            RemoveCandidates();
            CheckAndUpdateCandidates();
        }

        private void SolveHiddenSingleCandidates()
        {
            bool startOver = false;
            for (int number = 1; number <= 10; number++)
            {
                //It runs the number, and if it places a number, it 
                if (startOver)
                {
                    startOver = false;
                    number = 1;
                }
                else if (number == 10) break; //This is only needed when it solves number 9's

                //
                // SQARES
                //
                foreach (List<int[]> square in _allSquaresInOneList)
                {
                    //if (startOver) break;
                    List<int[]> temp = CrossTwoLists(_listOfAllCandidates[number - 1], square);
                    //now we got all candidates for number in x-square
                    if (temp.Count == 1)
                    {
                        SetData(temp[0], number);
                        //ReloadAllCandidates();
                        startOver = true;
                    }
                }
                //
                // ROWS AND COLUMNS
                //
                for (int i = 0; i < 9; i++)
                {
                    //if (startOver) break;
                    List<int[]> temp = CrossTwoLists(_listOfAllCandidates[number - 1], GetRow(i));
                    if (temp.Count == 1)
                    {
                        SetData(temp[0], number); 
                        //ReloadAllCandidates();
                        startOver = true;
                    }
                    //if (startOver) break;
                    temp = CrossTwoLists(_listOfAllCandidates[number - 1], GetColumn(i));
                    if (temp.Count == 1)
                    {
                        SetData(temp[0], number);
                        //ReloadAllCandidates();
                        startOver = true;
                    }
                }
            }
            //ReloadAllCandidates();
            RemoveCandidates();
            CheckAndUpdateCandidates();
            SolveSingleCandidates();
        }

        private void RemoveCandidates()
        {
            
            //https://www.youtube.com/watch?v=FgA0dx6XPY4&ab_channel=RFC963

            //Explaining the methods
            //https://www.youtube.com/watch?v=b123EURtu3I&ab_channel=RFC963
            //PrintTotalCandidates();
            //CheckAndUpdateCandidates();
            //RemoveNewlyFilledCandidates();
            RemoveCandidatesPointingPair();
            RemoveCandidatesClaimingPair();
            RemoveCandidatesNakedTriple();
            //PrintTotalCandidates();
        }

        private void RemoveNewlyFilledCandidates()
        {
            List<int[]> toRemove = new List<int[]>();
            for (int i = 0; i < 9; i++)
            {
                foreach (int[] box in _listOfAllCandidates[i])
                {
                    if (GetNumberFromIndex(box) != 0) toRemove.Add(box);
                }

                foreach (int[] box in toRemove)
                {
                    _listOfAllCandidates[i].Remove(box);
                    Console.WriteLine($"HAHAHAHHA REMOVED CANDIDATE FOR {i+1} AT [{box[0]} , {box[1]}]");
                }
            }

        }
        private void RemoveCandidatesPointingPair() //squares DONE
        {
            for (int squareNumber = 0; squareNumber < 9; squareNumber++)
            {
                List<List<int[]>> candidatesInSquare = GetCandidatesInArea(_allSquaresInOneList[squareNumber]);
                //now we need to check if all of the given candidates for one number is on the same row or column
                for (int number = 0; number < 9; number++)
                {
                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    //                             ROWS
                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    int toRemove = 99;
                    foreach (int[] box in candidatesInSquare[number])
                    {
                        if (candidatesInSquare[number].Count < 2) break;

                        if (toRemove == 99) toRemove = box[0];
                        else if (box[0] == toRemove) continue;
                        else toRemove = 999;
                    }

                    if (toRemove < 9) //we know that all spots for this number is on the same row
                    {
                        List<int[]> needsToBeRemoved = new List<int[]>();
                        foreach (int[] box in _listOfAllCandidates[number])
                        {
                            //we need to make sure we are not removing the ones that gave us this information
                            if (box[0] == toRemove)
                            {
                                if (!candidatesInSquare[number].Exists(x => x.SequenceEqual(box))) //if box is not on the list that gave us this information
                                {
                                    needsToBeRemoved.Add(box);
                                }

                            }
                        }

                        foreach (int[] box in needsToBeRemoved)
                        {
                            _listOfAllCandidates[number].Remove(box);
                            Console.WriteLine($"REMOVED CANDIDATE FOR {number + 1} AT [{box[0]} , {box[1]}]");
                        }
                    }
                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    //                             COLUMNS
                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    toRemove = 99;
                    foreach (int[] box in candidatesInSquare[number])
                    {
                        if (candidatesInSquare[number].Count < 2) break;

                        if (toRemove == 99) toRemove = box[1];
                        else if (box[1] == toRemove) continue;
                        else toRemove = 999;
                    }

                    if (toRemove < 9) //we know that all spots for this number is on the same column
                    {
                        List<int[]> needsToBeRemoved = new List<int[]>();
                        foreach (int[] box in _listOfAllCandidates[number])
                        {
                            //we need to make sure we are not removing the ones that gave us this information
                            if (box[1] == toRemove)
                            {
                                if (!candidatesInSquare[number].Exists(x => x.SequenceEqual(box))) //if box is not on the list that gave us this information
                                {
                                    needsToBeRemoved.Add(box);
                                }
                            }
                        }

                        foreach (int[] box in needsToBeRemoved)
                        {
                            _listOfAllCandidates[number].Remove(box);
                            Console.WriteLine($"REMOVED CANDIDATE FOR {number + 1} AT [{box[0]} , {box[1]}]");
                        }
                    }
                    toRemove = 99;
                }
            }
        }

        private void RemoveCandidatesClaimingPair() //rows and columns NOT DONE
        {
            for (int rowAndColumnNumber = 0; rowAndColumnNumber < 9; rowAndColumnNumber++) //0 to 9
            {
                //now we need to check if all of the given candidates for one number is in the same square
                for (int number = 0; number < 9; number++) //0 to 9
                {
                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    //                             ROWS
                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    List<List<int[]>> candidatesInArea = GetCandidatesInArea(GetRow(rowAndColumnNumber));

                    List<int[]> toRemove = new List<int[]>() { new[] { 0, 0 }}; //1
                    //this is now going to show the square that got all the candidates
                    foreach (int[] box in candidatesInArea[number])
                    {
                        if (candidatesInArea[number].Count < 2) break;
                        
                        if (toRemove.Count == 1) toRemove = GetSquareFromIndex(box);
                        else if (GetSquareFromIndex(box) == toRemove) continue;
                        else toRemove = new List<int[]>(){new []{0,0}, new []{0,0}}; //2
                    }

                    if (toRemove.Count == 9) //we know that all spots for this number is in the same square
                    {
                        List<int[]> needsToBeRemoved = new List<int[]>();
                        foreach (int[] box in _listOfAllCandidates[number])
                        {
                            //we need to make sure we are not removing the ones that gave us this information
                            if (GetSquareFromIndex(box) == toRemove && !candidatesInArea[number].Exists(x=>x.SequenceEqual(box)))
                            {
                                needsToBeRemoved.Add(box);
                            }
                        }

                        foreach (int[] box in needsToBeRemoved)
                        {
                            _listOfAllCandidates[number].Remove(box);
                            Console.WriteLine($"REMOVED CANDIDATE FOR {number + 1} AT [{box[0]} , {box[1]}]");
                        }
                    }

                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    //                             COLUMNS
                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    candidatesInArea = GetCandidatesInArea(GetColumn(rowAndColumnNumber));

                    toRemove = new List<int[]>() { new[] { 0, 0 } }; //1
                    //this is now going to show the square that got all the candidates
                    foreach (int[] box in candidatesInArea[number])
                    {
                        if (candidatesInArea[number].Count < 2) break;

                        if (toRemove.Count == 1) toRemove = GetSquareFromIndex(box);
                        else if (GetSquareFromIndex(box) == toRemove) continue;
                        else toRemove = new List<int[]>() { new[] { 0, 0 }, new[] { 0, 0 } }; //2
                    }

                    if (toRemove.Count == 9) //we know that all spots for this number is in the same square
                    {
                        List<int[]> needsToBeRemoved = new List<int[]>();
                        foreach (int[] box in _listOfAllCandidates[number])
                        {
                            //we need to make sure we are not removing the ones that gave us this information
                            //we need to make sure that the candidates are also in the square we need to remove from
                            if (GetSquareFromIndex(box) == toRemove && !candidatesInArea[number].Exists(x => x.SequenceEqual(box)))
                            {
                                needsToBeRemoved.Add(box);
                            }
                        }

                        foreach (int[] box in needsToBeRemoved)
                        {
                            _listOfAllCandidates[number].Remove(box);
                            Console.WriteLine($"REMOVED CANDIDATE FOR {number + 1} AT [{box[0]} , {box[1]}]");
                        }
                    }

                    toRemove = new List<int[]>() { new[] { 0, 0 } }; //1
                }
            }
        }

        private void RemoveCandidatesNakedTriple() //advanced shit NOT DONE
        {
            for (int squareRowColumnNumber = 0; squareRowColumnNumber < 9; squareRowColumnNumber++)
            {
                //square
                List<int[]> locations = GetFreeSpotsInList(_allSquaresInOneList[squareRowColumnNumber]);
                foreach (int[] box in locations)
                {
                    List<int[]> nakedTriples = new List<int[]>(){box};
                    List<int> numbersToWorkWith = GetCandidatesForIndex(box);
                    if (numbersToWorkWith.Count > 3) continue; //so its 2 or 3
                    else if (numbersToWorkWith.Count == 1)
                    {
                        SetData(box, numbersToWorkWith.First());
                        continue;
                    }
                    else if (numbersToWorkWith.Count == 3)
                    {
                        foreach (int[] box2 in locations)
                        {
                            if (box2.SequenceEqual(box)) continue; //we don't want to check the one that got us here

                            List<int> candidates = GetCandidatesForIndex(box2);

                            if (candidates.Count > 3) continue; //so this also got 2 or 3 potential numbers

                            bool toAdd = true;
                            foreach (int number in candidates)
                            {
                                if (!numbersToWorkWith.Contains(number)) toAdd = false;
                            }
                            if (toAdd) nakedTriples.Add(box2);
                        }
                        //If there is a naked triple, we can remove all other candidates in that square
                        if (nakedTriples.Count == 3)
                        {
                            //Console.WriteLine("--------------------------------------------------------------");
                            //Console.WriteLine($"NUMBERS: {numbersToWorkWith[0]} , {numbersToWorkWith[1]} , {numbersToWorkWith[2]}");
                            //Console.WriteLine($"IN: [{nakedTriples[0][0]} , {nakedTriples[0][1]}] , [{nakedTriples[1][0]} , {nakedTriples[1][1]}] , [{nakedTriples[2][0]} , {nakedTriples[2][1]}]");
                            foreach (int[] box3 in locations)
                            {
                                if (nakedTriples.Exists(x=>x.SequenceEqual(box3))) continue;
                                foreach (int i in numbersToWorkWith)
                                {
                                    if (GetCandidatesForIndex(box3).Contains(i))
                                    {
                                        //if the box3 is not included in the naked triple, but it contains some of the numbers, we want to remove these numbers from potential candidates
                                        _listOfAllCandidates[i - 1].Remove(box3);
                                        Console.WriteLine($"\t\t\tREMOVED CANDIDATE FOR {i} AT [{box3[0]} , {box3[1]}]");
                                    }
                                }
                            }
                            foreach (int[] hej in locations)
                            {
                                //PrintCandidatesForIndex(hej);
                            }
                            //Console.WriteLine("--------------------------------------------------------------");
                            nakedTriples = new List<int[]>() { box };
                        }
                    }
                    else if (numbersToWorkWith.Count == 2)
                    {
                        //continue;
                        //We are guessing on the last number to work with, and if we find a naked triple, we break
                        nakedTriples = new List<int[]>() { box };
                        int numberAdded = 0;

                        foreach (int[] box2 in locations)
                        {
                            if (box2.SequenceEqual(box)) continue; //we don't want to check the one that got us here

                            List<int> candidates = GetCandidatesForIndex(box2);

                            if (candidates.Count > 3) continue; //so this also got 2 or 3 potential numbers
                            if (candidates.Count == 2)
                            {
                                bool toAdd = true;
                                foreach (int number in candidates)
                                {
                                    if (!numbersToWorkWith.Contains(number)) toAdd = false;
                                }
                                if (toAdd) nakedTriples.Add(box2);
                            }
                            else if (candidates.Count == 3)
                            {
                                //HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE
                                //we are in the next box, and we want to make sure that 2 of the 3 candidates matches the 2 numbers to work with
                                int numbersMatches = 0;
                                foreach (int number in candidates)
                                {
                                    if (numbersToWorkWith.Contains(number)) numbersMatches++;
                                }

                                if (numbersMatches == 2)
                                {
                                    List<int> candidatesInThisBox = GetCandidatesForIndex(box2);
                                    for (int i = 0; i < 3; i++)
                                    {
                                        if (!numbersToWorkWith.Contains(candidatesInThisBox[i]))
                                        {
                                            numbersToWorkWith.Add(candidatesInThisBox[i]);
                                            numberAdded = candidatesInThisBox[i];
                                            break;
                                        }
                                    }
                                    //now we will run though all the other boxes and check if they match, and if not, remove the number we added
                                    //and take the next box from previous and continue
                                    if (numbersToWorkWith.Count == 3)
                                    {
                                        foreach (int[] box4 in locations)
                                        {
                                            if (box4.SequenceEqual(box)) continue; //we don't want to check the one that got us here
                                            if (box4.SequenceEqual(box2)) continue; //we don't want to check the one that got us here

                                            List<int> candidates2 = GetCandidatesForIndex(box4);

                                            if (candidates2.Count > 3) continue; //so this also got 2 or 3 potential numbers

                                            bool toAdd = true;
                                            foreach (int number in candidates2)
                                            {
                                                if (!numbersToWorkWith.Contains(number)) toAdd = false;
                                            }
                                            if (toAdd) nakedTriples.Add(box4);
                                        }
                                        if (nakedTriples.Count == 3)
                                        {
                                            //Console.WriteLine("--------------------------------------------------------------");
                                            //Console.WriteLine($"NUMBERS: {numbersToWorkWith[0]} , {numbersToWorkWith[1]} , {numbersToWorkWith[2]}");
                                            //Console.WriteLine($"IN: [{nakedTriples[0][0]} , {nakedTriples[0][1]}] , [{nakedTriples[1][0]} , {nakedTriples[1][1]}] , [{nakedTriples[2][0]} , {nakedTriples[2][1]}]");
                                            foreach (int[] box3 in locations)
                                            {
                                                if (nakedTriples.Exists(x => x.SequenceEqual(box3))) continue;
                                                foreach (int i in numbersToWorkWith)
                                                {
                                                    if (GetCandidatesForIndex(box3).Contains(i))
                                                    {
                                                        //if the box3 is not included in the naked triple, but it contains some of the numbers, we want to remove these numbers from potential candidates
                                                        _listOfAllCandidates[i - 1].Remove(box3);
                                                        Console.WriteLine($"\t\t\t\t\tREMOVED CANDIDATE FOR {i} AT [{box3[0]} , {box3[1]}]");
                                                    }
                                                }
                                            }
                                            foreach (int[] hej in locations)
                                            {
                                                //PrintCandidatesForIndex(hej);
                                            }
                                            //Console.WriteLine("--------------------------------------------------------------");
                                            nakedTriples = new List<int[]>() { box };
                                            numberAdded = 0;
                                        }
                                    }
                                }

                            }
                            
                        }

                        continue;
                        #region OLD
                        for (int guessedNumber = 1; guessedNumber < 9; guessedNumber++)
                        {
                            numbersToWorkWith.Add(guessedNumber);
                            foreach (int[] box2 in locations)
                            {
                                if (box2.SequenceEqual(box)) continue; //we don't want to check the one that got us here

                                List<int> candidates = GetCandidatesForIndex(box2);

                                if (candidates.Count > 3) continue; //so this also got 2 or 3 potential numbers

                                bool toAdd = true;
                                foreach (int number in candidates)
                                {
                                    if (!numbersToWorkWith.Contains(number)) toAdd = false;
                                }

                                if (toAdd)
                                {
                                    nakedTriples.Add(box2);
                                }
                            }
                            //If there is a naked triple, we can remove all other candidates in that square
                            if (nakedTriples.Count == 3)
                            {
                                Console.WriteLine("--------------------------------------------------------------");
                                foreach (int[] hej in locations)
                                {
                                    PrintCandidatesForIndex(hej);
                                }
                                Console.WriteLine($"NUMBERS: {numbersToWorkWith[0]} , {numbersToWorkWith[1]} , {numbersToWorkWith[2]}");
                                Console.WriteLine($"IN: [{nakedTriples[0][0]} , {nakedTriples[0][1]}] , [{nakedTriples[1][0]} , {nakedTriples[1][1]}] , [{nakedTriples[2][0]} , {nakedTriples[2][1]}]");
                                foreach (int[] box3 in locations)
                                {
                                    if (nakedTriples.Exists(x => x.SequenceEqual(box3))) continue;
                                    foreach (int i in numbersToWorkWith)
                                    {
                                        if (GetCandidatesForIndex(box3).Contains(i))
                                        {
                                            //if the box3 is not included in the naked triple, but it contains some of the numbers, we want to remove these numbers from potential candidates
                                            _listOfAllCandidates[i - 1].Remove(box3);
                                            Console.WriteLine($"\t\t\tREMOVED CANDIDATE FOR {i} AT [{box3[0]} , {box3[1]}]");
                                        }
                                    }
                                }
                                foreach (int[] hej in locations)
                                {
                                    PrintCandidatesForIndex(hej);
                                }
                                Console.WriteLine("--------------------------------------------------------------");
                                break;
                            }
                            nakedTriples = new List<int[]>() { box };
                        } 
                        #endregion
                    }

                }
            }
        }

        private void RemoveCandidatesHiddenPair() //advanced shit NOT DONE
        {

        }




        private List<int[]> CrossTwoLists(List<int[]> listToReturn, List<int[]> listToCheck)
        {
            List<int[]> toReturn = new List<int[]>();

            foreach (int[] box in listToReturn)
            {
                if (listToCheck.Exists(x=>x.SequenceEqual(box))) toReturn.Add(box);
            }

            return toReturn;
        }

        public void ReloadAllCandidates()
        {
            _listOfAllCandidates = new List<List<int[]>>()
            {
                new List<int[]>(),
                new List<int[]>(),
                new List<int[]>(),
                new List<int[]>(),
                new List<int[]>(),
                new List<int[]>(),
                new List<int[]>(),
                new List<int[]>(),
                new List<int[]>()
            };
            for (int i = 1; i <= 9; i++)
            {
                foreach (int[] box in _allIndexes)
                {
                    if (CanNumberBeHere(box, i))
                    {
                        _listOfAllCandidates[i-1].Add(box);
                    }
                }
            }
            RemoveCandidates();
        }

        private void CheckAndUpdateCandidates(int number = 0)
        {
            List<int[]> toRemove = new List<int[]>();
            if (number == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    foreach (int[] box in _listOfAllCandidates[i])
                    {
                        List<int> hej = GetCandidatesForIndex(box);
                        if (!CanNumberBeHere(box, i + 1) && !toRemove.Exists(x=>x.SequenceEqual(box)) && hej.Contains(i+1))
                        {
                            toRemove.Add(box);
                        }
                    }

                    foreach (int[] box in toRemove)
                    {
                        _listOfAllCandidates[i].Remove(box);
                        Console.WriteLine($"HAHAHAHHA REMOVED CANDIDATE FOR {i + 1} AT [{box[0]} , {box[1]}]");
                    }
                    toRemove = new List<int[]>();
                }
            }
            else
            {
                foreach (int[] box in _listOfAllCandidates[number-1])
                {
                    if (!CanNumberBeHere(box, number)) toRemove.Add(box);
                }

                foreach (int[] box in toRemove)
                {
                    _listOfAllCandidates[number-1].Remove(box);
                }
            }
        }

        public List<List<int[]>> GetCandidatesInArea(List<int[]> area)
        {
            //PrintCandidatesInArea(area);
            List<List<int[]>> toReturn = new List<List<int[]>>();

            for (int i = 0; i < 9; i++)
            {
                List<int[]> candidates = CrossTwoLists(area, _listOfAllCandidates[i]);
                toReturn.Add(candidates);
            }

            return toReturn;
        }

        public List<int> GetCandidatesForIndex(int[] index)
        {
            List<int> toReturn = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                //List<int[]> candidates = CrossTwoLists(new List<int[]>() { index }, _listOfAllCandidates[i]);

                if (_listOfAllCandidates[i].Exists(x=>x.SequenceEqual(index))) toReturn.Add(i+1);
            }

            return toReturn;
        }



        private void SetData(int[] location, int number)
        {
            if (_grid[location[0], location[1]] != 0) throw new Exception("Tried to place number on a filled spot");
            if (GetFreeSpotsInList(GetSquareFromIndex(location)).Count == 1)
            {
                List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                foreach (int i in GetNumbersInArea(GetSquareFromIndex(location)))
                {
                    if (numbers.Contains(i)) numbers.Remove(i);
                }

                //Console.WriteLine(GetFreeSpotsInList(GetSquareFromIndex(location)).Count);
                //Console.WriteLine(GetFreeSpotsInList(GetSquareFromIndex(location)).Count);
                List<int[]> test = GetFreeSpotsInList(GetSquareFromIndex(location));
                int[] fucked = GetFreeSpotsInList(GetSquareFromIndex(location))[0];
                if (!CanNumberBeHere(fucked, numbers[0]))
                {
                    //Print();
                    //Console.WriteLine($"WE ARE FUCKED AT [{fucked[0]} , {fucked[1]}] with {numbers[0]}");
                }
            }

            //PrintCandidatesForIndex(location);
            _grid[location[0], location[1]] = number;
            Console.WriteLine($"Number {number} was placed at[{location[0]} , {location[1]}]");
            ReloadAllCandidates();
            CheckAndUpdateCandidates();
            //PrintCandidatesForIndex(location);
            RemoveCandidates();
            //PrintCandidatesForIndex(location);
        }

        private void PrintCandidates(int number = 0)
        {
            if (number == 0)
            {
                for (int i = 0; i < _listOfAllCandidates.Count; i++)
                {
                    int n = i + 1;
                    Console.Write($"CANDIDATES FOR {n}: ");
                    foreach (int[] box in _listOfAllCandidates[i])
                    {
                        Console.Write($"[{box[0]} , {box[1]}]");
                    }
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                }
            }
            else if (number >= 1 && number <= 9)
            {
                Console.WriteLine($"CANDIDATES FOR {number}: ");
                foreach (int[] box in _listOfAllCandidates[number-1])
                {
                    Console.Write($"[{box[0]} , {box[1]}]");
                }
                Console.WriteLine(" ");
                Console.WriteLine(" ");
            }
            else
            {
                Console.WriteLine($"COULD NOT PRINT CANDIDATES FOR {number} SINCE ITS NOT BETWEEN 1 AND 9");
            }

            Console.WriteLine(" ");
        }

        private void PrintCandidatesInArea(List<int[]> area, int number = 0)
        {
            //ReloadAllCandidates();
            Console.WriteLine("");
            Console.WriteLine($"IN THE AREA: [{area.First()[0]} , {area.First()[1]}] to [{area.Last()[0]} , {area.Last()[1]}]");
            Console.WriteLine("-------------------------------");
            if (number == 0)
            {
                for (int i = 0; i < 9; i++)
                {


                    List<int[]> candidates = CrossTwoLists(area, _listOfAllCandidates[i]);
                    if (candidates.Count == 0) continue;
                    int n = i + 1;
                    Console.Write($"CANDIDATES FOR {n}: ");
                    foreach (int[] box in candidates)
                    {
                        Console.Write($"[{box[0]} , {box[1]}] ");
                    }
                    Console.WriteLine(" ");
                }
            }
            else if (number >= 1 && number <= 9)
            {
                List<int[]> candidates = CrossTwoLists(area, _listOfAllCandidates[number - 1]);
                Console.Write($"CANDIDATES FOR {number}: ");
                foreach (int[] box in candidates)
                {
                    Console.Write($"[{box[0]} , {box[1]}]");
                }
                Console.WriteLine(" ");
            }
            else
            {
                Console.WriteLine($"COULD NOT PRINT CANDIDATES FOR {number} SINCE ITS NOT BETWEEN 1 AND 9");
            }
        }

        public void PrintCandidatesForIndex(int[] index, bool showEmpties = false)
        {
            //Console.WriteLine("");
            //Console.Write($"IN THE BOX: [{index[0]} , {index[1]}]: ");
            bool none = true;
            for (int i = 0; i < 9; i++)
            {
                List<int[]> candidates = CrossTwoLists(new List<int[]>(){index}, _listOfAllCandidates[i]);
                if (candidates.Count == 0) continue;
                else if (none)
                {
                    Console.Write($"IN THE BOX: [{index[0]} , {index[1]}]: ");
                    none = false;
                }
                Console.Write($"{i+1}, ");
            }

            if (none && showEmpties == true)
            {
                Console.Write($"IN THE BOX: [{index[0]} , {index[1]}]: ");
                Console.Write("NONE");
                //Console.Write("\n");
            }
            if (none == false)Console.Write("\n");
            
        }

        private void PrintArea(List<int[]> area)
        {
            List<int[]> spotsToPrint = CrossTwoLists(area, _allIndexes);
            Console.WriteLine(" ");
            int j = 0;
            int l = 0;
            int k = 0;
            foreach (int[] index in _allIndexes)
            {
                int number = GetNumberFromIndex(index);
                if (j % 9 == 0 && j != 0) { Console.WriteLine(); k++; }
                if (l % 3 == 0 && l % 9 != 0) Console.Write("| ");
                if (k == 3) { Console.WriteLine("---------------------"); k = 0; }

                if (number == 0) Console.Write(" " + " ");
                else if (number == _originalCopy[index[0], index[1]] && spotsToPrint.Exists(x=>x.SequenceEqual(index)))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(number + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (spotsToPrint.Exists(x => x.SequenceEqual(index))) Console.Write(number + " ");
                else Console.Write(" " + " ");
                j++;
                l++;
            }

            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }

        private void PrintTotalCandidates(int number = 0)
        {
            int total = 0;
            if (number == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    total += _listOfAllCandidates[i].Count;
                }
            }
            else
            {
                total += _listOfAllCandidates[number - 1].Count;
            }
            Console.WriteLine($"TOTAL CANDIDATES: [{total}]");
        }

    }
}
