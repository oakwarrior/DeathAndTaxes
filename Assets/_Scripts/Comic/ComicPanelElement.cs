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
using UnityEngine;

public enum EScrollTriggerType
{
    SwapSprite,
    PlayClip,
    PlayLoopingClip,
    StopLoopingClip,
    Crossfade
}

[System.Serializable]
public class ScrollTrigger
{
    [SerializeField]
    public GameObject TriggerLocationMarker = null;
    [SerializeField]
    public List<EScrollTriggerType> Triggers = new List<EScrollTriggerType>();
    [SerializeField]
    public Sprite TriggerSprite = null;
    [SerializeField]
    public AudioClip TriggerClip = null;
    [System.NonSerialized]
    public bool bHasTriggered = false;
    [System.NonSerialized]
    public Sprite PreviousSprite = null;
    [System.NonSerialized]
    public AudioClip PreviousClip = null;
}

public class ComicPanelElement : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer Renderer = null;
    [SerializeField]
    List<ScrollTrigger> TriggerList = new List<ScrollTrigger>();
    [SerializeField]
    float ParallaxFactor = 1.0f;
    [SerializeField]
    float ParallaxOffsetLeft = 0.0f;
    [SerializeField]
    bool bInvertParallax = false;

    int PanelLine = 0;
    ComicPanel ParentPanel = null;
    AudioClip PreviousClip = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ComicManager.instance.GetCurrentPanelLine() == PanelLine && !ComicManager.instance.IsSwitchingLine())
        {
            for (int i = 0; i < TriggerList.Count; ++i)
            {
                if (!TriggerList[i].bHasTriggered && Camera.main.transform.position.x > TriggerList[i].TriggerLocationMarker.transform.position.x)
                {
                    EngageTrigger(TriggerList[i]);
                }
                if (TriggerList[i].bHasTriggered && Camera.main.transform.position.x < TriggerList[i].TriggerLocationMarker.transform.position.x)
                {
                    ReverseTrigger(TriggerList[i]);
                }
            }
        }

        if (ParentPanel != null && Renderer.sprite != null)
        {
            Bounds parentBounds = ParentPanel.Mask.bounds;
            float xOffset = (Renderer.sprite.bounds.extents.x - parentBounds.extents.x) * ParallaxFactor;

            Vector3 cameraPos = Camera.main.transform.position;
            cameraPos.z = 0;

            float cameraDistance = (ParentPanel.gameObject.transform.position.x - cameraPos.x);

            float distanceToCenterNormalizedLeft = MapValue(cameraDistance, -ParentPanel.Mask.bounds.extents.x, ParentPanel.Mask.bounds.extents.x, 0.0f, 1.0f);
            float elementX = 0.0f;

            if (bInvertParallax)
            {
                elementX = MapValue(cameraDistance, ParentPanel.Mask.bounds.extents.x, -ParentPanel.Mask.bounds.extents.x, -xOffset, xOffset);
            }
            else
            {
                elementX = MapValue(cameraDistance, -ParentPanel.Mask.bounds.extents.x, ParentPanel.Mask.bounds.extents.x, -xOffset, xOffset);
            }

            Vector3 elementPosition = new Vector3(elementX, 0);

            gameObject.transform.localPosition = elementPosition;
        }
    }

    public static float MapValue(float value, float from1, float to1, float from2, float to2)
    {
        if (from2 > to2)
        {
            return Mathf.Clamp((value - from1) / (to1 - from1) * (to2 - from2) + from2, to2, from2);

        }
        else
        {
            return Mathf.Clamp((value - from1) / (to1 - from1) * (to2 - from2) + from2, from2, to2);
        }
    }

    public void EngageTrigger(ScrollTrigger trigger)
    {
        trigger.bHasTriggered = true;
        if (trigger.Triggers.Contains(EScrollTriggerType.SwapSprite))
        {
            trigger.PreviousSprite = Renderer.sprite;
            Renderer.sprite = trigger.TriggerSprite;
        }
        if (trigger.Triggers.Contains(EScrollTriggerType.PlayClip) && trigger.TriggerClip != null)
        {
            AudioManager.instance.PlayOneShotEffect(trigger.TriggerClip);
        }
        if (trigger.Triggers.Contains(EScrollTriggerType.PlayLoopingClip) && trigger.TriggerClip != null)
        {
            AudioManager.instance.StartAmbientLoop(trigger.TriggerClip);
            trigger.PreviousClip = AudioManager.instance.SourceAmbientLoop.clip;
        }
        if (trigger.Triggers.Contains(EScrollTriggerType.StopLoopingClip))
        {
            AudioManager.instance.StopAmbientLoop();
        }
        if (trigger.Triggers.Contains(EScrollTriggerType.Crossfade))
        {
            trigger.PreviousClip = AudioManager.instance.SourceMusic.clip;
            AudioManager.instance.Crossfade(trigger.TriggerClip);
        }
    }

    public void ReverseTrigger(ScrollTrigger trigger)
    {
        trigger.bHasTriggered = false;
        if (trigger.Triggers.Contains(EScrollTriggerType.SwapSprite) && trigger.TriggerSprite != null)
        {
            Renderer.sprite = trigger.PreviousSprite;
        }
        if (trigger.Triggers.Contains(EScrollTriggerType.PlayLoopingClip))
        {
            //TODO: Add re-starting previous ambient loops?
            AudioManager.instance.StopAmbientLoop();
        }
        if (trigger.Triggers.Contains(EScrollTriggerType.Crossfade))
        {
            AudioManager.instance.Crossfade(trigger.PreviousClip);
        }
    }

    public void SetPanelLine(int lineIndex)
    {
        PanelLine = lineIndex;
    }

    public void SetPanel(ComicPanel panel)
    {
        ParentPanel = panel;
    }
}
