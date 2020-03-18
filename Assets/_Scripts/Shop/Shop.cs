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

public class Shop : MonoBehaviour
{
    public static Shop instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    List<GameObject> ShopItemSpawnMarkerList = new List<GameObject>();
    [SerializeField]
    GameObject Shelf;
    [SerializeField]
    SpriteRenderer MortimerRenderer;
    [SerializeField]
    SpriteRenderer SpritePriceRenderer;
    [SerializeField]
    public SpriteRenderer BackgroundRenderer;

    [SerializeField]
    TextMeshPro TextPrice;
    [SerializeField]
    TextMeshPro TextName;

    // Start is called before the first frame update
    void Start()
    {
        TextPrice.text = "";
        TextName.text = "";
        SpritePriceRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPriceText(int price)
    {
        if (price != 0)
        {
            if (SaveManager.instance.GetCurrentPlayerState().CanAfford(price))
            {
                TextPrice.text = "<color=#30E5FF>" + price.ToString() + "</color>";
                HUDManager.instance.UpdateMoneyShop(price);
            }
            else
            {
                TextPrice.text = "<color=#B0252A>" + price.ToString() + "</color>";
                HUDManager.instance.UpdateMoneyShop(price);
            }
            SpritePriceRenderer.enabled = true;
        }
        else
        {
            HUDManager.instance.UpdateMoney();
            TextPrice.text = "";
            SpritePriceRenderer.enabled = false;
        }
    }

    public void SetNameText(string name, item_type_category type)
    {
        switch (type)
        {
            case item_type_category.ItemVisualAccessory:
                name += " (Clothing)";
                break;
            case item_type_category.ItemToy:
            case item_type_category.ItemInfoTool:
                name += " (Widget)";
                break;
        }
        TextName.text = name;
    }

    public void SetNameText(string name)
    {
        TextName.text = name;
    }

    public GameObject GetShopItemSpawnMarkerForPosition(int i)
    {
        if(i == -1)
        {
            return ShopItemSpawnMarkerList[2];
        }
        return ShopItemSpawnMarkerList[i];
    }

    public GameObject GetShelf()
    {
        return Shelf;
    }
}
