using Assets.Scripts.TTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCPAdminCommands : MonoBehaviour
{
    TCPServerHelper tcpHelper;

    void Start()
    {
        tcpHelper = new TCPServerHelper();
        tcpHelper.ReceivedTextEvent += ((t) =>
        {
            if (t == "mute")
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => LipSync.Instance.source.Stop());
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
