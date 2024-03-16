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

public class Server : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button connectBtn;
    [SerializeField] private TMP_Text ipField;
    [SerializeField] private TMP_Text ipField2;
    [SerializeField] private TMP_InputField ipInput;

    private List<TcpClient> tcpClients;
    private TcpListener tcpListener;
    private bool isHost;

    private string data = "";

    void Start()
    {
        ipField.text = "";
        hostBtn.onClick.AddListener(Host);
        connectBtn.onClick.AddListener(Connect);
    }

    void Update()
    {
        ipField.text = data;
    }

    public void Connect()
    {
        TcpClient cl = new TcpClient();
        cl.Connect(ipInput.text, 6969);

        hostBtn.gameObject.SetActive(false);
        connectBtn.gameObject.SetActive(false);
        ipInput.gameObject.SetActive(false);
        ipField.gameObject.SetActive(false);
    }

    public void Host()
    {
        hostBtn.gameObject.SetActive(false);
        connectBtn.gameObject.SetActive(false);
        ipInput.gameObject.SetActive(false);

        isHost = true;

        string ip = null;

        IPAddress[] IPS = Dns.GetHostAddresses(Dns.GetHostName());

        foreach (IPAddress i in IPS)
        {
            if (i.AddressFamily == AddressFamily.InterNetwork)
            {

                Debug.Log("IP address: " + i);
                ip = i.ToString();
            }
        }

        ipField2.text = ip;

        tcpListener = new TcpListener(IPAddress.Any, 6969);
        tcpListener.Start();

        new Thread (() => { AcceptClients(); }).Start();
    }

    private void AcceptClients()
    {
        Debug.Log("Accepting");
        TcpClient tcpClient = tcpListener.AcceptTcpClient();
        tcpClients.Add(tcpClient);

        data += $"\nConnected, local:{tcpClient.Client.LocalEndPoint}, remote:{tcpClient.Client.RemoteEndPoint}";
        Debug.Log("\nConnected, local:{tcpClient.Client.LocalEndPoint}, remote:{tcpClient.Client.RemoteEndPoint}");
    }

    void OnDestroy()
    {

    }
}
