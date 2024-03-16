using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button connectBtn;
    [SerializeField] private TMP_Text ipField;
    [SerializeField] private TMP_InputField ipInput;
    Client client;

    void Start()
    {
        hostBtn.onClick.AddListener(Host);
        connectBtn.onClick.AddListener(Connect);
    }

    public void Connect()
    {
        client = new Client();
        client.ConnectToServer(ipInput.text, 6969);

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

        Server.Start(3, 6969);

        ipField.text = Server.IP;
    }
}
