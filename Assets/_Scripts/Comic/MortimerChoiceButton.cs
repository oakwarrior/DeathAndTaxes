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
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EMortimerChoice
{
    Restart,
    End
}

public class MortimerChoiceButton : MonoBehaviour
{
    [SerializeField]
    TextMeshPro Text;
    [SerializeField]
    SpriteRenderer CoinRenderer;

    [SerializeField]
    Color HighlightColor = new Color();

    [SerializeField]
    EMortimerChoice Choice;

    [SerializeField]
    Vector3 HoverMax;
    [SerializeField]
    Vector3 HoverMin;

    bool bIsHovering = false;

    float HoverAlpha = 0.5f;

    bool bHoverAlphaUp = false;

    Vector3 OriginPos;

    float SpinSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        OriginPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(SpeechBubbleManager.instance.IsBubbleSpeechActive())
        {
            return;
        }
        CoinRenderer.gameObject.transform.Rotate(Vector3.up, Time.deltaTime * SpinSpeed);

        if (bIsHovering)
        {
            if (bHoverAlphaUp)
            {
                HoverAlpha = Mathf.Clamp(HoverAlpha + Time.deltaTime / 3, 0.0f, 1.0f);
                if(HoverAlpha >= 1.0f)
                {
                    bHoverAlphaUp = !bHoverAlphaUp;
                }
            }
            else
            {
                HoverAlpha = Mathf.Clamp(HoverAlpha - Time.deltaTime / 3, 0.0f, 1.0f);
                if (HoverAlpha <= 0.0f)
                {
                    bHoverAlphaUp = !bHoverAlphaUp;
                }
            }
            CoinRenderer.gameObject.transform.position = Vector3.Lerp(HoverMin, HoverMax, HoverAlpha);
        }
        else
        {
            CoinRenderer.gameObject.transform.position = OriginPos;
        }
        
    }

    public void SetHover(bool val)
    {
        if(bIsHovering == val)
        {
            return;
        }
        bIsHovering = val;

        if(bIsHovering)
        {
            SpinSpeed = 120f;
            HoverAlpha = 0.5f;
            bHoverAlphaUp = false;
        }
        else
        {
            SpinSpeed = 0.0f;
            CoinRenderer.gameObject.transform.eulerAngles = new Vector3();
            CoinRenderer.gameObject.transform.position = OriginPos;
        }
    }

    public void SetChosen()
    {
        SetHover(false);
        StartCoroutine(ChosenRoutine());
    }

    IEnumerator ChosenRoutine()
    {
        AudioManager.instance.PlayOneShotEffect(AudioManager.instance.ClipMortimerCoin);
        float elapsedTime = 0.0f;
        while(elapsedTime < 3.0f)
        {
            SpinSpeed += 50.0f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        MortimerPostGame.instance.bChoiceMade = false;

        switch (Choice)
        {
            case EMortimerChoice.Restart:
                GameManager.instance.RestartGame(true);
                break;
            case EMortimerChoice.End:
                SceneManager.LoadScene("CreditsScene");
                //GameManager.instance.RestartGame();
                break;
        }
    }
}
