using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    bool pause = false;
    public void GetPause()
    {
        if (!pause)
        {
            Time.timeScale = 0;
            pause = true;
            ClickChange.End = true;
        }else
        {
            Time.timeScale = 1;
            pause = false;
            ClickChange.End = false;
        }
    }
}
