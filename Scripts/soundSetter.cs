using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundSetter : MonoBehaviour
{
    [SerializeField] private soundHandler soundHandlerScript;
    public void toggleSFX()
    {
        soundHandlerScript.toggleSFX();
    }
    public void toggleMusic()
    {
        soundHandlerScript.toggleSFX();
    }
}
