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
using UnityEngine.SceneManagement;

public class World : MonoBehaviour
{
    public static World instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    GameManager GameManagerTemplate;

    static bool bQuitForReal = false;

    List<GameObject> DDOLObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Application.wantsToQuit += WantsToQuit;

        Instantiate(GameManagerTemplate);

        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene("Intro");
    }

    IEnumerator QuitRoutine()
    {
        Debug.Log("I AM A QUIT ROUTINE");
        yield return new WaitForSeconds(0.4f);

        Application.Quit();
    }

    static bool WantsToQuit()
    {

        Debug.Log("Sweet embrace");
        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddDDOLObject(GameObject obj)
    {
        DDOLObjects.Add(obj);
    }

    public void DestroyAllDDOL()
    {
        for (int i = 0; i < DDOLObjects.Count; ++i)
        {
            Destroy(DDOLObjects[i]);
        }

        Destroy(this);
    }
}
