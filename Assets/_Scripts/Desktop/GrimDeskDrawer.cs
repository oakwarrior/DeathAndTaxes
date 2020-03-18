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

public class GrimDeskDrawer : MonoBehaviour, Interactable, Draggable
{
    public static GrimDeskDrawer instanceLeft;
    public static GrimDeskDrawer instanceRight;

    public static GrimDeskDrawer GetDrawerByType(ELeftOrRight type)
    {
        switch (type)
        {
            case ELeftOrRight.Left:
                return instanceLeft;
            case ELeftOrRight.Right:
                return instanceRight;
            case ELeftOrRight.MAX:
                return null;
        }
        return null;
    }

    [SerializeField]
    public ELeftOrRight Type;

    [SerializeField]
    public BoxCollider2D Collider;

    [SerializeField]
    public float YOffsetMax = 0.0f;
    [SerializeField]
    public float YOffsetMin = 0.0f;

    [SerializeField]
    public float YOffsetOpenThreshold = 0.0f;

    [SerializeField]
    AudioClip DrawerClose;
    [SerializeField]
    AudioClip DrawerOpen;

    public Vector3 GrabPosition;

    bool bIsDragging = false;

    public bool IsOpen()
    {
        return gameObject.transform.localPosition.y <= YOffsetOpenThreshold;
    }

    public string GetHoverText()
    {
        if(IsOpen())
        {

            int drawerHover = Random.Range(0, 100);
            if(drawerHover < 10)
            {
                return "I bet this bad boy can fit so much spaghetti in it...";
            }
            else
            {
                return "Good for organizing stuff!";
            }
        }
        else
        {
            return "I wonder what's inside";
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

    public void UpdateDragGrabPosition(Vector3 position)
    {
        position.z = gameObject.transform.position.z;
        GrabPosition = gameObject.transform.position - position;
    }

    public void ToggleDragging(bool drag)
    {
        bIsDragging = drag;

    }

    public void Interact()
    {

        if(IsOpen())
        {
            Vector3 temp = gameObject.transform.localPosition;
            temp.y = YOffsetMax;
            gameObject.transform.localPosition = temp;
            AudioManager.instance.PlayOneShotEffect(DrawerClose);
        }
        else
        {
            Vector3 temp = gameObject.transform.localPosition;
            temp.y = YOffsetMin;
            gameObject.transform.localPosition = temp;
            AudioManager.instance.PlayOneShotEffect(DrawerOpen);
        }
        


        AudioManager.instance.UpdateDrawerMuteStatus(Type);
    }

    public void Unhover()
    {

    }

    private void Awake()
    {
        switch (Type)
        {
            case ELeftOrRight.Left:
                instanceLeft = this;
                break;
            case ELeftOrRight.Right:
                instanceRight = this;
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
        if (IsDragging())
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = gameObject.transform.position.z;
            Vector3 newpos = worldPoint + GrabPosition;
            newpos.y = Mathf.Clamp(newpos.y, YOffsetMin + gameObject.transform.parent.transform.localPosition.y, YOffsetMax + gameObject.transform.parent.transform.localPosition.y);
            newpos.x = gameObject.transform.position.x;
            newpos.z = 0;
            gameObject.transform.position = newpos;
            newpos = gameObject.transform.localPosition;
            newpos.z = 2;
            gameObject.transform.localPosition = newpos;

            AudioManager.instance.UpdateDrawerMuteStatus(Type);
        }
    }

    public void HandleDropIntoDrawer(GrimDeskDrawer hitDrawer)
    {

    }

    public void HandleDragOutOfDrawer()
    {

    }
}
