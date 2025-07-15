using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicSetter : MonoBehaviour
{
    [SerializeField] private GameObject musicObject;
    private bool musicOn = true;

    void Start()
    {
        int musicSet = PlayerPrefs.GetInt("musicPref");
        if (musicSet == 2) { musicOn = false; } else { musicOn = true; }
    }
    public void toggleMusic()
    {
        musicOn = !musicOn;
        if (musicOn)
        {
            PlayerPrefs.SetInt("musicPref", 1);

        }
        else
        {
            PlayerPrefs.SetInt("musicPref", 2);

        }
        musicObject.SetActive(musicOn);
    }
}
