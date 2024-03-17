using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Text;

public class ServerHandle
{
    public static void Welcome(string json)
    {
        Packet.ClientWelcome clientWelcome = JsonUtility.FromJson<Packet.ClientWelcome>(json);

        Debug.Log(clientWelcome.message);

        // Create a new pinpoint
    }

    public static void Location(string json)
    {
        Packet.LocationData locationData = JsonUtility.FromJson<Packet.LocationData>(json);

        Debug.Log(locationData.id + ", " + locationData.longitude + ", " + locationData.latitude);

        UIManager.messageField2.text += 
        locationData.id + ", " + locationData.longitude + ", " + locationData.latitude + "\n";
    }
}