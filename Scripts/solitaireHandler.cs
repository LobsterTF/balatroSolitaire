using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class solitaireHandler : MonoBehaviour
{
    [SerializeField] private int[] solitaireSpotValues = new int[175]; // -1 is empty
    private GameObject[] cardSpots = new GameObject[175];
    [SerializeField] private List<int> deckOrder = new List<int>();
    private int wasteDepth;

    [SerializeField] private GameObject[] columnHolders;
    [SerializeField] private GameObject card, winBouncy, mainCanvas;
    [SerializeField] private Transform spawnPosition, stockVisual;

    [SerializeField] private int heldCardValue = -1, selectedColumn;
    [SerializeField] private GameObject currentHeldCardObj;
    [SerializeField] private List<GameObject> trailingHeldCards = new List<GameObject>();
    private List<int> trailingCardVals = new List<int>();
    private bool holdingCard, wasteCard, buildCard;
    private Vector3 mousePosOffset = new Vector3(-420, 0, 0);

    [SerializeField] private Transform[] wasteSpots;

    [SerializeField] private wasteAvailibilityChecker wasteAvailibilityCheckerScript;
    [SerializeField] private int[] buildValues = new int[4]; // -1 is empty
    [SerializeField] private Transform[] buildPositions = new Transform[4];

    private float screenWidth;
    private int revealedCards, moveCount;

    [SerializeField] private TMP_Text stockCountTxt, columnText, moveCounterText;
    [SerializeField] private float buildZoneDiv, columnZoneDiv;

    [SerializeField] private soundHandler soundHandlerScript;


    private bool hasWon;

    void Start()
    {
        fillDeck();
        shuffleDeck();
        assignCardSpots();
        debugPopulate();
        spawnCards();
    }
    
    void Update()
    {
        if (hasWon) { return; }
        
        screenWidth = Screen.width;
        if (Input.GetButtonDown("Fire1"))
        {
            
            getSelectedCard();
            holdingCard = true;

        }


        if (Input.GetButtonUp("Fire1"))
        {
            
            if (holdingCard && selectedColumn != -1)
            {
                placeCard();
            }else if (selectedColumn == -1 && trailingHeldCards.Count > 0)
            {
                dismissChain();
            }else if (selectedColumn == -1 && trailingHeldCards.Count == 0)
            {
                checkBuild();
            }
            dismissChain();
            holdingCard = false;
            heldCardValue = -1;
            checkWin();
        }
        if (holdingCard) // inrements of 115... use screen resolution
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            columnText.text = "Column: " + selectedColumn + ", mousePos: " + mousePos; 
            float buildZone = screenWidth / buildZoneDiv;
            float columnZone = screenWidth / columnZoneDiv;
            if (mousePos.x >= (columnZone * 7)+buildZone)
            {
                selectedColumn = -1;
            }else if (mousePos.x >= (columnZone * 6) + buildZone)
            {
                selectedColumn = 6;
            }else if (mousePos.x >= (columnZone * 5) + buildZone)
            {
                selectedColumn = 5;
            }else if (mousePos.x >= (columnZone * 4) + buildZone)
            {
                selectedColumn = 4;
            }else if (mousePos.x >= (columnZone * 3) + buildZone)
            {
                selectedColumn = 3;
            }else if (mousePos.x >= (columnZone * 2) + buildZone)
            {
                selectedColumn = 2;
            }else if (mousePos.x >= (columnZone * 1) + buildZone)
            {
                selectedColumn = 1;
            }else if (mousePos.x >= buildZone)
            {
                selectedColumn = 0;
            }
            if (mousePos.x < buildZone)
            {
                selectedColumn = -1;
            }
        }
    }
    void checkWin()
    {
        if (deckOrder.Count <= 0 || deckOrder.Count - wasteDepth <= 0) // if stock is empty
        {
            if (revealedCards > 20)
            {
                winSequence();
            }
        }
        else
        {
            return;
        }
    }
    void winSequence()
    {
        hasWon = true;
        GameObject[] cards = GameObject.FindGameObjectsWithTag("cardHolder");
        List<cardHandler> cardHandlerScripts = new List<cardHandler>();
        soundHandlerScript.callSound(1);


        for (int w = 0; w < cards.Length; w++)
        {
            cardHandler cardHandlerScript = cards[w].GetComponent<cardHandler>();

            cardHandlerScripts.Add(cardHandlerScript);
        }
        

        for (int i = 0; i < cards.Length; i++)
        {
            cardData cardDat = cardHandlerScripts[i].getCardData();
            for (int j = 0; j < buildPositions.Length; j++)
            {
                
                if ((int)cardDat.cardSuit == j)
                {
                    
                    StartCoroutine(moveDelay(cards[i], j, 1 + i));
                    cardHandlerScripts[i].callWin();
                    if ((int)cardDat.cardRank == 12 || (int)cardDat.cardRank == 25 || (int)cardDat.cardRank == 38 || (int)cardDat.cardRank == 51)
                    {
                        cardHandlerScripts[i].updateRow(13);
                    }
                }
            }
        }
        StartCoroutine(bounceCards(cards));

    }
    IEnumerator moveDelay(GameObject card, int j, float time)
    {
        yield return new WaitForSeconds(time/10);
        soundHandlerScript.callCardSound();

        card.transform.SetParent(buildPositions[j], false);

    }
    IEnumerator bounceCards(GameObject[] cards)
    {
        yield return new WaitForSeconds(7);
        GameObject bg = GameObject.FindGameObjectWithTag("backGround");
        bg.SetActive(false);
        /*GameObject bg = GameObject.FindGameObjectWithTag("backGround");
        bg.SetActive(false);*/
        for (int w = 0; w < cards.Length; w++)
        {
            GameObject card = cards[w];
            StartCoroutine(bounceDelay(card, w));



        }

    }
    IEnumerator bounceDelay(GameObject card, float time)
    {
        yield return new WaitForSeconds(time / 10);
        soundHandlerScript.callCardSound();

        GameObject bouncy = Instantiate(winBouncy, card.transform.position, Quaternion.identity, mainCanvas.transform);
        card.transform.SetParent(bouncy.transform, false);

    }
    void checkBuild()
    {
        cardData selectedCard = constructCard(heldCardValue);
        cardHandler cardHandlerScript = currentHeldCardObj.GetComponent<cardHandler>();
        if (!cardHandlerScript.checkFaceUp())
        {
            return;
        }
        if ((int)selectedCard.cardRank == buildValues[(int)selectedCard.cardSuit] + 1)
        {
            soundHandlerScript.callCardSound();

            buildValues[(int)selectedCard.cardSuit]++;
            for (int i = 0; i < solitaireSpotValues.Length; i++) // remove old spot
            {
                if (solitaireSpotValues[i] == heldCardValue)
                {
                    solitaireSpotValues[i] = -1;
                    
                    break;
                }

            }
            if (wasteCard)
            {
                for (int w = 0; w < deckOrder.Count; w++)
                {
                    if (heldCardValue == deckOrder[w])
                    {
                        deckOrder[w] = -1;
                        wasteAvailibilityCheckerScript.usedWasteCard();
                        wasteCard = false;
                        cardHandlerScript.clearAsWaste();
                        cardHandlerScript.setCovered(false);
                        break;
                    }
                }
            }
            cardHandlerScript.setInBuild(true);
            cardHandlerScript.updateRow((int)selectedCard.cardRank);
            currentHeldCardObj.transform.SetParent(buildPositions[(int)selectedCard.cardSuit], false);
            incrementMoveCounter();

        }

    }
    void dismissChain()
    {
        for (int i = 0; i < trailingHeldCards.Count; i++)
        {
            cardHandler cardHandlerScriptChain = trailingHeldCards[i].GetComponent<cardHandler>();
            cardHandlerScriptChain.stopChaining();
        }
        trailingHeldCards.Clear();
        trailingCardVals.Clear();
    }
    void placeCard()
    {
        cardHandler cardHandlerScript = currentHeldCardObj.GetComponent<cardHandler>();
        cardData placingCard = cardHandlerScript.getCardData();

        if (!cardHandlerScript.checkFaceUp()) // if face down
        {
            for (int j = 0; j < 25; j++)
            {
                if (solitaireSpotValues[j + (selectedColumn * 25)] == heldCardValue)
                {
                    if (solitaireSpotValues[j+1 + (selectedColumn * 25)] == -1)
                    {
                        cardHandlerScript.flipCard();
                        soundHandlerScript.callCardSound();

                        revealedCards++;
                    }
                        break;
                }
            }
            

            return;
        }
        for (int j = 0; j < 25; j++) // check validity
        {
            if (solitaireSpotValues[j + (selectedColumn * 25)] == -1)
            {
                cardData aboveCard = new cardData();
                if (j != 0)
                {
                    aboveCard = constructCard(solitaireSpotValues[j - 1 + (selectedColumn * 25)]);

                }
                if (j == 0 && (heldCardValue == 12 || heldCardValue == 25 || heldCardValue == 38|| heldCardValue == 51))
                {
                    // idk have a good day
                }
                else
                {
                    if ((int)placingCard.cardRank != (int)aboveCard.cardRank - 1) // check rank
                    {
                        return;
                    }
                    if ((int)placingCard.cardSuit == (int)aboveCard.cardSuit) // check suit
                    {
                        return;
                    }
                    if (((int)placingCard.cardSuit + (int)aboveCard.cardSuit == 2 || (int)placingCard.cardSuit + (int)aboveCard.cardSuit == 4)) // suit 2
                    {
                        return;
                    }
                }
                
                
                break;
            }
        }
        for (int i = 0; i < solitaireSpotValues.Length; i++) // remove old spot
        {
            if (solitaireSpotValues[i] == heldCardValue)
            {
                solitaireSpotValues[i] = -1;
                if (trailingCardVals.Count > 0)
                {
                    for (int j = 0; j < trailingCardVals.Count; j++)
                    {
                        solitaireSpotValues[i + j+1] = -1;
                    }
                }
                break;
            }
            
        }
        if ((int)placingCard.cardRank == buildValues[(int)placingCard.cardSuit] && buildCard)
        {
            buildValues[(int)placingCard.cardSuit]--;
            cardHandlerScript.setInBuild(false);
            buildCard = false;
        }
        for (int j = 0; j < 25; j++) // place card
        {
            if (solitaireSpotValues[j + (selectedColumn * 25)] == -1)
            {
                soundHandlerScript.callCardSound();

                solitaireSpotValues[j + (selectedColumn * 25)] = heldCardValue;
                currentHeldCardObj.transform.SetParent(cardSpots[j + (selectedColumn * 25)].transform, false);
                
                cardHandlerScript.updateRow(j);
                if (trailingCardVals.Count > 0)
                {
                    for (int w = 0; w < trailingCardVals.Count; w++)
                    {
                        solitaireSpotValues[(j + (selectedColumn * 25)) + w+1] = trailingCardVals[w];
                        trailingHeldCards[w].transform.SetParent(cardSpots[j + (selectedColumn * 25)+w+1].transform, false);
                        cardHandler cardHandlerScriptTrail = trailingHeldCards[w].GetComponent<cardHandler>();
                        cardHandlerScriptTrail.updateRow(j + w+1);
                    }
                }
                if (wasteCard)
                {
                    for (int w = 0; w < deckOrder.Count; w++)
                    {
                        if (heldCardValue == deckOrder[w])
                        {
                            deckOrder[w] = -1;
                            wasteAvailibilityCheckerScript.usedWasteCard();
                            cardHandlerScript.clearAsWaste();

                            wasteCard = false;
                            break;
                        }
                    }
                }
                incrementMoveCounter();

                break;
            }
        }
    }
    void incrementMoveCounter()
    {
        moveCount++;
        moveCounterText.text = moveCount.ToString();
    }
    void getSelectedCard()
    {
        currentHeldCardObj = EventSystem.current.currentSelectedGameObject;

        cardHandler cardHandlerScript = currentHeldCardObj.GetComponent<cardHandler>(); // issue for waste. currentHeldCardObj is null

        cardData cardInfo = cardHandlerScript.getCardData();

        if (cardHandlerScript.checkFaceUp())
        {
            soundHandlerScript.callSound(2);

        }

        //cardHandlerScript.flipCard();
        heldCardValue = cardToInt(cardInfo);
        if (cardHandlerScript.checkWaste())
        {
            Debug.Log("inWaste");
            wasteCard = true;
            
        }
        if (cardHandlerScript.checkInBuild())
        {
            buildCard = true;
            return;
        }
        for (int i = 0; i < solitaireSpotValues.Length; i++) // check if card chaining
        {
            if (solitaireSpotValues[i] == heldCardValue)
            {
                if (solitaireSpotValues[i+1] >= 0)
                {
                    if (!cardHandlerScript.checkFaceUp()) // wont chain using a facedown card
                    {
                        currentHeldCardObj = null;
                        holdingCard = false;
                        return;
                    }
                    Debug.Log("chain");
                    for (int j = 1;j < 25; j++)
                    {
                        
                        if (solitaireSpotValues[i + j] >= 0) // collect all cards below selected card
                        {
                            trailingHeldCards.Add(cardSpots[i + j].transform.GetChild(0).gameObject);
                            if (i+j % 25 == 0)
                            {
                                break;
                            }
                        }
                        else // stop if empty
                        {
                            break;
                        }
                        if ((i + j) % 25 == 0) // stop if at end of column
                        {
                            break;
                        }
                    }
                }
            }


        }
        if (trailingHeldCards.Count > 0) // let the cards know they're being chained and store their values
        {
            for (int i = 0; i < trailingHeldCards.Count; i++)
            {
                cardHandler cardHandlerScriptChain = trailingHeldCards[i].GetComponent<cardHandler>();
                cardHandlerScriptChain.callChaining(i + 1);
                trailingCardVals.Add(cardToInt(cardHandlerScriptChain.getCardData()));
            }
        }
    }
    void fillDeck() // add the cards to the deck... not shuffled
    {
        for (int i = 0; i < 52; i++)
        {
            deckOrder.Add(i);
        }
    }
    void shuffleDeck() // shuffle the cards
    {
        
        int n = deckOrder.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n - 1);
            int cardVal = deckOrder[k];
            deckOrder[k] = deckOrder[n];
            deckOrder[n] = cardVal;
        }
    }
    void assignCardSpots() // get the spots to put the cards
    {
        for (int i = 0; i < columnHolders.Length; i++)
        {
            if (columnHolders[i] == null)
            {
                break;
            }

            for (int j = 0; j < 25; j++)
            {
                string spotTag = "spot" + j.ToString();
                GameObject assignSpot = null;
                foreach (Transform tr in columnHolders[i].transform)
                {
                    if (tr.tag == spotTag)
                    {
                        assignSpot = tr.gameObject;
                    }
                }
                cardSpots[j + (25 * i)] = assignSpot;
            }
            
        }
    }
    void debugPopulate() // assign the values
    {
       
        for (int i = 0; i < solitaireSpotValues.Length; i++)
        {
            solitaireSpotValues[i] = -1;
        }
        for (int i = 0; i < buildValues.Length; i++)
        {
            buildValues[i] = -1;
        }
        int column = 1;
        int row = 0;
        for (int i = 0; i < solitaireSpotValues.Length; i++)
        {
            
            if (row >= column) // if card count in column is equal/greater than column 
            {
                row = 0;
                
                if (25 * column >= 175)
                {
                    return;
                }
                i = 25 * column; // go to next column
                column++;
            }
            int cardSel = deckOrder[0];

            
            solitaireSpotValues[i] = cardSel;
            deckOrder.RemoveAt(0);

            row++;
        }
    }
    void spawnCards() // spawn the cards using the values from the populate function
    {
        int time = 0;
        for (int i = 0; i < cardSpots.Length; i++)
        {
            if (solitaireSpotValues[i] == -1)
            {
                continue;
            }
            else
            {
                time++;
            }
            StartCoroutine(spawnDelay(i, time));
            
        }

    }
    IEnumerator spawnDelay(int i, int time)
    {
        yield return new WaitForSeconds(time / 9f);
        GameObject cardObj = Instantiate(card, spawnPosition.position, Quaternion.identity, cardSpots[i].transform);
        cardHandler cardHandlerScript = cardObj.GetComponent<cardHandler>();
        cardData cardValues = constructCard(solitaireSpotValues[i]);
        bool faceUp = false;
        if (solitaireSpotValues[i + 1] == -1) { faceUp = true; }
        int row = (i % 25);
        cardHandlerScript.spawn(cardValues, row, spawnPosition, faceUp);

        soundHandlerScript.callCardSound();
    }
    public void callDrawFromStock()
    {
        Debug.Log("draw");
        if (hasWon) { return; }
        incrementMoveCounter();

        int deckSize = deckOrder.Count;

        if (wasteDepth >= deckSize)
        {
            wasteDepth = 0;
            soundHandlerScript.callSound(0);

            for (int i = deckSize - 1; i >= 0; i--)
            {
                if (deckOrder[i] == -1)
                {
                    deckOrder.RemoveAt(i);
                    
                }
            }
            for (int i = 0; i < 3; i++)
            {
                foreach (Transform child in wasteSpots[i].transform)
                {
                    Destroy(child.gameObject);
                }
            }
            stockCountTxt.text = (deckOrder.Count - wasteDepth).ToString() + "/" + deckOrder.Count;

        }
        else
        {
            int drawSize = 2;
            for (int i = 0; i < 3; i++)
            {
                
                if (wasteDepth + i >= deckSize)
                {
                    drawSize -= 1;
                    continue;
                }
                
                GameObject cardObj = Instantiate(card, spawnPosition.position, Quaternion.identity, wasteSpots[i]);
                cardHandler cardHandlerScript = cardObj.GetComponent<cardHandler>();
                cardData cardValues = constructCard(deckOrder[wasteDepth + i]);
                cardHandlerScript.spawn(cardValues, 0, spawnPosition, true);
                cardHandlerScript.setAsWaste();
            }

            wasteAvailibilityCheckerScript.callDraw(drawSize);
            soundHandlerScript.callCardSound();

            wasteDepth += 3;
            stockCountTxt.text = (deckOrder.Count - wasteDepth).ToString() + "/" + deckOrder.Count;
            if (deckOrder.Count - wasteDepth < 0)
            {
                stockCountTxt.text = 0 + "/" + deckOrder.Count;
            }
        }
        int index = 0;
        foreach (Transform child in stockVisual.transform)
        {
            if (index < deckOrder.Count - wasteDepth)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
            index++;
        }
        

    }
    private cardData constructCard(int value) // convert the card value to cardData
    {
        cardData card = new cardData();
        int rankVal = value % 13;
        int suitVal = (int)Mathf.Round(value / 13);
        card.cardRank = (rank)rankVal;
        card.cardSuit = (suit)suitVal;
        return card;
    }
    private int cardToInt(cardData card)
    {
        int val = (int)(card.cardRank);
        // get suit
        val += ((int)(card.cardSuit)) * 13;
        return val;
    }
}
