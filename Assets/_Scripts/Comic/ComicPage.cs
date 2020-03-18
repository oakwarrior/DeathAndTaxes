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

[System.Serializable]
public class PanelLine
{
    [SerializeField]
    public List<ComicPanel> PanelList = new List<ComicPanel>();
}

public class ComicPage : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer PageBorderRenderer;
    [SerializeField]
    public List<PanelLine> PanelLineList = new List<PanelLine>();



    public float PageWidth = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        PageWidth = PageBorderRenderer.sprite.bounds.extents.x * 2;

        ComicManager.instance.SetCurrentComicPage(this);

        for (int i = 0; i < PanelLineList.Count; ++i)
        {
            for (int j = 0; j < PanelLineList[i].PanelList.Count; ++j)
            {
                List<ComicPanelElement> childElements = PanelLineList[i].PanelList[j].GetChildElements();
                for (int k = 0; k < childElements.Count; ++k)
                {
                    childElements[k].SetPanelLine(i);
                    childElements[k].SetPanel(PanelLineList[i].PanelList[j]);
                }
            }
        }

        AudioManager.instance.SwitchMusic(AudioManager.instance.IntroComicThemeDark, AudioManager.instance.IntroComicTheme, true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
