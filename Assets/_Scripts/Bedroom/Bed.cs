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

public class Bed : MonoBehaviour, Interactable
{
    public static Bed instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    SpriteRenderer Renderer;

    public string GetHoverText()
    {
        return "Bed";
    }

    public void Interact()
    {
        if (SaveManager.instance.GetCurrentPlayerState().IsDayDone())
        {
            SaveManager.instance.GetCurrentPlayerState().IncrementDayIndex();
            DesktopManager.instance.StartDay();
            Elevator.instance.GetElevatorButtonBySceneType(EScene.Desktop).ToggleEnable(true);
            Elevator.instance.GetElevatorButtonBySceneType(EScene.Office).ToggleEnable(false);
            //Elevator.instance.GetElevatorButtonBySceneType(EScene.Elevator).ToggleEnable(true);
            Elevator.instance.GetElevatorButtonBySceneType(EScene.Shop).ToggleEnable(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Renderer.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NotifyBedDayDone(bool isDone)
    {
        if (isDone)
        {
            Renderer.color = Color.green;
        }
        else
        {
            Renderer.color = Color.red;
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

    public void Unhover()
    {

    }
}
