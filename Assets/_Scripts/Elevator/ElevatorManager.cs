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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EScene
{
    Elevator,
    Desktop,
    Office,
    DressingRoom,
    Shop,
    Comic,
    Intro,
    PostGame,
    MAX
}

public class ElevatorManager : ManagerBase
{
    public static ElevatorManager instance;

    [SerializeField]
    List<GameScene> SceneList = new List<GameScene>();

    [SerializeField]
    Image BlackOverlay;

    Dictionary<EScene, GameScene> SpawnedSceneList = new Dictionary<EScene, GameScene>();

    GameScene CurrentScene = null;
    bool bIsFadingIn = false;
    bool bIsFadingOut = false;

    public bool bIsChangingScene = false;

    bool bEndDay = false;

    float CurrentSceneTime = 0.0f;

    private void Awake()
    {
        instance = this;
    }

    public float GetCurrentSceneTime()
    {
        return CurrentSceneTime;
    }

    public void ResetCurrentSceneTime()
    {
        CurrentSceneTime = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CurrentSceneTime += Time.deltaTime;
    }

    public override void InitManager()
    {
        base.InitManager();

        for (int i = 0; i < SceneList.Count; ++i)
        {

            GameScene scene = Instantiate(SceneList[i]);
            SpawnedSceneList.Add(scene.SceneType, scene);
            scene.InitScene();
        }

        CurrentScene = GetGameSceneByType(EScene.Intro);
    }

    public GameScene GetGameSceneByType(EScene type)
    {
        return SpawnedSceneList[type];
    }

    public EScene GetCurrentScene()
    {
        return CurrentScene.SceneType;
    }


    public void SwitchScene(EScene destinationScene, bool endDay = false)
    {
        Elevator.instance.bIsMovingToFloor = false;
        if (bIsChangingScene)
        {
            return;
        }
        InputManager.instance.LastHitInteractable = null;
        bEndDay = endDay;

        bIsChangingScene = true;
        ResetCurrentSceneTime();

        Elevator.instance.UpdatePlayerChibi();
        StartCoroutine(SwitchSceneRoutine(destinationScene));
    }

    public void HideSceneByType(EScene scene)
    {
        GameScene gameScene = GetGameSceneByType(scene);
        if (gameScene.gameObject.activeSelf)
        {
            gameScene.HideScene();
        }
        else
        {
            gameScene.ShowScene();
        }
    }

    public IEnumerator SwitchSceneRoutine(EScene destinationScene)
    {
        EScene previousScene = EScene.MAX;
        SaveManager.instance.GetCurrentPlayerState().SetLastOpenScene(destinationScene);
        AudioManager.instance.NotifySceneChange(destinationScene);
        SaveManager.instance.GetCurrentCarryoverPlayerState().UpdateArticyCurrentHead();
        SaveManager.instance.GetCurrentCarryoverPlayerState().UpdateArticyCurrentBody();

        yield return StartCoroutine(FadeInBackgroundRoutine(0.35f));
        if(destinationScene != EScene.Desktop)
        {
            HUDManager.instance.ToggleLore.gameObject.SetActive(false);
            
        }
        switch (destinationScene)
        {
            case EScene.Elevator:
                HUDManager.instance.ToggleHUD(true);
                Mirror.instance.CheckDialoguePendingStatus();
                SaveManager.instance.GetCurrentPlayerState().SetHasMirrorNotification(Mirror.instance.IsDialoguePending());

                
                if (ArticyGlobalVariables.Default.inventory.mirror)
                {
                    Elevator.instance.GetElevatorButtonBySceneType(EScene.DressingRoom).ToggleEnable(true);
                }
                else
                {
                    Elevator.instance.GetElevatorButtonBySceneType(EScene.DressingRoom).ToggleEnable(false);
                }

                break;
            case EScene.Desktop:
                HUDManager.instance.ToggleHUD(true);
                VoteCounter.instance.gameObject.SetActive(SaveManager.instance.CurrentOptions.StreamerMode);
                GrimDesk.instance.UpdateDeskItemOwnedStatus();
                DeskLampLight.instance.CheckSinBulbActive();
                HUDManager.instance.ToggleLore.gameObject.SetActive(false);
                break;
            case EScene.Office:
                HUDManager.instance.ToggleHUD(false);
                break;
            case EScene.DressingRoom:
                HUDManager.instance.ToggleHUD(true);
                break;
            case EScene.Shop:
                HUDManager.instance.ToggleHUD(true);
                break;
            case EScene.Comic:
                break;
            case EScene.Intro:
                break;
            case EScene.MAX:
                break;
        }
        
        if(bEndDay)
        {
            bEndDay = false;
            if (SaveManager.instance.GetCurrentPlayerState().IsDayDone())
            {
                SaveManager.instance.GetCurrentPlayerState().IncrementDayIndex();
                DesktopManager.instance.StartDay();
                SaveManager.instance.GetCurrentPlayerState().SetHasVisitedShopkeeperToday(false);
                Elevator.instance.GetElevatorButtonBySceneType(EScene.Desktop).ToggleEnable(true);
                Elevator.instance.GetElevatorButtonBySceneType(EScene.Office).ToggleEnable(false);
                //Elevator.instance.GetElevatorButtonBySceneType(EScene.Elevator).ToggleEnable(false);
                Elevator.instance.GetElevatorButtonBySceneType(EScene.Shop).ToggleEnable(true);
            }
        }
        InputManager.instance.LastHitInteractable = null;
        yield return new WaitForSeconds(0.15f);
        InputManager.instance.LastHitInteractable = null;
        if (CurrentScene != null)
        {
            CurrentScene.HideScene();
            previousScene = CurrentScene.SceneType;
        }
        CurrentScene = GetGameSceneByType(destinationScene);
        if (CurrentScene != null)
        {
            CurrentScene.ShowScene();
        }

        yield return StartCoroutine(FadeOutBackgroundRoutine(0.35f));
        yield return new WaitForSeconds(0.25f);

        switch (destinationScene)
        {
            case EScene.Elevator:
                //HUDManager.instance.ToggleHUD(true);

                break;
            case EScene.Desktop:
                //HUDManager.instance.ToggleHUD(true);
                Phone.instance.StartNewsQueueTimer();
                MarkerOfDeath.instance.MarkerAppear();
                ShopNotification.instanceOffice.ToggleVisible(false);
                break;
            case EScene.Office:
                //HUDManager.instance.ToggleHUD(true);
                break;
            case EScene.DressingRoom:
                //HUDManager.instance.ToggleHUD(true);
                ShopNotification.instanceMirror.ToggleVisible(false);
                break;
            case EScene.Shop:
                //HUDManager.instance.ToggleHUD(true);
                break;
            case EScene.Comic:
                //HUDManager.instance.ToggleHUD(false);

                break;
            case EScene.Intro:
                //HUDManager.instance.ToggleHUD(true);
                //SaveManager.instance.ClearSave();
                AudioManager.instance.NotifyGamePaused();
                break;
            case EScene.MAX:
                break;
        }
        bIsChangingScene = false;
    }

    public IEnumerator FadeOutBackgroundRoutine(float duration)
    {
        bIsFadingOut = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            BlackOverlay.color = new Color(BlackOverlay.color.r, BlackOverlay.color.g, BlackOverlay.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            yield return null;
        }
        bIsFadingOut = false;
    }

    public IEnumerator FadeInBackgroundRoutine(float duration)
    {
        bIsFadingIn = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            BlackOverlay.color = new Color(BlackOverlay.color.r, BlackOverlay.color.g, BlackOverlay.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            yield return null;
        }
        bIsFadingIn = false;
    }
}
