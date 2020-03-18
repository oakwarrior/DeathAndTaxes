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
using TMPro;
using UnityEngine;

public class VoteCounter : DeskItem, Draggable, Interactable
{

    public static VoteCounter instance;

    List<SpriteRenderer> Renderers = new List<SpriteRenderer>();

    private void Awake()
    {
        instance = this;

        Renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < Renderers.Count; ++i)
        {
            OriginalSortOrders.Add(Renderers[i], Renderers[i].sortingOrder);
        }
        List<TextMeshPro> textrenderers = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < textrenderers.Count; ++i)
        {
            OriginalSortOrdersText.Add(textrenderers[i], textrenderers[i].sortingOrder);
        }
        OriginalZ = gameObject.transform.localPosition.z;

        OriginPosition = gameObject.transform.position;


    }

    bool bIsDragging = false;
    public Vector3 GrabPosition;
    Dictionary<SpriteRenderer, int> OriginalSortOrders = new Dictionary<SpriteRenderer, int>();
    Dictionary<TextMeshPro, int> OriginalSortOrdersText = new Dictionary<TextMeshPro, int>();
    float OriginalZ = 0;

    Dictionary<string, bool> UserVoteMap = new Dictionary<string, bool>();

    bool bVoteInProgress = false;
    bool bVoteEnded = false;

    [SerializeField]
    GameObject BookOpen;
    [SerializeField]
    GameObject BookClosed;

    [SerializeField]
    GameObject ScaleTop;

    [SerializeField]
    GameObject ScaleLive;
    [SerializeField]
    GameObject ScaleDie;

    [SerializeField]
    TextMeshPro TextDiePercentage;
    [SerializeField]
    TextMeshPro TextLivePercentage;
    [SerializeField]
    TextMeshPro TextVoteResult;

    [SerializeField]
    TextMeshPro TextDieCommand;
    [SerializeField]
    TextMeshPro TextLiveCommand;

    [SerializeField]
    TextMeshPro TextTimer;
    [SerializeField]
    AudioClip ClipOpen;
    [SerializeField]
    AudioClip ClipClose;


    float CurrentVoteTimer = 0.0f;
    float CurrentAngle = 0.0f;
    float ScaleTargetAngle = 0.0f;

    Vector3 ScaleTopTargetEuler = Vector3.zero;
    Vector3 ScaleTargetEuler = Vector3.zero;

    int TotalVotes = 0;
    int DieVotes = 0;
    int LiveVotes = 0;

    // Start is called before the first frame update
    void Start()
    {
        BookOpen.gameObject.SetActive(false);
        BookClosed.gameObject.SetActive(true);

        TextLivePercentage.text = "50%";
        TextDiePercentage.text = "50%";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScaleAngle();

        if (bVoteInProgress)
        {
            CurrentVoteTimer = Mathf.Clamp(CurrentVoteTimer - Time.deltaTime, 0.0f, SaveManager.instance.CurrentOptions.StreamVoteTimer);

            if (CurrentVoteTimer <= 0.0f)
            {
                EndVote();
            }
        }
        TextTimer.text = CurrentVoteTimer.ToString("F2");

        if (IsDragging())
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = gameObject.transform.position.z;
            gameObject.transform.position = worldPoint + GrabPosition;
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
        gameObject.transform.SetParent(GrimDesk.instance.MasterObjectTransform);
        for (int i = 0; i < Renderers.Count; ++i)
        {
            Renderers[i].maskInteraction = SpriteMaskInteraction.None;
            Renderers[i].sortingOrder = OriginalSortOrders[Renderers[i]];
        }
        List<TextMeshPro> textrenderers = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < textrenderers.Count; ++i)
        {
            textrenderers[i].sortingOrder = OriginalSortOrdersText[textrenderers[i]];
        }

        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = OriginalZ;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = ELeftOrRight.MAX;
    }

    public void HandleDropIntoDrawer(GrimDeskDrawer hitDrawer)
    {
        gameObject.transform.SetParent(hitDrawer.transform);

        for (int i = 0; i < Renderers.Count; ++i)
        {
            Renderers[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            Renderers[i].sortingOrder = 1;
        }
        List<TextMeshPro> textrenderers = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        for (int i = 0; i < textrenderers.Count; ++i)
        {
            textrenderers[i].sortingOrder = 1;
        }
        Vector3 tmp = gameObject.transform.localPosition;
        tmp.z = -1;
        gameObject.transform.localPosition = tmp;

        ItemStatus.DrawerStatus = hitDrawer.Type;
    }


    public override string GetHoverText()
    {
        if (bVoteEnded)
        {
            if (LiveVotes > DieVotes)
            {
                return "Live!";
            }
            else if (LiveVotes < DieVotes)
            {
                return "Die!";
            }
            else
            {
                return "It's a tie! Damn these ethereal voices!";
            }
        }
        else
        {
            if (bVoteInProgress)
            {
                return "Speak to me, oh voices of beyond!";
            }
            else
            {
                return "I could commune with the Voices of The Ether to help me decide...";
            }
        }
        //return "Where are these numbers even coming from???";
    }

    public void Interact()
    {
        if (bVoteInProgress || bVoteEnded)
        {
            BookOpen.gameObject.SetActive(false);
            BookClosed.gameObject.SetActive(true);
            AudioManager.instance.PlayOneShotEffect(ClipClose);
            EndVote();
            bVoteEnded = false;
            ScaleTargetAngle = 0;
            ClearVote();
        }
        else
        {
            StartVote();
        }
        if(IsInDrawer())
        {
            HandleDragOutOfDrawer();
        }
    }

    public bool IsVoteInProgress()
    {
        return bVoteInProgress;
    }

    public void StartVote()
    {
        if (bVoteInProgress)
        {
            return;
        }
        AudioManager.instance.PlayOneShotEffect(ClipOpen);
        TextDieCommand.text = SaveManager.instance.CurrentOptions.StreamCommandDie;
        TextLiveCommand.text = SaveManager.instance.CurrentOptions.StreamCommandLive;

        TextVoteResult.text = "VOTE NOW!";
        BookClosed.gameObject.SetActive(false);
        BookOpen.gameObject.SetActive(true);
        CurrentVoteTimer = SaveManager.instance.CurrentOptions.StreamVoteTimer;
        bVoteInProgress = true;
    }

    public void ClearVote()
    {
        UserVoteMap.Clear();
        TotalVotes = 0;
        LiveVotes = 0;
        DieVotes = 0;
        TextLivePercentage.text = "50%";
        TextDiePercentage.text = "50%";
    }

    public void EndVote()
    {
        if (LiveVotes > DieVotes)
        {
            TextVoteResult.text = "LIVE!";
        }
        else if (LiveVotes < DieVotes)
        {
            TextVoteResult.text = "DIE!";
        }
        else
        {
            TextVoteResult.text = "TIED!";
        }

        bVoteInProgress = false;
        bVoteEnded = true;
    }

    public void RegisterVote(string username, bool die)
    {
        if (UserVoteMap.ContainsKey(username))
        {
            if (UserVoteMap[username])
            {
                DieVotes--;
            }
            else
            {
                LiveVotes--;
            }
            UserVoteMap[username] = die;
            if (UserVoteMap[username])
            {
                DieVotes++;
            }
            else
            {
                LiveVotes++;
            }
        }
        else
        {
            UserVoteMap.Add(username, die);
            TotalVotes++;
            if (die)
            {
                DieVotes++;
            }
            else
            {
                LiveVotes++;
            }
        }

        float liveRatio = (float)LiveVotes / TotalVotes;
        float dieRatio = (float)DieVotes / TotalVotes;
        //Debug.Log("liveRatio: " + liveRatio);
        //Debug.Log("dieRatio: " + dieRatio);

        //Debug.Log("livePercent: " + Mathf.RoundToInt(liveRatio * 100.0f));
        //Debug.Log("diePercent: " + Mathf.RoundToInt(dieRatio * 100.0f));

        SetScaleTargetAngle(Mathf.Lerp(-35, 35, dieRatio));


        TextLivePercentage.text = Mathf.RoundToInt(liveRatio * 100.0f) + "%";
        TextDiePercentage.text = Mathf.RoundToInt(dieRatio * 100.0f) + "%";
    }

    public void SetScaleTargetAngle(float angle)
    {
        ScaleTargetAngle = angle;
    }

    void UpdateScaleAngle()
    {
        float angleDelta = ScaleTargetAngle - CurrentAngle;

        CurrentAngle = Mathf.Clamp(CurrentAngle + Time.deltaTime * angleDelta, -35, 35);

        ScaleTopTargetEuler.z = CurrentAngle;
        ScaleTargetEuler.z = -CurrentAngle;

        ScaleTop.gameObject.transform.localEulerAngles = ScaleTopTargetEuler;
        ScaleLive.gameObject.transform.localEulerAngles = ScaleTargetEuler;
        ScaleDie.gameObject.transform.localEulerAngles = ScaleTargetEuler;
    }
}
