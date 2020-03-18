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

public class PhoneTable : DeskItem, Interactable, Draggable
{
    public static PhoneTable instance;

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<SpriteRenderer, int> OriginalSortOrders = new Dictionary<SpriteRenderer, int>();
    float OriginalZ = 0;
    List<SpriteRenderer> AllRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        instance = this;
        AllRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < AllRenderers.Count; ++i)
        {
            OriginalSortOrders.Add(AllRenderers[i], AllRenderers[i].sortingOrder);
        }
        OriginalZ = gameObject.transform.localPosition.z;
        Phone.instance.ImageNotification.gameObject.SetActive(false);
        OriginPosition = gameObject.transform.position;

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (IsDragging())
        {
            if(Phone.instance.Lerps.Count == 0)
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPoint.z = gameObject.transform.position.z;
                gameObject.transform.position = worldPoint + GrabPosition;
            }
        }
        else
        {
            if(!Phone.instance.bIsFocused && Phone.instance.Lerps.Count == 0)
            {
                if (IsInDrawer())
                {
                    if (!GetDrawer().Collider.OverlapPoint(gameObject.transform.position))
                    {
                        if(GetDrawer().Type  == ELeftOrRight.Left)
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
    }

    public override string GetHoverText()
    {
        return "I have a phone at least... Good for procrastinating and scrolling through Cawker?";
    }

    public void Interact()
    {
        if (FaxMachine.instance.bIsFaxTransmitting)
        {
            return;
        }

        Phone.instance.Interact();
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

        for (int i = 0; i < AllRenderers.Count; ++i)
        {
            AllRenderers[i].maskInteraction = SpriteMaskInteraction.None;
            AllRenderers[i].sortingOrder = OriginalSortOrders[AllRenderers[i]];
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = OriginalZ;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = ELeftOrRight.MAX;
    }

    public void HandleDropIntoDrawer(GrimDeskDrawer hitDrawer)
    {
        gameObject.transform.SetParent(hitDrawer.transform);


        for (int i = 0; i < AllRenderers.Count; ++i)
        {
            AllRenderers[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            AllRenderers[i].sortingOrder = 1;
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = -1;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }

    public override void RestoreStatus(DeskItemStatus status)
    {
        ItemStatus = status;

        Draggable trolo = GetComponent<Draggable>();

        switch (ItemStatus.DrawerStatus)
        {
            case ELeftOrRight.Left:
                trolo.HandleDropIntoDrawer(GrimDesk.instance.DrawerLeft);
                break;
            case ELeftOrRight.Right:
                trolo.HandleDropIntoDrawer(GrimDesk.instance.DrawerRight);
                break;
            case ELeftOrRight.MAX:
                break;
        }
        if(ItemStatus.Position.x < 9.3)
        {
            gameObject.transform.localPosition = ItemStatus.Position;

        }
        else
        {

        }
    }
}
