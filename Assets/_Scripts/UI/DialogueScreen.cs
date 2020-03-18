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
using Articy.Unity.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SortingHelper
{
    public DialogueFragment Fragment;
    public float PositionY;
}

public class DialogueScreen : MonoBehaviour
{
    public static DialogueScreen instance;

    private void Awake()
    {
        instance = this;
    }

    public Image ImageBackground;
    public Image ImageSpeakerBackground;
    public Image ImageSpeaker;

    public Image ImageChair;
    public Image ImageSpeakerGrimHead;
    public Image ImageSpeakerGrimHeadMask;
    public Image ImageSpeakerGrimBody;
    public Image ImageDesk;

    public Text TextCurrent;

    public DialogueChoiceButton ButtonChoiceTemplate;
    public DialogueContinueButton ButtonContinue;

    [SerializeField]
    public Color ColorYou;
    [SerializeField]
    public Color ColorConscience;
    [SerializeField]
    public Color ColorCat;
    [SerializeField]
    public Color ColorFate;

    [SerializeField]
    List<Sprite> SpriteFateSpeaker;
    [SerializeField]
    List<Sprite> SpriteCatSpeaker;
    [SerializeField]
    List<Sprite> SpriteCatSpeakerWithToy;

    [SerializeField]
    Sprite MaskSpriteSuitTie;
    [SerializeField]
    Sprite MaskSpriteSuitBowtie;
    [SerializeField]
    Sprite MaskSpriteCape;

    [SerializeField]
    RectTransform PanelChoice;

    List<DialogueChoiceButton> ButtonChoiceList = new List<DialogueChoiceButton>();

    bool bIsFadingIn = false;
    bool bIsFadingOut = false;

    bool bIsFadingOutBackground = false;

    bool bSkipRequested = false;

    // Start is called before the first frame update
    void Start()
    {

        HideChoiceButtons();
        HideContinueButton();

        HideDialogueScreen();

        ImageSpeaker.color = new Color(ImageSpeaker.color.r, ImageSpeaker.color.g, ImageSpeaker.color.b, 0);
        ImageSpeakerBackground.color = new Color(ImageSpeakerBackground.color.r, ImageSpeakerBackground.color.g, ImageSpeakerBackground.color.b, 0);

        ImageChair.color = new Color(ImageChair.color.r, ImageChair.color.g, ImageChair.color.b, 0);
        ImageSpeakerGrimHead.color = new Color(ImageSpeakerGrimHead.color.r, ImageSpeakerGrimHead.color.g, ImageSpeakerGrimHead.color.b, 0);
        ImageSpeakerGrimBody.color = new Color(ImageSpeakerGrimBody.color.r, ImageSpeakerGrimBody.color.g, ImageSpeakerGrimBody.color.b, 0);
        ImageDesk.color = new Color(ImageDesk.color.r, ImageDesk.color.g, ImageDesk.color.b, 0);
        ImageSpeakerGrimHeadMask.gameObject.SetActive(false);
        //ImageSpeakerGrimHeadMask.color = new Color(ImageSpeakerBackground.color.r, ImageSpeakerBackground.color.g, ImageSpeakerBackground.color.b, 0);
        //HideDialogueScreen();
    }

    bool bTriggerSitFadeInDone = false;

    // Update is called once per frame
    void Update()
    {
        if(ArticyGlobalVariables.Default.game.subplot_sat_desk && !bTriggerSitFadeInDone)
        {
            StartCoroutine(FadeInSpeakerRoutineSitDown(1.5f));
            bTriggerSitFadeInDone = true;
        }
    }

    bool bDialogueStarting = false;

    //public void UpdateFromNode(DialogueNode node, DialogueChoice choice = null, bool isFirstNode = false)
    //{
    //    StartCoroutine(UpdateDialogueRoutine(0.5f, node, choice, isFirstNode));
    //}

    public void HideChoiceButtons()
    {
        for (int i = 0; i < ButtonChoiceList.Count; ++i)
        {
            Destroy(ButtonChoiceList[i].gameObject);
        }
        ButtonChoiceList.Clear();
    }

    public void ShowChoiceButtons(Hub hub)
    {
        HideChoiceButtons();
        if (hub.OutputPins[0].Connections.Count <= 0)
        {
            Debug.LogError("Hub has no choices!");
        }

        List<SortingHelper> sorts = new List<SortingHelper>();

        for (int i = 0; i < hub.OutputPins[0].Connections.Count; ++i)
        {
            DialogueFragment frag = hub.OutputPins[0].Connections[i].Target as DialogueFragment;
            IObjectWithPosition positionObject = hub.OutputPins[0].Connections[i].Target as IObjectWithPosition;

            SortingHelper sort = new SortingHelper();
            sort.Fragment = frag;
            sort.PositionY = positionObject.Position.y;
            sorts.Add(sort);
        }

        sorts = sorts.OrderBy(f => f.PositionY).ToList();

        for (int i = 0; i < sorts.Count; ++i)
        {
            if (sorts[i].Fragment.InputPins[0].Evaluate())
            {
                DialogueChoiceButton choiceButton = Instantiate(ButtonChoiceTemplate);

                choiceButton.gameObject.SetActive(true);
                choiceButton.gameObject.transform.SetParent(PanelChoice, false);
                choiceButton.SetChoice(i, sorts[i].Fragment);
                ButtonChoiceList.Add(choiceButton);
            }
        }
    }

    public void ShowChoiceButtons(DialogueFragment hubFragment)
    {
        HideChoiceButtons();
        if (hubFragment.OutputPins[0].Connections.Count <= 0)
        {
            Debug.LogError("Hub has no choices!");
        }

        List<SortingHelper> sorts = new List<SortingHelper>();

        for (int i = 0; i < hubFragment.OutputPins[0].Connections.Count; ++i)
        {
            DialogueFragment frag = hubFragment.OutputPins[0].Connections[i].Target as DialogueFragment;
            IObjectWithPosition positionObject = hubFragment.OutputPins[0].Connections[i].Target as IObjectWithPosition;

            SortingHelper sort = new SortingHelper();
            sort.Fragment = frag;
            sort.PositionY = positionObject.Position.y;
            sorts.Add(sort);
        }

        sorts = sorts.OrderBy(f => f.PositionY).ToList();

        for (int i = 0; i < sorts.Count; ++i)
        {
            if (sorts[i].Fragment.InputPins[0].Evaluate())
            {
                DialogueChoiceButton choiceButton = Instantiate(ButtonChoiceTemplate);

                choiceButton.gameObject.SetActive(true);
                choiceButton.gameObject.transform.SetParent(PanelChoice, false);
                choiceButton.SetChoice(i, sorts[i].Fragment);
                ButtonChoiceList.Add(choiceButton);
            }
        }
    }

    public void ShowContinueButton()
    {
        ButtonContinue.gameObject.SetActive(true);
    }

    public void HideContinueButton()
    {
        ButtonContinue.gameObject.SetActive(false);
    }


    public void ShowDialogueScreen()
    {
        gameObject.SetActive(true);
        if (ArticyGlobalVariables.Default.game.fate_absent)
        {
            int index = 0;
            switch (SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt())
            {
                case 9:
                    index = 1;
                    break;
                case 10:
                    index = 2;
                    break;
                case 11:
                    index = 1;
                    break;
                case 12:
                    index = 1;
                    break;
                case 13:
                    index = 0;
                    break;
                case 20:
                    index = 2;
                    break;
                case 21:
                    index = 3;
                    break;
            }

            if (ArticyGlobalVariables.Default.game.toy_given_to_cat)
            {
                ImageSpeaker.sprite = SpriteCatSpeakerWithToy[index];
            }
            else
            {
                ImageSpeaker.sprite = SpriteCatSpeaker[index];
            }
        }
        else
        {
            ImageSpeaker.sprite = SpriteFateSpeaker[Random.Range(0, SpriteFateSpeaker.Count)];
        }
    }

    public void HideDialogueScreen()
    {
        gameObject.SetActive(false);
    }

    public void RequestSkipDialogue()
    {
        bSkipRequested = true;
    }

    IEnumerator FadeInRoutine(float duration)
    {
        bIsFadingIn = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            if (bSkipRequested)
            {
                elapsedTime = duration;
            }
            TextCurrent.color = new Color(TextCurrent.color.r, TextCurrent.color.g, TextCurrent.color.b, elapsedTime / duration);
            for (int i = 0; i < ButtonChoiceList.Count; ++i)
            {
                ButtonChoiceList[i].TextChoice.color = new Color(ButtonChoiceList[i].ColorDefault.r, ButtonChoiceList[i].ColorDefault.g, ButtonChoiceList[i].ColorDefault.b, elapsedTime / duration);
            }
            ButtonContinue.TextContinue.color = new Color(ButtonContinue.ColorDefault.r, ButtonContinue.ColorDefault.g, ButtonContinue.ColorDefault.b, elapsedTime / duration);


            yield return null;
        }
        bSkipRequested = false;
        bIsFadingIn = false;
    }


    IEnumerator FadeOutRoutine(float duration)
    {
        bIsFadingOut = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            if (bSkipRequested)
            {
                elapsedTime = duration;
            }
            TextCurrent.color = new Color(TextCurrent.color.r, TextCurrent.color.g, TextCurrent.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            for (int i = 0; i < ButtonChoiceList.Count; ++i)
            {
                ButtonChoiceList[i].TextChoice.color = new Color(ButtonChoiceList[i].TextChoice.color.r, ButtonChoiceList[i].TextChoice.color.g, ButtonChoiceList[i].TextChoice.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            }
            ButtonContinue.TextContinue.color = new Color(ButtonContinue.TextContinue.color.r, ButtonContinue.TextContinue.color.g, ButtonContinue.TextContinue.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            yield return null;
        }
        bSkipRequested = false;
        bIsFadingOut = false;
    }

    public void NotifyEndDialogue(EScene targetScene, bool showDemoEndScreen = false)
    {
        StartCoroutine(EndDialogueRoutine(targetScene, showDemoEndScreen));
    }

    IEnumerator EndDialogueRoutine(EScene targetScene, bool showDemoEndScreen = false)
    {
        bDialogueStarting = true;
        yield return StartCoroutine(FadeOutRoutine(1.0f));
        yield return StartCoroutine(FadeOutSpeakerRoutine(1.0f));
        if (showDemoEndScreen)
        {
            DemoEndScreen.instance.ShowDemoEndScreen();
            yield return StartCoroutine(DemoEndScreen.instance.FadeInDemoThanksText(1.0f));
        }
        yield return StartCoroutine(FadeOutBackgroundRoutine(1.0f));

        if (!showDemoEndScreen)
        {
            ElevatorManager.instance.SwitchScene(targetScene);
        }

        HideDialogueScreen();
        TextCurrent.text = "";
        bDialogueStarting = false;
    }

    public void NotifyStartDialogue()
    {
        StartCoroutine(StartDialogueRoutine());
    }

    public bool IsDialogueStarting()
    {
        return bDialogueStarting;
    }

    IEnumerator StartDialogueRoutine()
    {
        bDialogueStarting = true;
        yield return StartCoroutine(FadeInBackgroundRoutine(1.0f));
        yield return StartCoroutine(FadeInSpeakerRoutine(1.0f));
        yield return StartCoroutine(FadeInRoutine(1.0f));

        HUDManager.instance.SetDayCounter(SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex());

        bDialogueStarting = false;
    }

    public IEnumerator FadeOutSpeakerRoutine(float duration)
    {
        bIsFadingOut = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            if (SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() >= 28 &&
                ArticyGlobalVariables.Default.game.subplot_finale_activated &&
                !ArticyGlobalVariables.Default.profile.fate_spared)
            {

                ImageChair.color = new Color(ImageChair.color.r, ImageChair.color.g, ImageChair.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
                ImageSpeakerGrimHead.color = new Color(ImageSpeakerGrimHead.color.r, ImageSpeakerGrimHead.color.g, ImageSpeakerGrimHead.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
                ImageSpeakerGrimBody.color = new Color(ImageSpeakerGrimBody.color.r, ImageSpeakerGrimBody.color.g, ImageSpeakerGrimBody.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
                ImageDesk.color = new Color(ImageDesk.color.r, ImageDesk.color.g, ImageDesk.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
                ImageSpeakerGrimHeadMask.gameObject.SetActive(true);
            }
            else
            {
                ImageSpeaker.color = new Color(ImageSpeaker.color.r, ImageSpeaker.color.g, ImageSpeaker.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));

                ImageSpeakerGrimHeadMask.gameObject.SetActive(false);
            }

            ImageSpeakerBackground.color = new Color(ImageSpeakerBackground.color.r, ImageSpeakerBackground.color.g, ImageSpeakerBackground.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));

            yield return null;
        }
        bIsFadingOut = false;
    }

    public IEnumerator FadeInSpeakerRoutineSitDown(float duration)
    {
        bIsFadingIn = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            ImageSpeakerGrimHead.sprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentHeadAsset();
            ImageSpeakerGrimBody.sprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentBodyAsset();

            if (ImageSpeakerGrimHead.sprite.name.Contains("cthulhu"))
            {
                ImageSpeakerGrimHeadMask.sprite = null;
            }
            else
            {
                if (ImageSpeakerGrimBody.sprite.name.Contains("cape"))
                {
                    ImageSpeakerGrimHeadMask.sprite = MaskSpriteCape;
                }
                else if (ImageSpeakerGrimBody.sprite.name.Contains("bowtie"))
                {
                    ImageSpeakerGrimHeadMask.sprite = MaskSpriteSuitBowtie;
                }
                else
                {
                    ImageSpeakerGrimHeadMask.sprite = MaskSpriteSuitTie;
                }
            }

            //ImageChair.color = new Color(ImageChair.color.r, ImageChair.color.g, ImageChair.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            ImageSpeakerGrimHead.color = new Color(ImageSpeakerGrimHead.color.r, ImageSpeakerGrimHead.color.g, ImageSpeakerGrimHead.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            ImageSpeakerGrimBody.color = new Color(ImageSpeakerGrimBody.color.r, ImageSpeakerGrimBody.color.g, ImageSpeakerGrimBody.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            //ImageDesk.color = new Color(ImageDesk.color.r, ImageDesk.color.g, ImageDesk.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            ImageSpeakerGrimHeadMask.gameObject.SetActive(true);

            //ImageSpeakerBackground.color = new Color(ImageSpeakerBackground.color.r, ImageSpeakerBackground.color.g, ImageSpeakerBackground.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));

            yield return null;
        }
        bIsFadingIn = false;
    }

    public IEnumerator FadeInSpeakerRoutine(float duration)
    {
        bIsFadingIn = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            if (SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() >= 28 &&
                ArticyGlobalVariables.Default.game.subplot_finale_activated &&
                !ArticyGlobalVariables.Default.profile.fate_spared)
            {

                //ImageSpeakerGrimHead.sprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentHeadAsset();
                //ImageSpeakerGrimBody.sprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentBodyAsset();

                //if (ImageSpeakerGrimHead.sprite.name.Contains("cthulhu"))
                //{
                //    ImageSpeakerGrimHeadMask.sprite = null;
                //}
                //else
                //{
                //    if (ImageSpeakerGrimBody.sprite.name.Contains("cape"))
                //    {
                //        ImageSpeakerGrimHeadMask.sprite = MaskSpriteCape;
                //    }
                //    else if (ImageSpeakerGrimBody.sprite.name.Contains("bowtie"))
                //    {
                //        ImageSpeakerGrimHeadMask.sprite = MaskSpriteSuitBowtie;
                //    }
                //    else
                //    {
                //        ImageSpeakerGrimHeadMask.sprite = MaskSpriteSuitTie;
                //    }
                //}                //ImageSpeakerGrimHead.sprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentHeadAsset();
                //ImageSpeakerGrimBody.sprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentBodyAsset();

                //if (ImageSpeakerGrimHead.sprite.name.Contains("cthulhu"))
                //{
                //    ImageSpeakerGrimHeadMask.sprite = null;
                //}
                //else
                //{
                //    if (ImageSpeakerGrimBody.sprite.name.Contains("cape"))
                //    {
                //        ImageSpeakerGrimHeadMask.sprite = MaskSpriteCape;
                //    }
                //    else if (ImageSpeakerGrimBody.sprite.name.Contains("bowtie"))
                //    {
                //        ImageSpeakerGrimHeadMask.sprite = MaskSpriteSuitBowtie;
                //    }
                //    else
                //    {
                //        ImageSpeakerGrimHeadMask.sprite = MaskSpriteSuitTie;
                //    }
                //}

                ImageChair.color = new Color(ImageChair.color.r, ImageChair.color.g, ImageChair.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
                //ImageSpeakerGrimHead.color = new Color(ImageSpeakerGrimHead.color.r, ImageSpeakerGrimHead.color.g, ImageSpeakerGrimHead.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
                //ImageSpeakerGrimBody.color = new Color(ImageSpeakerGrimBody.color.r, ImageSpeakerGrimBody.color.g, ImageSpeakerGrimBody.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
                ImageDesk.color = new Color(ImageDesk.color.r, ImageDesk.color.g, ImageDesk.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
                ImageSpeakerGrimHeadMask.gameObject.SetActive(true);
            }
            else
            {
                ImageSpeaker.color = new Color(ImageSpeaker.color.r, ImageSpeaker.color.g, ImageSpeaker.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
                ImageSpeakerGrimHeadMask.gameObject.SetActive(false);
            }

            ImageSpeakerBackground.color = new Color(ImageSpeakerBackground.color.r, ImageSpeakerBackground.color.g, ImageSpeakerBackground.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));

            yield return null;
        }
        bIsFadingIn = false;
    }

    public IEnumerator FadeOutBackgroundRoutine(float duration)
    {
        bIsFadingOut = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            ImageBackground.color = new Color(ImageBackground.color.r, ImageBackground.color.g, ImageBackground.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
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
            ImageBackground.color = new Color(ImageBackground.color.r, ImageBackground.color.g, ImageBackground.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            yield return null;
        }
        bIsFadingIn = false;
    }

    IEnumerator UpdateDialogueRoutine(float duration, DialogueFragment fragment, bool isFirstNode = false)
    {
        if (isFirstNode)
        {
            TextCurrent.color = new Color(TextCurrent.color.r, TextCurrent.color.g, TextCurrent.color.b, 0.0f);
            for (int i = 0; i < ButtonChoiceList.Count; ++i)
            {
                ButtonChoiceList[i].TextChoice.color = new Color(ButtonChoiceList[i].TextChoice.color.r, ButtonChoiceList[i].TextChoice.color.g, ButtonChoiceList[i].TextChoice.color.b, 0.0f);
            }
            ButtonContinue.TextContinue.color = new Color(ButtonContinue.TextContinue.color.r, ButtonContinue.TextContinue.color.g, ButtonContinue.TextContinue.color.b, 0.0f);
        }
        else
        {
            yield return FadeOutRoutine(duration);
        }


        string text = "";

        bool saySpawnCounter = false;
        if (fragment.OutputPins[0].Connections.Count == 1)
        {
            Jump jump = fragment.OutputPins[0].Connections[0].Target as Jump;
            Hub hub = fragment.OutputPins[0].Connections[0].Target as Hub;

            if (jump != null)
            {
                //if(jump.Target as DialogueFragment != null)
                //{
                //    fragment = jump.Target as DialogueFragment;
                //}
                hub = jump.Target as Hub;
            }

            text = fragment.Text;
            if(text.Contains("[SPAWN_COUNTER]"))
            {
                text = text.Replace("[SPAWN_COUNTER]", SaveManager.instance.GetCurrentPlayerState().GetSpawnCounter().ToString());
                saySpawnCounter = true;
            }
            TextCurrent.text = text;

            if (fragment.Speaker == DialogueManager.instance.FateEntity)
            {
                TextCurrent.color = ColorFate;
            }
            else if (fragment.Speaker == DialogueManager.instance.FatePhoneEntity)
            {
                TextCurrent.color = ColorFate;
                TextCurrent.text = "<i>" + TextCurrent.text + "</i>";
            }
            else if (fragment.Speaker == DialogueManager.instance.ConscienceEntity)
            {
                TextCurrent.color = ColorConscience;
            }
            else if (fragment.Speaker == DialogueManager.instance.CatEntity || fragment.Speaker == DialogueManager.instance.CatToyEntity)
            {
                TextCurrent.color = ColorCat;
            }

            Condition condition = fragment.OutputPins[0].Connections[0].Target as Condition;

            while (condition != null)
            {
                bool conditionResult = condition.Evaluate();

                if (conditionResult)
                {
                    DialogueFragment conditionFragment = condition.OutputPins[0].Connections[0].Target as DialogueFragment;
                    hub = condition.OutputPins[0].Connections[0].Target as Hub;
                    condition = condition.OutputPins[0].Connections[0].Target as Condition;
                }
                else
                {
                    DialogueFragment conditionFragment = condition.OutputPins[1].Connections[0].Target as DialogueFragment;
                    hub = condition.OutputPins[1].Connections[0].Target as Hub;
                    condition = condition.OutputPins[1].Connections[0].Target as Condition;
                }
            }



            if (hub != null)
            {
                hub.OutputPins[0].Evaluate();
                ShowChoiceButtons(hub);
                HideContinueButton();
            }
            else
            {
                ShowContinueButton();
                HideChoiceButtons();
            }
        }
        else
        {
            text = fragment.Text;
            if (text.Contains("[SPAWN_COUNTER]"))
            {
                text = text.Replace("[SPAWN_COUNTER]", SaveManager.instance.GetCurrentPlayerState().GetSpawnCounter().ToString());
                saySpawnCounter = true;
            }
            TextCurrent.text = text;
            if (fragment.Speaker == DialogueManager.instance.FateEntity)
            {
                TextCurrent.color = ColorFate;
            }
            else if (fragment.Speaker == DialogueManager.instance.FatePhoneEntity)
            {
                TextCurrent.color = ColorFate;
                TextCurrent.text = "<i>" + TextCurrent.text + "</i>";
            }
            else if (fragment.Speaker == DialogueManager.instance.ConscienceEntity)
            {
                TextCurrent.color = ColorConscience;
            }
            else if (fragment.Speaker == DialogueManager.instance.CatEntity || fragment.Speaker == DialogueManager.instance.CatToyEntity)
            {
                TextCurrent.color = ColorCat;
            }

            ShowChoiceButtons(fragment);
            HideContinueButton();
        }

        if (fragment.StageDirections != "")
        {
            AudioClip clippy = AudioManager.instance.PlayVoiceClip(fragment.StageDirections, fragment.Speaker as Entity);
            if (saySpawnCounter)
            {
                AudioManager.instance.SaySpawnCounter(clippy.length);
            }
        }

        yield return FadeInRoutine(duration);
    }

    public bool IsBusy()
    {
        return bIsFadingIn || bIsFadingOut;
    }

    public void UpdateFromDialogueFragment(DialogueFragment fragment, bool isFirstNode = false)
    {
        StartCoroutine(UpdateDialogueRoutine(0.5f, fragment, isFirstNode));
    }

    public void UpdateFromHub(DialogueFragment fragment, bool isFirstNode = false)
    {
        StartCoroutine(UpdateDialogueRoutine(0.5f, fragment, isFirstNode));
    }
}