using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ThangVN.LevelPikachu
{

    public class PathFind
    {
        private int[,] grid;

        public PathFind(int[,] grid)
        {
            this.grid = grid;
        }
        public void PrintGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                string rowString = "";
                for (int j = 0; j < 8; j++)
                {
                    rowString += grid[i, j] + " ";
                }
                Debug.Log(rowString);
            }
        }

        public List<Block> FindPath(Block p1, Block p2)
        {
            if (p1.row != p2.row || p1.col != p2.col)
            {
                if (grid[p1.row, p1.col] == grid[p2.row, p2.col])
                {
                    if (p1.row == p2.row)
                    {
                        if (CheckLineX(p1.col, p2.col, p1.row))
                        {
                            Debug.Log("Check Line X");
                            return new List<Block> { p1, p2 };
                        }
                    }

                    if (p1.col == p2.col)
                    {
                        if (CheckLineY(p1.row, p2.row, p1.col))
                        {
                            Debug.Log("Check Line Y");
                            return new List<Block> { p1, p2 };
                        }
                    }

                    if (CheckRectX(p1, p2))
                    {
                        Debug.Log("Check Rect X");
                        return GetRectXPath(p1, p2);
                    }

                    if (CheckRectY(p1, p2))
                    {
                        Debug.Log("Check Rect Y");
                        return GetRectYPath(p1, p2);
                    }

                    if (CheckMoreLineX(p1, p2, -1))
                    {
                        Debug.Log("Check More Line X -1");
                        return GetMoreLineXPath(p1, p2, -1);
                    }

                    if (CheckMoreLineX(p1, p2, 1))
                    {
                        Debug.Log("Check More Line X 1");
                        return GetMoreLineXPath(p1, p2, 1);
                    }

                    if (CheckMoreLineY(p1, p2, -1))
                    {
                        Debug.Log("Check More Line Y -1");
                        return GetMoreLineYPath(p1, p2, -1);
                    }

                    if (CheckMoreLineY(p1, p2, 1))
                    {
                        Debug.Log("Check More Line Y 1");
                        return GetMoreLineYPath(p1, p2, 1);
                    }
                }
            }

            return null;
        }

        private bool CheckLineX(int y1, int y2, int x)
        {
            int min = Mathf.Min(y1, y2);
            int max = Mathf.Max(y1, y2);

            for (int y = min + 1; y < max; y++)
            {
                if (grid[x, y] != 0) return false;
            }

            return true;
        }

        private bool CheckLineY(int x1, int x2, int y)
        {
            int min = Mathf.Min(x1, x2);
            int max = Mathf.Max(x1, x2);

            for (int x = min + 1; x < max; x++)
            {
                if (grid[x, y] != 0) return false;
            }

            return true;
        }

        private bool CheckRectX(Block p1, Block p2)
        {
            Block pMinY = p1;
            Block pMaxY = p2;

            if (p1.col > p2.col)
            {
                pMinY = p2;
                pMaxY = p1;
            }

            Debug.Log("pMinY: " + $"{pMinY.row},{pMinY.col}");
            Debug.Log("pMaxY: " + $"{pMaxY.row},{pMaxY.col}");


            for (int y = pMinY.col; y <= pMaxY.col; y++)
            {
                Debug.Log("y: " + y);
                Debug.Log($"grid[{pMinY.row}, {y}]:" + grid[pMinY.row, y]);

                if (y > pMinY.col && grid[pMinY.row, y] != 0)
                {
                    return false;
                }

                Debug.Log($"grid[{pMaxY.row}, {y}]: )" + (grid[pMaxY.row, y]));
                Debug.Log($"CheckLineY({pMinY.row}, {pMaxY.row}, {y}): )" + CheckLineY(pMinY.row, pMaxY.row, y));
                Debug.Log($"CheckLineX({y}, {pMaxY.col}, {pMaxY.row}): )" + CheckLineX(y, pMaxY.col, pMaxY.row));

                if ((grid[pMaxY.row, y] == 0 || y == pMaxY.col)
                && CheckLineY(pMinY.row, pMaxY.row, y)
                && CheckLineX(y, pMaxY.col, pMaxY.row)
                )
                {
                    return true;
                }

            }

            return false;
        }

        private bool CheckRectY(Block p1, Block p2)
        {
            Block pMinX = p1;
            Block pMaxX = p2;

            if (p1.row > p2.row)
            {
                pMinX = p2;
                pMaxX = p1;
            }
            Debug.Log("pMinX: " + $"{pMinX.row},{pMinX.col}");
            Debug.Log("pMaxX: " + $"{pMaxX.row},{pMaxX.col}");

            for (int x = pMinX.row; x <= pMaxX.row; x++)
            {
                Debug.Log("x: " + x);
                Debug.Log($"grid[{x}, {pMinX.col}]:" + grid[x, pMinX.col]);

                if (x > pMinX.row && grid[x, pMinX.col] != 0)
                {
                    return false;
                }

                Debug.Log($"grid[{x}, {pMaxX.col}]: )" + (grid[x, pMaxX.col]));
                Debug.Log($"CheckLineY({x}, {pMaxX.row}, {pMaxX.col}): )" + CheckLineY(x, pMaxX.row, pMaxX.col));
                Debug.Log($"CheckLineX({pMinX.col}, {pMaxX.col}, {x}): )" + CheckLineX(pMinX.col, pMaxX.col, x));

                if ((grid[x, pMaxX.col] == 0 || x == pMaxX.row)
                    && CheckLineX(pMinX.col, pMaxX.col, x)
                    && CheckLineY(x, pMaxX.row, pMaxX.col))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckMoreLineX(Block p1, Block p2, int type)
        {
            Block pMinY = p1;
            Block pMaxY = p2;

            if (p1.col > p2.col)
            {
                pMinY = p2;
                pMaxY = p1;
            }

            int y = pMaxY.col + type;
            int row = pMinY.row;
            int colEnd = pMaxY.col;

            if (type == -1)
            {
                y = pMinY.col + type;
                row = pMaxY.row;
                colEnd = pMinY.col;
            }

            if ((grid[row, colEnd] == 0 || pMinY.col == pMaxY.col)
                && CheckLineX(pMinY.col, pMaxY.col, row))
            {
                while (grid[pMinY.row, y] == 0 && grid[pMaxY.row, y] == 0)
                {
                    if (CheckLineY(pMinY.row, pMaxY.row, y))
                    {
                        return true;
                    }

                    y += type;
                }

            }
            return false;
        }

        private bool CheckMoreLineY(Block p1, Block p2, int type)
        {
            Block pMinX = p1;
            Block pMaxX = p2;

            if (p1.row > p2.row)
            {
                pMinX = p2;
                pMaxX = p1;
            }

            int x = pMaxX.row + type;
            int col = pMinX.col;
            int rowEnd = pMaxX.row;
            if (type == -1)
            {
                x = pMinX.row + type;
                col = pMaxX.col;
                rowEnd = pMinX.row;
            }

            if ((grid[rowEnd, col] == 0 || pMinX.row == pMaxX.row)
                && CheckLineY(pMinX.row, pMaxX.row, col))
            {
                while (grid[x, pMinX.col] == 0
                    && grid[x, pMaxX.col] == 0)
                {
                    if (CheckLineX(pMinX.col, pMaxX.col, x))
                    {

                        return true;
                    }
                    x += type;
                }
            }
            return false;
        }

        private List<Block> GetRectXPath(Block p1, Block p2)
        {
            List<Block> path = new List<Block>();
            Block pMinY = p1;
            Block pMaxY = p2;

            if (p1.col > p2.col)
            {
                pMinY = p2;
                pMaxY = p1;
            }

            for (int y = pMinY.col; y <= pMaxY.col; y++)
            {
                if (y > pMinY.col && grid[pMinY.row, y] != 0)
                {
                    return null;
                }

                if ((grid[pMaxY.row, y] == 0 || y == pMaxY.col)
                    && CheckLineY(pMinY.row, pMaxY.row, y)
                    && CheckLineX(y, pMaxY.col, pMaxY.row))
                {
                    if (pMinY.col == y)
                    {
                        path.Add(pMinY);
                    }
                    else
                    {
                        path.Add(pMinY);
                        path.Add(GridManager.Instance.GetBlock(pMinY.row, y));
                    }

                    path.Add(GridManager.Instance.GetBlock(pMaxY.row, y));
                    path.Add(pMaxY);

                    return path;
                }
            }

            return null;
        }

        private List<Block> GetRectYPath(Block p1, Block p2)
        {
            List<Block> path = new List<Block>();
            Block pMinX = p1;
            Block pMaxX = p2;

            if (p1.row > p2.row)
            {
                pMinX = p2;
                pMaxX = p1;
            }

            for (int x = pMinX.row; x <= pMaxX.row; x++)
            {
                if (x > pMinX.row && grid[x, pMinX.col] != 0)
                {
                    return null;
                }

                if ((grid[x, pMaxX.col] == 0 || x == pMaxX.row)
                && CheckLineX(pMinX.col, pMaxX.col, x)
                && CheckLineY(x, pMaxX.row, pMaxX.col))
                {
                    if (pMinX.row == x)
                    {
                        path.Add(pMinX);
                    }
                    else
                    {
                        path.Add(pMinX);
                        path.Add(GridManager.Instance.GetBlock(x, pMinX.col));
                    }

                    path.Add(GridManager.Instance.GetBlock(x, pMaxX.col));
                    path.Add(pMaxX);

                    return path;
                }
            }

            return null;
        }

        private List<Block> GetMoreLineXPath(Block p1, Block p2, int type)
        {
            List<Block> path = new List<Block>();
            Block pMinY = p1;
            Block pMaxY = p2;

            if (p1.col > p2.col)
            {
                pMinY = p2;
                pMaxY = p1;
            }

            int y = pMaxY.col + type;
            int row = pMinY.row;
            int colEnd = pMaxY.col;

            if (type == -1)
            {
                colEnd = pMinY.col;
                y = pMinY.col + type;
                row = pMaxY.row;
            }

            if ((grid[row, colEnd] == 0 || pMinY.col == pMaxY.col)
                && CheckLineX(pMinY.col, pMaxY.col, row))
            {
                while (grid[pMinY.row, y] == 0 && grid[pMaxY.row, y] == 0)
                {
                    if (CheckLineY(pMinY.row, pMaxY.row, y))
                    {
                        path.Add(pMinY);
                        path.Add(GridManager.Instance.GetBlock(pMinY.row, y));
                        path.Add(GridManager.Instance.GetBlock(pMaxY.row, y));
                        path.Add(pMaxY);

                        return path;
                    }
                    y += type;
                }
            }

            return null;
        }

        private List<Block> GetMoreLineYPath(Block p1, Block p2, int type)
        {
            List<Block> path = new List<Block>();

            Block pMinX = p1;
            Block pMaxX = p2;

            if (p1.row > p2.row)
            {
                pMinX = p2;
                pMaxX = p1;
            }

            int x = pMaxX.row + type;
            int col = pMinX.col;
            int rowEnd = pMaxX.row;
            if (type == -1)
            {
                x = pMinX.row + type;
                col = pMaxX.col;
                rowEnd = pMinX.row;
            }

            if ((grid[rowEnd, col] == 0 || pMinX.row == pMaxX.row)
                && CheckLineY(pMinX.row, pMaxX.row, col))
            {
                while (grid[x, pMinX.col] == 0
                    && grid[x, pMaxX.col] == 0)
                {
                    if (CheckLineX(pMinX.col, pMaxX.col, x))
                    {

                        path.Add(pMinX);
                        path.Add(GridManager.Instance.GetBlock(x, pMinX.col));
                        path.Add(GridManager.Instance.GetBlock(x, pMaxX.col));
                        path.Add(pMaxX);

                        return path;
                    }
                    x += type;
                }
            }
            return null;
        }
    }
}