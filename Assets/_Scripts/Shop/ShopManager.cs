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
using Articy.Project_Of_Death.GlobalVariables;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : ManagerBase
{
    public static ShopManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private ArticyRef ItemFolderReference;

    [SerializeField]
    private ArticyRef StarterHeadReference;

    [SerializeField]
    private ArticyRef StarterBodyReference;

    [SerializeField]
    private ArticyRef ShopKeeperAltGreetReference;

    [SerializeField]
    private ArticyRef ShopKeeperHelloDialogueReference;

    [SerializeField]
    private ArticyRef MirrorReference;

    [SerializeField]
    private ArticyRef EraserReference;

    public template_item MirrorTemplate;
    public template_item EraserTemplate;

    Dictionary<int, List<template_item>> ItemDictionary = new Dictionary<int, List<template_item>>();

    List<template_item_variation> HeadList = new List<template_item_variation>();
    List<template_item_variation> BodyList = new List<template_item_variation>();

    List<template_item> BaseHeadList = new List<template_item>();
    List<template_item> BaseBodyList = new List<template_item>();

    [SerializeField]
    ShopItem ShopItemTemplate;

    List<ShopItem> SpawnedItems = new List<ShopItem>();

    List<DialogueFragment> AltGreetFragments = new List<DialogueFragment>();

    template_item StarterHead;
    template_item StarterBody;

    public void StartShopKeeperHello()
    {
        if (!ArticyGlobalVariables.Default.shop.keeper_hello_done)
        {
            Dialogue hello = ShopKeeperHelloDialogueReference.GetObject<Dialogue>();

            //ArticyGlobalVariables.Default.shop.keeper_hello_done = true;

            SpeechBubbleManager.instance.StartBubbleCascadeFromDialogue(hello);

            SaveManager.instance.GetCurrentCarryoverPlayerState().AddShopKeeperHello(0);
        }
        else
        {
            if (!SaveManager.instance.GetCurrentPlayerState().HasVisitedShopkeeperToday())
            {
                SpeechBubbleManager.instance.StartSingleBubble(AltGreetFragments[SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex()]);
                SaveManager.instance.GetCurrentCarryoverPlayerState().AddShopKeeperHello(SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex());
            }
        }
        SaveManager.instance.GetCurrentPlayerState().SetHasVisitedShopkeeperToday(true);
    }

    private void ReadItemData(template_item item)
    {
        if (item.Template.item_data.item_variable.RawScript == "")
        {
            Debug.Log("No item variable for item: " + item.Template.item_data.item_name);
            return;
        }

        if (item.Template.item_data.item_slot_number == -1 && item != GetStarterBody() && item != GetStarterHead())
        {
            Debug.Log("Item set to be not listed in shop: " + item.Template.item_data.item_name);
            return;
        }

        switch (item.Template.item_data.item_type_category)
        {
            case item_type_category.ItemVisualAccessory:
                if (item.Template.item_data.item_variations.Count > 0)
                {
                    int slotNum = item.Template.item_data.item_slot_number;
                    if (item == GetStarterBody() || item == GetStarterHead())
                    {
                        slotNum = 0;
                    }
                    ItemDictionary[slotNum].Add(item);
                }
                else
                {
                    Debug.Log("Skipping visual accessory without variations: " + item.DisplayName);
                }
                for (int k = 0; k < item.Template.item_data.item_variations.Count; ++k)
                {
                    template_item_variation variation = item.Template.item_data.item_variations[k] as template_item_variation;
                    switch (variation.Template.item_accessory_variation.item_accessory_type)
                    {
                        case item_accessory_type.item_head:
                            if (!BaseHeadList.Contains(item))
                            {
                                BaseHeadList.Add(item);
                            }
                            HeadList.Add(variation);
                            break;
                        case item_accessory_type.item_body:
                            if (!BaseBodyList.Contains(item))
                            {
                                BaseBodyList.Add(item);
                            }
                            BodyList.Add(variation);
                            break;
                    }
                }
                //HeadList.Add(item);
                //BodyList.Add(item);
                break;
            case item_type_category.ItemToy:
                ItemDictionary[item.Template.item_data.item_slot_number].Add(item);
                break;
            case item_type_category.ItemInfoTool:
                ItemDictionary[item.Template.item_data.item_slot_number].Add(item);
                break;
        }
    }

    public override void InitManager()
    {
        base.InitManager();
        instance = this;

        if (MirrorReference.HasReference)
        {
            MirrorTemplate = MirrorReference.GetObject<template_item>();
        }
        if (EraserReference.HasReference)
        {
            EraserTemplate = EraserReference.GetObject<template_item>();
        }
        if (ShopKeeperAltGreetReference.HasReference)
        {
            List<SortingHelper> sorts = new List<SortingHelper>();
            Dialogue altGreetDialogue = ShopKeeperAltGreetReference.GetObject<Dialogue>();
            for (int i = 0; i < altGreetDialogue.Children.Count; ++i)
            {
                DialogueFragment frag = altGreetDialogue.Children[i] as DialogueFragment;
                if (frag != null)
                {
                    IObjectWithPosition positionObject = frag as IObjectWithPosition;

                    SortingHelper sort = new SortingHelper();
                    sort.Fragment = frag;
                    sort.PositionY = positionObject.Position.y;
                    sorts.Add(sort);
                }
            }

            sorts.OrderBy(f => f.PositionY);

            for (int i = 0; i < sorts.Count; ++i)
            {
                AltGreetFragments.Add(sorts[i].Fragment);
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            ItemDictionary.Add(i, new List<template_item>());
        }
        if (StarterBodyReference.HasReference)
        {
            StarterBody = StarterBodyReference.GetObject<template_item>();
        }
        if (StarterHeadReference.HasReference)
        {
            StarterHead = StarterHeadReference.GetObject<template_item>();
        }

        if (ItemFolderReference.HasReference)
        {
            UserFolder itemFolder = ItemFolderReference.GetObject<UserFolder>();

            if (itemFolder != null)
            {
                for (int i = 0; i < itemFolder.Children.Count; ++i)
                {
                    UserFolder childFolder = itemFolder.Children[i] as UserFolder;
                    for (int j = 0; j < childFolder.Children.Count; ++j)
                    {
                        template_item item = childFolder.Children[j] as template_item;

                        UserFolder childSubfolder = childFolder.Children[j] as UserFolder;
                        if (childSubfolder != null)
                        {
                            for (int y = 0; y < childSubfolder.Children.Count; ++y)
                            {
                                template_item subfolderItem = childSubfolder.Children[y] as template_item;
                                if (subfolderItem != null)
                                {
                                    ReadItemData(subfolderItem);
                                }
                            }
                        }
                        if (item != null)
                        {
                            ReadItemData(item);
                        }
                    }
                }

            }
            Debug.Log("Found " + ItemDictionary[0].Count + " Slot 0 items");
            Debug.Log("Found " + ItemDictionary[1].Count + " Slot 1 items");
            Debug.Log("Found " + ItemDictionary[2].Count + " Slot 2 items");

            Debug.Log("Found " + HeadList.Count + " heads");
            Debug.Log("Found " + BodyList.Count + " bodies");
        }
    }

    public void NotifyStartDay(bool restoreFromSave)
    {
        ClearShop(restoreFromSave);
        PopulateShop(restoreFromSave);
    }

    public void ClearShop(bool restoreFromSave)
    {
        for (int i = SpawnedItems.Count - 1; i >= 0; --i)
        {
            RemoveSpawnedItem(SpawnedItems[i], restoreFromSave);
        }
    }

    public void RemoveSpawnedItem(ShopItem item, bool restoreFromSave)
    {
        SpawnedItems.Remove(item);
        if (!restoreFromSave)
        {
            SaveManager.instance.GetCurrentPlayerState().RemoveCurrentDayItem(item.ItemData.Id);
        }
        Destroy(item.gameObject);
    }

    private void SpawnItem(template_item rolledItem, Vector3 position)
    {
        ShopItem newItem = Instantiate(ShopItemTemplate);

        newItem.InitItemFromData(rolledItem);
        newItem.transform.SetParent(Shop.instance.GetShelf().gameObject.transform);
        newItem.transform.position = position;

        if (rolledItem.Template.item_data.item_name.Contains("REFLECTOR"))
        {
            newItem.transform.position = newItem.transform.position + new Vector3(0, 2.84f, 0);
        }

        SpawnedItems.Add(newItem);
    }

    public void PopulateShop(bool restoreFromSave)
    {
        List<template_item> unOwnedItems = new List<template_item>();
        PlayerState player = SaveManager.instance.GetCurrentPlayerState();
        if (restoreFromSave)
        {
            bool toySlotUsed = false;
            for (int i = 0; i < player.GetCurrentDayItems().Count; ++i)
            {
                template_item item = ArticyDatabase.GetObject(player.GetCurrentDayItems()[i]) as template_item;

                if (toySlotUsed && item.Template.item_data.item_slot_number == 2)
                {
                    GameObject marker = Shop.instance.GetShopItemSpawnMarkerForPosition(1);
                    SpawnItem(item, marker.transform.position);
                }
                else
                {
                    GameObject marker = Shop.instance.GetShopItemSpawnMarkerForPosition(item.Template.item_data.item_slot_number);
                    SpawnItem(item, marker.transform.position);
                    if (item.Template.item_data.item_slot_number == 2)
                    {
                        toySlotUsed = true;
                    }
                }

                //if (i >= shopItemSpawnMarkers.Count)
                //{
                //    Debug.LogError("Invalid number of items in savegame! Contact Ott!");
                //}
                //else
                //{

                //}
            }
        }
        else
        {
            bool isInfoToolSlotEmpty = true;
            template_item substituteItem = null;
            for (int j = 0; j < 3; ++j)
            {
                unOwnedItems.Clear();
                for (int i = 0; i < ItemDictionary[j].Count; ++i)
                {

                    //if (!SaveManager.instance.GetCurrentCarryoverPlayerState().IsItemOwnedByID(ItemDictionary[j][i].Id))
                    //{
                    //    unOwnedItems.Add(ItemDictionary[j][i]);
                    //}

                    if (!ItemDictionary[j][i].Template.item_data.item_variable.CallScript() && substituteItem != ItemDictionary[j][i])
                    {
                        if (j == 1)
                        {
                            isInfoToolSlotEmpty = false;
                        }
                        if (ItemDictionary[j][i] != GetStarterBody() && ItemDictionary[j][i] != GetStarterHead())
                        {
                            unOwnedItems.Add(ItemDictionary[j][i]);
                        }
                    }
                }

                if (isInfoToolSlotEmpty && j == 1)
                {
                    for (int i = 0; i < ItemDictionary[2].Count; ++i)
                    {
                        if (!ItemDictionary[2][i].Template.item_data.item_variable.CallScript())
                        {
                            if (ItemDictionary[2][i] != GetStarterBody() && ItemDictionary[2][i] != GetStarterHead())
                            {
                                unOwnedItems.Add(ItemDictionary[2][i]);
                            }
                        }
                    }

                }

                if (unOwnedItems.Count > 0)
                {
                    if (isInfoToolSlotEmpty && j == 1)
                    {
                        GameObject marker = Shop.instance.GetShopItemSpawnMarkerForPosition(1);

                        template_item rolledItem = unOwnedItems[Random.Range(0, unOwnedItems.Count)];
                        substituteItem = rolledItem;
                        unOwnedItems.Remove(rolledItem);
                        SpawnItem(rolledItem, marker.transform.position);
                        player.AddCurrentDayItem(rolledItem.Id);
                    }
                    else
                    {
                        GameObject marker = Shop.instance.GetShopItemSpawnMarkerForPosition(j);

                        template_item rolledItem = unOwnedItems[Random.Range(0, unOwnedItems.Count)];
                        unOwnedItems.Remove(rolledItem);
                        SpawnItem(rolledItem, marker.transform.position);
                        player.AddCurrentDayItem(rolledItem.Id);
                    }

                }
            }

        }

    }

    public Sprite GetHeadAssetByID(ulong id)
    {
        for (int i = 0; i < HeadList.Count; ++i)
        {
            if (HeadList[i].Id == id)
            {
                if (HeadList[i].PreviewImage != null && HeadList[i].PreviewImage.Asset != null)
                {
                    return HeadList[i].PreviewImage.Asset.LoadAssetAsSprite();
                }
            }
        }
        return null;
    }

    public Sprite GetBodyAssetByID(ulong id)
    {
        for (int i = 0; i < BodyList.Count; ++i)
        {
            if (BodyList[i].Id == id)
            {
                if (BodyList[i].PreviewImage != null && BodyList[i].PreviewImage.Asset != null)
                {
                    return BodyList[i].PreviewImage.Asset.LoadAssetAsSprite();
                }
            }
        }
        return null;
    }

    public template_item_variation GetHeadByID(ulong id)
    {
        for (int i = 0; i < HeadList.Count; ++i)
        {
            if (HeadList[i].Id == id)
            {
                return HeadList[i];
            }
        }
        return null;
    }


    public template_item_variation GetBodyByID(ulong id)
    {
        for (int i = 0; i < BodyList.Count; ++i)
        {
            if (BodyList[i].Id == id)
            {
                return BodyList[i];
            }
        }
        return null;
    }

    public template_item GetStarterHead()
    {
        return StarterHead;
    }

    public template_item GetStarterBody()
    {
        return StarterBody;
    }

    public template_item GetRandomHead()
    {
        template_item look = BaseHeadList[Random.Range(0, BaseHeadList.Count)];
        return look;
    }

    public template_item GetRandomBody()
    {
        template_item look = BaseBodyList[Random.Range(0, BaseBodyList.Count)];
        return look;
    }

    public void AddAllAccessories()
    {
        for (int j = 0; j < 3; ++j)
        {
            for (int i = 0; i < ItemDictionary[j].Count; ++i)
            {
                if (ItemDictionary[j][i].Template.item_data.item_type_category == item_type_category.ItemVisualAccessory)
                {
                    SaveManager.instance.AddItemDataToPlayer(ItemDictionary[j][i]);
                }
            }
        }
    }

    public List<template_item_variation> GetItemVariations(template_item item)
    {
        List<template_item_variation> variations = new List<template_item_variation>();
        for (int k = 0; k < item.Template.item_data.item_variations.Count; ++k)
        {
            template_item_variation variation = item.Template.item_data.item_variations[k] as template_item_variation;
            switch (variation.Template.item_accessory_variation.item_accessory_type)
            {
                case item_accessory_type.item_head:
                    variations.Add(variation);
                    break;
                case item_accessory_type.item_body:
                    variations.Add(variation);
                    break;
            }
        }

        return variations;
    }
}
