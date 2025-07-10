using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardSprites : MonoBehaviour
{
    [SerializeField] private Sprite[] cardSpriteArray;
    public Sprite getSprite(cardData card)
    {
        int selection = 0;
        // get Rank
        selection = (int)(card.cardRank);
        // get suit
        selection += ((int)(card.cardSuit)) * 13;
        return cardSpriteArray[selection];
    }
}
