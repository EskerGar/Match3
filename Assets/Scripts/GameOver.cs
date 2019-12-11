using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text score, time, game, need;
    private float cubeHave;
    private const float cubeNeed = 200;
    private float timeLeft = 60;

    private void Update()
    {
        if(!ClickChange.End)
        { 
            cubeHave = ClickChange.GetAmount();
            score.text = "Cubes: " + cubeHave.ToString();
            timeLeft-= .01f;
            time.text = "Time remain: " + timeLeft.ToString();
            need.text = "Goal: " + cubeNeed.ToString();
            if (cubeHave >= cubeNeed)
            { 
                game.text = "You Win!";
                ClickChange.End = true;
            }
            if (timeLeft <= 0)
            {
                Time.timeScale = 0;
                ClickChange.End = true;
                game.text = "You Lose!";
            }
        }
    }
}
