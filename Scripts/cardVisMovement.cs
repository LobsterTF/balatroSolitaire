using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardVisMovement : MonoBehaviour
{
    private Transform pairedCard;
    [SerializeField] private Transform tiltParent;
    private float speed = .3f, chainSpeed, randOffset;
    private bool isIdle = true, isHovering = false, isChaining;
    public void setFollowPoint(Transform trans)
    {
        pairedCard = trans;
    }
    public void idleSet(bool set)
    {
        if (set == false && isIdle == true)
        {
            tiltParent.eulerAngles = new Vector3(0, 0, 0);
        }
        isIdle = set;
        
    }
    public void setChainSpeed(int order)
    {
        /*if (order > 5) { order = 5; sub = .8f; }*/
        speed = .28f * (Mathf.Pow(order, -.75f));
    }
    public void resetSpeed()
    {
        speed = .3f;
    }
    public void hoverSet(bool set)
    {
        isHovering = set;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (speed != .3f)
        {
            isIdle = false;
        }
        transform.position = Vector3.Lerp(transform.position, pairedCard.position, speed);
        float xDif = transform.position.x - pairedCard.position.x;
        float yDif = transform.position.y - pairedCard.position.y;
        float distance = (transform.position - pairedCard.position).magnitude / 250;
        float disRaw = (transform.position - pairedCard.position).magnitude;
        float angle = Mathf.Atan2(xDif, yDif);

        

        if (isIdle)
        {
            if (!isHovering)
            {
                float sine = Mathf.Sin(Time.time + randOffset);
                float cosine = Mathf.Cos(Time.time + randOffset);
                tiltParent.eulerAngles = new Vector3(cosine * 14, sine * 14, 0);
            }
            else
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                Vector3 lookDir = new Vector3(transform.position.x - mousePos.x, transform.position.y - mousePos.y, 0);
                if (lookDir.magnitude > 100)
                {
                    isHovering = false;
                }
                tiltParent.eulerAngles = new Vector3(-lookDir.y/3.1f, lookDir.x/3.1f, 0);
            }


        }
        
        

        transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp((Mathf.Rad2Deg * angle) * distance, -85,85));
        
    }
    void Awake()
    {
        randOffset = Random.Range(-3f, 3f);
    }
}
