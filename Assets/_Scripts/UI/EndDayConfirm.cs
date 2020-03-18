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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndDayConfirm : MonoBehaviour
{
    public static EndDayConfirm instance;

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



    // Start is called before the first frame update
    void Start()
    {
        ButtonYes.onClick.AddListener(OnYesClicked);
        ButtonNo.onClick.AddListener(OnNoClicked);

        Hide();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnYesClicked()
    {
        ElevatorManager.instance.SwitchScene(EScene.Elevator, true);

        Hide();
    }

    public void OnNoClicked()
    {
        //AudioManager.instance.PlayButtonClick();
        InputManager.instance.FrameLock();
        Hide();
    }

    public void Show()
    {
        Cursor.visible = true;

        gameObject.SetActive(true);
    }


    public bool IsVisible()
    {
        return gameObject.activeSelf;
    }

    public void Hide()
    {
        if (MarkerOfDeath.instance.IsPickedUp())
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        gameObject.SetActive(false);
    }
}
