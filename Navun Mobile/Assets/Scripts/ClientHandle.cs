using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Text;

public class ClientHandle
{
    public static void Welcome(string json)
    {
        Packet.ServerWelcome serverWelcome = JsonUtility.FromJson<Packet.ServerWelcome>(json);

        UIManager.client.myId = serverWelcome.id;
        Debug.Log(serverWelcome.message);

        ClientSend.Welcome("Hello there");
    }
}