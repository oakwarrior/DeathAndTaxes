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
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MortimerPostGame : MonoBehaviour
{
    public static MortimerPostGame instance;

    public void Awake()
    {
        instance = this;
    }

    [SerializeField]
    MortimerChoiceButton ButtonRestart;
    [SerializeField]
    MortimerChoiceButton ButtonEnd;

    [SerializeField]
    public SpriteRenderer BackgroundRenderer;

    [SerializeField]
    TextMeshPro TextChoice;

    public bool bChoiceMade = false;

    // Start is called before the first frame update
    void Start()
    {
        if(SaveManager.instance.GetCurrentCarryoverPlayerState().HasGameFinishedOnceProperly())
        {
            SpeechBubbleManager.instance.StartBubbleCascadeFromDialogue(GameManager.instance.PostGameDialogue);
        }
        else
        {
            SpeechBubbleManager.instance.StartBubbleCascadeFromDialogue(GameManager.instance.PostGameDialogueSubplot);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (bChoiceMade || SpeechBubbleManager.instance.IsBubbleSpeechActive())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        if (Input.GetMouseButtonUp(0))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == ButtonRestart.gameObject)
                {
                    ButtonRestart.SetChosen();
                    //GameManager.instance.RestartGame(true);
                    bChoiceMade = true;
                }
                if (hit.collider.gameObject == ButtonEnd.gameObject)
                {
                    ButtonEnd.SetChosen();
                    //GameManager.instance.RestartGame();
                    bChoiceMade = true;
                }
            }
        }


        if (hit.collider != null)
        {
            if (hit.collider.gameObject == ButtonRestart.gameObject)
            {
                TextChoice.text = "A New Chance!";
                ButtonRestart.SetHover(true);
                ButtonEnd.SetHover(false);
            }
            if (hit.collider.gameObject == ButtonEnd.gameObject)
            {
                TextChoice.text = "The End.";
                ButtonEnd.SetHover(true);
                ButtonRestart.SetHover(false);
            }

        }
        else
        {
            TextChoice.text = "";
            ButtonEnd.SetHover(false);
            ButtonRestart.SetHover(false);
        }
    }
}
