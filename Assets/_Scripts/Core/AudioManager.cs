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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public enum EDesktopMusic
{
    Normal,
    Swing,
    Chill,
    None
}

public class AudioManager : ManagerBase
{
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioSource SourceEffects;
    public AudioSource SourceEffectsLeftDrawer;
    public AudioSource SourceEffectsRightDrawer;
    public AudioSource SourceMusic;
    public AudioSource SourceMusicSecondary;
    public AudioSource SourceVoice;
    public AudioSource SourceAmbientLoop;
    public AudioSource SourceTest;
    public AudioSource SourceFidgetLoop;
    public AudioSource SourceChaosGlobeLoop;
    public AudioSource SourceOfficeAmbience;

    public AudioClip ClipButtonClick;
    public List<AudioClip> ClipButtonClicksMenu = new List<AudioClip>();
    public AudioClip ClipTest;
    [SerializeField]
    public AudioClip MarkDeath;
    [SerializeField]
    public AudioClip MarkSpare;
    [SerializeField]
    public AudioClip MarkErase;

    [SerializeField]
    public AudioClip MenuTheme;
    [SerializeField]
    public AudioClip ElevatorTheme;
    [SerializeField]
    public AudioClip ShopTheme;
    [SerializeField]
    public AudioClip BedroomTheme;
    [SerializeField]
    public AudioClip FateTheme;
    [SerializeField]
    float FateThemeTimestampFirst;
    [SerializeField]
    float FateThemeTimestampSecond;
    [SerializeField]
    float FateThemeTimestampThird;
    [SerializeField]
    public AudioClip DeskThemeNormal;
    [SerializeField]
    public AudioClip DeskThemeSwing;
    [SerializeField]
    public AudioClip DeskThemeChill;
    [SerializeField]
    public AudioClip IntroComicTheme;
    [SerializeField]
    public AudioClip IntroComicThemeDark;
    [SerializeField]
    public AudioClip ClipElevatorDing;
    [SerializeField]
    public AudioClip ClipMortimerCoin;

    [SerializeField]
    float ElevatorThemeTimestampFirst;
    [SerializeField]
    float ElevatorThemeTimestampSecond;

    [SerializeField]
    float MortimerThemeTimestampFirst;
    [SerializeField]
    float MortimerThemeTimestampSecond;
    [SerializeField]
    float MortimerThemeTimestampThird;

    [SerializeField]
    float DesktopThemeTimestampFirst;
    [SerializeField]
    float DesktopThemeTimestampSecond;
    [SerializeField]
    float DesktopThemeTimestampThird;

    [SerializeField]
    float DesktopThemeSwingTimestampFirst;
    [SerializeField]
    float DesktopThemeSwingTimestampSecond;
    [SerializeField]
    float DesktopThemeSwingTimestampThird;

    [SerializeField]
    float DesktopThemeChillTimestampFirst;
    [SerializeField]
    float DesktopThemeChillTimestampSecond;
    [SerializeField]
    float DesktopThemeChillTimestampThird;



    [SerializeField]
    List<AudioClip> ClipsUp = new List<AudioClip>();
    [SerializeField]
    List<AudioClip> ClipsDown = new List<AudioClip>();

    AudioClip PausedClip;
    AudioClip PausedClipCrossfade;
    float PausedClipTime;
    float PausedClipCrossfadeTime;

    bool bIsFadingIn = false;
    bool bIsFadingOut = false;

    bool bIsCrossfading = false;

    bool bPlayedFateOnce = false;
    bool bPlayedDesktopOnce = false;
    bool bPlayedElevatorOnce = false;
    bool bPlayedMortimerOnce = false;

    float TargetMusicVolume = 0.0f;
    float TargetMusicVolumeAlpha = 0.0f;

    public override void InitManager()
    {
        base.InitManager();
        UpdateVolumes();
    }

    // Start is called before the first frame update
    void Start()
    {
        SourceChaosGlobeLoop.volume = 0.0f;
        StartCoroutine(SwitchMusicRoutine(MenuTheme, MenuTheme));
    }

    // Update is called once per frame
    void Update()
    {
        TargetMusicVolumeAlpha = Mathf.Clamp(TargetMusicVolumeAlpha + Time.deltaTime, 0.0f, 1.0f);
        SourceMusic.volume = Mathf.Lerp(SourceMusic.volume, TargetMusicVolume, TargetMusicVolumeAlpha);
        if (ElevatorManager.instance != null)
        {
            if (ElevatorManager.instance.GetCurrentScene() == EScene.Desktop)
            {
                SourceOfficeAmbience.mute = false;
            }
            else
            {
                SourceOfficeAmbience.mute = true;
            }
        }
    }

    Coroutine SaySpawnRoutine = null;

    public void SaySpawnCounter(float previousClipTime)
    {
        SaySpawnRoutine = StartCoroutine(SaySpawnCounterRoutine(previousClipTime));
    }

    IEnumerator SaySpawnCounterRoutine(float previousClipTime)
    {
        yield return new WaitForSeconds(previousClipTime + 0.27f);
        int spawnCounter = SaveManager.instance.GetCurrentPlayerState().GetSpawnCounter();

        string haha = spawnCounter.ToString();
        int firstNumber = (int)char.GetNumericValue(haha[0]);
        int secondNumber = (int)char.GetNumericValue(haha[1]);

        PlayVoiceClip(ClipsUp[firstNumber]);
        yield return new WaitForSeconds(ClipsUp[firstNumber].length + Random.Range(-0.05f, 0.05f) + 0.37f);
        PlayVoiceClip(ClipsDown[secondNumber]);
        //yield return new WaitForSeconds(ClipsUp[secondNumber].length + Random.Range(-0.05f, 0.05f));
        SaySpawnRoutine = null;
    }

    public AudioClip PlayVoiceClip(AudioClip clip)
    {
        SourceVoice.Stop();
        SourceVoice.pitch = 1.0f;
        if (clip != null)
        {
            SourceVoice.clip = clip;
            SourceVoice.Play();
        }
        return clip;
    }

    public AudioClip PlayVoiceClip(string clipName, Entity speaker)
    {
        if (SaySpawnRoutine != null)
        {
            StopCoroutine(SaySpawnRoutine);
            SaySpawnRoutine = null;
        }
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)
        string path = ""; // isFate ? "Audio/Fate/" + clipName : "Audio/" + clipName;

        if (speaker == DialogueManager.instance.FateEntity || speaker == DialogueManager.instance.FatePhoneEntity)
        {
            path = "Audio/Fate/" + clipName;
        }
        if (speaker == DialogueManager.instance.ConscienceEntity)
        {
            path = "Audio/Conscience/" + clipName;
        }
        if (speaker == DialogueManager.instance.ShopkeeperEntity)
        {
            path = "Audio/" + clipName;
        }
        if (speaker == DialogueManager.instance.CatEntity)
        {
            path = "Audio/Cat/" + clipName;
        }
        if (speaker == DialogueManager.instance.CatToyEntity)
        {
            path = "Audio/" + clipName;
        }

        AudioClip audioClip = Resources.Load<AudioClip>(path);
        //string clipPath = System.IO.Path.Combine(, clipName);
        SourceVoice.Stop();
        if (audioClip != null)
        {
            SourceVoice.clip = audioClip;
            SourceVoice.Play();
        }
        return audioClip;
    }

    public bool IsVoiceClipPlaying()
    {
        return SourceVoice.isPlaying;
    }

    public void PlayOneShotEffect(AudioClip clip, float volumeScale = 1.0f)
    {
        SourceEffects.PlayOneShot(clip, volumeScale);
    }

    public void PlayDrawerEffect(AudioClip clip, ELeftOrRight drawerType, float volumeScale = 1.0f)
    {
        switch (drawerType)
        {
            case ELeftOrRight.Left:
                SourceEffectsLeftDrawer.Stop();
                SourceEffectsLeftDrawer.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster * volumeScale, 0.0f, 1.0f);
                SourceEffectsLeftDrawer.clip = clip;
                SourceEffectsLeftDrawer.Play();
                break;
            case ELeftOrRight.Right:
                SourceEffectsRightDrawer.Stop();
                SourceEffectsRightDrawer.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster * volumeScale, 0.0f, 1.0f);
                SourceEffectsRightDrawer.clip = clip;
                SourceEffectsRightDrawer.Play();
                break;
            case ELeftOrRight.MAX:
                break;
        }
    }

    public void ToggleMuteFidgetSpinnerLoop(bool mute)
    {
        SourceFidgetLoop.mute = mute;
    }

    public void ToggleFidgetSpinnerLoop(bool play)
    {
        if (play)
        {
            SourceFidgetLoop.Play();
        }
        else
        {
            SourceFidgetLoop.Stop();
        }
    }

    public void UpdateDrawerMuteStatus(ELeftOrRight drawerType)
    {
        switch (drawerType)
        {
            case ELeftOrRight.Left:
                SourceEffectsLeftDrawer.mute = !GrimDeskDrawer.instanceLeft.IsOpen();
                //Debug.Log("left drawer mute: " + SourceEffectsLeftDrawer.mute);
                break;
            case ELeftOrRight.Right:
                SourceEffectsRightDrawer.mute = !GrimDeskDrawer.instanceRight.IsOpen();
                //Debug.Log("right drawer mute: " + SourceEffectsRightDrawer.mute);
                break;
            case ELeftOrRight.MAX:
                break;
        }
    }


    public void UpdateFidgetLoopVolume(float factor)
    {
        SourceFidgetLoop.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster * factor, 0.0f, 1.0f);
    }

    public void UpdateChaosGlobeLoopVolume(float factor)
    {
        SourceChaosGlobeLoop.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster * factor, 0.0f, 1.0f);
    }

    public void UpdateMusicVolume()
    {
        if (bIsCrossfading || bIsFadingIn || bIsFadingOut)
        {
            return;
        }
        TargetMusicVolume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);
        TargetMusicVolumeAlpha = 0.0f;
        //SourceMusic.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);
    }

    public void UpdateVolumes()
    {
        TargetMusicVolume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);

        //SourceMusic.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);
        SourceEffects.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);
        SourceEffectsLeftDrawer.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);
        SourceEffectsRightDrawer.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);
        SourceVoice.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeVoice * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);
        SourceAmbientLoop.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);
        SourceFidgetLoop.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);
        SourceOfficeAmbience.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.VolumeGeneral * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f);

        TargetMusicVolumeAlpha = 0.0f;
    }

    public void PlayMusic(AudioClip clip, AudioClip crossfadeClip, float mainTime = 0.0f, float secondaryTime = 0.0f)
    {
        if (clip == null)
        {
            SourceMusic.mute = true;
            //Debug.LogWarning("Tried to play null music");
            return;
        }
        else
        {
            SourceMusic.mute = false;
        }
        if (clip == FateTheme)
        {
            if (bPlayedFateOnce)
            {
                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        mainTime = FateThemeTimestampFirst;
                        break;
                    case 1:
                        mainTime = FateThemeTimestampSecond;
                        break;
                    case 2:
                        mainTime = FateThemeTimestampThird;
                        break;

                }
            }
            else
            {
                mainTime = FateThemeTimestampFirst;
                bPlayedFateOnce = true;
            }
        }

        // NOOOOOOOOOOOOOOOOOO AAAAAAAAA MY EYES
        if (clip == ElevatorTheme)
        {
            if (bPlayedElevatorOnce)
            {
                int rand = Random.Range(0, 2);
                switch (rand)
                {
                    case 0:
                        mainTime = ElevatorThemeTimestampFirst;
                        break;
                    case 1:
                        mainTime = ElevatorThemeTimestampSecond;
                        break;

                }
            }
            else
            {
                mainTime = ElevatorThemeTimestampFirst;
                bPlayedElevatorOnce = true;
            }
        }

        // NOOOOOOOOOOOOOOOOOO AAAAAAAAA MY EYES
        if (clip == ShopTheme)
        {
            if (bPlayedMortimerOnce)
            {
                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        mainTime = MortimerThemeTimestampFirst;
                        break;
                    case 1:
                        mainTime = MortimerThemeTimestampSecond;
                        break;
                    case 2:
                        mainTime = MortimerThemeTimestampThird;
                        break;

                }
            }
            else
            {
                mainTime = MortimerThemeTimestampThird; // yes this is correct don't do it Ott
                bPlayedMortimerOnce = true;
            }
        }

        // NOOOOOOOOOOOOOOOOOO AAAAAAAAA MY EYES
        if (clip == DeskThemeNormal)
        {
            if (bPlayedDesktopOnce)
            {
                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        mainTime = DesktopThemeTimestampFirst;
                        break;
                    case 1:
                        mainTime = DesktopThemeTimestampSecond;
                        break;
                    case 2:
                        mainTime = DesktopThemeTimestampThird;
                        break;
                }
            }
            else
            {
                mainTime = DesktopThemeTimestampFirst;
                bPlayedDesktopOnce = true;
            }
        }

        // NOOOOOOOOOOOOOOOOOO AAAAAAAAA MY EYES
        if (clip == DeskThemeSwing)
        {
            if (bPlayedDesktopOnce)
            {
                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        mainTime = DesktopThemeSwingTimestampFirst;
                        break;
                    case 1:
                        mainTime = DesktopThemeSwingTimestampSecond;
                        break;
                    case 2:
                        mainTime = DesktopThemeSwingTimestampThird;
                        break;
                }
            }
            else
            {
                mainTime = DesktopThemeSwingTimestampFirst;
                bPlayedDesktopOnce = true;
            }
        }

        // NOOOOOOOOOOOOOOOOOO AAAAAAAAA MY EYES
        if (clip == DeskThemeChill)
        {
            if (bPlayedDesktopOnce)
            {
                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        mainTime = DesktopThemeChillTimestampFirst;
                        break;
                    case 1:
                        mainTime = DesktopThemeChillTimestampSecond;
                        break;
                    case 2:
                        mainTime = DesktopThemeChillTimestampThird;
                        break;
                }
            }
            else
            {
                mainTime = DesktopThemeChillTimestampFirst;
                bPlayedDesktopOnce = true;
            }
        }

        SourceMusic.Stop();
        //SourceMusic.loop = true;
        SourceMusic.clip = clip;
        SourceMusic.ignoreListenerVolume = true;
        SourceMusic.time = mainTime;
        SourceMusic.Play();

        SourceMusicSecondary.Stop();
        //SourceMusicSecondary.loop = true;
        SourceMusicSecondary.clip = crossfadeClip;
        SourceMusicSecondary.ignoreListenerVolume = true;
        SourceMusicSecondary.mute = true;
        SourceMusicSecondary.time = secondaryTime;
        SourceMusicSecondary.Play();
    }

    public void PlayButtonClick()
    {
        SourceEffects.PlayOneShot(ClipButtonClick, 0.7f);
    }

    public void PlayButtonClickMenu()
    {
        SourceEffects.PlayOneShot(ClipButtonClicksMenu[Random.Range(0, ClipButtonClicksMenu.Count)]);
    }

    public void PlayTestClip(float volume)
    {
        SourceTest.PlayOneShot(ClipTest, volume * 0.65f);
    }

    public void ToggleMusic()
    {
        SourceMusic.mute = !SourceMusic.mute;
    }

    public void UpdateDesktopMusic()
    {
        SwitchMusic(GetCurrentDesktopTheme(), GetCurrentDesktopTheme(), true);
    }

    public void StartAmbientLoop(AudioClip clip)
    {
        SourceAmbientLoop.Stop();
        SourceAmbientLoop.loop = true;
        SourceAmbientLoop.clip = clip;
        SourceAmbientLoop.Play();
    }

    public void StopAmbientLoop()
    {
        SourceAmbientLoop.Stop();
    }

    public void NotifyGamePaused()
    {
        if (SourceMusic.clip == MenuTheme)
        {
            return;
        }

        PausedClip = SourceMusic.clip;
        PausedClipTime = SourceMusic.time;
        PausedClipCrossfade = SourceMusicSecondary.clip;
        PausedClipCrossfadeTime = SourceMusicSecondary.time;

        if(SceneManager.GetActiveScene().name == "EndComic")
        {
            PlayMusic(MenuTheme, MenuTheme);
        }
        else
        {
            StartCoroutine(SwitchMusicRoutine(MenuTheme, MenuTheme));
        }
    }

    public void NotifyGameUnPaused()
    {
        if(PausedClip == null)
        {
            return;
        }
        StartCoroutine(SwitchMusicRoutine(PausedClip, PausedClipCrossfade, PausedClipTime, PausedClipCrossfadeTime));

    }

    public AudioClip GetCurrentDesktopTheme()
    {
        switch (SaveManager.instance.GetCurrentCarryoverPlayerState().CurrentDesktopMusic)
        {
            case EDesktopMusic.Normal:
                return DeskThemeNormal;
            case EDesktopMusic.Swing:
                return DeskThemeSwing;
            case EDesktopMusic.Chill:
                return DeskThemeChill;
            case EDesktopMusic.None:
                return null;
        }
        return null;
    }

    public void SwitchMusic(AudioClip newClip, AudioClip crossfadeClip, bool loop)
    {
        SourceMusic.loop = loop;
        StartCoroutine(SwitchMusicRoutine(newClip, crossfadeClip));
    }

    public void NotifySceneChange(EScene newScene)
    {
        SourceMusic.loop = true;
        switch (newScene)
        {
            case EScene.Elevator:
                StartCoroutine(SwitchMusicRoutine(ElevatorTheme, ElevatorTheme));
                break;
            case EScene.Desktop:
                StartCoroutine(SwitchMusicRoutine(GetCurrentDesktopTheme(), GetCurrentDesktopTheme()));
                break;
            case EScene.Office:
                StartCoroutine(SwitchMusicRoutine(FateTheme, FateTheme));
                break;
            case EScene.DressingRoom:
                StartCoroutine(SwitchMusicRoutine(BedroomTheme, BedroomTheme));
                break;
            case EScene.Shop:
                StartCoroutine(SwitchMusicRoutine(ShopTheme, ShopTheme));
                break;
            case EScene.Comic:


                break;
            case EScene.Intro:
                StartCoroutine(SwitchMusicRoutine(MenuTheme, MenuTheme));
                break;
            case EScene.MAX:
                break;
            case EScene.PostGame:
                SwitchMusic(IntroComicThemeDark, IntroComicTheme, true);

                break;
        }
    }

    public void Crossfade(AudioClip toClip)
    {
        StartCoroutine(CrossfadeRoutine(toClip, 1.0f));
    }

    public IEnumerator CrossfadeRoutine(AudioClip toClip, float duration)
    {
        bIsCrossfading = true;
        TargetMusicVolumeAlpha = 0.0f;
        float time = SourceMusic.time;
        SourceMusicSecondary.clip = toClip;
        SourceMusicSecondary.mute = false;
        SourceMusicSecondary.time = time;
        SourceMusicSecondary.Play();

        AudioClip previousClip = SourceMusic.clip;

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float volumeFactor = Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f);
            if (volumeFactor < 0.1f)
            {
                SourceMusic.clip = toClip;
                SourceMusic.time = SourceMusicSecondary.time;
                SourceMusic.Play();
            }
            TargetMusicVolume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f) * volumeFactor;
            //SourceMusic.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f) * volumeFactor;
            volumeFactor = Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f);
            SourceMusicSecondary.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f) * volumeFactor;
            yield return null;
        }

        TargetMusicVolume = SourceMusicSecondary.volume;
        //SourceMusic.volume = SourceMusicSecondary.volume;
        SourceMusicSecondary.clip = previousClip;
        SourceMusicSecondary.time = SourceMusic.time;
        SourceMusicSecondary.Play();
        SourceMusicSecondary.mute = true;

        bIsCrossfading = false;
    }

    public IEnumerator SwitchMusicRoutine(AudioClip newClip, AudioClip crossfadeClip, float mainTime = 0.0f, float secondaryTime = 0.0f)
    {
        yield return StartCoroutine(FadeOutMusicRoutine(0.6f));
        PlayMusic(newClip, crossfadeClip, mainTime, secondaryTime);
        yield return StartCoroutine(FadeInMusicRoutine(0.6f));
    }

    public IEnumerator FadeOutMusicRoutine(float duration)
    {
        TargetMusicVolumeAlpha = 0.0f;
        bIsFadingOut = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float volumeFactor = Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f);
            TargetMusicVolume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f) * volumeFactor;
            //SourceMusic.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f) * volumeFactor;
            yield return null;
        }
        bIsFadingOut = false;
    }

    public IEnumerator FadeInMusicRoutine(float duration)
    {
        TargetMusicVolumeAlpha = 0.0f;
        bIsFadingIn = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float volumeFactor = Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f);
            TargetMusicVolume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f) * volumeFactor;
            //SourceMusic.volume = Mathf.Clamp(SaveManager.instance.CurrentOptions.GetVolumeMusicLowered() * SaveManager.instance.CurrentOptions.VolumeMaster, 0.0f, 1.0f) * volumeFactor;
            yield return null;
        }
        bIsFadingIn = false;
    }
}
