using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Text;

public class ServerSend
{
    public static void SendToAllClients(Packet packet)
    {
        for (int i = 1; i <= Server.MaxClients; i++)
        {
            if (Server.serverClients[i].tcp.socket != null)
            {
                //Server.serverClients[i].tcp.SendDataTCP(packet);
                Debug.Log("Send packet");
            }
        }
    }
}