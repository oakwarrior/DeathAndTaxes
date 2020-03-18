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
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EChaosThreshold
{
    Critical,
    Dire,
    Good,
    Utopian,
    MAX
}

public class ChaosGlobe : DeskItem, Draggable, Interactable, Ownable
{
    public static ChaosGlobe instance;



    private void Awake()
    {
        instance = this;

        gameObject.transform.localScale = ScaleDesk;
        RotationDesktop = gameObject.transform.rotation;

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

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<SpriteRenderer, int> OriginalSortOrders = new Dictionary<SpriteRenderer, int>();
    Dictionary<TextMeshPro, int> OriginalSortOrdersText = new Dictionary<TextMeshPro, int>();
    Dictionary<ParticleSystemRenderer, int> OriginalSortOrdersParticles = new Dictionary<ParticleSystemRenderer, int>();
    float OriginalZ = 0;



    [SerializeField]
    Vector3 ScaleFocus;
    [SerializeField]
    Vector3 ScaleDesk;

    [SerializeField]
    SpriteRenderer InsideRenderer;
    [SerializeField]
    SpriteRenderer EcologyRenderer;
    [SerializeField]
    SpriteRenderer PeaceRenderer;
    [SerializeField]
    SpriteRenderer ProsperityRenderer;
    [SerializeField]
    SpriteRenderer HealthRenderer;

    [SerializeField]
    SpriteRenderer InsideTargetRenderer;
    [SerializeField]
    SpriteRenderer EcologyTargetRenderer;
    [SerializeField]
    SpriteRenderer PeaceTargetRenderer;
    [SerializeField]
    SpriteRenderer ProsperityTargetRenderer;
    [SerializeField]
    SpriteRenderer HealthTargetRenderer;


    bool bSituationDirty = false;

    [SerializeField]
    List<Sprite> InsideSprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> EcologySprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> PeaceSprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> ProsperitySprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> HealthSprites = new List<Sprite>();

    EPaperworkStatus Status = EPaperworkStatus.DESK;

    [SerializeField]
    StatusParticle ParticleInside;
    [SerializeField]
    StatusParticle ParticleEcology;
    [SerializeField]
    StatusParticle ParticlePeace;
    [SerializeField]
    StatusParticle ParticleProsperity;
    [SerializeField]
    StatusParticle ParticleHealth;

    [SerializeField]
    public float GlobeZOffset = -24;

    Vector3 PositionDesktop;
    Quaternion RotationDesktop;

    float CurrentSlowdownCooldown = 0.0f;
    public int TimesClicked;


    // Start is called before the first frame update
    void Start()
    {

    }

    public float LoopClipFactor = 0.0f;

    Vector3 WorldPointForDrag;
    Vector3 PreviousPos;
    Vector3 DragDirection;

    // Update is called once per frame
    void Update()
    {
        if (bSituationDirty)
        {
            UpdateSituation();
        }
        //ParticleInside.ClampParticles();
        AudioManager.instance.UpdateChaosGlobeLoopVolume(LoopClipFactor);
        if (IsDragging())
        {
            WorldPointForDrag = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            WorldPointForDrag.z = gameObject.transform.position.z;
            PreviousPos = gameObject.transform.position;
            if (Status == EPaperworkStatus.DESK)
            {
                //gameObject.transform.position = PositionDesktop = worldPoint + GrabPosition;
                WorldPointForDrag.z = gameObject.transform.position.z;
                gameObject.transform.position = PositionDesktop = WorldPointForDrag + GrabPosition;
                //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, OriginalZ);

            }
            else if (Status == EPaperworkStatus.FOCUS)
            {
                //PaperworkManager.instance.UpdateFocusPosition(worldPoint + GrabPosition);
                gameObject.transform.position = PositionDesktop = WorldPointForDrag + GrabPosition;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, GlobeZOffset);
            }

            LoopClipFactor = Mathf.Clamp(LoopClipFactor + (gameObject.transform.position - PreviousPos).magnitude * Time.deltaTime * 1.5f, 0.0f, 1.0f);
            DragDirection = (gameObject.transform.position - PreviousPos).normalized;
            DragDirection.z = 0;
            ParticleInside.SpeedUpParticles(Time.deltaTime * (gameObject.transform.position - PreviousPos).magnitude * 80, DragDirection);
            ParticleEcology.SpeedUpParticles(Time.deltaTime * (gameObject.transform.position - PreviousPos).magnitude * 80, DragDirection);
            ParticlePeace.SpeedUpParticles(Time.deltaTime * (gameObject.transform.position - PreviousPos).magnitude * 80, DragDirection);
            ParticleProsperity.SpeedUpParticles(Time.deltaTime * (gameObject.transform.position - PreviousPos).magnitude * 80, DragDirection);
            ParticleHealth.SpeedUpParticles(Time.deltaTime * (gameObject.transform.position - PreviousPos).magnitude * 80, DragDirection);

            CurrentSlowdownCooldown = 3.0f;
        }
        else
        {
            LoopClipFactor = Mathf.Clamp(LoopClipFactor - Time.deltaTime * 0.12f, 0.0f, 1.0f);
            if (CurrentSlowdownCooldown <= 0.0f)
            {
                ParticleInside.SlowDownParticles(Time.deltaTime * -0.01f);
                ParticleInside.FallDownParticles();
            }
            else
            {
                CurrentSlowdownCooldown -= Time.deltaTime;
            }


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

            //ParticleInside.FallDownParticles();
        }
        //UpdateVisualization();
    }

    private void OnEnable()
    {
        MarkSituationDirty();
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
        ParticleInside.gameObject.SetActive(true);
        ParticleEcology.gameObject.SetActive(true);
        ParticlePeace.gameObject.SetActive(true);
        ParticleProsperity.gameObject.SetActive(true);
        ParticleHealth.gameObject.SetActive(true);

        ParticleInside.PlayParticles();
        ParticleEcology.PlayParticles();
        ParticlePeace.PlayParticles();
        ParticleProsperity.PlayParticles();
        ParticleHealth.PlayParticles();

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

    public void HandleDropIntoDrawer(GrimDeskDrawer hitDrawer)
    {
        if (Status == EPaperworkStatus.FOCUS)
        {
            return;
        }

        ParticleInside.gameObject.SetActive(false);
        ParticleEcology.gameObject.SetActive(false);
        ParticlePeace.gameObject.SetActive(false);
        ParticleProsperity.gameObject.SetActive(false);
        ParticleHealth.gameObject.SetActive(false);

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
            particleRenderers[i].sortingOrder = 1;
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = -1.1f;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }

    public IEnumerator SwitchParameterSpriteRoutine(EStat stat, int targetSpriteIndex)
    {
        SpriteRenderer targetRenderer = null;
        SpriteRenderer baseRenderer = null;
        Sprite targetSprite = null;
        switch (stat)
        {
            case EStat.ECOLOGY:
                targetRenderer = EcologyTargetRenderer;
                baseRenderer = EcologyRenderer;
                targetSprite = EcologySprites[targetSpriteIndex];
                break;
            case EStat.HEALTH:
                targetRenderer = HealthTargetRenderer;
                baseRenderer = HealthRenderer;
                targetSprite = HealthSprites[targetSpriteIndex];

                break;
            case EStat.PROSPERITY:
                targetRenderer = ProsperityTargetRenderer;
                baseRenderer = ProsperityRenderer;
                targetSprite = ProsperitySprites[targetSpriteIndex];

                break;
            case EStat.PEACE:
                targetRenderer = PeaceTargetRenderer;
                baseRenderer = PeaceRenderer;
                targetSprite = PeaceSprites[targetSpriteIndex];

                break;
            case EStat.MAX:
                targetRenderer = InsideTargetRenderer;
                baseRenderer = InsideRenderer;
                targetSprite = InsideSprites[targetSpriteIndex];

                break;
        }
        if (targetSprite != baseRenderer.sprite)
        {
            float elapsedTime = 0.0f;
            float duration = 1.0f;
            float alpha = 0.0f;
            Color col = baseRenderer.color;
            targetRenderer.sprite = targetSprite;
            while (elapsedTime < duration)
            {
                elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime, 0.0f, duration);
                alpha = Mathf.Clamp(elapsedTime / duration, 0.0f, 1.0f);

                col.a = 1.0f - alpha;
                baseRenderer.color = col;
                col.a = alpha;
                targetRenderer.color = col;


                yield return null;
            }

            baseRenderer.sprite = targetSprite;
            col.a = 1.0f;
            baseRenderer.color = col;
            col.a = 0.0f;
            targetRenderer.color = col;
        }
        else
        {
            yield return null;
        }
    }

    public void UpdateGlobeParticlesForStat(EStat stat)
    {
        EGoodBadNeutral status = EGoodBadNeutral.Neutral;
        float parameter = 0;
        PlayerState player = SaveManager.instance.GetCurrentPlayerState();
        if (stat == EStat.MAX)
        {
            parameter = (player.GetStat(EStat.ECOLOGY) + player.GetStat(EStat.PEACE) + player.GetStat(EStat.PROSPERITY) + player.GetStat(EStat.HEALTH)) / 4;

            if (parameter > GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Good])
            {
                status = EGoodBadNeutral.Good;
            }
            else if (parameter < GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Dire])
            {
                status = EGoodBadNeutral.Bad;
            }
        }
        else
        {
            parameter = player.GetStat(stat);

            if (parameter > 0)
            {
                status = EGoodBadNeutral.Good;
            }
            else if (parameter < 0)
            {
                status = EGoodBadNeutral.Bad;
            }
        }



        switch (stat)
        {
            case EStat.MAX:
                ParticleInside.SetStatus(status);
                break;
            case EStat.ECOLOGY_DAILY:
                ParticleEcology.SetStatus(status);
                break;
            case EStat.HEALTH_DAILY:
                ParticleHealth.SetStatus(status);
                break;
            case EStat.PROSPERITY_DAILY:
                ParticleProsperity.SetStatus(status);
                break;
            case EStat.PEACE_DAILY:
                ParticlePeace.SetStatus(status);
                break;

        }
    }

    public int GetParameterThresholdIndex(EStat stat, float parameter)
    {
        switch (stat)
        {
            case EStat.LOYALTY:
            {
                float avg = (ArticyGlobalVariables.Default.rep.ecology + ArticyGlobalVariables.Default.rep.peace + ArticyGlobalVariables.Default.rep.prosperity + ArticyGlobalVariables.Default.rep.health) / 4;
                if (parameter < 0)
                {
                    return Mathf.Max(GetParameterThresholdIndex(EStat.MAX, avg) - 1, 0);
                }
                else if (parameter > 0)
                {
                    return Mathf.Min(GetParameterThresholdIndex(EStat.MAX, avg) + 1, 4);
                }
                else
                {
                    return GetParameterThresholdIndex(EStat.MAX, avg);
                }
            }
            case EStat.DEATH_TOTAL:
                break;
            case EStat.DEATH_DAILY:
                break;
            case EStat.SPARE_TOTAL:
                break;
            case EStat.SPARE_DAILY:
                break;
            case EStat.ECOLOGY:
            case EStat.HEALTH:
            case EStat.PROSPERITY:
            case EStat.PEACE:
            {
                if (parameter <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Critical]) // -infinity to -350
                {
                    //shit
                    return 0;
                }
                else if (parameter <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Dire]) // -349 to -100
                {
                    //low
                    return 1;

                }
                else if (parameter <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Good]) // -99 to 100
                {
                    //med
                    return 2;

                }
                else if (parameter <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Utopian]) // 101 to 350
                {
                    //high
                    return 3;
                }
                else // 351 to +infinity
                {
                    //super
                    return 4;
                }
            }
            case EStat.ECOLOGY_DAILY:
            {
                if (parameter < 0)
                {
                    return Mathf.Max(GetParameterThresholdIndex(EStat.ECOLOGY, ArticyGlobalVariables.Default.rep.ecology) - 1, 0);
                }
                else if (parameter > 0)
                {
                    return Mathf.Min(GetParameterThresholdIndex(EStat.ECOLOGY, ArticyGlobalVariables.Default.rep.ecology) + 1, 4);
                }
                else
                {
                    return GetParameterThresholdIndex(EStat.ECOLOGY, ArticyGlobalVariables.Default.rep.ecology);
                }
            }
            case EStat.HEALTH_DAILY:
            {
                if (parameter < 0)
                {
                    return Mathf.Max(GetParameterThresholdIndex(EStat.HEALTH, ArticyGlobalVariables.Default.rep.health) - 1, 0);
                }
                else if (parameter > 0)
                {
                    return Mathf.Min(GetParameterThresholdIndex(EStat.HEALTH, ArticyGlobalVariables.Default.rep.health) + 1, 4);
                }
                else
                {
                    return GetParameterThresholdIndex(EStat.HEALTH, ArticyGlobalVariables.Default.rep.health);
                }
            }
            case EStat.PROSPERITY_DAILY:
            {
                if (parameter < 0)
                {
                    return Mathf.Max(GetParameterThresholdIndex(EStat.PROSPERITY, ArticyGlobalVariables.Default.rep.prosperity) - 1, 0);
                }
                else if (parameter > 0)
                {
                    return Mathf.Min(GetParameterThresholdIndex(EStat.PROSPERITY, ArticyGlobalVariables.Default.rep.prosperity) + 1, 4);
                }
                else
                {
                    return GetParameterThresholdIndex(EStat.PROSPERITY, ArticyGlobalVariables.Default.rep.prosperity);
                }
            }
            case EStat.PEACE_DAILY:
            {
                if (parameter < 0)
                {
                    return Mathf.Max(GetParameterThresholdIndex(EStat.PEACE, ArticyGlobalVariables.Default.rep.peace) - 1, 0);
                }
                else if (parameter > 0)
                {
                    return Mathf.Min(GetParameterThresholdIndex(EStat.PEACE, ArticyGlobalVariables.Default.rep.peace) + 1, 4);
                }
                else
                {
                    return GetParameterThresholdIndex(EStat.PEACE, ArticyGlobalVariables.Default.rep.peace);
                }
            }
            case EStat.MAX:
            {
                if (parameter <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Critical]) // -infinity to -350
                {
                    //shit
                    return 0;
                }
                else if (parameter <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Dire]) // -349 to -100
                {
                    //low
                    return 1;

                }
                else if (parameter <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Good]) // -99 to 100
                {
                    //med
                    return 2;

                }
                else if (parameter <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Utopian]) // 101 to 350
                {
                    //high
                    return 3;
                }
                else // 351 to +infinity
                {
                    //super
                    return 4;
                }
            }
        }

        return 0;

    }

    public void MarkSituationDirty()
    {
        bSituationDirty = true;
    }

    private void UpdateSituation()
    {
        StartCoroutine(SwitchParameterSpriteRoutine(EStat.ECOLOGY, GetParameterThresholdIndex(EStat.ECOLOGY, ArticyGlobalVariables.Default.rep.ecology)));
        StartCoroutine(SwitchParameterSpriteRoutine(EStat.PEACE, GetParameterThresholdIndex(EStat.PEACE, ArticyGlobalVariables.Default.rep.peace)));
        StartCoroutine(SwitchParameterSpriteRoutine(EStat.PROSPERITY, GetParameterThresholdIndex(EStat.PROSPERITY, ArticyGlobalVariables.Default.rep.prosperity)));
        StartCoroutine(SwitchParameterSpriteRoutine(EStat.HEALTH, GetParameterThresholdIndex(EStat.HEALTH, ArticyGlobalVariables.Default.rep.health)));

        //EcologyRenderer.sprite = EcologySprites[];
        //PeaceRenderer.sprite = PeaceSprites[GetParameterThresholdIndex(EStat.PEACE, ArticyGlobalVariables.Default.rep.peace)];
        //ProsperityRenderer.sprite = ProsperitySprites[GetParameterThresholdIndex(EStat.PROSPERITY, ArticyGlobalVariables.Default.rep.prosperity)];
        //HealthRenderer.sprite = HealthSprites[GetParameterThresholdIndex(EStat.HEALTH, ArticyGlobalVariables.Default.rep.health)];

        UpdateGlobeParticlesForStat(EStat.ECOLOGY_DAILY);
        UpdateGlobeParticlesForStat(EStat.PEACE_DAILY);
        UpdateGlobeParticlesForStat(EStat.PROSPERITY_DAILY);
        UpdateGlobeParticlesForStat(EStat.HEALTH_DAILY);

        float average = (ArticyGlobalVariables.Default.rep.ecology + ArticyGlobalVariables.Default.rep.prosperity + ArticyGlobalVariables.Default.rep.peace + ArticyGlobalVariables.Default.rep.health) / 4;

        //InsideRenderer.sprite = InsideSprites[GetParameterThresholdIndex(EStat.MAX, average)];

        StartCoroutine(SwitchParameterSpriteRoutine(EStat.MAX, GetParameterThresholdIndex(EStat.MAX, ArticyGlobalVariables.Default.rep.worst_parameter_value)));
        UpdateGlobeParticlesForStat(EStat.MAX);

        bSituationDirty = false;
    }


    //public string GetHoverText()
    //{
    //    if (GameManager.instance.bLoreMode)
    //    {
    //        return GetItemTemplate().Template.item_data.item_description;
    //    }
    //    else
    //    {
    //        return GetItemTemplate().Template.item_data.item_name;
    //    }
    //    //return "What a marvellous thing...";
    //}

    public void Interact()
    {
        TimesClicked++;

        if (ItemStatus.DrawerStatus != ELeftOrRight.MAX)
        {
            HandleDragOutOfDrawer();
        }
        switch (Status)
        {
            case EPaperworkStatus.DESK:
                FocusGlobe();
                break;
            case EPaperworkStatus.FOCUS:
                UnFocusGlobe();
                break;
            case EPaperworkStatus.FAX:
                //whut
                break;
        }
    }

    public void HandleUsed()
    {

    }

    public void FocusGlobe()
    {

        StartCoroutine(LerpToPosition(PositionDesktop.z, GlobeZOffset, ScaleDesk, ScaleFocus, RotationDesktop, Quaternion.identity, EPaperworkStatus.FOCUS));
        Status = EPaperworkStatus.FOCUS;

        //AudioManager.instance.PlayOneShotEffect(ClipPaperFocus);
    }

    public void UnFocusGlobe()
    {
        StartCoroutine(LerpToPosition(GlobeZOffset, PositionDesktop.z, ScaleFocus, ScaleDesk, Quaternion.identity, RotationDesktop, EPaperworkStatus.DESK));
        Status = EPaperworkStatus.DESK;
        //AudioManager.instance.PlayOneShotEffect(ClipPaperFocus);
    }

    IEnumerator LerpToPosition(float fromZ, float toZ, Vector3 fromScale, Vector3 toScale, Quaternion fromRotation, Quaternion toRotation, EPaperworkStatus status)
    {
        float elapsedTime = 0;
        float duration = 0.4f;
        Vector3 tempPos = gameObject.transform.position;
        while (elapsedTime < 0.4f)
        {
            tempPos.z = Mathf.Lerp(fromZ, toZ, (elapsedTime / duration));
            gameObject.transform.position = tempPos;
            gameObject.transform.localScale = Vector3.Lerp(fromScale, toScale, (elapsedTime / duration));
            gameObject.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, (elapsedTime / duration));
            elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime, 0.0f, 0.4f);
            yield return null;
        }
        tempPos.z = toZ;
        gameObject.transform.position = tempPos;
        gameObject.transform.localScale = toScale;
        gameObject.transform.rotation = toRotation;

    }

    public void HandleOwnedStatus()
    {
        if (ArticyGlobalVariables.Default.inventory.snowglobe)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
