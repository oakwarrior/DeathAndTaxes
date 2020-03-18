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


public class ElevatorButton : MonoBehaviour, Interactable
{
    //public static ElevatorButton instanceBedroom;

    //private void Awake()
    //{
    //    //if(DestinationScene == EScene.DressingRoom)
    //}
    [SerializeField]
    public EScene DestinationScene;
    [SerializeField]
    SpriteRenderer Renderer;
    [SerializeField]
    string TextHover;
    [SerializeField]
    Sprite SpriteHover;
    [SerializeField]
    Sprite SpriteUnhover;

    [SerializeField]
    bool AskForDayEnd = false;

    bool bIsEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        //if (bIsEnabled)
        //{
        //    Renderer.color = Color.green;
        //}
        //else
        //{
        //    Renderer.color = Color.red;
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hover()
    {
        if(SpriteHover != null)
        {
            Renderer.sprite = SpriteHover;
        }
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
        if (SpriteHover != null && SpriteUnhover != null)
        {
            Renderer.sprite = SpriteUnhover;
        }
    }

    public void ToggleEnable(bool isEnabled)
    {
        bIsEnabled = isEnabled;
        Renderer.enabled = bIsEnabled;
    }

    public void Interact()
    {
        if (bIsEnabled)
        {
            if (DestinationScene == EScene.Elevator)
            {
                if (AskForDayEnd)
                {
                    EndDayConfirm.instance.Show();
                }
                else
                {
                    ElevatorManager.instance.SwitchScene(DestinationScene);
                    AudioManager.instance.PlayOneShotEffect(AudioManager.instance.ClipElevatorDing);
                }
            }
            else
            {
                Elevator.instance.MoveToFloor(DestinationScene);
            }
        }
    }

    public string GetHoverText()
    {
        if(DestinationScene == EScene.DressingRoom)
        {
            if(ArticyGlobalVariables.Default.inventory.mirror)
            {
                return TextHover;
            }
            else
            {
                return TextHover + " - I need a mirror for this...";
            }
        }
        return TextHover;
    }
}
