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

public class ComicPageEnd : ComicPage
{
    public static ComicPageEnd instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    public ComicPanel PanelEcologyGood;
    [SerializeField]
    public ComicPanel PanelHealthGood;
    [SerializeField]
    public ComicPanel PanelPeaceGood;
    [SerializeField]
    public ComicPanel PanelProsperityGood;

    [SerializeField]
    public ComicPanel PanelEcologyBad;
    [SerializeField]
    public ComicPanel PanelHealthBad;
    [SerializeField]
    public ComicPanel PanelPeaceBad;
    [SerializeField]
    public ComicPanel PanelProsperityBad;

    [SerializeField]
    public ComicPanel PanelOverallGood;
    [SerializeField]
    public ComicPanel PanelOverallNeutral;
    [SerializeField]
    public ComicPanel PanelOverallBad;

    [SerializeField]
    public ComicPanel PanelPersonalFatePet;
    [SerializeField]
    public ComicPanel PanelPersonalFateFired;
    [SerializeField]
    public ComicPanel PanelPersonalFateTakeover;
    [SerializeField]
    public ComicPanel PanelPersonalFateMurder;

    [SerializeField]
    public Transform PanelParent;

    [SerializeField]
    TextMeshPro TextEpilogueOne;
    [SerializeField]
    TextMeshPro TextEpilogueTwo;
    [SerializeField]
    TextMeshPro TextEpilogueThree;
    [SerializeField]
    TextMeshPro TextEpilogueFour;

    [SerializeField]
    string EpilogueTextEcologyGood;
    [SerializeField]
    string EpilogueTextEcologyBad;
    [SerializeField]
    string EpilogueTextPeaceGood;
    [SerializeField]
    string EpilogueTextPeaceBad;
    [SerializeField]
    string EpilogueTextProsperityGood;
    [SerializeField]
    string EpilogueTextProsperityBad;
    [SerializeField]
    string EpilogueTextHealthGood;
    [SerializeField]
    string EpilogueTextHealthBad;

    [SerializeField]
    string EpilogueTextChaosBad;
    [SerializeField]
    string EpilogueTextChaosNeutral;
    [SerializeField]
    string EpilogueTextChaosGood;

    [SerializeField]
    string EpilogueTextPersonalPet;
    [SerializeField]
    string EpilogueTextPersonalFired;
    [SerializeField]
    string EpilogueTextPersonalTakeover;
    [SerializeField]
    string EpilogueTextPersonalMurderGood;
    [SerializeField]
    string EpilogueTextPersonalMurderBad;


    [SerializeField]
    AudioClip EndClipGoodChaosGoodTakeover;
    [SerializeField]
    AudioClip EndClipGoodChaosEvilTakeover;
    [SerializeField]
    AudioClip EndClipBadChaosFiredPet;
    [SerializeField]
    AudioClip EndClipBadChaosEvilTakeover;
    [SerializeField]
    AudioClip EndClipNeutralChaosGoodTakeover;
    [SerializeField]
    AudioClip EndClipNeutralChaosEvilTakeover;

    [SerializeField]
    GameObject[] PanelMarkerList = new GameObject[(int)EEndComicPanelType.MAX];

    // Start is called before the first frame update
    void Start()
    {
        PageWidth = PageBorderRenderer.sprite.bounds.extents.x * 2;
        List<EGalleryPanelType> panelTypes = new List<EGalleryPanelType>();

        EChaosThreshold chaosStatus;
        // "Good" panel
        // apocalypse ending
        if ((ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true))
        {

            KeyValuePair<string, int> secondWorstParameter = SaveManager.instance.GetCurrentPlayerState().GetSecondWorstParameter();
            ComicPanel spawnedPanel = null;
            if (secondWorstParameter.Key == "ecology")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bEcologyLow)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bEcologyLow = true;
                    panelTypes.Add(EGalleryPanelType.EcologyLow);
                }
                spawnedPanel = Instantiate(PanelEcologyBad, PanelParent);
                TextEpilogueOne.text = EpilogueTextEcologyBad;
            }
            if (secondWorstParameter.Key == "health")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bHealthLow)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bHealthLow = true;
                    panelTypes.Add(EGalleryPanelType.HealthLow);
                }
                spawnedPanel = Instantiate(PanelHealthBad, PanelParent);
                TextEpilogueOne.text = EpilogueTextHealthBad;
            }
            if (secondWorstParameter.Key == "peace")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bPeaceLow)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bPeaceLow = true;
                    panelTypes.Add(EGalleryPanelType.PeaceLow);
                }
                spawnedPanel = Instantiate(PanelPeaceBad, PanelParent);
                TextEpilogueOne.text = EpilogueTextPeaceBad;
            }
            if (secondWorstParameter.Key == "prosperity")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bProsperityLow)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bProsperityLow = true;
                    panelTypes.Add(EGalleryPanelType.ProsperityLow);
                }
                spawnedPanel = Instantiate(PanelProsperityBad, PanelParent);
                TextEpilogueOne.text = EpilogueTextProsperityBad;
            }
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.GoodPanel].transform.localPosition;

            List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>(spawnedPanel.GetComponentsInChildren<SpriteRenderer>());
            for (int i = 0; i < spriteRenderers.Count; ++i)
            {
                spriteRenderers[i].sortingLayerID = SortingLayer.NameToID("EndComic1");
            }
            spawnedPanel.Mask.frontSortingLayerID = SortingLayer.NameToID("EndComic1");
            spawnedPanel.Mask.backSortingLayerID = SortingLayer.NameToID("EndComic1");

            PanelLineList[0].PanelList.Add(spawnedPanel);
        }
        else
        {
            ComicPanel spawnedPanel = null;
            if (ArticyGlobalVariables.Default.rep.best_parameter_name == "ecology")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bEcologyHigh)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bEcologyHigh = true;
                    panelTypes.Add(EGalleryPanelType.EcologyHigh);
                }
                spawnedPanel = Instantiate(PanelEcologyGood, PanelParent);
                TextEpilogueOne.text = EpilogueTextEcologyGood;
            }
            if (ArticyGlobalVariables.Default.rep.best_parameter_name == "health")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bHealthHigh)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bHealthHigh = true;
                    panelTypes.Add(EGalleryPanelType.HealthHigh);
                }
                spawnedPanel = Instantiate(PanelHealthGood, PanelParent);
                TextEpilogueOne.text = EpilogueTextHealthGood;
            }
            if (ArticyGlobalVariables.Default.rep.best_parameter_name == "peace")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bPeaceHigh)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bPeaceHigh = true;
                    panelTypes.Add(EGalleryPanelType.PeaceHigh);
                }
                spawnedPanel = Instantiate(PanelPeaceGood, PanelParent);
                TextEpilogueOne.text = EpilogueTextPeaceGood;
            }
            if (ArticyGlobalVariables.Default.rep.best_parameter_name == "prosperity")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bProsperityHigh)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bProsperityHigh = true;
                    panelTypes.Add(EGalleryPanelType.ProsperityHigh);
                }
                spawnedPanel = Instantiate(PanelProsperityGood, PanelParent);
                TextEpilogueOne.text = EpilogueTextProsperityGood;
            }
            if (spawnedPanel != null)
            {
                spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.GoodPanel].transform.localPosition;
                PanelLineList[0].PanelList.Add(spawnedPanel);
            }

        }

        float averageRep = (ArticyGlobalVariables.Default.rep.ecology + ArticyGlobalVariables.Default.rep.prosperity + ArticyGlobalVariables.Default.rep.peace + ArticyGlobalVariables.Default.rep.health) / 4;

        // "Bad" panel
        //goodest ending
        if (ArticyGlobalVariables.Default.game.finished &&
            !(ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true) &&
            averageRep > GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Utopian])
        {
            KeyValuePair<string, int> secondBestParameter = SaveManager.instance.GetCurrentPlayerState().GetSecondBestParameter();
            ComicPanel spawnedPanel = null;
            if (secondBestParameter.Key == "ecology")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bEcologyHigh)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bEcologyHigh = true;
                    panelTypes.Add(EGalleryPanelType.EcologyHigh);
                }
                spawnedPanel = Instantiate(PanelEcologyGood, PanelParent);
                TextEpilogueTwo.text = EpilogueTextEcologyGood;
            }
            if (secondBestParameter.Key == "health")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bHealthHigh)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bHealthHigh = true;
                    panelTypes.Add(EGalleryPanelType.HealthHigh);
                }
                spawnedPanel = Instantiate(PanelHealthGood, PanelParent);
                TextEpilogueTwo.text = EpilogueTextHealthGood;
            }
            if (secondBestParameter.Key == "peace")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bPeaceHigh)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bPeaceHigh = true;
                    panelTypes.Add(EGalleryPanelType.PeaceHigh);
                }
                spawnedPanel = Instantiate(PanelPeaceGood, PanelParent);
                TextEpilogueTwo.text = EpilogueTextPeaceGood;
            }
            if (secondBestParameter.Key == "prosperity")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bProsperityHigh)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bProsperityHigh = true;
                    panelTypes.Add(EGalleryPanelType.ProsperityHigh);
                }
                spawnedPanel = Instantiate(PanelProsperityGood, PanelParent);
                TextEpilogueTwo.text = EpilogueTextProsperityGood;
            }
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.BadPanel].transform.localPosition;
            List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>(spawnedPanel.GetComponentsInChildren<SpriteRenderer>());
            for (int i = 0; i < spriteRenderers.Count; ++i)
            {
                spriteRenderers[i].sortingLayerID = SortingLayer.NameToID("EndComic2");
            }
            spawnedPanel.Mask.frontSortingLayerID = SortingLayer.NameToID("EndComic2");
            spawnedPanel.Mask.backSortingLayerID = SortingLayer.NameToID("EndComic2");

            PanelLineList[0].PanelList.Add(spawnedPanel);

            //KeyValuePair<string, int> secondBestParameter = SaveManager.instance.GetCurrentPlayerState().GetSecondBestParameter();
            //if (secondBestParameter.Key == "ecology")
            //{
            //    //PanelRenderer.sprite = ComicManager.instance.GetCurrentComicPage().PanelEcologyGood;
            //}
            //if (secondBestParameter.Key == "health")
            //{
            //    //PanelRenderer.sprite = ComicManager.instance.GetCurrentComicPage().PanelHealthGood;
            //}
            //if (secondBestParameter.Key == "peace")
            //{
            //    //PanelRenderer.sprite = ComicManager.instance.GetCurrentComicPage().PanelPeaceGood;
            //}
            //if (secondBestParameter.Key == "prosperity")
            //{
            //    //PanelRenderer.sprite = ComicManager.instance.GetCurrentComicPage().PanelProsperityGood;
            //}
        }
        // other
        else
        {
            ComicPanel spawnedPanel = null;
            if (ArticyGlobalVariables.Default.rep.worst_parameter_name == "ecology")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bEcologyLow)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bEcologyLow = true;
                    panelTypes.Add(EGalleryPanelType.EcologyLow);
                }
                spawnedPanel = Instantiate(PanelEcologyBad, PanelParent);
                TextEpilogueTwo.text = EpilogueTextEcologyBad;
            }
            if (ArticyGlobalVariables.Default.rep.worst_parameter_name == "health")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bHealthLow)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bHealthLow = true;
                    panelTypes.Add(EGalleryPanelType.HealthLow);
                }
                spawnedPanel = Instantiate(PanelHealthBad, PanelParent);
                TextEpilogueTwo.text = EpilogueTextHealthBad;
            }
            if (ArticyGlobalVariables.Default.rep.worst_parameter_name == "peace")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bPeaceLow)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bPeaceLow = true;
                    panelTypes.Add(EGalleryPanelType.PeaceLow);
                }
                spawnedPanel = Instantiate(PanelPeaceBad, PanelParent);
                TextEpilogueTwo.text = EpilogueTextPeaceBad;
            }
            if (ArticyGlobalVariables.Default.rep.worst_parameter_name == "prosperity")
            {
                if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bProsperityLow)
                {
                    SaveManager.instance.GetCurrentCarryoverPlayerState().bProsperityLow = true;
                    panelTypes.Add(EGalleryPanelType.ProsperityLow);
                }
                spawnedPanel = Instantiate(PanelProsperityBad, PanelParent);
                TextEpilogueTwo.text = EpilogueTextProsperityBad;
            }
            if (spawnedPanel != null)
            {
                spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.BadPanel].transform.localPosition;
                PanelLineList[0].PanelList.Add(spawnedPanel);
            }
        }

        if ((ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true))
        {
            if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bChaosHigh)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().bChaosHigh = true;
                panelTypes.Add(EGalleryPanelType.ChaosHigh);
            }
            chaosStatus = EChaosThreshold.Critical;
            ComicPanel spawnedPanel = Instantiate(PanelOverallBad, PanelParent);
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.OverallPanel].transform.localPosition;
            PanelLineList[0].PanelList.Add(spawnedPanel);
            TextEpilogueThree.text = EpilogueTextChaosBad;
        }
        else if (averageRep >= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Utopian])
        {
            if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bChaosLow)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().bChaosLow = true;
                panelTypes.Add(EGalleryPanelType.ChaosLow);
            }
            chaosStatus = EChaosThreshold.Utopian;
            ComicPanel spawnedPanel = Instantiate(PanelOverallGood, PanelParent);
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.OverallPanel].transform.localPosition;
            PanelLineList[0].PanelList.Add(spawnedPanel);
            TextEpilogueThree.text = EpilogueTextChaosGood;
        }
        else
        {
            if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bChaosMid)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().bChaosMid = true;
                panelTypes.Add(EGalleryPanelType.ChaosMid);
            }
            chaosStatus = EChaosThreshold.Good;
            ComicPanel spawnedPanel = Instantiate(PanelOverallNeutral, PanelParent);
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.OverallPanel].transform.localPosition;
            PanelLineList[0].PanelList.Add(spawnedPanel);
            TextEpilogueThree.text = EpilogueTextChaosNeutral;
        }

        bool killedFate = false;
        //whacked Fate
        if (ArticyGlobalVariables.Default.game.subplot_finale_activated && !ArticyGlobalVariables.Default.profile.fate_spared)
        {
            if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bPersonalMurder)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().bPersonalMurder = true;
                panelTypes.Add(EGalleryPanelType.PersonalMurder);
            }
            killedFate = true;
            ComicPanel spawnedPanel = Instantiate(PanelPersonalFateMurder, PanelParent);
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.PersonalPanel].transform.localPosition;
            PanelLineList[0].PanelList.Add(spawnedPanel);
            if ((ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true))
            {
                TextEpilogueFour.text = EpilogueTextPersonalMurderBad;
            }
            else
            {
                TextEpilogueFour.text = EpilogueTextPersonalMurderGood;
            }

            //PanelRenderer.sprite = ComicManager.instance.GetCurrentComicPage().PanelPersonalFateMurder;
        }
        //Fate's Pet
        else if (ArticyGlobalVariables.Default.day.fourteen_got_extreme &&
                    (ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
                    ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
                    ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
                    ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true))
        {
            if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bPersonalPet)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().bPersonalPet = true;
                panelTypes.Add(EGalleryPanelType.PersonalPet);
            }

            ComicPanel spawnedPanel = Instantiate(PanelPersonalFatePet, PanelParent);
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.PersonalPanel].transform.localPosition;
            PanelLineList[0].PanelList.Add(spawnedPanel);
            TextEpilogueFour.text = EpilogueTextPersonalPet;
            //PanelRenderer.sprite = ComicManager.instance.GetCurrentComicPage().PanelPersonalFatePet;
        }
        //I'll be back
        else if (ArticyGlobalVariables.Default.game.finished &&
                    (ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
                    ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
                    ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
                    ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true))
        {

            if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bPersonalFired)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().bPersonalFired = true;
                panelTypes.Add(EGalleryPanelType.PersonalFired);
            }
            ComicPanel spawnedPanel = Instantiate(PanelPersonalFateFired, PanelParent);
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.PersonalPanel].transform.localPosition;
            PanelLineList[0].PanelList.Add(spawnedPanel);
            TextEpilogueFour.text = EpilogueTextPersonalFired;
            //PanelRenderer.sprite = ComicManager.instance.GetCurrentComicPage().PanelPersonalFateFired;
        }
        //the goodest
        else if (ArticyGlobalVariables.Default.game.finished &&
                    !(ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
                    ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
                    ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
                    ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true))
        {
            if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bPersonalTakeover)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().bPersonalTakeover = true;
                panelTypes.Add(EGalleryPanelType.PersonalTakeover);
            }

            ComicPanel spawnedPanel = Instantiate(PanelPersonalFateTakeover, PanelParent);
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.PersonalPanel].transform.localPosition;
            PanelLineList[0].PanelList.Add(spawnedPanel);
            TextEpilogueFour.text = EpilogueTextPersonalTakeover;
            //PanelRenderer.sprite = ComicManager.instance.GetCurrentComicPage().PanelPersonalFateTakeover;
        }
        // error fallback?
        else
        {
            Debug.LogError("Extreme shitstorm explosion ending comic");
            ComicPanel spawnedPanel = Instantiate(PanelPersonalFateFired, PanelParent);
            spawnedPanel.transform.localPosition = PanelMarkerList[(int)EEndComicPanelType.PersonalPanel].transform.localPosition;
            PanelLineList[0].PanelList.Add(spawnedPanel);
            TextEpilogueFour.text = "...i am error...";
            //PanelRenderer.sprite = ComicManager.instance.GetCurrentComicPage().PanelPersonalFateFired;
        }
        if (!SaveManager.instance.GetCurrentPlayerState().bSentGalleryUpdate)
        {
            SaveManager.instance.GetCurrentPlayerState().bSentGalleryUpdate = true;
        }


        SaveManager.instance.MarkSavegameDirty();

        switch (chaosStatus)
        {
            case EChaosThreshold.Critical:
                if (killedFate)
                {
                    AudioManager.instance.SwitchMusic(EndClipBadChaosEvilTakeover, EndClipBadChaosEvilTakeover, false);
                }
                else
                {
                    AudioManager.instance.SwitchMusic(EndClipBadChaosFiredPet, EndClipBadChaosFiredPet, false);
                }
                break;
            case EChaosThreshold.Good:
                if (killedFate)
                {
                    AudioManager.instance.SwitchMusic(EndClipNeutralChaosEvilTakeover, EndClipNeutralChaosEvilTakeover, false);

                }
                else
                {
                    AudioManager.instance.SwitchMusic(EndClipNeutralChaosGoodTakeover, EndClipNeutralChaosGoodTakeover, false);
                }
                break;
            case EChaosThreshold.Utopian:
                if (killedFate)
                {
                    AudioManager.instance.SwitchMusic(EndClipGoodChaosEvilTakeover, EndClipGoodChaosEvilTakeover, false);
                }
                else
                {
                    AudioManager.instance.SwitchMusic(EndClipGoodChaosGoodTakeover, EndClipGoodChaosGoodTakeover, false);
                }
                break;
        }


        ComicManager.instance.SetCurrentComicPage(this);

        for (int i = 0; i < PanelLineList.Count; ++i)
        {
            for (int j = 0; j < PanelLineList[i].PanelList.Count; ++j)
            {
                List<ComicPanelElement> childElements = PanelLineList[i].PanelList[j].GetChildElements();
                for (int k = 0; k < childElements.Count; ++k)
                {
                    childElements[k].SetPanelLine(i);
                    childElements[k].SetPanel(PanelLineList[i].PanelList[j]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
