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

public class GrimDesk : MonoBehaviour, Interactable
{
    public static GrimDesk instance;

    [SerializeField]
    public GameObject PaperWorkSpawnMarker;
    [SerializeField]
    public GameObject MoneySpawnMarker;

    [SerializeField]
    public Transform MasterObjectTransform;

    [SerializeField]
    List<DeskItem> DeskObjects = new List<DeskItem>();

    [SerializeField]
    List<Ownable> BuyableDeskObjects = new List<Ownable>();

    [SerializeField]
    SalaryCoin SalaryCoinTemplate = null;

    List<SalaryCoin> SalaryCoinList = new List<SalaryCoin>();

    [SerializeField]
    public GrimDeskDrawer DrawerLeft;
    [SerializeField]
    public GrimDeskDrawer DrawerRight;

    float CoinWailingTimer = 0.0f;

    public void CoinWail()
    {
        CoinWailingTimer = Random.Range(5.0f, 10.0f);

        List<SalaryCoin> wailableCoins = new List<SalaryCoin>();
        for (int i = 0; i < SalaryCoinList.Count; ++i)
        {
            if (!SalaryCoinList[i].IsInDrawer() || SalaryCoinList[i].IsInDrawer() && SalaryCoinList[i].GetDrawer().IsOpen())
            {
                wailableCoins.Add(SalaryCoinList[i]);
            }
        }

        if (wailableCoins.Count > 0)
        {
            SalaryCoin wailCoin = wailableCoins[Random.Range(0, wailableCoins.Count)];
            wailCoin.Wail();
        }
    }

    private SalaryCoin SpawnSalaryCoin()
    {
        SalaryCoin newCoin = Instantiate(SalaryCoinTemplate);
        newCoin.gameObject.transform.SetParent(MasterObjectTransform);
        newCoin.gameObject.transform.position = MoneySpawnMarker.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        newCoin.name += SalaryCoinList.Count;
        SalaryCoinList.Add(newCoin);
        return newCoin;
    }

    public void SpawnSalary(int amount)
    {
        int coins = amount / 100;
        for (int i = 0; i < coins; ++i)
        {
            SpawnSalaryCoin();
        }
    }

    public void SpendSalary(int amount)
    {
        int coins = Mathf.Abs(amount) / 100;
        for (int i = 0; i < coins; ++i)
        {
            SalaryCoin coinToSpend = SalaryCoinList[SalaryCoinList.Count - 1];
            SalaryCoinList.Remove(coinToSpend);
            Destroy(coinToSpend.gameObject);
        }

        SaveManager.instance.MarkSavegameDirty();
    }

    public void RestoreSalaryCoins()
    {
        //int coins = ArticyGlobalVariables.Default.inventory.money / 100;

        foreach (KeyValuePair<string, DeskItemStatus> entry in SaveManager.instance.GetCurrentCarryoverPlayerState().ItemPositions)
        {
            if (entry.Key.Contains("SalaryCoin"))
            {
                SalaryCoin coin = SpawnSalaryCoin();
                coin.name = entry.Key;
                coin.RestoreStatus(entry.Value);
            }
        }

        //for(int i = 0; i < coins; ++i)
        //{

        //}
    }

    public string GetHoverText()
    {
        return "";
    }

    public void Interact()
    {
        if (MarkerOfDeath.instance.IsPickedUp())
        {
            MarkerOfDeath.instance.Interact();
        }
        if (Eraser.instance.IsPickedUp())
        {
            Eraser.instance.Interact();
        }
    }

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < DeskObjects.Count; ++i)
        {
            Ownable buyable = (Ownable)DeskObjects[i].GetComponentInChildren(typeof(Ownable));
            if (buyable != null)
            {
                BuyableDeskObjects.Add(buyable);
            }
        }
    }

    public Dictionary<string, DeskItemStatus> GetDeskItemsStatus()
    {
        Dictionary<string, DeskItemStatus> dic = new Dictionary<string, DeskItemStatus>();

        for (int i = 0; i < DeskObjects.Count; ++i)
        {
            DeskObjects[i].ItemStatus.Position = DeskObjects[i].gameObject.transform.localPosition;
            dic.Add(DeskObjects[i].ToString(), DeskObjects[i].ItemStatus);
        }

        for (int i = 0; i < PaperworkManager.instance.PaperworkList.Count; ++i)
        {
            PaperworkManager.instance.PaperworkList[i].ItemStatus.Position = PaperworkManager.instance.PaperworkList[i].gameObject.transform.localPosition;
            dic.Add(PaperworkManager.instance.PaperworkList[i].ToString(), PaperworkManager.instance.PaperworkList[i].ItemStatus);
        }

        for (int i = 0; i < SalaryCoinList.Count; ++i)
        {
            SalaryCoinList[i].ItemStatus.Position = SalaryCoinList[i].gameObject.transform.localPosition;
            dic.Add(SalaryCoinList[i].ToString(), SalaryCoinList[i].ItemStatus);
        }

        return dic;
    }

    public bool AreAllItemsInDrawers()
    {
        bool allInDesk = true;
        for (int i = 0; i < DeskObjects.Count; ++i)
        {
            if(DeskObjects[i] == Egg.instance)
            {
                continue;
            }
            if (DeskObjects[i].gameObject.activeSelf && DeskObjects[i].ItemStatus.DrawerStatus == ELeftOrRight.MAX)
            {
                return false;
            }
        }
        return allInDesk;
    }

    public void RestoreDeskItemStatus(Dictionary<string, DeskItemStatus> dic)
    {
        for (int i = 0; i < DeskObjects.Count; ++i)
        {
            if (dic.ContainsKey(DeskObjects[i].ToString()))
            {
                DeskObjects[i].RestoreStatus(dic[DeskObjects[i].ToString()]);
            }
        }

        for (int i = 0; i < PaperworkManager.instance.PaperworkList.Count; ++i)
        {
            if (dic.ContainsKey(PaperworkManager.instance.PaperworkList[i].ToString()))
            {
                PaperworkManager.instance.PaperworkList[i].RestoreStatus(dic[PaperworkManager.instance.PaperworkList[i].ToString()]);

            }
            else
            {
                Debug.LogError("No saved profile data found for: " + PaperworkManager.instance.PaperworkList[i].ToString());
            }
        }

        RestoreSalaryCoins();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CoinWailingTimer -= Time.deltaTime;
        if (CoinWailingTimer <= 0.0f)
        {
            CoinWail();
        }
    }

    public void Hover()
    {

    }

    public void UpdateDragGrabPosition(Vector3 position)
    {

    }

    public bool CanDrag()
    {
        return false;
    }

    public bool IsDragging()
    {
        return false;
    }

    public void ToggleDragging(bool drag)
    {

    }

    public void UpdateDeskItemOwnedStatus()
    {
        for (int i = 0; i < BuyableDeskObjects.Count; ++i)
        {
            BuyableDeskObjects[i].HandleOwnedStatus();
        }
    }

    public void Unhover()
    {

    }
}
