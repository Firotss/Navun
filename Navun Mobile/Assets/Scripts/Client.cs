using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;
using System.Text;

public class Client
{
    public static int dataBufferSize = 4096;
    public int myId = 0;
    public TCP tcp;

    public Client()
    {
        tcp = new TCP();
    }

    public void ConnectToServer(string ip, int port)
    {
        tcp.Connect(ip, port);
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer;

        public void Connect(string ip, int port)
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            try
            {
                socket.Connect(ip, port);

                Debug.Log("Connected");
            }
            catch
            {
                Debug.Log("Connection error");
            }

            stream = socket.GetStream();

            ReceiveDataTCP();
        }

        public void SendDataTCP(Packet packet)
        {
            Task.Run(() =>
            {
                byte[] sendBuffer = packet.GetBuffer();

                try
                {
                    stream.Write(sendBuffer, 0, sendBuffer.Length);
                }
                catch
                {
                    Debug.Log("Disconnect");
                }
            });
        }

        public void ReceiveDataTCP()
        {
            Task.Run(() =>
            {
                receiveBuffer = new byte[dataBufferSize];

                try
                {
                    if (stream.Read(receiveBuffer, 0, dataBufferSize) <= 0)
                    {
                        Debug.Log("Disconnected no data received");
                        UIManager.messageField2.text += "Disconnected no data received\n";
                        return;
                    }

                    HandleData(receiveBuffer);
                }
                catch
                {
                    Debug.Log("Disconnect");
                }

                ReceiveDataTCP();
            });
        }
    }

    private static void HandleData(byte[] buffer)
    {
        Packet packet = new Packet(buffer);
        string json = packet.Deserialize();

        int actionId = (int)(json[0] - '0');
        json = json.Remove(0, 1);

        Debug.Log(json);

        switch (actionId)
        {
            case 0: ThreadManager.RunOnMainThread(() => {ClientHandle.Welcome(json);});
            break;
            case 1: ThreadManager.RunOnMainThread(() => {ClientHandle.Location(json);});
                break;
        }
    }
}
