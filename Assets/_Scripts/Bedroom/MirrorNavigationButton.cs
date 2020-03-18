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

public enum ELeftOrRight
{
    Left,
    Right,
    MAX
}

public enum EAccessoryType
{
    Head,
    Body,
}


public class MirrorNavigationButton : MonoBehaviour, Interactable
{
    [SerializeField]
    ELeftOrRight Direction;
    [SerializeField]
    EAccessoryType Type;
    [SerializeField]
    SpriteRenderer Renderer;
    [SerializeField]
    Sprite SpriteHover;

    Sprite SpritePrevious;

    public string GetHoverText()
    {
        return "";

        switch (Direction)
        {
            case ELeftOrRight.Left:
                return "Previous Look";
            case ELeftOrRight.Right:
                return "Next Look";
        }
        return "haha it broke";
    }

    public void Hover()
    {
        if (SpriteHover != null)
        {
            SpritePrevious = Renderer.sprite;
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
        if (SpriteHover != null && SpritePrevious != null)
        {
            Renderer.sprite = SpritePrevious;
        }
    }

    public void Interact()
    {
        AudioManager.instance.PlayButtonClickMenu();

        switch (Direction)
        {
            case ELeftOrRight.Left:
                if (Type == EAccessoryType.Body)
                {
                    Mirror.instance.PreviousBody();

                }
                if (Type == EAccessoryType.Head)
                {
                    Mirror.instance.PreviousHead();
                }
                break;
            case ELeftOrRight.Right:
                if (Type == EAccessoryType.Body)
                {
                    Mirror.instance.NextBody();

                }
                if (Type == EAccessoryType.Head)
                {
                    Mirror.instance.NextHead();
                }
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
