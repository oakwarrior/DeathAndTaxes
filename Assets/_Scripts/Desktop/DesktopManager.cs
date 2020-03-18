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

#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Articy.Project_Of_Death;
using Articy.Project_Of_Death.Features;
using System;
using Articy.Project_Of_Death.GlobalVariables;

public class DesktopManager : ManagerBase
{

    public static DesktopManager instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioClip MusicBackground;


    [TextArea]
    public string FirstDayPhoneFeed;

    List<template_profile> Profiles;
    List<template_profile> AllProfiles = new List<template_profile>();

    Dictionary<int, List<template_profile>> DailyProfiles = new Dictionary<int, List<template_profile>>();
    public void ClearProfiles()
    {
        Profiles.Clear();
        DailyProfiles.Clear();
    }

    public void StartDay(bool restoreFromSave = false)
    {
        FaxMachine.instance.NotifyStartDay();

        if (SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex() == 12 &&
            (ArticyGlobalVariables.Default.game.cat_extremely_annoyed == true ||
            ArticyGlobalVariables.Default.game.toy_given_to_cat == false))
        {
            PawPrints.instance.TogglePawPrints(true);
        }
        else
        {
            PawPrints.instance.TogglePawPrints(false);
        }

        RollDay(restoreFromSave);
        if (restoreFromSave)
        {
            SaveManager.instance.GetCurrentPlayerState().RestoreNewsText();
        }
        else
        {
            //Phone.instance.RollSituationNews();
        }

        SaveManager.instance.GetCurrentPlayerState().SetDayDone(false);
        GetPeople();
        LetterOfFate.instance.UpdateForDay(restoreFromSave);
        Calendar.instance.UpdateForDay();
        HUDManager.instance.UpdateMoney();
        //HandleInventory();

        ChaosGlobe.instance.MarkSituationDirty();

        if (!restoreFromSave)
        {
            ArticyGlobalVariables.Default.day.current_count = SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt();
            SaveManager.instance.GetCurrentPlayerState().ResetStat(EStat.DEATH_DAILY);
            SaveManager.instance.GetCurrentPlayerState().ResetStat(EStat.SPARE_DAILY);
            SaveManager.instance.GetCurrentPlayerState().ResetStat(EStat.ECOLOGY_DAILY);
            SaveManager.instance.GetCurrentPlayerState().ResetStat(EStat.HEALTH_DAILY);
            SaveManager.instance.GetCurrentPlayerState().ResetStat(EStat.PEACE_DAILY);
            SaveManager.instance.GetCurrentPlayerState().ResetStat(EStat.PROSPERITY_DAILY);
        }


        PaperworkManager.instance.NotifyStartDay(restoreFromSave);
        ShopManager.instance.NotifyStartDay(restoreFromSave);
        HUDManager.instance.SetDayCounter(SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex());
        SaveManager.instance.MarkSavegameDirty();
    }

    public void HandleInventory()
    {
        if (ArticyGlobalVariables.Default.inventory.cactus)
        {

        }
        if (ArticyGlobalVariables.Default.inventory.eraser)
        {

        }
        if (ArticyGlobalVariables.Default.inventory.radio)
        {

        }
        if (ArticyGlobalVariables.Default.inventory.fidget_thing)
        {

        }
        if (ArticyGlobalVariables.Default.inventory.toy_cat)
        {

        }
        if (ArticyGlobalVariables.Default.inventory.desklamp)
        {

        }
        if (ArticyGlobalVariables.Default.inventory.coin)
        {

        }
        if (ArticyGlobalVariables.Default.inventory.calendar)
        {

        }
        if (ArticyGlobalVariables.Default.inventory.award_plaque)
        {

        }
        if (ArticyGlobalVariables.Default.inventory.snowglobe)
        {

        }
    }

    void GetPeople()
    {
        Profiles = new List<template_profile>(DailyProfiles[SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt()]);

        //this should all be handled before I guess? EDIT: OR NOT LOL DO NOT TOUCH
        List<template_profile> profilesToRemove = new List<template_profile>();
        foreach (template_profile profile in Profiles)
        {
            if (!profile.Template.profile_basic_data.profile_is_enabled)
            {
                profilesToRemove.Add(profile);
            }
        }

        foreach (template_profile profile in profilesToRemove)
        {
            Profiles.Remove(profile);
        }
    }

    public List<template_profile> GetProfiles()
    {
        return Profiles;
    }

    public override void InitManager()
    {
        base.InitManager();
    }

    public void RollDay(bool restoreFromSave)
    {
        List<template_profile> profiles = new List<template_profile>();
        int currentDayIndex = SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex();

        if (restoreFromSave)
        {
            List<ProfileState> profileStates = SaveManager.instance.GetCurrentPlayerState().GetCurrentDayProfiles();
            for (int i = 0; i < profileStates.Count; ++i)
            {
                template_profile profile = ArticyDatabase.GetObject(profileStates[i].ProfileID) as template_profile;
                if (profile != null)
                {
                    profiles.Add(profile);
                }
                else
                {
                    Debug.LogError("Invalid Profile ID: " + profileStates[i].ProfileID.ToString() + " saved for day: " + (currentDayIndex + 1));
                }
            }
        }
        else
        {
            template_day day = GameManager.instance.GetCurrentDay();

            if (day == null)
            {
                Debug.LogError("DAY NOT FOUND; PROBABLY GOING TO EXPLODE");
            }
            template_day_task dayTask = day.Template.day.day_task_slot as template_day_task;

            if (dayTask == null)
            {
                Debug.LogError("DAY TASK NOT FOUND; PROBABLY GOING TO EXPLODE");
            }
            dayTask.Template.task.task_profile_count.CallScript();


            Debug.Log("Getting " + ArticyGlobalVariables.Default.day.profile_count + " profiles for day " + (currentDayIndex + 1) + "; Day Name: " + day.DisplayName);

            List<ArticyObject> dailyProfileTemplates = new List<ArticyObject>(dayTask.Template.task.task_daily_profiles);
            for (int j = 0; j < ArticyGlobalVariables.Default.day.profile_count; ++j)
            {
                int index = j;
                if (dayTask.Template.task.task_profile_shuffler)
                {
                    index = UnityEngine.Random.Range(0, dailyProfileTemplates.Count);
                }
                else
                {

                }
                template_profile dailyProfile = dailyProfileTemplates[index] as template_profile;
                template_random_profile dailyRandomProfile = dailyProfileTemplates[index] as template_random_profile;

                if (dayTask.Template.task.task_profile_shuffler)
                {
                    dailyProfileTemplates.RemoveAt(index);
                }
                if (dailyRandomProfile != null)
                {
                    dailyProfile = ProfileManager.instance.RollProfileByRandoDefinition(dailyRandomProfile);
                }
                if (dailyProfile == null)
                {
                    Debug.LogError("Rolled null profile for day " + (currentDayIndex + 1) + "!");
                    continue;
                }
                else if (!dailyProfile.Template.profile_basic_data.profile_is_enabled)
                {
                    Debug.Log("Skipping un-enabled profile: " + dailyProfile.Template.profile_basic_data.profile_name + " for day: " + (currentDayIndex + 1) + "!");
                }
                else
                {
                    profiles.Add(dailyProfile);
                    AllProfiles.Add(dailyProfile);
                }
            }
            int profileDelta = ArticyGlobalVariables.Default.day.profile_count - profiles.Count;
            for (int i = 0; i < profileDelta; ++i)
            {
                template_profile dailyProfile = null;
                dailyProfile = ProfileManager.instance.RollProfileByRandoDefinition(ProfileManager.instance.AnyRandomTemplate);
                if (dailyProfile == null)
                {
                    Debug.LogError("Rolled null profile for day " + (currentDayIndex + 1) + "!");
                    continue;
                }
                else
                {
                    profiles.Add(dailyProfile);
                    AllProfiles.Add(dailyProfile);
                }
            }

            SaveManager.instance.GetCurrentPlayerState().SetCurrentDayProfiles(profiles);
            // grab only its general properties
        }

        List<ulong> profileIDs = new List<ulong>();

        for (int i = 0; i < profiles.Count; ++i)
        {
            profileIDs.Add(profiles[i].Id);

            if (!restoreFromSave)
            {
                Phone.instance.AddNewsText(profiles[i].Template.profile_basic_data.profile_appearance_news_first);
                Phone.instance.AddNewsText(profiles[i].Template.profile_basic_data.profile_appearance_news_second);
            }
        }
        Debug.Log("Found " + profiles.Count + " profiles for day " + (currentDayIndex + 1));

        if (!restoreFromSave)
        {
            SaveManager.instance.GetCurrentPlayerState().AddDailyProfiles(currentDayIndex + 1, profileIDs);
        }
        DailyProfiles.Add(currentDayIndex + 1, profiles);
    }

    public int GetProfileCountForDay(int day)
    {
        return SaveManager.instance.GetCurrentPlayerState().GetDailyProfileIDs(day).Count;
    }

    public int GetProfileCountForCurrentDay()
    {
        return GetProfileCountForDay(SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt());
    }

    public void FinishDay()
    {
        Phone.instance.ClearNews();

        int deathOccupationMarks = 0;
        int spareOccupationMarks = 0;
        int deathOlderMarks = 0;
        int spareOlderMarks = 0;
        int deathYoungerMarks = 0;
        int spareYoungerMarks = 0;

        foreach (template_profile profile in Profiles)
        {
            Paperwork paperwork = PaperworkManager.instance.GetPreviousPaperworkByProfile(profile);

            if (profile.Template.profile_basic_data.profile_marked_for_death)
            {
                if (paperwork != null && paperwork.bFateAttentionSpare)
                {
                    ArticyGlobalVariables.Default.day.four_spared_fates_extra_profile = false;
                }

                if (GameManager.instance.GetCurrentDayTask().Template.task.task_death_occupation_amount > 0 &&
                    (int)GameManager.instance.GetCurrentDayTask().Template.task.task_death_occupation_selector == (int)profile.Template.profile_basic_data.profile_occupation_selector)
                {
                    deathOccupationMarks++;
                }

                int ageOlder = 0;
                int.TryParse(profile.Template.profile_basic_data.profile_age_value, out ageOlder);
                if (GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_older_amount > 0 &&
                (int)GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_older >= ageOlder)
                {
                    deathOlderMarks++;
                }

                int ageYounger = 0;
                int.TryParse(profile.Template.profile_basic_data.profile_age_value, out ageYounger);
                if (GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_younger_amount > 0 &&
                (int)GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_younger <= ageYounger)
                {
                    deathYoungerMarks++;
                }

                ApplyConsequence(profile.Template.profile_death_data);
                SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.DEATH_TOTAL, 1);
            }
            else
            {
                if (paperwork != null && paperwork.bFateAttentionSpare)
                {
                    ArticyGlobalVariables.Default.day.four_spared_fates_extra_profile = true;
                }

                if (GameManager.instance.GetCurrentDayTask().Template.task.task_spare_occupation_amount > 0 &&
                    (int)GameManager.instance.GetCurrentDayTask().Template.task.task_spare_occupation_selector == (int)profile.Template.profile_basic_data.profile_occupation_selector)
                {
                    spareOccupationMarks++;
                }

                int ageOlder = 0;
                int.TryParse(profile.Template.profile_basic_data.profile_age_value, out ageOlder);
                if (GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_older_amount > 0 &&
                (int)GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_older >= ageOlder)
                {
                    spareOlderMarks++;
                }

                int ageYounger = 0;
                int.TryParse(profile.Template.profile_basic_data.profile_age_value, out ageYounger);
                if (GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_younger_amount > 0 &&
                (int)GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_younger <= ageYounger)
                {
                    spareYoungerMarks++;
                }

                ApplyConsequence(profile.Template.profile_spare_data);
                SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.SPARE_TOTAL, 1);
            }
        }
        SaveManager.instance.GetCurrentPlayerState().SetFaxSent(true);

        SaveManager.instance.GetCurrentCarryoverPlayerState().ResetTaskDeviationCounter();

        if (GameManager.instance.GetCurrentDayTask().Template.task.task_spare_occupation_amount > 0 && 
            spareOccupationMarks < GameManager.instance.GetCurrentDayTask().Template.task.task_spare_occupation_amount)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementTaskDeviationCounter();
        }
        if (GameManager.instance.GetCurrentDayTask().Template.task.task_death_occupation_amount > 0 && 
            deathOccupationMarks < GameManager.instance.GetCurrentDayTask().Template.task.task_death_occupation_amount)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementTaskDeviationCounter();
        }
        if (GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_older_amount > 0 && 
            spareOlderMarks < GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_older_amount)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementTaskDeviationCounter();
        }
        if (GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_older_amount > 0 && 
            deathOlderMarks < GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_older_amount)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementTaskDeviationCounter();
        }
        if (GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_younger_amount > 0 &&
            spareYoungerMarks < GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_younger_amount)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementTaskDeviationCounter();
        }
        if (GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_younger_amount > 0 &&
            deathYoungerMarks < GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_younger_amount)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementTaskDeviationCounter();
        }

        Debug.Log("Day " + (SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt()) +
            " task overview:" +
            "\nSpare Occupation requirement: " + GameManager.instance.GetCurrentDayTask().Template.task.task_spare_occupation_selector + " value: " + GameManager.instance.GetCurrentDayTask().Template.task.task_spare_occupation_amount + " current: " + spareOccupationMarks +
            "\nSpare Older requirement: " + GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_older + " value: " + GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_older_amount + " current: " + spareOlderMarks +
            "\nSpare Younger requirement: " + GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_younger + " value: " + GameManager.instance.GetCurrentDayTask().Template.task.task_spare_threshold_younger_amount + " current: " + spareYoungerMarks +
            "\nDeath Occupation requirement: " + GameManager.instance.GetCurrentDayTask().Template.task.task_death_occupation_selector + " value: " + GameManager.instance.GetCurrentDayTask().Template.task.task_death_occupation_amount + " current: " + deathOccupationMarks +
            "\nDeath Older requirement: " + GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_older + " value: " + GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_older_amount + " current: " + deathOlderMarks +
            "\nDeath Younger requirement: " + GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_younger + " value: " + GameManager.instance.GetCurrentDayTask().Template.task.task_death_threshold_younger_amount + " current: " + deathYoungerMarks +
            "\nDeviation Counter: " + SaveManager.instance.GetCurrentCarryoverPlayerState().GetTaskDeviationCounter());

        if (SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex() == 19)
        {
            ArticyGlobalVariables.Default.day.twenty_done_cons_trigger = true;
        }

        if (GameManager.instance.GetLoyaltyFactor() >= 1.0f)
        {
            SaveManager.instance.GetCurrentPlayerState().bRuleBender = false;
        }

        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.LOYALTY, Mathf.CeilToInt(100 * GameManager.instance.GetLoyaltyFactor()));


        Profiles.Clear();

        KeyValuePair<string, int> bestParameter = SaveManager.instance.GetCurrentPlayerState().GetBestParameter();
        KeyValuePair<string, int> worstParameter = SaveManager.instance.GetCurrentPlayerState().GetWorstParameter();

        ArticyGlobalVariables.Default.rep.best_parameter_name = bestParameter.Key;
        ArticyGlobalVariables.Default.rep.best_parameter_value = bestParameter.Value;

        ArticyGlobalVariables.Default.rep.worst_parameter_name = worstParameter.Key;
        ArticyGlobalVariables.Default.rep.worst_parameter_value = worstParameter.Value;

        float averageRep = (ArticyGlobalVariables.Default.rep.ecology + ArticyGlobalVariables.Default.rep.prosperity + ArticyGlobalVariables.Default.rep.peace + ArticyGlobalVariables.Default.rep.health) / 4;

        if (ArticyGlobalVariables.Default.profile.evil_ecology_trigger)
        {
            if (ArticyGlobalVariables.Default.rep.ecology > GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Dire])
            {
                ArticyGlobalVariables.Default.profile.evil_ecology_trigger = false;
            }
        }
        else
        {
            if (ArticyGlobalVariables.Default.rep.ecology <= -500)
            {
                ArticyGlobalVariables.Default.profile.evil_ecology_trigger = true;
            }
        }
        if (ArticyGlobalVariables.Default.profile.evil_health_trigger)
        {
            if (ArticyGlobalVariables.Default.rep.health > GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Dire])
            {
                ArticyGlobalVariables.Default.profile.evil_health_trigger = false;
            }
        }
        else
        {
            if (ArticyGlobalVariables.Default.rep.health <= -500)
            {
                ArticyGlobalVariables.Default.profile.evil_health_trigger = true;
            }
        }
        if (ArticyGlobalVariables.Default.profile.evil_peace_trigger)
        {
            if (ArticyGlobalVariables.Default.rep.peace > GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Dire])
            {
                ArticyGlobalVariables.Default.profile.evil_peace_trigger = false;
            }
        }
        else
        {
            if (ArticyGlobalVariables.Default.rep.peace <= -500)
            {
                ArticyGlobalVariables.Default.profile.evil_peace_trigger = true;
            }
        }
        if (ArticyGlobalVariables.Default.profile.evil_prosperity_trigger)
        {
            if (ArticyGlobalVariables.Default.rep.prosperity > GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Dire])
            {
                ArticyGlobalVariables.Default.profile.evil_prosperity_trigger = false;
            }
        }
        else
        {
            if (ArticyGlobalVariables.Default.rep.prosperity <= -500)
            {
                ArticyGlobalVariables.Default.profile.evil_prosperity_trigger = true;
            }
        }

        Debug.Log("Day " + (SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt()) +
            " over\nWorst parameter: " + ArticyGlobalVariables.Default.rep.worst_parameter_name + " value: " + ArticyGlobalVariables.Default.rep.worst_parameter_value +
            "\nBest parameter: " + ArticyGlobalVariables.Default.rep.best_parameter_name + " value: " + ArticyGlobalVariables.Default.rep.best_parameter_value +
            "\nAverageRep: " + averageRep);

        ExitDesktop();
    }

    public void ApplyParameterConsequence(profile_death_dataFeature profile_death_data)
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH, Mathf.RoundToInt(profile_death_data.profile_death_healthcare_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY, Mathf.RoundToInt(profile_death_data.profile_death_ecology_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY, Mathf.RoundToInt(profile_death_data.profile_death_prosperity_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE, Mathf.RoundToInt(profile_death_data.profile_death_peace_value));

        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH_DAILY, Mathf.RoundToInt(profile_death_data.profile_death_healthcare_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY_DAILY, Mathf.RoundToInt(profile_death_data.profile_death_ecology_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY_DAILY, Mathf.RoundToInt(profile_death_data.profile_death_prosperity_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE_DAILY, Mathf.RoundToInt(profile_death_data.profile_death_peace_value));

        ChaosGlobe.instance.MarkSituationDirty();
    }

    public void ReverseParameterConsequence(profile_death_dataFeature profile_death_data)
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH, Mathf.RoundToInt(-profile_death_data.profile_death_healthcare_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY, Mathf.RoundToInt(-profile_death_data.profile_death_ecology_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY, Mathf.RoundToInt(-profile_death_data.profile_death_prosperity_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE, Mathf.RoundToInt(-profile_death_data.profile_death_peace_value));

        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH_DAILY, Mathf.RoundToInt(-profile_death_data.profile_death_healthcare_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY_DAILY, Mathf.RoundToInt(-profile_death_data.profile_death_ecology_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY_DAILY, Mathf.RoundToInt(-profile_death_data.profile_death_prosperity_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE_DAILY, Mathf.RoundToInt(-profile_death_data.profile_death_peace_value));

        ChaosGlobe.instance.MarkSituationDirty();
    }

    public void ApplyParameterConsequence(profile_spare_dataFeature profile_spare_data)
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH, Mathf.RoundToInt(profile_spare_data.profile_spare_healthcare_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY, Mathf.RoundToInt(profile_spare_data.profile_spare_ecology_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY, Mathf.RoundToInt(profile_spare_data.profile_spare_prosperity_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE, Mathf.RoundToInt(profile_spare_data.profile_spare_peace_value));

        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH_DAILY, Mathf.RoundToInt(profile_spare_data.profile_spare_healthcare_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY_DAILY, Mathf.RoundToInt(profile_spare_data.profile_spare_ecology_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY_DAILY, Mathf.RoundToInt(profile_spare_data.profile_spare_prosperity_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE_DAILY, Mathf.RoundToInt(profile_spare_data.profile_spare_peace_value));

        ChaosGlobe.instance.MarkSituationDirty();
    }

    public void ReverseParameterConsequence(profile_spare_dataFeature profile_spare_data)
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH, Mathf.RoundToInt(-profile_spare_data.profile_spare_healthcare_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY, Mathf.RoundToInt(-profile_spare_data.profile_spare_ecology_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY, Mathf.RoundToInt(-profile_spare_data.profile_spare_prosperity_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE, Mathf.RoundToInt(-profile_spare_data.profile_spare_peace_value));

        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH_DAILY, Mathf.RoundToInt(-profile_spare_data.profile_spare_healthcare_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY_DAILY, Mathf.RoundToInt(-profile_spare_data.profile_spare_ecology_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY_DAILY, Mathf.RoundToInt(-profile_spare_data.profile_spare_prosperity_value));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE_DAILY, Mathf.RoundToInt(-profile_spare_data.profile_spare_peace_value));

        ChaosGlobe.instance.MarkSituationDirty();
    }

    private void ApplyConsequence(profile_death_dataFeature profile_death_data)
    {
        profile_death_data.profile_death_instruction.CallScript();

        for (int i = 0; i < profile_death_data.profile_death_disabler.Count; ++i)
        {
            template_profile disabledProfile = profile_death_data.profile_death_disabler[i] as template_profile;
            disabledProfile.Template.profile_basic_data.profile_is_enabled = false;
            SaveManager.instance.GetCurrentPlayerState().AddDisabledProfile(disabledProfile.Id);
        }

        for (int i = 0; i < profile_death_data.profile_death_enabler.Count; ++i)
        {
            template_profile enabledProfile = profile_death_data.profile_death_enabler[i] as template_profile;
            enabledProfile.Template.profile_basic_data.profile_is_enabled = true;
            SaveManager.instance.GetCurrentPlayerState().AddEnabledProfile(enabledProfile.Id);
        }



        Phone.instance.AddNewsText(profile_death_data.profile_death_news_first);
        Phone.instance.AddNewsText(profile_death_data.profile_death_news_second);
        Phone.instance.AddNewsText(profile_death_data.profile_death_news_third);
    }

    private void ApplyConsequence(profile_spare_dataFeature profile_spare_data)
    {
        profile_spare_data.profile_spare_instruction.CallScript();

        for (int i = 0; i < profile_spare_data.profile_spare_disabler.Count; ++i)
        {
            template_profile disabledProfile = profile_spare_data.profile_spare_disabler[i] as template_profile;
            disabledProfile.Template.profile_basic_data.profile_is_enabled = false;
            SaveManager.instance.GetCurrentPlayerState().AddDisabledProfile(disabledProfile.Id);
        }

        for (int i = 0; i < profile_spare_data.profile_spare_enabler.Count; ++i)
        {
            template_profile enabledProfile = profile_spare_data.profile_spare_enabler[i] as template_profile;
            enabledProfile.Template.profile_basic_data.profile_is_enabled = true;
            SaveManager.instance.GetCurrentPlayerState().AddEnabledProfile(enabledProfile.Id);
        }



        Phone.instance.AddNewsText(profile_spare_data.profile_spare_news_first);
        Phone.instance.AddNewsText(profile_spare_data.profile_spare_news_second);
        Phone.instance.AddNewsText(profile_spare_data.profile_spare_news_third);
    }

    void ExitDesktop()
    {
        if (GameManager.instance.GameMode == EGameMode.DEMO)
        {
            ElevatorManager.instance.SwitchScene(EScene.Office);
        }
        else
        {
            ElevatorManager.instance.SwitchScene(EScene.Elevator);

            Elevator.instance.GetElevatorButtonBySceneType(EScene.Desktop).ToggleEnable(false);
            Elevator.instance.GetElevatorButtonBySceneType(EScene.Office).ToggleEnable(true);
            //Elevator.instance.GetElevatorButtonBySceneType(EScene.Elevator).ToggleEnable(true);
            Elevator.instance.GetElevatorButtonBySceneType(EScene.Shop).ToggleEnable(true);
        }



        //if (PlayerManager.instance.GetCurrentDayIndex() < GameManager.instance.GetDays().transform.childCount)
        //{
        //    Debug.Log("Starting day " + PlayerManager.instance.GetCurrentDayIndex());
        //}
        //else
        //{
        //    Debug.Log("Game Over");
        //}
        SaveManager.instance.MarkSavegameDirty();
    }

}
