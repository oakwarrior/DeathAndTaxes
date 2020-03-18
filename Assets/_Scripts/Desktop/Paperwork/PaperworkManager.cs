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
using Articy.Project_Of_Death;
using Articy.Project_Of_Death.GlobalVariables;

public class PaperworkManager : ManagerBase
{
    public static PaperworkManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Paperwork PaperworkTemplate;

    public List<string> PaperworkHoverTexts = new List<string>();
    public List<string> PaperworkHoverTextsUnhappy = new List<string>();


    public List<Paperwork> PaperworkList = new List<Paperwork>();

    List<Paperwork> PaperworkListPrevious = new List<Paperwork>();
    //Paperwork FocusedPaperwork = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Paperwork GetPaperworkByProfile(template_profile profile)
    {
        for(int i = 0; i < PaperworkList.Count; ++i)
        {
            if(PaperworkList[i].GetProfile() == profile)
            {
                return PaperworkList[i];
            }
        }
        Debug.LogError("PROFILE QUERIED BUT NOT FOUND, PAPERWORKMANAGER SHAT ITSELF");
        return null;
    }

    public Paperwork GetPreviousPaperworkByProfile(template_profile profile)
    {
        for (int i = 0; i < PaperworkListPrevious.Count; ++i)
        {
            if (PaperworkListPrevious[i].GetProfile() == profile)
            {
                return PaperworkListPrevious[i];
            }
        }
        Debug.LogError("PROFILE QUERIED BUT NOT FOUND, PAPERWORKMANAGER SHAT ITSELF");
        return null;
    }

    public void CleanUpPaperwork()
    {
        InputManager.instance.LastHitInteractable = null;
        for (int i = 0; i < PaperworkListPrevious.Count; ++i)
        {
            Destroy(PaperworkListPrevious[i].gameObject);
        }
        PaperworkListPrevious.Clear();
    }

    public void NotifyStartDay(bool restoreFromSave)
    {
        List<template_profile> profiles = new List<template_profile>(DesktopManager.instance.GetProfiles());
        int profileCount = profiles.Count;
        if(!restoreFromSave)
        {
            if (SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() == 4)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().FateAttentionProfileIndex = Random.Range(0, profileCount);
            }
            else
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().FateAttentionProfileIndex = -1;
            }
            SaveManager.instance.MarkSavegameDirty();
        }

        //TODO: ARTICY VAR TO CHECK IF IT HAS BEEN DEADED OR LIVE
        int fateAttentionProfileIndex = SaveManager.instance.GetCurrentCarryoverPlayerState().FateAttentionProfileIndex;


        for (int i = 0; i < profileCount; ++i)
        {
            Paperwork newPaperwork = Instantiate(PaperworkTemplate);
            if(restoreFromSave)
            {
                newPaperwork.InitFromProfile(profiles[i], GrimDesk.instance.PaperWorkSpawnMarker.gameObject.transform.position + new Vector3(1.0f * (i % 4), 0.3f * (i % 4) - 0.6f * Mathf.FloorToInt(i / 4), -i * 2 - 3), GrimDesk.instance.PaperWorkSpawnMarker.gameObject.transform.rotation, restoreFromSave, i, i == fateAttentionProfileIndex);
            }
            else
            {
                int profileIndex = Random.Range(0, profiles.Count);
                newPaperwork.InitFromProfile(profiles[profileIndex], GrimDesk.instance.PaperWorkSpawnMarker.gameObject.transform.position + new Vector3(1.0f * (i % 4), 0.3f * (i % 4) - 0.6f * Mathf.FloorToInt(i / 4), -i * 2 - 3), GrimDesk.instance.PaperWorkSpawnMarker.gameObject.transform.rotation, restoreFromSave, i, i == fateAttentionProfileIndex);
                profiles.RemoveAt(profileIndex);
            }
            newPaperwork.name += i;
            //newPaperwork.StartFadeInParticle();
            PaperworkList.Add(newPaperwork);
        }


    }

    public void NotifyEndDay()
    {
        PaperworkListPrevious = new List<Paperwork>(PaperworkList);

        for (int i = 0; i < PaperworkList.Count; ++i)
        {
            if (PaperworkList[i].GetProfile().Template.profile_basic_data.profile_marked_for_death)
            {
                SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.DEATH_DAILY, 1);
            }
            else
            {
                SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.SPARE_DAILY, 1);
            }
            PaperworkList[i].StartFadeOutParticle();
            //Destroy(PaperworkList[i].gameObject);
        }
        PaperworkList.Clear();
    }

    //public void NotifyPaperworkFocused(Paperwork paperwork)
    //{
    //    if (FocusedPaperwork != null)
    //    {
    //        FocusedPaperwork.UnFocusPaperwork();
    //    }
    //    FocusedPaperwork = paperwork;
    //}

    //public void NotifyPaperworkUnFocused()
    //{
    //    FocusedPaperwork = null;
    //}

    public string GetRandomHoverText()
    {
        if(ArticyGlobalVariables.Default.game.work_complaint_counter >= 2)
        {
            int rando = Random.Range(0, 2);
            if(rando == 0)
            {
                return PaperworkHoverTextsUnhappy[Random.Range(0, PaperworkHoverTextsUnhappy.Count)];
            }
            else
            {
                return PaperworkHoverTexts[Random.Range(0, PaperworkHoverTexts.Count)];
            }
        }
        else
        {
            return PaperworkHoverTexts[Random.Range(0, PaperworkHoverTexts.Count)];
        }
    }

    public bool AreAllMarked()
    {
        for (int i = 0; i < PaperworkList.Count; ++i)
        {
            if (!PaperworkList[i].IsMarked())
            {
                return false;
            }
        }

        return true;
    }
}
