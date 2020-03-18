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
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class ResolutionSerializable
{
    //
    // Summary:
    //     Resolution width in pixels.
    public int width { get; set; }
    //
    // Summary:
    //     Resolution height in pixels.
    public int height { get; set; }
    //
    // Summary:
    //     Resolution's vertical refresh rate in Hz.
    public int refreshRate { get; set; }

    public ResolutionSerializable()
    {

    }

    public ResolutionSerializable(Resolution res)
    {
        width = res.width;
        height = res.height;
        refreshRate = res.refreshRate;
    }

    public Resolution ToResolution()
    {
        Resolution res = new Resolution();
        res.width = width;
        res.height = height;
        res.refreshRate = refreshRate;
        return res;
    }
}

[System.Serializable]
public class Options
{
    public Options()
    {

    }

    [SerializeField]
    public ResolutionSerializable CurrentResolution = new ResolutionSerializable();
    [SerializeField]
    public float VolumeMaster = 1.0f;
    [SerializeField]
    public float VolumeGeneral = 1.0f;
    [SerializeField]
    public float VolumeVoice = 1.0f;
    [SerializeField]
    public float VolumeMusic = 1.0f;
    [SerializeField]
    public FullScreenMode ScreenMode = FullScreenMode.FullScreenWindow;
    [SerializeField]
    public bool SkipMarkPopUp = false;
    [SerializeField]
    public bool StreamerMode = false;
    [SerializeField]
    public string StreamChannel = "";
    [SerializeField]
    public string StreamCommandLive = "!spare";
    [SerializeField]
    public string StreamCommandDie = "!die";
    [SerializeField]
    public int StreamVoteTimer = 30;

    public float GetVolumeMusicLowered()
    {
        if (SpeechBubbleManager.instance.IsBubbleSpeechActive() || DialogueManager.instance.IsDialogueActive())
        {
            //Debug.Log("dialogue noise");
            return VolumeMusic * 0.7f * 0.4f;
        }
        else
        {
            //Debug.Log("regular noise");

            return VolumeMusic * 0.7f;

        }
    }

    public void SetVolumeMusic(float val)
    {
        VolumeMusic = val;
    }
}

public class OptionsManager : ManagerBase
{
    public static OptionsManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    TMP_Dropdown DropdownResolution;
    [SerializeField]
    TMP_Dropdown DropdownFullScreenMode;
    [SerializeField]
    Slider SliderVolumeMaster;
    [SerializeField]
    Slider SliderVolumeMusic;
    [SerializeField]
    Slider SliderVolumeVoice;
    [SerializeField]
    Slider SliderVolumeGeneral;
    [SerializeField]
    Toggle ToggleSkipMarkConfirmation;
    [SerializeField]
    Toggle ToggleStreamerMode;
    [SerializeField]
    TMP_InputField InputStreamChannelName;
    [SerializeField]
    TMP_InputField InputStreamCommandLive;
    [SerializeField]
    TMP_InputField InputStreamCommandDie;
    [SerializeField]
    TMP_InputField InputStreamVoteTimer;

    [SerializeField]
    Image ImageBackground;

    [SerializeField]
    VideoPlayer MainMenuPlayer;
    //[SerializeField]
    //VideoClip LoopClip;

    [SerializeField]
    Button ButtonBack;

    [SerializeField]
    Button ButtonStreamerSettingsOpen;

    [SerializeField]
    Button ButtonStreamerSettingsClose;

    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitOptionsRoutine());
    }

    IEnumerator InitOptionsRoutine()
    {
        //wait for init
        while(SaveManager.instance == null)
        {
            yield return null;
        }

        SaveManager.instance.LoadOptions();

        SliderVolumeMaster.value = SaveManager.instance.CurrentOptions.VolumeMaster;
        SliderVolumeGeneral.value = SaveManager.instance.CurrentOptions.VolumeGeneral;
        SliderVolumeVoice.value = SaveManager.instance.CurrentOptions.VolumeVoice;
        SliderVolumeMusic.value = SaveManager.instance.CurrentOptions.VolumeMusic;

        ToggleSkipMarkConfirmation.isOn = SaveManager.instance.CurrentOptions.SkipMarkPopUp;
        ToggleStreamerMode.isOn = SaveManager.instance.CurrentOptions.StreamerMode;

        SliderVolumeMaster.onValueChanged.AddListener(OnSliderChanged);
        SliderVolumeGeneral.onValueChanged.AddListener(OnSliderChanged);
        SliderVolumeVoice.onValueChanged.AddListener(OnSliderChanged);
        SliderVolumeMusic.onValueChanged.AddListener(OnSliderChanged);
        ToggleSkipMarkConfirmation.onValueChanged.AddListener(OnSkipMarkConfirmationChanged);
        ToggleStreamerMode.onValueChanged.AddListener(OnStreamerModeChanged);
        InputStreamChannelName.onValueChanged.AddListener(OnStreamNameChanged);
        InputStreamCommandDie.onValueChanged.AddListener(OnStreamCommandDieChanged);
        InputStreamCommandLive.onValueChanged.AddListener(OnStreamCommandLiveChanged);
        InputStreamVoteTimer.onValueChanged.AddListener(OnStreamVoteTimerChanged);
        ButtonStreamerSettingsOpen.onClick.AddListener(OnStreamerSettingsOpenClicked);
        ButtonStreamerSettingsClose.onClick.AddListener(OnStreamerSettingsCloseClicked);

        PopulateOptions();

        Hide();

        IntroController.instance.HandleStreamerMode();

        ButtonStreamerSettingsClose.gameObject.SetActive(false);
    }

    public override void InitManager()
    {
        base.InitManager();


    }

    public void OnStreamerSettingsOpenClicked()
    {
        AudioManager.instance.PlayButtonClickMenu();
        ButtonStreamerSettingsClose.gameObject.SetActive(true);
    }

    public void OnStreamerSettingsCloseClicked()
    {
        AudioManager.instance.PlayButtonClickMenu();
        ButtonStreamerSettingsClose.gameObject.SetActive(false);
    }

    public void OnStreamCommandDieChanged(string val)
    {
        SaveManager.instance.CurrentOptions.StreamCommandDie = val.ToLower();
        SaveManager.instance.SaveOptions(SaveManager.instance.CurrentOptions);
    }

    public void OnStreamCommandLiveChanged(string val)
    {
        SaveManager.instance.CurrentOptions.StreamCommandLive = val.ToLower();
        SaveManager.instance.SaveOptions(SaveManager.instance.CurrentOptions);
    }

    public void OnStreamVoteTimerChanged(string val)
    {
        SaveManager.instance.CurrentOptions.StreamVoteTimer = Mathf.Abs(int.Parse(val));
        SaveManager.instance.SaveOptions(SaveManager.instance.CurrentOptions);
    }

    public void OnStreamNameChanged(string val)
    {
        SaveManager.instance.CurrentOptions.StreamChannel = val.ToLower();
        SaveManager.instance.SaveOptions(SaveManager.instance.CurrentOptions);
    }

    public void OnStreamerModeChanged(bool val)
    {
        SaveManager.instance.CurrentOptions.StreamerMode = val;
        SaveManager.instance.SaveOptions(SaveManager.instance.CurrentOptions);
        AudioManager.instance.PlayTestClip(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster);

        if (VoteCounter.instance != null)
        {
            VoteCounter.instance.gameObject.SetActive(val);
        }
    }

    public void OnSkipMarkConfirmationChanged(bool val)
    {
        SaveManager.instance.CurrentOptions.SkipMarkPopUp = val;
        SaveManager.instance.SaveOptions(SaveManager.instance.CurrentOptions);
        AudioManager.instance.PlayTestClip(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster);
    }

    public void OnSliderChanged(float val)
    {
        Options currentOptions = SaveManager.instance.CurrentOptions;
        currentOptions.VolumeMaster = SliderVolumeMaster.value;
        currentOptions.VolumeGeneral = SliderVolumeGeneral.value;
        currentOptions.VolumeVoice = SliderVolumeVoice.value;
        currentOptions.SetVolumeMusic(SliderVolumeMusic.value);
        SaveManager.instance.SaveOptions(currentOptions);
    }

    [SerializeField]
    RawImage RawImageBackground;
    [SerializeField]
    Vector3 UltraWideScaleForBackground = new Vector3(1.4f, 1.4f, 1.0f);
    [SerializeField]
    Vector3 UltraNotWideScaleForBackground = new Vector3(1.5f, 1.5f, 1.0f);

    // Update is called once per frame
    void Update()
    {
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
    }

    public void Show()
    {
        if (SceneManager.GetActiveScene().name == "EndComic")
        {
            Time.timeScale = 0.0f;
        }
        gameObject.SetActive(true);

#if !(UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX)
        MainMenuPlayer.Stop();
        MainMenuPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, IntroController.LoopClipName);
        //MainMenuPlayer.clip = LoopClip;
        MainMenuPlayer.isLooping = true;
        MainMenuPlayer.Play();

#else
        ImageBackground.gameObject.SetActive(true);
#endif

        ToggleSkipMarkConfirmation.isOn = SaveManager.instance.CurrentOptions.SkipMarkPopUp;
        ToggleStreamerMode.isOn = SaveManager.instance.CurrentOptions.StreamerMode;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void PopulateOptions()
    {
        Options currentOptions = SaveManager.instance.CurrentOptions;

        List<Resolution> resolutions = new List<Resolution>(Screen.resolutions);
        DropdownResolution.onValueChanged.AddListener(delegate { SetResolution(resolutions[DropdownResolution.value]); });
        for (int i = 0; i < resolutions.Count; ++i)
        {
            DropdownResolution.options.Add(new TMP_Dropdown.OptionData(ResToString(resolutions[i])));

            if (resolutions[i].width == currentOptions.CurrentResolution.width &&
                resolutions[i].height == currentOptions.CurrentResolution.height)
            {
                DropdownResolution.value = i;
            }
            //DropdownResolution.options[i].text = ResToString(resolutions[i]);
        }

        DropdownFullScreenMode.onValueChanged.AddListener(delegate { SetFullScreenMode((FullScreenMode)DropdownFullScreenMode.value); });
        for (int i = 0; i < 4; ++i)
        {
            DropdownFullScreenMode.options.Add(new TMP_Dropdown.OptionData(FullScreenModeToString((FullScreenMode)i)));

            if ((int)currentOptions.ScreenMode == i)
            {
                DropdownFullScreenMode.value = i;
            }
            //DropdownResolution.options[i].text = ResToString(resolutions[i]);
        }
        DropdownResolution.RefreshShownValue();
        DropdownFullScreenMode.RefreshShownValue();

        SliderVolumeMaster.value = currentOptions.VolumeMaster;
        SliderVolumeGeneral.value = currentOptions.VolumeGeneral;
        SliderVolumeVoice.value = currentOptions.VolumeVoice;
        SliderVolumeMusic.value = currentOptions.VolumeMusic;
        InputStreamChannelName.text = currentOptions.StreamChannel;
        InputStreamCommandDie.text = currentOptions.StreamCommandDie;
        InputStreamCommandLive.text = currentOptions.StreamCommandLive;
        InputStreamVoteTimer.text = currentOptions.StreamVoteTimer.ToString();

        ButtonBack.onClick.AddListener(OnBackClicked);
    }

    public void OnBackClicked()
    {
        Options currentOptions = SaveManager.instance.CurrentOptions;
        currentOptions.VolumeMaster = SliderVolumeMaster.value;
        currentOptions.VolumeGeneral = SliderVolumeGeneral.value;
        currentOptions.VolumeVoice = SliderVolumeVoice.value;
        currentOptions.SetVolumeMusic(SliderVolumeMusic.value);
        SaveManager.instance.SaveOptions(currentOptions);

        Hide();
        IntroController.instance.Show(true);
        IntroController.instance.HandleStreamerMode();
    }

    public void SetResolution(Resolution resolution)
    {
        if (Screen.currentResolution.width != resolution.width || Screen.currentResolution.height != resolution.height)
        {
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            Options currentOptions = SaveManager.instance.CurrentOptions;
            currentOptions.CurrentResolution = new ResolutionSerializable(resolution);
            SaveManager.instance.SaveOptions(currentOptions);

            AudioManager.instance.PlayButtonClick();
        }
    }

    public void SetStreamerMode(bool val)
    {
        Options currentOptions = SaveManager.instance.CurrentOptions;
        currentOptions.StreamerMode = val;
        SaveManager.instance.SaveOptions(currentOptions);
    }

    public void SetSkipMarkPopUp(bool val)
    {
        Options currentOptions = SaveManager.instance.CurrentOptions;
        currentOptions.SkipMarkPopUp = val;
        SaveManager.instance.SaveOptions(currentOptions);
    }

    public void SetFullScreenMode(FullScreenMode mode)
    {
        if (Screen.fullScreenMode != mode)
        {
            Screen.SetResolution(SaveManager.instance.CurrentOptions.CurrentResolution.width, SaveManager.instance.CurrentOptions.CurrentResolution.height, mode);

            switch (mode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                    Screen.fullScreen = true;
                    break;
                case FullScreenMode.FullScreenWindow:
                    Screen.fullScreen = true;
                    break;
                case FullScreenMode.MaximizedWindow:
                    Screen.fullScreen = false;
                    break;
                case FullScreenMode.Windowed:
                    Screen.fullScreen = false;
                    break;
            }

            Options currentOptions = SaveManager.instance.CurrentOptions;
            currentOptions.ScreenMode = mode;
            SaveManager.instance.SaveOptions(currentOptions);

            AudioManager.instance.PlayButtonClick();
        }
    }

    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }

    string FullScreenModeToString(FullScreenMode mode)
    {
        switch (mode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                return "Exclusive Fullscreen";
            case FullScreenMode.FullScreenWindow:
                return "Fullscreen Window";
            case FullScreenMode.MaximizedWindow:
                return "Maximized Window";
            case FullScreenMode.Windowed:
                return "Window";
        }
        return "";
    }
}
