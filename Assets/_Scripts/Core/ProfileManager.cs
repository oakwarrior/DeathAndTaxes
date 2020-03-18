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
using Articy.Project_Of_Death;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : ManagerBase
{
    public static ProfileManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private ArticyRef RandoProfilesReference;

    [SerializeField]
    private ArticyRef PlantProfilesReference;

    [SerializeField]
    private ArticyRef LastHumanProfilesReference;

    [SerializeField]
    private ArticyRef DeveloperProfilesReference;

    [SerializeField]
    private ArticyRef AnyRandomTemplateReference;

    [SerializeField]
    private List<ArticyRef> SubplotProfileReferences = new List<ArticyRef>();


    public template_random_profile AnyRandomTemplate;

    public List<template_profile> RandoProfileList = new List<template_profile>();
    public List<template_profile> LastHumanProfileList = new List<template_profile>();
    public List<template_profile> PlantProfileList = new List<template_profile>();
    public List<template_profile> DeveloperProfileList = new List<template_profile>();
    public List<template_profile> SubplotProfiles = new List<template_profile>();

    List<template_random_profile> AllRandoTemplates = new List<template_random_profile>();

    public override void InitManager()
    {
        base.InitManager();
        instance = this;
        AllRandoTemplates = ArticyDatabase.GetAllOfType<template_random_profile>();



        if (RandoProfilesReference.HasReference)
        {
            UserFolder randoFolder = RandoProfilesReference.GetObject<UserFolder>();

            for (int i = 0; i < randoFolder.Children.Count; ++i)
            {
                UserFolder childRandoFolder = randoFolder.Children[i] as UserFolder;
                if (childRandoFolder != null)
                {
                    for (int j = 0; j < childRandoFolder.Children.Count; ++j)
                    {
                        template_profile randoProfile = childRandoFolder.Children[j] as template_profile;

                        //if (randoProfile.PreviewImage != null && randoProfile.PreviewImage.Asset != null)
                        //{
                        //    ImagePhoto.sprite = AssignedProfile.PreviewImage.Asset.LoadAssetAsSprite();
                        //}

                        if (randoProfile != null && randoProfile.PreviewImage != null && randoProfile.PreviewImage.Asset != null)
                        {
                            RandoProfileList.Add(randoProfile);
                        }
                    }
                }
            }
        }

        //Debug.Log("Found " + RandoProfileList.Count + " rando profiles");

        if (LastHumanProfilesReference.HasReference)
        {
            UserFolder randoFolderLastHumans = LastHumanProfilesReference.GetObject<UserFolder>();
            for (int i = 0; i < randoFolderLastHumans.Children.Count; ++i)
            {
                template_profile randoProfile = randoFolderLastHumans.Children[i] as template_profile;

                if (randoProfile != null && randoProfile.PreviewImage != null && randoProfile.PreviewImage.Asset != null)
                {
                    LastHumanProfileList.Add(randoProfile);
                }
            }
        }
        //Debug.Log("Found " + LastHumanProfileList.Count + " last human rando profiles");

        if (DeveloperProfilesReference.HasReference)
        {
            UserFolder devFolder = DeveloperProfilesReference.GetObject<UserFolder>();

            for (int j = 0; j < devFolder.Children.Count; ++j)
            {
                template_profile devProfile = devFolder.Children[j] as template_profile;
                DeveloperProfileList.Add(devProfile);
            }
        }

        //Debug.Log("Found " + DeveloperProfileList.Count + " dev profiles");

        if (PlantProfilesReference.HasReference)
        {
            UserFolder randoFolderPlants = PlantProfilesReference.GetObject<UserFolder>();
            for (int i = 0; i < randoFolderPlants.Children.Count; ++i)
            {
                template_profile randoProfile = randoFolderPlants.Children[i] as template_profile;

                if (randoProfile != null && randoProfile.PreviewImage != null && randoProfile.PreviewImage.Asset != null)
                {
                    PlantProfileList.Add(randoProfile);
                }
            }
        }

        if (AnyRandomTemplateReference.HasReference)
        {
            AnyRandomTemplate = AnyRandomTemplateReference.GetObject<template_random_profile>();
        }
        //Debug.Log("Found " + PlantProfileList.Count + " plant rando profiles");

        for (int i = 0; i < SubplotProfileReferences.Count; ++i)
        {
            if (SubplotProfileReferences[i].HasReference)
            {
                template_profile subplotProfile = SubplotProfileReferences[i].GetObject<template_profile>();

                if (subplotProfile != null)
                {
                    SubplotProfiles.Add(subplotProfile);
                }
            }
        }
        //Debug.Log("Found " + SubplotProfiles.Count + " subplot profiles");

        string trolo = "";
        for (int i = 0; i < AllRandoTemplates.Count; ++i)
        {
            trolo += "Found " + GetRandoCandidatesByTemplate(AllRandoTemplates[i]) + " : " + AllRandoTemplates[i].DisplayName + "'s\n";
        }
        //Debug.Log(trolo);

        List<template_profile> profiles = RandoProfileList; //ArticyDatabase.GetAllOfType<template_profile>();

        int ecoPlusSpare = 0;
        int healthPlusSpare = 0;
        int peacePlusSpare = 0;
        int prosperityPlusSpare = 0;

        int ecoPlusDeath = 0;
        int healthPlusDeath = 0;
        int peacePlusDeath = 0;
        int prosperityPlusDeath = 0;

        int ecoMinusDeath = 0;
        int healthMinusDeath = 0;
        int peaceMinusDeath = 0;
        int prosperityMinusDeath = 0;

        int ecoMinusSpare = 0;
        int healthMinusSpare = 0;
        int peaceMinusSpare = 0;
        int prosperityMinusSpare = 0;

        for (int i = 0; i < profiles.Count; ++i)
        {
            if (profiles[i].Template.profile_death_data.profile_death_ecology_value > 0)
            {
                ecoPlusDeath += Mathf.RoundToInt(profiles[i].Template.profile_death_data.profile_death_ecology_value);
            }
            else
            {
                ecoMinusDeath += Mathf.RoundToInt(profiles[i].Template.profile_death_data.profile_death_ecology_value);
            }
            if (profiles[i].Template.profile_death_data.profile_death_peace_value > 0)
            {
                peacePlusDeath += Mathf.RoundToInt(profiles[i].Template.profile_death_data.profile_death_peace_value);
            }
            else
            {
                peaceMinusDeath += Mathf.RoundToInt(profiles[i].Template.profile_death_data.profile_death_peace_value);
            }
            if (profiles[i].Template.profile_death_data.profile_death_healthcare_value > 0)
            {
                healthPlusDeath += Mathf.RoundToInt(profiles[i].Template.profile_death_data.profile_death_healthcare_value);
            }
            else
            {
                healthMinusDeath += Mathf.RoundToInt(profiles[i].Template.profile_death_data.profile_death_healthcare_value);
            }
            if (profiles[i].Template.profile_death_data.profile_death_prosperity_value > 0)
            {
                prosperityPlusDeath += Mathf.RoundToInt(profiles[i].Template.profile_death_data.profile_death_prosperity_value);
            }
            else
            {
                prosperityMinusDeath += Mathf.RoundToInt(profiles[i].Template.profile_death_data.profile_death_prosperity_value);
            }


            if (profiles[i].Template.profile_spare_data.profile_spare_ecology_value > 0)
            {
                ecoPlusSpare += Mathf.RoundToInt(profiles[i].Template.profile_spare_data.profile_spare_ecology_value);
            }
            else
            {
                ecoMinusSpare += Mathf.RoundToInt(profiles[i].Template.profile_spare_data.profile_spare_ecology_value);
            }
            if (profiles[i].Template.profile_spare_data.profile_spare_peace_value > 0)
            {
                peacePlusSpare += Mathf.RoundToInt(profiles[i].Template.profile_spare_data.profile_spare_peace_value);
            }
            else
            {
                peaceMinusSpare += Mathf.RoundToInt(profiles[i].Template.profile_spare_data.profile_spare_peace_value);
            }
            if (profiles[i].Template.profile_spare_data.profile_spare_healthcare_value > 0)
            {
                healthPlusSpare += Mathf.RoundToInt(profiles[i].Template.profile_spare_data.profile_spare_healthcare_value);
            }
            else
            {
                healthMinusSpare += Mathf.RoundToInt(profiles[i].Template.profile_spare_data.profile_spare_healthcare_value);
            }
            if (profiles[i].Template.profile_spare_data.profile_spare_prosperity_value > 0)
            {
                prosperityPlusSpare += Mathf.RoundToInt(profiles[i].Template.profile_spare_data.profile_spare_prosperity_value);
            }
            else
            {
                prosperityMinusSpare += Mathf.RoundToInt(profiles[i].Template.profile_spare_data.profile_spare_prosperity_value);
            }
        }
        //Debug.Log("MEGASTATS\n" +
        //    "Eco Plus Spare: " + ecoPlusSpare + "\n" +
        //    "Health Plus Spare: " + healthPlusSpare + "\n" +
        //    "Peace Plus Spare: " + peacePlusSpare + "\n" +
        //    "Prosperity Plus Spare: " + prosperityPlusSpare + "\n" +

        //    "Eco Minus Spare: " + ecoMinusSpare + "\n" +
        //    "Health Minus Spare: " + healthMinusSpare + "\n" +
        //    "Peace Minus Spare: " + peaceMinusSpare + "\n" +
        //    "Prosperity Minus Spare: " + prosperityMinusSpare + "\n" +

        //    "Eco Plus Death: " + ecoPlusDeath + "\n" +
        //    "Health Plus Death: " + healthPlusDeath + "\n" +
        //    "Peace Plus Death: " + peacePlusDeath + "\n" +
        //    "Prosperity Plus Death: " + prosperityPlusDeath + "\n" +

        //    "Eco Minus Death: " + ecoMinusDeath + "\n" +
        //    "Health Minus Death: " + healthMinusDeath + "\n" +
        //    "Peace Minus Death: " + peaceMinusDeath + "\n" +
        //    "Prosperity Minus Death: " + prosperityMinusDeath + "\n"

        //    );
    }

    public void RemoveRolledRandoFromPool(template_profile profile)
    {
        RandoProfileList.Remove(profile);
        LastHumanProfileList.Remove(profile);
        PlantProfileList.Remove(profile);
    }

    public void RemoveRolledRandoFromPoolByID(ulong randoID)
    {

        for (int i = 0; i < PlantProfileList.Count; ++i)
        {
            if (PlantProfileList[i].Id == randoID)
            {
                PlantProfileList.RemoveAt(i);
                return;
            }
        }

        for (int i = 0; i < LastHumanProfileList.Count; ++i)
        {
            if (LastHumanProfileList[i].Id == randoID)
            {
                LastHumanProfileList.RemoveAt(i);
                return;
            }
        }

        for (int i = 0; i < RandoProfileList.Count; ++i)
        {
            if (RandoProfileList[i].Id == randoID)
            {
                RandoProfileList.RemoveAt(i);
                return;
            }
        }



        Debug.LogError("Attempted to remove Rando but did not find it in Pool!");
    }

    public bool IsProfileModerate(template_profile profile)
    {
        if (profile.Template.profile_death_data.profile_death_ecology_value <= -80 || profile.Template.profile_death_data.profile_death_ecology_value >= 80 ||
            profile.Template.profile_death_data.profile_death_peace_value <= -80 || profile.Template.profile_death_data.profile_death_peace_value >= 80 ||
            profile.Template.profile_death_data.profile_death_healthcare_value <= -80 || profile.Template.profile_death_data.profile_death_healthcare_value >= 80 ||
            profile.Template.profile_death_data.profile_death_prosperity_value <= -80 || profile.Template.profile_death_data.profile_death_prosperity_value >= 80)
        {
            return false;
        }
        if (profile.Template.profile_spare_data.profile_spare_ecology_value <= -80 || profile.Template.profile_spare_data.profile_spare_ecology_value >= 80 ||
            profile.Template.profile_spare_data.profile_spare_peace_value <= -80 || profile.Template.profile_spare_data.profile_spare_peace_value >= 80 ||
            profile.Template.profile_spare_data.profile_spare_healthcare_value <= -80 || profile.Template.profile_spare_data.profile_spare_healthcare_value >= 80 ||
            profile.Template.profile_spare_data.profile_spare_prosperity_value <= -80 || profile.Template.profile_spare_data.profile_spare_prosperity_value >= 80)
        {
            return false;
        }
        return true;
    }

    public template_profile RollProfileByRandoDefinition(template_random_profile rando)
    {
        template_profile rolledRando = null;
        if (RandoProfileList.Count == 0)
        {
            Debug.LogError("OUT OF RANDOS");
            return null;
        }

        if (rando.Template.task_random_profile_generator.profile_generate_any_random)
        {
            List<template_profile> randoCandidates = new List<template_profile>();
            List<template_profile> randoCandidatesEnabled = new List<template_profile>();
            for (int i = 0; i < RandoProfileList.Count; ++i)
            {
                if (RandoProfileList[i].Template.profile_basic_data.profile_is_enabled && IsProfileModerate(RandoProfileList[i]))
                {
                    randoCandidates.Add(RandoProfileList[i]);
                    if (SaveManager.instance.GetCurrentPlayerState().IsProfileEnabled(RandoProfileList[i]))
                    {
                        randoCandidatesEnabled.Add(RandoProfileList[i]);
                        Debug.Log("ADDED ENABLED RANDO TO ROLL POOL");
                        // nudge the odds lololol
                        //for (int x = 0; x < 15; ++x)
                        //{
                        //    randoCandidates.Add(RandoProfileList[i]);
                        //}
                    }
                }
                else
                {
                    continue;
                }
            }
            if (randoCandidatesEnabled.Count > 0)
            {
                rolledRando = randoCandidatesEnabled[Random.Range(0, randoCandidatesEnabled.Count)];
                Debug.Log("ROLLED ENABLED RANDO");
            }
            else if (randoCandidates.Count > 0)
            {
                rolledRando = randoCandidates[Random.Range(0, randoCandidates.Count)];
            }
            else
            {
                Debug.LogError("RAN OUT OF RANDO PROFILES :'( pls tell writers to write more");
            }
        }
        else
        {
            List<template_profile> randoCandidates = new List<template_profile>();

            List<template_profile> targetRandoList = null;

            if (rando.Template.task_random_profile_generator.profile_generate_occupation == profile_occupation_selector.job_lasthuman)
            {
                targetRandoList = LastHumanProfileList;
            }
            else if (rando.Template.task_random_profile_generator.profile_generate_occupation == profile_occupation_selector.job_plant)
            {
                targetRandoList = PlantProfileList;
            }
            else
            {
                targetRandoList = RandoProfileList;
            }

            for (int i = 0; i < targetRandoList.Count; ++i)
            {
                if (!targetRandoList[i].Template.profile_basic_data.profile_is_enabled)
                {
                    continue;
                }

                switch (rando.Template.task_random_profile_generator.profile_generate_spare_death_both)
                {
                    case profile_generate_spare_death_both.profile_generate_death_only: // death
                    {
                        // death profile
                        if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_ecology_value < rando.Template.task_random_profile_generator.profile_generate_ecology_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_ecology_value > rando.Template.task_random_profile_generator.profile_generate_ecology_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_healthcare_value < rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_healthcare_value > rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_peace_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_peace_value < rando.Template.task_random_profile_generator.profile_generate_peace_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_peace_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_peace_value > rando.Template.task_random_profile_generator.profile_generate_peace_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_prosperity_value < rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_prosperity_value > rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max)
                            {
                                continue;
                            }
                        }

                        break;

                    }
                    case profile_generate_spare_death_both.profile_generate_spare_only: //spare
                    {
                        // spare profile
                        if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_ecology_value < rando.Template.task_random_profile_generator.profile_generate_ecology_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_ecology_value > rando.Template.task_random_profile_generator.profile_generate_ecology_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_healthcare_value < rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_healthcare_value > rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_peace_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_peace_value < rando.Template.task_random_profile_generator.profile_generate_peace_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_peace_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_peace_value > rando.Template.task_random_profile_generator.profile_generate_peace_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_prosperity_value < rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_prosperity_value > rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max)
                            {
                                continue;
                            }
                        }
                        break;
                    }
                    //case profile_generate_spare_death_both.profile_generate_both_death_spare: //both - i have no idea how the fuck imma do dis
                    //{
                    //    // death profile
                    //    if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_min != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_ecology_value < rando.Template.task_random_profile_generator.profile_generate_ecology_value_min &&
                    //            targetRandoList[i].Template.profile_spare_data.profile_spare_ecology_value < rando.Template.task_random_profile_generator.profile_generate_ecology_value_min)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_max != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_ecology_value > rando.Template.task_random_profile_generator.profile_generate_ecology_value_max)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_healthcare_value < rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_healthcare_value > rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_peace_value_min != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_peace_value < rando.Template.task_random_profile_generator.profile_generate_peace_value_min)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_peace_value_max != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_peace_value > rando.Template.task_random_profile_generator.profile_generate_peace_value_max)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_prosperity_value < rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_prosperity_value > rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //
                    //    break;
                    //}

                }




                // basic profile
                if (rando.Template.task_random_profile_generator.profile_generate_occupation != 0)
                {
                    if (targetRandoList[i].Template.profile_basic_data.profile_occupation_selector != rando.Template.task_random_profile_generator.profile_generate_occupation)
                    {
                        continue;
                    }
                }
                if (rando.Template.task_random_profile_generator.profile_generate_younger_than != 0)
                {
                    try
                    {
                        if (int.Parse(targetRandoList[i].Template.profile_basic_data.profile_age_value) > rando.Template.task_random_profile_generator.profile_generate_younger_than)
                        {
                            continue;
                        }
                    }
                    catch
                    {
                        Debug.LogWarning("Could not parse profile age for: " + targetRandoList[i].Template.profile_basic_data.profile_name);
                        continue;
                    }

                }
                if (rando.Template.task_random_profile_generator.profile_generate_older_than != 0)
                {
                    try
                    {
                        if (int.Parse(targetRandoList[i].Template.profile_basic_data.profile_age_value) < rando.Template.task_random_profile_generator.profile_generate_older_than)
                        {
                            continue;
                        }
                    }
                    catch
                    {
                        Debug.LogWarning("Could not parse profile age for: " + targetRandoList[i].Template.profile_basic_data.profile_name);
                        continue;
                    }

                }

                randoCandidates.Add(targetRandoList[i]);
            }

            if (randoCandidates.Count > 0)
            {
                rolledRando = randoCandidates[Random.Range(0, randoCandidates.Count)];

            }
            else
            {
                Debug.LogError("No suitable rando for: '" + rando.DisplayName + "' found! Reason: No candidates matched conditions");
            }
        }

        if (rolledRando != null)
        {
            SaveManager.instance.GetCurrentPlayerState().AddRolledRandoID(rolledRando.Id);
            RemoveRolledRandoFromPool(rolledRando);
        }
        else
        {
            Debug.LogError("No suitable rando found!");
        }
        return rolledRando;
    }


    public int GetRandoCandidatesByTemplate(template_random_profile rando)
    {
        if (rando.Template.task_random_profile_generator.profile_generate_any_random)
        {
            List<template_profile> randoCandidates = new List<template_profile>();
            for (int i = 0; i < RandoProfileList.Count; ++i)
            {
                if (RandoProfileList[i].Template.profile_basic_data.profile_is_enabled && IsProfileModerate(RandoProfileList[i]))
                {
                    randoCandidates.Add(RandoProfileList[i]);
                }
                else
                {
                    continue;
                }
            }
            if (randoCandidates.Count > 0)
            {
                return randoCandidates.Count;
            }
            else
            {
                Debug.LogError("RAN OUT OF RANDO PROFILES :'( pls tell writers to write more");
                return 0;
            }
        }
        else
        {
            List<template_profile> randoCandidates = new List<template_profile>();

            List<template_profile> targetRandoList = null;

            if (rando.Template.task_random_profile_generator.profile_generate_occupation == profile_occupation_selector.job_lasthuman)
            {
                targetRandoList = LastHumanProfileList;
            }
            else if (rando.Template.task_random_profile_generator.profile_generate_occupation == profile_occupation_selector.job_plant)
            {
                targetRandoList = PlantProfileList;
            }
            else
            {
                targetRandoList = RandoProfileList;
            }

            for (int i = 0; i < targetRandoList.Count; ++i)
            {
                if (!targetRandoList[i].Template.profile_basic_data.profile_is_enabled)
                {
                    continue;
                }

                switch (rando.Template.task_random_profile_generator.profile_generate_spare_death_both)
                {
                    case profile_generate_spare_death_both.profile_generate_death_only: // death
                    {
                        // death profile
                        if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_ecology_value < rando.Template.task_random_profile_generator.profile_generate_ecology_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_ecology_value > rando.Template.task_random_profile_generator.profile_generate_ecology_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_healthcare_value < rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_healthcare_value > rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_peace_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_peace_value < rando.Template.task_random_profile_generator.profile_generate_peace_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_peace_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_peace_value > rando.Template.task_random_profile_generator.profile_generate_peace_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_prosperity_value < rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_death_data.profile_death_prosperity_value > rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max)
                            {
                                continue;
                            }
                        }

                        break;

                    }
                    case profile_generate_spare_death_both.profile_generate_spare_only: //spare
                    {
                        // spare profile
                        if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_ecology_value < rando.Template.task_random_profile_generator.profile_generate_ecology_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_ecology_value > rando.Template.task_random_profile_generator.profile_generate_ecology_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_healthcare_value < rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_healthcare_value > rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_peace_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_peace_value < rando.Template.task_random_profile_generator.profile_generate_peace_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_peace_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_peace_value > rando.Template.task_random_profile_generator.profile_generate_peace_value_max)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_prosperity_value < rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min)
                            {
                                continue;
                            }
                        }
                        if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max != 0)
                        {
                            if (targetRandoList[i].Template.profile_spare_data.profile_spare_prosperity_value > rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max)
                            {
                                continue;
                            }
                        }
                        break;
                    }
                    //case profile_generate_spare_death_both.profile_generate_both_death_spare: //both - i have no idea how the fuck imma do dis
                    //{
                    //    // death profile
                    //    if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_min != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_ecology_value < rando.Template.task_random_profile_generator.profile_generate_ecology_value_min &&
                    //            targetRandoList[i].Template.profile_spare_data.profile_spare_ecology_value < rando.Template.task_random_profile_generator.profile_generate_ecology_value_min)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_ecology_value_max != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_ecology_value > rando.Template.task_random_profile_generator.profile_generate_ecology_value_max)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_healthcare_value < rando.Template.task_random_profile_generator.profile_generate_healthcare_value_min)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_healthcare_value > rando.Template.task_random_profile_generator.profile_generate_healthcare_value_max)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_peace_value_min != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_peace_value < rando.Template.task_random_profile_generator.profile_generate_peace_value_min)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_peace_value_max != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_peace_value > rando.Template.task_random_profile_generator.profile_generate_peace_value_max)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_prosperity_value < rando.Template.task_random_profile_generator.profile_generate_prosperity_value_min)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    if (rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max != 0)
                    //    {
                    //        if (targetRandoList[i].Template.profile_death_data.profile_death_prosperity_value > rando.Template.task_random_profile_generator.profile_generate_prosperity_value_max)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //
                    //    break;
                    //}

                }




                // basic profile
                if (rando.Template.task_random_profile_generator.profile_generate_occupation != 0)
                {
                    if (targetRandoList[i].Template.profile_basic_data.profile_occupation_selector != rando.Template.task_random_profile_generator.profile_generate_occupation)
                    {
                        continue;
                    }
                }
                if (rando.Template.task_random_profile_generator.profile_generate_younger_than != 0)
                {
                    try
                    {
                        if (int.Parse(targetRandoList[i].Template.profile_basic_data.profile_age_value) > rando.Template.task_random_profile_generator.profile_generate_younger_than)
                        {
                            continue;
                        }
                    }
                    catch
                    {
                        Debug.LogWarning("Could not parse profile age for: " + targetRandoList[i].Template.profile_basic_data.profile_name);
                        continue;
                    }

                }
                if (rando.Template.task_random_profile_generator.profile_generate_older_than != 0)
                {
                    try
                    {
                        if (int.Parse(targetRandoList[i].Template.profile_basic_data.profile_age_value) < rando.Template.task_random_profile_generator.profile_generate_older_than)
                        {
                            continue;
                        }
                    }
                    catch
                    {
                        Debug.LogWarning("Could not parse profile age for: " + targetRandoList[i].Template.profile_basic_data.profile_name);
                        continue;
                    }

                }

                randoCandidates.Add(targetRandoList[i]);
            }

            if (randoCandidates.Count > 0)
            {
                return randoCandidates.Count;

            }
            else
            {
                Debug.LogError("No suitable rando for: '" + rando.DisplayName + "' found! Reason: No candidates matched conditions");
                return 0;
            }
        }

    }
}
