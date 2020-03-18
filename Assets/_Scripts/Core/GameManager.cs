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
using Articy.Project_Of_Death.GlobalVariables;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameMode
{
    RELEASE,
    DEMO,
}

public enum EAspectRatio
{
    a21_9,
    a16_9,
    a16_10,
    a3_2,
    a4_3,
    a5_4,
}

public class GameManager : ManagerBase
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;

    }

    public EGameMode GameMode;
    [SerializeField]
    public int DemoDaysCount = 7;
    [SerializeField]
    public bool bDebugMode = false;
    public List<ManagerBase> ManagerList = new List<ManagerBase>();

    List<template_day> Days = new List<template_day>();

    [SerializeField]
    private ArticyRef MasterReference;

    [SerializeField]
    private ArticyRef PostGameDialogueReference;
    public Dialogue PostGameDialogue;
    [SerializeField]
    private ArticyRef PostGameDialogueSubplotReference;
    public Dialogue PostGameDialogueSubplot;

    [SerializeField]
    private ArticyRef GrimReaperPaperworkProfileRef;
    public template_profile GrimReaperPaperworkProfile;
    [SerializeField]
    private ArticyRef FatePaperworkProfileRef;
    public template_profile FatePaperworkProfile;
    [SerializeField]
    private ArticyRef LanzoPaperworkProfileRef;
    public template_profile LanzoPaperworkProfile;

    [SerializeField]
    public Sprite GrimReaperProfileAvatar;

    [SerializeField]
    public string Oof;

    [SerializeField]
    public Color PaletteRedRegular;
    [SerializeField]
    public Color PaletteYellowRegular;
    [SerializeField]
    public Color PaletteBlueRegular;

    [SerializeField]
    public int[] ChaosThresholds = new int[(int)EChaosThreshold.MAX] { -350, -100, 100, 350 };

    Dialogue IntroDialogue = null;
    Dialogue NewGamePlusIntroDialogue = null;

    template_day CurrentDay = null;
    public System.DateTime TimeStart;

    public bool bLoreMode;

    public EAspectRatio CurrentAspectRatio;

    // Start is called before the first frame update
    void Start()
    {
        InitManager();
    }

    public override void InitManager()
    {
        ArticyDatabase.Localization.Language = "en";

        TimeStart = System.DateTime.Now;
        //ResetArticyVariables();

        base.InitManager();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;




        if (MasterReference.HasReference)
        {
            FlowFragment masterFragment = MasterReference.GetObject<FlowFragment>();

            if (masterFragment != null)
            {
                //Debug.Log("Found master fragment: " + masterFragment.ToString());
                IntroDialogue = masterFragment.Children[0] as Dialogue;
                for (int i = 0; i < masterFragment.OutputPins.Count; ++i)
                {
                    for (int j = 0; j < masterFragment.OutputPins[i].Connections.Count; ++j)
                    {
                        SetCurrentDay(masterFragment.OutputPins[i].Connections[j].Target as template_day);
                    }
                }


                
            }

            if (GrimReaperPaperworkProfileRef.HasReference)
            {
                GrimReaperPaperworkProfile = GrimReaperPaperworkProfileRef.GetObject<template_profile>();
            }
            if (FatePaperworkProfileRef.HasReference)
            {
                FatePaperworkProfile = FatePaperworkProfileRef.GetObject<template_profile>();
            }

            if (LanzoPaperworkProfileRef.HasReference)
            {
                LanzoPaperworkProfile = LanzoPaperworkProfileRef.GetObject<template_profile>();
            }

            if (PostGameDialogueReference.HasReference)
            {
                PostGameDialogue = PostGameDialogueReference.GetObject<Dialogue>();
            }

            if (PostGameDialogueSubplotReference.HasReference)
            {
                PostGameDialogueSubplot = PostGameDialogueSubplotReference.GetObject<Dialogue>();
            }
        }


        List<ManagerBase> managers = new List<ManagerBase>();
        for (int i = 0; i < ManagerList.Count; ++i)
        {
            ManagerBase newManager = Instantiate(ManagerList[i]);
            managers.Add(newManager);
        }

        for (int i = 0; i < managers.Count; ++i)
        {
            managers[i].InitManager();
        }





        //HUDManager.instance.ToggleHUD(false);

    }

    public void SetCurrentDay(template_day day)
    {
        if(day == null)
        {
            Debug.LogError("NULL DAY BEING SET");
        }
        CurrentDay = day;
    }

    public void RestartGame(bool newGamePlus = false)
    {
        //ResetArticyVariables();
        //int spawnCounter = SaveManager.instance.GetCurrentPlayerState().GetSpawnCounter();
        //int demoCounter = SaveManager.instance.GetCurrentPlayerState().GetRestartCounter();
        //SaveManager.instance.ClearSave();
        //SaveManager.instance.GetCurrentPlayerState().SetSpawnCounter(spawnCounter);
        //SaveManager.instance.GetCurrentPlayerState().SetRestartCounter(demoCounter);

        //SaveManager.instance.MarkSavegameDirty();

        if (newGamePlus)
        {
            SaveManager.instance.NotifyNewGamePlusPending();
        }
        else
        {

        }
        World.instance.DestroyAllDDOL();
        SceneManager.LoadScene("World");
    }

    public void ResetArticyVariables()
    {
        ArticyGlobalVariables.Default.ResetVariables();
        if (SaveManager.instance != null)
        {
            SaveManager.instance.MarkSavegameDirty();
        }
    }

    public void NotifyEndDay()
    {
        SetCurrentDay(ProgressDay(CurrentDay));
        SaveManager.instance.MarkSavegameDirty();
    }


    Vector3 RetroBackgroundScale = new Vector3(1.4f, 1.4f, 1.4f);

    Vector3 UltraRetroBackgroundScale = new Vector3(1.3f, 1.3f, 1.3f);

    Vector3 RegularMortimerBackgroundScale = new Vector3(1.22f, 1.22f, 1.22f);
    Vector3 NotUltraWideMortimerBackgroundScale = new Vector3(1.4f, 1.4f, 1.4f);
    Vector3 UltraWideMortimerBackgroundScale = new Vector3(1.6f, 1.6f, 1.6f);

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.aspect >= 2.3f) // 21:9 and up
        {
            CurrentAspectRatio = EAspectRatio.a21_9;
        }
        else if (Camera.main.aspect >= 1.7f) // 16:9
        {
            CurrentAspectRatio = EAspectRatio.a16_9;
        }
        else if (Camera.main.aspect >= 1.6f) // 16:10
        {
            CurrentAspectRatio = EAspectRatio.a16_10;
        }
        else if (Camera.main.aspect >= 1.5f) // 3:2
        {
            CurrentAspectRatio = EAspectRatio.a3_2;
        }
        else if (Camera.main.aspect >= 1.3f) // 4:3
        {
            CurrentAspectRatio = EAspectRatio.a4_3;
        }
        else // 5:4
        {
            CurrentAspectRatio = EAspectRatio.a5_4;
        }

        if (SceneManager.GetActiveScene().name == "Game" || SceneManager.GetActiveScene().name.Contains("Credits"))
        {
            switch (CurrentAspectRatio)
            {
                case EAspectRatio.a21_9:
                    Elevator.instance.BackgroundRenderer.gameObject.transform.localScale = RetroBackgroundScale;
                    Shop.instance.BackgroundRenderer.gameObject.transform.localScale = RetroBackgroundScale;
                    Mirror.instance.BackgroundRenderer.gameObject.transform.localScale = RetroBackgroundScale;
                    if (Camera.main.orthographicSize != 5.4f)
                    {
                        Camera.main.orthographicSize = 5.4f;
                    }
                    break;
                case EAspectRatio.a16_9:
                    Elevator.instance.BackgroundRenderer.gameObject.transform.localScale = Vector3.one;
                    Shop.instance.BackgroundRenderer.gameObject.transform.localScale = Vector3.one;
                    Mirror.instance.BackgroundRenderer.gameObject.transform.localScale = Vector3.one;
                    if (Camera.main.orthographicSize != 5.4f)
                    {
                        Camera.main.orthographicSize = 5.4f;
                    }
                    break;
                case EAspectRatio.a16_10:
                    Elevator.instance.BackgroundRenderer.gameObject.transform.localScale = UltraRetroBackgroundScale;
                    Shop.instance.BackgroundRenderer.gameObject.transform.localScale = UltraRetroBackgroundScale;
                    Mirror.instance.BackgroundRenderer.gameObject.transform.localScale = UltraRetroBackgroundScale;
                    if (Camera.main.orthographicSize != 6.0f)
                    {
                        Camera.main.orthographicSize = 6.0f;
                    }
                    break;
                case EAspectRatio.a3_2:
                    Elevator.instance.BackgroundRenderer.gameObject.transform.localScale = UltraRetroBackgroundScale;
                    Shop.instance.BackgroundRenderer.gameObject.transform.localScale = UltraRetroBackgroundScale;
                    Mirror.instance.BackgroundRenderer.gameObject.transform.localScale = UltraRetroBackgroundScale;
                    if (Camera.main.orthographicSize != 6.4f)
                    {
                        Camera.main.orthographicSize = 6.4f;
                    }
                    break;
                case EAspectRatio.a4_3:
                    Elevator.instance.BackgroundRenderer.gameObject.transform.localScale = UltraRetroBackgroundScale;
                    Shop.instance.BackgroundRenderer.gameObject.transform.localScale = UltraRetroBackgroundScale;
                    Mirror.instance.BackgroundRenderer.gameObject.transform.localScale = UltraRetroBackgroundScale;
                    if (Camera.main.orthographicSize != 7.0f)
                    {
                        Camera.main.orthographicSize = 7.0f;
                    }
                    break;
                case EAspectRatio.a5_4:
                    Elevator.instance.BackgroundRenderer.gameObject.transform.localScale = RetroBackgroundScale;
                    Shop.instance.BackgroundRenderer.gameObject.transform.localScale = RetroBackgroundScale;
                    Mirror.instance.BackgroundRenderer.gameObject.transform.localScale = RetroBackgroundScale;
                    if (Camera.main.orthographicSize != 7.6f)
                    {
                        Camera.main.orthographicSize = 7.6f;
                    }
                    break;
            }
        }
        else/* if (SceneManager.GetActiveScene().name.Contains("Comic"))*/
        {



            switch (CurrentAspectRatio)
            {
                case EAspectRatio.a21_9:
                    if (Camera.main.orthographicSize != 6.5f)
                    {
                        Camera.main.orthographicSize = 6.5f;
                    }
                    if (SceneManager.GetActiveScene().name.Contains("PostGame"))
                    {
                        if (MortimerPostGame.instance != null)
                        {
                            MortimerPostGame.instance.BackgroundRenderer.gameObject.transform.localScale = UltraWideMortimerBackgroundScale;
                        }
                    }
                    break;
                case EAspectRatio.a16_9:
                    if (Camera.main.orthographicSize != 6.5f)
                    {
                        Camera.main.orthographicSize = 6.5f;
                    }
                    if (SceneManager.GetActiveScene().name.Contains("PostGame"))
                    {
                        if (MortimerPostGame.instance != null)
                        {
                            MortimerPostGame.instance.BackgroundRenderer.gameObject.transform.localScale = RegularMortimerBackgroundScale;
                        }
                    }
                    break;
                case EAspectRatio.a16_10:
                    if (Camera.main.orthographicSize != 6.5f)
                    {
                        Camera.main.orthographicSize = 6.5f;
                    }
                    if (SceneManager.GetActiveScene().name.Contains("PostGame"))
                    {
                        if (MortimerPostGame.instance != null)
                        {
                            MortimerPostGame.instance.BackgroundRenderer.gameObject.transform.localScale = NotUltraWideMortimerBackgroundScale;
                        }
                    }
                    break;
                case EAspectRatio.a3_2:
                    if (Camera.main.orthographicSize != 6.5f)
                    {
                        Camera.main.orthographicSize = 6.5f;
                    }
                    if (SceneManager.GetActiveScene().name.Contains("PostGame"))
                    {
                        if (MortimerPostGame.instance != null)
                        {
                            MortimerPostGame.instance.BackgroundRenderer.gameObject.transform.localScale = NotUltraWideMortimerBackgroundScale;
                        }
                    }
                    break;
                case EAspectRatio.a4_3:
                    if (Camera.main.orthographicSize != 7.2f)
                    {
                        Camera.main.orthographicSize = 7.2f;
                    }
                    if (SceneManager.GetActiveScene().name.Contains("PostGame"))
                    {
                        if (MortimerPostGame.instance != null)
                        {
                            MortimerPostGame.instance.BackgroundRenderer.gameObject.transform.localScale = NotUltraWideMortimerBackgroundScale;
                        }
                    }
                    break;
                case EAspectRatio.a5_4:
                    if (Camera.main.orthographicSize != 7.6f)
                    {
                        Camera.main.orthographicSize = 7.6f;
                    }
                    if (SceneManager.GetActiveScene().name.Contains("PostGame"))
                    {
                        if (MortimerPostGame.instance != null)
                        {
                            MortimerPostGame.instance.BackgroundRenderer.gameObject.transform.localScale = NotUltraWideMortimerBackgroundScale;
                        }
                    }
                    break;
            }
        }


        //if(GameMode == EGameMode.DEMO && SaveManager.instance != null && SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() >= 8)
        //{
        //    Debug.Log("Quit lol");
        //    Application.Quit();
        //}
    }

    public template_day ProgressDay(template_day currentDay)
    {
        if (currentDay.OutputPins.Count == 0 || currentDay.OutputPins[0].Connections.Count == 0)
        {
            Debug.LogError("Broken day! No connection! " + currentDay.DisplayName);
            return null;
        }
        template_day day = currentDay.OutputPins[0].Connections[0].Target as template_day;
        //Hub hub = currentDay.OutputPins[0].Connections[0].Target as Hub;
        Condition condition = currentDay.OutputPins[0].Connections[0].Target as Condition;
        currentDay.OutputPins[0].Evaluate();

        //Debug.Log("fate absent: " + ArticyGlobalVariables.Default.game.fate_absent);
        //Debug.Log("thirteen no profiles: " + ArticyGlobalVariables.Default.day.thirteen_no_profiles);

        while (condition != null)
        {
            bool conditionResult = condition.Evaluate();

            if (conditionResult)
            {
                condition.OutputPins[0].Evaluate();
                day = condition.OutputPins[0].Connections[0].Target as template_day;
                //hub = condition.OutputPins[0].Connections[0].Target as Hub;
                condition = condition.OutputPins[0].Connections[0].Target as Condition;
            }
            else
            {
                condition.OutputPins[1].Evaluate();
                day = condition.OutputPins[1].Connections[0].Target as template_day;
                //hub = condition.OutputPins[1].Connections[0].Target as Hub;
                condition = condition.OutputPins[1].Connections[0].Target as Condition;
            }
        }
        SaveManager.instance.MarkSavegameDirty();
        if (day != null)
        {
            return day;
        }
        //if (hub != null)
        //{
        //    return hub;
        //}
        Debug.LogError("No day found! Ott fix pls");
        return null;
    }

    //public template_day GetDay(int dayIndex)
    //{
    //    return Days[dayIndex];
    //}

    //public List<template_day> GetDays()
    //{
    //    return Days;
    //}

    //public template_day GetCurrentDay()
    //{
    //    return Days[SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex()];
    //}

    public template_day GetCurrentDay()
    {
        return CurrentDay;
    }

    public Dialogue GetCurrentDayDialogue()
    {
        return GetCurrentDay().Template.day.day_fate_slot as Dialogue;
    }

    public template_day_task GetCurrentDayTask()
    {
        return GetCurrentDay().Template.day.day_task_slot as template_day_task;
    }

    public Dialogue GetIntroDialogue()
    {
        return IntroDialogue;
    }

    public bool ShouldGetPaid()
    {
        return ArticyGlobalVariables.Default.day.death_count == GetCurrentDayTask().Template.task.task_death_count || GetCurrentDayTask().Template.task.task_death_count == 0;
    }

    public float GetLoyaltyFactor()
    {
        if (ArticyGlobalVariables.Default.day.death_count >= DesktopManager.instance.GetProfileCountForCurrentDay())
        {
            return 0.0f;
        }
        else if (ArticyGlobalVariables.Default.day.death_count <= 0)
        {
            return 0.0f;
        }
        else if (ArticyGlobalVariables.Default.day.death_count == GetCurrentDayTask().Template.task.task_death_count)
        {
            return 1.0f;
        }
        else if (ArticyGlobalVariables.Default.day.death_count > GetCurrentDayTask().Template.task.task_death_count)
        {
            return GetCurrentDayTask().Template.task.task_death_count / ArticyGlobalVariables.Default.day.death_count;
        }
        else if (ArticyGlobalVariables.Default.day.death_count < GetCurrentDayTask().Template.task.task_death_count)
        {
            return ArticyGlobalVariables.Default.day.death_count / GetCurrentDayTask().Template.task.task_death_count;
        }
        else
        {
            Debug.LogError("Invalid case for loyalty calculation! Contact the Ott");
            return 1.0f;
        }

    }
}
