using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SodukoSolver
{
    public class WorkerV2
    {

        private int[,] _grid;
        private int[,] _backup;


        public enum Difficulty
        {
            Clear, Easy, Medium, Hard, VeryHard, Expert
        }

        private Difficulty _difficulty;
        private bool _isReadyToBeSolved = false;

        private List<int[]> _allIndexes = new List<int[]>();
        private List<List<int[]>> _allSqaresInOneList = new List<List<int[]>>();

        private List<int[]> _sqare1Indexes = new List<int[]>();
        private List<int[]> _sqare2Indexes = new List<int[]>();
        private List<int[]> _sqare3Indexes = new List<int[]>();
        private List<int[]> _sqare4Indexes = new List<int[]>();
        private List<int[]> _sqare5Indexes = new List<int[]>();
        private List<int[]> _sqare6Indexes = new List<int[]>();
        private List<int[]> _sqare7Indexes = new List<int[]>();
        private List<int[]> _sqare8Indexes = new List<int[]>();
        private List<int[]> _sqare9Indexes = new List<int[]>();
        private bool _isSolved;


        public bool IsSolved
        {
            get { return _isSolved; }
            set { _isSolved = value; }
        }
        public WorkerV2(Difficulty difficulty)
        {
            _difficulty = difficulty;
            SetDifficulty();
            //_grid = _solution_hard;
            getAndsetAllIndexes();
        }

        public void Start()
        {
            Print();

            CollectInformation();

            Solve();
        }

        public void Solve()
        {
            SolveWithMath();

            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine($"----------------------------AFTER PURE MATH.-----------------------");
            Console.WriteLine("-------------------------------------------------------------------");

            Print();

            Check();

            TakeBackup();

            for (int i = 1; i <= 9; i++)
            {
                if (!IsSolved) GuessOnNumber(i);
                else break;
                Check();
            }

            if (!IsSolved)
            {
                GuessOnNumberAdvanced();
            }

            Print();

            IsItCorrect();
        }

        private bool SolveARow(int row)
        {

            List<int[]> freeSpots = GetFreeSpotsInList(GetRow(row));
            //if (freeSpots.Count == 0) return true;
            //Console.WriteLine($"Working on row: {row}");
            for (int i = 1; i <= 9; i++)
            {
                if (PlaceNumberInArea(GetRow(row), i, new List<int[]>()))
                {
                    i = 1;
                    return true;
                }
            }

            return false;
        }

        private bool SolveAColumn(int column)
        {

            List<int[]> freeSpots = GetFreeSpotsInList(GetColumn(column));
            //if (freeSpots.Count == 0) return true;
            //Console.WriteLine($"Working on column: {column}");
            for (int i = 1; i <= 9; i++)
            {
                if (PlaceNumberInArea(GetColumn(column), i, new List<int[]>()))
                {
                    i = 1;
                    return true;
                }
            }

            return false;
        }

        private bool SolveASqare(List<int[]> sqare)
        {
            //if (GetFreeSpotsInList(sqare).Count == 0 && GetSqareName(sqare) != "sqare9") return true;
            //Console.WriteLine($"Working on: {GetSqareName(sqare)}");
            for (int i = 1; i <= 9; i++)
            {
                if (PlaceNumberInArea(sqare, i))
                {
                    i = 1;
                    return true;
                }
            }

            return false;
        }

        public void SolveWithMath()
        {
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (SolveASqare(_allSqaresInOneList[i])) i = 0;
                    if (SolveARow(i)) i = 0;
                    if (SolveAColumn(i)) i = 0;
                }
                if (GetFreeSpotsInList(_allIndexes).Count == 0) break;
            }
        }
        /// <summary>
        /// Takes a list of locations and tries to place the number
        /// </summary>
        /// <param name="area">this would be a sqare, a row, or a column</param>
        private bool PlaceNumberInArea(List<int[]> area, int number, List<int[]> cantGoHere = null)
        {
            bool toReturn = false;

            //int NumberOfSuitableSpots = 0;
            if (!GetNumbersInArea(area).Contains(number))
            {
                List<int[]> SuitableSpots = new List<int[]>();
                int[] finalDestination = new[] { 99, 99 };
                List<int[]> freeSpots = GetFreeSpotsInList(area);
                if (freeSpots.Count == 1)
                {
                    List<int> numbers = new List<int>{1,2,3,4,5,6,7,8,9};
                    List<int> exestingNumbers = GetNumbersInArea(area);
                    foreach (int i in exestingNumbers)
                    {
                        numbers.Remove(i);
                    }
                    //Console.WriteLine("HAHAHAHAHAHA");
                    if (CanNumberBeHere(freeSpots[0], numbers[0])) SetData(freeSpots[0], numbers[0]);
                    //else Console.WriteLine("WE ARE FUCKED");
                    //Print();
                }
                else
                {
                    foreach (int[] box in freeSpots)
                    {
                        if (CanNumberBeHere(box, number) && (cantGoHere == null || !cantGoHere.Exists(x => x.SequenceEqual(box))))
                        {
                            SuitableSpots.Add(box);
                            finalDestination = box;
                        }
                    }
                    if (SuitableSpots.Count == 1)
                    {
                        SetData(finalDestination, number);
                        toReturn = true;
                    }
                    else if (SuitableSpots.Count > 1 && SuitableSpots.Count <= 3 && cantGoHere == null)
                    {
                        #region old
                        //for (int i = 0; i < 2; i++)
                        //{
                        //    int numberIsPressentHere = 99;
                        //    foreach (int[] box in SuitableSpots)
                        //    {
                        //        if (numberIsPressentHere == 99) numberIsPressentHere = box[i];
                        //        else if (numberIsPressentHere != box[i]) numberIsPressentHere = 99;
                        //    }

                        //    if (numberIsPressentHere != 99)
                        //    {
                        //        //then we now that the number is pressent on that row
                        //        List<int[]> spotsToIgnore = new List<int[]>();
                        //        if (i == 0) spotsToIgnore = GetRow(numberIsPressentHere);
                        //        else if (i == 1) GetColumn(numberIsPressentHere);

                        //        List<List<int[]>> otherSqaresToCheck = FindAffectedSqares(spotsToIgnore);

                        //        foreach (List<int[]> sqare in otherSqaresToCheck)
                        //        {
                        //            PlaceNumberInArea(sqare, number, spotsToIgnore);
                        //            Console.WriteLine("ADVANCED");
                        //            i += 5;
                        //            break;
                        //        }
                        //    }
                        //} 
                        #endregion
                        List<int[]> spotsToIgnore = new List<int[]>();

                        if (SuitableSpots.Count == 2)
                        {
                            List<int[]> s = SuitableSpots;
                            if (s[0][0] == s[1][0]) spotsToIgnore = GetRow(s[0][0]);
                            else if(s[0][1] == s[1][1]) spotsToIgnore = GetColumn(s[0][1]);
                        }
                        else if (SuitableSpots.Count == 3)
                        {
                            List<int[]> s = SuitableSpots;
                            if (s[0][0] == s[1][0] && s[0][0] == s[2][0]) spotsToIgnore = GetRow(s[0][0]);
                            else if (s[0][1] == s[1][1] && s[0][1] == s[2][1]) spotsToIgnore = GetColumn(s[0][1]);
                        }

                        List<List<int[]>> otherSqaresToCheck = FindAffectedSqares(spotsToIgnore);
                        otherSqaresToCheck.Remove(area);

                        foreach (List<int[]> sqare in otherSqaresToCheck)
                        {
                            bool test = PlaceNumberInArea(sqare, number, spotsToIgnore);
                            //Console.WriteLine("ADVANCED");
                            break;
                        }

                    }
                    finalDestination = new []{99,99};
                }
            }

            return toReturn;
        }

        private void GuessOnNumber(int number)
        {
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine($"----------------------------WILD-GUESSING-[{number}]----------------------");
            Console.WriteLine("-------------------------------------------------------------------");

            List<int[]> freeSpots = GetFreeSpotsInList(_allIndexes);
            List<int[]> acceptedSpots = new List<int[]>();


            foreach (int[] free in freeSpots)
            {
                if (CanNumberBeHere(free, number)) acceptedSpots.Add(free);
            }
            for (int i = 0; i < acceptedSpots.Count; i++)
            {
                int[] spot = acceptedSpots[i];
                Console.WriteLine($"TRYING TO PLACE {number} ON: [{spot[0]} , {spot[1]}]");
                SetData(spot, number);
                SolveWithMath();
                //Print();
                if (GetTotal() != 405)
                {
                    _grid = (int[,])_backup.Clone();
                    Console.WriteLine("FUCK GO BACK");
                    //Print();
                    continue;
                }
                else
                {
                    Console.WriteLine("IT WORKED, IT IS COMPLETE");
                    break;
                }
            }
            if (GetTotal() != 405)
            {
                _grid = (int[,])_backup.Clone();
            }
        }

        private void GuessOnNumberAdvanced()
        {
            //veryhard= start from 7
            for (int number = 1; number <= 9; number++)
            {
                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine($"----------------------WILD-GUESSING-ADVANCED----------------------");
                Console.WriteLine("-------------------------------------------------------------------");

                List<int[]> freeSpots = GetFreeSpotsInList(_allIndexes);
                List<int[]> acceptedSpots = new List<int[]>();


                foreach (int[] free in freeSpots)
                {
                    if (CanNumberBeHere(free, number)) acceptedSpots.Add(free);
                }

                //Console.WriteLine($"THE NUMBER {number} CAN BE {acceptedSpots.Count} PLACES");
                //continue;

                for (int i = 0; i < acceptedSpots.Count; i++)
                {
                    int[] spot = acceptedSpots[i];
                    Console.WriteLine($"TRYING TO PLACE {number} ON: [{spot[0]} , {spot[1]}] - [ADVANCED]");
                    SetData(spot, number);
                    List<int> numbers = new List<int>{1,2,3,4,5,6,7,8,9};
                    numbers.Remove(number);
                    for (int j = 0; j < numbers.Count; j++)
                    {
                        if (!IsSolved) GuessOnNumber(numbers[j]);
                        else break;
                        Check();
                    }
                    if (IsSolved) break;

                    SolveWithMath();

                    Check();
                    //Print();
                    if (!IsSolved)
                    {
                        _grid = (int[,])_backup.Clone();
                        Console.WriteLine("FUCK GO BACK");
                        //Print();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("IT WORKED, IT IS COMPLETE");
                        break;
                    }
                }
                if (GetTotal() != 405)
                {
                    _grid = (int[,])_backup.Clone();
                }
                Check();
                if (IsSolved) break;
            }
        }

        private List<List<int[]>> FindAffectedSqares(List<int[]> area)
        {
            List<List<int[]>> toReturn = new List<List<int[]>>();

            foreach (List<int[]> sqares in _allSqaresInOneList)
            {
                foreach (int[] box in GetFreeSpotsInList(sqares))
                {
                    if (area.Exists(x => x.SequenceEqual(box)))
                    {
                        toReturn.Add(sqares);
                        break;
                    }
                }
            }

            return toReturn;
        }

        private bool CanNumberBeHere(int[] location, int number)
        {
            bool toReturn = false;

            bool Column = ColumnContainsTheNumber(location[1], number);
            bool Row = RowContainsTheNumber(location[0], number);
            bool Sqare = SqareContainsTheNumber(GetSqareFromIndex(location), number);

            if (!Column && !Row && !Sqare)
            {
                //if neither column, row or sqare got the number
                if (GetNumberFromIndex(location) == 0) toReturn = true;
            }

            return toReturn;
        }

        private void getAndsetAllIndexes()
        {
            int j1 = 0;
            for (int i = 0; i < _grid.Length; i++)
            {
                if (j1 == 9) j1 = 0;

                if (i < 9*1) {_allIndexes.Add(new []{0,j1}); j1++;}
                else if (i < 9*2) { _allIndexes.Add(new []{1, j1}); j1++;}
                else if (i < 9*3) { _allIndexes.Add(new []{2, j1}); j1++;}
                else if (i < 9*4) { _allIndexes.Add(new []{3, j1}); j1++;}
                else if (i < 9*5) { _allIndexes.Add(new []{4, j1}); j1++;}
                else if (i < 9*6) { _allIndexes.Add(new []{5, j1}); j1++;}
                else if (i < 9*7) { _allIndexes.Add(new []{6, j1}); j1++;}
                else if (i < 9*8) { _allIndexes.Add(new []{7, j1}); j1++;}
                else if (i < 9*9) { _allIndexes.Add(new []{8, j1}); j1++;}
            }

            _sqare1Indexes.AddRange(_allIndexes.GetRange(9*0+0,3));
            _sqare1Indexes.AddRange(_allIndexes.GetRange(9*1+0,3));
            _sqare1Indexes.AddRange(_allIndexes.GetRange(9*2+0,3));

            _sqare2Indexes.AddRange(_allIndexes.GetRange(9*0+3, 3));
            _sqare2Indexes.AddRange(_allIndexes.GetRange(9*1+3, 3));
            _sqare2Indexes.AddRange(_allIndexes.GetRange(9*2+3, 3));

            _sqare3Indexes.AddRange(_allIndexes.GetRange(9 * 0 + 6, 3));
            _sqare3Indexes.AddRange(_allIndexes.GetRange(9 * 1 + 6, 3));
            _sqare3Indexes.AddRange(_allIndexes.GetRange(9 * 2 + 6, 3));

            _sqare4Indexes.AddRange(_allIndexes.GetRange(9 * 3 + 0, 3));
            _sqare4Indexes.AddRange(_allIndexes.GetRange(9 * 4 + 0, 3));
            _sqare4Indexes.AddRange(_allIndexes.GetRange(9 * 5 + 0, 3));

            _sqare5Indexes.AddRange(_allIndexes.GetRange(9 * 3 + 3, 3));
            _sqare5Indexes.AddRange(_allIndexes.GetRange(9 * 4 + 3, 3));
            _sqare5Indexes.AddRange(_allIndexes.GetRange(9 * 5 + 3, 3));

            _sqare6Indexes.AddRange(_allIndexes.GetRange(9 * 3 + 6, 3));
            _sqare6Indexes.AddRange(_allIndexes.GetRange(9 * 4 + 6, 3));
            _sqare6Indexes.AddRange(_allIndexes.GetRange(9 * 5 + 6, 3));

            _sqare7Indexes.AddRange(_allIndexes.GetRange(9 * 6 + 0, 3));
            _sqare7Indexes.AddRange(_allIndexes.GetRange(9 * 7 + 0, 3));
            _sqare7Indexes.AddRange(_allIndexes.GetRange(9 * 8 + 0, 3));

            _sqare8Indexes.AddRange(_allIndexes.GetRange(9 * 6 + 3, 3));
            _sqare8Indexes.AddRange(_allIndexes.GetRange(9 * 7 + 3, 3));
            _sqare8Indexes.AddRange(_allIndexes.GetRange(9 * 8 + 3, 3));

            _sqare9Indexes.AddRange(_allIndexes.GetRange(9 * 6 + 6, 3));
            _sqare9Indexes.AddRange(_allIndexes.GetRange(9 * 7 + 6, 3));
            _sqare9Indexes.AddRange(_allIndexes.GetRange(9 * 8 + 6, 3));

            _allSqaresInOneList.Add(_sqare1Indexes);
            _allSqaresInOneList.Add(_sqare2Indexes);
            _allSqaresInOneList.Add(_sqare3Indexes);
            _allSqaresInOneList.Add(_sqare4Indexes);
            _allSqaresInOneList.Add(_sqare5Indexes);
            _allSqaresInOneList.Add(_sqare6Indexes);
            _allSqaresInOneList.Add(_sqare7Indexes);
            _allSqaresInOneList.Add(_sqare8Indexes);
            _allSqaresInOneList.Add(_sqare9Indexes);

        }

        private int GetNumberFromIndex(int[] i)
        {
            return _grid[i[0], i[1]];
        }

        private string GetSqareName(List<int[]> sqare)
        {
            string toReturn = "";

            if (sqare == _sqare1Indexes) toReturn = "Sqare1";
            else if (sqare == _sqare2Indexes) toReturn = "Sqare2";
            else if (sqare == _sqare3Indexes) toReturn = "Sqare3";
            else if (sqare == _sqare4Indexes) toReturn = "Sqare4";
            else if (sqare == _sqare5Indexes) toReturn = "Sqare5";
            else if (sqare == _sqare6Indexes) toReturn = "Sqare6";
            else if (sqare == _sqare7Indexes) toReturn = "Sqare7";
            else if (sqare == _sqare8Indexes) toReturn = "Sqare8";
            else if (sqare == _sqare9Indexes) toReturn = "Sqare9";

            return toReturn;
        }

        private List<int[]> GetSqareFromIndex(int[] i)
        {
            List<int[]> toReturn = new List<int[]>();

            foreach (List<int[]> i2 in _allSqaresInOneList)
            {
                if (i2.Exists(x=>x.SequenceEqual(i)))
                {
                    toReturn = i2;
                    break;
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

        private bool SqareContainsTheNumber(List<int[]> sqare, int number)
        {
            bool toReturn = false;
            foreach (int[] i in sqare)
            {
                if (GetNumberFromIndex(i) == number) toReturn = true;
            }
            return toReturn;
        }

        public void CollectInformation()
        {
            while (!_isReadyToBeSolved)
            {
                Console.WriteLine("write the information you have, ex: 3 4 8, that would mean that row number 3 on the 4th place is an number 8");
                Console.WriteLine("you can write \"show\", to see the information provided, and when you are done, write \"done\"");
                while (!_isReadyToBeSolved)
                {
                    Console.Write("numbers: ");
                    string s = Console.ReadLine();
                    if (s == "done")
                    {
                        _isReadyToBeSolved = true;
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
                            SetDataInGridFromConsole(i1, i2, i3);
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

        private void SetDataInGridFromConsole(int y, int x, int value)
        {
            _grid[y - 1, x - 1] = value;
        }

        private void SetData(int[] location, int number)
        {
            if (_grid[location[0], location[1]] != 0) throw new Exception("Tried to place number on a filled spot");

            _grid[location[0], location[1]] = number;

            if (GetFreeSpotsInList(GetSqareFromIndex(location)).Count == 1)
            {
                List<int> numbers = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
                foreach (int i in GetNumbersInArea(GetSqareFromIndex(location)))
                {
                    if (numbers.Contains(i)) numbers.Remove(i);
                }

                //Console.WriteLine(GetFreeSpotsInList(GetSqareFromIndex(location)).Count);
                //Console.WriteLine(GetFreeSpotsInList(GetSqareFromIndex(location)).Count);
                List <int[]> test = GetFreeSpotsInList(GetSqareFromIndex(location));
                int[] fucked = GetFreeSpotsInList(GetSqareFromIndex(location))[0];
                if (!CanNumberBeHere(fucked, numbers[0]))
                {
                    //Print();
                    //Console.WriteLine($"WE ARE FUCKED AT [{fucked[0]} , {fucked[1]}] with {numbers[0]}");
                }
            }
            //Console.WriteLine($"Number {number} was placed at[{location[0]} , {location[1]}]");
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
                if (k == 3) { Console.WriteLine("---------------------"); k = 0; }
                if (i == 0) Console.Write(" " + " ");
                else Console.Write(i + " ");
                j++;
                l++;
            }

            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }

        public void PrintAllSqareIndexes()
        {
            Console.WriteLine("sqare1");
            foreach (int[] i in _sqare1Indexes)
            {
                Console.WriteLine($"[{i[0]} , {i[1]}]");
            }

            Console.WriteLine("sqare2");
            foreach (int[] i in _sqare2Indexes)
            {
                Console.WriteLine($"[{i[0]} , {i[1]}]");
            }

            Console.WriteLine("sqare3");
            foreach (int[] i in _sqare3Indexes)
            {
                Console.WriteLine($"[{i[0]} , {i[1]}]");
            }

            Console.WriteLine("sqare4");
            foreach (int[] i in _sqare4Indexes)
            {
                Console.WriteLine($"[{i[0]} , {i[1]}]");
            }

            Console.WriteLine("sqare5");
            foreach (int[] i in _sqare5Indexes)
            {
                Console.WriteLine($"[{i[0]} , {i[1]}]");
            }

            Console.WriteLine("sqare6");
            foreach (int[] i in _sqare6Indexes)
            {
                Console.WriteLine($"[{i[0]} , {i[1]}]");
            }

            Console.WriteLine("sqare7");
            foreach (int[] i in _sqare7Indexes)
            {
                Console.WriteLine($"[{i[0]} , {i[1]}]");
            }

            Console.WriteLine("sqare8");
            foreach (int[] i in _sqare8Indexes)
            {
                Console.WriteLine($"[{i[0]} , {i[1]}]");
            }

            Console.WriteLine("sqare9");
            foreach (int[] i in _sqare9Indexes)
            {
                Console.WriteLine($"[{i[0]} , {i[1]}]");
            }
        }

        public void PrintArea(List<int[]> indexes)
        {
            Console.WriteLine("Area:");
            foreach (int[] i in indexes)
            {
                Console.Write($"[{i[0]} , {i[1]}] ");
            }

            Console.WriteLine("");
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
            }
        }

        public void IsItCorrect(bool info = false)
        {
            Console.WriteLine("CHECKING THE RESULT");
            bool toReturn = true;
            int grandTotal = 0;

            foreach (List<int[]> sqare in _allSqaresInOneList)
            {
                int total = 0;
                foreach (int[] box in sqare)
                {
                    total += GetNumberFromIndex(box);
                }

                if (info)
                {
                    Console.WriteLine($"ERROR - {GetSqareName(sqare)} had a total of {total}");
                    if (total != 45) toReturn = false;
                } else if (total != 45) { toReturn = false; Console.WriteLine($"ERROR - {GetSqareName(sqare)} had a total of {total}"); }

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
                } else if (total != 45) { toReturn = false; Console.WriteLine($"ERROR - Row: {i}, had a total of {total}"); }

                total = 0;

                foreach (int[] box in GetColumn(i))
                {
                    total += GetNumberFromIndex(box);
                }

                if (info)
                {
                    Console.WriteLine($"ERROR - Column: {i}, had a total of {total}");
                    if (total != 45) toReturn = false;
                } else if (total != 45) { toReturn = false; Console.WriteLine($"ERROR - Column: {i}, had a total of {total}"); }

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

        public void CheckPlacedNumbers(bool info = false)
        {
            foreach (int[] box in _allIndexes)
            {
                int number = GetNumberFromIndex(box);
                if (number == 0) continue;
                _grid[box[0], box[1]] = 0;
                if (!CanNumberBeHere(box, number)) Console.WriteLine($"NUMBER {number} CANT BE AT [{box[0]} , {box[1]}]");
                else if (info) Console.WriteLine($"[{box[0]} , {box[1]}] CORRECT");
                _grid[box[0], box[1]] = number;
            }
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

        private void Check()
        {
            if (GetTotal() == 405) IsSolved = true;
            else IsSolved = false;
        }

        private void TakeBackup()
        {
            _backup = (int[,])_grid.Clone();
        }
    }
}
