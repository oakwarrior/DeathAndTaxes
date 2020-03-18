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


public enum EComparisonOperation
{
    GREATER_THAN,
    GREATER_OR_EQUAL_THAN,
    LESSER_THAN,
    LESSER_OR_EQUAL_THAN,
    EQUAL
}

public class DialogueManager : ManagerBase
{
    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    ArticyRef YouRef;
    [SerializeField]
    ArticyRef FateRef;
    [SerializeField]
    ArticyRef FatePhoneRef;
    [SerializeField]
    ArticyRef CatRef;
    [SerializeField]
    ArticyRef ConscienceRef;
    [SerializeField]
    ArticyRef ShopkeeperRef;
    [SerializeField]
    ArticyRef CatToyRef;

    public Entity YouEntity;
    public Entity FateEntity;
    public Entity CatToyEntity;
    public Entity FatePhoneEntity;
    public Entity CatEntity;
    public Entity ConscienceEntity;
    public Entity ShopkeeperEntity;

    public Dialogue CurrentDialogue;
    DialogueFragment CurrentDialogueFragment;

    int CurrentDialogueSectionIndex = 0;

    bool bStartDayOnEnd = false;

    //public bool bEndDemo = false;

    public override void InitManager()
    {
        base.InitManager();
        if (YouRef.HasReference)
        {
            YouEntity = YouRef.GetObject<Entity>();
        }
        if (FateRef.HasReference)
        {
            FateEntity = FateRef.GetObject<Entity>();
        }
        if (CatToyRef.HasReference)
        {
            CatToyEntity = CatToyRef.GetObject<Entity>();
        }
        if (FatePhoneRef.HasReference)
        {
            FatePhoneEntity = FatePhoneRef.GetObject<Entity>();
        }
        if (ConscienceRef.HasReference)
        {
            ConscienceEntity = ConscienceRef.GetObject<Entity>();
        }
        if (ShopkeeperRef.HasReference)
        {
            ShopkeeperEntity = ShopkeeperRef.GetObject<Entity>();
        }
        if(CatRef.HasReference)
        {
            CatEntity = CatRef.GetObject<Entity>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartIntro()
    {
        StartCoroutine(StartIntroRoutine());

    }

    IEnumerator StartIntroRoutine()
    {
        DialogueScreen.instance.ShowDialogueScreen();
        yield return new WaitForSeconds(0.75f);
        yield return StartCoroutine(DialogueScreen.instance.FadeInSpeakerRoutine(1.0f));
        StartDialogue(GameManager.instance.GetIntroDialogue(), true);
    }

    public void StartDayEndDialogue(bool startDayOnEnd = true)
    {
        StartCoroutine(StartDayEndDialogueRoutine(startDayOnEnd));
    }

    IEnumerator StartDayEndDialogueRoutine(bool startDayOnEnd = true)
    {
        DialogueScreen.instance.ShowDialogueScreen();
        DialogueScreen.instance.HideContinueButton();
        DialogueScreen.instance.HideChoiceButtons();
        yield return StartCoroutine(DialogueScreen.instance.FadeInBackgroundRoutine(1.0f));
        yield return StartCoroutine(DialogueScreen.instance.FadeInSpeakerRoutine(1.0f));
        StartDialogue(GameManager.instance.GetCurrentDayDialogue(), startDayOnEnd);

        //bEndDemo = dayIndex >= 7;
    }

    public void ClearDialogueData()
    {
        CurrentDialogue = null;
        CurrentDialogueSectionIndex = 0;
    }

    public void ProceedCurrentDialogue(DialogueFragment choice = null)
    {
        ProceedDialogue(choice);
    }

    public ArticyObject GetNextDialogueElementFromFragment(DialogueFragment currentFragment)
    {
        if (currentFragment.OutputPins[0].Connections.Count == 0)
        {
            return null;
        }
        DialogueFragment fragment = currentFragment.OutputPins[0].Connections[0].Target as DialogueFragment;
        Hub hub = currentFragment.OutputPins[0].Connections[0].Target as Hub;
        Condition condition = currentFragment.OutputPins[0].Connections[0].Target as Condition;
        Jump jump = currentFragment.OutputPins[0].Connections[0].Target as Jump;

        while (condition != null)
        {
            bool conditionResult = condition.Evaluate();

            if (conditionResult)
            {
                condition.OutputPins[0].Evaluate();
                fragment = condition.OutputPins[0].Connections[0].Target as DialogueFragment;
                hub = condition.OutputPins[0].Connections[0].Target as Hub;
                jump = condition.OutputPins[0].Connections[0].Target as Jump;
                condition = condition.OutputPins[0].Connections[0].Target as Condition;
                if (jump != null)
                {
                    condition = jump.Target as Condition;
                }
            }
            else
            {
                condition.OutputPins[1].Evaluate();
                fragment = condition.OutputPins[1].Connections[0].Target as DialogueFragment;
                hub = condition.OutputPins[1].Connections[0].Target as Hub;
                jump = condition.OutputPins[1].Connections[0].Target as Jump;
                condition = condition.OutputPins[1].Connections[0].Target as Condition;
                if (jump != null)
                {
                    condition = jump.Target as Condition;
                }
            }
        }

        if (jump != null)
        {
            fragment = jump.Target as DialogueFragment;
            hub = jump.Target as Hub;
        }

        if (fragment != null)
        {
            fragment.OutputPins[0].Evaluate();
            return fragment;
        }
        if (hub != null)
        {
            hub.OutputPins[0].Evaluate();
            return hub;
        }


        return null;
    }

    public void ProceedDialogue(DialogueFragment choiceFragment = null)
    {

        CurrentDialogueFragment.OutputPins[0].Evaluate();
        if (choiceFragment == null)
        {

        }
        else
        {
            CurrentDialogueFragment = choiceFragment;
            CurrentDialogueFragment.OutputPins[0].Evaluate();
        }

        ArticyObject element = GetNextDialogueElementFromFragment(CurrentDialogueFragment);

        if(SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() == 22)
        {
            IntroController.instance.CheckCustomNameBooleans();
        }

        DialogueFragment fragment = element as DialogueFragment;
        Hub hub = element as Hub;

        if (fragment != null)
        {
            CurrentDialogueFragment = fragment;
            DialogueScreen.instance.UpdateFromDialogueFragment(CurrentDialogueFragment);
        }
        else if (hub != null)
        {
            DialogueScreen.instance.UpdateFromHub(CurrentDialogueFragment);
        }
        else
        {
            EndDialogueInstantly();
        }
    }

    public void RequestEndDialogue()
    {

    }

    public void EndDialogueInstantly()
    {
        bool demoEnd = SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex() >= GameManager.instance.DemoDaysCount - 1 || ArticyGlobalVariables.Default.game.game_over_fired;
        if (GameManager.instance.GameMode == EGameMode.DEMO)
        {
            DialogueScreen.instance.NotifyEndDialogue(EScene.Desktop, demoEnd);
        }
        else
        {
            int salaryAmount = 0;
            if (CurrentDialogue == GameManager.instance.GetIntroDialogue())
            {
                DialogueScreen.instance.NotifyEndDialogue(EScene.Desktop);
            }
            else
            {
                if(GameManager.instance.ShouldGetPaid())
                {
                    if (ArticyGlobalVariables.Default.game.got_salary_raise)
                    {
                        int salary = 400;
                        if(SaveManager.instance.GetCurrentCarryoverPlayerState().GetTaskDeviationCounter() > 0)
                        {
                            salary = Mathf.Max(0, salary - SaveManager.instance.GetCurrentCarryoverPlayerState().GetTaskDeviationCounter() * 100);
                        }
                        SaveManager.instance.GetCurrentPlayerState().ModifyMoney(salary);
                        salaryAmount += salary;
                    }
                    else
                    {
                        int salary = 300;
                        if (SaveManager.instance.GetCurrentCarryoverPlayerState().GetTaskDeviationCounter() > 0)
                        {
                            salary = Mathf.Max(0, salary - SaveManager.instance.GetCurrentCarryoverPlayerState().GetTaskDeviationCounter() * 100);
                        }
                        SaveManager.instance.GetCurrentPlayerState().ModifyMoney(salary);
                        salaryAmount += salary;
                    }

                }

                if (ArticyGlobalVariables.Default.game.game_over_fired)
                {


                    //SaveManager.instance.GetCurrentPlayerState().TransferAllMarkedProfilesToCarryOver();

                    SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameStarted(false);
                    if(!SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameFinishedOnce() && 
                        !SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameFinishedOnceProperly())
                    {
                        GameManager.instance.ResetArticyVariables();
                    }
                    else
                    {
                        ArticyGlobalVariables.Default.game.game_over_fired = false;
                    }
                    SaveManager.instance.ForceSave(); // TODO: check if safe

                    GameManager.instance.RestartGame();
                }
                else
                {
                    if (ArticyGlobalVariables.Default.game.salary_bonus_pending)
                    {
                        ArticyGlobalVariables.Default.game.salary_bonus_pending = false;
                        SaveManager.instance.GetCurrentPlayerState().ModifyMoney(500);
                        salaryAmount += 500;
                    }

                    if (ArticyGlobalVariables.Default.game.backpay)
                    {
                        salaryAmount += ArticyGlobalVariables.Default.game.backpay_amount;
                        SaveManager.instance.GetCurrentPlayerState().ReleaseBackpay();
                    }

                    if(ArticyGlobalVariables.Default.game.salary_on)
                    {
                        MoneyNotification.instance.ShowMoneyNotification(salaryAmount);
                    }

                    if (SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() >= 28)
                    {

                        //SaveManager.instance.GetCurrentPlayerState().TransferAllMarkedProfilesToCarryOver();

                        if (ArticyGlobalVariables.Default.game.finished)
                        {
                            SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameFinishedOnceProperly(true);
                        }
                        else
                        {

                        }
                        SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameFinished(true);
                    }

                    if (SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameFinished())
                    {
                        DialogueScreen.instance.NotifyEndDialogue(EScene.Comic);
                    }
                    else
                    {
                        DialogueScreen.instance.NotifyEndDialogue(EScene.Elevator);
                    }
                }


            }
        }

        if (CurrentDialogue == GameManager.instance.GetIntroDialogue())
        {
            SaveManager.instance.GetCurrentPlayerState().SetIntroDone(true);
            DesktopManager.instance.StartDay();
        }
        else
        {
            Elevator.instance.GetElevatorButtonBySceneType(EScene.Office).ToggleEnable(false);
            SaveManager.instance.GetCurrentPlayerState().SetDayDone(true);
            if (GameManager.instance.GameMode == EGameMode.DEMO)
            {
                if (!demoEnd)
                {
                    SaveManager.instance.GetCurrentPlayerState().IncrementDayIndex();
                    DesktopManager.instance.StartDay();
                }
            }
        }

        ClearDialogueData();

        AudioManager.instance.UpdateMusicVolume();

    }

    public bool IsDialogueActive()
    {
        return CurrentDialogue != null;
    }

    public void StartDialogue(Dialogue dialogue, bool startOnDayEnd)
    {
        Cursor.visible = true;


        CurrentDialogue = dialogue;

        DialogueFragment startFragment = GetStartFragmentForDialogue(dialogue);

        CurrentDialogueFragment = startFragment;
        AudioManager.instance.UpdateMusicVolume();
        DialogueScreen.instance.UpdateFromDialogueFragment(CurrentDialogueFragment);

        bStartDayOnEnd = startOnDayEnd;
    }

    public DialogueFragment GetStartFragmentForDialogue(Dialogue dialogue)
    {
        Condition condition = dialogue.InputPins[0].Connections[0].Target as Condition;
        Jump jump = dialogue.InputPins[0].Connections[0].Target as Jump;
        DialogueFragment dialogueFragment = dialogue.InputPins[0].Connections[0].Target as DialogueFragment;

        while (condition != null)
        {
            bool conditionResult = condition.Evaluate();

            if (conditionResult)
            {
                condition.OutputPins[0].Evaluate();
                dialogueFragment = condition.OutputPins[0].Connections[0].Target as DialogueFragment;
                jump = condition.OutputPins[0].Connections[0].Target as Jump;
                condition = condition.OutputPins[0].Connections[0].Target as Condition;
            }
            else
            {
                condition.OutputPins[1].Evaluate();
                dialogueFragment = condition.OutputPins[1].Connections[0].Target as DialogueFragment;
                jump = condition.OutputPins[1].Connections[0].Target as Jump;
                condition = condition.OutputPins[1].Connections[0].Target as Condition;
            }

            if (jump != null)
            {
                condition = jump.Target as Condition;
                dialogueFragment = jump.Target as DialogueFragment;
            }
        }

        dialogueFragment.OutputPins[0].Evaluate();
        return dialogueFragment;
    }


}
