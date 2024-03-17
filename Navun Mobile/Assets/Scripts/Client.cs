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

            Task.Run(() => {SendDataTCP();}); 
            Task.Run(() => {ReceiveDataTCP();}); 
        }

        public void SendDataTCP(/*Packet packet*/)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes($"Message received from {socket.Client.LocalEndPoint}");

            try
            {
                stream.Write(sendBuffer, 0, sendBuffer.Length);
            }
            catch
            {
                Debug.Log("Disconnect");
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
                    UIManager.messageField2.text += "Disconnected no data received\n";
                    return;
                }

                string message = Encoding.UTF8.GetString(receiveBuffer);
                message = message.Trim('\0');

                Debug.Log(message);
                ThreadManager.RunOnMainThread(()=>{UIManager.messageField2.text += $"{message}\n";});
            }
            catch
            {
                 Debug.Log("Disconnect");
            }
        }
    }
}
