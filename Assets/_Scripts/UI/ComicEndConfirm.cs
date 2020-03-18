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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComicEndConfirm : MonoBehaviour
{
    public static ComicEndConfirm instance;
    private void Awake()
    {
        instance = this;
    }

    public Button ButtonYes;
    public Button ButtonNo;
    public TextMeshProUGUI TextPrompt;



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
        InputManager.instance.FrameLock();

        SaveManager.instance.GetCurrentCarryoverPlayerState().UpdateArticyCurrentBody();
        SaveManager.instance.GetCurrentCarryoverPlayerState().UpdateArticyCurrentHead();
        SceneManager.LoadScene("Game");
        ElevatorManager.instance.SwitchScene(EScene.Office);

        Hide();
    }

    public void OnNoClicked()
    {
        InputManager.instance.FrameLock();
        //AudioManager.instance.PlayButtonClick();
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Cursor.visible = true;
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
