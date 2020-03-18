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
using TMPro;
using UnityEngine;

public class Calendar : DeskItem, Draggable, Interactable, Ownable
{
    public static Calendar instance;

    List<SpriteRenderer> Renderers = new List<SpriteRenderer>();

    private void Awake()
    {
        instance = this;

        Renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < Renderers.Count; ++i)
        {
            OriginalSortOrders.Add(Renderers[i], Renderers[i].sortingOrder);
        }
        for(int i = 0; i < DayMarkers.Count; ++i)
        {
            SpriteRenderer rendo = DayMarkers[i].GetComponentInChildren<SpriteRenderer>();
            if(rendo != null)
            {
                Renderers.Add(rendo);
                if(!OriginalSortOrders.ContainsKey(rendo))
                {
                    OriginalSortOrders.Add(rendo, rendo.sortingOrder);
                }
            }
        }
        List<TextMeshPro> textrenderers = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < textrenderers.Count; ++i)
        {
            OriginalSortOrdersText.Add(textrenderers[i], textrenderers[i].sortingOrder);
        }
        OriginalZ = gameObject.transform.localPosition.z;
        OriginPosition = gameObject.transform.position;
    }

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<SpriteRenderer, int> OriginalSortOrders = new Dictionary<SpriteRenderer, int>();
    Dictionary<TextMeshPro, int> OriginalSortOrdersText = new Dictionary<TextMeshPro, int>();
    float OriginalZ = 0;

    [SerializeField]
    List<GameObject> DayMarkers = new List<GameObject>();

    

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

    public void UpdateForDay()
    {
        for(int i = 0; i < DayMarkers.Count; ++i)
        {
            if(i < SaveManager.instance.GetCurrentPlayerState().GetCurrentDayIndex())
            {
                DayMarkers[i].SetActive(true);
            }
            else
            {
                DayMarkers[i].SetActive(false);
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
        for (int i = 0; i < Renderers.Count; ++i)
        {
            Renderers[i].maskInteraction = SpriteMaskInteraction.None;
            Renderers[i].sortingOrder = OriginalSortOrders[Renderers[i]];
        }
        List<TextMeshPro> textrenderers = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < textrenderers.Count; ++i)
        {
            textrenderers[i].sortingOrder = OriginalSortOrdersText[textrenderers[i]];
        }

        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = OriginalZ;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = ELeftOrRight.MAX;
    }

    public void HandleDropIntoDrawer(GrimDeskDrawer hitDrawer)
    {
        gameObject.transform.SetParent(hitDrawer.transform);

        for (int i = 0; i < Renderers.Count; ++i)
        {
            Renderers[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            Renderers[i].sortingOrder = 1;
        }
        List<TextMeshPro> textrenderers = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < textrenderers.Count; ++i)
        {
            textrenderers[i].sortingOrder = 1;
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = -0.9f;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }

    public void Interact()
    {
        
    }

    public void HandleUsed()
    {

    }

    public void HandleOwnedStatus()
    {
        if (ArticyGlobalVariables.Default.inventory.calendar)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
