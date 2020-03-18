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
using UnityEngine.UI;

public class DebugPanelHUD : MonoBehaviour
{
    public static DebugPanelHUD instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    Button ButtonDebugPlusEco;
    [SerializeField]
    Button ButtonDebugMinusEco;
    [SerializeField]
    Button ButtonDebugPlusPeace;
    [SerializeField]
    Button ButtonDebugMinusPeace;
    [SerializeField]
    Button ButtonDebugPlusHealth;
    [SerializeField]
    Button ButtonDebugMinusHealth;
    [SerializeField]
    Button ButtonDebugPlusProsperity;
    [SerializeField]
    Button ButtonDebugMinusProsperity;
    [SerializeField]
    Button ButtonDebugSkipDialogue;
    [SerializeField]
    Button ButtonDebugFinishDay;
    [SerializeField]
    Button ButtonDebugSetGameFinished;
    [SerializeField]
    Button ButtonDebugSetSubplotFinaleActive;
    [SerializeField]
    Button ButtonDebugSetSelfiePrimed;
    [SerializeField]
    Button ButtonDebugAddAllItems;
    [SerializeField]
    Button ButtonDebugCheatMoney;
    [SerializeField]
    Button ButtonDebugAddAllAccessories;
    [SerializeField]
    Button ButtonDebugRefreshShop;
    [SerializeField]
    Button ButtonDebugPlusLoyalty;
    [SerializeField]
    Button ButtonDebugMinusLoyalty;
    [SerializeField]
    Button ButtonDebugEcoTrigger;
    [SerializeField]
    Button ButtonDebugPeaceTrigger;
    [SerializeField]
    Button ButtonDebugHealthTrigger;
    [SerializeField]
    Button ButtonDebugProsperityTrigger;

    // Start is called before the first frame update
    void Start()
    {
        ButtonDebugPlusEco.onClick.AddListener(InputManager.instance.DebugIncreaseEcologyParameter);
        ButtonDebugMinusEco.onClick.AddListener(InputManager.instance.DebugDecreaseEcologyParameter);
        ButtonDebugPlusPeace.onClick.AddListener(InputManager.instance.DebugIncreasePeaceParameter);
        ButtonDebugMinusPeace.onClick.AddListener(InputManager.instance.DebugDecreasePeaceParameter);
        ButtonDebugPlusHealth.onClick.AddListener(InputManager.instance.DebugIncreaseHealthParameter);
        ButtonDebugMinusHealth.onClick.AddListener(InputManager.instance.DebugDecreaseHealthParameter);
        ButtonDebugPlusProsperity.onClick.AddListener(InputManager.instance.DebugIncreaseProsperityParameter);
        ButtonDebugMinusProsperity.onClick.AddListener(InputManager.instance.DebugDecreaseProsperityParameter);
        ButtonDebugSkipDialogue.onClick.AddListener(InputManager.instance.DebugSkipDialogue);
        ButtonDebugFinishDay.onClick.AddListener(InputManager.instance.DebugFinishDay);
        ButtonDebugSetGameFinished.onClick.AddListener(InputManager.instance.DebugSetGameFinished);
        ButtonDebugSetSubplotFinaleActive.onClick.AddListener(InputManager.instance.DebugSetSubplotFinaleActive);
        ButtonDebugSetSelfiePrimed.onClick.AddListener(InputManager.instance.DebugSetSelfiePrimed);
        ButtonDebugAddAllItems.onClick.AddListener(InputManager.instance.DebugAddAllItems);
        ButtonDebugCheatMoney.onClick.AddListener(InputManager.instance.DebugCheatMoney);
        ButtonDebugAddAllAccessories.onClick.AddListener(InputManager.instance.DebugAddAllAccessories);
        ButtonDebugRefreshShop.onClick.AddListener(InputManager.instance.DebugResetDayAlsoRefreshesShop);
        ButtonDebugPlusLoyalty.onClick.AddListener(InputManager.instance.DebugIncreaseLoyalty);
        ButtonDebugMinusLoyalty.onClick.AddListener(InputManager.instance.DebugDecreaseLoyalty);
        ButtonDebugEcoTrigger.onClick.AddListener(InputManager.instance.DebugSetEvilTriggerEco);
        ButtonDebugPeaceTrigger.onClick.AddListener(InputManager.instance.DebugSetEvilTriggerPeace);
        ButtonDebugHealthTrigger.onClick.AddListener(InputManager.instance.DebugSetEvilTriggerHealth);
        ButtonDebugProsperityTrigger.onClick.AddListener(InputManager.instance.DebugSetEvilTriggerProsperity);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
