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
using UnityEngine.UI;
using Articy.Project_Of_Death;
using Articy.Project_Of_Death.GlobalVariables;

public enum EPaperworkStatus
{
    DESK,
    FOCUS,
    FAX,
}

public class Paperwork : DeskItem, Interactable, Draggable
{

    template_profile AssignedProfile;

    public Text TextName;
    public Text TextAge;
    public Text TextPosition;
    public Text TextSituation;
    public Text LabelName;
    public Text LabelAge;
    public Text LabelPosition;
    public Text LabelLive;
    public Text LabelDie;
    public Image ImagePhoto;
    public Image ImageMarkDie;
    public Image ImageMarkLive;
    public Image ImageBackgroundMarkDie;
    public Image ImageBackgroundMarkLive;
    public Image ImageBackground;
    public Image ImageBackgroundBoxName;
    public Image ImageBackgroundBoxAge;
    public Image ImageBackgroundBoxPosition;

    public SpriteRenderer SpriteSinOverlay;
    public SpriteRenderer SpriteSinEco;
    public SpriteRenderer SpriteSinEcoPlus;
    public SpriteRenderer SpriteSinEcoMinus;
    public SpriteRenderer SpriteSinProsperity;
    public SpriteRenderer SpriteSinProsperityPlus;
    public SpriteRenderer SpriteSinProsperityMinus;
    public SpriteRenderer SpriteSinHealth;
    public SpriteRenderer SpriteSinHealthPlus;
    public SpriteRenderer SpriteSinHealthMinus;
    public SpriteRenderer SpriteSinPeace;
    public SpriteRenderer SpriteSinPeacePlus;
    public SpriteRenderer SpriteSinPeaceMinus;
    public Sprite SpritePlus;
    public Sprite SpriteMinus;
    public Sprite SpriteUnknown;

    public PaperworkMark MarkDie;
    public PaperworkMark MarkLive;

    public Texture TextureSkull;
    public Texture TextureSkullWhite;
    public Texture TextureAnkh;

    public Vector3 ScaleDesktop;
    public Vector3 ScaleFocus;
    public Vector3 PositionFocus;

    public Color EdgeColor1;
    public Color EdgeColor2;

    public AudioClip ClipPaperFocus;

    Vector3 PositionDesktop;
    Quaternion RotationDesktop;

    public EPaperworkStatus Status;

    public Material BlackMaterial;
    public Material YellowMaterial;

    public GameObject FaxTraceMarker;

    [SerializeField]
    public EPaperworkMarkType MarkStatus = EPaperworkMarkType.Unmarked;

    public int numOfClicks = 0;

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<Canvas, int> OriginalSortOrders = new Dictionary<Canvas, int>();
    Dictionary<SpriteRenderer, int> OriginalSortOrdersSprite = new Dictionary<SpriteRenderer, int>();
    float OriginalZ = 0;

    bool bFadingOut = false;
    bool bFadingIn = false;

    public bool bIsCreditsProfile = false;

    [SerializeField]
    public float PaperworkZOffset = -24;

    public ParticleAttraction ParticleTemplate;

    ParticleAttraction FadeInParticle;
    ParticleAttraction FadeOutParticle;
    ParticleSystem FadeInParticleSystem;
    ParticleSystem FadeOutParticleSystem;

    [SerializeField]
    Line TemplateLine;

    [SerializeField]
    Transform LinesParent;

    [SerializeField]
    PolygonCollider2D ColliderPaperwork;
    [SerializeField]
    BoxCollider2D ColliderMarkLive;
    [SerializeField]
    BoxCollider2D ColliderMarkDie;


    List<Line> AllLines = new List<Line>();
    Line CurrentLine;

    bool markedForDestroy = false;

    int OriginalSortOrder = 7;

    public Color SubplotProfileBackgroundColor = new Color(0.7960784f, 0.1882353f, 0.1882353f);

    //bool bHasBeenDoomedBefore = false;
    //bool bHasBeenSparedBefore = false;

    private void Awake()
    {
        List<Canvas> renderers = new List<Canvas>(GetComponentsInChildren<Canvas>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            OriginalSortOrders.Add(renderers[i], renderers[i].sortingOrder);
        }
        List<SpriteRenderer> spriterenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < spriterenderers.Count; ++i)
        {
            OriginalSortOrdersSprite.Add(spriterenderers[i], spriterenderers[i].sortingOrder);
        }
        OriginalZ = gameObject.transform.localPosition.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        TextName.material = new Material(YellowMaterial);
        TextAge.material = new Material(YellowMaterial);
        TextPosition.material = new Material(YellowMaterial);
        TextSituation.material = new Material(YellowMaterial);
        LabelName.material = new Material(YellowMaterial);
        LabelAge.material = new Material(YellowMaterial);
        LabelPosition.material = new Material(YellowMaterial);
        LabelLive.material = new Material(YellowMaterial);
        LabelDie.material = new Material(YellowMaterial);
        ImagePhoto.material = new Material(YellowMaterial);
        ImageMarkDie.material = new Material(ImageMarkDie.material);
        ImageMarkLive.material = new Material(ImageMarkLive.material);
        //ImageBackground.material = new Material(YellowMaterial);
        ImageBackgroundMarkDie.material = new Material(YellowMaterial);
        ImageBackgroundMarkLive.material = new Material(YellowMaterial);
        ImageBackgroundBoxName.material = new Material(YellowMaterial);
        ImageBackgroundBoxAge.material = new Material(YellowMaterial);
        ImageBackgroundBoxPosition.material = new Material(YellowMaterial);
        SpriteSinOverlay.material = new Material(YellowMaterial);
        SpriteSinEco.material = new Material(YellowMaterial);
        SpriteSinEcoPlus.material = new Material(YellowMaterial);
        SpriteSinEcoMinus.material = new Material(YellowMaterial);
        SpriteSinProsperity.material = new Material(YellowMaterial);
        SpriteSinProsperityPlus.material = new Material(YellowMaterial);
        SpriteSinProsperityMinus.material = new Material(YellowMaterial);
        SpriteSinHealth.material = new Material(YellowMaterial);
        SpriteSinHealthPlus.material = new Material(YellowMaterial);
        SpriteSinHealthMinus.material = new Material(YellowMaterial);
        SpriteSinPeace.material = new Material(YellowMaterial);
        SpriteSinPeacePlus.material = new Material(YellowMaterial);
        SpriteSinPeaceMinus.material = new Material(YellowMaterial);

        UnHoverMark();
    }

    public void StartFadeInParticle()
    {
        if (FadeInParticle == null)
        {
            FadeInParticle = Instantiate(ParticleTemplate);
            FadeInParticle.transform.position = Vector3.zero;
            FadeInParticle.gameObject.transform.SetParent(FaxMachine.instance.ParticleMarkerAppear, false);
            FadeInParticle.Target = gameObject.transform;
            FadeInParticleSystem = FadeInParticle.GetComponent<ParticleSystem>();
            FadeInParticleSystem.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", TextureSkullWhite);
            //FadeInParticleSystem.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", new Color(1.0f, 0.9698f, 0.7843f, 0.75f));
        }

        if (FadeOutParticle == null)
        {
            FadeOutParticle = Instantiate(ParticleTemplate);
            FadeOutParticle.transform.position = Vector3.zero;
            FadeOutParticle.gameObject.transform.SetParent(gameObject.transform, false);
            //FadeOutParticle.Target = FaxMachine.instance.gameObject.transform;
            FadeOutParticleSystem = FadeOutParticle.GetComponent<ParticleSystem>();
        }


        FadeInParticleSystem.Play();
        StartCoroutine(FadeInPaperwork(3.6f));
    }

    public void OnEnable()
    {

        StartFadeInParticle();
    }

    public void StartFadeOutParticle()
    {
        FadeOutParticleSystem.Play();

        if (MarkStatus == EPaperworkMarkType.Die)
        {
            if (!bIsCreditsProfile)
            {
                FadeOutParticleSystem.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", TextureSkull);
                FadeOutParticle.Target = FaxMachine.instance.ParticleMarkerDeath;
            }
        }
        if (MarkStatus == EPaperworkMarkType.Live)
        {
            FadeOutParticleSystem.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", TextureAnkh);
            if (!bIsCreditsProfile)
            {
                FadeOutParticle.Target = FaxMachine.instance.ParticleMarkerSpare;
            }
        }

        StartCoroutine(FadeOutPaperwork(3.6f));
    }

    float DraggedDistance = 0.0f;
    Vector3 PreviousDragPoint;
    // Update is called once per frame
    void Update()
    {
        if (IsInDrawer())
        {
            if (gameObject.transform.localPosition.z != -1)
            {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -1);
            }
        }
        else
        {
            if (Status == EPaperworkStatus.FOCUS)
            {
                if (gameObject.transform.position.z != GetZOffset() && LerpRoutine == null)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, GetZOffset());
                }
            }
            else
            {
                if (gameObject.transform.position.z != OriginPosition.z && LerpRoutine == null)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, OriginPosition.z);
                }
            }
        }

        if (IsDragging())
        {
            if (MarkerOfDeath.instance.IsPickedUp() && Status == EPaperworkStatus.FOCUS)
            {
                if (CurrentLine != null)
                {
                    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPoint.z = gameObject.transform.position.z;
                    Vector3 currentPoint = worldPoint;

                    if (ColliderMarkDie.OverlapPoint(worldPoint))
                    {
                        if (Status == EPaperworkStatus.FOCUS && MarkStatus == EPaperworkMarkType.Unmarked)
                        {
                            MarkDie.Interact();
                        }
                        ToggleDragging(false);
                    }
                    else if (ColliderMarkLive.OverlapPoint(worldPoint))
                    {
                        if (Status == EPaperworkStatus.FOCUS && MarkStatus == EPaperworkMarkType.Unmarked)
                        {
                            MarkLive.Interact();
                        }
                        ToggleDragging(false);
                    }
                    else if (ColliderPaperwork.OverlapPoint(worldPoint))
                    {
                        DraggedDistance += (gameObject.transform.position - currentPoint).magnitude;
                        if (DraggedDistance > 0.1f)
                        {
                            CurrentLine.AddPoint(LinesParent.transform.InverseTransformPoint(currentPoint));
                            DraggedDistance = 0.0f;
                        }
                    }
                    else
                    {
                        ToggleDragging(false);
                    }
                }


            }
            else
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPoint.z = gameObject.transform.position.z;
                if (Status == EPaperworkStatus.DESK)
                {
                    SetPosition(PositionDesktop = worldPoint + GrabPosition);
                    if (IsInDrawer())
                    {
                        SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, worldPoint.z));
                    }
                    else
                    {
                        SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, OriginalZ));
                    }
                }
                else if (Status == EPaperworkStatus.FOCUS)
                {
                    //PaperworkManager.instance.UpdateFocusPosition(worldPoint + GrabPosition);
                    SetPosition(worldPoint + GrabPosition);
                    SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, GetZOffset()));
                    //gameObject.transform.position = worldPoint + GrabPosition;
                    //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, GetZOffset());
                }
            }
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

    public void SetPosition(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    public void UpdateDragGrabPosition(Vector3 position)
    {
        position.z = gameObject.transform.position.z;
        GrabPosition = gameObject.transform.position - position;
    }

    public void HandleDragOutOfDrawer()
    {
        if (bIsCreditsProfile || Status == EPaperworkStatus.FOCUS || MarkerOfDeath.instance.IsPickedUp())
        {
            return;
        }

        gameObject.transform.SetParent(GrimDesk.instance.MasterObjectTransform);
        List<Canvas> renderers = new List<Canvas>(GetComponentsInChildren<Canvas>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            //renderers[i].mask = SpriteMaskInteraction.None;
            renderers[i].sortingOrder = OriginalSortOrders[renderers[i]];
        }
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < spriteRenderers.Count; ++i)
        {
            spriteRenderers[i].sortingOrder = OriginalSortOrdersSprite[spriteRenderers[i]];
        }
        for (int i = 0; i < AllLines.Count; ++i)
        {
            AllLines[i].LineRenderer.sortingOrder = 7;
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = OriginalZ;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = ELeftOrRight.MAX;
    }

    public void HandleDropIntoDrawer(GrimDeskDrawer hitDrawer)
    {
        if (bIsCreditsProfile || Status == EPaperworkStatus.FOCUS || MarkerOfDeath.instance.IsPickedUp())
        {
            return;
        }
        gameObject.transform.SetParent(hitDrawer.transform);

        List<Canvas> renderers = new List<Canvas>(GetComponentsInChildren<Canvas>());
        for (int i = 0; i < renderers.Count; ++i)
        {
            //renderers[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            renderers[i].sortingOrder = 1;
        }
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < spriteRenderers.Count; ++i)
        {
            spriteRenderers[i].sortingOrder = 2;
        }
        for (int i = 0; i < AllLines.Count; ++i)
        {
            AllLines[i].LineRenderer.sortingOrder = 1;
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = -1;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }

    public void Hover()
    {

    }



    public bool CanDrag()
    {
        return true;
        //return !MarkerOfDeath.instance.IsPickedUp();
    }

    public bool IsDragging()
    {
        return bIsDragging;
    }

    public void ToggleDragging(bool drag)
    {
        bIsDragging = drag;
        if (bIsDragging)
        {
            if (MarkerOfDeath.instance.IsPickedUp() && Status == EPaperworkStatus.FOCUS)
            {
                CurrentLine = Instantiate(TemplateLine);
                CurrentLine.transform.SetParent(LinesParent);
                CurrentLine.LineRenderer.material = new Material(BlackMaterial);
                CurrentLine.LineRenderer.material.SetTexture("_MainTex", null);
                CurrentLine.LineRenderer.material.SetFloat("_Level", 0.0f);
                CurrentLine.LineRenderer.material.SetColor("_Color", Color.black);
                if (IsInDrawer())
                {
                    CurrentLine.LineRenderer.sortingOrder = 1;
                }
                if (Status == EPaperworkStatus.FOCUS)
                {
                    CurrentLine.LineRenderer.startWidth = 0.2f;
                    CurrentLine.LineRenderer.endWidth = 0.2f;
                }
                else
                {
                    CurrentLine.LineRenderer.startWidth = 0.083f;
                    CurrentLine.LineRenderer.endWidth = 0.083f;
                }
                CurrentLine.transform.localPosition = Vector3.zero;
                CurrentLine.transform.localScale = Vector3.one;

            }
        }
        else
        {
            if (CurrentLine != null)
            {
                AllLines.Add(CurrentLine);
                CurrentLine = null;
            }
        }
    }

    public void Unhover()
    {

    }

    int OrderIndex = 0;
    public bool bFateAttentionSpare = false;

    public void InitFromProfile(template_profile profile, Vector3 initialPosition, Quaternion initialRotation, bool restoreFromSave, int orderIndex, bool fateAttention)
    {
        AssignedProfile = profile;
        OrderIndex = orderIndex;
        OriginPosition = initialPosition;
        bFateAttentionSpare = fateAttention;

        TextName.text = AssignedProfile.Template.profile_basic_data.profile_name.Replace("[SPAWN_COUNTER]", SaveManager.instance.GetCurrentPlayerState().GetSpawnCounter().ToString());
        TextAge.text = AssignedProfile.Template.profile_basic_data.profile_age_value;
        TextPosition.text = AssignedProfile.Template.profile_basic_data.profile_job;
        TextSituation.text = AssignedProfile.Template.profile_basic_data.profile_bio;
        if (bFateAttentionSpare)
        {
            TextSituation.text += " Grim. I am watching you. You should mark this profile to *live*. -Fate";
        }
        if (AssignedProfile.PreviewImage != null && AssignedProfile.PreviewImage.Asset != null)
        {
            if (GetProfile() == GameManager.instance.GrimReaperPaperworkProfile)
            {
                ImagePhoto.sprite = GameManager.instance.GrimReaperProfileAvatar;
            }
            else
            {
                ImagePhoto.sprite = AssignedProfile.PreviewImage.Asset.LoadAssetAsSprite();
            }
        }
        gameObject.transform.SetParent(GrimDesk.instance.MasterObjectTransform);
        gameObject.transform.position = PositionDesktop = initialPosition;
        gameObject.transform.rotation = RotationDesktop = initialRotation;
        gameObject.transform.localScale = ScaleDesktop;

        OriginalZ = gameObject.transform.localPosition.z;

        Status = EPaperworkStatus.DESK;


        MarkDie.NotifyUnFocused();
        MarkLive.NotifyUnFocused();

        if (restoreFromSave)
        {
            if (GetProfile() != GameManager.instance.GrimReaperPaperworkProfile)
            {
                switch (SaveManager.instance.GetCurrentPlayerState().GetProfileMarkStatusByID(profile.Id))
                {
                    case EPaperworkMarkType.Live:
                        MarkPaperworkLive();
                        FaxMachine.instance.NotifySpare();
                        MarkLive.ToggleMarkIcon(true);
                        ImageMarkLive.enabled = true;
                        MarkStatus = EPaperworkMarkType.Live;
                        break;
                    case EPaperworkMarkType.Die:
                        MarkPaperworkDie();
                        FaxMachine.instance.NotifyDeath();
                        MarkDie.ToggleMarkIcon(true);
                        ImageMarkDie.enabled = true;
                        MarkStatus = EPaperworkMarkType.Die;
                        break;
                    case EPaperworkMarkType.Unmarked:
                        break;
                }
            }
        }
        else
        {
            MarkStatus = EPaperworkMarkType.Unmarked;

            ImageMarkLive.enabled = false;
            ImageMarkDie.enabled = false;
        }
        SpriteSinOverlay.material = new Material(BlackMaterial);
        SpriteSinEco.material = new Material(BlackMaterial);
        SpriteSinEcoPlus.material = new Material(BlackMaterial);
        SpriteSinEcoMinus.material = new Material(BlackMaterial);
        SpriteSinProsperity.material = new Material(BlackMaterial);
        SpriteSinProsperityPlus.material = new Material(BlackMaterial);
        SpriteSinProsperityMinus.material = new Material(BlackMaterial);
        SpriteSinHealth.material = new Material(BlackMaterial);
        SpriteSinHealthPlus.material = new Material(BlackMaterial);
        SpriteSinHealthMinus.material = new Material(BlackMaterial);
        SpriteSinPeace.material = new Material(BlackMaterial);
        SpriteSinPeacePlus.material = new Material(BlackMaterial);
        SpriteSinPeaceMinus.material = new Material(BlackMaterial);

        ImageBackground.material = new Material(YellowMaterial);
        if (ProfileManager.instance.SubplotProfiles.Contains(GetProfile()))
        {
            ImageBackground.material.SetColor("_Color", SubplotProfileBackgroundColor);
        }

        //if (SaveManager.instance.GetCurrentCarryoverPlayerState().HasProfileBeenDoomedBefore(GetProfile().Id))
        //{
        //    bHasBeenDoomedBefore = true;
        //}
        //if (SaveManager.instance.GetCurrentCarryoverPlayerState().HasProfileBeenSparedBefore(GetProfile().Id))
        //{
        //    bHasBeenSparedBefore = true;
        //}
    }

    public void Interact()
    {
        if (bFadingOut)
        {
            return;
        }
        if (ItemStatus.DrawerStatus != ELeftOrRight.MAX)
        {
            HandleDragOutOfDrawer();
        }

        if (Eraser.instance.IsPickedUp())
        {
            EraseLines();
        }
        else
        {
            switch (Status)
            {
                case EPaperworkStatus.DESK:
                    FocusPaperwork();
                    //PaperworkManager.instance.NotifyPaperworkFocused(this);
                    break;
                case EPaperworkStatus.FOCUS:
                    UnFocusPaperwork();
                    //PaperworkManager.instance.NotifyPaperworkUnFocused();
                    break;
                case EPaperworkStatus.FAX:
                    break;
            }
        }

    }

    public void MarkPaperworkLive()
    {
        //ImageMarkLive.enabled = true;
        MarkStatus = EPaperworkMarkType.Live;

        AssignedProfile.Template.profile_basic_data.profile_marked_for_death = false;
        numOfClicks++;
    }

    public void MarkPaperworkDie()
    {
        //ImageMarkDie.enabled = true;
        MarkStatus = EPaperworkMarkType.Die;

        AssignedProfile.Template.profile_basic_data.profile_marked_for_death = true;
        numOfClicks++;
        if (GetProfile() == GameManager.instance.GrimReaperPaperworkProfile)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().bDarwinAward = true;
            SaveManager.instance.ForceSave();
            GameManager.instance.RestartGame();
        }
    }

    public bool IsMarked()
    {
        return MarkStatus != EPaperworkMarkType.Unmarked;
    }

    public void UnMarkPaperwork()
    {
        ImageMarkLive.enabled = false;
        ImageMarkDie.enabled = false;

        AssignedProfile.Template.profile_basic_data.profile_marked_for_death = false;
        numOfClicks++;
    }

    public float GetZOffset()
    {
        return PaperworkZOffset - OrderIndex;
    }

    private void OnDisable()
    {
        if (LerpRoutine != null)
        {
            StopCoroutine(LerpRoutine);
            LerpRoutine = null;
        }
    }

    Coroutine LerpRoutine = null;

    public void FocusPaperwork()
    {
        if (LerpRoutine != null)
        {
            StopCoroutine(LerpRoutine);
        }
        LerpRoutine = StartCoroutine(LerpToPosition(PositionDesktop.z, GetZOffset(), ScaleDesktop, ScaleFocus, RotationDesktop, Quaternion.identity, EPaperworkStatus.FOCUS, 0.083f, 0.2f));
        Status = EPaperworkStatus.FOCUS;

        AudioManager.instance.PlayOneShotEffect(ClipPaperFocus);
    }

    public void UnFocusPaperwork()
    {
        if (LerpRoutine != null)
        {
            StopCoroutine(LerpRoutine);
        }
        LerpRoutine = StartCoroutine(LerpToPosition(GetZOffset(), PositionDesktop.z, ScaleFocus, ScaleDesktop, Quaternion.identity, RotationDesktop, EPaperworkStatus.DESK, 0.2f, 0.083f));
        Status = EPaperworkStatus.DESK;
        AudioManager.instance.PlayOneShotEffect(ClipPaperFocus);
    }

    IEnumerator LerpToPosition(float fromZ, float toZ, Vector3 fromScale, Vector3 toScale, Quaternion fromRotation, Quaternion toRotation, EPaperworkStatus status, float fromLineWidth, float toLineWidth)
    {
        float elapsedTime = 0;
        float duration = 0.4f;
        float alpha = 0.0f;
        Vector3 tempPos = gameObject.transform.position;
        while (elapsedTime < 0.4f)
        {
            elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime, 0.0f, 0.4f);

            alpha = (elapsedTime / duration);
            tempPos.z = Mathf.Lerp(fromZ, toZ, alpha);
            gameObject.transform.position = tempPos;
            gameObject.transform.localScale = Vector3.Lerp(fromScale, toScale, alpha);
            gameObject.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, alpha);

            for (int i = 0; i < AllLines.Count; ++i)
            {
                AllLines[i].LineRenderer.startWidth = Mathf.Lerp(fromLineWidth, toLineWidth, alpha);
                AllLines[i].LineRenderer.endWidth = Mathf.Lerp(fromLineWidth, toLineWidth, alpha);
            }

            yield return null;
        }
        tempPos.z = toZ;
        gameObject.transform.position = tempPos;
        gameObject.transform.localScale = toScale;
        gameObject.transform.rotation = toRotation;

        switch (status)
        {
            case EPaperworkStatus.DESK:
                MarkDie.NotifyUnFocused();
                MarkLive.NotifyUnFocused();
                break;
            case EPaperworkStatus.FOCUS:
                MarkDie.NotifyFocused();
                MarkLive.NotifyFocused();
                break;
            case EPaperworkStatus.FAX:
                break;
        }

        LerpRoutine = null;
    }

    public void HoverMark(EPaperworkMarkType type)
    {
        switch (type)
        {
            case EPaperworkMarkType.Die:
                if (SaveManager.instance.GetCurrentCarryoverPlayerState().HasProfileBeenDoomedBefore(GetProfile().Id) || SaveManager.instance.GetCurrentCarryoverPlayerState().HasProfileBeenSparedBefore(GetProfile().Id) && ArticyGlobalVariables.Default.inventory.sin_bulb)
                {
                    SpriteSinEcoPlus.sprite = SpritePlus;
                    SpriteSinEcoMinus.sprite = SpriteMinus;
                    SpriteSinProsperityPlus.sprite = SpritePlus;
                    SpriteSinProsperityMinus.sprite = SpriteMinus;
                    SpriteSinHealthPlus.sprite = SpritePlus;
                    SpriteSinHealthMinus.sprite = SpriteMinus;
                    SpriteSinPeacePlus.sprite = SpritePlus;
                    SpriteSinPeaceMinus.sprite = SpriteMinus;

                    if (GetProfile().Template.profile_death_data.profile_death_ecology_value > 0)
                    {
                        SpriteSinEcoPlus.gameObject.SetActive(true);
                        SpriteSinEcoMinus.gameObject.SetActive(false);
                    }
                    else if (GetProfile().Template.profile_death_data.profile_death_ecology_value < 0)
                    {
                        SpriteSinEcoPlus.gameObject.SetActive(false);
                        SpriteSinEcoMinus.gameObject.SetActive(true);
                    }
                    else
                    {
                        SpriteSinEcoPlus.gameObject.SetActive(false);
                        SpriteSinEcoMinus.gameObject.SetActive(false);
                    }

                    if (GetProfile().Template.profile_death_data.profile_death_peace_value > 0)
                    {
                        SpriteSinPeacePlus.gameObject.SetActive(true);
                        SpriteSinPeaceMinus.gameObject.SetActive(false);
                    }
                    else if (GetProfile().Template.profile_death_data.profile_death_peace_value < 0)
                    {
                        SpriteSinPeacePlus.gameObject.SetActive(false);
                        SpriteSinPeaceMinus.gameObject.SetActive(true);
                    }
                    else
                    {
                        SpriteSinPeacePlus.gameObject.SetActive(false);
                        SpriteSinPeaceMinus.gameObject.SetActive(false);
                    }

                    if (GetProfile().Template.profile_death_data.profile_death_healthcare_value > 0)
                    {
                        SpriteSinHealthPlus.gameObject.SetActive(true);
                        SpriteSinHealthMinus.gameObject.SetActive(false);
                    }
                    else if (GetProfile().Template.profile_death_data.profile_death_healthcare_value < 0)
                    {
                        SpriteSinHealthPlus.gameObject.SetActive(false);
                        SpriteSinHealthMinus.gameObject.SetActive(true);
                    }
                    else
                    {
                        SpriteSinHealthPlus.gameObject.SetActive(false);
                        SpriteSinHealthMinus.gameObject.SetActive(false);
                    }

                    if (GetProfile().Template.profile_death_data.profile_death_prosperity_value > 0)
                    {
                        SpriteSinProsperityPlus.gameObject.SetActive(true);
                        SpriteSinProsperityMinus.gameObject.SetActive(false);
                    }
                    else if (GetProfile().Template.profile_death_data.profile_death_prosperity_value < 0)
                    {
                        SpriteSinProsperityPlus.gameObject.SetActive(false);
                        SpriteSinProsperityMinus.gameObject.SetActive(true);
                    }
                    else
                    {
                        SpriteSinProsperityPlus.gameObject.SetActive(false);
                        SpriteSinProsperityMinus.gameObject.SetActive(false);
                    }
                }
                else
                {
                    SpriteSinEcoPlus.gameObject.SetActive(true);
                    SpriteSinEcoMinus.gameObject.SetActive(true);
                    SpriteSinProsperityPlus.gameObject.SetActive(true);
                    SpriteSinProsperityMinus.gameObject.SetActive(true);
                    SpriteSinHealthPlus.gameObject.SetActive(true);
                    SpriteSinHealthMinus.gameObject.SetActive(true);
                    SpriteSinPeacePlus.gameObject.SetActive(true);
                    SpriteSinPeaceMinus.gameObject.SetActive(true);

                    SpriteSinEcoPlus.sprite = SpriteUnknown;
                    SpriteSinEcoMinus.sprite = SpriteUnknown;
                    SpriteSinProsperityPlus.sprite = SpriteUnknown;
                    SpriteSinProsperityMinus.sprite = SpriteUnknown;
                    SpriteSinHealthPlus.sprite = SpriteUnknown;
                    SpriteSinHealthMinus.sprite = SpriteUnknown;
                    SpriteSinPeacePlus.sprite = SpriteUnknown;
                    SpriteSinPeaceMinus.sprite = SpriteUnknown;
                }


                break;
            case EPaperworkMarkType.Live:
                if (SaveManager.instance.GetCurrentCarryoverPlayerState().HasProfileBeenSparedBefore(GetProfile().Id) || SaveManager.instance.GetCurrentCarryoverPlayerState().HasProfileBeenDoomedBefore(GetProfile().Id) && ArticyGlobalVariables.Default.inventory.sin_bulb)
                {
                    SpriteSinEcoPlus.sprite = SpritePlus;
                    SpriteSinEcoMinus.sprite = SpriteMinus;
                    SpriteSinProsperityPlus.sprite = SpritePlus;
                    SpriteSinProsperityMinus.sprite = SpriteMinus;
                    SpriteSinHealthPlus.sprite = SpritePlus;
                    SpriteSinHealthMinus.sprite = SpriteMinus;
                    SpriteSinPeacePlus.sprite = SpritePlus;
                    SpriteSinPeaceMinus.sprite = SpriteMinus;

                    if (GetProfile().Template.profile_spare_data.profile_spare_ecology_value > 0)
                    {
                        SpriteSinEcoPlus.gameObject.SetActive(true);
                        SpriteSinEcoMinus.gameObject.SetActive(false);
                    }
                    else if (GetProfile().Template.profile_spare_data.profile_spare_ecology_value < 0)
                    {
                        SpriteSinEcoPlus.gameObject.SetActive(false);
                        SpriteSinEcoMinus.gameObject.SetActive(true);
                    }
                    else
                    {
                        SpriteSinEcoPlus.gameObject.SetActive(false);
                        SpriteSinEcoMinus.gameObject.SetActive(false);
                    }

                    if (GetProfile().Template.profile_spare_data.profile_spare_peace_value > 0)
                    {
                        SpriteSinPeacePlus.gameObject.SetActive(true);
                        SpriteSinPeaceMinus.gameObject.SetActive(false);
                    }
                    else if (GetProfile().Template.profile_spare_data.profile_spare_peace_value < 0)
                    {
                        SpriteSinPeacePlus.gameObject.SetActive(false);
                        SpriteSinPeaceMinus.gameObject.SetActive(true);
                    }
                    else
                    {
                        SpriteSinPeacePlus.gameObject.SetActive(false);
                        SpriteSinPeaceMinus.gameObject.SetActive(false);
                    }

                    if (GetProfile().Template.profile_spare_data.profile_spare_healthcare_value > 0)
                    {
                        SpriteSinHealthPlus.gameObject.SetActive(true);
                        SpriteSinHealthMinus.gameObject.SetActive(false);
                    }
                    else if (GetProfile().Template.profile_spare_data.profile_spare_healthcare_value < 0)
                    {
                        SpriteSinHealthPlus.gameObject.SetActive(false);
                        SpriteSinHealthMinus.gameObject.SetActive(true);
                    }
                    else
                    {
                        SpriteSinHealthPlus.gameObject.SetActive(false);
                        SpriteSinHealthMinus.gameObject.SetActive(false);
                    }

                    if (GetProfile().Template.profile_spare_data.profile_spare_prosperity_value > 0)
                    {
                        SpriteSinProsperityPlus.gameObject.SetActive(true);
                        SpriteSinProsperityMinus.gameObject.SetActive(false);
                    }
                    else if (GetProfile().Template.profile_spare_data.profile_spare_prosperity_value < 0)
                    {
                        SpriteSinProsperityPlus.gameObject.SetActive(false);
                        SpriteSinProsperityMinus.gameObject.SetActive(true);
                    }
                    else
                    {
                        SpriteSinProsperityPlus.gameObject.SetActive(false);
                        SpriteSinProsperityMinus.gameObject.SetActive(false);
                    }
                }
                else
                {
                    SpriteSinEcoPlus.gameObject.SetActive(true);
                    SpriteSinEcoMinus.gameObject.SetActive(true);
                    SpriteSinProsperityPlus.gameObject.SetActive(true);
                    SpriteSinProsperityMinus.gameObject.SetActive(true);
                    SpriteSinHealthPlus.gameObject.SetActive(true);
                    SpriteSinHealthMinus.gameObject.SetActive(true);
                    SpriteSinPeacePlus.gameObject.SetActive(true);
                    SpriteSinPeaceMinus.gameObject.SetActive(true);

                    SpriteSinEcoPlus.sprite = SpriteUnknown;
                    SpriteSinEcoMinus.sprite = SpriteUnknown;
                    SpriteSinProsperityPlus.sprite = SpriteUnknown;
                    SpriteSinProsperityMinus.sprite = SpriteUnknown;
                    SpriteSinHealthPlus.sprite = SpriteUnknown;
                    SpriteSinHealthMinus.sprite = SpriteUnknown;
                    SpriteSinPeacePlus.sprite = SpriteUnknown;
                    SpriteSinPeaceMinus.sprite = SpriteUnknown;
                }

                break;
            case EPaperworkMarkType.Unmarked:
                break;
        }
    }

    public void UnHoverMark()
    {
        SpriteSinEcoPlus.gameObject.SetActive(false);
        SpriteSinEcoMinus.gameObject.SetActive(false);
        SpriteSinProsperityPlus.gameObject.SetActive(false);
        SpriteSinProsperityMinus.gameObject.SetActive(false);
        SpriteSinHealthPlus.gameObject.SetActive(false);
        SpriteSinHealthMinus.gameObject.SetActive(false);
        SpriteSinPeacePlus.gameObject.SetActive(false);
        SpriteSinPeaceMinus.gameObject.SetActive(false);
    }

    public override string GetHoverText()
    {
        if (GetProfile() == GameManager.instance.GrimReaperPaperworkProfile)
        {
            return "What the hell is this?";
        }
        if (GetProfile() == GameManager.instance.FatePaperworkProfile)
        {
            return "Holy... what?!";
        }
        else
        {
            return TextName.text + ". " + PaperworkManager.instance.GetRandomHoverText();
        }
    }

    public template_profile GetProfile()
    {
        return AssignedProfile;
    }


    public IEnumerator FadeInPaperwork(float duration)
    {
        TextName.material.SetFloat("_Level", 1.0f);
        TextAge.material.SetFloat("_Level", 1.0f);
        TextPosition.material.SetFloat("_Level", 1.0f);
        TextSituation.material.SetFloat("_Level", 1.0f);
        LabelName.material.SetFloat("_Level", 1.0f);
        LabelAge.material.SetFloat("_Level", 1.0f);
        LabelPosition.material.SetFloat("_Level", 1.0f);
        LabelLive.material.SetFloat("_Level", 1.0f);
        LabelDie.material.SetFloat("_Level", 1.0f);
        ImagePhoto.material.SetFloat("_Level", 1.0f);
        ImageMarkLive.material.SetFloat("_Level", 1.0f);
        ImageMarkDie.material.SetFloat("_Level", 1.0f);
        ImageBackgroundMarkDie.material.SetFloat("_Level", 1.0f);
        ImageBackgroundMarkLive.material.SetFloat("_Level", 1.0f);
        ImageBackground.material.SetFloat("_Level", 1.0f);
        ImageBackgroundBoxAge.material.SetFloat("_Level", 1.0f);
        ImageBackgroundBoxName.material.SetFloat("_Level", 1.0f);
        ImageBackgroundBoxPosition.material.SetFloat("_Level", 1.0f);

        SpriteSinOverlay.material.SetFloat("_Level", 1.0f);
        SpriteSinEco.material.SetFloat("_Level", 1.0f);
        SpriteSinEcoPlus.material.SetFloat("_Level", 1.0f);
        SpriteSinEcoMinus.material.SetFloat("_Level", 1.0f);
        SpriteSinProsperity.material.SetFloat("_Level", 1.0f);
        SpriteSinProsperityPlus.material.SetFloat("_Level", 1.0f);
        SpriteSinProsperityMinus.material.SetFloat("_Level", 1.0f);
        SpriteSinHealth.material.SetFloat("_Level", 1.0f);
        SpriteSinHealthPlus.material.SetFloat("_Level", 1.0f);
        SpriteSinHealthMinus.material.SetFloat("_Level", 1.0f);
        SpriteSinPeace.material.SetFloat("_Level", 1.0f);
        SpriteSinPeacePlus.material.SetFloat("_Level", 1.0f);
        SpriteSinPeaceMinus.material.SetFloat("_Level", 1.0f);


        yield return new WaitForSeconds(1.5f);
        FadeInParticleSystem.Stop();
        bFadingIn = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1.0f - Mathf.Clamp(elapsedTime / duration, 0.0f, 1.0f);

            TextName.material.SetFloat("_Level", alpha);
            TextAge.material.SetFloat("_Level", alpha);
            TextPosition.material.SetFloat("_Level", alpha);
            TextSituation.material.SetFloat("_Level", alpha);
            LabelName.material.SetFloat("_Level", alpha);
            LabelAge.material.SetFloat("_Level", alpha);
            LabelPosition.material.SetFloat("_Level", alpha);
            LabelLive.material.SetFloat("_Level", alpha);
            LabelDie.material.SetFloat("_Level", alpha);
            ImagePhoto.material.SetFloat("_Level", alpha);
            ImageMarkLive.material.SetFloat("_Level", alpha);
            ImageMarkDie.material.SetFloat("_Level", alpha);
            ImageBackgroundMarkDie.material.SetFloat("_Level", alpha);
            ImageBackgroundMarkLive.material.SetFloat("_Level", alpha);
            ImageBackground.material.SetFloat("_Level", alpha);
            ImageBackgroundBoxAge.material.SetFloat("_Level", alpha);
            ImageBackgroundBoxName.material.SetFloat("_Level", alpha);
            ImageBackgroundBoxPosition.material.SetFloat("_Level", alpha);

            SpriteSinOverlay.material.SetFloat("_Level", alpha);
            SpriteSinEco.material.SetFloat("_Level", alpha);
            SpriteSinEcoPlus.material.SetFloat("_Level", alpha);
            SpriteSinEcoMinus.material.SetFloat("_Level", alpha);
            SpriteSinProsperity.material.SetFloat("_Level", alpha);
            SpriteSinProsperityPlus.material.SetFloat("_Level", alpha);
            SpriteSinProsperityMinus.material.SetFloat("_Level", alpha);
            SpriteSinHealth.material.SetFloat("_Level", alpha);
            SpriteSinHealthPlus.material.SetFloat("_Level", alpha);
            SpriteSinHealthMinus.material.SetFloat("_Level", alpha);
            SpriteSinPeace.material.SetFloat("_Level", alpha);
            SpriteSinPeacePlus.material.SetFloat("_Level", alpha);
            SpriteSinPeaceMinus.material.SetFloat("_Level", alpha);

            //TextThanks.color = new Color(TextThanks.color.r, TextThanks.color.g, TextThanks.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            //TextVisitWebsite.color = new Color(TextVisitWebsite.color.r, TextVisitWebsite.color.g, TextVisitWebsite.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            //TextRestart.color = new Color(TextRestart.color.r, TextRestart.color.g, TextRestart.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            yield return null;
        }
        bFadingIn = false;

    }

    public IEnumerator FadeOutPaperwork(float duration)
    {
        bFadingOut = true;

        if (MarkStatus == EPaperworkMarkType.Die)
        {
            TextName.material = new Material(BlackMaterial);
            TextAge.material = new Material(BlackMaterial);
            TextPosition.material = new Material(BlackMaterial);
            TextSituation.material = new Material(BlackMaterial);
            LabelName.material = new Material(BlackMaterial);
            LabelAge.material = new Material(BlackMaterial);
            LabelPosition.material = new Material(BlackMaterial);
            LabelLive.material = new Material(BlackMaterial);
            LabelDie.material = new Material(BlackMaterial);
            ImagePhoto.material = new Material(BlackMaterial);
            ImageMarkDie.material = new Material(BlackMaterial);
            ImageMarkLive.material = new Material(BlackMaterial);
            ImageBackground.material = new Material(BlackMaterial);
            if (ProfileManager.instance.SubplotProfiles.Contains(GetProfile()))
            {
                ImageBackground.material.SetColor("_Color", SubplotProfileBackgroundColor);
            }
            ImageBackgroundMarkDie.material = new Material(BlackMaterial);
            ImageBackgroundMarkLive.material = new Material(BlackMaterial);
            ImageBackgroundBoxName.material = new Material(BlackMaterial);
            ImageBackgroundBoxAge.material = new Material(BlackMaterial);
            ImageBackgroundBoxPosition.material = new Material(BlackMaterial);

            SpriteSinOverlay.material = new Material(BlackMaterial);
            SpriteSinEco.material = new Material(BlackMaterial);
            SpriteSinEcoPlus.material = new Material(BlackMaterial);
            SpriteSinEcoMinus.material = new Material(BlackMaterial);
            SpriteSinProsperity.material = new Material(BlackMaterial);
            SpriteSinProsperityPlus.material = new Material(BlackMaterial);
            SpriteSinProsperityMinus.material = new Material(BlackMaterial);
            SpriteSinHealth.material = new Material(BlackMaterial);
            SpriteSinHealthPlus.material = new Material(BlackMaterial);
            SpriteSinHealthMinus.material = new Material(BlackMaterial);
            SpriteSinPeace.material = new Material(BlackMaterial);
            SpriteSinPeacePlus.material = new Material(BlackMaterial);
            SpriteSinPeaceMinus.material = new Material(BlackMaterial);
        }
        else
        {

        }

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            if (InputManager.instance.LastHitInteractable == GetComponent<Interactable>())
            {
                InputManager.instance.LastHitInteractable = null;
            }

            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Clamp(elapsedTime / duration, 0.0f, 1.0f);

            if (alpha > 0.5f)
            {
                FadeOutParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            TextName.material.SetFloat("_Level", alpha);
            TextAge.material.SetFloat("_Level", alpha);
            TextPosition.material.SetFloat("_Level", alpha);
            TextSituation.material.SetFloat("_Level", alpha);
            LabelName.material.SetFloat("_Level", alpha);
            LabelAge.material.SetFloat("_Level", alpha);
            LabelPosition.material.SetFloat("_Level", alpha);
            LabelLive.material.SetFloat("_Level", alpha);
            LabelDie.material.SetFloat("_Level", alpha);
            ImagePhoto.material.SetFloat("_Level", alpha);
            ImageMarkLive.material.SetFloat("_Level", alpha);
            ImageMarkDie.material.SetFloat("_Level", alpha);
            ImageBackgroundMarkDie.material.SetFloat("_Level", alpha);
            ImageBackgroundMarkLive.material.SetFloat("_Level", alpha);
            ImageBackground.material.SetFloat("_Level", alpha);
            ImageBackgroundBoxAge.material.SetFloat("_Level", alpha);
            ImageBackgroundBoxName.material.SetFloat("_Level", alpha);
            ImageBackgroundBoxPosition.material.SetFloat("_Level", alpha);

            SpriteSinOverlay.material.SetFloat("_Level", alpha);
            SpriteSinEco.material.SetFloat("_Level", alpha);
            SpriteSinEcoPlus.material.SetFloat("_Level", alpha);
            SpriteSinEcoMinus.material.SetFloat("_Level", alpha);
            SpriteSinProsperity.material.SetFloat("_Level", alpha);
            SpriteSinProsperityPlus.material.SetFloat("_Level", alpha);
            SpriteSinProsperityMinus.material.SetFloat("_Level", alpha);
            SpriteSinHealth.material.SetFloat("_Level", alpha);
            SpriteSinHealthPlus.material.SetFloat("_Level", alpha);
            SpriteSinHealthMinus.material.SetFloat("_Level", alpha);
            SpriteSinPeace.material.SetFloat("_Level", alpha);
            SpriteSinPeacePlus.material.SetFloat("_Level", alpha);
            SpriteSinPeaceMinus.material.SetFloat("_Level", alpha);

            for (int i = 0; i < AllLines.Count; ++i)
            {
                AllLines[i].LineRenderer.material.SetFloat("_Level", alpha);
            }

            //TextThanks.color = new Color(TextThanks.color.r, TextThanks.color.g, TextThanks.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            //TextVisitWebsite.color = new Color(TextVisitWebsite.color.r, TextVisitWebsite.color.g, TextVisitWebsite.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            //TextRestart.color = new Color(TextRestart.color.r, TextRestart.color.g, TextRestart.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));

            yield return null;
        }
        bFadingOut = false;



        if (bIsCreditsProfile)
        {
            while (FadeOutParticleSystem.IsAlive())
            {
                yield return null;
            }
        }
        if (bIsCreditsProfile)
        {
            InputManager.instance.LastHitInteractable = null;
            Destroy(gameObject);
        }
    }

    bool bIsErasingLines = false;
    public void EraseLines()
    {
        if (!bIsErasingLines)
        {
            AudioManager.instance.PlayOneShotEffect(AudioManager.instance.MarkErase);
            StartCoroutine(EraseLinesRoutine());
        }
    }
    IEnumerator EraseLinesRoutine()
    {
        bIsErasingLines = true;
        float elapsedTime = 0.0f;
        float duration = 0.75f;
        float alpha = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            alpha = Mathf.Clamp(elapsedTime / duration, 0.0f, 1.0f);
            for (int i = 0; i < AllLines.Count; ++i)
            {
                AllLines[i].LineRenderer.material.SetFloat("_Level", alpha);
            }
            yield return null;
        }

        for (int i = 0; i < AllLines.Count; ++i)
        {
            Destroy(AllLines[i].gameObject);
        }

        AllLines.Clear();
        bIsErasingLines = false;
    }
}
