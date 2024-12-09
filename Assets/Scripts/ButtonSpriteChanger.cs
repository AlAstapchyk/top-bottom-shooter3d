using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSpriteChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buttonImage;
    public Sprite normalSprite;
    public Sprite hoverSprite;

    private void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (normalSprite != null)
            buttonImage.sprite = normalSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSprite != null)
            buttonImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (normalSprite != null)
            buttonImage.sprite = normalSprite;
    }
}
