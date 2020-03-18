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

public class MarkConfirm : MonoBehaviour
{
    public static MarkConfirm instance;
    private void Awake()
    {
        instance = this;
    }

    public Button ButtonYes;
    public Button ButtonNo;
    public Toggle ToggleOptOut;
    public TextMeshProUGUI TextPrompt;
    public string TextDefault;
    public string TextSelfie;
    public string TextSelfieLive;
    public string TextUsurpator;
    public string TextUsurpatorLive;

    public Paperwork PendingPaperwork;
    public PaperworkMark PendingMark;
    public EPaperworkMarkType PendingMarkType;



    // Start is called before the first frame update
    void Start()
    {
        ButtonYes.onClick.AddListener(OnYesClicked);
        ButtonNo.onClick.AddListener(OnNoClicked);
        ToggleOptOut.onValueChanged.AddListener(OnOptOutChanged);

        Hide();
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void OnYesClicked()
    {
        InputManager.instance.FrameLock();
        switch (PendingMarkType)
        {
            case EPaperworkMarkType.Live:
            {
                PendingPaperwork.MarkPaperworkLive();
                break;
            }
            case EPaperworkMarkType.Die:
            {
                PendingPaperwork.MarkPaperworkDie();
                break;
            }
        }
        PendingMark.ConfirmMark();

        if (ToggleOptOut.isOn)
        {
            OptionsManager.instance.SetSkipMarkPopUp(true);
        }
        Hide();
    }

    public void OnOptOutChanged(bool val)
    {
        AudioManager.instance.PlayTestClip(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster);
    }

    public void OnNoClicked()
    {
        InputManager.instance.FrameLock();
        //AudioManager.instance.PlayButtonClick();
        Hide();
        ToggleOptOut.isOn = false;
    }

    public void Show(Paperwork pendingPaperwork, EPaperworkMarkType pendingMarkType, PaperworkMark pendingMark)
    {
        PendingPaperwork = pendingPaperwork;
        if(PendingPaperwork.GetProfile() == GameManager.instance.GrimReaperPaperworkProfile)
        {
            if(pendingMarkType == EPaperworkMarkType.Die)
            {
                TextPrompt.text = TextSelfie;
            }
            else
            {
                TextPrompt.text = TextSelfieLive;
            }
        }
        else if (PendingPaperwork.GetProfile() == GameManager.instance.FatePaperworkProfile)
        {
            if (pendingMarkType == EPaperworkMarkType.Die)
            {
                TextPrompt.text = TextUsurpator;
            }
            else
            {
                TextPrompt.text = TextUsurpatorLive;
            }
        }
        else
        {
            TextPrompt.text = TextDefault;
        }
        PendingMarkType = pendingMarkType;
        PendingMark = pendingMark;

        if (SaveManager.instance.CurrentOptions.SkipMarkPopUp)
        {
            ToggleOptOut.gameObject.SetActive(false);
        }
        else
        {
            ToggleOptOut.isOn = false;
            ToggleOptOut.gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
        Cursor.visible = true;
    }

    public bool IsVisible()
    {
        return gameObject.activeSelf;
    }

    public void Hide()
    {
        Cursor.visible = false;
        PendingPaperwork = null;
        gameObject.SetActive(false);
    }
}
