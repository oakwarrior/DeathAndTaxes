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

public class SalaryCoin : DeskItem, Interactable, Draggable, Ownable
{
    public static SalaryCoin instance;

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<SpriteRenderer, int> OriginalSortOrders = new Dictionary<SpriteRenderer, int>();
    Dictionary<TextMeshPro, int> OriginalSortOrdersText = new Dictionary<TextMeshPro, int>();
    Dictionary<ParticleSystemRenderer, int> OriginalSortOrdersParticles = new Dictionary<ParticleSystemRenderer, int>();
    float OriginalZ = 0;

    int PendingSpins = 0;
    bool bIsSpinning = false;
    float SpinStartY;

    [SerializeField]
    SpriteRenderer CoinRenderer;
    [SerializeField]
    Sprite SpriteHeads;
    [SerializeField]
    Sprite SpriteTails;

    Vector3 BaseScale;
    [SerializeField]
    Vector3 SpinScale;

    [SerializeField]
    List<AudioClip> WailClips = new List<AudioClip>();

    [SerializeField]
    List<AudioClip> FlipClips = new List<AudioClip>();
    [SerializeField]
    List<AudioClip> DropClips = new List<AudioClip>();

    [SerializeField]
    ParticleSystem ParticleWail;

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
        List<ParticleSystemRenderer> particlerenderers = new List<ParticleSystemRenderer>(GetComponentsInChildren<ParticleSystemRenderer>());
        for (int i = 0; i < particlerenderers.Count; ++i)
        {
            OriginalSortOrdersParticles.Add(particlerenderers[i], particlerenderers[i].sortingOrder);
        }
        OriginalZ = gameObject.transform.localPosition.z;
        OriginPosition = gameObject.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        int rando = Random.Range(0, 2);
        if(rando == 0)
        {
            CoinRenderer.sprite = SpriteTails;
        }
        else
        {
            CoinRenderer.sprite = SpriteHeads;
        }
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
        if (bIsSpinning)
        {
            return;
        }
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
        List<ParticleSystemRenderer> particleRenderers = new List<ParticleSystemRenderer>(GetComponentsInChildren<ParticleSystemRenderer>());
        for (int i = 0; i < particleRenderers.Count; ++i)
        {
            particleRenderers[i].sortingOrder = OriginalSortOrdersParticles[particleRenderers[i]];
        }

        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = OriginalZ;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = ELeftOrRight.MAX;
    }

    public IEnumerator StopWailParticleRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        ParticleWail.Stop();
    }

    public void Wail()
    {
        if(WailClips.Count == 0)
        {
            return;
        }
        ParticleWail.Play();
        if(IsInDrawer())
        {
            AudioManager.instance.PlayDrawerEffect(WailClips[Random.Range(0, WailClips.Count)], GetDrawer().Type, 0.3f);
        }
        else
        {
            AudioManager.instance.PlayOneShotEffect(WailClips[Random.Range(0, WailClips.Count)], 0.25f);
        }
        StartCoroutine(StopWailParticleRoutine());
    }





    public void HandleDropIntoDrawer(GrimDeskDrawer hitDrawer)
    {
        if (bIsSpinning)
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
        List<TextMeshPro> textrenderers = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < textrenderers.Count; ++i)
        {
            textrenderers[i].sortingOrder = 1;
        }
        List<ParticleSystemRenderer> particleRenderers = new List<ParticleSystemRenderer>(GetComponentsInChildren<ParticleSystemRenderer>());
        for (int i = 0; i < particleRenderers.Count; ++i)
        {
            particleRenderers[i].sortingOrder = 2;
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = -1;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }

    public override string GetHoverText()
    {
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0:
                return "What is that noise?";
            case 1:
                return "Is my money *wailing*..?";
            case 2:
                return "Shiny!";
            case 3:
                return "Cha-ching!";
            case 4:
                return "Maybe it will stop making noise if I put it in my drawers...";
        }
        return "What is that noise?";
    }

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

        if (ItemStatus.DrawerStatus != ELeftOrRight.MAX)
        {
            HandleDragOutOfDrawer();
        }

        int spins = Random.Range(3, 9); //3, 4, 5, 6, 7, 8
        if (spins % 2 == 0)
        {
            spins++;
        }

        if (FlipClips.Count > 0)
        {
            AudioManager.instance.PlayOneShotEffect(FlipClips[Random.Range(0, FlipClips.Count)]);
        }

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
                if (CoinRenderer.sprite == SpriteHeads)
                {
                    CoinRenderer.sprite = SpriteTails;
                }
                else
                {
                    CoinRenderer.sprite = SpriteHeads;
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
            if(alpha > 0.9f)
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

        renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            renderers[i].sortingOrder = OriginalSortOrders[renderers[i]];
        }
        bPlayedDropSound = false;
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
