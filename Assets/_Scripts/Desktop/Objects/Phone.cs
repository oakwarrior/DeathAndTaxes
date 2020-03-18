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
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Phone : MonoBehaviour, Interactable
{
    public static Phone instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject PhoneHand;
    public GameObject PhoneTableRef;

    public GameObject PhoneTableMarkerOff;

    public GameObject PhoneHandMarker;
    public GameObject PhoneHandMarkerOff;

    [SerializeField]
    AudioClip NotificationSound;
    [SerializeField]
    AudioClip PhoneSound;
    [SerializeField]
    AudioClip PhoneBuzz;

    [SerializeField]
    List<string> NewsEcologyPositive = new List<string>();
    [SerializeField]
    List<string> NewsHealthPositive = new List<string>();
    [SerializeField]
    List<string> NewsProsperityPositive = new List<string>();
    [SerializeField]
    List<string> NewsPeacePositive = new List<string>();

    [SerializeField]
    List<string> NewsEcologyNegative = new List<string>();
    [SerializeField]
    List<string> NewsHealthNegative = new List<string>();
    [SerializeField]
    List<string> NewsProsperityNegative = new List<string>();
    [SerializeField]
    List<string> NewsPeaceNegative = new List<string>();

    Vector3 PhoneTableLocation;
    public SpriteRenderer ImageNotification;
    public TextMeshProUGUI TextNews;
    public TextMeshProUGUI TextClock;

    bool bHasNotification;

    List<string> NewsQueue = new List<string>();

    public bool bIsFocused = false;

    Vector3 InitialNewsPosition;

    Quaternion OriginRotation;
    [SerializeField]
    float VibrateRotationCW;
    [SerializeField]
    float VibrateRotationCCW;

    public List<bool> Lerps = new List<bool>();

    public int TimesClicked;

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

    public void Unhover()
    {

    }

    void Start()
    {
        
        InitialNewsPosition = TextNews.transform.position;
        OriginRotation = PhoneTableRef.gameObject.transform.rotation;
    }

    void Update()
    {
        string decimalBS = System.DateTime.Now.Minute < 10 ? "0" : "";
        TextClock.text = System.DateTime.Now.Hour.ToString() + ":" + decimalBS + System.DateTime.Now.Minute.ToString();
    }

    void FocusPhone()
    {
        PhoneTableLocation = PhoneTableRef.gameObject.transform.position;
        AudioManager.instance.PlayOneShotEffect(PhoneSound);

        StartCoroutine(LerpToPosition(PhoneTableRef, PhoneTableLocation, PhoneTableMarkerOff.gameObject.transform.position, 0.35f));
        StartCoroutine(LerpToPosition(PhoneHand, PhoneHandMarkerOff.gameObject.transform.position, PhoneHandMarker.gameObject.transform.position, 0.55f));

        HideNotification();

    }

    void UnFocusPhone()
    {
        StartCoroutine(LerpToPosition(PhoneTableRef, PhoneTableMarkerOff.gameObject.transform.position, PhoneTableLocation, 0.35f));
        StartCoroutine(LerpToPosition(PhoneHand, PhoneHandMarker.gameObject.transform.position, PhoneHandMarkerOff.gameObject.transform.position, 0.55f));

        HideNotification();
    }

    public void AddNewsText(string text, bool addToStart = false, bool fromSaveGame = false)
    {
        if (text != "")
        {
            if (addToStart)
            {
                NewsQueue.Insert(0, text);
            }
            else
            {
                NewsQueue.Add(text);
            }
        }
        if(!fromSaveGame)
        {
            SaveManager.instance.GetCurrentPlayerState().AddNewsText(text, addToStart);
        }
    }

    public void RollSituationNews()
    {
        if (SaveManager.instance.GetCurrentPlayerState().GetStat(EStat.ECOLOGY_DAILY) <= -45)
        {
            AddNewsText("<b><i><color=black>" + NewsEcologyNegative[Random.Range(0, NewsEcologyNegative.Count)] + "</color></i></b>", true);
        }
        if (SaveManager.instance.GetCurrentPlayerState().GetStat(EStat.ECOLOGY_DAILY) >= 45)
        {
            AddNewsText("<b><i><color=black>" + NewsEcologyPositive[Random.Range(0, NewsEcologyPositive.Count)] + "</color></i></b>", true);
        }

        if (SaveManager.instance.GetCurrentPlayerState().GetStat(EStat.HEALTH_DAILY) <= -45)
        {
            AddNewsText("<b><i><color=black>" + NewsHealthNegative[Random.Range(0, NewsHealthNegative.Count)] + "</color></i></b>", true);
        }
        if (SaveManager.instance.GetCurrentPlayerState().GetStat(EStat.HEALTH_DAILY) >= 45)
        {
            AddNewsText("<b><i><color=black>" + NewsHealthPositive[Random.Range(0, NewsHealthPositive.Count)] + "</color></i></b>", true);
        }

        if (SaveManager.instance.GetCurrentPlayerState().GetStat(EStat.PEACE_DAILY) <= -45)
        {
            AddNewsText("<b><i><color=black>" + NewsPeaceNegative[Random.Range(0, NewsPeaceNegative.Count)] + "</color></i></b>", true);
        }
        if (SaveManager.instance.GetCurrentPlayerState().GetStat(EStat.PEACE_DAILY) >= 45)
        {
            AddNewsText("<b><i><color=black>" + NewsPeacePositive[Random.Range(0, NewsPeacePositive.Count)] + "</color></i></b>", true);
        }

        if (SaveManager.instance.GetCurrentPlayerState().GetStat(EStat.PROSPERITY_DAILY) <= -45)
        {
            AddNewsText("<b><i><color=black>" + NewsProsperityNegative[Random.Range(0, NewsProsperityNegative.Count)] + "</color></i></b>", true);
        }
        if (SaveManager.instance.GetCurrentPlayerState().GetStat(EStat.PROSPERITY_DAILY) >= 45)
        {
            AddNewsText("<b><i><color=black>" + NewsProsperityPositive[Random.Range(0, NewsProsperityPositive.Count)] + "</color></i></b>", true);
        }
    }

    public void StartNewsQueueTimer()
    {
        StartCoroutine(PopNewsQueueRoutine(Random.Range(2.0f, 5.0f)));
    }

    IEnumerator PopNewsQueueRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        if (NewsQueue.Count > 0)
        {
            int numOfNews = Random.Range(7, Mathf.Min(NewsQueue.Count, 15));
            for (int i = 0; i < numOfNews && NewsQueue.Count > 0; ++i)
            {
                TextNews.text += NewsQueue[0] + "\n\n";
                NewsQueue.RemoveAt(0);
            }
            PlayNotification();
        }

        if (NewsQueue.Count > 0)
        {
            StartNewsQueueTimer();
        }
    }

    public void ClearNews()
    {
        TextNews.text = "";
        HideNotification();
        TextNews.transform.position = InitialNewsPosition;
        TextNews.enabled = false;
        SaveManager.instance.GetCurrentPlayerState().ClearSavedNews();
    }

    public void PlayNotification()
    {
        TextNews.enabled = true;
        if (bIsFocused)
        {
            return;
        }
        AudioManager.instance.PlayOneShotEffect(NotificationSound);

        bHasNotification = true;
        ImageNotification.gameObject.SetActive(true);
        
        if(!PhoneTable.instance.IsInDrawer())
        {
            StartCoroutine(VibrateRoutine());
        }
    }

    public void HideNotification()
    {
        bHasNotification = false;
        ImageNotification.gameObject.SetActive(false);
    }

    public void Interact()
    {
        if (FaxMachine.instance.bIsFaxTransmitting)
        {
            return;
        }

        if (Lerps.Count > 0)
        {
            return;
        }

        TimesClicked++;
        bIsFocused = !bIsFocused;
        if (bIsFocused)
        {
            FocusPhone();
        }
        else
        {
            UnFocusPhone();
        }
    }

    IEnumerator LerpToPosition(GameObject obj, Vector3 from, Vector3 to, float duration)
    {
        Lerps.Add(true);
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(from, to, (elapsedTime / duration));
            yield return null;
        }
        Lerps.Remove(true);
    }

    IEnumerator VibrateRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 eulerAngles = PhoneTableRef.gameObject.transform.eulerAngles;
        AudioManager.instance.PlayOneShotEffect(PhoneBuzz);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCW);


        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.rotation = OriginRotation;

        AudioManager.instance.PlayOneShotEffect(PhoneBuzz);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCCW);
        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, VibrateRotationCW);


        yield return new WaitForSeconds(0.1f);
        PhoneTableRef.gameObject.transform.rotation = OriginRotation;


    }

    public string GetHoverText()
    {
        return "I wonder if I'm making a difference?";
    }


}
