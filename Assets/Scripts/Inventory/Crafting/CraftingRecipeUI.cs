using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeUI : MonoBehaviour
{
    [SerializeField] RectTransform arrowParent;
    [SerializeField] ItemSlot[] itemSlots;

    //public GameObject notEnough;
    public Animator anim;

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
        
        anim.SetBool("isDisplayed", true);
        yield return new WaitForSecondsRealtime(4f);
        anim.SetBool("isDisplayed", false); //this is not being set to false all of a sudden?
        StopCoroutine(NotEnoughMaterials());
    }

}
