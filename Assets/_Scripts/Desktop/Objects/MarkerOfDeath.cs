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

public class MarkerOfDeath : DeskItem, Interactable, Draggable
{
    public static MarkerOfDeath instance;

    private void Awake()
    {
        instance = this;

        OriginalSortOrder = GetComponent<SpriteRenderer>().sortingOrder;
        OriginalZ = gameObject.transform.localPosition.z;
        OriginPosition = gameObject.transform.position;
    }

    bool bIsPickedUp;

    public SpriteRenderer RendererMarker;
    public BoxCollider2D Collider;

    public Sprite SpriteActive;
    public Sprite SpriteInactive;

    public GameObject ActiveOffsetMarker;

    public AudioClip ClipPickUp;
    public AudioClip ClipPutDown;

    public int TimesClicked;

    [SerializeField]
    public float MarkerZOffset = -25;

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    int OriginalSortOrder = 0;
    float OriginalZ = 0;

    public bool bIsCreditsMarker = false;

    

    // Start is called before the first frame update
    void Start()
    {
        RendererMarker.sharedMaterial = new Material(RendererMarker.sharedMaterial);
        RendererMarker.material = new Material(RendererMarker.material);
        RendererMarker.material.SetFloat("_Level", 1.0f);
        //MarkerAppear();
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
            gameObject.transform.position = mousePosition - ActiveOffsetMarker.gameObject.transform.localPosition;
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
        if (FaxMachine.instance.bIsFaxTransmitting)
        {
            return;
        }
        TimesClicked++;
        bIsPickedUp = !bIsPickedUp;
        Cursor.visible = !bIsPickedUp;

        if (bIsPickedUp)
        {
            HandleDragOutOfDrawer();

            RendererMarker.sprite = SpriteActive;
            AudioManager.instance.PlayOneShotEffect(ClipPickUp);
            Collider.enabled = false;

            if (Eraser.instance.IsPickedUp())
            {
                Eraser.instance.Drop();
            }
        }
        else
        {
            Drop();
        }
    }

    public void Drop()
    {
        bIsPickedUp = false;
        Cursor.visible = !bIsPickedUp;
        RendererMarker.sprite = SpriteInactive;
        AudioManager.instance.PlayOneShotEffect(ClipPutDown);
        Collider.enabled = true;
    }

    public bool IsPickedUp()
    {
        return bIsPickedUp;
    }

    public override string GetHoverText()
    {
        return "The Marker of Death. Better pick it up and start marking some files? Welp..";
    }

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
        if (bIsCreditsMarker)
        {
            return;
        }

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
        if (bIsCreditsMarker)
        {
            return;
        }
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

    public void MarkerAppear()
    {
        StartCoroutine(FadeInMarker(1.2f));
    }

    public void MarkerDisappear()
    {
        StartCoroutine(FadeOutMarker(1.2f));
    }

    public IEnumerator FadeInMarker(float duration)
    {
        RendererMarker.material.SetFloat("_Level", 1.0f);


        yield return new WaitForSeconds(1.5f);
        //FadeInParticleSystem.Stop();
        //bFadingIn = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1.0f - Mathf.Clamp(elapsedTime / duration, 0.0f, 1.0f);

            RendererMarker.material.SetFloat("_Level", alpha);


            //TextThanks.color = new Color(TextThanks.color.r, TextThanks.color.g, TextThanks.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            //TextVisitWebsite.color = new Color(TextVisitWebsite.color.r, TextVisitWebsite.color.g, TextVisitWebsite.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            //TextRestart.color = new Color(TextRestart.color.r, TextRestart.color.g, TextRestart.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            yield return null;
        }
        //bFadingIn = false;

    }

    public IEnumerator FadeOutMarker(float duration)
    {
        //bFadingOut = true;

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Clamp(elapsedTime / duration, 0.0f, 1.0f);

            //if (alpha > 0.5f)
            //{
            //    FadeOutParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            //}

            RendererMarker.material.SetFloat("_Level", alpha);


            //TextThanks.color = new Color(TextThanks.color.r, TextThanks.color.g, TextThanks.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            //TextVisitWebsite.color = new Color(TextVisitWebsite.color.r, TextVisitWebsite.color.g, TextVisitWebsite.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            //TextRestart.color = new Color(TextRestart.color.r, TextRestart.color.g, TextRestart.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));

            yield return null;
        }

        if (bIsPickedUp)
        {
            Interact();
        }
        //bFadingOut = false;

        //while (FadeOutParticleSystem.IsAlive())
        //{
        //    yield return null;
        //}
        //Destroy(gameObject);
    }

    //public override void RestoreStatus(DeskItemStatus status)
    //{
    //    ItemStatus = status;

    //    Draggable trolo = GetComponent<Draggable>();

    //    switch (ItemStatus.DrawerStatus)
    //    {
    //        case ELeftOrRight.Left:
    //            trolo.HandleDropIntoDrawer(GrimDesk.instance.DrawerLeft);
    //            break;
    //        case ELeftOrRight.Right:
    //            trolo.HandleDropIntoDrawer(GrimDesk.instance.DrawerRight);
    //            break;
    //        case ELeftOrRight.MAX:
    //            break;
    //    }
    //    if (ItemStatus.Position.x < 9.3 && ItemStatus.Position.x > 9.5)
    //    {
    //        gameObject.transform.localPosition = ItemStatus.Position;

    //    }
    //    else
    //    {

    //    }
    //}
}
