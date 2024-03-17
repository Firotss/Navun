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

            Task.Run(() => { SendDataTCP(); });
            Task.Run(() => { ReceiveDataTCP(); });
        }

        public void SendDataTCP()
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes($"Id = {id}");

            try
            {
                stream.Write(sendBuffer, 0, sendBuffer.Length);
            }
            catch
            {
                Debug.Log("Disconnect");
                UIManager.messageField2.text += "Disconnect\n";
            }
        }

        public void ReceiveDataTCP()
        {
            receiveBuffer = new byte[dataBufferSize];

            try
            {

                if (stream.Read(receiveBuffer, 0, dataBufferSize) <= 0)
                {
                    Debug.Log("Disconnected no data received");
                    return;
                }

                string message = Encoding.UTF8.GetString(receiveBuffer);
                message = message.Trim('\0');

                Debug.Log(message);

                ThreadManager.RunOnMainThread(() => { UIManager.messageField2.text += $"{message}\n"; });
            }
            catch
            {
                Debug.Log("Disconnect");
                UIManager.messageField2.text += "Disconnect\n";
            }
        }
    }
}