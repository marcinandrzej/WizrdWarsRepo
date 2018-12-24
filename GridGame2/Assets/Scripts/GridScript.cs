using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScript
{
    //Block grid
    public void BlockGrid(bool enable, GameObject[,] tab, int cellCount)
    {
        for (int i = 0; i < cellCount; i++)
        {
            for (int j = 0; j < cellCount; j++)
            {
                if (tab[i, j] != null)
                {
                    tab[i, j].GetComponent<Button>().enabled = enable;
                }
            }
        }
    }

    //Create list of blocks to destroy
    public void CheckNeighbours(int x, int y, int colIndex, List<GameObject> toDestroy, GameObject[,] tab, int cellCount)
    {
        if (CheckBlock((x - 1), y, colIndex, tab, cellCount) && !toDestroy.Contains(tab[x - 1, y]))
        {
            toDestroy.Add(tab[(x - 1), y]);
            CheckNeighbours((x - 1), y, colIndex, toDestroy, tab, cellCount);
        }
        if (CheckBlock((x + 1), y, colIndex, tab, cellCount) && !toDestroy.Contains(tab[x + 1, y]))
        {
            toDestroy.Add(tab[(x + 1), y]);
            CheckNeighbours((x + 1), y, colIndex, toDestroy, tab, cellCount);
        }
        if (CheckBlock(x, (y - 1), colIndex, tab, cellCount) && !toDestroy.Contains(tab[x, (y - 1)]))
        {
            toDestroy.Add(tab[x, (y - 1)]);
            CheckNeighbours(x, (y - 1), colIndex, toDestroy, tab, cellCount);
        }
        if (CheckBlock(x, (y + 1), colIndex, tab, cellCount) && !toDestroy.Contains(tab[x, (y + 1)]))
        {
            toDestroy.Add(tab[x, (y + 1)]);
            CheckNeighbours(x, (y + 1), colIndex, toDestroy, tab, cellCount);
        }
    }

    public bool CheckBlock(int x, int y, int colIndex, GameObject[,] tab, int cellCount)
    {
        if (x >= 0 && x < cellCount && y >= 0 && y < cellCount)
        {
            if (tab[x, y] != null)
            {
                if (tab[x, y].GetComponent<ButtonScript>().ColorIndex == colIndex)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Move row
    public void BlocksFall(GameObject[,] tab, int cellCount)
    {
        for (int x = 0; x < cellCount; x++)
        {
            for (int y = (cellCount - 1); y > 0; y--)
            {
                while (tab[x, y] == null && CanFall(x, y, tab))
                {
                    Fall(x, y, tab);
                }
            }
        }
    }

    public bool CanFall(int x, int y, GameObject[,] tab)
    {
        for (int i = 0; i < y; i++)
        {
            if (tab[x, i] != null)
                return true;
        }
        return false;
    }

    public void Fall(int x, int y, GameObject[,] tab)
    {
        for (int i = y; i > 0; i--)
        {
            tab[x, i] = tab[x, i - 1];
        }
        tab[x, 0] = null;
    }

    //Move column
    public void ColumnMove(GameObject[,] tab, int cellCount)
    {
        for (int _x = 0; _x < cellCount - 1; _x++)
        {
            while (EmptyColumn(_x, tab, cellCount) && CanMove(_x, tab, cellCount))
            {
                MoveColumn(_x, tab, cellCount);
            }
        }
    }

    public bool EmptyColumn(int x, GameObject[,] tab, int cellCount)
    {
        if (tab[x, (cellCount - 1)] != null)
            return false;
        return true;
    }

    public bool CanMove(int x, GameObject[,] tab, int cellCount)
    {
        for (int i = x + 1; i < cellCount; i++)
        {
            if (!EmptyColumn(i, tab, cellCount))
                return true;
        }
        return false;
    }

    public void MoveColumn(int _x, GameObject[,] tab, int cellCount)
    {
        for (int x = _x; x < cellCount - 1; x++)
        {
            for (int y = 0; y < cellCount; y++)
            {
                tab[x, y] = tab[x + 1, y];
            }
        }
        for (int i = 0; i < cellCount; i++)
        {
            tab[cellCount - 1, i] = null;
        }
    }
}
