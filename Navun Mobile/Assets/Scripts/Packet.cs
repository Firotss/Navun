using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Text;
using Unity.VisualScripting;

public class Packet
{
    public struct ServerWelcome
    {
        public int id;
        public string message; 
    }

    public struct ClientWelcome
    {
        public string message;
    }

    public struct LocationData
    {
        public int id;
        public float latitude;
        public float longitude;
    }

    private byte[] buffer;

    public Packet(byte[] buffer)
    {
        this.buffer = buffer;
    }

    public Packet(int type, string jsonString)
    {
        jsonString = type + jsonString;
        buffer = Encoding.UTF8.GetBytes(jsonString);
    }

    public byte[] GetBuffer()
    {
        return buffer;
    }

    public string Deserialize()
    {
        return Encoding.UTF8.GetString(buffer).Trim('\0');
    }
}