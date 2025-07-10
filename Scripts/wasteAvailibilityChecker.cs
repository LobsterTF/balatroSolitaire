using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wasteAvailibilityChecker : MonoBehaviour
{
    private int wasteDepth;
    [SerializeField] private Transform[] wasteSpots;

    public void callDraw(int drawSize)
    {
        wasteDepth = drawSize;
        updateCovered();
    }
    public void usedWasteCard()
    {
        wasteDepth--;
        if (wasteDepth < 0)
        {
            int mostCards = 0;
            int cardCount = 0;
            for (int i = 0; i < wasteSpots.Length; i++)
            {
                int lastCardCount = cardCount;
                cardCount = 0;
                foreach (Transform card in wasteSpots[i].transform)
                {
                    cardCount++;
                }
              if (cardCount >= lastCardCount)
              {
                    mostCards = i;
              }
            }
            wasteDepth = mostCards;
        }
        updateCovered();
    }
    public void updateCovered()
    {
        for (int i = 0; i < wasteSpots.Length; i++)
        {
            if (i == wasteDepth)
            {
                foreach (Transform card in wasteSpots[i].transform)
                {
                    cardHandler cardScript = card.gameObject.GetComponent<cardHandler>();
                    cardScript.setCovered(false);
                    cardScript.setCovered(false);
                    cardScript.updateRow(3);
                }
            }
            else
            {
                foreach (Transform card in wasteSpots[i].transform)
                {
                    cardHandler cardScript = card.gameObject.GetComponent<cardHandler>();
                    cardScript.updateRow(0);
                    cardScript.setCovered(true);
                }
            }
        }
    }
}
