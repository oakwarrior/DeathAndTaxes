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

public class GalleryScreen : MonoBehaviour
{
    public static GalleryScreen instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    Button ButtonBack;

    [SerializeField]
    Image ImagePeaceHigh;
    [SerializeField]
    Image ImageProsperityHigh;
    [SerializeField]
    Image ImageEcologyHigh;
    [SerializeField]
    Image ImageHealthHigh;

    [SerializeField]
    Image ImagePeaceLow;
    [SerializeField]
    Image ImageProsperityLow;
    [SerializeField]
    Image ImageEcologyLow;
    [SerializeField]
    Image ImageHealthLow;

    [SerializeField]
    Image ImageChaosLow;
    [SerializeField]
    Image ImageChaosMid;
    [SerializeField]
    Image ImageChaosHigh;

    [SerializeField]
    Image ImagePersonalPet;
    [SerializeField]
    Image ImagePersonalFired;
    [SerializeField]
    Image ImagePersonalTakeover;
    [SerializeField]
    Image ImagePersonalMurder;

    [SerializeField]
    Sprite SpriteUnknown;

    [SerializeField]
    Sprite SpritePeaceHigh;
    [SerializeField]
    Sprite SpriteProsperityHigh;
    [SerializeField]
    Sprite SpriteEcologyHigh;
    [SerializeField]
    Sprite SpriteHealthHigh;
    [SerializeField]
    Sprite SpritePeaceLow;
    [SerializeField]
    Sprite SpriteProsperityLow;
    [SerializeField]
    Sprite SpriteEcologyLow;
    [SerializeField]
    Sprite SpriteHealthLow;
    [SerializeField]
    Sprite SpriteChaosLow;
    [SerializeField]
    Sprite SpriteChaosMid;
    [SerializeField]
    Sprite SpriteChaosHigh;
    [SerializeField]
    Sprite SpritePersonalPet;
    [SerializeField]
    Sprite SpritePersonalFired;
    [SerializeField]
    Sprite SpritePersonalTakeover;
    [SerializeField]
    Sprite SpritePersonalMurder;
    [SerializeField]
    TextMeshProUGUI TextTooltip;

    List<GalleryPercentageIndicator> PercentageIndicators = new List<GalleryPercentageIndicator>();


    // Start is called before the first frame update
    void Start()
    {
        ButtonBack.onClick.AddListener(Hide);
        PercentageIndicators = new List<GalleryPercentageIndicator>(GetComponentsInChildren<GalleryPercentageIndicator>());
        Hide();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTooltip(string text)
    {
        TextTooltip.text = text;
    }

    public void OnBackClicked()
    {
        Hide();
        IntroController.instance.Show(true);
        IntroController.instance.HandleStreamerMode();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        IntroController.instance.Show(true);
    }

    public void Show()
    {
        if (SceneManager.GetActiveScene().name == "EndComic")
        {
            Time.timeScale = 0.0f;
        }

        SaveManager.instance.GetCurrentCarryoverPlayerState().SetHasGalleryNotification(false);

        CarryoverPlayerState state = SaveManager.instance.GetCurrentCarryoverPlayerState();



        if (state.bPeaceHigh)
        {
            ImagePeaceHigh.sprite = SpritePeaceHigh;
        }
        else
        {
            ImagePeaceHigh.sprite = SpriteUnknown;
        }
        if (state.bProsperityHigh)
        {
            ImageProsperityHigh.sprite = SpriteProsperityHigh;
        }
        else
        {
            ImageProsperityHigh.sprite = SpriteUnknown;
        }
        if (state.bEcologyHigh)
        {
            ImageEcologyHigh.sprite = SpriteEcologyHigh;
        }
        else
        {
            ImageEcologyHigh.sprite = SpriteUnknown;
        }
        if (state.bHealthHigh)
        {
            ImageHealthHigh.sprite = SpriteHealthHigh;
        }
        else
        {
            ImageHealthHigh.sprite = SpriteUnknown;
        }

        if (state.bPeaceLow)
        {
            ImagePeaceLow.sprite = SpritePeaceLow;
        }
        else
        {
            ImagePeaceLow.sprite = SpriteUnknown;
        }
        if (state.bProsperityLow)
        {
            ImageProsperityLow.sprite = SpriteProsperityLow;
        }
        else
        {
            ImageProsperityLow.sprite = SpriteUnknown;
        }
        if (state.bEcologyLow)
        {
            ImageEcologyLow.sprite = SpriteEcologyLow;
        }
        else
        {
            ImageEcologyLow.sprite = SpriteUnknown;
        }
        if (state.bHealthLow)
        {
            ImageHealthLow.sprite = SpriteHealthLow;
        }
        else
        {
            ImageHealthLow.sprite = SpriteUnknown;
        }

        if (state.bChaosHigh)
        {
            ImageChaosHigh.sprite = SpriteChaosHigh;
        }
        else
        {
            ImageChaosHigh.sprite = SpriteUnknown;
        }
        if (state.bChaosMid)
        {
            ImageChaosMid.sprite = SpriteChaosMid;
        }
        else
        {
            ImageChaosMid.sprite = SpriteUnknown;
        }
        if (state.bChaosLow)
        {
            ImageChaosLow.sprite = SpriteChaosLow;
        }
        else
        {
            ImageChaosLow.sprite = SpriteUnknown;
        }

        if (state.bPersonalPet)
        {
            ImagePersonalPet.sprite = SpritePersonalPet;
        }
        else
        {
            ImagePersonalPet.sprite = SpriteUnknown;
        }
        if (state.bPersonalTakeover)
        {
            ImagePersonalTakeover.sprite = SpritePersonalTakeover;
        }
        else
        {
            ImagePersonalTakeover.sprite = SpriteUnknown;
        }
        if (state.bPersonalMurder)
        {
            ImagePersonalMurder.sprite = SpritePersonalMurder;
        }
        else
        {
            ImagePersonalMurder.sprite = SpriteUnknown;
        }
        if (state.bPersonalFired)
        {
            ImagePersonalFired.sprite = SpritePersonalFired;
        }
        else
        {
            ImagePersonalFired.sprite = SpriteUnknown;
        }

        for (int i = 0; i < PercentageIndicators.Count; ++i)
        {
            PercentageIndicators[i].UpdatePercentage();
        }
        gameObject.SetActive(true);

    }
}
