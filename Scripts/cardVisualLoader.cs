using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cardVisualLoader : MonoBehaviour
{
    [SerializeField] private cardSprites cardSpritesScript;
    [SerializeField] private cardHandler cardHandlerScript;
    [SerializeField] private Image spriteRend;

    // Start is called before the first frame update
    public void setSprite(Sprite sprite)
    {
        spriteRend.sprite = sprite;
    }
    public Image getTargGraphic()
    {
        return spriteRend;
    }

}
