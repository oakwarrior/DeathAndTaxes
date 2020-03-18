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
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : ManagerBase
{

    public static SessionManager instance;

    private void Awake()
    {
        instance = this;
    }

    public string IntroScene = "";
    public string DesktopScene = "";
    public float IdleTimerThreshold = 180.0f;

    float IdleTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RestartSession()
    {
        //SceneManager.LoadScene(IntroScene);
    }

    public void RestartDesktop()
    {
        //SceneManager.LoadScene(DesktopScene);
    }

    public void TickIdleTimer(float deltaTime)
    {
        //if (GameManager.instance.GameMode == EGameMode.RELEASE || GameManager.instance.GameMode == EGameMode.DEBUG)
        //{
        //    return;
        //}

        //IdleTimer += deltaTime;
        //if (IdleTimer >= IdleTimerThreshold)
        //{
        //    RestartSession();
        //}
    }

    public void ResetIdleTimer()
    {
        //IdleTimer = 0.0f;
    }

    public string GetIdleTimerString()
    {
        return IdleTimer.ToString("F2") + "/" + IdleTimerThreshold.ToString("F2");
    }
}
