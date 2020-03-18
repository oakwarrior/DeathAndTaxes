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


[System.Serializable]
public class ElevatorFloorPoint
{
    [SerializeField]
    public EScene DestinationType;
    [SerializeField]
    public float DestinationHeight;
}

public class Elevator : MonoBehaviour
{
    public static Elevator instance;

    void Awake()
    {
        instance = this;
        for (int i = 0; i < ButtonList.Count; ++i)
        {
            ButtonDictionary.Add(ButtonList[i].DestinationScene, ButtonList[i]);
        }
    }

    [SerializeField]
    public SpriteRenderer BackgroundRenderer;
    [SerializeField]
    SpriteRenderer BuildingRenderer;
    [SerializeField]
    SpriteRenderer ElevatorRenderer;

    [SerializeField]
    Sprite BackgroundChaos;
    [SerializeField]
    Sprite BackgroundDay;
    [SerializeField]
    Sprite BuildingDay;
    [SerializeField]
    Sprite BackgroundNight;
    [SerializeField]
    Sprite BuildingNight;

    float ScrollVelocity = 0.0f;
    [SerializeField]
    float MaxScrollVelocity = 10.0f;

    [SerializeField]
    float YOffsetMin = -24.0f;
    [SerializeField]
    float YOffsetMax = 18.0f;
    [SerializeField]
    List<ElevatorFloorPoint> FloorList = new List<ElevatorFloorPoint>();

    [SerializeField]
    SpriteRenderer ChibiHeadRenderer;
    [SerializeField]
    SpriteRenderer ChibiBodyRenderer;

    [SerializeField]
    Sprite HeadSkull;
    [SerializeField]
    Sprite HeadSugarSkull;
    [SerializeField]
    Sprite HeadAnubis;
    [SerializeField]
    Sprite HeadKitty;
    [SerializeField]
    Sprite HeadCthulhu;
    [SerializeField]
    Sprite HeadPlague;
    [SerializeField]
    Sprite HeadJohn;
    [SerializeField]
    Sprite HeadJane;

    [SerializeField]
    Sprite BodySuit;
    [SerializeField]
    Sprite BodyCape;

    public bool bIsMovingToFloor = false;
    bool bPlayedDing = false;

    [SerializeField]
    List<ElevatorButton> ButtonList = new List<ElevatorButton>();

    Dictionary<EScene, ElevatorButton> ButtonDictionary = new Dictionary<EScene, ElevatorButton>();

    Vector3 ScrollPosition;
    Vector3 MoveToFloorOrigin;
    Vector3 MoveToFloorTarget;

    [SerializeField]
    GameObject BackgroundParent;
    [SerializeField]
    GameObject BuildingParent;

    private void Start()
    {

    }

    [SerializeField]
    List<GameObject> RandomDayChibiLayoutsFatePresent = new List<GameObject>();

    [SerializeField]
    List<GameObject> RandomDayChibiLayoutsFateGone = new List<GameObject>();

    [SerializeField]
    List<GameObject> RandomNightChibiLayoutsFatePresent = new List<GameObject>();

    [SerializeField]
    List<GameObject> RandomNightChibiLayoutsFateGone = new List<GameObject>();


    Coroutine CurrentMoveCoroutine;

    [SerializeField]
    GameObject FateChibiOffice = null;

    EScene TargetFloor;

    GameObject CurrentChibiLayout;

    public void SetDaySprites()
    {
        bool whackedFate = ArticyGlobalVariables.Default.game.subplot_finale_activated &&
            !ArticyGlobalVariables.Default.profile.fate_spared &&
            SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() >= 28;

        float averageRepStatus = (ArticyGlobalVariables.Default.rep.ecology + ArticyGlobalVariables.Default.rep.prosperity + ArticyGlobalVariables.Default.rep.peace + ArticyGlobalVariables.Default.rep.health) / 4;


        bool badStatus = (ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
                            ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
                            ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
                            ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true) ||
                            averageRepStatus <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Critical] / 2;

        if (whackedFate || badStatus)
        {
            BackgroundRenderer.sprite = BackgroundChaos;
        }
        else
        {
            BackgroundRenderer.sprite = BackgroundDay;
        }

        BuildingRenderer.sprite = BuildingDay;

        if (CurrentChibiLayout != null)
        {
            CurrentChibiLayout.SetActive(false);
        }

        if (ArticyGlobalVariables.Default.game.fate_absent || whackedFate)
        {
            CurrentChibiLayout = RandomDayChibiLayoutsFateGone[Random.Range(0, RandomDayChibiLayoutsFateGone.Count)];
        }
        else
        {
            CurrentChibiLayout = RandomDayChibiLayoutsFatePresent[Random.Range(0, RandomDayChibiLayoutsFatePresent.Count)];
        }


        CurrentChibiLayout.SetActive(true);
    }

    public void SetNightSprites()
    {
        bool whackedFate = ArticyGlobalVariables.Default.game.subplot_finale_activated &&
            !ArticyGlobalVariables.Default.profile.fate_spared &&
            SaveManager.instance.GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() >= 28;

        float averageRepStatus = (ArticyGlobalVariables.Default.rep.ecology + ArticyGlobalVariables.Default.rep.prosperity + ArticyGlobalVariables.Default.rep.peace + ArticyGlobalVariables.Default.rep.health) / 4;


        bool badStatus = (ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
                            ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
                            ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
                            ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true) ||
                            averageRepStatus <= GameManager.instance.ChaosThresholds[(int)EChaosThreshold.Critical] / 2;

        if (whackedFate || badStatus)
        {
            BackgroundRenderer.sprite = BackgroundChaos;
        }
        else
        {
            BackgroundRenderer.sprite = BackgroundNight;
        }

        BuildingRenderer.sprite = BuildingNight;

        if (CurrentChibiLayout != null)
        {
            CurrentChibiLayout.SetActive(false);
        }
        if (ArticyGlobalVariables.Default.game.fate_absent || whackedFate)
        {
            CurrentChibiLayout = RandomNightChibiLayoutsFateGone[Random.Range(0, RandomNightChibiLayoutsFateGone.Count)];
        }
        else
        {
            CurrentChibiLayout = RandomNightChibiLayoutsFatePresent[Random.Range(0, RandomNightChibiLayoutsFatePresent.Count)];
        }

        CurrentChibiLayout.SetActive(true);
    }

    private void Update()
    {
        if (bIsMovingToFloor && ElevatorManager.instance.bIsChangingScene)
        {
            ScrollVelocity = 0.0f;
            return;
        }

        //scroll damping
        if (ScrollVelocity > 0.0f)
        {
            ScrollVelocity = Mathf.Clamp(ScrollVelocity - Time.deltaTime * 10.0f, 0.0f, MaxScrollVelocity);
        }

        if (ScrollVelocity < 0.0f)
        {
            ScrollVelocity = Mathf.Clamp(ScrollVelocity + Time.deltaTime * 10.0f, -MaxScrollVelocity, 0.0f);
        }

        if (ScrollVelocity != 0.0f)
        {
            Vector3 newPos = ScrollPosition + ScrollVelocity * Vector3.up * Time.deltaTime;

            newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, YOffsetMin, YOffsetMax));

            SetElevatorPosition(newPos);

            if (ScrollPosition.y >= YOffsetMax)
            {
                ScrollVelocity = 0.0f;
                //IncrementPanelLine();
            }
            if (ScrollPosition.y <= YOffsetMin)
            {
                ScrollVelocity = 0.0f;
                //DecrementPanelLine();
            }

        }
    }

    public void SetElevatorPosition(Vector3 pos)
    {
        ScrollPosition = pos;

        BuildingParent.gameObject.transform.position = ScrollPosition;
        BackgroundParent.gameObject.transform.position = ScrollPosition;
        SaveManager.instance.GetCurrentPlayerState().ElevatorHeight = ScrollPosition.y;
    }

    public void AddElevatorVelocity(float velocity)
    {
        ScrollVelocity = Mathf.Clamp(ScrollVelocity + velocity, -MaxScrollVelocity, MaxScrollVelocity);
    }

    public ElevatorButton GetElevatorButtonBySceneType(EScene type)
    {
        return ButtonDictionary[type];
    }

    public void MoveToFloor(EScene floor)
    {
        TargetFloor = floor;
        if (bIsMovingToFloor)
        {
            StopCoroutine(MoveToFloorRoutine());

            if(TargetFloor == EScene.DressingRoom)
            {
                ShopNotification.instanceMirror.ToggleSpriteOpen(true);
            }
            if(TargetFloor == EScene.Desktop)
            {
                ShopNotification.instanceOffice.ToggleSpriteOpen(true);
            }
            CurrentMoveCoroutine = StartCoroutine(MoveToFloorRoutine());
        }
        else
        {
            if (TargetFloor == EScene.DressingRoom)
            {
                ShopNotification.instanceMirror.ToggleSpriteOpen(true);
            }
            if (TargetFloor == EScene.Desktop)
            {
                ShopNotification.instanceOffice.ToggleSpriteOpen(true);
            }
            CurrentMoveCoroutine = StartCoroutine(MoveToFloorRoutine());
        }
    }

    public void CancelMoveToFloor()
    {
        ShopNotification.instanceOffice.ToggleSpriteOpen(false);
        ShopNotification.instanceMirror.ToggleSpriteOpen(false);

        bIsMovingToFloor = false;
        if (CurrentMoveCoroutine != null)
        {
            StopCoroutine(CurrentMoveCoroutine);
        }
    }

    public IEnumerator MoveToFloorRoutine()
    {
        bIsMovingToFloor = true;
        MoveToFloorOrigin = ScrollPosition;
        MoveToFloorTarget = new Vector3();
        for (int i = 0; i < FloorList.Count; ++i)
        {
            if (FloorList[i].DestinationType == TargetFloor)
            {
                MoveToFloorTarget.y = FloorList[i].DestinationHeight;
            }
        }

        float elapsedTime = 0;
        float duration = Vector3.Distance(MoveToFloorOrigin, MoveToFloorTarget) / 3.0f;
        bPlayedDing = false;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            SetElevatorPosition(Vector3.Lerp(MoveToFloorOrigin, MoveToFloorTarget, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f)));

            if ((duration - elapsedTime) < 0.2f && !bPlayedDing)
            {
                AudioManager.instance.PlayOneShotEffect(AudioManager.instance.ClipElevatorDing);
                bPlayedDing = true;
            }
            yield return null;
        }

        ScrollVelocity = 0.0f;
        bIsMovingToFloor = false;
        CurrentMoveCoroutine = null;
        ElevatorManager.instance.SwitchScene(TargetFloor);
    }

    public Sprite GetChibiHeadSprite()
    {
        return ChibiHeadRenderer.sprite;
    }

    public void UpdatePlayerChibi()
    {
        Sprite headSprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentHeadAsset();
        Sprite bodySprite = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentBodyAsset();

        if(headSprite == null)
        {
            ChibiHeadRenderer.sprite = HeadSkull;
            return;
        }
        if(bodySprite == null)
        {
            ChibiBodyRenderer.sprite = BodySuit;
            return;
        }
        if (headSprite.name.Contains("_cthulhu_"))
        {
            ChibiHeadRenderer.sprite = HeadCthulhu;
        }
        else if (headSprite.name.Contains("_skull_"))
        {
            ChibiHeadRenderer.sprite = HeadSkull;
        }
        else if (headSprite.name.Contains("_sugarskull_"))
        {
            ChibiHeadRenderer.sprite = HeadSugarSkull;
        }
        else if (headSprite.name.Contains("_plague_"))
        {
            ChibiHeadRenderer.sprite = HeadPlague;
        }
        else if (headSprite.name.Contains("_anubis_"))
        {
            ChibiHeadRenderer.sprite = HeadAnubis;
        }
        else if (headSprite.name.Contains("_kitty_"))
        {
            ChibiHeadRenderer.sprite = HeadKitty;
        }
        else if (headSprite.name.Contains("_john_"))
        {
            ChibiHeadRenderer.sprite = HeadJohn;
        }
        else if (headSprite.name.Contains("_jane_"))
        {
            ChibiHeadRenderer.sprite = HeadJane;
        }

        if (bodySprite.name.Contains("_tie_") || bodySprite.name.Contains("_bowtie_"))
        {
            ChibiBodyRenderer.sprite = BodySuit;
        }
        else if (bodySprite.name.Contains("_cape_"))
        {
            ChibiBodyRenderer.sprite = BodyCape;
        }
    }
}
