using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgStarter : MonoBehaviour
{

    [SerializeField] private GameObject bg;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            bg.SetActive(true);
        }
        
    }

}
