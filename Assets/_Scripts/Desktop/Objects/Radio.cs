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

public class Radio : DeskItem, Interactable, Draggable, Ownable
{
    public static Radio instance;

    private void Awake()
    {
        instance = this;

        List<SpriteRenderer> renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            OriginalSortOrders.Add(renderers[i], renderers[i].sortingOrder);
        }
        OriginalZ = gameObject.transform.localPosition.z;
        OriginPosition = gameObject.transform.position;

    }

    public AudioClip ClipInteract;

    [SerializeField]
    public AudioClip ClipPositiveEcology;
    [SerializeField]
    public AudioClip ClipPositivePeace;
    [SerializeField]
    public AudioClip ClipPositiveProsperity;
    [SerializeField]
    public AudioClip ClipPositiveHealth;

    [SerializeField]
    public AudioClip ClipNegativeEcology;
    [SerializeField]
    public AudioClip ClipNegativePeace;
    [SerializeField]
    public AudioClip ClipNegativeProsperity;
    [SerializeField]
    public AudioClip ClipNegativeHealth;

    

    public int TimesClicked;

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<SpriteRenderer, int> OriginalSortOrders = new Dictionary<SpriteRenderer, int>();
    float OriginalZ = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsDragging())
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = gameObject.transform.position.z;
            gameObject.transform.position = worldPoint + GrabPosition;
        }
        else
        {
            if (IsInDrawer())
            {
                if (!GetDrawer().Collider.OverlapPoint(gameObject.transform.position))
                {
                    if (GetDrawer().Type == ELeftOrRight.Left)
                    {
                        gameObject.transform.localPosition = new Vector3(-5, 0, -1);
                    }
                    else
                    {
                        gameObject.transform.localPosition = new Vector3(5, 0, -1);
                    }
                }
            }
            else
            {
                CheckAndCorrectOutOfBounds();
            }
        }
    }


    //public string GetHoverText()
    //{
    //    return "An old radio. It only plays one song though.";
    //}

    public void Interact()
    {
        TimesClicked++;
        AudioManager.instance.PlayOneShotEffect(ClipInteract);

        EDesktopMusic currentMusic = SaveManager.instance.GetCurrentCarryoverPlayerState().CurrentDesktopMusic;
        if(currentMusic == EDesktopMusic.None)
        {
            currentMusic = EDesktopMusic.Normal;
        }
        else
        {
            currentMusic++;
        }

        SaveManager.instance.GetCurrentCarryoverPlayerState().CurrentDesktopMusic = currentMusic;

        AudioManager.instance.UpdateDesktopMusic();

        SaveManager.instance.MarkSavegameDirty();
        
    }

    public void Hover()
    {

    }

    public bool CanDrag()
    {
        return true;
    }

    public bool IsDragging()
    {
        return bIsDragging;
    }

    public void ToggleDragging(bool drag)
    {
        bIsDragging = drag;
    }

    public void Unhover()
    {

    }

    public void UpdateDragGrabPosition(Vector3 position)
    {
        position.z = gameObject.transform.position.z;
        GrabPosition = gameObject.transform.position - position;
    }

    public void HandleDragOutOfDrawer()
    {
        gameObject.transform.SetParent(GrimDesk.instance.MasterObjectTransform);
        List<SpriteRenderer> renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            renderers[i].maskInteraction = SpriteMaskInteraction.None;
            renderers[i].sortingOrder = OriginalSortOrders[renderers[i]];
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = OriginalZ;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = ELeftOrRight.MAX;
    }

    public void HandleDropIntoDrawer(GrimDeskDrawer hitDrawer)
    {
        gameObject.transform.SetParent(hitDrawer.transform);

        List<SpriteRenderer> renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            renderers[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            renderers[i].sortingOrder = 1;
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = -1;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }

    public void HandleUsed()
    {

    }

    public void HandleOwnedStatus()
    {
        if (ArticyGlobalVariables.Default.inventory.radio)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
