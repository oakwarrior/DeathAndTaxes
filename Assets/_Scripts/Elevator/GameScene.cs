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

public class GameScene : MonoBehaviour
{
    [SerializeField]
    public EScene SceneType;
    [SerializeField]
    List<Canvas> CanvasList = new List<Canvas>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideScene()
    {
        gameObject.SetActive(false);

        switch (SceneType)
        {
            case EScene.Elevator:
                break;
            case EScene.Desktop:
                Spinner.instance.StopSpinner();
                AudioManager.instance.UpdateChaosGlobeLoopVolume(0);
                ChaosGlobe.instance.LoopClipFactor = 0.0f;
                PaperworkManager.instance.CleanUpPaperwork();
                break;
            case EScene.Office:
                break;
            case EScene.DressingRoom:
                break;
            case EScene.Shop:
                break;
            case EScene.Comic:
                break;
            case EScene.Intro:
                IntroController.instance.Hide(true);
                break;
            case EScene.MAX:
                break;
        }
    }

    public void ShowScene()
    {
        HUDManager.instance.ToggleMoney(false);
        gameObject.SetActive(true);
        switch (SceneType)
        {
            case EScene.Elevator:
                
                Elevator.instance.GetElevatorButtonBySceneType(EScene.Desktop).ToggleEnable(!SaveManager.instance.GetCurrentPlayerState().IsFaxSent());
                Elevator.instance.GetElevatorButtonBySceneType(EScene.Shop).ToggleEnable(true);
                Elevator.instance.GetElevatorButtonBySceneType(EScene.Office).ToggleEnable(!SaveManager.instance.GetCurrentPlayerState().IsDayDone() && SaveManager.instance.GetCurrentPlayerState().IsFaxSent());
                //Elevator.instance.GetElevatorButtonBySceneType(EScene.Elevator).ToggleEnable(true);
                break;
            case EScene.Desktop:
                break;
            case EScene.Office:
                if (!SaveManager.instance.GetCurrentPlayerState().IsIntroDone())
                {
                    DialogueManager.instance.StartIntro();
                    Elevator.instance.GetElevatorButtonBySceneType(EScene.Desktop).ToggleEnable(true);
                    Elevator.instance.GetElevatorButtonBySceneType(EScene.Shop).ToggleEnable(false);
                    Elevator.instance.GetElevatorButtonBySceneType(EScene.Office).ToggleEnable(false);
                }
                else
                {
                    if (GameManager.instance.GameMode == EGameMode.DEMO)
                    {
                        DialogueManager.instance.StartDayEndDialogue();
                    }
                    else
                    {
                        DialogueManager.instance.StartDayEndDialogue(false);
                    }
                }
                break;
            case EScene.DressingRoom:
                Mirror.instance.ToggleMirror(true);
                break;
            case EScene.Shop:
                HUDManager.instance.ToggleMoney(true);
                ShopManager.instance.StartShopKeeperHello();
                break;
            case EScene.Comic:
                if(SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameFinished())
                {
                    SceneManager.LoadScene("EndComic");
                }
                else
                {
                    if(SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameFinishedOnceProperly())
                    {
                        SceneManager.LoadScene("AltIntroComic");
                        ArticyGlobalVariables.Default.game.finished = true;
                    }
                    else
                    {
                        SceneManager.LoadScene("IntroComic");
                    }
                }
                break;
            case EScene.Intro:
                SceneManager.LoadScene("Intro");
                break;
            case EScene.PostGame:

                SaveManager.instance.GetCurrentCarryoverPlayerState().SetNewGamePlusEnabled(true);
                
                SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameFinishedOnce(true);
                SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameStarted(false);
                SceneManager.LoadScene("PostGame");
                break;
            case EScene.MAX:
                break;
        }
    }

    public void InitScene()
    {
        DontDestroyOnLoad(gameObject);
        World.instance.AddDDOLObject(gameObject);

        HideScene();
        Debug.Log("Scene " + gameObject.name);
        for (int i = 0; i < CanvasList.Count; ++i)
        {
            CanvasList[i].worldCamera = Camera.main;
        }
        switch (SceneType)
        {
            case EScene.Elevator:
                break;
            case EScene.Desktop:
                break;
            case EScene.Office:
                break;
            case EScene.DressingRoom:
                break;
            case EScene.Shop:
                break;
            case EScene.Comic:
                break;
            case EScene.Intro:
                gameObject.transform.SetParent(HUDManager.instance.gameObject.transform, false);
                ShowScene();
                break;
            case EScene.PostGame:
                break;
            case EScene.MAX:
                break;
        }
    }
}
