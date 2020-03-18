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

public class MoneyNotification : MonoBehaviour
{
    public static MoneyNotification instance;

    private void Awake()
    {
        instance = this;
    }


    [SerializeField]
    SpriteRenderer RendererMoney;
    [SerializeField]
    TextMeshPro TextMoney;
    [SerializeField]
    AudioClip ClipCashMoney;

    Vector3 OriginPosition;
    Color OriginColorText;
    Color OriginColorIcon;
    Color FadedColorText;
    Color FadedColorIcon;
    Vector3 Offset = new Vector3(0, 0.6f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        OriginPosition = gameObject.transform.localPosition;
        OriginColorText = Color.white;
        OriginColorIcon = Color.white;
        FadedColorText = OriginColorText;
        FadedColorText.a = 0.0f;
        FadedColorIcon = RendererMoney.color;
        FadedColorIcon.a = 0.0f;

        TextMoney.color = FadedColorText;
        RendererMoney.color = FadedColorIcon;
    }

    bool bPendingNotification = false;
    int PendingAmount = 0;

    // Update is called once per frame
    void Update()
    {
        if (bPendingNotification)
        {
            StartCoroutine(MoneyNotificationRoutine(5.0f, PendingAmount));
            bPendingNotification = false;
        }
    }

    private void OnEnable()
    {
        if(bPendingNotification)
        {
            StartCoroutine(MoneyNotificationRoutine(5.0f, PendingAmount));
            bPendingNotification = false;
        }
    }

    private void OnDisable()
    {
        if (bPendingNotification)
        {
            bPendingNotification = false;
        }

        TextMoney.color = FadedColorText;
        RendererMoney.color = FadedColorIcon;
    }

    public void ShowMoneyNotification(int amount)
    {
        bPendingNotification = true;
        PendingAmount = amount;
    }

    public IEnumerator MoneyNotificationRoutine(float duration, int amount)
    {
        float elapsedTime = 0.0f;
        float alpha = 0.0f;
        Vector3 tempPos = OriginPosition;

        AudioManager.instance.PlayOneShotEffect(ClipCashMoney);

        TextMoney.text = "+" + amount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            alpha = (elapsedTime / duration);

            if (alpha < 0.25f)
            {
                TextMoney.color = Color.Lerp(FadedColorText, OriginColorText, alpha / 0.25f);
                RendererMoney.color = Color.Lerp(FadedColorIcon, OriginColorIcon, alpha / 0.25f);
            }
            if (alpha > 0.75f)
            {
                TextMoney.color = Color.Lerp(FadedColorText, OriginColorText, (1.0f - alpha) / 0.25f);
                RendererMoney.color = Color.Lerp(FadedColorIcon, OriginColorIcon, (1.0f - alpha) / 0.25f);
            }

            tempPos = Vector3.Lerp(OriginPosition, OriginPosition + Offset, alpha);
            gameObject.transform.localPosition = tempPos;

            yield return null;
        }


    }

}
