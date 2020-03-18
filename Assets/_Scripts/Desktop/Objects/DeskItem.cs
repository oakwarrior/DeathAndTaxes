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

public class DeskItem : MonoBehaviour
{
    [SerializeField]
    public DeskItemStatus ItemStatus;
    [SerializeField]
    ArticyRef ItemTemplateReference;

    template_item ItemTemplate;

    [SerializeField]
    protected Vector3 OriginPosition;

    public GrimDeskDrawer GetDrawer()
    {
        switch (ItemStatus.DrawerStatus)
        {
            case ELeftOrRight.Left:
                return GrimDeskDrawer.instanceLeft;
            case ELeftOrRight.Right:
                return GrimDeskDrawer.instanceRight;
            case ELeftOrRight.MAX:
                return null;
        }
        return null;
    }

    public void CheckAndCorrectOutOfBounds()
    {
        if (!IsInDrawer())
        {
            if (gameObject.transform.position.x < -9.6f ||
                    gameObject.transform.position.x > 9.6f ||
                    gameObject.transform.position.y < -5.4f ||
                    gameObject.transform.position.y > 5.4f)
            {
                gameObject.transform.position = OriginPosition;
            }
        }
    }

    public bool IsInDrawer()
    {
        return ItemStatus.DrawerStatus != ELeftOrRight.MAX;
    }

    public template_item GetItemTemplate()
    {
        if (ItemTemplate == null)
        {
            if (ItemTemplateReference.HasReference)
            {
                ItemTemplate = ItemTemplateReference.GetObject<template_item>();
            }
        }
        return ItemTemplate;
    }

    public virtual string GetHoverText()
    {
        if (GetItemTemplate() != null)
        {
            if (GameManager.instance.bLoreMode)
            {
                return GetItemTemplate().Template.item_data.item_description;
            }
            else
            {
                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        return GetItemTemplate().Template.item_data.item_flavour_text_first;
                    case 1:
                        return GetItemTemplate().Template.item_data.item_flavour_text_second;
                    case 2:
                        return GetItemTemplate().Template.item_data.item_flavour_text_third;
                }
            }
        }
        return "";
    }

    public virtual void RestoreStatus(DeskItemStatus status)
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

        gameObject.transform.localPosition = ItemStatus.Position;
    }

}
