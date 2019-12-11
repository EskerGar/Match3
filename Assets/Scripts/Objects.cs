using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Objects
{
    private static readonly GameObject[,] cubes = new GameObject[8, 8];

    public static GameObject GetCubes(int x, int y) => cubes[x, y];

    public static int[] GetCubes(GameObject cube)
    {
        int[] xy = new int[2];
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (cube.Equals(cubes[i, j]))
                {
                    xy[0] = i;
                    xy[1] = j;
                }
        return xy;
    }

    public static void SetCubes(GameObject cube, int x, int y) => cubes[x, y] = cube;

    public static Color ChooseColor()
    {
        int color = Random.Range(1, 10);
        switch (color)
        {
            case 1: return Color.red;
            case 2: return Color.blue;
            case 3: return Color.green;
            case 4: return Color.yellow;
            case 5: return Color.black;
            case 6: return Color.cyan;
            case 7: return Color.magenta;
            case 8: return Color.gray;
            default: return Color.white;
        };
    }

}
