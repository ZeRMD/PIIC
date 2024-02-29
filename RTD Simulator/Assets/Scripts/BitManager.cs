using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;
using System;

public class BitManager : MonoBehaviour
{
    private TcpListener tcpListener;
    private Thread listenerThread;

    private TcpClient tcpClient;
    private PortManager portManager;

    bool isConnected = false;

    Queue<SetParameters> parameters = new Queue<SetParameters>();

    private struct SetParameters
    {
        public int portIndex;
        public bool value;
    }
    private void Start()
    {
        this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 3000);
        this.listenerThread = new Thread(new ThreadStart(ListenForClients));
        this.listenerThread.Start();
        portManager = GetComponent<PortManager>();
    }
    private void ListenForClients()
    {
        this.tcpListener.Start();

        while (true)
        {
            // blocks until a client has connected to the server
            TcpClient client = this.tcpListener.AcceptTcpClient();

            // create a thread to handle communication with the connected client
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
            clientThread.Start(client);
        }
    }

    private void HandleClientComm(object clientObj)
    {
        isConnected = true;
        tcpClient = (TcpClient)clientObj;
        NetworkStream clientStream = tcpClient.GetStream();

        byte[] request = new byte[8];
        clientStream.Read(request);

        byte[] responseBuffer = { };

        Debug.Log((char)request[0]);
        char letterRequest = (char)request[0];
        int portNumber = request[1] - '0';
        int portIndex = request[2] - '0';

        if (letterRequest == 'S')
        {
            int intValue = request[4] - '0';
            bool newValue = (intValue == 1);
            SetBitValue(portIndex, newValue);
        }
        else
        {
            int bitValue = GetBitValue( portIndex);
            responseBuffer = BitConverter.GetBytes(bitValue);
        }

        clientStream.Write(responseBuffer, 0, responseBuffer.Length);

        tcpClient.GetStream().Close();
        tcpClient.Close();
        isConnected = false;
    }

    int GetBitValue( int portIndex)
    {
        Debug.Log(" I " + portIndex);
        bool value = portManager.GetPortValue('I', portIndex);

        Debug.Log("Returned value of " + portIndex + " " + value);

        return value ? 1 : 0;
    }

    void SetBitValue( int portIndex, bool value)
    {
        SetParameters p = new SetParameters();
 
        p.portIndex = portIndex; 
        p.value = value;

        parameters.Enqueue(p);

        Debug.Log("Changed value of Q" + portIndex + " to " + value);
    }

    private void OnApplicationQuit()
    {
        if (isConnected)
        {
            tcpClient.GetStream().Close();
            tcpClient.Close();
        }
        listenerThread.Abort();
    }

    private void Update()
    {
        if (parameters.Count > 0) 
        {
            SetParameters p = parameters.Dequeue();
            portManager.ChangePortValue('Q',p.portIndex, p.value);
        }
    }
}