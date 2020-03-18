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

public enum EEndComicPanelType
{
    GoodPanel,
    BadPanel,
    OverallPanel,
    PersonalPanel,
    MAX
}

public enum EEndComicPersonalPanelType
{
    Fired,
    Pet,
    Takeover,
    Murder,
    MAX
}

public class ComicPanelEndPersonal : ComicPanel
{
    [SerializeField]
    EEndComicPersonalPanelType Type;

    [SerializeField]
    SpriteRenderer BackgroundRenderer;
    [SerializeField]
    SpriteRenderer HeadRenderer;
    [SerializeField]
    SpriteRenderer BodyRenderer;

    [SerializeField]
    List<Sprite> Heads = new List<Sprite>();
    [SerializeField]
    List<Sprite> Bodies = new List<Sprite>();

    [SerializeField]
    Sprite BackgroundBad;

    [SerializeField]
    Sprite BackgroundGood;


    // Start is called before the first frame update
    void Start()
    {
        string headName = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentHeadMagicName();
        string bodyName = SaveManager.instance.GetCurrentCarryoverPlayerState().GetCurrentBodyMagicName();

        // forgive me padre for i have sinned
        if (HeadRenderer != null)
        {
            for (int i = 0; i < Heads.Count; ++i)
            {
                if(Heads[i].name.ToLower().Contains(headName.ToLower()))
                {
                    HeadRenderer.sprite = Heads[i];
                    break;
                }
            }
        }

        // may god have mercy on my soul lol
        if (BodyRenderer != null)
        {
            if(bodyName.ToLower().Contains("suit"))
            {
                BodyRenderer.sprite = Bodies[0];
            }
            else
            {
                BodyRenderer.sprite = Bodies[1];
            }
        }

        if(BackgroundRenderer != null)
        {
            if ((ArticyGlobalVariables.Default.profile.evil_ecology_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_health_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_peace_trigger == true ||
            ArticyGlobalVariables.Default.profile.evil_prosperity_trigger == true))
            {
                BackgroundRenderer.sprite = BackgroundBad;
            }
            else
            {
                BackgroundRenderer.sprite = BackgroundGood;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


}
