using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class cardHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private cardData cardInfo;
    [SerializeField] private cardSprites cardSpritesScript;
    [SerializeField] private GameObject cardVisPrefab, visualHandler, cardVis, selectedParent;
    private GameObject visualHandRow;

    [SerializeField] private Selectable selectionLogic;
    private cardAnimHandler cardAnimHandlerScript;

    private bool isHovering, isHeld, canShake = true, beingChained;
    [SerializeField] private bool faceUp = true, freeMode = false, covered, inWaste, inBuild;
    [SerializeField] private int assignedRow;
    [SerializeField] private Image cardImageRaycast;
    private int chainLevel;
    private bool hasWon, isMobile = false;

    private soundHandler soundHandlerScript;

    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            isMobile = true;
        }
    }
    public void spawn(cardData cardVals, int row, Transform spawnPos, bool face)
    {
        cardInfo = cardVals;
        assignedRow = row;
        faceUp = face;
        visualHandRow = GameObject.FindWithTag("row"+assignedRow.ToString());
        selectedParent = GameObject.FindWithTag("selectedHolder");
        visualHandler = GameObject.FindWithTag("visualHandler");
        cardSpritesScript = visualHandler.GetComponent<cardSprites>(); // get manager script

        GameObject soundHandlerObj = GameObject.FindWithTag("soundPlayer");
        soundHandlerScript = soundHandlerObj.GetComponent<soundHandler>(); // get manager script


        spawnVisual(spawnPos);
    }
    public void callChaining(int level)
    {
        beingChained = true;
        chainLevel = level;
        cardAnimHandlerScript.callChain(level);
    }
    public void stopChaining()
    {
        beingChained = false;
        chainLevel = 0;
        cardAnimHandlerScript.callChain(-1);

    }
    public void setCovered(bool state)
    {
        covered = state;
    }
    public void setInBuild(bool state)
    {
        inBuild = state;
    }
    public bool checkInBuild()
    {
        return inBuild;
    }
    public void updateRow(int row)
    {
        assignedRow = row;
        visualHandRow = GameObject.FindWithTag("row" + assignedRow.ToString());
    }
    void spawnVisual(Transform spawnPos)
    {
        cardData card = getCardData();
        cardVis = Instantiate(cardVisPrefab, spawnPos.position, Quaternion.identity, visualHandler.transform);
        cardVisualLoader spriteSet = cardVis.GetComponent<cardVisualLoader>(); // get manager script
        cardVisMovement cardVisMovementScript = cardVis.GetComponent<cardVisMovement>(); // get manager script
        selectionLogic.targetGraphic = spriteSet.getTargGraphic();
        cardAnimHandlerScript = cardVis.GetComponent<cardAnimHandler>();

        cardVisMovementScript.setFollowPoint(this.transform);
        spriteSet.setSprite(cardSpritesScript.getSprite(card));
    }
    public cardData getCardData()
    {
        return cardInfo;
    }
    public void flipCard()
    {
        faceUp = true;
    }
    void FixedUpdate()
    {

        if (beingChained)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y - (chainLevel*35), 0);
            transform.position = mousePos;
        }
        else
        if (isHeld)
        {
            cardVis.transform.SetParent(selectedParent.transform);
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0) ;
            transform.position = mousePos;
        }
        else
        {
            cardVis.transform.SetParent(visualHandRow.transform);

        }
    }
    public bool checkFaceUp()
    {
        return faceUp;
    }
    public void callWin()
    {
        hasWon = true;
    }
    bool calledWin = false;
    void Update()
    {
        if (hasWon)
        {
            isHeld = false;
            if (!calledWin)
            {
                cardAnimHandlerScript.callDrag();

            }
            calledWin = true;
            return;
        }
        if (!freeMode && !isHeld && !beingChained || !faceUp)
        {
            transform.localPosition = Vector3.zero;
        }
        
        if (Input.GetButton("Fire1") && faceUp && !covered)
        {
            cardAnimHandlerScript.endHover();
            if (EventSystem.current.currentSelectedGameObject == this.gameObject)
            {
                cardAnimHandlerScript.callDrag();
                isHeld = true;
            }
            
        }
        else
        {
            if (!isHovering)
            {
                cardAnimHandlerScript.callIdle();

            }

            isHeld = false;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (EventSystem.current.currentSelectedGameObject == this.gameObject && faceUp && !covered)
            {
                cardAnimHandlerScript.callHover();
            }

        }

        cardAnimHandlerScript.faceUpCard(faceUp);
        if (covered)
        {
            cardImageRaycast.raycastTarget = false;

        }
        else
        {
            cardImageRaycast.raycastTarget = true;
        }
        
    }
    public bool checkWaste()
    {
        return inWaste;
    }
    public void setAsWaste()
    {
        inWaste = true;
    }
    public void clearAsWaste()
    {
        inWaste = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (canShake && !Input.GetButton("Fire1") && faceUp && !covered)
        {
            cardAnimHandlerScript.callHover();
            if (!isMobile)
            {
                soundHandlerScript.callSound(3);
            }
            
            canShake = false;
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (!isHeld)
        {
            cardAnimHandlerScript.endHover();

            canShake = true;
        }
    }
    void OnDestroy()
    {
        Destroy(cardVis);
    }
}
