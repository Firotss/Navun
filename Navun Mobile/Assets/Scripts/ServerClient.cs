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
using System.Buffers;
using System;

public class ServerClient
{
    public static int dataBufferSize = 4096;
    public int id;
    public TCP tcp;

    public ServerClient(int clientId)
    {
        id = clientId;
        tcp = new TCP(id);
    }

    public class TCP
    {
        public TcpClient socket;
        private readonly int id;
        private NetworkStream stream;
        private byte[] receiveBuffer;

        public TCP(int id)
        {
            this.id = id;
        }

        public void Connect(TcpClient socket)
        {
            this.socket = socket;
            this.socket.ReceiveBufferSize = dataBufferSize;
            this.socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            ReceiveDataTCP();
            ServerSend.Welcome(id, "Welcome to the server!");
        }

        public void SendDataTCP(Packet packet)
        {
            Task.Run(() => { 
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
            Task.Run(() => { 
            receiveBuffer = new byte[dataBufferSize];

            try
            {
                if (stream.Read(receiveBuffer, 0, dataBufferSize) <= 0)
                {
                    Debug.Log("Disconnected no data received");
                    return;
                }

                Debug.Log("Handle");
                HandleData(receiveBuffer);
            }
            catch
            {
                Debug.Log("Disconnect");
            }

            Debug.Log("Received");
            ReceiveDataTCP();
            });
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
                case 0: ThreadManager.RunOnMainThread(() => {ServerHandle.Welcome(json);});
                break;
            }
        }
    }
}