using System.Collections;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public GameObject cube;
    bool readyToStart = false;
    const int size = 8;
    void Start()
    {
        float x = 10f, y = 10f; 
        for(int i = 0; i < size; i++)
            for(int j = 0; j > -size; j--) 
            { 
                var cub = Instantiate(cube, new Vector3(x + i, y + j, 0), Quaternion.identity);
                cub.GetComponent<Renderer>().material.color = Objects.ChooseColor();
                Objects.SetCubes(cub, i, -j);
            } 
    }

    private void Update()
    {
        if(!ClickChange.End)
        { 
            if(!readyToStart)
            { 
                for (int i = 0; i < size; i++)
                    for( int j = 0; j < size; j++)
                    CheckColor(i, j);
                readyToStart = true;
            }
            ClickChange.DeleteCube();
            if ((Time.time - ClickChange.TimeClick) > 5)
                ClickChange.HintCube = ClickChange.Hint();
            ClickChange.Refresh();
        }
    }

    private void CheckColor( int i, int j)
    {
        ChangeColor(i, i + 1, i - 1, j, j);
        ChangeColor(j, i, i, j + 1, j - 1);
    }

    private void ChangeColor(int k, int iplus, int iminus, int jplus, int jminus)
    {
        Color color1, color2;
        if (k != 0 && k != 7)
        {
            color1 = Objects.GetCubes(iminus, jminus).GetComponent<Renderer>().material.color;
            color2 = Objects.GetCubes(iplus, jplus).GetComponent<Renderer>().material.color;
            while (color1.Equals(color2))
            {
                Objects.GetCubes(iplus, jplus).GetComponent<Renderer>().material.color = Objects.ChooseColor();
                color2 = Objects.GetCubes(iplus, jplus).GetComponent<Renderer>().material.color;
            }
        }
    }
}
