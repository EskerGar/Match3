using System.Collections.Generic;
using UnityEngine;

public class ArrayIntCompare : IComparer<int[]>
{
    public int Compare(int[] x, int[] y)
    {
        if (x[1].Equals(y[1]))
            return x[0].CompareTo(y[0]);
        else
            return x[1].CompareTo(y[1]);
    }
}

public static class ClickChange
{
    private static int[] xy = new int[2];
    private static int amount = 0;
    private static bool needHint = false;

    public static bool End { get; set; } = false;
    public static bool FirstClick { get; private set; } = true;
    public static GameObject FirstCube { get; set; }
    public static GameObject HintCube { get; set; }

    public static float TimeClick { get; set; }
    public static Color FirstColor { get; set; }
    public static Color SecondColor { get; set; }
    private static List<int[]> RemoveCubes { get; } = new List<int[]>();

    public static int GetAmount() => amount;

    public static bool GetHint() => needHint;

    public static void HintFalse() => needHint = false;

    public static bool CheckClick(GameObject cube1, GameObject go)
    {
        GameObject cuberight = null, cubeleft = null, cubeup = null, cubedown = null;
        SetXY(Objects.GetCubes(cube1));
        var xy = GetXY();
        if (xy[0] != 7)
            cuberight = Objects.GetCubes(xy[0] + 1, xy[1]);
        if (xy[0] != 0)
            cubeleft = Objects.GetCubes(xy[0] - 1, xy[1]);
        if (xy[1] != 0)
            cubeup = Objects.GetCubes(xy[0], xy[1] - 1);
        if (xy[1] != 7)
            cubedown = Objects.GetCubes(xy[0], xy[1] + 1);
        if (go.Equals(cuberight) || go.Equals(cubeleft) || go.Equals(cubeup) || go.Equals(cubedown))
            return true;
        else return false;
    }

    public static bool CheckAllow(GameObject cube, Color firstColor, GameObject firstCube)
    {
        int count;
        var xy = Objects.GetCubes(cube);
        int x = xy[0], y = xy[1];
        count = CheckPlusX(firstColor, firstCube, x, y, false);
        count += CheckMinusX(firstColor, firstCube, x, y);
        count += CheckPlusY(firstColor, firstCube, x, y, false);
        count += CheckMinusY(firstColor, firstCube, x, y);
        if (count > 1)
            return true;
        else return false;
    }

    public static GameObject Hint()
    {
        GameObject cubeFirst = null, cubeSecond = null;
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                cubeFirst = Objects.GetCubes(i, j);
                if(j != 7)
                    cubeSecond = Objects.GetCubes(i, j + 1);
                if (!needHint && CheckAllow(cubeSecond, cubeFirst.GetComponent<Renderer>().material.color, cubeFirst))
                {
                    cubeFirst.transform.localScale = new Vector3(.75f, .75f, 1);
                    needHint = true;
                    return cubeFirst;
                }
            }
        return cubeFirst;
    }

    private static bool CheckAllowField()
    {
        bool checkallow = false, checkclick = false;
        GameObject cubeFirst, cubeSecond;
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                cubeFirst = Objects.GetCubes(i, j);
                if (j != 7)
                    cubeSecond = Objects.GetCubes(i, j + 1);
                else
                    cubeSecond = Objects.GetCubes(i, j - 1);
                if (!checkallow)
                    checkallow = CheckAllow(cubeSecond, cubeFirst.GetComponent<Renderer>().material.color, cubeFirst);
                if (!checkclick)
                    checkclick = CheckClick(cubeFirst, cubeSecond);
            }
        if (checkallow && checkclick)
            return true;
        else return false;
    }

    public static void Refresh()
    {
        GameObject cube;
        while(!CheckAllowField())
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    cube = Objects.GetCubes(i, j);
                    cube.GetComponent<Renderer>().material.color = Objects.ChooseColor();
                }
            CheckAllowField();
        }
    }

    public static void DeleteCube()
    {
        int y;
        int[] curCube;
        GameObject cube, cubes;
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                cube = Objects.GetCubes(i, j);
                CheckPlusX(cube.GetComponent<Renderer>().material.color, cube, i, j, true);
                CheckPlusY(cube.GetComponent<Renderer>().material.color, cube, i, j, true);
                RemoveCubes.Sort(new ArrayIntCompare());
                amount += RemoveCubes.Count;
                if (RemoveCubes.Count != 0)
                    for (int k = 0; k < RemoveCubes.Count; k++)
                    {
                        curCube = RemoveCubes[k];
                        y = curCube[1];
                        while (y != -1)
                        {
                            cubes = Objects.GetCubes(curCube[0], y);
                            if (y == 0)
                                cubes.GetComponent<Renderer>().material.color = Objects.ChooseColor();
                            else
                                cubes.GetComponent<Renderer>().material.color = Objects.GetCubes(curCube[0], y - 1).GetComponent<Renderer>().material.color;
                            y--;
                        }
                    }
                RemoveCubes.Clear();
            }
    }

    public static int[] GetXY() => xy;

    public static void SetXY(int[] xy1) => xy = xy1;

    public static void Clicked() => FirstClick = !FirstClick;

    private static void AddCube(int x, int y)
    {
        bool add = true;
        int[] pos = new int[2] { x, y };
        foreach (var v in RemoveCubes)
            if (v[0] == pos[0] && v[1] == pos[1])
                add = false;
        if(add)
            RemoveCubes.Add(new int[2] { x,y});
    }

    public static int CheckPlusX(Color FirstColor, GameObject FirstCube, int x, int y, bool delete)
    {
        int count = 0;
        Color col;
        List<int[]> curList = new List<int[]>
        {
            new int[2] { x, y }
        };
        if (x != 7)
        {
            x++;
            col = Objects.GetCubes(x, y).GetComponent<Renderer>().material.color;
            while (FirstColor.Equals(col) &&
                   !FirstCube.Equals(Objects.GetCubes(x, y)))
            {
                count++;
                curList.Add(new int[2] { x, y });
                if (x >= 7)
                    break;
                x++;
                col = Objects.GetCubes(x, y).GetComponent<Renderer>().material.color;
            }
        }
        if (curList.Count > 2 && delete)
            foreach (var v in curList)
                AddCube(v[0], v[1]);
        return count;
    }

    public static int CheckPlusY(Color FirstColor, GameObject FirstCube, int x, int y, bool delete)
    {
        int count = 0;
        Color col;
        List<int[]> curList = new List<int[]>
        {
            new int[2] { x, y }
        };
        if (y != 7)
        {
            y++;
            col = Objects.GetCubes(x, y).GetComponent<Renderer>().material.color;
            while (FirstColor.Equals(col) &&
                   !FirstCube.Equals(Objects.GetCubes(x, y)))
            {
                count++;
                curList.Add(new int[2] { x, y });
                if (y >= 7)
                    break;
                y++;
                col = Objects.GetCubes(x, y).GetComponent<Renderer>().material.color;
            }
        }
        if (curList.Count > 2 && delete)
            foreach (var v in curList)
                AddCube(v[0], v[1]);
        return count;
    }

    public static int CheckMinusX(Color FirstColor, GameObject FirstCube, int x, int y)
    {
        int count = 0;
        Color col;
        if (x != 0)
        {
            x--;
            col = Objects.GetCubes(x, y).GetComponent<Renderer>().material.color;
            while (FirstColor.Equals(col) &&
                   !FirstCube.Equals(Objects.GetCubes(x, y)))
            {
                count++;
                if (x <= 0)
                    break;
                x--;
                col = Objects.GetCubes(x, y).GetComponent<Renderer>().material.color;
            }
        }
        return count;
    }

    public static int CheckMinusY(Color FirstColor, GameObject FirstCube, int x, int y)
    {
        int count = 0;
        Color col;
        if (y != 0)
        {
            y--;
            col = Objects.GetCubes(x, y).GetComponent<Renderer>().material.color;
            while (FirstColor.Equals(col) &&
                   !FirstCube.Equals(Objects.GetCubes(x, y)))
            {
                count++;
                if (y <= 0)
                    break;
                y--;
                col = Objects.GetCubes(x, y).GetComponent<Renderer>().material.color;
            }
        }
        return count;
    }
}
