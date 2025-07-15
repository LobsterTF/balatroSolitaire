using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgPropertiesHandler : MonoBehaviour
{
    [SerializeField] private Renderer bgMat;

    [SerializeField] private float scrollSpeed;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        //scrollSpeed = Mathf.Lerp(1, 10, Time.time);
        bgMat.material.SetFloat("_scrollSpeed", scrollSpeed);
    
    }
    void Start()
    {
        bgMat.material.SetFloat("_randSeed2", Random.Range(0, 100));
    }

}
