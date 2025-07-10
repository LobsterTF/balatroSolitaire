using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cardData
{
    [SerializeField] public suit cardSuit;
    [SerializeField] public rank cardRank;
    

}
[System.Serializable]
public enum suit { Hearts, Spades, Diamonds, Clubs }
[System.Serializable]
public enum rank { Ace, two, three, four, five, six, seven, eight, nine, ten, Jack, Queen, King }