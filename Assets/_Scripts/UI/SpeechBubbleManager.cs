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
using TMPro;
using UnityEngine;

public class SpeechBubbleManager : ManagerBase
{
    public static SpeechBubbleManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    GameObject SpeechBubbleSpeakerMarkerConscience;

    [SerializeField]
    GameObject SpeechBubbleSpeakerMarkerShopkeeper;

    [SerializeField]
    List<SpeechBubble> ShopkeeperBubbleTemplates = new List<SpeechBubble>();

    [SerializeField]
    List<GameObject> SpeechBubbleMarkers = new List<GameObject>();

    [SerializeField]
    List<SpeechBubble> SpeechBubbleTemplates = new List<SpeechBubble>();

    [SerializeField]
    SpeechBubble FallbackBubble = null;

    [SerializeField]
    public CloudParticleEngine CloudParticleTemplate;

    [SerializeField]
    public Color ColorYou;
    [SerializeField]
    public Color ColorConscience;
    [SerializeField]
    public Color ColorShopkeeper;

    [SerializeField]
    float FadeTime = 3.0f;

    float CurrentFadeTime = 0.0f;
    DialogueFragment CurrentDialogueFragment;
    Hub CurrentHub;
    bool bIsBubbleSpeechActive = false;

    SpeechBubble SpeechBubbleSpeaker = null;
    List<SpeechBubble> SpawnedBubbles = new List<SpeechBubble>();

    public SpeechBubble GetSpeakerBubble()
    {
        return SpeechBubbleSpeaker;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitManager();
    }

    public bool IsBubbleSpeechActive()
    {
        return bIsBubbleSpeechActive;
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsBubbleSpeechActive)
        {
            if (CurrentHub == null)
            {
                CurrentFadeTime -= Time.deltaTime;
                if (CurrentFadeTime <= 0.0f)
                {
                    bIsBubbleSpeechActive = false;
                    

                    ArticyObject element = DialogueManager.instance.GetNextDialogueElementFromFragment(CurrentDialogueFragment);

                    if (element != null && element != CurrentDialogueFragment)
                    {
                        ContinueBubbleCascadeFromElement(element);
                    }
                    else
                    {
                        HideAllBubbles();
                        AudioManager.instance.UpdateMusicVolume();
                    }
                }
            }
        }
    }

    public SpeechBubble GetSpeechBubbleForString(string text)
    {
        List<SpeechBubble> fittingTemplates = new List<SpeechBubble>();
        for (int i = 0; i < SpeechBubbleTemplates.Count; ++i)
        {
            if (SpeechBubbleTemplates[i].CanFitText(text))
            {
                fittingTemplates.Add(SpeechBubbleTemplates[i]);
            }
        }

        if(fittingTemplates.Count > 0)
        {
            return fittingTemplates[Random.Range(0, fittingTemplates.Count)];
        }

        return FallbackBubble;
    }

    public override void InitManager()
    {
        base.InitManager();

        HideAllBubbles();
    }

    public void HideAllBubbles()
    {
        ShopMortimer.instance.SetMortimerTalking(false);
        if (SpeechBubbleSpeaker != null)
        {
            Destroy(SpeechBubbleSpeaker.gameObject);
        }
        for (int i = 0; i < SpawnedBubbles.Count; ++i)
        {
            Destroy(SpawnedBubbles[i].gameObject);
        }
        SpawnedBubbles.Clear();
    }

    public void SkipBubble()
    {
        CurrentFadeTime = 0.0f;
    }

    private void MakeSpeakerBubble(bool addFX = true)
    {
        if (CurrentDialogueFragment.Speaker == DialogueManager.instance.ShopkeeperEntity)
        {
            SpeechBubbleSpeaker = Instantiate(ShopkeeperBubbleTemplates[Random.Range(0, ShopkeeperBubbleTemplates.Count)]);
            SpeechBubbleSpeaker.gameObject.transform.SetParent(SpeechBubbleSpeakerMarkerShopkeeper.gameObject.transform);
            SpeechBubbleSpeaker.gameObject.transform.localPosition = new Vector3();
            SpeechBubbleSpeaker.gameObject.transform.localScale = new Vector3(1, 1, 1);

            if(addFX)
            {
                ShopMortimer.instance.SetMortimerTalking(true);
            }
        }
        else
        {
            SpeechBubbleSpeaker = Instantiate(GetSpeechBubbleForString(CurrentDialogueFragment.Text));
            SpeechBubbleSpeaker.gameObject.transform.SetParent(SpeechBubbleSpeakerMarkerConscience.gameObject.transform);
            SpeechBubbleSpeaker.gameObject.transform.localPosition = new Vector3();
            SpeechBubbleSpeaker.gameObject.transform.localScale = new Vector3(1, 1, 1);
            SpeechBubbleSpeaker.InitOrbit();
        }

        if (addFX && CurrentDialogueFragment.StageDirections != "")
        {
            AudioClip playedClip = AudioManager.instance.PlayVoiceClip(CurrentDialogueFragment.StageDirections, CurrentDialogueFragment.Speaker as Entity);
            CurrentFadeTime = playedClip != null ? playedClip.length + 0.2f : FadeTime;
        }

        SpeechBubbleSpeaker.ShowBubble(CurrentDialogueFragment);
    }

    public void StartBubbleCascadeFromDialogue(Dialogue dialogue)
    {
        if(dialogue == null)
        {
            Debug.LogError("WTF null dialogue");
            return;
        }
        CurrentFadeTime = FadeTime;

        CurrentDialogueFragment = DialogueManager.instance.GetStartFragmentForDialogue(dialogue);

        if (SpeechBubbleSpeaker != null)
        {
            Destroy(SpeechBubbleSpeaker.gameObject);
        }

        Shop.instance.SetPriceText(0);
        Shop.instance.SetNameText("");

        MakeSpeakerBubble();

        bIsBubbleSpeechActive = true;
        AudioManager.instance.UpdateMusicVolume();
        Debug.Log("Bubble Dialogue");
    }

    public void StartSingleBubble(DialogueFragment frag)
    {
        CurrentFadeTime = FadeTime;

        CurrentDialogueFragment = frag;

        if (SpeechBubbleSpeaker != null)
        {
            Destroy(SpeechBubbleSpeaker.gameObject);
        }

        Shop.instance.SetPriceText(0);
        Shop.instance.SetNameText("");

        MakeSpeakerBubble();

        bIsBubbleSpeechActive = true;
        AudioManager.instance.UpdateMusicVolume();
        Debug.Log("Bubble Dialogue");
    }

    public void ContinueBubbleCascadeFromElement(ArticyObject element)
    {
        CurrentFadeTime = FadeTime;

        //clicked on speaker bubble
        //if (element == null)
        //{
        //    element = DialogueManager.instance.GetNextDialogueElementFromFragment(CurrentDialogueFragment);
        //}

        // end
        if (element == null)
        {
            HideAllBubbles();
            bIsBubbleSpeechActive = false;
            return;
        }

        DialogueFragment fragment = element as DialogueFragment;
        Hub hub = element as Hub;

        bIsBubbleSpeechActive = true;

        HideAllBubbles();

        //for (int i = 0; i < SpeechBubbleOptions.Count; ++i)
        //{
        //    SpeechBubbleOptions[i].HideBubble();
        //}

        if (fragment != null)
        {
            CurrentDialogueFragment.OutputPins[0].Evaluate();
            //CurrentDialogueFragment.
            CurrentDialogueFragment = fragment;
            //if (SpeechBubbleSpeaker != null)
            //{
            //    Destroy(SpeechBubbleSpeaker.gameObject);
            //}

            MakeSpeakerBubble();

            CurrentHub = null;
        }
        else if (hub != null)
        {
            CurrentHub = hub;

            MakeSpeakerBubble(false);

            hub.OutputPins[0].Evaluate();

            for (int i = 0; i < hub.OutputPins[0].Connections.Count; ++i)
            {
                //if(hub.OutputPins[0].Connections[i].)
                DialogueFragment frag = hub.OutputPins[0].Connections[i].Target as DialogueFragment;
                if (frag.InputPins[0].Evaluate())
                {
                    SpeechBubble newBubble = Instantiate(GetSpeechBubbleForString(frag.Text));
                    newBubble.ShowBubble(frag);
                    newBubble.gameObject.transform.SetParent(SpeechBubbleMarkers[i].gameObject.transform);
                    newBubble.gameObject.transform.localPosition = new Vector3();
                    newBubble.gameObject.transform.localScale = new Vector3(1, 1, 1);
                    newBubble.InitOrbit();



                    SpawnedBubbles.Add(newBubble);
                    //SpeechBubbleOptions[i].ShowBubble(frag);
                }
            }
        }
        else
        {
            Debug.LogError("Fail ott pls fix speechbubblemanager");
            SpeechBubbleSpeaker.HideBubble();
        }
    }
}
