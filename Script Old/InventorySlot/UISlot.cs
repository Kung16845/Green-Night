using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    public int iDItems;
    public Image itemimage;
    public List<Sprite> sprites = new List<Sprite>();
    public void SetDataUI(DataItems dataItems)
    {
        Debug.Log("Set Data");
        Debug.Log("Name Items : " + dataItems.iDItems + " NameImage : " + dataItems.nameImage);

        iDItems = dataItems.iDItems;
        if (itemimage == null)
        {
            Debug.LogError("itemimage is not assigned.");
            return;
        }
        if (iDItems == 1)
        {
            if (sprites.Count > 0 && sprites[0] != null)
            {
                itemimage.sprite = sprites.ElementAt(0);
                Debug.Log(itemimage.sprite.name);
            }
            else
            {
                Debug.LogWarning("Sprite for Poison smoke not found.");
            }
        }
        else if (iDItems == 2)
        {
            if (sprites.Count > 1 && sprites[1] != null)
            {
                itemimage.sprite = sprites.ElementAt(1);
            }
            else
            {
                Debug.LogWarning("Sprite for Granade not found.");
            }
        }
        else if (iDItems == 3)
        {
            if (sprites.Count > 2 && sprites[2] != null)
            {
                itemimage.sprite = sprites.ElementAt(2);
            }
            else
            {
                Debug.LogWarning("Sprite for Molotov not found.");
            }
        }
        else if (iDItems == 4)
        {
            if (sprites.Count > 0 && sprites[3] != null)
            {
                itemimage.sprite = sprites.ElementAt(3);
            }
            else
            {
                Debug.LogWarning("Sprite for Tool Items not found.");
            }
        }
    }
    public void RemoveDataUI()
    {
        iDItems = 0;
        itemimage.sprite = null;
    }
}
