using System;
using System.Collections.Generic;
using UnityEngine;


public class ServerSend
{
    private static void SendTCPData(int toClient, Packet packet)
    {
        Server.serverClients[toClient].tcp.SendDataTCP(packet);
    }

    private static void SendToAllClients(Packet packet)
    {
        for (int i = 1; i <= Server.MaxClients; i++)
        {
            if (Server.serverClients[i].tcp.socket != null)
            {
                Server.serverClients[i].tcp.SendDataTCP(packet);
                Debug.Log("Send packet");
            }
        }
    }

    public static void Welcome(int toClient, string msg)
    {
        Packet.ServerWelcome serverWelcome;
        serverWelcome.id = toClient;
        serverWelcome.message = msg;

        string json = JsonUtility.ToJson(serverWelcome);

        Packet packet = new Packet(0, json);

        SendTCPData(toClient, packet);
    }

    // List of locations
    public static void Location(float latitude, float longitude)
    {
        Packet.LocationData ld;
        ld.id = 0;
        ld.latitude = latitude;
        ld.longitude = longitude;

        string json = JsonUtility.ToJson(ld);

        Packet packet = new Packet(1, json);

        SendToAllClients(packet);

        for (int i = 1; i <= Server.MaxClients; i++)
        {
            if (Server.serverClients[i].tcp.socket != null)
            {
                Packet.LocationData locationData;
                locationData.id = Server.serverClients[i].id;
                locationData.latitude = Server.serverClients[i].latitude;
                locationData.longitude = Server.serverClients[i].longitude;

                json = JsonUtility.ToJson(locationData);
                Debug.Log(json);
                packet = new Packet(1, json);

                SendToAllClients(packet);
            }
        }
    }

    public static void Lost(int id)
    {
        Packet.Lost lost;
        lost.id = id;

        string json = JsonUtility.ToJson(lost);

        Packet packet = new Packet(2, json);

        SendToAllClients(packet);
    }

    /*
    0 - Connect
    1 - Locations
    2 - Lost
    */
}