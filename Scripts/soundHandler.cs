using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource[] cardSound, snd;
    public void callSound(int index)
    {
        snd[index].Play();
    }
    public void callCardSound()
    {
        cardSound[Random.Range(0,cardSound.Length)].Play();
    }
}
