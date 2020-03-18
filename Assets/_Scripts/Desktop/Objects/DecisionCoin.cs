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

public class DecisionCoin : DeskItem, Interactable, Draggable, Ownable
{
    public static DecisionCoin instance;

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<SpriteRenderer, int> OriginalSortOrders = new Dictionary<SpriteRenderer, int>();
    Dictionary<TextMeshPro, int> OriginalSortOrdersText = new Dictionary<TextMeshPro, int>();
    float OriginalZ = 0;

    int PendingSpins = 0;
    bool bIsSpinning = false;
    float SpinStartY;

    [SerializeField]
    SpriteRenderer CoinRenderer;
    [SerializeField]
    Sprite SpriteSkull;
    [SerializeField]
    Sprite SpriteAnkh;

    [SerializeField]
    List<AudioClip> FlipClips = new List<AudioClip>();
    [SerializeField]
    List<AudioClip> DropClips = new List<AudioClip>();

    Vector3 BaseScale;
    [SerializeField]
    Vector3 SpinScale = new Vector3(1.3f, 1.3f, 1.3f);

    bool bPlayedDropSound = false;
    

    private void Awake()
    {
        instance = this;

        BaseScale = gameObject.transform.localScale;
        List<SpriteRenderer> renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            OriginalSortOrders.Add(renderers[i], renderers[i].sortingOrder);
        }
        List<TextMeshPro> textrenderers = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < textrenderers.Count; ++i)
        {
            OriginalSortOrdersText.Add(textrenderers[i], textrenderers[i].sortingOrder);
        }
        OriginalZ = gameObject.transform.localPosition.z;
        OriginPosition = gameObject.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bIsSpinning)
        {

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

        List<SpriteRenderer> renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            renderers[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            renderers[i].sortingOrder = 1;
        }
        List<TextMeshPro> textrenderers = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < textrenderers.Count; ++i)
        {
            textrenderers[i].sortingOrder = 1;
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = -1;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }


    //public string GetHoverText()
    //{
    //    if(CoinRenderer.sprite.name.Contains("cactus"))
    //    {
    //        return "FLIP THAT CACTUS LIKE A MANIAC";
    //    }
    //    return "Live or Die?";
    //}

    public void Interact()
    {
        if (FaxMachine.instance.bIsFaxTransmitting)
        {
            return;
        }

        // IT SPIN
        if (bIsSpinning || IsInDrawer())
        {
            return;
        }

        if(FlipClips.Count > 0)
        {
            AudioManager.instance.PlayOneShotEffect(FlipClips[Random.Range(0, FlipClips.Count)]);
        }

        SaveManager.instance.GetCurrentCarryoverPlayerState().AmountOfCoinFlips++;
        SaveManager.instance.MarkSavegameDirty();
        if (ItemStatus.DrawerStatus != ELeftOrRight.MAX)
        {
            HandleDragOutOfDrawer();
        }

        int spins = Random.Range(3, 9); //5, 6, 7, 8, 9, 10

        PendingSpins = spins;
        SpinStartY = gameObject.transform.position.y;

        StartCoroutine(SpinRoutine(0.9f));
    }

    public IEnumerator SpinRoutine(float duration)
    {
        bIsSpinning = true;
        List<SpriteRenderer> renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            renderers[i].sortingOrder = 10;
        }
        bPlayedDropSound = false;
        float elapsedTime = 0;
        float spinnyCounter = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Vector3 newBlarg = transform.eulerAngles;
            float alpha = elapsedTime / duration;
            newBlarg.x = Mathf.Lerp(0, (PendingSpins * 360.0f), alpha) % 360;
            float dif = transform.eulerAngles.x - newBlarg.x;
            spinnyCounter += dif;
            Vector3 pos = gameObject.transform.position;
            if (Mathf.Abs(spinnyCounter) > 90.0f)
            {
                spinnyCounter = 0.0f;
                if (CoinRenderer.sprite == SpriteSkull)
                {
                    CoinRenderer.sprite = SpriteAnkh;
                }
                else
                {
                    CoinRenderer.sprite = SpriteSkull;
                }
            }



            if (alpha < 0.7f)
            {
                pos.y = SpinStartY + Mathf.Lerp(0.0f, 0.9f, ComicPanelElement.MapValue(alpha, 0.0f, 0.7f, 0.0f, 1.0f));
                gameObject.transform.localScale = Vector3.Lerp(BaseScale, SpinScale, ComicPanelElement.MapValue(alpha, 0.0f, 0.7f, 0.0f, 1.0f));
            }
            else
            {
                pos.y = SpinStartY + Mathf.Lerp(0.0f, 0.9f, ComicPanelElement.MapValue(alpha, 0.7f, 1.0f, 1.0f, 0.0f));
                gameObject.transform.localScale = Vector3.Lerp(BaseScale, SpinScale, ComicPanelElement.MapValue(alpha, 0.7f, 1.0f, 1.0f, 0.0f));
            }

            if (alpha > 0.9f)
            {
                if (DropClips.Count > 0 && !bPlayedDropSound)
                {
                    AudioManager.instance.PlayOneShotEffect(DropClips[Random.Range(0, DropClips.Count)]);
                    bPlayedDropSound = true;
                }
            }

            gameObject.transform.position = pos;

            transform.eulerAngles = newBlarg;

            yield return null;
        }
        gameObject.transform.eulerAngles = Vector3.zero;
        bPlayedDropSound = false;
        renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            renderers[i].sortingOrder = OriginalSortOrders[renderers[i]];
        }

        bIsSpinning = false;
    }

    public void HandleUsed()
    {

    }

    public void HandleOwnedStatus()
    {
        if (ArticyGlobalVariables.Default.inventory.coin)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
