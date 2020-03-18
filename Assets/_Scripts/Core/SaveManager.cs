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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : ManagerBase
{

    public static SaveManager instance;

    SaveData LocalCopyOfData = null;
    public Options CurrentOptions = new Options();

    private void Awake()
    {
        instance = this;
    }

    bool bIsLoading = false;
    bool bIsSaving = false;
    bool bSaveNextFrame = false;

    [System.Serializable]
    public class SaveData
    {
        public System.DateTime LastPlayedTime = System.DateTime.Now;
        public string Version = "";
        public PlayerState CurrentPlayerState;
        public CarryoverPlayerState LatestFinishedPlayerState;
        public bool bNewGamePlusPending = false;

        public SaveData()
        {
            CurrentPlayerState = new PlayerState();
            LatestFinishedPlayerState = new CarryoverPlayerState();
        }

        public void InitData()
        {
            Version = Application.version;
            LastPlayedTime = System.DateTime.Now;

            CurrentPlayerState.SaveArticyVariables();
            if (GameManager.instance != null)
            {
                CurrentPlayerState.SetCurrentDaySaved(GameManager.instance.GetCurrentDay().Id);
            }
            if (GrimDesk.instance != null)
            {
                LatestFinishedPlayerState.ItemPositions = GrimDesk.instance.GetDeskItemsStatus();
            }
            instance.SaveCarryoverVars();
        }

        public void ApplyCurrentSaveStateData()
        {
            if (CurrentPlayerState != null)
            {
                CurrentPlayerState.SetSpawnCounter(CurrentPlayerState.GetSpawnCounter());

                //PlayerManager.instance.SetPlayerState(CurrentPlayerState);
                CurrentPlayerState.LoadArticyState();

                List<ulong> alreadyRolledRandoIDs = CurrentPlayerState.GetAlreadyRolledRandoIDs();
                for (int i = 0; i < alreadyRolledRandoIDs.Count; ++i)
                {
                    ProfileManager.instance.RemoveRolledRandoFromPoolByID(alreadyRolledRandoIDs[i]);
                }

                GameManager.instance.SetCurrentDay(CurrentPlayerState.GetCurrentDaySaved());

                if (!CurrentPlayerState.IsFaxSent())
                {
                    if (CurrentPlayerState.IsIntroDone())
                    {
                        DesktopManager.instance.StartDay(true);
                    }
                    Elevator.instance.SetDaySprites();
                }
                else
                {
                    ShopManager.instance.NotifyStartDay(true);
                    Elevator.instance.SetNightSprites();
                }

                Elevator.instance.GetElevatorButtonBySceneType(EScene.Elevator).ToggleEnable(CurrentPlayerState.IsDayDone());

                ShopNotification.instanceOffice.ToggleVisible(CurrentPlayerState.HasOfficeShopNotification());
                ShopNotification.instanceMirror.ToggleVisible(CurrentPlayerState.HasMirrorShopNotification());

                GrimDesk.instance.RestoreDeskItemStatus(LatestFinishedPlayerState.ItemPositions);

                Elevator.instance.SetElevatorPosition(new Vector3(0, CurrentPlayerState.ElevatorHeight));

                ElevatorManager.instance.SwitchScene(CurrentPlayerState.GetLastOpenScene());
                HUDManager.instance.UpdateMoney();

            }
        }
    }

    public override void InitManager()
    {
        base.InitManager();

        Load();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bSaveNextFrame)
        {
            Save();
            //SaveBackup();
        }
    }

    private void Save()
    {
        //if (!Directory.Exists("Saves"))
        //{
        //    Directory.CreateDirectory("Saves");
        //}
        if (bIsLoading)
        {
            return;
        }
        if (bIsSaving)
        {
            Debug.LogError("Save conflict!");
            return;
        }
        if(SceneManager.GetActiveScene().name == "Intro")
        {
            Debug.Log("Skipping Save since we are in the menu and nothing has changed, really. And if it has, may the gods have mercy on us all.");
            bSaveNextFrame = false;
            return;
        }

        bIsSaving = true;
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "Saves");
        string savePathBackup = System.IO.Path.Combine(Application.persistentDataPath, "SaveBackups");

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        if (GameManager.instance.GameMode == EGameMode.DEMO)
        {
            savePathBackup = System.IO.Path.Combine(savePath, "SaveBackup_DayDEMO" + GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() + ".RIP");
            savePath = System.IO.Path.Combine(savePath, "SaveDEMO.RIP");
        }
        else
        {
            savePathBackup = System.IO.Path.Combine(savePath, "SaveBackup_Day" + GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() + ".RIP");
            savePath = System.IO.Path.Combine(savePath, "Save.RIP");
        }
        if (LocalCopyOfData != null)
        {

            string tempSaveFilePath = savePath + "temp";

            FileStream saveFile = File.Create(tempSaveFilePath);
            Debug.Log("Creating new save - temp: " + tempSaveFilePath);

            LocalCopyOfData.InitData();

            formatter.Serialize(saveFile, LocalCopyOfData);

            saveFile.Close();
            if(File.Exists(savePath))
            {
                if(File.Exists(savePathBackup))
                {
                    Debug.Log("Deleting previous backup");
                    File.Delete(savePathBackup);
                }
                File.Move(savePath, savePathBackup);
                Debug.Log("Backup Done for Day " + GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt());
            }
            else
            {
                Debug.Log("No backup exists yet, creating, Day " + GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt());
            }
            

            Debug.Log("Renaming temp file to save file");
            File.Move(savePath + "temp", savePath);

            bSaveNextFrame = false;

            bIsSaving = false;

            Debug.Log("Game Saved");
        }
        else
        {
            Debug.LogError("Game NOT saved, something exploded!");
        }

    }

    private void SaveBackup()
    {
        //if (!Directory.Exists("Saves"))
        //{
        //    Directory.CreateDirectory("Saves");
        //}
        if (bIsLoading)
        {
            return;
        }
        if (bIsSaving)
        {
            Debug.LogError("Save conflict!");
            return;
        }
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            Debug.Log("Skipping Save since we are in the menu and nothing has changed, really. And if it has, may the gods have mercy on us all.");
            return;
        }
        bIsSaving = true;
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "SaveBackups");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        if (GameManager.instance.GameMode == EGameMode.DEMO)
        {
            savePath = System.IO.Path.Combine(savePath, "SaveBackupDEMO_Day" + GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() + ".RIP");
        }
        else
        {
            savePath = System.IO.Path.Combine(savePath, "SaveBackup_Day" + GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt() + ".RIP");
        }
        FileStream saveFile = File.Create(savePath);
        LocalCopyOfData.InitData();

        formatter.Serialize(saveFile, LocalCopyOfData);

        saveFile.Close();

        bSaveNextFrame = false;

        bIsSaving = false;

        Debug.Log("Save Backup Made for Day: " + GetCurrentPlayerState().GetCurrentDayNumberNotIndexThisHasOneAddedToIt());
    }

    public void Load()
    {
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "Saves");
        if (GameManager.instance.GameMode == EGameMode.DEMO)
        {
            savePath = System.IO.Path.Combine(savePath, "SaveDEMO.RIP");
        }
        else
        {
            savePath = System.IO.Path.Combine(savePath, "Save.RIP");
        }
        if (File.Exists(savePath))
        {
            bIsLoading = true;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open(savePath, FileMode.Open);

            bool failed = false;
            try
            {
                LocalCopyOfData = (SaveData)formatter.Deserialize(saveFile);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load savegame, renaming file to: " + savePath + "_CORRUPT");
                Debug.LogError(e);
                saveFile.Close();
                failed = true;
                File.Move(savePath, savePath + "_CORRUPT");
                LocalCopyOfData = new SaveData();
            }

            //PlayerManager.instance.SetPlayerState(LocalCopyOfData.CurrentPlayerState);
            if (!failed)
            {
                saveFile.Close();
            }

            if (LocalCopyOfData.bNewGamePlusPending)
            {
                StartCoroutine(StartNewGamePlusRoutine());
            }
            else
            {
                StartCoroutine(EnableIntroRoutine());
            }

            //IntroController.instance.Hide();
        }
        else
        {
            Debug.Log("No save file found, creating clean save");
            LocalCopyOfData = new SaveData();
            //Save();
            StartCoroutine(EnableIntroRoutine());
        }

        bIsLoading = false;
    }

    IEnumerator EnableIntroRoutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        IntroController.instance.InitIntroController();
    }

    IEnumerator StartNewGamePlusRoutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        IntroController.instance.StartNewGamePlus();
        LocalCopyOfData.bNewGamePlusPending = false;
    }

    public void ApplyLoadState()
    {
        LocalCopyOfData.ApplyCurrentSaveStateData();
    }

    public void SaveOptions(Options newOptions)
    {
        //if (!Directory.Exists("Saves"))
        //{
        //    Directory.CreateDirectory("Saves");
        //}

        BinaryFormatter formatter = new BinaryFormatter();

        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "Saves");

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        savePath = System.IO.Path.Combine(savePath, "Options.RIP");

        FileStream saveFile = File.Create(savePath);
        CurrentOptions = newOptions;

        formatter.Serialize(saveFile, CurrentOptions);

        saveFile.Close();

        ApplyOptions();

        Debug.Log("OPTIONS SAVED");
    }

    public void LoadOptions()
    {
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "Saves");
        savePath = System.IO.Path.Combine(savePath, "Options.RIP");
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open(savePath, FileMode.Open);

            try
            {
                CurrentOptions = (Options)formatter.Deserialize(saveFile);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                CurrentOptions = new Options();
            }

            saveFile.Close();

            ApplyOptions();
            Debug.Log("OPTIONS LOADED");
        }
        else
        {
            CurrentOptions = new Options();
            CurrentOptions.CurrentResolution.height = Screen.currentResolution.height;
            CurrentOptions.CurrentResolution.width = Screen.currentResolution.width;
            CurrentOptions.CurrentResolution.refreshRate = Screen.currentResolution.refreshRate;
            CurrentOptions.ScreenMode = Screen.fullScreenMode;
            SaveOptions(CurrentOptions);
        }
    }

    public void ApplyOptions()
    {
        AudioManager.instance.UpdateVolumes();
    }

    public void ClearSave()
    {
        LocalCopyOfData = new SaveData();
        GameManager.instance.ResetArticyVariables();
        //CurrentOptions = new Options();
        //CurrentOptions.CurrentResolution.height = Screen.currentResolution.height;
        //CurrentOptions.CurrentResolution.width = Screen.currentResolution.width;
        //CurrentOptions.CurrentResolution.refreshRate = Screen.currentResolution.refreshRate;
        //CurrentOptions.ScreenMode = Screen.fullScreenMode;
        //SaveOptions(new Options());
        MarkSavegameDirty();
    }

    public void MarkSavegameDirty()
    {
        bSaveNextFrame = true;
    }



    public PlayerState GetCurrentPlayerState()
    {
        return LocalCopyOfData.CurrentPlayerState;
    }

    public CarryoverPlayerState GetCurrentCarryoverPlayerState()
    {
        return LocalCopyOfData.LatestFinishedPlayerState;
    }

    public SaveData GetSaveData()
    {
        return LocalCopyOfData;
    }

    public bool AreAllItemsOwned()
    {
        return AreAllAccessoriesOwned() &&
        ArticyGlobalVariables.Default.inventory.cactus &&
        GetCurrentCarryoverPlayerState().EraserBuyCount > 0 &&
        ArticyGlobalVariables.Default.inventory.radio &&
        ArticyGlobalVariables.Default.inventory.fidget_thing &&
        (ArticyGlobalVariables.Default.inventory.toy_cat || ArticyGlobalVariables.Default.game.toy_given_to_cat) &&
        ArticyGlobalVariables.Default.inventory.desklamp &&
        ArticyGlobalVariables.Default.inventory.coin &&
        ArticyGlobalVariables.Default.inventory.calendar &&
        //ArticyGlobalVariables.Default.inventory.award_plaque &&
        ArticyGlobalVariables.Default.inventory.snowglobe &&
        //ArticyGlobalVariables.Default.inventory.sin_bulb &&
        ArticyGlobalVariables.Default.inventory.mirror;
    }

    public bool AreAllAccessoriesOwned()
    {
        return ArticyGlobalVariables.Default.inventory.anubis &&
            ArticyGlobalVariables.Default.inventory.cat_ears &&
            ArticyGlobalVariables.Default.inventory.smoking_man &&
            ArticyGlobalVariables.Default.inventory.librarian &&
            ArticyGlobalVariables.Default.inventory.plague_doctor &&
            ArticyGlobalVariables.Default.inventory.sugar_skull &&
            ArticyGlobalVariables.Default.inventory.bonehead &&
            ArticyGlobalVariables.Default.inventory.great_old_one &&
            ArticyGlobalVariables.Default.inventory.cape &&
            ArticyGlobalVariables.Default.inventory.suit_bowtie &&
            ArticyGlobalVariables.Default.inventory.suit_tie;
    }

    public void AddItemDataToPlayer(template_item data)
    {
        if (AreAllAccessoriesOwned())
        {
            GetCurrentCarryoverPlayerState().bRIPFashion = true;
        }
        //        if (!GetCurrentCarryoverPlayerState().IsItemOwnedByID(data.Id))

        if (!data.Template.item_data.item_variable.CallScript())
        {
            data.Template.item_data.item_instruction_onbuy.CallScript();
            //GetCurrentCarryoverPlayerState().AddOwnedItemID(data.Id);
            switch (data.Template.item_data.item_type_category)
            {
                case item_type_category.ItemVisualAccessory:
                    List<template_item_variation> variations = ShopManager.instance.GetItemVariations(data);
                    for (int i = 0; i < variations.Count; ++i)
                    {
                        switch (variations[i].Template.item_accessory_variation.item_accessory_type)
                        {
                            case item_accessory_type.item_head:
                                GetCurrentCarryoverPlayerState().AddOwnedHeadID(variations[i].Id);
                                break;
                            case item_accessory_type.item_body:
                                GetCurrentCarryoverPlayerState().AddOwnedBodyID(variations[i].Id);
                                break;
                        }
                    }
                    break;
                case item_type_category.ItemToy:
                    break;
                case item_type_category.ItemInfoTool:
                    break;
            }
        }
        else
        {
            Debug.LogError("Trying to add item that is already owned! Tell Ott that " + data.DisplayName + " is trying to break stuff!");
        }
    }

    public void SaveCarryoverVars()
    {
        List<IStoredVariable> inventoryVars = new List<IStoredVariable>(ArticyGlobalVariables.Default.NamespaceVariableMap["inventory"]);
        List<IStoredVariable> shopVars = new List<IStoredVariable>(ArticyGlobalVariables.Default.NamespaceVariableMap["shop"]);

        Dictionary<string, object> inventoryMap = null;
        inventoryMap = new Dictionary<string, object>();
        for (int i = 0; i < inventoryVars.Count; ++i)
        {
            object chuckTesta = ArticyGlobalVariables.Default.GetVariableByString<object>(inventoryVars[i].FullQualifiedName);
            inventoryMap.Add(inventoryVars[i].FullQualifiedName, chuckTesta);
        }

        Dictionary<string, object> shopMap = null;
        shopMap = new Dictionary<string, object>();
        for (int i = 0; i < shopVars.Count; ++i)
        {
            object chuckTesta = ArticyGlobalVariables.Default.GetVariableByString<object>(shopVars[i].FullQualifiedName);
            shopMap.Add(shopVars[i].FullQualifiedName, chuckTesta);
        }

        GetCurrentCarryoverPlayerState().InventoryVars = inventoryMap;
        GetCurrentCarryoverPlayerState().ShopVars = shopMap;
    }

    public void HandleStartNewGamePlus()
    {
        int spawnCounter = GetCurrentPlayerState().GetSpawnCounter();
        LocalCopyOfData.CurrentPlayerState = new PlayerState();
        LocalCopyOfData.CurrentPlayerState.SetSpawnCounter(spawnCounter);
        LocalCopyOfData.CurrentPlayerState.IncrementSpawnCounter();

        ArticyDatabase.DefaultGlobalVariables.ResetVariables();

        foreach (string key in GetCurrentCarryoverPlayerState().InventoryVars.Keys)
        {
            ArticyGlobalVariables.Default.SetVariableByString(key, GetCurrentCarryoverPlayerState().InventoryVars[key]);
        }

        foreach (string key in GetCurrentCarryoverPlayerState().ShopVars.Keys)
        {
            ArticyGlobalVariables.Default.SetVariableByString(key, GetCurrentCarryoverPlayerState().ShopVars[key]);
        }
        ArticyGlobalVariables.Default.game.salary_on = true;
        if (ArticyGlobalVariables.Default.game.toy_given_to_cat)
        {
            ArticyGlobalVariables.Default.inventory.toy_cat = false;
        }
        ArticyGlobalVariables.Default.shop.keeper_hello_done = false;

        GetCurrentPlayerState().SetHasOfficeShopNotification(true);

        GrimDesk.instance.RestoreSalaryCoins();

        GetCurrentCarryoverPlayerState().SetGameFinished(false);

        HUDManager.instance.UpdateMoney();

        GetCurrentCarryoverPlayerState().SetGameStarted(true);
        Debug.Log("Carryover complete");
    }

    public void NotifyNewGamePlusPending()
    {
        LocalCopyOfData.bNewGamePlusPending = true;
        Save();
    }

    public void ForceSave()
    {
        Save();
    }
}
