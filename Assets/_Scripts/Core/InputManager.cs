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
using Articy.Project_Of_Death.GlobalVariables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputManager : ManagerBase
{
    public static InputManager instance;

    private void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.SetCursor(TextureCursor, CursorHotSpot, CursorMode);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CursorEnableRoutine());
    }

    IEnumerator CursorEnableRoutine()
    {
        yield return new WaitForEndOfFrame();
        Cursor.visible = true;
    }

    [SerializeField]
    Texture2D TextureCursor;
    [SerializeField]
    CursorMode CursorMode = CursorMode.Auto;
    [SerializeField]
    Vector2 CursorHotSpot = Vector2.zero;

    Vector3 LastMousePosition;
    Vector2 LastTouchPosition;

    Vector3 DragStart;
    Vector3 DragEnd;

    public bool bLockUntilInputEnd = false;
    bool DragStarted = false;

    public Interactable LastHitInteractable = null;
    Interactable MouseDownInteractable = null;

    Draggable CurrentDraggable = null;

    bool bFrameLock = false;


    public override void InitManager()
    {
        base.InitManager();

    }

    private void HandleComicInput()
    {
        if (!bLockUntilInputEnd)
        {
            //comic
            if (Input.touchCount > 0)
            {
                Touch touchy = Input.GetTouch(0);
                if (touchy.phase == TouchPhase.Began)
                {
                    LastTouchPosition = touchy.position;
                }

                if (touchy.phase == TouchPhase.Moved)
                {
                    if ((touchy.position - LastTouchPosition).x > 0)
                    {
                        ComicManager.instance.AddCameraVelocity(-0.8f);
                    }
                    if ((touchy.position - LastTouchPosition).x < 0)
                    {
                        ComicManager.instance.AddCameraVelocity(0.8f);
                    }
                }
            }


            if (Input.GetMouseButton(0))
            {
                if ((Input.mousePosition - LastMousePosition).x > 0)
                {
                    ComicManager.instance.AddCameraVelocity(-0.8f);
                }
                if ((Input.mousePosition - LastMousePosition).x < 0)
                {
                    ComicManager.instance.AddCameraVelocity(0.8f);
                }

                LastMousePosition = Input.mousePosition;

            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            Interactable hitInteractable = null;
            if (hit.collider != null)
            {
                hitInteractable = (Interactable)hit.collider.gameObject.GetComponentInParent(typeof(Interactable));
            }

            if (hitInteractable != null)
            {
                if (LastHitInteractable != hitInteractable)
                {
                    LastHitInteractable = hitInteractable;
                    LastHitInteractable.Hover();
                }
            }
            else
            {
                if (LastHitInteractable != null)
                {
                    LastHitInteractable.Unhover();
                    LastHitInteractable = null;
                }
            }

            Vector3 cameraPos = Camera.main.transform.position;
            if (Input.mouseScrollDelta.y > 0.0f) //left
            {
                ComicManager.instance.AddCameraVelocity(-1.2f);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                ComicManager.instance.AddCameraVelocity(-0.2f);
            }
            if (Input.mouseScrollDelta.y < 0.0f) //right
            {
                ComicManager.instance.AddCameraVelocity(1.2f);
                //Camera.main.transform.position = cameraPos + new Vector3(-1.0f, 0.0f) * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                ComicManager.instance.AddCameraVelocity(0.2f);
            }

            //if (GameManager.instance != null && (GameManager.instance.GameMode == EGameMode.DEBUG) && Input.GetKeyDown(KeyCode.S))
            //{
            //    ComicManager.instance.EndComic();
            //}
        }
        else
        {
            if (Input.touchCount > 0)
            {
                bLockUntilInputEnd = false;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            Interactable hitInteractable = null;
            if (hit.collider != null)
            {
                hitInteractable = (Interactable)hit.collider.gameObject.GetComponentInParent(typeof(Interactable));
            }

            if (Input.GetMouseButton(0))
            {
                bLockUntilInputEnd = false;

                if (hitInteractable != null)
                {
                    bLockUntilInputEnd = true;
                }
            }

            if (hitInteractable != null)
            {
                if (LastHitInteractable != hitInteractable)
                {
                    LastHitInteractable = hitInteractable;
                    LastHitInteractable.Hover();
                }
            }
            else
            {
                if (LastHitInteractable != null)
                {
                    LastHitInteractable.Unhover();
                    LastHitInteractable = null;
                }
            }

            Vector3 cameraPos = Camera.main.transform.position;
            if (Input.mouseScrollDelta.y > 0.0f) //left
            {
                bLockUntilInputEnd = false;
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                bLockUntilInputEnd = false;
            }
            if (Input.mouseScrollDelta.y < 0.0f) //right
            {
                bLockUntilInputEnd = false;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                bLockUntilInputEnd = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            Interactable hitInteractable = null;
            if (hit.collider != null)
            {
                hitInteractable = (Interactable)hit.collider.gameObject.GetComponentInParent(typeof(Interactable));

                if (hitInteractable != null)
                {
                    hitInteractable.Interact();
                }
            }
        }
        else if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            Interactable hitInteractable = null;
            if (hit.collider != null)
            {
                hitInteractable = (Interactable)hit.collider.gameObject.GetComponentInParent(typeof(Interactable));

                if (hitInteractable != null)
                {
                    hitInteractable.Interact();
                }
            }
        }
    }

    public void DebugSkipDialogue()
    {
        if (DialogueManager.instance.IsDialogueActive() && !DialogueScreen.instance.IsDialogueStarting())
        {
            DialogueManager.instance.EndDialogueInstantly();
        }
    }

    public void DebugUpdateBestWorstParameters()
    {
        KeyValuePair<string, int> bestParameter = SaveManager.instance.GetCurrentPlayerState().GetBestParameter();
        KeyValuePair<string, int> worstParameter = SaveManager.instance.GetCurrentPlayerState().GetWorstParameter();

        ArticyGlobalVariables.Default.rep.best_parameter_name = bestParameter.Key;
        ArticyGlobalVariables.Default.rep.best_parameter_value = bestParameter.Value;

        ArticyGlobalVariables.Default.rep.worst_parameter_name = worstParameter.Key;
        ArticyGlobalVariables.Default.rep.worst_parameter_value = worstParameter.Value;
    }

    public void DebugDecreaseHealthParameter()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH, Mathf.RoundToInt(-100));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH_DAILY, Mathf.RoundToInt(-100));

        DebugUpdateBestWorstParameters();

        ChaosGlobe.instance.MarkSituationDirty();

    }

    public void DebugIncreaseHealthParameter()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH, Mathf.RoundToInt(100));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.HEALTH_DAILY, Mathf.RoundToInt(100));

        DebugUpdateBestWorstParameters();
        ChaosGlobe.instance.MarkSituationDirty();

    }

    public void DebugDecreaseLoyalty()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.LOYALTY, Mathf.RoundToInt(-100));
    }

    public void DebugIncreaseLoyalty()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.LOYALTY, Mathf.RoundToInt(100));
    }

    public void DebugDecreaseEcologyParameter()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY, Mathf.RoundToInt(-100));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY_DAILY, Mathf.RoundToInt(-100));
        DebugUpdateBestWorstParameters();
        ChaosGlobe.instance.MarkSituationDirty();
    }

    public void DebugIncreaseEcologyParameter()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY, Mathf.RoundToInt(100));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.ECOLOGY_DAILY, Mathf.RoundToInt(100));

        DebugUpdateBestWorstParameters();

        ChaosGlobe.instance.MarkSituationDirty();
    }

    public void DebugDecreaseProsperityParameter()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY, Mathf.RoundToInt(-100));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY_DAILY, Mathf.RoundToInt(-100));
        DebugUpdateBestWorstParameters();
        ChaosGlobe.instance.MarkSituationDirty();
    }

    public void DebugIncreaseProsperityParameter()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY, Mathf.RoundToInt(100));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PROSPERITY_DAILY, Mathf.RoundToInt(100));
        DebugUpdateBestWorstParameters();
        ChaosGlobe.instance.MarkSituationDirty();
    }

    public void DebugDecreasePeaceParameter()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE, Mathf.RoundToInt(-100));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE_DAILY, Mathf.RoundToInt(-100));
        DebugUpdateBestWorstParameters();
        ChaosGlobe.instance.MarkSituationDirty();
    }

    public void DebugIncreasePeaceParameter()
    {
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE, Mathf.RoundToInt(100));
        SaveManager.instance.GetCurrentPlayerState().ModifyStat(EStat.PEACE_DAILY, Mathf.RoundToInt(100));
        DebugUpdateBestWorstParameters();
        ChaosGlobe.instance.MarkSituationDirty();
    }


    public void DebugClearSave()
    {
        SaveManager.instance.ClearSave();
        SaveManager.instance.GetCurrentPlayerState().SetSpawnCounter(Random.Range(10, 100));
        SaveManager.instance.GetCurrentPlayerState().SetRestartCounter(0);
    }

    public void DebugFinishDay()
    {
        DesktopManager.instance.FinishDay();
    }

    public void DebugSetGameFinished()
    {
        ArticyGlobalVariables.Default.game.finished = true;


    }

    public void DebugSetEvilTriggerEco()
    {
        ArticyGlobalVariables.Default.profile.evil_ecology_trigger = true;
        SaveManager.instance.MarkSavegameDirty();
    }
    public void DebugSetEvilTriggerPeace()
    {
        ArticyGlobalVariables.Default.profile.evil_peace_trigger = true;
        SaveManager.instance.MarkSavegameDirty();
    }
    public void DebugSetEvilTriggerHealth()
    {
        ArticyGlobalVariables.Default.profile.evil_health_trigger = true;
        SaveManager.instance.MarkSavegameDirty();
    }
    public void DebugSetEvilTriggerProsperity()
    {
        ArticyGlobalVariables.Default.profile.evil_prosperity_trigger = true;
        SaveManager.instance.MarkSavegameDirty();
    }

    public void DebugSetSubplotFinaleActive()
    {
        ArticyGlobalVariables.Default.game.finished = false;
        ArticyGlobalVariables.Default.game.subplot_finale_activated = true;

        GameManager.instance.FatePaperworkProfile.Template.profile_basic_data.profile_is_enabled = true;

        for (int j = 0; j < ProfileManager.instance.SubplotProfiles.Count; ++j)
        {
            ProfileManager.instance.SubplotProfiles[j].Template.profile_death_data.profile_death_instruction.CallScript();
            ProfileManager.instance.SubplotProfiles[j].Template.profile_spare_data.profile_spare_instruction.CallScript();

            for (int i = 0; i < ProfileManager.instance.SubplotProfiles[j].Template.profile_spare_data.profile_spare_disabler.Count; ++i)
            {
                template_profile disabledProfile = ProfileManager.instance.SubplotProfiles[j].Template.profile_spare_data.profile_spare_disabler[i] as template_profile;
                disabledProfile.Template.profile_basic_data.profile_is_enabled = false;
                SaveManager.instance.GetCurrentPlayerState().AddDisabledProfile(disabledProfile.Id);
            }

            for (int i = 0; i < ProfileManager.instance.SubplotProfiles[j].Template.profile_spare_data.profile_spare_enabler.Count; ++i)
            {
                template_profile enabledProfile = ProfileManager.instance.SubplotProfiles[j].Template.profile_spare_data.profile_spare_enabler[i] as template_profile;
                enabledProfile.Template.profile_basic_data.profile_is_enabled = true;
                SaveManager.instance.GetCurrentPlayerState().AddEnabledProfile(enabledProfile.Id);
            }

            for (int i = 0; i < ProfileManager.instance.SubplotProfiles[j].Template.profile_death_data.profile_death_disabler.Count; ++i)
            {
                template_profile disabledProfile = ProfileManager.instance.SubplotProfiles[j].Template.profile_death_data.profile_death_disabler[i] as template_profile;
                disabledProfile.Template.profile_basic_data.profile_is_enabled = false;
                SaveManager.instance.GetCurrentPlayerState().AddDisabledProfile(disabledProfile.Id);
            }

            for (int i = 0; i < ProfileManager.instance.SubplotProfiles[j].Template.profile_death_data.profile_death_enabler.Count; ++i)
            {
                template_profile enabledProfile = ProfileManager.instance.SubplotProfiles[j].Template.profile_death_data.profile_death_enabler[i] as template_profile;
                enabledProfile.Template.profile_basic_data.profile_is_enabled = true;
                SaveManager.instance.GetCurrentPlayerState().AddEnabledProfile(enabledProfile.Id);
            }
        }

    }

    public void DebugSetSelfiePrimed()
    {
        ArticyGlobalVariables.Default.rep.loyalty = 1400;
        ArticyGlobalVariables.Default.game.work_complaint_counter = 3;
    }

    public void DebugAddAllItems()
    {
        ArticyGlobalVariables.Default.inventory.cactus = true;
        ArticyGlobalVariables.Default.inventory.eraser = true;
        ArticyGlobalVariables.Default.inventory.radio = true;
        ArticyGlobalVariables.Default.inventory.fidget_thing = true;
        ArticyGlobalVariables.Default.inventory.toy_cat = true;
        ArticyGlobalVariables.Default.inventory.desklamp = true;
        ArticyGlobalVariables.Default.inventory.coin = true;
        ArticyGlobalVariables.Default.inventory.calendar = true;
        ArticyGlobalVariables.Default.inventory.award_plaque = true;
        ArticyGlobalVariables.Default.inventory.snowglobe = true;
        ArticyGlobalVariables.Default.inventory.sin_bulb = true;
        ArticyGlobalVariables.Default.inventory.mirror = true;

        GrimDesk.instance.UpdateDeskItemOwnedStatus();
        DeskLampLight.instance.CheckSinBulbActive();
    }

    public void DebugCheatMoney()
    {
        ArticyGlobalVariables.Default.game.salary_on = true;
        SaveManager.instance.GetCurrentPlayerState().ModifyMoney(500);
        MoneyNotification.instance.ShowMoneyNotification(500);

    }

    public void DebugAddAllAccessories()
    {
        ShopManager.instance.AddAllAccessories();
    }

    public void DebugResetDayAlsoRefreshesShop()
    {
        ShopManager.instance.NotifyStartDay(false);
    }

    public void DebugForceSave()
    {
        SaveManager.instance.MarkSavegameDirty();
    }

    private void HandleDebugInput()
    {
        if (OptionsManager.instance.gameObject.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DebugSkipDialogue();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                DebugDecreaseHealthParameter();

            }
            else
            {
                DebugIncreaseHealthParameter();

            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                DebugDecreaseEcologyParameter();

            }
            else
            {
                DebugIncreaseEcologyParameter();

            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                DebugDecreaseProsperityParameter();

            }
            else
            {
                DebugIncreaseProsperityParameter();

            }
            ChaosGlobe.instance.MarkSituationDirty();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                DebugDecreasePeaceParameter();

            }
            else
            {
                DebugIncreasePeaceParameter();

            }
            ChaosGlobe.instance.MarkSituationDirty();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DebugSkipDialogue();

        }
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    DemoEndScreen.instance.ShowDemoEndScreen();
        //    StartCoroutine(DemoEndScreen.instance.FadeInDemoThanksText(1.0f));
        //}
        if (Input.GetKeyDown(KeyCode.C))
        {
            DebugClearSave();

        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            DebugFinishDay();

            DebugSetGameFinished();

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            DebugSetSubplotFinaleActive();

        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            DebugSetSelfiePrimed();

        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            DebugAddAllAccessories();

        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            DebugAddAllItems();

        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            DebugCheatMoney();

        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            DebugResetDayAlsoRefreshesShop();

        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            DebugForceSave();

        }
    }

    private bool IsUIBlockingInput()
    {
        return FaxConfirm.instance.gameObject.activeInHierarchy ||
        MarkConfirm.instance.gameObject.activeInHierarchy ||
        DialogueManager.instance.IsDialogueActive() ||
        SpeechBubbleManager.instance.IsBubbleSpeechActive() ||
        DemoEndScreen.instance.gameObject.activeInHierarchy ||
        DialogueScreen.instance.gameObject.activeInHierarchy ||
        IntroController.instance.gameObject.activeInHierarchy ||
        OptionsManager.instance.gameObject.activeInHierarchy ||
        GalleryScreen.instance.gameObject.activeInHierarchy ||
        bFrameLock
        /*IsPointerOverUIObject()*/;
    }

    private void HandleElevatorInput()
    {

        if (Input.touchCount > 0 && LastHitInteractable == null)
        {
            Touch touchy = Input.GetTouch(0);
            if (touchy.phase == TouchPhase.Began)
            {
                LastTouchPosition = touchy.position;
            }

            if (touchy.phase == TouchPhase.Moved)
            {
                if ((touchy.position - LastTouchPosition).y > 0)
                {
                    Elevator.instance.AddElevatorVelocity(-0.8f);
                }
                if ((touchy.position - LastTouchPosition).y < 0)
                {
                    Elevator.instance.AddElevatorVelocity(0.8f);
                }
            }
        }



        if (Input.GetMouseButton(0) && LastHitInteractable == null)
        {
            if ((Elevator.instance.gameObject.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y) < 0)
            {
                Elevator.instance.AddElevatorVelocity(0.8f);
            }
            if ((Elevator.instance.gameObject.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y) > 0)
            {
                Elevator.instance.AddElevatorVelocity(-0.8f);
            }

            LastMousePosition = Input.mousePosition;
        }

        Vector3 cameraPos = Camera.main.transform.position;
        if (Input.mouseScrollDelta.y > 0.0f) //up
        {
            Elevator.instance.AddElevatorVelocity(-1.6f);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            Elevator.instance.AddElevatorVelocity(-1.2f);
        }
        if (Input.mouseScrollDelta.y < 0.0f) //down
        {
            Elevator.instance.AddElevatorVelocity(1.6f);
            //Camera.main.transform.position = cameraPos + new Vector3(-1.0f, 0.0f) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            Elevator.instance.AddElevatorVelocity(1.2f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Elevator.instance.CancelMoveToFloor();
        }
        //if (GameManager.instance != null && (GameManager.instance.GameMode == EGameMode.DEBUG) && Input.GetKeyDown(KeyCode.S))
        //{
        //    ComicManager.instance.EndComic();
        //}
    }

    private void HandleInteractableInput(bool bLeftMouseDown, bool bLeftMouseUp)
    {

        //int layerMask = 1 << LayerMask.NameToLayer("Container");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        Interactable hitInteractable = null;
        if (hit.collider != null)
        {
            //hitInteractable = (Interactable)hit.collider.gameObject.GetComponent(typeof(Interactable));
            //if(hitInteractable == null)
            //{
            //}
            hitInteractable = (Interactable)hit.collider.gameObject.GetComponentInParent(typeof(Interactable));
            //TODO: remove
            //Draggable newDraggable = hit.collider.gameObject.GetComponentInParent<Draggable>();
            //if (CurrentDraggable != null && CurrentDraggable != newDraggable)
            //{
            //    CurrentDraggable.ToggleDragging(false);
            //}

            if (hitInteractable != null && LastHitInteractable != hitInteractable)
            {
                if (LastHitInteractable != null)
                {
                    LastHitInteractable.Unhover();
                }
                LastHitInteractable = hitInteractable;
                if (LastHitInteractable != null)
                {
                    LastHitInteractable.Hover();
                }
                HUDManager.instance.SetHoverText(hitInteractable.GetHoverText());
            }
            else
            {

            }
        }
        else
        {

            //TODO: remove
            //if (CurrentDraggable != null)
            //{
            //    CurrentDraggable.ToggleDragging(false);
            //}

            if (LastHitInteractable != null)
            {
                LastHitInteractable.Unhover();
            }

            HUDManager.instance.SetHoverText("");
            LastHitInteractable = null;
        }

        // once
        if (bLeftMouseDown && !IsPointerOverUIObject() && hit.collider != null)
        {
            DragStart = Input.mousePosition;
            CurrentDraggable = hit.collider.gameObject.GetComponentInParent<Draggable>();
            if (CurrentDraggable != null && CurrentDraggable.CanDrag())
            {
                CurrentDraggable.UpdateDragGrabPosition(hit.point);
            }

            MouseDownInteractable = hit.collider.gameObject.GetComponentInParent<Interactable>();
        }


        // continous
        if (Input.GetMouseButton(0))
        {
            if (Vector3.Distance(DragStart, Input.mousePosition) >= 15.0f)
            {
                if (CurrentDraggable != null && CurrentDraggable.CanDrag())
                {
                    if (!CurrentDraggable.IsDragging())
                    {
                        //CurrentDraggable.UpdateDragGrabPosition(hit.point);
                        CurrentDraggable.ToggleDragging(true);
                    }

                    int layerMaskDrag = 1 << LayerMask.NameToLayer("Drawer");
                    Ray rayDrag = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hitDrag = Physics2D.GetRayIntersection(rayDrag, Mathf.Infinity, layerMaskDrag);
                    GrimDeskDrawer hitDrawer = null;

                    if (hitDrag.collider != null)
                    {
                        //Debug.Log("drag hit: " + hitDrag.collider.gameObject);

                        //hitInteractable = (Interactable)hit.collider.gameObject.GetComponent(typeof(Interactable));
                        //if(hitInteractable == null)
                        //{
                        //}
                        hitDrawer = hitDrag.collider.gameObject.GetComponentInParent<GrimDeskDrawer>();
                        if (hitDrawer != null)
                        {
                            if (hitDrawer.IsOpen())
                            {
                                CurrentDraggable.HandleDropIntoDrawer(hitDrawer);
                            }
                        }
                        else
                        {
                            CurrentDraggable.HandleDragOutOfDrawer();
                        }
                    }
                    else
                    {
                        CurrentDraggable.HandleDragOutOfDrawer();
                    }
                }
            }
        }

        // once
        if (bLeftMouseUp && !IsPointerOverUIObject())
        {
            DragEnd = Input.mousePosition;

            if (hitInteractable != null)
            {
                if (MouseDownInteractable == hitInteractable)
                {
                    if (CurrentDraggable != null)
                    {
                        if (Vector3.Distance(DragStart, DragEnd) < 15.0f)
                        {
                            hitInteractable.Interact();
                        }
                    }
                    else
                    {
                        if (hit.collider.gameObject.GetComponentInParent<Phone>())
                        {
                            if (Vector3.Distance(DragStart, DragEnd) < 15.0f)
                            {
                                hitInteractable.Interact();
                            }
                        }
                        else
                        {
                            hitInteractable.Interact();
                        }
                    }
                }
                MouseDownInteractable = null;
            }
            else
            {
                if (MarkerOfDeath.instance.IsPickedUp())
                {
                    MarkerOfDeath.instance.Interact();
                }
                else if (Eraser.instance.IsPickedUp())
                {
                    Eraser.instance.Interact();
                }
                else
                {

                }
            }
            if (CurrentDraggable != null)
            {
                if (CurrentDraggable.IsDragging())
                {
                    CurrentDraggable.ToggleDragging(false);
                    Paperwork draggedPaperwork = CurrentDraggable as Paperwork;
                    if (draggedPaperwork != null && draggedPaperwork.Status == EPaperworkStatus.DESK)
                    {
                        int layerMaskDrag = 1 << LayerMask.NameToLayer("Fax");
                        RaycastHit2D hitFax = Physics2D.Raycast(draggedPaperwork.gameObject.transform.position, Vector2.down, Mathf.Infinity, layerMaskDrag);
                        if (hitFax.collider != null)
                        {
                            FaxMachine misterfaxobeat = hitFax.collider.gameObject.GetComponent<FaxMachine>();
                            if (misterfaxobeat != null && !misterfaxobeat.IsInDrawer())
                            {
                                misterfaxobeat.Interact();
                            }
                        }
                        else
                        {
                            hitFax = Physics2D.Raycast(draggedPaperwork.FaxTraceMarker.gameObject.transform.position, Vector2.down, Mathf.Infinity, layerMaskDrag);
                            if (hitFax.collider != null)
                            {
                                FaxMachine misterfaxobeat = hitFax.collider.gameObject.GetComponent<FaxMachine>();
                                if (misterfaxobeat != null)
                                {
                                    misterfaxobeat.Interact();
                                }
                            }
                        }

                    }
                    SaveManager.instance.MarkSavegameDirty();
                }
                CurrentDraggable = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugPanelHUD.instance.gameObject.activeSelf || EndDayConfirm.instance.gameObject.activeSelf)
        {
            return;
        }

        if (ElevatorManager.instance.bIsChangingScene)
        {
            if (CurrentDraggable != null)
            {
                CurrentDraggable = null;
            }
            if (LastHitInteractable != null)
            {
                LastHitInteractable.Unhover();
                LastHitInteractable = null;
            }
            if (MouseDownInteractable != null)
            {
                MouseDownInteractable = null;
            }
            HUDManager.instance.TextHoverShop.text = "";
            HUDManager.instance.TextHover.text = "";
            return;
        }

        //Cursor.visible = false;
        if (Input.GetKeyDown(KeyCode.F10) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionsManager.instance.gameObject.activeSelf)
            {
                OptionsManager.instance.OnBackClicked();
            }
            else if (GalleryScreen.instance.gameObject.activeSelf)
            {
                GalleryScreen.instance.OnBackClicked();
            }
            else
            {
                if (SceneManager.GetActiveScene().name != "Intro")
                {
                    IntroController.instance.Toggle();
                }
            }
        }


        //else
        //{
        //    SessionManager.instance.TickIdleTimer(Time.deltaTime);
        //}

        bool leftMouseUp = Input.GetMouseButtonUp(0);
        bool leftMouseDown = Input.GetMouseButtonDown(0);


        if (leftMouseUp)
        {
            SessionManager.instance.ResetIdleTimer();
        }

        if (SceneManager.GetActiveScene().name.Contains("Comic"))
        {
            if (!IsUIBlockingInput())
            {
                HandleComicInput();
            }
        }
        else
        {
            if (Application.isEditor || GameManager.instance.bDebugMode)
            {
                //HandleDebugInput();
            }

            if (IsUIBlockingInput())
            {
                HUDManager.instance.SetHoverText("");

                if (DialogueManager.instance.IsDialogueActive() && Input.GetMouseButtonUp(0))
                {
                    DialogueScreen.instance.RequestSkipDialogue();
                }

                return;
            }
            else
            {
                if (ElevatorManager.instance.GetCurrentScene() == EScene.Elevator)
                {
                    HandleElevatorInput();
                }

                HandleInteractableInput(leftMouseDown, leftMouseUp);
            }

        }
    }

    public void FrameLock()
    {
        StartCoroutine(FrameLockRoutine());
    }

    public IEnumerator FrameLockRoutine()
    {
        bFrameLock = true;
        yield return new WaitForEndOfFrame();
        bFrameLock = false;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        for (int i = 0; i < results.Count; ++i)
        {
            if (results[i].gameObject.GetComponent<FaxConfirm>() != null || results[i].gameObject.GetComponent<MarkConfirm>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool MouseScreenCheck(Vector3 mousePos)
    {
#if UNITY_EDITOR
        if (mousePos.x <= 0 || mousePos.y <= 0 || mousePos.x >= UnityEditor.Handles.GetMainGameViewSize().x - 1 || mousePos.y >= UnityEditor.Handles.GetMainGameViewSize().y - 1)
        {
            return false;
        }
#else
        if (mousePos.x <= 0 || mousePos.y <= 0 || mousePos.x >= Screen.width - 1 || mousePos.y >= Screen.height - 1) {
        return false;
        }
#endif
        else
        {
            return true;
        }
    }

}
