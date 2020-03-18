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
using Articy.Project_Of_Death.GlobalVariables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : ManagerBase
{
    public static HUDManager instance;

    private void Awake()
    {
        instance = this;
    }

    public TextMeshProUGUI TextHover;
    public TextMeshProUGUI TextHoverShop;
    public TextMeshProUGUI TextMoney;
    //public TextMeshProUGUI TextDayCounter;
    public Image IconMoney;
    public Canvas MainCanvas;
    public GameObject PanelHUD;
    public Button ButtonToggleDebugPanel;
    public CanvasScaler Scaler;

    public Toggle ToggleLore;

    public override void InitManager()
    {
        base.InitManager();
        ButtonToggleDebugPanel.onClick.AddListener(OnDebugPanelClicked);
        ToggleLore.onValueChanged.AddListener(OnLoreToggleChanged);
        ToggleHUD(false);
        ToggleLore.gameObject.SetActive(false);
        //MainCanvas.worldCamera = Camera.main;
    }

    public void OnLoreToggleChanged(bool val)
    {
        GameManager.instance.bLoreMode = val;
    }

    public void OnDebugPanelClicked()
    {
        DebugPanelHUD.instance.Toggle();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.CurrentAspectRatio == EAspectRatio.a21_9)
        {
            Scaler.matchWidthOrHeight = 1.0f;
        }
        else
        {
            Scaler.matchWidthOrHeight = 0.0f;
        }
    }

    public void ToggleHUDManager(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void ToggleHUD(bool isVisible)
    {
        ButtonToggleDebugPanel.gameObject.SetActive(GameManager.instance.bDebugMode);
        PanelHUD.SetActive(isVisible);
        //if (ArticyGlobalVariables.Default.game.salary_on)
        //{
        //    TextMoney.enabled = true;
        //    IconMoney.enabled = true;
        //}
        //else
        //{
        //    TextMoney.enabled = false;
        //    IconMoney.enabled = false;
        //}
    }

    public void ToggleMoney(bool val)
    {
        TextMoney.enabled = val;
        IconMoney.enabled = val;
    }

    public void UpdateMoney()
    {
        TextMoney.text = "" + ArticyGlobalVariables.Default.inventory.money;
    }

    public void UpdateMoneyShop(int price)
    {
        if(ArticyGlobalVariables.Default.inventory.money - price < 0)
        {
            TextMoney.text = "" + ArticyGlobalVariables.Default.inventory.money + " - " + price + " = <color=#B0252A>" + (ArticyGlobalVariables.Default.inventory.money - price) + "</color>";
        }
        else
        {
            TextMoney.text = "" + ArticyGlobalVariables.Default.inventory.money + " - " + price + " = <color=#30E5FF>" + (ArticyGlobalVariables.Default.inventory.money - price) + "</color>";
        }
    }

    public void SetHoverText(string text)
    {
        if(ElevatorManager.instance.GetCurrentScene() == EScene.Shop)
        {
            TextHoverShop.text = text;
            TextHover.text = "";
        }
        else
        {
            if(!FaxMachine.instance.bIsFaxTransmitting)
            {
                TextHover.text = text;
            }
            else
            {
                TextHover.text = "";
            }
            TextHoverShop.text = "";
        }
    }

    public void SetDayCounter(int day)
    {
        //TextDayCounter.gameObject.SetActive(true);
        //TextDayCounter.text = "Day: " + (day + 1).ToString();
    }
}