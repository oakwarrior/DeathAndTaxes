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
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TextSpeechBubble;
    [SerializeField]
    Button ButtonSpeechBubble;


    [SerializeField]
    int SupportedLength = 0;
    [SerializeField]
    int LongestWord = 30;

    [SerializeField]
    float MaxFloatSpeed = 50.0f;
    [SerializeField]
    float MinFloatSpeed = 1.0f;

    [SerializeField]
    bool bMakeCloudPoof = true;

    public float CurrentFloatSpeed = 0.0f;

    DialogueFragment CurrentFragment = null;


    public Vector3 OrbitAxis = Vector3.up;
    public Vector3 DesiredOrbitPosition;
    public float OrbitRadius = 100.0f;
    public float OrbitRadiusSpeed = 0.5f;

    void Start()
    {
        ButtonSpeechBubble.onClick.AddListener(OnBubbleClicked);
    }

    public void InitOrbit()
    {
        MinFloatSpeed = -50;
        MaxFloatSpeed = 50;
        CurrentFloatSpeed = Random.Range(MinFloatSpeed, MaxFloatSpeed);
        //transform.localPosition = (transform.position - transform.parent.position).normalized * radius;
        transform.localPosition = new Vector3(OrbitRadius, 0);
    }

    void Update()
    {
        CurrentFloatSpeed = Mathf.Clamp(CurrentFloatSpeed + Random.Range(-100.0f, 100.0f) * Time.deltaTime, MinFloatSpeed, MaxFloatSpeed);

        transform.RotateAround(transform.parent.position, OrbitAxis, CurrentFloatSpeed * Time.deltaTime);
        DesiredOrbitPosition = (transform.position - transform.parent.position).normalized * OrbitRadius + transform.parent.position;
        transform.position = Vector3.MoveTowards(transform.position, DesiredOrbitPosition, Time.deltaTime * OrbitRadiusSpeed);
        transform.localEulerAngles = Vector3.zero;
    }


    public bool CanFitText(string text)
    {
        string[] stringOfWords = text.Split(' ');
        int longestWordLength = stringOfWords.OrderByDescending(n => n.Length).First().Length;

        return SupportedLength >= text.Length && LongestWord >= longestWordLength;
    }

    public void ToggleBubbleVisibility(bool isEnabled)
    {
        gameObject.SetActive(isEnabled);
    }

    public void ShowBubble(DialogueFragment fragment)
    {
        CurrentFragment = fragment;

        ToggleBubbleVisibility(true);

        TextSpeechBubble.text = CurrentFragment.Text;
        if (CurrentFragment.Speaker == DialogueManager.instance.YouEntity)
        {
            TextSpeechBubble.color = SpeechBubbleManager.instance.ColorYou;
        }
        else if (CurrentFragment.Speaker == DialogueManager.instance.ConscienceEntity)
        {
            TextSpeechBubble.color = SpeechBubbleManager.instance.ColorConscience;
            //GameManager.instance.PaletteBlueRegular;
        }
        else if (CurrentFragment.Speaker == DialogueManager.instance.ShopkeeperEntity)
        {
            TextSpeechBubble.color = SpeechBubbleManager.instance.ColorShopkeeper;
            //GameManager.instance.PaletteBlueRegular;
        }


    }

    public void HideBubble()
    {
        ToggleBubbleVisibility(false);
    }

    public void OnBubbleClicked()
    {
        if (this == SpeechBubbleManager.instance.GetSpeakerBubble())
        {
            SpeechBubbleManager.instance.SkipBubble();
        }
        else
        {
            SpeechBubbleManager.instance.ContinueBubbleCascadeFromElement(DialogueManager.instance.GetNextDialogueElementFromFragment(CurrentFragment));
        }

        if(bMakeCloudPoof)
        {
            CloudParticleEngine particle = Instantiate(SpeechBubbleManager.instance.CloudParticleTemplate);

            particle.InitCloudPoof();

            particle.gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        CurrentFragment = null;
    }
}
