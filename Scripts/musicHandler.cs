using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource, altAudioSource, ambienceAudioSource;
    public float volume;
    private bool musicEnabled;
    [SerializeField] private Animator soundAnimator;
    private bool callMusic = false, firstRestart = true;
    private int timesPassed;
    void Start()
    {
        /*int musicSet = PlayerPrefs.GetInt("music");
        if (musicSet == 0) { musicEnabled = false; } else { musicEnabled = true; }*/
    }
    public void toggleMusic()
    {
        musicEnabled = !musicEnabled;
    }
    

    void OnEnable()
    {
        CancelInvoke();
        soundAnimator.Play("musicInit", 0);
    }
    void LateUpdate()
    {
        
        if (!audioSource.isPlaying && !altAudioSource.isPlaying)
        {
            if (callMusic)
            {
                callMusic = false;
                float randTime = Random.Range(30,300f);
                /*int playAmbience = Random.Range(1, 4 - timesPassed);
                if (playAmbience == 1 && !firstRestart)
                {
                    timesPassed = 0;
                    ambienceAudioSource.Play();
                }
                else
                {
                    timesPassed++;
                }*/
                
                Invoke("startMusic", randTime);
            }
        }
        else
        {
            CancelInvoke();

            callMusic = true;
        }
    }
    void startMusic()
    {
        if (firstRestart)
        {
            soundAnimator.Play("altMusicInit", 0);
            firstRestart = false;
            return;
        }
        int randSel = Random.Range(0, 2);
        if (randSel == 0)
        {
            soundAnimator.Play("musicInit", 0);

        }
        else
        {
            soundAnimator.Play("altMusicInit", 0);

        }
    }
    public void startStop(bool In)
    {
        if (In)
        {
            
            
                soundAnimator.Play("fadeIn", 0);

            
        }
        else
        {

            soundAnimator.Play("fade", 0);

        }
    }
}
