using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] RectTransform arrowParent;
    [SerializeField] ItemSlot[] itemSlots;
#pragma warning restore 0649

    public GameObject notEnough;

    public ItemContainer itemContainer;

    private CraftingRecipe craftingRecipe;
    public CraftingRecipe CraftingRecipe
    {
        get { return craftingRecipe; }
        set { SetCraftingRecipe(value); }
    }

    private void OnValidate()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>(includeInactive: true);
    }

    public void OnCraftButtonClick()
    {
        if(craftingRecipe != null && itemContainer != null)
        {
            if(craftingRecipe.CanCraft(itemContainer))
            {
                if (!itemContainer.IsFull())
                {
                    craftingRecipe.Craft(itemContainer);
                }
                else
                {
                    Debug.Log("inventory is full.");
                    
                }
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("CantCraft"); //CANT CRAFT SOUND
                StartCoroutine(NotEnoughMaterials());
            }
        }
    }

    private void SetCraftingRecipe(CraftingRecipe newCraftingRecipe)
    {
        craftingRecipe = newCraftingRecipe;

        if(craftingRecipe != null)
        {
            int slotIndex = 0;
            slotIndex = SetSlots(craftingRecipe.Materials, slotIndex);
            arrowParent.SetSiblingIndex(slotIndex);
            slotIndex = SetSlots(craftingRecipe.Results, slotIndex);

            for (int i = slotIndex; i < itemSlots.Length; i++)
            {
                itemSlots[i].transform.parent.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private int SetSlots(IList<ItemAmount> itemAmountList, int slotIndex)
    {
        for (int i = 0; i < itemAmountList.Count; i++, slotIndex++)
        {
            ItemAmount itemAmount = itemAmountList[i];
            ItemSlot itemSlot = itemSlots[slotIndex];

            itemSlot.Item = itemAmount.Item;
            itemSlot.Amount = itemAmount.Amount;
            itemSlot.transform.parent.gameObject.SetActive(true);
        }
        return slotIndex;
    }

    IEnumerator NotEnoughMaterials()
    {
        notEnough.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        notEnough.SetActive(false);
        StopCoroutine(NotEnoughMaterials());
    }

}
