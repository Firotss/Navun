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
}