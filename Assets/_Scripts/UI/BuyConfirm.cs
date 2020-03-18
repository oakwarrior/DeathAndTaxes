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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyConfirm : MonoBehaviour
{
    public static BuyConfirm instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    public TextMeshProUGUI TextConfirm;
    [SerializeField]
    public Button ButtonYes;
    [SerializeField]
    public Button ButtonNo;
    [SerializeField]
    public Button ButtonOk;

    public ShopItem PendingItem;


    // Start is called before the first frame update
    void Start()
    {
        ButtonYes.onClick.AddListener(OnYesClicked);
        ButtonNo.onClick.AddListener(OnNoClicked);
        ButtonOk.onClick.AddListener(OnOkClicked);

        Hide();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnYesClicked()
    {
        if (PendingItem.ItemData.Template.item_data.item_type_category == item_type_category.ItemVisualAccessory || PendingItem.ItemData == ShopManager.instance.MirrorTemplate)
        {
            SaveManager.instance.GetCurrentPlayerState().SetHasMirrorShopNotification(true);
        }
        else
        {
            SaveManager.instance.GetCurrentPlayerState().SetHasOfficeShopNotification(true);
        }
        SaveManager.instance.AddItemDataToPlayer(PendingItem.ItemData);

        SaveManager.instance.GetCurrentPlayerState().ModifyMoney(-Mathf.RoundToInt(PendingItem.GetPrice()));
        if(PendingItem.ItemData == ShopManager.instance.EraserTemplate)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().bEraserLocationReset = false;
            SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementEraserBuyCount();
        }
        ShopManager.instance.RemoveSpawnedItem(PendingItem, false);

        if(ShopManager.instance.EraserTemplate == PendingItem.ItemData)
        {
            if(!ArticyGlobalVariables.Default.shop.keeper_story_eraser_done)
            {
                SpeechBubbleManager.instance.StartBubbleCascadeFromDialogue(PendingItem.GetItemQuip());
            }
        }
        else
        {
            SpeechBubbleManager.instance.StartBubbleCascadeFromDialogue(PendingItem.GetItemQuip());
        }
        PendingItem = null;
        Hide();
    }

    public void OnOkClicked()
    {
        //AudioManager.instance.PlayButtonClick();
        InputManager.instance.FrameLock();
        PendingItem = null;
        Hide();
    }

    public void OnNoClicked()
    {
        //AudioManager.instance.PlayButtonClick();
        InputManager.instance.FrameLock();
        PendingItem = null;
        Hide();
    }

    public void ShowForItem(ShopItem item)
    {
        PendingItem = item;
        Cursor.visible = true;
        gameObject.SetActive(true);
        ButtonYes.gameObject.SetActive(true);
        ButtonNo.gameObject.SetActive(true);
        ButtonOk.gameObject.SetActive(false);
        TextConfirm.text = "Buy " + item.ItemData.Template.item_data.item_name + "?";
    }

    public bool IsVisible()
    {
        return gameObject.activeSelf;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
