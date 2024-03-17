using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.NetworkInformation;
using System.Linq;
using M2MqttUnity;
using System;

public class Server : MonoBehaviour
{
    public static int MaxClients { get; private set; }
    public static Dictionary<int, ServerClient> serverClients = new Dictionary<int, ServerClient>();
    private static TcpListener tcpListener;
    public static string IP { get; private set; }
    public static int Port {get;private set;}

    public static void Start(int maxClients, int port)
    {
        MaxClients = maxClients;
        Port = port;

        InitializeData();

        tcpListener = new TcpListener(IPAddress.Any, 6969);
        tcpListener.Start();

        Task.Run(() => { AcceptClients(); });
    }

    private static void InitializeData()
    {
        IPAddress[] IPS = Dns.GetHostAddresses(Dns.GetHostName());

        foreach (IPAddress i in IPS)
        {
            if (i.AddressFamily == AddressFamily.InterNetwork)
            {
                Debug.Log("IP address: " + i);
                IP = i.ToString();
            }
        }

        for (int i = 1; i <= MaxClients; i++)
        {
            serverClients.Add(i, new ServerClient(i));
        }
    }
    
    private static void AcceptClients()
    {
        TcpClient client = tcpListener.AcceptTcpClient();
        Debug.Log("Connection from " + client.Client.RemoteEndPoint);
        UIManager.messageField1.text += "Connection from " + client.Client.RemoteEndPoint + "\n";

        Task.Run(() => { AcceptClients(); });

        for (int i = 1; i <= MaxClients; i++)
        {
            if (serverClients[i].tcp.socket == null)
            {
                serverClients[i].tcp.Connect(client);
                return;
            }
        }

        Debug.Log("Full");
        UIManager.messageField1.text += "Server full \n";
    }
}
