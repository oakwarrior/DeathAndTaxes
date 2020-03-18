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
using UnityEngine.EventSystems;

public enum EGalleryPanelType
{
    PeaceHigh,
    ProsperityHigh,
    EcologyHigh,
    HealthHigh,
    PeaceLow,
    ProsperityLow,
    EcologyLow,
    HealthLow,
    ChaosLow,
    ChaosMid,
    ChaosHigh,
    PersonalPet,
    PersonalFired,
    PersonalTakeover,
    PersonalMurder,
    MAX
}

public class GalleryElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    EGalleryPanelType Type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        GalleryScreen.instance.UpdateTooltip("Locked");

        switch (Type)
        {
            case EGalleryPanelType.PeaceHigh:
                GalleryScreen.instance.UpdateTooltip("High Peace");
                break;
            case EGalleryPanelType.ProsperityHigh:
                GalleryScreen.instance.UpdateTooltip("High Prosperity");
                break;
            case EGalleryPanelType.EcologyHigh:
                GalleryScreen.instance.UpdateTooltip("High Ecology");
                break;
            case EGalleryPanelType.HealthHigh:
                GalleryScreen.instance.UpdateTooltip("High Health");
                break;
            case EGalleryPanelType.PeaceLow:
                GalleryScreen.instance.UpdateTooltip("Low Peace");
                break;
            case EGalleryPanelType.ProsperityLow:
                GalleryScreen.instance.UpdateTooltip("Low Prosperity");
                break;
            case EGalleryPanelType.EcologyLow:
                GalleryScreen.instance.UpdateTooltip("Low Ecology");
                break;
            case EGalleryPanelType.HealthLow:
                GalleryScreen.instance.UpdateTooltip("Low Health");
                break;
            case EGalleryPanelType.ChaosLow:
                GalleryScreen.instance.UpdateTooltip("Utopian");
                break;
            case EGalleryPanelType.ChaosMid:
                GalleryScreen.instance.UpdateTooltip("Balanced");
                break;
            case EGalleryPanelType.ChaosHigh:
                GalleryScreen.instance.UpdateTooltip("Chaos");
                break;
            case EGalleryPanelType.PersonalPet:
                GalleryScreen.instance.UpdateTooltip("Fate's Pet");
                break;
            case EGalleryPanelType.PersonalFired:
                GalleryScreen.instance.UpdateTooltip("Fired");
                break;
            case EGalleryPanelType.PersonalTakeover:
                GalleryScreen.instance.UpdateTooltip("Takeover");
                break;
            case EGalleryPanelType.PersonalMurder:
                GalleryScreen.instance.UpdateTooltip("Usurper");
                break;
            case EGalleryPanelType.MAX:
                GalleryScreen.instance.UpdateTooltip("");
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GalleryScreen.instance.UpdateTooltip("");
    }
}

