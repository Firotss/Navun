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

    public static void Location(string json)
    {
        Packet.LocationData locationData = JsonUtility.FromJson<Packet.LocationData>(json);

        Debug.Log(locationData.id);

        UIManager.messageField2.text += 
        locationData.id + ", " + locationData.longitude + ", " + locationData.latitude + "\n";
    }
}