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

public class PawPrints : MonoBehaviour, Interactable
{
    public static PawPrints instance;

    public string GetHoverText()
    {
        int randomHover = Random.Range(0, 4);
        switch (randomHover)
        {
            case 0:
                return "No way... Is that..? Really?";
            case 1:
                return "It must have been that damn cat!";
            case 2:
                return "Cute. Real cute. Thanks, cat.";
            case 3:
                return "Noooo... No way!";
        }
        return "No way...";
    }

    public void Interact()
    {

    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TogglePawPrints(bool visible)
    {
        gameObject.SetActive(visible);
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
