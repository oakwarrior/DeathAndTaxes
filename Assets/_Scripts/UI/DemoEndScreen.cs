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
using UnityEngine.Networking;
using UnityEngine.UI;

public class DemoEndScreen : MonoBehaviour
{
    public static DemoEndScreen instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    List<string> LikedReactions = new List<string>();
    [SerializeField]
    List<string> DislikedReactions = new List<string>();
    [SerializeField]
    List<string> FeedbackQueries = new List<string>();
    [SerializeField]
    List<string> WishlistQueries = new List<string>();

    public TextMeshProUGUI TextThanks;
    public TextMeshProUGUI TextRestart;
    public TextMeshProUGUI TextVisitWebsite;

    public TextMeshProUGUI TextDidYouLike;
    public TextMeshProUGUI TextDisliked;
    public TextMeshProUGUI TextLiked;
    public TextMeshProUGUI TextFeedback;
    public TextMeshProUGUI TextWishlist;
    public TextMeshProUGUI TextDislikedWebGL;
    public TextMeshProUGUI TextLikedWebGL;



    public Button ButtonLikedDemo;
    public Button ButtonDislikedDemo;

    public Button ButtonLikedDemoWebGL;
    public Button ButtonDislikedDemoWebGL;

    public Button ButtonWishlistYes;
    public Button ButtonWishlistNo;

    public Button ButtonFeedbackYes;
    public Button ButtonFeedbackNo;

    public Button ButtonVisitWebsite;

    public Button ButtonExit;

    bool bLiked = false;
    bool bFadingOut = false;

    // Start is called before the first frame update
    void Start()
    {
        TextThanks.color = new Color(TextThanks.color.r, TextThanks.color.g, TextThanks.color.b, 0.0f);
        TextVisitWebsite.color = new Color(TextThanks.color.r, TextThanks.color.g, TextThanks.color.b, 0.0f);
        //TextRestart.color = new Color(TextRestart.color.r, TextRestart.color.g, TextRestart.color.b, 0.0f);
        HideDemoEndScreen();

        ButtonLikedDemo.onClick.AddListener(OnClickedLikedYes);
        ButtonDislikedDemo.onClick.AddListener(OnClickedLikedNo);
        ButtonWishlistYes.onClick.AddListener(OnClickedWishlistYes);
        ButtonWishlistNo.onClick.AddListener(OnClickedWishlistNo);
        ButtonFeedbackYes.onClick.AddListener(OnClickedFeedbackYes);
        ButtonFeedbackNo.onClick.AddListener(OnClickedFeedbackNo);
        ButtonVisitWebsite.onClick.AddListener(OnClickedWebsite);
        ButtonExit.onClick.AddListener(OnExitClicked);
        ButtonLikedDemoWebGL.onClick.AddListener(OnClickedLikedYesWebGL);
        ButtonDislikedDemoWebGL.onClick.AddListener(OnClickedLikedNoWebGL);

        TextDidYouLike.gameObject.SetActive(false);
        TextDisliked.gameObject.SetActive(false);
        TextLiked.gameObject.SetActive(false);
        TextFeedback.gameObject.SetActive(false);
        TextWishlist.gameObject.SetActive(false);
        ButtonLikedDemo.gameObject.SetActive(false);
        ButtonDislikedDemo.gameObject.SetActive(false);
        ButtonWishlistYes.gameObject.SetActive(false);
        ButtonWishlistNo.gameObject.SetActive(false);
        ButtonFeedbackYes.gameObject.SetActive(false);
        ButtonFeedbackNo.gameObject.SetActive(false);
        ButtonExit.gameObject.SetActive(false);
        ButtonLikedDemoWebGL.gameObject.SetActive(false);
        ButtonDislikedDemoWebGL.gameObject.SetActive(false);
        TextDislikedWebGL.gameObject.SetActive(false);
        TextLikedWebGL.gameObject.SetActive(false);

#if UNITY_WEBGL
        ButtonVisitWebsite.gameObject.SetActive(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnExitClicked()
    {
        if (bFadingOut)
        {
            return;
        }
        StartCoroutine(OnRestartClickedRoutine());
    }


    IEnumerator OnRestartClickedRoutine()
    {
        yield return StartCoroutine(FadeOutDemoThanksText(1.0f));
        //ElevatorManager.instance.SwitchScene(EScene.Intro);
        //DesktopManager.instance.ClearProfiles();
        //HideDemoEndScreen();
        Application.Quit();
    }

    public void ShowDemoEndScreen()
    {
        gameObject.SetActive(true);
        //HUDManager.instance.TextDayCounter.gameObject.SetActive(false);
        SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameStarted(false);
        SaveManager.instance.GetCurrentCarryoverPlayerState().SetGameStartedOnce(false);


    }

    public void HideDemoEndScreen()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator FadeInDemoThanksText(float duration)
    {
        bFadingOut = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            TextThanks.color = new Color(TextThanks.color.r, TextThanks.color.g, TextThanks.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            TextVisitWebsite.color = new Color(TextVisitWebsite.color.r, TextVisitWebsite.color.g, TextVisitWebsite.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            //TextRestart.color = new Color(TextRestart.color.r, TextRestart.color.g, TextRestart.color.b, Mathf.Clamp((elapsedTime / duration), 0.0f, 1.0f));
            yield return null;
        }
        bFadingOut = false;

        TextDidYouLike.gameObject.SetActive(true);

#if UNITY_STANDALONE
        ButtonLikedDemo.gameObject.SetActive(true);
        ButtonDislikedDemo.gameObject.SetActive(true);
#endif

#if UNITY_WEBGL
        
        ButtonLikedDemoWebGL.gameObject.SetActive(true);
        ButtonDislikedDemoWebGL.gameObject.SetActive(true);
#endif



    }

    public IEnumerator FadeOutDemoThanksText(float duration)
    {
        bFadingOut = true;

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            TextThanks.color = new Color(TextThanks.color.r, TextThanks.color.g, TextThanks.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            TextVisitWebsite.color = new Color(TextVisitWebsite.color.r, TextVisitWebsite.color.g, TextVisitWebsite.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));
            //TextRestart.color = new Color(TextRestart.color.r, TextRestart.color.g, TextRestart.color.b, Mathf.Clamp(1.0f - (elapsedTime / duration), 0.0f, 1.0f));

            yield return null;
        }
        bFadingOut = false;
    }

    public void OnClickedLikedYesWebGL()
    {
        TextDidYouLike.gameObject.SetActive(false);
        ButtonLikedDemoWebGL.gameObject.SetActive(false);
        ButtonDislikedDemoWebGL.gameObject.SetActive(false);

        TextLikedWebGL.gameObject.SetActive(true);
        ButtonExit.gameObject.SetActive(true);

    }

    public void OnClickedLikedNoWebGL()
    {
        TextDidYouLike.gameObject.SetActive(false);
        ButtonLikedDemoWebGL.gameObject.SetActive(false);
        ButtonDislikedDemoWebGL.gameObject.SetActive(false);

        TextDislikedWebGL.gameObject.SetActive(true);
        ButtonExit.gameObject.SetActive(true);
    }

    public void OnClickedLikedYes()
    {
        bLiked = true;
        TextDidYouLike.gameObject.SetActive(false);
        ButtonLikedDemo.gameObject.SetActive(false);
        ButtonDislikedDemo.gameObject.SetActive(false);

        TextLiked.gameObject.SetActive(true);
        ButtonWishlistYes.gameObject.SetActive(true);
        ButtonWishlistNo.gameObject.SetActive(true);
    }

    public void OnClickedLikedNo()
    {
        TextDidYouLike.gameObject.SetActive(false);
        ButtonLikedDemo.gameObject.SetActive(false);
        ButtonDislikedDemo.gameObject.SetActive(false);

        TextDisliked.gameObject.SetActive(true);
        ButtonFeedbackYes.gameObject.SetActive(true);
        ButtonFeedbackNo.gameObject.SetActive(true);
    }

    public void OnClickedWebsite()
    {
        Application.OpenURL("https://deathandtaxesgame.com/");
    }

    public void OnClickedWishlistYes()
    {
        Application.OpenURL("https://store.steampowered.com/app/1166290/Death_and_Taxes/fromgame");
        TextLiked.gameObject.SetActive(false);
        TextDisliked.gameObject.SetActive(false);
        if (bLiked)
        {
            TextFeedback.gameObject.SetActive(true);
            ButtonFeedbackYes.gameObject.SetActive(true);
            ButtonFeedbackNo.gameObject.SetActive(true);

            TextWishlist.gameObject.SetActive(false);
            ButtonWishlistYes.gameObject.SetActive(false);
            ButtonWishlistNo.gameObject.SetActive(false);
        }
        else
        {
            ButtonExit.gameObject.SetActive(true);

            TextWishlist.gameObject.SetActive(false);
            ButtonWishlistYes.gameObject.SetActive(false);
            ButtonWishlistNo.gameObject.SetActive(false);
        }
    }

    public void OnClickedWishlistNo()
    {
        TextLiked.gameObject.SetActive(false);
        TextDisliked.gameObject.SetActive(false);
        if (bLiked)
        {
            TextFeedback.gameObject.SetActive(true);
            ButtonFeedbackYes.gameObject.SetActive(true);
            ButtonFeedbackNo.gameObject.SetActive(true);

            TextWishlist.gameObject.SetActive(false);
            ButtonWishlistYes.gameObject.SetActive(false);
            ButtonWishlistNo.gameObject.SetActive(false);
        }
        else
        {
            ButtonExit.gameObject.SetActive(true);

            TextWishlist.gameObject.SetActive(false);
            ButtonWishlistYes.gameObject.SetActive(false);
            ButtonWishlistNo.gameObject.SetActive(false);
        }
    }

    public void OnClickedFeedbackYes()
    {
        TextLiked.gameObject.SetActive(false);
        TextDisliked.gameObject.SetActive(false);

        //email Id to send the mail to
        string email = "placeholdergameworks@gmail.com";
        //subject of the mail
        string subject = MyEscapeURL("Death and Taxes Demo feedback");
        //body of the mail which consists of Device Model and its Operating System
        string body = MyEscapeURL("Please write your feedback here! :) \n\n\n\n");
        //Open the Default Mail App
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);

        if (bLiked)
        {
            TextFeedback.gameObject.SetActive(false);
            ButtonFeedbackYes.gameObject.SetActive(false);
            ButtonFeedbackNo.gameObject.SetActive(false);

            ButtonExit.gameObject.SetActive(true);
        }
        else
        {
            TextFeedback.gameObject.SetActive(false);
            ButtonFeedbackYes.gameObject.SetActive(false);
            ButtonFeedbackNo.gameObject.SetActive(false);

            TextWishlist.gameObject.SetActive(true);
            ButtonWishlistYes.gameObject.SetActive(true);
            ButtonWishlistNo.gameObject.SetActive(true);
        }
    }

    public void OnClickedFeedbackNo()
    {
        TextLiked.gameObject.SetActive(false);
        TextDisliked.gameObject.SetActive(false);

        if (bLiked)
        {
            TextFeedback.gameObject.SetActive(false);
            ButtonFeedbackYes.gameObject.SetActive(false);
            ButtonFeedbackNo.gameObject.SetActive(false);

            ButtonExit.gameObject.SetActive(true);
        }
        else
        {
            TextFeedback.gameObject.SetActive(false);
            ButtonFeedbackYes.gameObject.SetActive(false);
            ButtonFeedbackNo.gameObject.SetActive(false);

            TextWishlist.gameObject.SetActive(true);
            ButtonWishlistYes.gameObject.SetActive(true);
            ButtonWishlistNo.gameObject.SetActive(true);
        }
    }

    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }


}
