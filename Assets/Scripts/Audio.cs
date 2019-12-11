using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    public AudioClip clip1;
    private AudioSource player;
    private bool stopped = false;
    private void Start()
    {
        player = GetComponent<AudioSource>();
        player.clip = clip1;
        player.Play();
    }

    public void Stop()
    {
        if (!stopped)
        { 
            player.Stop();
            stopped = true;
            GetComponentInChildren<Text>().text = "Audio Off";
        }
        else
        { 
            player.Play();
            stopped = false;
            GetComponentInChildren<Text>().text = "Audio On";
        }
    }
}
