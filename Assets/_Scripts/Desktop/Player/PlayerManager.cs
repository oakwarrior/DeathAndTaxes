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
using Articy.Project_Of_Death.GlobalVariables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStat
{
    LOYALTY,
    DEATH_TOTAL,
    DEATH_DAILY,
    SPARE_TOTAL,
    SPARE_DAILY,
    ECOLOGY,
    HEALTH,
    PROSPERITY,
    PEACE,
    ECOLOGY_DAILY,
    HEALTH_DAILY,
    PROSPERITY_DAILY,
    PEACE_DAILY,
    MAX
}



public class PlayerManager : ManagerBase
{
    public static PlayerManager instance;

    PlayerState Player;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public override void InitManager()
    {
        base.InitManager();

        if (Player == null)
        {
            Player = new PlayerState();
        }
    }



    //public void SetPlayerState(PlayerState state)
    //{
    //    Player = state;
    //}

    //public PlayerState GetPlayerState()
    //{
    //    return Player;
    //}
}
