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
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ProfileState
{
    [SerializeField]
    public ulong ProfileID;
    [SerializeField]
    public EPaperworkMarkType MarkStatus;
}

[System.Serializable]
public class Vector3Serializeable
{
    public float x;
    public float y;
    public float z;

    public Vector3Serializeable()
    {

    }

    public Vector3Serializeable(Vector3 source)
    {
        this.x = source.x;
        this.y = source.y;
        this.z = source.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(Vector3Serializeable rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3Serializeable(Vector3 rValue)
    {
        return new Vector3Serializeable(rValue);
    }
}

[System.Serializable]
public class QuaternionSerializeable
{
    public float x;
    public float y;
    public float z;
    public float w;

    public QuaternionSerializeable()
    {

    }

    public QuaternionSerializeable(Quaternion source)
    {
        this.x = source.x;
        this.y = source.y;
        this.z = source.z;
        this.w = source.w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }

    /// <summary>
    /// Automatic conversion from SerializableQuaternion to Quaternion
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Quaternion(QuaternionSerializeable rValue)
    {
        return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }

    /// <summary>
    /// Automatic conversion from Quaternion to SerializableQuaternion
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator QuaternionSerializeable(Quaternion rValue)
    {
        return new QuaternionSerializeable(rValue);
    }
}

[System.Serializable]
public class DeskItemStatus
{
    [SerializeField]
    public Vector3Serializeable Position;
    [SerializeField]
    public ELeftOrRight DrawerStatus = ELeftOrRight.MAX;
}

[System.Serializable]
public class CarryoverPlayerState
{
    [SerializeField]
    public Dictionary<string, object> InventoryVars = new Dictionary<string, object>();
    [SerializeField]
    public Dictionary<string, object> ShopVars = new Dictionary<string, object>();
    [SerializeField]
    public List<ulong> OwnedHeads = new List<ulong>();
    [SerializeField]
    public List<ulong> OwnedBodies = new List<ulong>();
    [SerializeField]
    int CurrentHeadIndex = 0;
    [SerializeField]
    int CurrentBodyIndex = 0;
    [SerializeField]
    public Dictionary<string, DeskItemStatus> ItemPositions = new Dictionary<string, DeskItemStatus>();
    [SerializeField]
    bool bNewGamePlusEnabled = false;
    [SerializeField]
    bool bHasStartedGameOnce = false;
    [SerializeField]
    bool bHasFinishedGameOnce = false;
    [SerializeField]
    bool bHasFinishedGameOnceProperly = false;
    [SerializeField]
    bool bHasStartedGame = false;
    [SerializeField]
    bool bHasFinishedGame = false;
    [SerializeField]
    public int AmountOfEraserUses = 0;
    [SerializeField]
    public int AmountOfCoinFlips = 0;
    [SerializeField]
    public bool bDarwinAward = false;
    [SerializeField]
    public bool bRIPFashion = false;
    [SerializeField]
    public List<int> ShopKeeperHellos = new List<int>();

    [SerializeField]
    public List<ulong> DoomedProfiles = new List<ulong>();
    [SerializeField]
    public List<ulong> SparedProfiles = new List<ulong>();
    [SerializeField]
    public int NewGamePlusCount = 1;
    [SerializeField]
    public int EraserBuyCount = 1;
    [SerializeField]
    public bool bEraserLocationReset = false;
    [SerializeField]
    public int FateAttentionProfileIndex = -1;

    [SerializeField]
    public bool bPeaceHigh = false;
    [SerializeField]
    public bool bProsperityHigh = false;
    [SerializeField]
    public bool bEcologyHigh = false;
    [SerializeField]
    public bool bHealthHigh = false;
    [SerializeField]
    public bool bPeaceLow = false;
    [SerializeField]
    public bool bProsperityLow = false;
    [SerializeField]
    public bool bEcologyLow = false;
    [SerializeField]
    public bool bHealthLow = false;
    [SerializeField]
    public bool bChaosLow = false;
    [SerializeField]
    public bool bChaosMid = false;
    [SerializeField]
    public bool bChaosHigh = false;
    [SerializeField]
    public bool bPersonalPet = false;
    [SerializeField]
    public bool bPersonalFired = false;
    [SerializeField]
    public bool bPersonalTakeover = false;
    [SerializeField]
    public bool bPersonalMurder = false;
    [SerializeField]
    public ECustomName CustomName = ECustomName.DeathAndTaxes;
    [SerializeField]
    bool bHasGalleryUpdate = false;
    [SerializeField]
    public EDesktopMusic CurrentDesktopMusic = EDesktopMusic.Normal;
    [SerializeField]
    int TaskDeviationCounter = 0;

    public int GetTaskDeviationCounter()
    {
        return TaskDeviationCounter;
    }

    public void ResetTaskDeviationCounter()
    {
        TaskDeviationCounter = 0;
    }

    public void IncrementTaskDeviationCounter()
    {
        TaskDeviationCounter++;
    }

    public bool HasGalleryNotification()
    {
        return bHasGalleryUpdate;
    }

    public void SetHasGalleryNotification(bool val)
    {
        bHasGalleryUpdate = val;
        GalleryNotification.instance.ToggleVisible(val);
        SaveManager.instance.MarkSavegameDirty();
    }
    public void IncrementEraserBuyCount()
    {
        EraserBuyCount++;
        SaveManager.instance.MarkSavegameDirty();
    }

    public void AddDoomedProfile(ulong id)
    {
        if (!HasProfileBeenDoomedBefore(id))
        {
            DoomedProfiles.Add(id);
            SaveManager.instance.MarkSavegameDirty();
        }
    }

    public void AddSparedProfile(ulong id)
    {
        if (!HasProfileBeenSparedBefore(id))
        {
            SparedProfiles.Add(id);
            SaveManager.instance.MarkSavegameDirty();
        }
    }

    public bool HasProfileBeenDoomedBefore(ulong id)
    {
        return DoomedProfiles.Contains(id);
    }

    public bool HasProfileBeenSparedBefore(ulong id)
    {
        return SparedProfiles.Contains(id);
    }

    public void AddShopKeeperHello(int dayIndex)
    {
        if (!ShopKeeperHellos.Contains(dayIndex))
        {
            ShopKeeperHellos.Add(dayIndex);
        }
    }

    public void SetGameFinishedOnce(bool finished)
    {
        bHasFinishedGameOnce = finished;
        SaveManager.instance.MarkSavegameDirty();
    }

    public bool HasGameFinishedOnce()
    {
        return bHasFinishedGameOnce;
    }

    public void SetGameFinishedOnceProperly(bool finished)
    {
        bHasFinishedGameOnceProperly = finished;
        SaveManager.instance.MarkSavegameDirty();
    }

    public bool HasGameFinishedOnceProperly()
    {
        return bHasFinishedGameOnceProperly;
    }

    public void SetGameFinished(bool finished)
    {
        if(finished && bHasFinishedGame != finished)
        {
            SaveManager.instance.GetCurrentCarryoverPlayerState().NewGamePlusCount++;
        }
        bHasFinishedGame = finished;
        SaveManager.instance.MarkSavegameDirty();
    }

    public bool HasGameFinished()
    {
        return bHasFinishedGame;
    }

    public void SetGameStarted(bool started)
    {
        bHasStartedGame = started;
        SaveManager.instance.MarkSavegameDirty();
    }

    public bool HasGameStarted()
    {
        return bHasStartedGame;
    }

    public void SetGameStartedOnce(bool started)
    {
        bHasStartedGameOnce = started;
        SaveManager.instance.MarkSavegameDirty();
    }

    public bool HasGameStartedOnce()
    {
        return bHasStartedGameOnce;
    }

    public bool IsNewGamePlusEnabled()
    {
        return bNewGamePlusEnabled;
    }

    public void SetNewGamePlusEnabled(bool val)
    {
        bNewGamePlusEnabled = val;
        SaveManager.instance.MarkSavegameDirty();
    }

    public void IncrementHeadIndex()
    {
        CurrentHeadIndex++;
        if (CurrentHeadIndex >= OwnedHeads.Count)
        {
            CurrentHeadIndex = 0;
        }


        UpdateArticyCurrentHead();

        SaveManager.instance.MarkSavegameDirty();
    }

    public void UpdateArticyCurrentHead()
    {
        template_item parentItem = GetCurrentHeadItem().Template.item_accessory_variation.item_parent_reference as template_item;

        if (parentItem.Template.item_data.item_name.Contains("THE FACE OF THE GREAT OLD ONE"))
        {
            ArticyGlobalVariables.Default.inventory.current_head = "cthulhu";
        }
        else if (parentItem.Template.item_data.item_name.Contains("REGULAR BONEHEAD"))
        {
            ArticyGlobalVariables.Default.inventory.current_head = "bonehead";
        }
        else if (parentItem.Template.item_data.item_name.Contains("THE CALAVERA"))
        {
            ArticyGlobalVariables.Default.inventory.current_head = "sugarskull";
        }
        else if (parentItem.Template.item_data.item_name.Contains("THE PLAGUE DOCTOR"))
        {
            ArticyGlobalVariables.Default.inventory.current_head = "plague";
        }
        else if (parentItem.Template.item_data.item_name.Contains("EYE OF ANPU"))
        {
            ArticyGlobalVariables.Default.inventory.current_head = "anubis";
        }
        else if (parentItem.Template.item_data.item_name.Contains("EARS OF THE BEAST"))
        {
            ArticyGlobalVariables.Default.inventory.current_head = "kitty";
        }
        else if (parentItem.Template.item_data.item_name.Contains("UNHOLY SMOKES"))
        {
            ArticyGlobalVariables.Default.inventory.current_head = "john";
        }
        else if (parentItem.Template.item_data.item_name.Contains("ACADEMIC VISAGE"))
        {
            ArticyGlobalVariables.Default.inventory.current_head = "jane";
        }

        Debug.Log("Current Head: " + ArticyGlobalVariables.Default.inventory.current_head);
    }

    public string GetCurrentHeadMagicName()
    {
        return ArticyGlobalVariables.Default.inventory.current_head;
    }

    public string GetCurrentBodyMagicName()
    {
        return ArticyGlobalVariables.Default.inventory.current_body;
    }

    public void DecrementHeadIndex()
    {
        CurrentHeadIndex--;
        if (CurrentHeadIndex < 0)
        {
            CurrentHeadIndex = OwnedHeads.Count - 1;
        }


        UpdateArticyCurrentHead();

        SaveManager.instance.MarkSavegameDirty();
    }

    public Sprite GetCurrentHeadAsset()
    {
        if(OwnedHeads.Count == 0)
        {
            return null;
        }
        return ShopManager.instance.GetHeadAssetByID(OwnedHeads[CurrentHeadIndex]);
    }

    public template_item_variation GetCurrentHeadItem()
    {
        if (OwnedHeads.Count == 0)
        {
            return null;
        }
        return ShopManager.instance.GetHeadByID(OwnedHeads[CurrentHeadIndex]);
    }

    public void IncrementBodyIndex()
    {
        CurrentBodyIndex++;
        if (CurrentBodyIndex >= OwnedBodies.Count)
        {
            CurrentBodyIndex = 0;
        }


        UpdateArticyCurrentBody();

        SaveManager.instance.MarkSavegameDirty();
    }

    public void UpdateArticyCurrentBody()
    {
        template_item parentItem = GetCurrentBodyItem().Template.item_accessory_variation.item_parent_reference as template_item;
        if (parentItem.Template.item_data.item_name.Contains("GRIM CAPE"))
        {
            ArticyGlobalVariables.Default.inventory.current_body = "cape";
        }
        else if (parentItem.Template.item_data.item_name.Contains("A FANCY SUIT (REGULAR EDITION)"))
        {
            ArticyGlobalVariables.Default.inventory.current_body = "tie";
        }
        else if (parentItem.Template.item_data.item_name.Contains("A FANCY SUIT (BOWTIE EDITION)"))
        {
            ArticyGlobalVariables.Default.inventory.current_body = "bowtie";
        }

        Debug.Log("Current Body: " + ArticyGlobalVariables.Default.inventory.current_body);
    }

    public void DecrementBodyIndex()
    {
        CurrentBodyIndex--;
        if (CurrentBodyIndex < 0)
        {
            CurrentBodyIndex = OwnedBodies.Count - 1;
        }


        UpdateArticyCurrentBody();

        SaveManager.instance.MarkSavegameDirty();
    }

    public Sprite GetCurrentBodyAsset()
    {
        if (OwnedBodies.Count == 0)
        {
            return null;
        }
        return ShopManager.instance.GetBodyAssetByID(OwnedBodies[CurrentBodyIndex]);
    }

    public template_item_variation GetCurrentBodyItem()
    {
        if (OwnedBodies.Count == 0)
        {
            return null;
        }
        return ShopManager.instance.GetBodyByID(OwnedBodies[CurrentBodyIndex]);
    }

    public void AddOwnedHeadID(ulong lookID)
    {
        if (!OwnedHeads.Contains(lookID))
        {
            OwnedHeads.Add(lookID);
        }
        SaveManager.instance.MarkSavegameDirty();
    }

    public void AddOwnedBodyID(ulong lookID)
    {
        if (!OwnedBodies.Contains(lookID))
        {
            OwnedBodies.Add(lookID);
        }
        SaveManager.instance.MarkSavegameDirty();
    }
}

[System.Serializable]
public class PlayerState
{
    // Game State
    [SerializeField]
    int SpawnCounter = 0;
    [SerializeField]
    int DemoRestartCounter = 0;
    [SerializeField]
    private int DayIndex = 0;
    [SerializeField]
    bool bIntroDone = false;
    [SerializeField]
    bool bDayDone = false;
    [SerializeField]
    bool bFaxSent = false;

    [SerializeField]
    bool bShopkeeperVisited = false;

    [SerializeField]
    EScene LastOpenScene;
    [SerializeField]
    public int LetterTextIndex = 0;
    [SerializeField]
    public int LetterOpenSpriteIndex = 0;
    [SerializeField]
    public int LetterClosedSpriteIndex = 0;
    [SerializeField]
    public float ElevatorHeight = 0.0f;

    // Articy State
    [SerializeField]
    List<ProfileState> CurrentDayProfiles = new List<ProfileState>();
    [SerializeField]
    Dictionary<int, List<ulong>> DailyProfileIDs = new Dictionary<int, List<ulong>>();
    [SerializeField]
    List<ulong> CurrentDayItems = new List<ulong>();
    [SerializeField]
    ulong CurrentDay;


    [SerializeField]
    bool bHasOfficeShopNotification = false;
    [SerializeField]
    bool bHasMirrorShopNotification = false;

    [SerializeField]
    List<ulong> RolledRandos = new List<ulong>();
    [SerializeField]
    List<ulong> EnabledProfiles = new List<ulong>();
    [SerializeField]
    List<ulong> DisabledProfiles = new List<ulong>();

    [SerializeField]
    Dictionary<string, object> ArticyVariables = null;

    [SerializeField]
    List<string> SavedNews = new List<string>();
    [SerializeField]
    public bool bRuleBender = true;

    [SerializeField]
    bool bHasMirrorNotification = false;

    [SerializeField]
    public bool bSentGalleryUpdate = false;

    [SerializeField]
    bool bHasGalleryUpdate = false;



    //[SerializeField]
    //public List<ulong> DoomedProfilesCurrentPlaythrough = new List<ulong>();
    //[SerializeField]
    //public List<ulong> SparedProfilesCurrentPlaythrough = new List<ulong>();


    public PlayerState()
    {

    }

    //public void TransferAllMarkedProfilesToCarryOver()
    //{
    //    for (int i = 0; i < DoomedProfilesCurrentPlaythrough.Count; ++i)
    //    {
    //        SaveManager.instance.GetCurrentCarryoverPlayerState().AddDoomedProfile(DoomedProfilesCurrentPlaythrough[i]);
    //    }

    //    for (int i = 0; i < SparedProfilesCurrentPlaythrough.Count; ++i)
    //    {
    //        SaveManager.instance.GetCurrentCarryoverPlayerState().AddSparedProfile(SparedProfilesCurrentPlaythrough[i]);
    //    }

    //    SaveManager.instance.MarkSavegameDirty();
    //}

    //public void AddDoomedProfile(ulong id)
    //{
    //    if (!HasProfileBeenDoomedBefore(id))
    //    {
    //        DoomedProfilesCurrentPlaythrough.Add(id);
    //    }

    //    SaveManager.instance.MarkSavegameDirty();
    //}

    //public void AddSparedProfile(ulong id)
    //{
    //    if (!HasProfileBeenSparedBefore(id))
    //    {
    //        SparedProfilesCurrentPlaythrough.Add(id);
    //    }

    //    SaveManager.instance.MarkSavegameDirty();
    //}

    //public bool HasProfileBeenDoomedBefore(ulong id)
    //{
    //    return DoomedProfilesCurrentPlaythrough.Contains(id);
    //}

    //public bool HasProfileBeenSparedBefore(ulong id)
    //{
    //    return SparedProfilesCurrentPlaythrough.Contains(id);
    //}

    public bool IsProfileEnabled(template_profile profile)
    {
        return EnabledProfiles.Contains(profile.Id);
    }

    public bool HasOfficeShopNotification()
    {
        return bHasOfficeShopNotification;
    }

    public bool HasMirrorNotification()
    {
        return bHasMirrorNotification;
    }

    public bool HasMirrorShopNotification()
    {
        return bHasMirrorShopNotification;
    }

    public void SetHasMirrorNotification(bool val)
    {
        bHasMirrorNotification = val;
        MirrorNotification.instance.ToggleVisible(val);
        SaveManager.instance.MarkSavegameDirty();
    }

    public void SetHasOfficeShopNotification(bool val)
    {
        bHasOfficeShopNotification = val;
        ShopNotification.instanceOffice.ToggleVisible(val);
        SaveManager.instance.MarkSavegameDirty();
    }

    public void SetHasMirrorShopNotification(bool val)
    {
        bHasMirrorShopNotification = val;
        ShopNotification.instanceMirror.ToggleVisible(val);
        SaveManager.instance.MarkSavegameDirty();
    }

    public void RestoreNewsText()
    {
        for (int i = 0; i < SavedNews.Count; ++i)
        {
            Phone.instance.AddNewsText(SavedNews[i], false, true);
        }
    }

    public void AddNewsText(string text, bool addToStart = false)
    {
        if (text != "")
        {
            if (addToStart)
            {
                SavedNews.Insert(0, text);
            }
            else
            {
                SavedNews.Add(text);
            }
        }
    }

    public void ClearSavedNews()
    {
        SavedNews.Clear();
    }

    public bool HasVisitedShopkeeperToday()
    {
        return bShopkeeperVisited;
    }

    public void SetHasVisitedShopkeeperToday(bool val)
    {
        bShopkeeperVisited = val;
    }

    public List<ulong> GetDailyProfileIDs(int day)
    {
        return DailyProfileIDs[day];
    }

    public void AddDailyProfiles(int day, List<ulong> profiles)
    {
        DailyProfileIDs.Add(day, profiles);
    }



    public void SaveArticyVariables()
    {
        ArticyVariables = ArticyDatabase.DefaultGlobalVariables.Variables;
        //SaveManager.instance.MarkSavegameDirty();
    }

    public template_day GetCurrentDaySaved()
    {
        return ArticyDatabase.GetObject(CurrentDay) as template_day;
    }

    public void SetCurrentDaySaved(ulong id)
    {
        CurrentDay = id;
        SaveManager.instance.MarkSavegameDirty();
    }

    public void LoadArticyState()
    {
        if (ArticyVariables != null)
        {
            ArticyDatabase.DefaultGlobalVariables.Variables = ArticyVariables;

            for (int i = 0; i < EnabledProfiles.Count; ++i)
            {
                template_profile profile = ArticyDatabase.GetObject(EnabledProfiles[i]) as template_profile;
                profile.Template.profile_basic_data.profile_is_enabled = true;
            }

            for (int i = 0; i < DisabledProfiles.Count; ++i)
            {
                template_profile profile = ArticyDatabase.GetObject(DisabledProfiles[i]) as template_profile;
                profile.Template.profile_basic_data.profile_is_enabled = false;
            }

        }
    }

    public void AddEnabledProfile(ulong id)
    {
        if (!EnabledProfiles.Contains(id))
        {
            EnabledProfiles.Add(id);
        }
        SaveManager.instance.MarkSavegameDirty();
    }

    public void AddDisabledProfile(ulong id)
    {
        if (!DisabledProfiles.Contains(id))
        {
            DisabledProfiles.Add(id);
        }
        SaveManager.instance.MarkSavegameDirty();
    }

    public List<ulong> GetCurrentDayItems()
    {
        return CurrentDayItems;
    }

    public void AddCurrentDayItem(ulong id)
    {
        CurrentDayItems.Add(id);
        SaveManager.instance.MarkSavegameDirty();
    }

    public void RemoveCurrentDayItem(ulong id)
    {
        CurrentDayItems.Remove(id);
    }

    public void ClearCurrentDayItems()
    {
        CurrentDayItems.Clear();
    }

    public int GetRestartCounter()
    {
        return DemoRestartCounter;
    }

    public void IncrementRestartCounter()
    {
        DemoRestartCounter++;
        Debug.Log("Restart Counter: " + DemoRestartCounter);
        //SaveManager.instance.Save();
    }

    public void SetRestartCounter(int counter)
    {
        DemoRestartCounter = counter;
        Debug.Log("Restart Counter: " + DemoRestartCounter);
        //SaveManager.instance.Save();
    }

    public bool IsIntroDone()
    {
        return bIntroDone;
    }

    public void SetIntroDone(bool isDone)
    {
        bIntroDone = isDone;
    }

    public bool IsDayDone()
    {
        return bDayDone;
    }

    public void SetDayDone(bool dayDone)
    {
        bDayDone = dayDone;
        Elevator.instance.GetElevatorButtonBySceneType(EScene.Elevator).ToggleEnable(dayDone);
        //Bed.instance.NotifyBedDayDone(dayDone);
        if (!dayDone)
        {
            SaveManager.instance.GetCurrentPlayerState().SetFaxSent(false);
        }
        else
        {

        }
        SaveManager.instance.MarkSavegameDirty();
    }

    public bool IsFaxSent()
    {
        return bFaxSent;
    }

    public void SetFaxSent(bool faxSent)
    {
        bFaxSent = faxSent;

        if (faxSent)
        {
            Elevator.instance.SetNightSprites();
        }
        else
        {
            Elevator.instance.SetDaySprites();
        }
        SaveManager.instance.MarkSavegameDirty();
    }

    public EScene GetLastOpenScene()
    {
        return LastOpenScene;
    }

    public void SetLastOpenScene(EScene scene)
    {
        LastOpenScene = scene;
        SaveManager.instance.MarkSavegameDirty();
    }

    public int GetCurrentDayIndex()
    {
        return DayIndex;
    }

    public int GetCurrentDayNumberNotIndexThisHasOneAddedToIt()
    {
        return DayIndex + 1;
    }

    public void IncrementDayIndex()
    {
        DayIndex++;

        GameManager.instance.NotifyEndDay();
    }

    public int GetStat(EStat stat)
    {
        switch (stat)
        {
            case EStat.LOYALTY:
                return ArticyGlobalVariables.Default.rep.loyalty;
            case EStat.DEATH_TOTAL:
                return ArticyGlobalVariables.Default.rep.death_total;
            case EStat.DEATH_DAILY:
                return ArticyGlobalVariables.Default.day.death_count;
            case EStat.ECOLOGY:
                return ArticyGlobalVariables.Default.rep.ecology;
            case EStat.HEALTH:
                return ArticyGlobalVariables.Default.rep.health;
            case EStat.PROSPERITY:
                return ArticyGlobalVariables.Default.rep.prosperity;
            case EStat.PEACE:
                return ArticyGlobalVariables.Default.rep.peace;
            case EStat.SPARE_TOTAL:
                return ArticyGlobalVariables.Default.rep.spare_total;
            case EStat.SPARE_DAILY:
                return ArticyGlobalVariables.Default.day.spare_count;
            case EStat.ECOLOGY_DAILY:
                return ArticyGlobalVariables.Default.day.ecology_change;
            case EStat.HEALTH_DAILY:
                return ArticyGlobalVariables.Default.day.health_change;
            case EStat.PROSPERITY_DAILY:
                return ArticyGlobalVariables.Default.day.prosperity_change;
            case EStat.PEACE_DAILY:
                return ArticyGlobalVariables.Default.day.peace_change;
        }

        Debug.LogError("Stat " + stat.ToString() + " did not return correct value!");
        return 0;
    }

    public void ModifyStat(EStat stat, int amount)
    {
        if (amount != 0)
        {
            switch (stat)
            {
                case EStat.LOYALTY:
                    ArticyGlobalVariables.Default.rep.loyalty += amount;
                    break;
                case EStat.DEATH_TOTAL:
                    ArticyGlobalVariables.Default.rep.death_total += amount;
                    break;
                case EStat.DEATH_DAILY:
                    ArticyGlobalVariables.Default.day.death_count += amount;
                    if (ArticyGlobalVariables.Default.day.death_count >= DesktopManager.instance.GetProfileCountForCurrentDay())
                    {
                        ArticyGlobalVariables.Default.day.death_count = -1; //all
                    }
                    break;
                case EStat.MAX:
                    break;
                case EStat.ECOLOGY:
                    ArticyGlobalVariables.Default.rep.ecology += amount;
                    break;
                case EStat.ECOLOGY_DAILY:
                    ArticyGlobalVariables.Default.day.ecology_change += amount;
                    break;
                case EStat.HEALTH:
                    ArticyGlobalVariables.Default.rep.health += amount;
                    break;
                case EStat.HEALTH_DAILY:
                    ArticyGlobalVariables.Default.day.health_change += amount;
                    break;
                case EStat.PROSPERITY:
                    ArticyGlobalVariables.Default.rep.prosperity += amount;
                    break;
                case EStat.PROSPERITY_DAILY:
                    ArticyGlobalVariables.Default.day.prosperity_change += amount;
                    break;
                case EStat.PEACE:
                    ArticyGlobalVariables.Default.rep.peace += amount;
                    break;
                case EStat.PEACE_DAILY:
                    ArticyGlobalVariables.Default.day.peace_change += amount;
                    break;
                case EStat.SPARE_TOTAL:
                    ArticyGlobalVariables.Default.rep.spare_total += amount;
                    break;
                case EStat.SPARE_DAILY:
                    ArticyGlobalVariables.Default.day.spare_count += amount;
                    if (ArticyGlobalVariables.Default.day.spare_count >= DesktopManager.instance.GetProfileCountForCurrentDay())
                    {
                        ArticyGlobalVariables.Default.day.spare_count = -1; //all
                    }
                    break;
            }
            Debug.Log("Modified stat " + stat.ToString() + " by " + amount.ToString() + " from " + (GetStat(stat) - amount).ToString() + " to " + GetStat(stat).ToString());
        }
        SaveManager.instance.MarkSavegameDirty();
    }

    public void ResetStat(EStat stat)
    {
        switch (stat)
        {
            case EStat.LOYALTY:
                ArticyGlobalVariables.Default.rep.loyalty = 0;
                break;
            case EStat.DEATH_TOTAL:
                ArticyGlobalVariables.Default.rep.death_total = 0;
                break;
            case EStat.DEATH_DAILY:
                ArticyGlobalVariables.Default.day.death_count = 0;
                break;
            case EStat.MAX:
                break;
            case EStat.ECOLOGY:
                ArticyGlobalVariables.Default.rep.ecology = 0;
                break;
            case EStat.HEALTH:
                ArticyGlobalVariables.Default.rep.health = 0;
                break;
            case EStat.PROSPERITY:
                ArticyGlobalVariables.Default.rep.prosperity = 0;
                break;
            case EStat.PEACE:
                ArticyGlobalVariables.Default.rep.peace = 0;
                break;
            case EStat.SPARE_TOTAL:
                ArticyGlobalVariables.Default.rep.spare_total = 0;
                break;
            case EStat.SPARE_DAILY:
                ArticyGlobalVariables.Default.day.spare_count = 0;
                break;
            case EStat.ECOLOGY_DAILY:
                ArticyGlobalVariables.Default.day.ecology_change = 0;
                break;
            case EStat.HEALTH_DAILY:
                ArticyGlobalVariables.Default.day.health_change = 0;
                break;
            case EStat.PROSPERITY_DAILY:
                ArticyGlobalVariables.Default.day.prosperity_change = 0;
                break;
            case EStat.PEACE_DAILY:
                ArticyGlobalVariables.Default.day.peace_change = 0;
                break;
        }
    }

    public int GetSpawnCounter()
    {
        return SpawnCounter;
    }

    public void IncrementSpawnCounter()
    {
        SpawnCounter++;
        if(SpawnCounter >= 100)
        {
            SpawnCounter = 10;
        }
        Debug.Log("Spawn Counter: " + SpawnCounter);
        SaveManager.instance.MarkSavegameDirty();
        //SaveManager.instance.Save();
    }

    public void SetSpawnCounter(int counter)
    {
        SpawnCounter = counter;
        Debug.Log("Spawn Counter: " + SpawnCounter);
        SaveManager.instance.MarkSavegameDirty();
        //SaveManager.instance.Save();
    }

    public void AddRolledRandoID(ulong randoID)
    {
        RolledRandos.Add(randoID);
        SaveManager.instance.MarkSavegameDirty();
    }

    public bool IsRandoRolledByID(ulong randoID)
    {
        return RolledRandos.Contains(randoID);
    }

    public List<ulong> GetAlreadyRolledRandoIDs()
    {
        return RolledRandos;
    }



    public bool CanAfford(int darksouls)
    {
        return ArticyGlobalVariables.Default.inventory.money >= darksouls;
    }

    public void ReleaseBackpay()
    {
        ArticyGlobalVariables.Default.game.backpay = false;

        ArticyGlobalVariables.Default.inventory.money += ArticyGlobalVariables.Default.game.backpay_amount;
        GrimDesk.instance.SpawnSalary(ArticyGlobalVariables.Default.game.backpay_amount);
        ArticyGlobalVariables.Default.game.backpay_amount = 0;
        HUDManager.instance.UpdateMoney();
        SaveManager.instance.MarkSavegameDirty();
    }

    public void ModifyMoney(int amount)
    {
        if (amount > 0)
        {
            if (ArticyGlobalVariables.Default.game.salary_on)
            {
                if(ArticyGlobalVariables.Default.inventory.money < 10000)
                {
                    ArticyGlobalVariables.Default.inventory.money += amount;
                    GrimDesk.instance.SpawnSalary(amount);
                    Debug.Log("Added " + amount + " darksouls");
                }
                else
                {
                    Debug.Log("Taxes engaged: " + amount + " not added to darksouls pool due to being over deposit limit");
                }
            }
            else
            {
                ArticyGlobalVariables.Default.game.backpay_amount += amount;
                Debug.Log("Added to backpay pool: " + amount);
            }
        }
        else
        {
            ArticyGlobalVariables.Default.inventory.money += amount;
            GrimDesk.instance.SpendSalary(amount);
            Debug.Log("Spent " + amount + " darksouls");
        }

        HUDManager.instance.UpdateMoney();
        SaveManager.instance.MarkSavegameDirty();
    }



    public void SetCurrentDayProfiles(List<template_profile> profiles)
    {
        CurrentDayProfiles.Clear();
        for (int i = 0; i < profiles.Count; ++i)
        {
            ProfileState state = new ProfileState();
            state.ProfileID = profiles[i].Id;
            state.MarkStatus = EPaperworkMarkType.Unmarked;
            CurrentDayProfiles.Add(state);
        }
        SaveManager.instance.MarkSavegameDirty();
    }

    public EPaperworkMarkType GetProfileMarkStatusByID(ulong id)
    {
        for (int i = 0; i < CurrentDayProfiles.Count; ++i)
        {
            if (CurrentDayProfiles[i].ProfileID == id)
            {
                return CurrentDayProfiles[i].MarkStatus;
            }
        }

        Debug.LogError("Didn't find profile status! " + id);

        return EPaperworkMarkType.Unmarked;
    }

    public void SetProfileMarkStatusByID(ulong id, EPaperworkMarkType status)
    {
        for (int i = 0; i < CurrentDayProfiles.Count; ++i)
        {
            if (CurrentDayProfiles[i].ProfileID == id)
            {
                CurrentDayProfiles[i].MarkStatus = status;
                SaveManager.instance.MarkSavegameDirty();
                return;
            }
        }

        Debug.LogError("Didn't find profile status! " + id);
    }

    public List<ProfileState> GetCurrentDayProfiles()
    {
        return CurrentDayProfiles;
    }

    //DISGUSTIIING
    public KeyValuePair<string, int> GetWorstParameter()
    {
        KeyValuePair<string, int> worstParameter = new KeyValuePair<string, int>("health", ArticyGlobalVariables.Default.rep.health);

        if (ArticyGlobalVariables.Default.rep.peace <= worstParameter.Value)
        {
            worstParameter = new KeyValuePair<string, int>("peace", ArticyGlobalVariables.Default.rep.peace);
        }
        if (ArticyGlobalVariables.Default.rep.prosperity <= worstParameter.Value)
        {
            worstParameter = new KeyValuePair<string, int>("prosperity", ArticyGlobalVariables.Default.rep.prosperity);
        }
        if (ArticyGlobalVariables.Default.rep.ecology <= worstParameter.Value)
        {
            worstParameter = new KeyValuePair<string, int>("ecology", ArticyGlobalVariables.Default.rep.ecology);
        }

        return worstParameter;
    }

    //DISGUSTIIING
    public KeyValuePair<string, int> GetBestParameter()
    {
        KeyValuePair<string, int> bestParameter = new KeyValuePair<string, int>("health", ArticyGlobalVariables.Default.rep.health);

        if (ArticyGlobalVariables.Default.rep.peace >= bestParameter.Value)
        {
            bestParameter = new KeyValuePair<string, int>("peace", ArticyGlobalVariables.Default.rep.peace);
        }
        if (ArticyGlobalVariables.Default.rep.prosperity >= bestParameter.Value)
        {
            bestParameter = new KeyValuePair<string, int>("prosperity", ArticyGlobalVariables.Default.rep.prosperity);
        }
        if (ArticyGlobalVariables.Default.rep.ecology >= bestParameter.Value)
        {
            bestParameter = new KeyValuePair<string, int>("ecology", ArticyGlobalVariables.Default.rep.ecology);
        }

        return bestParameter;
    }

    //DISGUSTIIING
    public KeyValuePair<string, int> GetSecondBestParameter()
    {
        KeyValuePair<string, int> bestParameter = GetBestParameter();
        if (bestParameter.Key == "peace")
        {
            KeyValuePair<string, int> secondBestParameter = new KeyValuePair<string, int>("health", ArticyGlobalVariables.Default.rep.health);
            if (ArticyGlobalVariables.Default.rep.prosperity >= bestParameter.Value)
            {
                secondBestParameter = new KeyValuePair<string, int>("prosperity", ArticyGlobalVariables.Default.rep.prosperity);
            }
            if (ArticyGlobalVariables.Default.rep.ecology >= bestParameter.Value)
            {
                secondBestParameter = new KeyValuePair<string, int>("ecology", ArticyGlobalVariables.Default.rep.ecology);
            }
            return secondBestParameter;
        }
        else if (bestParameter.Key == "prosperity")
        {
            KeyValuePair<string, int> secondBestParameter = new KeyValuePair<string, int>("health", ArticyGlobalVariables.Default.rep.health);
            if (ArticyGlobalVariables.Default.rep.peace >= bestParameter.Value)
            {
                secondBestParameter = new KeyValuePair<string, int>("peace", ArticyGlobalVariables.Default.rep.peace);
            }
            if (ArticyGlobalVariables.Default.rep.ecology >= bestParameter.Value)
            {
                secondBestParameter = new KeyValuePair<string, int>("ecology", ArticyGlobalVariables.Default.rep.ecology);
            }
            return secondBestParameter;
        }
        else if (bestParameter.Key == "health")
        {
            KeyValuePair<string, int> secondBestParameter = new KeyValuePair<string, int>("prosperity", ArticyGlobalVariables.Default.rep.prosperity);
            if (ArticyGlobalVariables.Default.rep.peace >= bestParameter.Value)
            {
                secondBestParameter = new KeyValuePair<string, int>("peace", ArticyGlobalVariables.Default.rep.peace);
            }
            if (ArticyGlobalVariables.Default.rep.ecology >= bestParameter.Value)
            {
                secondBestParameter = new KeyValuePair<string, int>("ecology", ArticyGlobalVariables.Default.rep.ecology);
            }
            return secondBestParameter;
        }
        else if (bestParameter.Key == "ecology")
        {
            KeyValuePair<string, int> secondBestParameter = new KeyValuePair<string, int>("prosperity", ArticyGlobalVariables.Default.rep.prosperity);
            if (ArticyGlobalVariables.Default.rep.peace >= bestParameter.Value)
            {
                secondBestParameter = new KeyValuePair<string, int>("peace", ArticyGlobalVariables.Default.rep.peace);
            }
            if (ArticyGlobalVariables.Default.rep.health >= bestParameter.Value)
            {
                secondBestParameter = new KeyValuePair<string, int>("health", ArticyGlobalVariables.Default.rep.health);
            }
            return secondBestParameter;
        }
        else
        {
            Debug.LogError("CRITICAL BEST PARAM FAILURE - CALL THE OTT");
        }

        return bestParameter;
    }

    //DISGUSTIIING
    public KeyValuePair<string, int> GetSecondWorstParameter()
    {
        KeyValuePair<string, int> worstParameter = GetWorstParameter();
        if (worstParameter.Key == "peace")
        {
            KeyValuePair<string, int> secondWorstParameter = new KeyValuePair<string, int>("health", ArticyGlobalVariables.Default.rep.health);
            if (ArticyGlobalVariables.Default.rep.prosperity <= worstParameter.Value)
            {
                secondWorstParameter = new KeyValuePair<string, int>("prosperity", ArticyGlobalVariables.Default.rep.prosperity);
            }
            if (ArticyGlobalVariables.Default.rep.ecology <= worstParameter.Value)
            {
                secondWorstParameter = new KeyValuePair<string, int>("ecology", ArticyGlobalVariables.Default.rep.ecology);
            }
            return secondWorstParameter;
        }
        else if (worstParameter.Key == "prosperity")
        {
            KeyValuePair<string, int> secondWorstParameter = new KeyValuePair<string, int>("health", ArticyGlobalVariables.Default.rep.health);
            if (ArticyGlobalVariables.Default.rep.peace <= worstParameter.Value)
            {
                secondWorstParameter = new KeyValuePair<string, int>("peace", ArticyGlobalVariables.Default.rep.peace);
            }
            if (ArticyGlobalVariables.Default.rep.ecology <= worstParameter.Value)
            {
                secondWorstParameter = new KeyValuePair<string, int>("ecology", ArticyGlobalVariables.Default.rep.ecology);
            }
            return secondWorstParameter;
        }
        else if (worstParameter.Key == "health")
        {
            KeyValuePair<string, int> secondWorstParameter = new KeyValuePair<string, int>("prosperity", ArticyGlobalVariables.Default.rep.prosperity);
            if (ArticyGlobalVariables.Default.rep.peace <= worstParameter.Value)
            {
                secondWorstParameter = new KeyValuePair<string, int>("peace", ArticyGlobalVariables.Default.rep.peace);
            }
            if (ArticyGlobalVariables.Default.rep.ecology <= worstParameter.Value)
            {
                secondWorstParameter = new KeyValuePair<string, int>("ecology", ArticyGlobalVariables.Default.rep.ecology);
            }
            return secondWorstParameter;
        }
        else if (worstParameter.Key == "ecology")
        {
            KeyValuePair<string, int> secondWorstParameter = new KeyValuePair<string, int>("prosperity", ArticyGlobalVariables.Default.rep.prosperity);
            if (ArticyGlobalVariables.Default.rep.peace <= worstParameter.Value)
            {
                secondWorstParameter = new KeyValuePair<string, int>("peace", ArticyGlobalVariables.Default.rep.peace);
            }
            if (ArticyGlobalVariables.Default.rep.health <= worstParameter.Value)
            {
                secondWorstParameter = new KeyValuePair<string, int>("health", ArticyGlobalVariables.Default.rep.health);
            }
            return secondWorstParameter;
        }
        else
        {
            Debug.LogError("CRITICAL WORST PARAM FAILURE - CALL THE OTT");
        }

        return worstParameter;
    }

}
