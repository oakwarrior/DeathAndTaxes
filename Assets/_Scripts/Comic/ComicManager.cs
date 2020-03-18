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
using UnityEngine.SceneManagement;

public enum EPanelDirection
{
    Forward,
    Backward,
    MAX
}

public class ComicManager : ManagerBase
{
    public static ComicManager instance;


    ComicPage CurrentComicPage = null;
    ComicPanel CurrentFirstPanel = null;
    ComicPanel CurrentLastPanel = null;

    int CurrentPanelLine = 0;
    int NumOfPanels = -1;

    [SerializeField]
    float PanelOffset = 0.5f;
    [SerializeField]
    float MaxScrollVelocity = 10.0f;

    bool bIsSwitchingLine = false;

    float ScrollVelocity = 0.0f;

    float PanelLineLerpAlpha = 0.0f;
    Vector3 PanelSwitchOrigin = new Vector3();
    Vector3 PanelSwitchTarget = new Vector3();

    bool bEndOfLineReached = false;
    EPanelDirection PendingPanelDirection = EPanelDirection.MAX;



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //IntroController.instance.Hide();
    }

    public override void InitManager()
    {
        base.InitManager();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.bDebugMode)
        {
            MaxScrollVelocity = 50.0f;
        }

        if (CurrentComicPage == null || CurrentComicPage == ComicPageEnd.instance)
        {
            return;
        }

        //scroll damping
        if (ScrollVelocity > 0.0f)
        {
            ScrollVelocity = Mathf.Clamp(ScrollVelocity - Time.deltaTime * 5.0f, 0.0f, MaxScrollVelocity);
        }

        if (ScrollVelocity < 0.0f)
        {
            ScrollVelocity = Mathf.Clamp(ScrollVelocity + Time.deltaTime * 5.0f, -MaxScrollVelocity, 0.0f);
        }

        if (bIsSwitchingLine)
        {
            ScrollVelocity = 0.0f;
            PanelLineLerpAlpha = Mathf.Clamp(PanelLineLerpAlpha + Time.deltaTime * 2.0f, 0.0f, 1.0f);

            Camera.main.transform.position = Vector3.Lerp(PanelSwitchOrigin, PanelSwitchTarget, PanelLineLerpAlpha);
            if (PanelLineLerpAlpha == 1.0f)
            {
                bIsSwitchingLine = false;
            }
        }
        else if (ScrollVelocity != 0.0f)
        {

            Vector3 newPos = Camera.main.transform.position + ScrollVelocity * Vector3.right * Time.deltaTime;

            newPos = new Vector3(Mathf.Clamp(newPos.x, CurrentFirstPanel.gameObject.transform.position.x - PanelOffset, CurrentLastPanel.gameObject.transform.position.x + PanelOffset), newPos.y, -1);
            Camera.main.transform.position = newPos;

            if (Camera.main.transform.position.x >= CurrentLastPanel.gameObject.transform.position.x + PanelOffset - 0.01f)
            {
                EndOfLineReached(EPanelDirection.Forward);
                //PendingPanelDirection = EPanelDirection.Forward;
                //SetEndOfLineReached(true);
                //ScrollVelocity = 0.0f;
                //IncrementPanelLine();
            }
            if (Camera.main.transform.position.x <= CurrentFirstPanel.gameObject.transform.position.x - PanelOffset + 0.01f)
            {
                EndOfLineReached(EPanelDirection.Backward);
                //PendingPanelDirection = EPanelDirection.Backward;
                //SetEndOfLineReached(true);
                //ScrollVelocity = 0.0f;
                //DecrementPanelLine();
            }

        }

    }

    public void EndOfLineReached(EPanelDirection dir)
    {
        PendingPanelDirection = dir;
        SetEndOfLineReached(true);
        ScrollVelocity = 0.0f;
    }

    public bool IsSwitchingLine()
    {
        return bIsSwitchingLine;
    }

    public void AddCameraVelocity(float velocity)
    {
        if (IsSwitchingLine() || CurrentComicPage == ComicPageEnd.instance)
        {
            return;
        }
        if (bEndOfLineReached && PendingPanelDirection == EPanelDirection.Forward && velocity > 0.0f)
        {
            IncrementPanelLine();
        }
        else if (bEndOfLineReached && PendingPanelDirection == EPanelDirection.Backward && velocity < 0.0f)
        {
            DecrementPanelLine();
        }
        else
        {
            SetEndOfLineReached(false);
            ScrollVelocity = Mathf.Clamp(ScrollVelocity + velocity, -MaxScrollVelocity, MaxScrollVelocity);
        }
    }

    public void SetEndOfLineReached(bool reached)
    {
        bEndOfLineReached = reached;
        if (bEndOfLineReached)
        {
            switch (PendingPanelDirection)
            {
                case EPanelDirection.Forward:
                    ComicCamera.instance.ShowRightMarker();
                    break;
                case EPanelDirection.Backward:
                    if(CurrentPanelLine != 0)
                    {
                        ComicCamera.instance.ShowLeftMarker();
                    }
                    break;
                case EPanelDirection.MAX:
                    break;
            }
            InputManager.instance.bLockUntilInputEnd = true;
        }
        else
        {
            ComicCamera.instance.HideMarkers();
        }
    }

    public void SetCameraPosition(Vector3 newPos)
    {
        newPos.z = -1;
        Camera.main.transform.position = newPos;
    }

    public ComicPage GetCurrentComicPage()
    {
        return CurrentComicPage;
    }

    public void SetCurrentComicPage(ComicPage page)
    {
        bEndOfLineReached = false;
        CurrentComicPage = page;
        SetCurrentPanelLine(0, EPanelDirection.MAX);
        SetCameraPosition(CurrentComicPage.PanelLineList[0].PanelList[0].gameObject.transform.position - new Vector3(PanelOffset * 0.9f, 0.0f));
        NumOfPanels = page.PanelLineList.Count;


        if(ComicAppearance.instance != null)
        {
            ComicAppearance.instance.Refresh();
        }

        if(CurrentComicPage == ComicPageEnd.instance)
        {
            StartCoroutine(EndScrollRoutine(33.0f));
        }

    }

    IEnumerator EndScrollRoutine(float duration)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            SetCameraPosition(Vector3.Lerp(CurrentComicPage.PanelLineList[0].PanelList[0].gameObject.transform.position - new Vector3(PanelOffset * 0.9f, 0.0f), CurrentComicPage.PanelLineList[0].PanelList[CurrentComicPage.PanelLineList[0].PanelList.Count - 1].gameObject.transform.position, elapsedTime / duration));

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        
        EndOfLineReached(EPanelDirection.Forward);
    }

    public void IncrementPanelLine()
    {
        CurrentPanelLine++;
        if (CurrentPanelLine >= NumOfPanels)
        {
            CurrentPanelLine = NumOfPanels - 1;
            EndComic();
        }
        else
        {
            SetCurrentPanelLine(CurrentPanelLine, EPanelDirection.Forward);
            bIsSwitchingLine = true;
            SetEndOfLineReached(false);
        }
    }

    public void DecrementPanelLine()
    {
        CurrentPanelLine--;

        if (CurrentPanelLine < 0)
        {
            SetEndOfLineReached(false);
            CurrentPanelLine = 0;
        }
        else
        {
            bIsSwitchingLine = true;
            SetEndOfLineReached(false);
            SetCurrentPanelLine(CurrentPanelLine, EPanelDirection.Backward);
        }

    }

    public void EndComic()
    {
        InputManager.instance.LastHitInteractable = null;
        if (SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameFinished())
        {
            ElevatorManager.instance.SwitchScene(EScene.PostGame);
        }
        else
        {
            ComicEndConfirm.instance.Show();

        }
    }

    public int GetCurrentPanelLine()
    {
        return CurrentPanelLine;
    }

    public void SetCurrentPanelLine(int lineIndex, EPanelDirection dir)
    {
        PanelLineLerpAlpha = 0.0f;

        CurrentPanelLine = lineIndex;
        CurrentFirstPanel = CurrentComicPage.PanelLineList[lineIndex].PanelList[0];
        CurrentLastPanel = CurrentComicPage.PanelLineList[lineIndex].PanelList[CurrentComicPage.PanelLineList[lineIndex].PanelList.Count - 1];

        if (dir == EPanelDirection.Forward)
        {
            PanelSwitchOrigin = CurrentComicPage.PanelLineList[lineIndex - 1].PanelList[CurrentComicPage.PanelLineList[lineIndex - 1].PanelList.Count - 1].gameObject.transform.position;

            PanelSwitchOrigin.z = -1;
            PanelSwitchTarget = CurrentFirstPanel.gameObject.transform.position - new Vector3(PanelOffset * 0.9f, 0.0f);
            PanelSwitchTarget.z = -1;
        }
        if (dir == EPanelDirection.Backward)
        {
            PanelSwitchOrigin = CurrentComicPage.PanelLineList[lineIndex + 1].PanelList[0].gameObject.transform.position;

            PanelSwitchOrigin.z = -1;
            PanelSwitchTarget = CurrentLastPanel.gameObject.transform.position + new Vector3(PanelOffset * 0.9f, 0.0f);
            PanelSwitchTarget.z = -1;
        }

    }

}
