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

using Articy.Project_Of_Death;
using Articy.Project_Of_Death.GlobalVariables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public enum ECustomName
{
    DeathAndTaxes,
    SugarAndSpice,
    RainbowsAndPuppies,
    BloodAndGuts,
    BeepsAndBoops,
    MAX,
}

public enum EPostVideoAction
{
    NewGame,
    NewGamePlus,
    Continue,
    Quit,
}

public class IntroController : MonoBehaviour
{
    public static IntroController instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    VideoPlayer MainMenuPlayer;

    [SerializeField]
    RawImage RawImageBackground;

    [SerializeField]
    Image ImageBackground;

    [SerializeField]
    Image ImageNameOverlay;

    [SerializeField]
    Sprite[] SpriteListNameOverlay = new Sprite[(int)ECustomName.MAX];

    [SerializeField]
    Vector3 UltraWideScaleForBackground = new Vector3(1.4f, 1.4f, 1.0f);
    [SerializeField]
    Vector3 UltraNotWideScaleForBackground = new Vector3(1.5f, 1.5f, 1.0f);

    //[SerializeField]
    //VideoClip IntroClip;
    //[SerializeField]
    //VideoClip LoopClip;
    //[SerializeField]
    //VideoClip OutroClip;

    public const string IntroClipName = "MenuIntro.mp4";
    public const string LoopClipName = "MenuLoop.mp4";
    public const string OutroClipName = "MenuOutro.mp4";

    public Button ButtonNewGame;
    public Button ButtonNewGamePlus;
    public Button ButtonContinue;
    public Button ButtonOptions;
    public Button ButtonExit;
    public Button ButtonQuit;
    public Button ButtonMainMenu;
    public Button ButtonGallery;
    public Button ButtonWishlist;

    public TextMeshProUGUI TextButtonContinue;

    public GameObject DebugPanel;
    public TMP_InputField InputDebugDay;

    public TextMeshProUGUI TextVersion;

    bool bPressedOptions = false;

    float TimeBeforeStart = 0.0f;

    EPostVideoAction PostVideoAction;


    void Start()
    {

        ButtonExit.onClick.AddListener(OnExitClicked);
        ButtonNewGame.onClick.AddListener(OnNewGameClicked);
        ButtonNewGamePlus.onClick.AddListener(OnNewGamePlusClicked);

        ButtonContinue.onClick.AddListener(OnContinueClicked);
        ButtonOptions.onClick.AddListener(OnOptionsClicked);
        ButtonQuit.onClick.AddListener(OnQuitClicked);
        ButtonMainMenu.onClick.AddListener(OnMainMenuClicked);
        ButtonGallery.onClick.AddListener(OnGalleryClicked);
        ButtonMainMenu.gameObject.SetActive(false);
        ButtonWishlist.onClick.AddListener(OnWishlistClicked);
        gameObject.SetActive(false);

    }

    public void HideNewGamePlusButton()
    {
        ButtonNewGamePlus.gameObject.SetActive(false);
    }

    public void HideContinueButton()
    {
        ButtonContinue.gameObject.SetActive(false);
    }

    public void InitIntroController()
    {
        gameObject.SetActive(true);

        if (!GameManager.instance.bDebugMode)
        {
            if(DebugPanel != null)
            {
                DebugPanel.SetActive(false);
            }
            TextVersion.text = "Version: " + Application.version;
        }
        else
        {
            TextVersion.text = "Version: " + Application.version + " DEVTOOLS ON";
        }

        if (GameManager.instance.GameMode == EGameMode.DEMO)
        {
            TextVersion.text = TextVersion.text + " DEMO";
        }

        HideExitButton();

        if (!SaveManager.instance.GetCurrentCarryoverPlayerState().IsNewGamePlusEnabled())
        {
            HideNewGamePlusButton();
        }

        if (GameManager.instance.GameMode != EGameMode.DEMO)
        {
            //ButtonWishlist.gameObject.SetActive(false);
        }
        else
        {
            ButtonGallery.gameObject.SetActive(false);
        }


        if (!SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameStarted())
        {
            HideContinueButton();
        }
        else
        {
            TextButtonContinue.text = "Continue\nDay " + SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt();
        }


        //gameObject.SetActive(false);
#if !(UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX)
        //MainMenuPlayer.clip = IntroClip;
        MainMenuPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, IntroClipName);

        MainMenuPlayer.isLooping = false;
        MainMenuPlayer.loopPointReached += OnVideoEnd;
        MainMenuPlayer.Play();

#else
        ImageBackground.gameObject.SetActive(true);

#endif
    }

    public void OnWishlistClicked()
    {
        //Application.OpenURL("https://store.steampowered.com/app/1166290/Death_and_Taxes/fromtorrent");
        Application.OpenURL("https://discordapp.com/invite/GNHBWSV");
    }

    public void HandleStreamerMode()
    {
        if (SaveManager.instance.CurrentOptions.StreamerMode)
        {
            //TwitchClient.instance.ConnectToStream();
        }
    }

    private void StartNewGame()
    {
        Debug.Log("New Game started, clear save");
        SaveManager.instance.ClearSave();

        SaveManager.instance.GetCurrentPlayerState().SetSpawnCounter(Random.Range(10, 100));
        SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameStartedOnce(true);
        SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameStarted(true);
        SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameFinished(false);

        template_item starterBody = ShopManager.instance.GetStarterBody();
        template_item starterHead = ShopManager.instance.GetStarterHead();
        if (starterBody != null)
        {
            //SaveManager.instance.GetCurrentCarryoverPlayerState().AddOwnedItemID(starterBody.Id);
            starterBody.Template.item_data.item_instruction_onbuy.CallScript();
            List<template_item_variation> variations = ShopManager.instance.GetItemVariations(starterBody);
            for (int i = 0; i < variations.Count; ++i)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().AddOwnedBodyID(variations[i].Id);
            }
        }
        if (starterHead != null)
        {
            //SaveManager.instance.GetCurrentCarryoverPlayerState().AddOwnedItemID(starterHead.Id);
            starterHead.Template.item_data.item_instruction_onbuy.CallScript();
            List<template_item_variation> variations = ShopManager.instance.GetItemVariations(starterHead);
            for (int i = 0; i < variations.Count; ++i)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().AddOwnedHeadID(variations[i].Id);
            }
        }

        if (InputDebugDay != null && InputDebugDay.text != "")
        {
            SceneManager.LoadScene("Game");
            int day = int.Parse(InputDebugDay.text);
            for (int i = 0; i < day - 1; ++i)
            {
                SaveManager.instance.GetCurrentPlayerState().IncrementDayIndex();
            }
            DesktopManager.instance.StartDay();
            SaveManager.instance.GetCurrentPlayerState().SetIntroDone(true);

            ElevatorManager.instance.SwitchScene(EScene.Elevator);

        }
        else
        {
            ElevatorManager.instance.SwitchScene(EScene.Comic);
        }
    }
#if !(UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX)

    public void OnVideoEnd(VideoPlayer vp)
    {
        if (vp.url.Contains(IntroClipName))
        {
            //MainMenuPlayer.clip = LoopClip;
            MainMenuPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, LoopClipName);
            MainMenuPlayer.isLooping = true;
            MainMenuPlayer.Play();
        }
        else if (vp.url.Contains(LoopClipName))
        {

        }
        else if (vp.url.Contains(OutroClipName))
        {
            switch (PostVideoAction)
            {
                case EPostVideoAction.NewGame:
                    StartNewGame();
                    break;
                case EPostVideoAction.NewGamePlus:
                    StartNewGamePlus();
                    break;
                case EPostVideoAction.Continue:
                    ContinueGame();
                    break;
                case EPostVideoAction.Quit:
                    Application.Quit();
                    break;
            }

            //vp.Stop();
        }
    }
#endif

    void Update()
    {
        TimeBeforeStart += Time.deltaTime;

        if (GameManager.instance != null)
        {

#if !(UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX)
            if (GameManager.instance.CurrentAspectRatio == EAspectRatio.a21_9)
            {
                RawImageBackground.transform.localScale = UltraWideScaleForBackground;
            }
            else if (GameManager.instance.CurrentAspectRatio == EAspectRatio.a16_9)
            {
                RawImageBackground.transform.localScale = Vector3.one;
            }
            else
            {
                RawImageBackground.transform.localScale = UltraNotWideScaleForBackground;
            }
#else
            if (GameManager.instance.CurrentAspectRatio == EAspectRatio.a21_9)
            {
                ImageBackground.transform.localScale = UltraWideScaleForBackground * 1.2f;
            }
            else if(GameManager.instance.CurrentAspectRatio == EAspectRatio.a16_9) 
            {
                ImageBackground.transform.localScale = Vector3.one * 1.2f;
            }
            else
            {
                ImageBackground.transform.localScale = UltraNotWideScaleForBackground;
            }
#endif
        }
        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetButton("Fire1"))
        //{
        //    SceneManager.LoadScene("MainComic");
        //}

        /*f (this.countdown >= 0f)
        {
            this.countdown -= Time.deltaTime;
            if (this.countdown < 0f)
            {
                SceneManager.LoadScene("MainComic");
            }
        }*/
    }

    public void ShowExitButton()
    {
        ButtonExit.gameObject.SetActive(true);
    }

    public void HideExitButton()
    {
        ButtonExit.gameObject.SetActive(false);
    }



    public void OnExitClicked()
    {
        InputManager.instance.FrameLock();

        Hide(true);

        AudioManager.instance.NotifyGameUnPaused();
    }

    public void OnMainMenuClicked()
    {
        GameManager.instance.RestartGame(false);
    }

    public void OnGalleryClicked()
    {
        GalleryScreen.instance.Show();
        Hide(false);
    }

    public void OnQuitClicked()
    {
        Application.Quit();

        //#if !(UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX)

        //        if (!MainMenuPlayer.url.Contains(OutroClipName))
        //        {
        //            PostVideoAction = EPostVideoAction.Quit;

        //            MainMenuPlayer.Stop();
        //            MainMenuPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, OutroClipName);
        //            //MainMenuPlayer.clip = OutroClip;
        //            MainMenuPlayer.isLooping = false;
        //            MainMenuPlayer.Play();
        //        }
        //#else
        //        Application.Quit();

        //#endif

    }

    public void StartNewGamePlusSequence()
    {
#if !(UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX)

        if (!MainMenuPlayer.url.Contains(OutroClipName))
        {
            PostVideoAction = EPostVideoAction.NewGamePlus;

            MainMenuPlayer.Stop();
            MainMenuPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, OutroClipName);
            //MainMenuPlayer.clip = OutroClip;
            MainMenuPlayer.isLooping = false;
            MainMenuPlayer.Play();

        }
#else
        StartNewGamePlus();
#endif

    }

    public void OnNewGamePlusClicked()
    {

        if (SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameStarted())
        {
            RestartConfirm.instance.Show(true);
        }
        else
        {
            StartNewGamePlusSequence();
        }

    }


    public void StartNewGameSequence()
    {
#if !(UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX)

        if (!MainMenuPlayer.url.Contains(OutroClipName))
        {
            PostVideoAction = EPostVideoAction.NewGame;

            MainMenuPlayer.Stop();
            MainMenuPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, OutroClipName);
            //MainMenuPlayer.clip = OutroClip;
            MainMenuPlayer.isLooping = false;
            MainMenuPlayer.Play();
#if !DISABLESTEAMWORKS

#endif
        }
#else
        StartNewGame();
#endif
    }

    public void OnNewGameClicked()
    {
        if (SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameStarted() || SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameFinishedOnce())
        {
            RestartConfirm.instance.Show(false);
        }
        else
        {
            StartNewGameSequence();
        }

    }

    public void OnOptionsClicked()
    {
        OptionsManager.instance.Show();
        Hide(false);

        bPressedOptions = true;
    }

    public void OnContinueClicked()
    {
#if !(UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX)

        if (!MainMenuPlayer.url.Contains(OutroClipName))
        {
            PostVideoAction = EPostVideoAction.Continue;

            MainMenuPlayer.Stop();
            MainMenuPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, OutroClipName);
            MainMenuPlayer.isLooping = false;
            MainMenuPlayer.Play();

        }

#else
        ContinueGame();

#endif

    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Game");

        SaveManager.instance.ApplyLoadState();

        AudioManager.instance.NotifyGameUnPaused();
    }

    public void StartNewGamePlus()
    {
        Debug.Log("New Game Plus " + SaveManager.instance.GetCurrentCarryoverPlayerState().NewGamePlusCount + " started, carry over saved data");
        SaveManager.instance.HandleStartNewGamePlus();

        //if (InputDebugDay.text != "")
        //{
        //    int day = int.Parse(InputDebugDay.text);
        //    for (int i = 0; i < day - 1; ++i)
        //    {
        //        SaveManager.instance.GetCurrentPlayerState().IncrementDayIndex();
        //    }
        //    DesktopManager.instance.StartDay();
        //    SaveManager.instance.GetCurrentPlayerState().SetIntroDone(true);

        //    ElevatorManager.instance.SwitchScene(EScene.Elevator);

        //}
        //else
        //{
        ElevatorManager.instance.SwitchScene(EScene.Comic);
        //}
    }

    public void Toggle()
    {
        if (SceneManager.GetActiveScene().name != "Intro")
        {
            if (gameObject.activeSelf)
            {
                Hide(true);
                AudioManager.instance.NotifyGameUnPaused();
            }
            else
            {
                Show();
            }
            ButtonContinue.gameObject.SetActive(false);
            ButtonMainMenu.gameObject.SetActive(true);
        }
        else
        {
            ButtonMainMenu.gameObject.SetActive(false);
            ButtonContinue.gameObject.SetActive(true);
        }
    }


    public void CheckCustomNameBooleans()
    {
        if (ArticyGlobalVariables.Default.game.name_beeps_and_boops)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().CustomName = ECustomName.BeepsAndBoops;
        }
        else if (ArticyGlobalVariables.Default.game.name_blood_and_guts)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().CustomName = ECustomName.BloodAndGuts;
        }
        else if (ArticyGlobalVariables.Default.game.name_rainbow_and_puppies)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().CustomName = ECustomName.RainbowsAndPuppies;
        }
        else if (ArticyGlobalVariables.Default.game.name_sugar_and_space)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().CustomName = ECustomName.SugarAndSpice;
        }
        else
        {
            //nothing and Death and Taxes
            SaveManager.instance.GetCurrentCarryoverPlayerState().CustomName = ECustomName.DeathAndTaxes;
        }
    }

    public void Show(bool cameFromOptions = false)
    {
        if (SceneManager.GetActiveScene().name == "EndComic")
        {
            Time.timeScale = 0.0f;
        }
        gameObject.SetActive(true);

        if (SceneManager.GetActiveScene().name != "Intro" && SceneManager.GetActiveScene().name != "World")
        {
            ShowExitButton();
            ButtonNewGame.gameObject.SetActive(false);
            HideNewGamePlusButton();
            if (SceneManager.GetActiveScene().name.Contains("Comic"))
            {
                //HUDManager.instance.ToggleHUDManager(true);
            }
        }
        else
        {
            HideExitButton();
            ButtonNewGame.gameObject.SetActive(true);
        }

#if !(UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX)

        MainMenuPlayer.Stop();
        MainMenuPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, LoopClipName);
        //MainMenuPlayer.clip = LoopClip;
        MainMenuPlayer.isLooping = true;
        MainMenuPlayer.Play();

#endif

        if (!cameFromOptions)
        {
            AudioManager.instance.NotifyGamePaused();
        }



        ImageNameOverlay.sprite = SpriteListNameOverlay[(int)SaveManager.instance.GetCurrentCarryoverPlayerState().CustomName];

        if (SaveManager.instance.GetCurrentCarryoverPlayerState().CustomName == ECustomName.DeathAndTaxes)
        {
            ImageNameOverlay.color = new Color(0, 0, 0, 0);
        }
        else
        {
            ImageNameOverlay.color = Color.white;
        }
    }

    public void Hide(bool restoreTimeScale)
    {
        if(restoreTimeScale)
        {
            Time.timeScale = 1.0f;
        }


        gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name != "Intro")
        {
            if (SceneManager.GetActiveScene().name.Contains("Comic"))
            {
                //HUDManager.instance.ToggleHUD(false);
            }
        }
        else
        {


        }
    }
}
