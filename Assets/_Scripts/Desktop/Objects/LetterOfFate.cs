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
using TMPro;
using UnityEngine;

public class LetterOfFate : DeskItem, Interactable, Draggable
{
    public static LetterOfFate instance;

    private void Awake()
    {
        instance = this;

        List<SpriteRenderer> renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            OriginalSortOrders.Add(renderers[i], renderers[i].sortingOrder);
        }
        TextComponents = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < TextComponents.Count; ++i)
        {
            OriginalSortOrdersText.Add(TextComponents[i], TextComponents[i].sortingOrder);
        }
        OriginalZ = gameObject.transform.localPosition.z;

        Renderer.sprite = DailyClosedSprite;
        TextLetterTop.enabled = false;
        TextLetterBottom.enabled = false;
        TextLetterFront.enabled = true;

        OriginPosition = gameObject.transform.position;

    }

    public SpriteRenderer Renderer;

    public TextMeshPro TextLetterTop;
    public TextMeshPro TextLetterBottom;
    public TextMeshPro TextLetterFront;

    public Vector3 RotationDrawerEuler;
    public Vector3 RotationDeskEuler;

    public Transform LetterMarker;

    public List<Sprite> DailyLetterSpritesClosed = new List<Sprite>();

    public List<Sprite> DailyLetterSpritesOpen = new List<Sprite>();

    public List<string> DailyLetterFrontText = new List<string>();

    public Collider2D ColliderOpen;
    public Collider2D ColliderClosed;

    Sprite DailyOpenSprite = null;
    Sprite DailyClosedSprite = null;
    bool bIsOpen = false;

    

    public int TimesClicked;

    [SerializeField]
    AudioClip ClipInteract;

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<SpriteRenderer, int> OriginalSortOrders = new Dictionary<SpriteRenderer, int>();
    Dictionary<TextMeshPro, int> OriginalSortOrdersText = new Dictionary<TextMeshPro, int>();
    List<TextMeshPro> TextComponents = new List<TextMeshPro>();
    float OriginalZ = 0;

    // Start is called before the first frame update
    void Start()
    {
        ColliderOpen.enabled = false;
        ColliderClosed.enabled = true;
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
        for (int i = 0; i < TextComponents.Count; ++i)
        {
            TextComponents[i].sortingOrder = OriginalSortOrdersText[TextComponents[i]];
            TextComponents[i].gameObject.SetActive(true);
        }



        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = OriginalZ;
        gameObject.transform.localPosition = tmp;
        gameObject.transform.eulerAngles = RotationDeskEuler;
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
        for (int i = 0; i < TextComponents.Count; ++i)
        {
            TextComponents[i].sortingOrder = 1;
            TextComponents[i].gameObject.SetActive(false);
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = -1;
        gameObject.transform.localPosition = tmp;

        gameObject.transform.eulerAngles = RotationDrawerEuler;

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }


    public override string GetHoverText()
    {
        return "My instructions for the day...";
    }

    public void Interact()
    {
        TimesClicked++;
        AudioManager.instance.PlayOneShotEffect(ClipInteract);
        if (bIsOpen)
        {
            CloseLetter();
        }
        else
        {
            OpenLetter();
        }
    }

    public void OpenLetter()
    {
        bIsOpen = true;
        Renderer.sprite = DailyOpenSprite;
        TextLetterTop.enabled = true;
        TextLetterBottom.enabled = true;
        TextLetterFront.enabled = false;
        ColliderOpen.enabled = true;
        ColliderClosed.enabled = false;
    }

    public void CloseLetter()
    {
        bIsOpen = false;
        Renderer.sprite = DailyClosedSprite;
        TextLetterTop.enabled = false;
        TextLetterBottom.enabled = false;
        TextLetterFront.enabled = true;
        ColliderOpen.enabled = false;
        ColliderClosed.enabled = true;
    }

    public void UpdateForDay(bool restoreFromSave)
    {
        template_day day = GameManager.instance.GetCurrentDay();
        template_day_task task = day.Template.day.day_task_slot as template_day_task;
        PlayerState player = SaveManager.instance.GetCurrentPlayerState();

        if (!restoreFromSave)
        {
            player.LetterTextIndex = Random.Range(0, DailyLetterFrontText.Count);
            player.LetterOpenSpriteIndex = Random.Range(0, DailyLetterSpritesOpen.Count);
            player.LetterClosedSpriteIndex = Random.Range(0, DailyLetterSpritesClosed.Count);
            if(IsInDrawer())
            {
                HandleDragOutOfDrawer();
            }
            gameObject.transform.localPosition = LetterMarker.localPosition;
        }
        else
        {

        }

        TextLetterFront.text = DailyLetterFrontText[player.LetterTextIndex];
        DailyOpenSprite = DailyLetterSpritesOpen[player.LetterOpenSpriteIndex];
        DailyClosedSprite = DailyLetterSpritesClosed[player.LetterClosedSpriteIndex];

        TextLetterTop.text = task.Template.task.task_description_top;
        TextLetterBottom.text = task.Template.task.task_description_bottom;

        CloseLetter();

    }
}
