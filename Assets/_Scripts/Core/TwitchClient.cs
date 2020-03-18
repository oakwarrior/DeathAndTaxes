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
using TwitchLib.Client.Models;
using TwitchLib.Unity;

public class TwitchClient : MonoBehaviour
{
    public static TwitchClient instance;

    private void Awake()
    {
        instance = this;

    }

    public Client ClientReference;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        World.instance.AddDDOLObject(gameObject);

        Application.runInBackground = true;
    }

    public void ConnectToStream()
    {
        if (ClientReference != null)
        {
            Debug.Log("ConnectToStream - Client reference existing, closing connection and killing reference");
            ClientReference.Disconnect();
            ClientReference = null;
        }
        Debug.Log("Connecting as: " + "BOTNAME" + " to: " + SaveManager.instance.CurrentOptions.StreamChannel);
        ConnectionCredentials creds = new ConnectionCredentials("BOTNAME", "");
        ClientReference = new Client();
        ClientReference.Initialize(creds, SaveManager.instance.CurrentOptions.StreamChannel);

        ClientReference.OnMessageReceived += ClientReference_OnMessageReceived;
        ClientReference.OnConnected += ClientReference_OnConnected;
        ClientReference.OnConnectionError += ClientReference_OnConnectionError;

        ClientReference.Connect();
    }

    private void ClientReference_OnConnectionError(object sender, TwitchLib.Client.Events.OnConnectionErrorArgs e)
    {
        Debug.LogError("ConnectionError: " + e.Error + " with username: " + e.BotUsername);

    }

    private void ClientReference_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
    {
        Debug.Log("Connected: " + e.AutoJoinChannel + " with username: " + e.BotUsername);
    }

    private void ClientReference_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        Debug.Log("Sender: " + sender + "; " + e.ChatMessage.Username + " wrote: " + e.ChatMessage.Message + " is broadcaster: " + e.ChatMessage.IsBroadcaster);

        if (VoteCounter.instance.IsVoteInProgress())
        {
            if (e.ChatMessage.Message.ToLower().Contains(SaveManager.instance.CurrentOptions.StreamCommandDie.ToLower()))
            {
                VoteCounter.instance.RegisterVote(e.ChatMessage.Username, true);
            }
            if (e.ChatMessage.Message.ToLower().Contains(SaveManager.instance.CurrentOptions.StreamCommandLive.ToLower()))
            {
                VoteCounter.instance.RegisterVote(e.ChatMessage.Username, false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ClientReference.SendMessage(ClientReference.JoinedChannels[0], "TEST TEST TEST ONE TWO THREE");
        }
    }
}
