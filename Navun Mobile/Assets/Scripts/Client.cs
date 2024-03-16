using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

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

            receiveBuffer = new byte[dataBufferSize];

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

            // try
            // {
            //     stream.Read(receiveBuffer, 0, dataBufferSize);
                
            //     Debug.Log(System.Text.Encoding.UTF8.GetString(receiveBuffer));
            // }
            // catch
            // {
            //     Debug.Log("Could not establish connection");
            // }
        }
    }
}
