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

public class Eraser : DeskItem, Draggable, Interactable, Ownable
{
    public static Eraser instance;

    private void Awake()
    {
        instance = this;

        OriginalSortOrder = GetComponent<SpriteRenderer>().sortingOrder;
        OriginalZ = gameObject.transform.localPosition.z;
        OriginPosition = gameObject.transform.position;

    }

    bool bIsPickedUp;

    public SpriteRenderer RendererEraser;
    public PolygonCollider2D Collider;

    public GameObject SpawnMarker;
    public GameObject ActiveOffsetEraser;

    public AudioClip ClipPickUp;
    public AudioClip ClipPutDown;

    public int TimesClicked;

    [SerializeField]
    public float MarkerZOffset = -25;

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    int OriginalSortOrder = 0;
    float OriginalZ = 0;
    //bool bLocationReset = false;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bIsPickedUp)
        {
            var mousePosition = Input.mousePosition;
            //mousePosition.z = 0.0f;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = MarkerZOffset;
            gameObject.transform.position = mousePosition - ActiveOffsetEraser.gameObject.transform.localPosition;
        }
        else
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
    }

    public void Interact()
    {
        TimesClicked++;
        bIsPickedUp = !bIsPickedUp;
        Cursor.visible = !bIsPickedUp;

        if (bIsPickedUp)
        {
            HandleDragOutOfDrawer();

            AudioManager.instance.PlayOneShotEffect(ClipPickUp);
            Collider.enabled = false;

            if (MarkerOfDeath.instance.IsPickedUp())
            {
                MarkerOfDeath.instance.Drop();
            }
        }
        else
        {
            AudioManager.instance.PlayOneShotEffect(ClipPutDown);
            Collider.enabled = true;
        }
    }

    public void Drop()
    {
        bIsPickedUp = false;
        Cursor.visible = !bIsPickedUp;
        AudioManager.instance.PlayOneShotEffect(ClipPutDown);
        Collider.enabled = true;
    }

    public bool IsPickedUp()
    {
        return bIsPickedUp;
    }

    //public string GetHoverText()
    //{
    //    return "Oops..";
    //}

    public void Hover()
    {

    }

    public void UpdateDragGrabPosition(Vector3 position)
    {
        position.z = gameObject.transform.position.z;
        GrabPosition = gameObject.transform.position - position;
    }

    public bool CanDrag()
    {
        return true;
    }

    public bool IsDragging()
    {
        return bIsDragging;
    }

    public void HandleDragOutOfDrawer()
    {
        gameObject.transform.SetParent(GrimDesk.instance.MasterObjectTransform);
        List<SpriteRenderer> renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            renderers[i].maskInteraction = SpriteMaskInteraction.None;
            renderers[i].sortingOrder = OriginalSortOrder;
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

    public void ToggleDragging(bool drag)
    {
        bIsDragging = drag;

        if (!drag)
        {

        }
        else
        {
            //HandleDragOutOfDrawer();

        }
    }

    public void Unhover()
    {

    }

    public void HandleUsed()
    {
        AudioManager.instance.PlayOneShotEffect(AudioManager.instance.MarkErase);

        ArticyGlobalVariables.Default.inventory.eraser = false;
        Drop();

        gameObject.SetActive(false);

        SaveManager.instance.MarkSavegameDirty();
    }

    public void HandleOwnedStatus()
    {
        if (ArticyGlobalVariables.Default.inventory.eraser)
        {
            gameObject.SetActive(true);
            if (!SaveManager.instance.GetCurrentCarryoverPlayerState().bEraserLocationReset)
            {
                SaveManager.instance.GetCurrentCarryoverPlayerState().bEraserLocationReset = true;
                gameObject.transform.position = SpawnMarker.gameObject.transform.position;
                //bLocationReset = true;
            }
        }
        else
        {
            gameObject.SetActive(false);
            SaveManager.instance.GetCurrentCarryoverPlayerState().bEraserLocationReset = false;
        }
        SaveManager.instance.MarkSavegameDirty();
    }

    public override void RestoreStatus(DeskItemStatus status)
    {
        base.RestoreStatus(status);
    }

}
