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
using UnityEngine;

public class ComicAppearance : MonoBehaviour
{
    public static ComicAppearance instance;

    [SerializeField]
    SpriteRenderer LookRendererHead;
    [SerializeField]
    SpriteRenderer LookRendererBody;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleComicAppearanceVisibility(bool isVisible)
    {
        Refresh();
        gameObject.SetActive(isVisible);
        if (isVisible)
        {
            //SpeechBubbleManager.instance.StartBubbleCascadeFromDialogue(DialogueList[0]);
            Bed.instance.gameObject.SetActive(false);
            MirrorButton.instance.gameObject.SetActive(false);
        }
        else
        {
            Bed.instance.gameObject.SetActive(true);
            MirrorButton.instance.gameObject.SetActive(true);

        }
    }

    public void PreviousHead()
    {
        SaveManager.instance.GetCurrentCarryoverPlayerState().DecrementHeadIndex();
        Refresh();
    }

    public void NextHead()
    {
        SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementHeadIndex();
        Refresh();
    }

    public void PreviousBody()
    {
        SaveManager.instance.GetCurrentCarryoverPlayerState().DecrementBodyIndex();
        Refresh();
    }

    public void NextBody()
    {
        SaveManager.instance.GetCurrentCarryoverPlayerState().IncrementBodyIndex();
        Refresh();
    }

    [SerializeField]
    Sprite MaskSpriteSuitTie;
    [SerializeField]
    Sprite MaskSpriteSuitBowtie;
    [SerializeField]
    Sprite MaskSpriteCape;
    [SerializeField]
    SpriteMask Mask;

    public void Refresh()
    {
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
