using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class stockHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private solitaireHandler solitaireHandlerScript;
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        solitaireHandlerScript.callDrawFromStock();

    }

}
