using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragCard : MonoBehaviour, ISelectHandler
{
    [SerializeField] private GameObject selectedCard;
    public void OnSelect(BaseEventData eventData)
    {
        selectedCard = EventSystem.current.currentSelectedGameObject;
        
    }
    // Update is called once per frame
    void Update()
    {
        /*if (selectedCard != null)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            selectedCard.transform.position = mousePos;
        }*/
    }
}
