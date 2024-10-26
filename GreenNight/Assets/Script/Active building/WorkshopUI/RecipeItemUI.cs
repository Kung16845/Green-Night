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
            itemIconImage.sprite = itemIcon;

        if (itemNameText != null)
            itemNameText.text = recipeItemData.itemName;

        if (amountText != null)
            amountText.text = $"Amount: {amountHave} / {recipeItemData.amountNeeded}";
    }
}
