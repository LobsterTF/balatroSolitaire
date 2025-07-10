using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardAnimHandler : MonoBehaviour
{
    [SerializeField] private Animator cardAnimator;
    [SerializeField] private cardVisMovement cardVisMovementScript;
    [SerializeField] private shadowScript shadowScriptScript; // Smile smile
    [SerializeField] private GameObject cardFace, cardBack;
    public void callDrag()
    {
        cardAnimator.Play("cardDrag", 0);
        cardVisMovementScript.idleSet(false);
        shadowScriptScript.setDragging(true);
    }
    public void callIdle()
    {
        cardAnimator.Play("cardDefault", 0);
        cardVisMovementScript.idleSet(true);
        shadowScriptScript.setDragging(false);

    }
    public void callHover()
    {
        cardAnimator.Play("cardShake", 0);
        cardVisMovementScript.hoverSet(true);

    }
    public void endHover()
    {
        cardVisMovementScript.hoverSet(false);

    }
    public void callChain(int order)
    {
        if (order == -1)
        {
            cardVisMovementScript.resetSpeed();
            return;
        }
        cardVisMovementScript.setChainSpeed(order);
    }
    public void faceUpCard(bool state)
    {
        if (state)
        {
            cardFace.SetActive(true);
            cardBack.SetActive(false);
        }
        else
        {
            cardFace.SetActive(false);
            cardBack.SetActive(true);
        }
    }
}
