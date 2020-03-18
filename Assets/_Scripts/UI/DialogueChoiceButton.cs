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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueChoiceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Button ButtonReference;
    public TextMeshProUGUI TextChoice;

    public Color ColorDefault;
    public Color ColorHighlight;

    DialogueFragment AssignedChoiceFragment;

    bool bIsHighlighted = false;

    // Start is called before the first frame update
    void Start()
    {
        ButtonReference.onClick.AddListener(OnChoiceClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsHighlighted)
        {
            if (DialogueScreen.instance.IsBusy())
            {
                TextChoice.color = new Color(TextChoice.color.r, TextChoice.color.g, TextChoice.color.b, TextChoice.color.a);
            }
            else
            {
                TextChoice.color = new Color(ColorHighlight.r, ColorHighlight.g, ColorHighlight.b, TextChoice.color.a);
            }
        }
        else
        {
            if (DialogueScreen.instance.IsBusy())
            {
                TextChoice.color = new Color(TextChoice.color.r, TextChoice.color.g, TextChoice.color.b, TextChoice.color.a);
            }
            else
            {
                TextChoice.color = new Color(ColorDefault.r, ColorDefault.g, ColorDefault.b, TextChoice.color.a);
            }
        }
    }

    public void SetChoice(int index, DialogueFragment choiceFragment)
    {
        AssignedChoiceFragment = choiceFragment;

        string text = /*(index + 1) + ". " + */ choiceFragment.Text;
        text = text.Replace("[SPAWN_COUNTER]", SaveManager.instance.GetCurrentPlayerState().GetSpawnCounter().ToString());

        TextChoice.text = text;
    }

    public void OnChoiceClicked()
    {
        if (DialogueScreen.instance.IsBusy())
        {
            return;
        }
        AudioManager.instance.PlayButtonClick();


        DialogueManager.instance.ProceedCurrentDialogue(AssignedChoiceFragment);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bIsHighlighted = true;
        if (DialogueScreen.instance.IsBusy())
        {
            return;
        }
        TextChoice.color = new Color(ColorHighlight.r, ColorHighlight.g, ColorHighlight.b, TextChoice.color.a);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bIsHighlighted = false;
        if (DialogueScreen.instance.IsBusy())
        {
            return;
        }
        TextChoice.color = new Color(ColorDefault.r, ColorDefault.g, ColorDefault.b, TextChoice.color.a);
    }

    public void DeHighlight()
    {
        bIsHighlighted = false;
        TextChoice.color = new Color(ColorDefault.r, ColorDefault.g, ColorDefault.b, TextChoice.color.a);
    }

    public void OnDisable()
    {
        TextChoice.color = ColorDefault;
    }
}
