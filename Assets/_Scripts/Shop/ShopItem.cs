//Copyright 2020 Placeholder Gameworks
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
//and associated documentation files (the "Software"), to deal in the Software without restriction, 
//including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
//subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
//FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.
using Articy.Project_Of_Death;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class ShopItem : MonoBehaviour, Interactable
{
    [SerializeField]
    public SpriteRenderer Renderer;

    [SerializeField]
    Collider2D Collider;

    public template_item ItemData;

    public string GetHoverText()
    {
        if (ItemData != null)
        {
            return ItemData.Template.item_data.item_description;

            List<string> flavourStrings = new List<string>();
            if (ItemData.Template.item_data.item_flavour_text_first != "")
            {
                flavourStrings.Add(ItemData.Template.item_data.item_flavour_text_first);
            }
            if (ItemData.Template.item_data.item_flavour_text_second != "")
            {
                flavourStrings.Add(ItemData.Template.item_data.item_flavour_text_second);
            }
            if (ItemData.Template.item_data.item_flavour_text_third != "")
            {
                flavourStrings.Add(ItemData.Template.item_data.item_flavour_text_third);
            }
            if (flavourStrings.Count > 0)
            {
                return flavourStrings[Random.Range(0, flavourStrings.Count)];
            }
            else
            {
                return "No flavour text available";
            }
        }
        else
        {
            return "There's nothing there..";
        }
    }

    public int GetPrice()
    {
        if(ItemData == ShopManager.instance.EraserTemplate)
        {
            return Mathf.RoundToInt(Mathf.Clamp(ItemData.Template.item_data.item_price_value * SaveManager.instance.GetCurrentCarryoverPlayerState().EraserBuyCount, ItemData.Template.item_data.item_price_value, 2000));
        }
        else
        {
            return Mathf.RoundToInt(ItemData.Template.item_data.item_price_value);
        }
    }

    public void Interact()
    {
        //        if (!SaveManager.instance.GetCurrentCarryoverPlayerState().IsItemOwnedByID(ItemData.Id))

        if (!ItemData.Template.item_data.item_variable.CallScript())
        {
            
            if (SaveManager.instance.GetCurrentPlayerState().CanAfford(GetPrice()))
            {
                BuyConfirm.instance.ShowForItem(this);
                //if(ItemData.Template.item_data.item_type_category == item_type_category.ItemVisualAccessory || ItemData == ShopManager.instance.MirrorTemplate)
                //{
                //    SaveManager.instance.GetCurrentPlayerState().SetHasMirrorShopNotification(true);
                //}
                //else
                //{
                //    SaveManager.instance.GetCurrentPlayerState().SetHasOfficeShopNotification(true);
                //}
                //SaveManager.instance.AddItemDataToPlayer(ItemData);

                //SaveManager.instance.GetCurrentPlayerState().ModifyMoney(-Mathf.RoundToInt(ItemData.Template.item_data.item_price_value));
                //ShopManager.instance.RemoveSpawnedItem(this, false);

                //SpeechBubbleManager.instance.StartBubbleCascadeFromDialogue(GetItemQuip());
            }
        }

    }

    public void Hover()
    {
        Shop.instance.SetPriceText(Mathf.RoundToInt(GetPrice()));
        Shop.instance.SetNameText(ItemData.Template.item_data.item_name, ItemData.Template.item_data.item_type_category);
    }

    public void Unhover()
    {
        Shop.instance.SetPriceText(0);
        Shop.instance.SetNameText("");
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (ItemData != null)
        //{
        //    if (SaveManager.instance.GetCurrentPlayerState().CanAfford(Mathf.RoundToInt(ItemData.Template.item_data.item_price_value)))
        //    {
        //        //TextPrice.color = Color.green;
        //    }
        //    else
        //    {
        //        //TextPrice.color = Color.red;
        //    }
        //}
    }

    public void InitItemFromData(template_item itemData)
    {
        ItemData = itemData;

        if (ItemData != null)
        {
            Asset iconAsset = ItemData.Template.item_data.item_icon as Asset;

            if (iconAsset != null && iconAsset != null)
            {
                Renderer.sprite = iconAsset.LoadAssetAsSprite();
                // scale uniformly (or something?)
                //var bounds = Renderer.sprite.bounds;
                //var factor = 3 / bounds.size.y;
                //gameObject.transform.localScale = new Vector3(factor, factor, factor);
            }
            //TextPrice.text = ItemData.Template.item_data.item_price_value.ToString("F0");

            if(ItemData == ShopManager.instance.MirrorTemplate)
            {
                Collider.offset = new Vector2(Collider.offset.x, -1.69f);
            }
            Renderer.sortingOrder = ItemData.Template.item_data.item_slot_number == 1 ? 2 : 3;

        }
    }

    public Dialogue GetItemQuip()
    {
        return ItemData.Template.item_data.item_dialogue as Dialogue;
    }

    public bool CanDrag()
    {
        return false;
    }

    public bool IsDragging()
    {
        return false;
    }

    public void ToggleDragging(bool drag)
    {

    }

    public void UpdateDragGrabPosition(Vector3 position)
    {
    }
}
