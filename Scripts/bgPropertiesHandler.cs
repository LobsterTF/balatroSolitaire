using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgPropertiesHandler : MonoBehaviour
{
    [SerializeField] private Renderer bgMat;
    // Start is called before the first frame update
    void Start()
    {
        bgMat.material.SetFloat("_randSeed2", Random.Range(0, 100));
    }

}
