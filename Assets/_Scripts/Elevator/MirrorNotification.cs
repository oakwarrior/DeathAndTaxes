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

public class MirrorNotification : MonoBehaviour
{
    public static MirrorNotification instance;

    private void Awake()
    {
        instance = this;

    }

    [SerializeField]
    SpriteRenderer NotificationRenderer;

    [SerializeField]
    float MaxFloatSpeed = 50.0f;
    [SerializeField]
    float MinFloatSpeed = 1.0f;

    public float CurrentFloatSpeed = 0.0f;

    public Vector3 OrbitAxis = Vector3.forward;
    public Vector3 DesiredOrbitPosition;
    public float OrbitRadius = 1;
    public float OrbitRadiusSpeed = 0.5f;

    public void InitOrbit()
    {
        CurrentFloatSpeed = Random.Range(MinFloatSpeed, MaxFloatSpeed);
        //transform.localPosition = (transform.position - transform.parent.position).normalized * radius;
        transform.localPosition = new Vector3(OrbitRadius, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.instance == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            ToggleVisible(SaveManager.instance.GetCurrentPlayerState().HasMirrorNotification());
        }
    }

    // Update is called once per frame
    void Update()
    {
        CurrentFloatSpeed = Mathf.Clamp(CurrentFloatSpeed + Random.Range(-200.0f, 200.0f) * Time.deltaTime, MinFloatSpeed, MaxFloatSpeed);

        transform.RotateAround(transform.parent.position, OrbitAxis, CurrentFloatSpeed * Time.deltaTime);
        DesiredOrbitPosition = (transform.position - transform.parent.position).normalized * OrbitRadius + transform.parent.position;
        transform.position = Vector3.MoveTowards(transform.position, DesiredOrbitPosition, Time.deltaTime * OrbitRadiusSpeed);
        transform.localEulerAngles = Vector3.zero;
    }

    public void ToggleVisible(bool val)
    {
        gameObject.SetActive(val);
        if (val)
        {
            InitOrbit();
        }
    }
}
