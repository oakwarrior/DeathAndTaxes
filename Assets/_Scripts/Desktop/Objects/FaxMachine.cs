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
using TMPro;
using UnityEngine;

public class FaxMachine : DeskItem, Interactable, Draggable
{
    public static FaxMachine instance;

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

        OriginPosition = gameObject.transform.position;
    }
    public AudioClip ClipSendFax;
    public AudioClip ClipKilledFate;

    [SerializeField]
    AudioClip ClipInteract;

    public Transform ParticleMarkerDeath;
    public Transform ParticleMarkerSpare;
    public Transform ParticleMarkerAppear;

    [SerializeField]
    public TextMeshPro TextDeathCounter;
    [SerializeField]
    public TextMeshPro TextSpareCounter;

    public int TimesClicked;

    public bool bIsFaxTransmitting;

    int DeathCounter = 0;
    int SpareCounter = 0;

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<SpriteRenderer, int> OriginalSortOrders = new Dictionary<SpriteRenderer, int>();
    Dictionary<TextMeshPro, int> OriginalSortOrdersText = new Dictionary<TextMeshPro, int>();
    List<TextMeshPro> TextComponents = new List<TextMeshPro>();
    float OriginalZ = 0;

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

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }

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

    public void NotifySpare()
    {
        SpareCounter++;
        if (SpareCounter < 10)
        {
            TextSpareCounter.text = "0" + SpareCounter.ToString();
        }
        else
        {
            TextSpareCounter.text = SpareCounter.ToString();
        }
    }

    public void NotifyDeath()
    {
        DeathCounter++;
        if (DeathCounter < 10)
        {
            TextDeathCounter.text = "0" + DeathCounter.ToString();

        }
        else
        {
            TextDeathCounter.text = DeathCounter.ToString();
        }
    }

    public void NotifyEraseSpare()
    {
        SpareCounter--;
        if (SpareCounter < 10)
        {
            TextSpareCounter.text = "0" + SpareCounter.ToString();
        }
        else
        {
            TextSpareCounter.text = SpareCounter.ToString();
        }
    }

    public void NotifyEraseDeath()
    {
        DeathCounter--;
        if (DeathCounter < 10)
        {
            TextDeathCounter.text = "0" + DeathCounter.ToString();

        }
        else
        {
            TextDeathCounter.text = DeathCounter.ToString();
        }
    }

    public void NotifyStartDay()
    {
        SpareCounter = 0;
        DeathCounter = 0;
        TextDeathCounter.text = "00";
        TextSpareCounter.text = "00";
    }

    public void StartTransmission(bool endDay)
    {
        //MarkerOfDeath.instance.MarkerDisappear();

        StartCoroutine(TransmissionRoutine(endDay));
        //bIsFaxTransmitting = true;
        //yield return new WaitForSeconds(0.8f);
        //bIsFaxTransmitting = false;
        //if(endDay)
        //{
        //    DesktopManager.instance.FinishDay();
        //}
    }

    public IEnumerator TransmissionRoutine(bool endDay)
    {
        if (bIsFaxTransmitting)
        {
            yield return null;
        }
        else
        {
            if (endDay)
            {
                PaperworkManager.instance.NotifyEndDay();
            }
            bIsFaxTransmitting = true;
            yield return new WaitForSeconds(2.0f);
            MarkerOfDeath.instance.MarkerDisappear();
            yield return new WaitForSeconds(1.25f);
            bIsFaxTransmitting = false;
            if (endDay)
            {
                DesktopManager.instance.FinishDay();
            }

            if (MarkerOfDeath.instance.IsPickedUp())
            {
                MarkerOfDeath.instance.Interact();
            }
        }
    }

    public override string GetHoverText()
    {
        if (PaperworkManager.instance.AreAllMarked())
        {
            return "DEUS FAX MACHINA. I need to use this to end my shift.. why do they still use these..?";
        }
        else
        {
            return "DEUS FAX MACHINA. I still need to mark some of the profiles before I end my shift.";
        }
    }

    public void Interact()
    {
        if (bIsFaxTransmitting)
        {
            return;
        }

        TimesClicked++;

        if (PaperworkManager.instance.AreAllMarked())
        {
            FaxConfirm.instance.ShowComplete();
        }
        else
        {
            FaxConfirm.instance.ShowIncomplete();
        }

        AudioManager.instance.PlayOneShotEffect(ClipInteract);
    }


}
