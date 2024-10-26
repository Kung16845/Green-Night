using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemData : MonoBehaviour
{
    public TextMeshProUGUI count;
    public int idItem;
    public SlotType slotType;
    public SlotType slotTypeParent;
    public Image itemIconSprite; // Keep this as Image because it's a UI element

    public void UpdateDataUI(ItemClass itemClass)
    {
        int countItem = itemClass.quantityItem;

        if (itemIconSprite != null && itemClass.IconSprite != null)
        {
            // Assign the sprite from itemClass.IconSprite to itemIconSprite
            itemIconSprite.sprite = itemClass.IconSprite.sprite;
        }

        if (slotTypeParent == SlotType.SlotBoxes)
        {
            count.text = countItem.ToString();
        }
        else
        {
            count.text = countItem.ToString() + "/" + itemClass.maxCountItem.ToString();
        }
    }
}
