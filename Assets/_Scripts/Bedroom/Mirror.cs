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

public class Mirror : MonoBehaviour, Interactable
{
    public static Mirror instance;

    [SerializeField]
    SpriteRenderer LookRendererHead;
    [SerializeField]
    SpriteRenderer LookRendererBody;
    [SerializeField]
    TextMeshPro TextHead;
    [SerializeField]
    TextMeshPro TextBody;
    [SerializeField]
    Sprite MaskSpriteSuitTie;
    [SerializeField]
    Sprite MaskSpriteSuitBowtie;
    [SerializeField]
    Sprite MaskSpriteCape;
    [SerializeField]
    SpriteMask Mask;
    //[SerializeField]
    //ElevatorButton ElevatorButtonBedroom;

    [SerializeField]
    public SpriteRenderer BackgroundRenderer;

    [SerializeField]
    private ArticyRef ConscienceDialogueFolder;

    [SerializeField]
    private ArticyRef ConscienceDialogueIntro;

    [SerializeField]
    private ArticyRef ConscienceDialogueBanter;

    [SerializeField]
    private ArticyRef ConscienceDialogueEcologyWarning;

    [SerializeField]
    private ArticyRef ConscienceDialogueEcologyFailure;

    [SerializeField]
    private ArticyRef ConscienceDialoguePeaceWarning;

    [SerializeField]
    private ArticyRef ConscienceDialoguePeaceFailure;

    [SerializeField]
    private ArticyRef ConscienceDialogueHealthWarning;

    [SerializeField]
    private ArticyRef ConscienceDialogueHealthFailure;

    [SerializeField]
    private ArticyRef ConscienceDialogueProsperityWarning;

    [SerializeField]
    private ArticyRef ConscienceDialogueProsperityFailure;



    private List<template_conscience> DialogueList = new List<template_conscience>();

    private List<template_conscience> PendingDialogueList = new List<template_conscience>();

    public string GetHoverText()
    {
        return "";
    }

    public void Interact()
    {
        ElevatorManager.instance.SwitchScene(EScene.Elevator);
        //ToggleMirror(false);
    }

    public void Hover()
    {

    }

    public void UpdateDragGrabPosition(Vector3 position)
    {

    }

    public bool CanDrag()
    {
        return false;
    }

    public bool IsDragging()
    {
        return false;
    }

    public void ToggleDragging(bool drag)
    {

    }

    public void Unhover()
    {

    }

    private void Awake()
    {
        instance = this;

        if (ConscienceDialogueFolder.HasReference)
        {
            ArticyObject conscienceFolder = ConscienceDialogueFolder.GetObject<ArticyObject>();

            if (conscienceFolder != null)
            {
                for (int i = 0; i < conscienceFolder.Children.Count; ++i)
                {
                    template_conscience dialogue = conscienceFolder.Children[i] as template_conscience;
                     
                    if (dialogue != null)
                    {
                        DialogueList.Add(dialogue);
                    }
                }
            }
        }
        
    }

    public bool IsDialoguePending()
    {
        return PendingDialogueList.Count > 0;
    }

    public void CheckDialoguePendingStatus() // TODO: maybe make this less shit?
    {
        for (int i = 0; i < DialogueList.Count; ++i)
        {
            if(DialogueList[i].Template.dialogue_appearance_check.dialogue_appearance_condition.CallScript())
            {
                if(!PendingDialogueList.Contains(DialogueList[i]))
                {
                    PendingDialogueList.Add(DialogueList[i]);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //ToggleMirror(false);


    }

    private void OnEnable()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMirror(bool isVisible)
    {
        RefreshMirror();
        gameObject.SetActive(isVisible);
        if (isVisible)
        {
            CheckDialoguePendingStatus();
            if(PendingDialogueList.Count > 0)
            {
                SpeechBubbleManager.instance.StartBubbleCascadeFromDialogue(PendingDialogueList[0]);
                PendingDialogueList.RemoveAt(0);
            }

            //Bed.instance.gameObject.SetActive(false);
            //ElevatorButtonBedroom.gameObject.SetActive(false);
            //MirrorButton.instance.gameObject.SetActive(false);
        }
        else
        {
            //Bed.instance.gameObject.SetActive(true);
            //ElevatorButtonBedroom.gameObject.SetActive(true);
            //MirrorButton.instance.gameObject.SetActive(true);

        }
    }

    public void PreviousHead()
    {
        SaveManager.instance.GetCurrentCarryoverPlayerState().DecrementHeadIndex();
        RefreshMirror();
    }

    public void NextHead()
    {
        SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementHeadIndex();
        RefreshMirror();
    }

    public void PreviousBody()
    {
        SaveManager.instance.GetCurrentCarryoverPlayerState().DecrementBodyIndex();
        RefreshMirror();
    }

    public void NextBody()
    {
        SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementBodyIndex();
        RefreshMirror();
    }

    public void RefreshMirror()
    {
        TextHead.text = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentHeadItem().Template.item_accessory_variation.item_variation_name;
        TextBody.text = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentBodyItem().Template.item_accessory_variation.item_variation_name;

        LookRendererHead.sprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentHeadAsset();
        LookRendererBody.sprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentBodyAsset();

        if (LookRendererHead.sprite.name.Contains("cthulhu"))
        {
            LookRendererHead.maskInteraction = SpriteMaskInteraction.None;
        }
        else
        {
            LookRendererHead.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            if (LookRendererBody.sprite.name.Contains("cape"))
            {
                Mask.sprite = MaskSpriteCape;
            }
            else if (LookRendererBody.sprite.name.Contains("bowtie"))
            {
                Mask.sprite = MaskSpriteSuitBowtie;
            }
            else
            {
                Mask.sprite = MaskSpriteSuitTie;
            }
        }

    }
}
