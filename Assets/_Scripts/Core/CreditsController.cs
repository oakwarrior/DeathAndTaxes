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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    public static CreditsController instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    List<GameObject> PaperworkMarkers = new List<GameObject>();

    int DeveloperProfileCount = 0;
    int DeveloperProfileIndex = 0;

    Paperwork CurrentPaperwork = null;

    bool bCreditsDone = false;

    [SerializeField]
    TextMeshProUGUI TextSpecialThanks;
    [SerializeField]
    TextMeshProUGUI TextBackToMainMenu;

    [SerializeField]
    Button ButtonBackToMainMenu;

    List<template_profile> SparedDevs = new List<template_profile>();
    List<template_profile> DoomedDevs = new List<template_profile>();

    public void AddSparedDev(template_profile dev)
    {
        SparedDevs.Add(dev);
    }

    public void AddDoomedDev(template_profile dev)
    {
        DoomedDevs.Add(dev);
    }

    // Start is called before the first frame update
    void Start()
    {
        Color smthColor = new Color(1, 1, 1, 0);
        TextBackToMainMenu.color = smthColor;
        TextSpecialThanks.color = smthColor;

        DeveloperProfileIndex = 0;
        DeveloperProfileCount = ProfileManager.instance.DeveloperProfileList.Count;

        SpawnCreditsPaperwork();
        ButtonBackToMainMenu.onClick.AddListener(OnBackToMainMenuClicked);

        MarkerOfDeath.instance.MarkerAppear();
    }

    public void OnBackToMainMenuClicked()
    {
        GameManager.instance.RestartGame();
    }

    public void SpawnCreditsPaperwork()
    {
        if(DeveloperProfileIndex >= DeveloperProfileCount)
        {
            bCreditsDone = true;
            MarkerOfDeath.instance.MarkerDisappear();
            StartCoroutine(ThankRoutine());
        }
        else
        {
            Paperwork newPaperwork = Instantiate(PaperworkManager.instance.PaperworkTemplate);
            newPaperwork.bIsCreditsProfile = true;
            newPaperwork.InitFromProfile(ProfileManager.instance.DeveloperProfileList[DeveloperProfileIndex], GrimDesk.instance.PaperWorkSpawnMarker.gameObject.transform.position, GrimDesk.instance.PaperWorkSpawnMarker.gameObject.transform.rotation, false, 0, false);
            newPaperwork.gameObject.transform.SetParent(gameObject.transform);
            int spawnMarkerIndex = DeveloperProfileIndex % PaperworkMarkers.Count;
            Vector3 spawnPos = newPaperwork.gameObject.transform.position;

            spawnPos.x = PaperworkMarkers[spawnMarkerIndex].transform.position.x;
            spawnPos.y = PaperworkMarkers[spawnMarkerIndex].transform.position.y;

            newPaperwork.gameObject.transform.position = spawnPos;

            newPaperwork.FocusPaperwork();

            CurrentPaperwork = newPaperwork;
            DeveloperProfileIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentPaperwork == null && bCreditsDone)
        {
            
        }
    }

    IEnumerator ThankRoutine()
    {
        float elapsedTime = 0.0f;
        float duration = 2.5f;
        TextSpecialThanks.gameObject.SetActive(true);
        ButtonBackToMainMenu.gameObject.SetActive(true);
        Color smthColor = new Color(1, 1, 1, 0);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            smthColor.a = Mathf.Clamp(elapsedTime / duration, 0.0f, 1.0f);

            TextBackToMainMenu.color = smthColor;
            TextSpecialThanks.color = smthColor;

            yield return null;
        }

    }

    
}
