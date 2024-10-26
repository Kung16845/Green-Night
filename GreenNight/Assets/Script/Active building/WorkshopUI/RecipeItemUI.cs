using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeItemUI : MonoBehaviour
{
    public Image itemIconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI amountText;

    public void Initialize(RecipeItem recipeItemData, int amountHave, Sprite itemIcon)
    {
        if (itemIconImage != null)
        {
            if (itemIcon != null)
            {
                itemIconImage.sprite = itemIcon;
                Debug.Log($"Item icon set for {recipeItemData.itemName} with sprite {itemIcon.name}");
            }
            else
            {
                Debug.LogWarning($"Item icon is null for item: {recipeItemData.itemName}");
            }
        }
        else
        {
            Debug.LogWarning("itemIconImage is null in RecipeItemUI");
        }

        if (itemNameText != null)
            itemNameText.text = recipeItemData.itemName;

        if (amountText != null)
            amountText.text = $"{amountHave} / {recipeItemData.amountNeeded}";
    }

}
