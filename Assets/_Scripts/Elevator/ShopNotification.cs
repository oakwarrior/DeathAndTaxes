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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNotification : MonoBehaviour
{
    public static ShopNotification instanceOffice;
    public static ShopNotification instanceMirror;

    private void Awake()
    {
        if(IAmSoTiredOfThis == 0)
        {
            instanceOffice = this;
        }
        else
        {
            instanceMirror = this;
        }
    }

    [SerializeField]
    int IAmSoTiredOfThis = 0;

    [SerializeField]
    SpriteRenderer NotificationRenderer;

    [SerializeField]
    Sprite SpriteClosed;

    [SerializeField]
    Sprite SpriteOpen;

    // Start is called before the first frame update
    void Start()
    {
        if(SaveManager.instance == null)
        {
            gameObject.SetActive(false);

        }
        else
        {
            if (IAmSoTiredOfThis == 0)
            {
                ToggleVisible(SaveManager.instance.GetCurrentPlayerState().HasOfficeShopNotification());
            }
            else
            {
                ToggleVisible(SaveManager.instance.GetCurrentPlayerState().HasMirrorShopNotification());

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleSpriteOpen(bool val)
    {
        if(val)
        {
            NotificationRenderer.sprite = SpriteOpen;
        }
        else
        {
            NotificationRenderer.sprite = SpriteClosed;
        }
    }

    public void ToggleVisible(bool val)
    {
        gameObject.SetActive(val);
    }
}
