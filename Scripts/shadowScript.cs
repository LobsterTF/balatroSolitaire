using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowScript : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    private bool dragging = false;
    void Update()
    {
        if (dragging)
        {
            transform.position = transform.parent.position + offset;
        }
        else
        {
            transform.position = transform.parent.position;

        }
    }
    public void setDragging(bool set)
    {
        dragging = set;
    }
}
