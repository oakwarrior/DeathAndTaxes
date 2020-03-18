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

public class ChibiController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer ChibiRenderer;
    [SerializeField]
    int AllowedStepsLeft;
    [SerializeField]
    int AllowedStepsRight;
    [SerializeField]
    float StepDelay = 0.2f;
    [SerializeField]
    float StepLength = 0.15f;

    int PendingSteps = 0;
    int CurrentStepDelta = 0;
    float CurrentStepDelay = 0.0f;
    bool bIsMoving;
    ELeftOrRight CurrentStepDirection;
    ELeftOrRight CurrentStepRotation;

    float MovementDelayTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        MovementDelayTimer = Random.Range(5.0f, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsMoving)
        {
            CurrentStepDelay -= Time.deltaTime;

            if (CurrentStepDelay <= 0.0f)
            {
                Step();
            }
        }
        else
        {
            if ((AllowedStepsLeft != 0 || AllowedStepsRight != 0))
            {
                if (MovementDelayTimer <= 0.0f)
                {
                    StartMoving();
                }
                else
                {
                    MovementDelayTimer -= Time.deltaTime;
                }
            }
        }
    }

    public void StartMoving()
    {
        CurrentStepDelay = 0.0f;
        bIsMoving = true;

        bool canMoveRight = AllowedStepsRight > 0 && CurrentStepDelta < AllowedStepsRight || AllowedStepsRight == 0 && CurrentStepDelta < 0;
        bool canMoveLeft = AllowedStepsLeft > 0 && CurrentStepDelta > -AllowedStepsLeft || AllowedStepsLeft == 0 && CurrentStepDelta > 0;

        if (canMoveRight && !canMoveLeft)
        {
            CurrentStepDirection = ELeftOrRight.Right;
            ChibiRenderer.flipX = false;
            PendingSteps = Random.Range(1, AllowedStepsLeft + AllowedStepsRight + 1);

        }
        else if (!canMoveRight && canMoveLeft)
        {
            CurrentStepDirection = ELeftOrRight.Left;
            ChibiRenderer.flipX = true;
            PendingSteps = Random.Range(1, AllowedStepsLeft + AllowedStepsRight + 1);
        }
        else if (canMoveLeft && canMoveRight)
        {
            CurrentStepDirection = Random.Range(0, 2) == 0 ? ELeftOrRight.Left : ELeftOrRight.Right;
            switch (CurrentStepDirection)
            {
                case ELeftOrRight.Left:
                    PendingSteps = Random.Range(1, Mathf.Abs(AllowedStepsLeft + CurrentStepDelta) + 1);
                    ChibiRenderer.flipX = true;
                    break;
                case ELeftOrRight.Right:
                    PendingSteps = Random.Range(1, Mathf.Abs(AllowedStepsRight - CurrentStepDelta) + 1);
                    ChibiRenderer.flipX = false;
                    break;
            }
        }
    }

    public void Step()
    {
        switch (CurrentStepRotation)
        {
            case ELeftOrRight.Left:
                gameObject.transform.eulerAngles = new Vector3(0, 0, 15);
                CurrentStepRotation = ELeftOrRight.Right;
                
                break;
            case ELeftOrRight.Right:
                gameObject.transform.eulerAngles = new Vector3(0, 0, -15);
                CurrentStepRotation = ELeftOrRight.Left;
                break;
        }
        PendingSteps--;
        CurrentStepDelay = StepDelay;

        //StartCoroutine(StepRoutine());
        switch (CurrentStepDirection)
        {
            case ELeftOrRight.Left:
            {
                CurrentStepDelta--;
                gameObject.transform.position = gameObject.transform.position + new Vector3(-StepLength, 0.0f);
                break;
            }

            case ELeftOrRight.Right:
            {
                CurrentStepDelta++;
                gameObject.transform.position = gameObject.transform.position + new Vector3(StepLength, 0.0f);
                break;
            }
        }
        if (PendingSteps <= 0)
        {
            bIsMoving = false;
            gameObject.transform.eulerAngles = new Vector3();
            MovementDelayTimer = Random.Range(1.0f, 10.0f);
        }
    }

    public IEnumerator StepRoutine()
    {
        float elapsedTime = 0.0f;

        Vector3 originalPos = gameObject.transform.localPosition;

        while (elapsedTime < StepDelay)
        {
            elapsedTime += Time.deltaTime;
            gameObject.transform.localPosition = Vector3.Lerp(originalPos, CurrentStepDirection == ELeftOrRight.Left ? originalPos + new Vector3(-StepLength, 0) : originalPos + new Vector3(StepLength, 0), elapsedTime / StepDelay);
            yield return null;
        }

        gameObject.transform.localPosition = Vector3.Lerp(originalPos, CurrentStepDirection == ELeftOrRight.Left ? originalPos + new Vector3(-StepLength, 0) : originalPos + new Vector3(StepLength, 0), 1.0f);
    }
}
