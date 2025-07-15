using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource[] cardSound, snd;
    private bool sfxOn = true;
    void Start()
    {
        int sfxSet = PlayerPrefs.GetInt("sfxPref");
        if (sfxSet == 2) { sfxOn = false; } else { sfxOn = true; }
    }
    public void toggleSFX()
    {
        sfxOn = !sfxOn;
        if (sfxOn)
        {
            PlayerPrefs.SetInt("sfxPref", 1);

        }
        else
        {
            PlayerPrefs.SetInt("sfxPref", 2);

        }
    }
    public void callSound(int index)
    {
        if (!sfxOn) { return; }
        snd[index].Play();
    }
    public void callCardSound()
    {
        if (!sfxOn) { return; }
        cardSound[Random.Range(0,cardSound.Length)].Play();
    }
}
