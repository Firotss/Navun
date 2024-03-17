using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Text;

public class ClientSend
{
    private static void SendTCPData(Packet packet)
    {
        UIManager.client.tcp.SendDataTCP(packet);
    }

    public static void Welcome(string msg)
    {
        Packet.ClientWelcome clientWelcome;
        clientWelcome.message = msg;

        string json = JsonUtility.ToJson(clientWelcome);

        Packet packet = new Packet(0, json);

        SendTCPData(packet);
    }

    public static void Location(float latitude, float longitude)
    {
        Packet.LocationData locationData;
        locationData.id = UIManager.client.myId;
        locationData.latitude = latitude;
        locationData.longitude = longitude;

        string json = JsonUtility.ToJson(locationData);

        Packet packet = new Packet(1, json);

        SendTCPData(packet);
    }
}